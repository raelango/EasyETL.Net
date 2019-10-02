using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.ComponentModel;

namespace EasyETL.Extractors
{
    [DisplayName("PDF Extractor")]
    public class PDFContentExtractor : AbstractContentExtractor
    {
        public override Stream GetStream(Stream inStream)
        {            
            return new MemoryStream(Encoding.Default.GetBytes(GetPDFContents(new PdfReader(inStream))));
        }

        private string GetPDFContents(PdfReader reader)
        {
            string strText = string.Empty;
            int intNumPages = reader.NumberOfPages;
            for (int page = 1; page <= intNumPages; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                using (PdfReader pReader = new PdfReader(reader))
                {
                    String pageContents = PdfTextExtractor.GetTextFromPage(pReader, page, its);
                    pageContents = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(pageContents)));
                    strText += pageContents;
                }
            }
            return strText;
        }



    }
}
