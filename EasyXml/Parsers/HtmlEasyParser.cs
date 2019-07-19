using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using System.Xml;
using Sgml;
using System.Xml.Xsl;

namespace EasyXml.Parsers
{
    public class HtmlEasyParser : AbstractEasyParser
    {
        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null, XslCompiledTransform xslt = null)
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
