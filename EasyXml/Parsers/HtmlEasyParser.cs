using System.IO;
using System.Xml;
using Sgml;
using System.Xml.Xsl;

namespace EasyXml.Parsers
{
    public class HtmlEasyParser : AbstractEasyParser
    {
        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            SgmlReader sgmlReader = new SgmlReader();
            sgmlReader.DocType = "HTML";
            sgmlReader.WhitespaceHandling = WhitespaceHandling.All;
            sgmlReader.CaseFolding = Sgml.CaseFolding.ToLower;
            sgmlReader.InputStream = txtReader;

            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }
            xDoc.PreserveWhitespace = true;
            xDoc.XmlResolver = null;
            xDoc.Load(sgmlReader);
            return xDoc;        
        }
    }
}
