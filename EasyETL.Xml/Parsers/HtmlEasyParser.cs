using System.IO;
using System.Xml;
using Sgml;
using System.Xml.Xsl;
using System.Collections.Generic;
using System.ComponentModel;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("Html")]
    public class HtmlEasyParser : MultipleLineEasyParser
    {
        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            SgmlReader sgmlReader = new SgmlReader
            {
                DocType = "HTML",
                WhitespaceHandling = WhitespaceHandling.All,
                CaseFolding = Sgml.CaseFolding.ToLower,
                InputStream = txtReader
            };

            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }
            xDoc.PreserveWhitespace = true;
            xDoc.XmlResolver = null;
            xDoc.Load(sgmlReader);
            return xDoc;        
        }
    
        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict["parsertype"] = "Html";
            return resultDict;
        }

    }
}
