using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging; 

namespace EasyETL.Extractors
{
    public class WordContentExtractor: AbstractContentExtractor
    {
        public override Stream GetStream(Stream inStream)
        {
            string content = ReadWordDocument(WordprocessingDocument.Open(inStream, false));
            return new MemoryStream(Encoding.Default.GetBytes(content));
        }

        /// <summary> 
        ///  Read Word Document 
        /// </summary> 
        /// <returns>Plain Text in document </returns> 
        public string ReadWordDocument(WordprocessingDocument package)
        {
            StringBuilder sb = new StringBuilder();
            OpenXmlElement element = package.MainDocumentPart.Document.Body;
            if (element == null)
            {
                return string.Empty;
            }
            sb.Append(GetPlainText(element));
            return sb.ToString();
        }

        /// <summary> 
        ///  Read Plain Text in all XmlElements of word document 
        /// </summary> 
        /// <param name="element">XmlElement in document</param> 
        /// <returns>Plain Text in XmlElement</returns> 
        public string GetPlainText(OpenXmlElement element)
        {
            StringBuilder PlainTextInWord = new StringBuilder();
            foreach (OpenXmlElement section in element.Elements())
            {
                switch (section.LocalName)
                {
                    // Text 
                    case "t":
                        PlainTextInWord.Append(section.InnerText);
                        break;


                    case "cr":                          // Carriage return 
                    case "br":                          // Page break 
                        PlainTextInWord.Append(Environment.NewLine);
                        break;


                    // Tab 
                    case "tab":
                        PlainTextInWord.Append("\t");
                        break;


                    // Paragraph 
                    case "p":
                        PlainTextInWord.Append(GetPlainText(section));
                        PlainTextInWord.AppendLine(Environment.NewLine);
                        break;


                    default:
                        PlainTextInWord.Append(GetPlainText(section));
                        break;
                }
            }


            return PlainTextInWord.ToString();
        } 

    }
}
