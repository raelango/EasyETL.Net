using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using word=DocumentFormat.OpenXml.Wordprocessing;
using excel=DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.CustomProperties;

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

        public OfficeDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet, fileName)
        {
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
                        using (WordprocessingDocument doc = WordprocessingDocument.Create(_fileName, WordprocessingDocumentType.Document))
                        {
                            CustomFilePropertiesPart customProp = doc.AddCustomFilePropertiesPart();
                            SetFileProperties(customProp);

                            MainDocumentPart mainDoc = doc.AddMainDocumentPart();
                            mainDoc.Document = new word.Document();
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
                        }
                        break;
                    case OfficeFileType.ExcelWorkbook:
                        using (SpreadsheetDocument workbook = SpreadsheetDocument.Create(_fileName, SpreadsheetDocumentType.Workbook))
                        {

                            CustomFilePropertiesPart customProp = workbook.AddCustomFilePropertiesPart();
                            SetFileProperties(customProp);
                            
                            WorkbookPart workbookPart = workbook.AddWorkbookPart();

                            workbook.WorkbookPart.Workbook = new excel.Workbook();

                            workbook.WorkbookPart.Workbook.Sheets = new excel.Sheets();

                            foreach (System.Data.DataTable table in _dataSet.Tables)
                            {

                                WorksheetPart sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                                excel.SheetData sheetData = new excel.SheetData();
                                sheetPart.Worksheet = new excel.Worksheet(sheetData);

                                excel.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<excel.Sheets>();
                                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                                uint sheetId = 1;
                                if (sheets.Elements<excel.Sheet>().Count() > 0)
                                {
                                    sheetId =
                                        sheets.Elements<excel.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                                }

                                excel.Sheet sheet = new excel.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                                sheets.Append(sheet);

                                excel.Row headerRow = new excel.Row();

                                List<String> columns = new List<string>();
                                foreach (System.Data.DataColumn column in table.Columns)
                                {
                                    columns.Add(column.ColumnName);

                                    excel.Cell cell = new excel.Cell();
                                    cell.DataType = excel.CellValues.String;
                                    cell.CellValue = new excel.CellValue(column.ColumnName);
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

                            }
                        }
                        break;
                }


            }
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
                tableCell.Append(GetParagraph(dataColumn.ColumnName));
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
                excelRow.Append(new excel.Cell() { CellValue = new excel.CellValue(dataColumn.ColumnName) });
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
