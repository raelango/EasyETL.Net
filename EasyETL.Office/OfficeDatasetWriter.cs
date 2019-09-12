using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using word = DocumentFormat.OpenXml.Wordprocessing;
using excel = DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.CustomProperties;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EasyETL.Writers
{
    public enum OfficeFileType
    {
        WordDocument,
        ExcelWorkbook
        //PowerpointPresentation
    }
    public class OfficeDatasetWriter : FileDatasetWriter
    {
        public string TemplateFileName = String.Empty;
        public Dictionary<string, string> DocProperties = new Dictionary<string, string>();

        public OfficeFileType DestinationType = OfficeFileType.WordDocument;

        public OfficeDatasetWriter(OfficeFileType fileType = OfficeFileType.WordDocument)
            : base()
        {
            DestinationType = fileType;
        }

        public OfficeDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public OfficeDatasetWriter(DataSet dataSet, string fileName, string templateFileName = "")
            : base(dataSet, fileName)
        {
            TemplateFileName = templateFileName;
        }


        public override void Write()
        {
            if (!String.IsNullOrWhiteSpace(_fileName))
            {
                DocProperties["FileName"] = _fileName;
                DocProperties["TableCount"] = _dataSet.Tables.Count.ToString();
                switch (DestinationType)
                {
                    case OfficeFileType.WordDocument:
                        WordprocessingDocument doc;
                        if (File.Exists(TemplateFileName))
                        {
                            doc = WordprocessingDocument.CreateFromTemplate(TemplateFileName);
                            doc = (WordprocessingDocument)doc.SaveAs(_fileName);
                        }
                        else
                        {
                            doc = WordprocessingDocument.Create(_fileName, WordprocessingDocumentType.Document);
                        }
                        CustomFilePropertiesPart customProp = doc.CustomFilePropertiesPart;
                        if (customProp == null) customProp = doc.AddCustomFilePropertiesPart();
                        SetFileProperties(customProp);

                        MainDocumentPart mainDoc = doc.MainDocumentPart;
                        if (mainDoc == null) mainDoc = doc.AddMainDocumentPart();
                        if (mainDoc.Document == null) mainDoc.Document = new word.Document();
                        word.Body body = new word.Body();
                        bool firstTable = true;
                        foreach (DataTable dt in _dataSet.Tables)
                        {
                            if (!firstTable)
                            {
                                body.Append(GetPageBreak());
                            }
                            else
                            {
                                firstTable = false;
                            }
                            body.Append(GetParagraph(dt.TableName));
                            body.Append(GetWordTable(dt));
                        }
                        mainDoc.Document.Append(body);
                        mainDoc.Document.Save();
                        doc.Dispose();
                        doc = null;
                        break;
                    case OfficeFileType.ExcelWorkbook:
                        SpreadsheetDocument spreadSheet;
                        if (File.Exists(TemplateFileName))
                        {
                            spreadSheet = SpreadsheetDocument.CreateFromTemplate(TemplateFileName);
                            spreadSheet = (SpreadsheetDocument)spreadSheet.SaveAs(_fileName);
                        }
                        else
                        {
                            spreadSheet = SpreadsheetDocument.Create(_fileName, SpreadsheetDocumentType.Workbook);
                            spreadSheet.Save();
                        }
                        using (SpreadsheetDocument workbook = spreadSheet)
                        {

                            CustomFilePropertiesPart excelCustomProp = workbook.AddCustomFilePropertiesPart();
                            SetFileProperties(excelCustomProp);

                            if (workbook.WorkbookPart == null) workbook.AddWorkbookPart();
                            if (workbook.WorkbookPart.Workbook == null) workbook.WorkbookPart.Workbook = new excel.Workbook();
                            if (workbook.WorkbookPart.Workbook.Sheets == null) workbook.WorkbookPart.Workbook.Sheets = new excel.Sheets();
                            excel.Sheets sheets = workbook.WorkbookPart.Workbook.Sheets;
                            foreach (DataTable table in _dataSet.Tables)
                            {

                                excel.SheetData sheetData = null;
                                WorksheetPart sheetPart = null;
                                excel.Sheet sheet = null;
                                foreach (OpenXmlElement element in sheets.Elements())
                                {
                                    if (element is Sheet)
                                    {
                                        sheet = (Sheet)element;
                                        if (sheet.Name.Value.Equals(table.TableName, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            //Assign the sheetPart 
                                            sheetPart = (WorksheetPart)workbook.WorkbookPart.GetPartById(sheet.Id.Value);
                                            sheetData = sheetPart.Worksheet.GetFirstChild<SheetData>();
                                            break;
                                        }
                                    }
                                    sheet = null;
                                }

                                if (sheet == null)
                                {
                                    sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>(); //Create a new WorksheetPart
                                    sheetData = new excel.SheetData(); //create a new SheetData
                                    sheetPart.Worksheet = new excel.Worksheet(sheetData); /// Create a new Worksheet with the sheetData and link it to the sheetPart...

                                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart); //get the ID of the sheetPart. 
                                    sheet = new excel.Sheet() { Id = relationshipId, SheetId = 1, Name = table.TableName }; //create a new sheet
                                    sheets.Append(sheet); //append the sheet to the sheets.
                                }


                                excel.Row headerRow = new excel.Row();

                                List<String> columns = new List<string>();
                                foreach (System.Data.DataColumn column in table.Columns)
                                {
                                    columns.Add(column.ColumnName);

                                    excel.Cell cell = new excel.Cell();
                                    cell.DataType = excel.CellValues.String;
                                    cell.CellValue = new excel.CellValue(GetColumnName(column));
                                    headerRow.AppendChild(cell);
                                }


                                sheetData.AppendChild(headerRow);

                                foreach (System.Data.DataRow dsrow in table.Rows)
                                {
                                    excel.Row newRow = new excel.Row();
                                    foreach (String col in columns)
                                    {
                                        excel.Cell cell = new excel.Cell();
                                        cell.DataType = excel.CellValues.String;
                                        cell.CellValue = new excel.CellValue(dsrow[col].ToString()); //
                                        newRow.AppendChild(cell);
                                    }

                                    sheetData.AppendChild(newRow);
                                }
                                sheetPart.Worksheet.Save();
                            }
                            workbook.WorkbookPart.Workbook.Save();
                            workbook.Save();
                            workbook.Close();
                        }
                        spreadSheet = null;
                        break;
                }


            }
        }


        private static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0) return null;
            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }

        private void SetFileProperties(CustomFilePropertiesPart customProp)
        {
            customProp.Properties = new Properties();
            int kvIndex = 2;
            foreach (KeyValuePair<string, string> kvPair in DocProperties)
            {
                CustomDocumentProperty newProp = new CustomDocumentProperty() { Name = kvPair.Key, FormatId = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}", PropertyId = Int32Value.FromInt32(kvIndex), VTLPWSTR = new DocumentFormat.OpenXml.VariantTypes.VTLPWSTR(kvPair.Value) };
                customProp.Properties.Append(newProp);
                kvIndex++;
            }
        }

        protected word.Paragraph GetParagraph(string text, word.ParagraphStyleId styleID = null)
        {
            word.Paragraph paragraph = new word.Paragraph();
            paragraph.Append(new word.Run(new word.Text(text)));
            if (styleID != null)
            {
                paragraph.ParagraphProperties = new word.ParagraphProperties();
                paragraph.ParagraphProperties.ParagraphStyleId = styleID;
            }
            return paragraph;
        }

        protected word.Table GetWordTable(DataTable dataTable)
        {
            word.Table table = new word.Table();

            #region Set Table Properties
            word.TableProperties tableProperties = new word.TableProperties();
            word.TableWidth tableWidth = new word.TableWidth() { Type = word.TableWidthUnitValues.Pct, Width = "5000" };
            tableProperties.Append(tableWidth);
            UInt32Value borderWidth = UInt32Value.FromUInt32(5);
            word.TableBorders tableBorders = new word.TableBorders(
                        new word.TopBorder
                        {
                            Val = new EnumValue<word.BorderValues>(word.BorderValues.Single),
                            Size = borderWidth
                        },
                        new word.BottomBorder
                        {
                            Val = new EnumValue<word.BorderValues>(word.BorderValues.Single),
                            Size = borderWidth
                        },
                        new word.LeftBorder
                        {
                            Val = new EnumValue<word.BorderValues>(word.BorderValues.Single),
                            Size = borderWidth
                        },
                        new word.RightBorder
                        {
                            Val = new EnumValue<word.BorderValues>(word.BorderValues.Single),
                            Size = borderWidth
                        },
                        new word.InsideHorizontalBorder
                        {
                            Val = new EnumValue<word.BorderValues>(word.BorderValues.Single),
                            Size = borderWidth
                        },
                        new word.InsideVerticalBorder
                        {
                            Val = new EnumValue<word.BorderValues>(word.BorderValues.Single),
                            Size = borderWidth
                        });
            tableProperties.Append(tableBorders);
            table.AppendChild<word.TableProperties>(tableProperties);
            #endregion

            word.TableGrid tableGrid = new word.TableGrid();
            word.TableRow headerRow = new word.TableRow();

            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                word.GridColumn gridColumn = new word.GridColumn();
                word.TableCell tableCell = new word.TableCell();
                tableCell.Append(GetParagraph(GetColumnName(dataColumn)));
                headerRow.Append(tableCell);
                tableGrid.Append(gridColumn);
            }
            table.Append(tableGrid);
            table.Append(headerRow);



            foreach (DataRow row in dataTable.Rows)
            {
                word.TableRow tableRow = new word.TableRow();

                foreach (object cellItem in row.ItemArray)
                {
                    word.TableCell tableCell = new word.TableCell();
                    tableCell.Append(GetParagraph(cellItem.ToString()));
                    tableRow.Append(tableCell);
                }
                table.Append(tableRow);
            }

            return table;
        }

        protected word.Paragraph GetPageBreak()
        {
            return new word.Paragraph(new word.Run(new word.Break() { Type = word.BreakValues.Page }));
        }


        protected void PopulateExcelTable(DataTable dataTable, excel.SheetData sheetData)
        {
            excel.Row excelRow = new excel.Row();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                excelRow.Append(new excel.Cell() { CellValue = new excel.CellValue(GetColumnName(dataColumn)) });
            }
            sheetData.AppendChild(excelRow);

            foreach (DataRow row in dataTable.Rows)
            {
                excelRow = new excel.Row();
                foreach (object cellItem in row.ItemArray)
                {
                    excelRow.Append(new excel.Cell() { CellValue = new excel.CellValue(cellItem.ToString()) });
                }
                sheetData.AppendChild(excelRow);
            }
        }


    }
}
