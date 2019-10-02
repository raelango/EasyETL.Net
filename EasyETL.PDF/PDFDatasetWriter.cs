using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.ComponentModel;
using EasyETL.Attributes;

namespace EasyETL.Writers
{
    [DisplayName("PDF Writer")]
    [EasyField("IncludeHeader", "Include Table Header", "True", "", "True;False")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    public class PDFDatasetWriter : FileDatasetWriter
    {
        public PDFDatasetWriter()
            : base()
        {
        }

        public PDFDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public PDFDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet, fileName)
        {
        }

        public override void Write()
        {
            ExportFileName = PopulatedName(ExportFileName);
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(ExportFileName, FileMode.Create));
            document.Open();

            foreach (DataTable dataTable in _dataSet.Tables)
            {
                if (PrintHeader) document.Add(new Phrase(dataTable.TableName));
                PdfPTable table = new PdfPTable(dataTable.Columns.Count);
                table.WidthPercentage = 100;

                if (PrintTableHeader)
                {

                    //Set columns names in the pdf file
                    for (int k = 0; k < dataTable.Columns.Count; k++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(GetColumnName(dataTable.Columns[k])));

                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(51, 102, 102);

                        table.AddCell(cell);
                    }
                }

                //Add values of DataTable in pdf file
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));

                        //Align the cell in the center
                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                        table.AddCell(cell);
                    }
                }
                document.Add(table);
            }
            document.Close();
        }

    }
}
