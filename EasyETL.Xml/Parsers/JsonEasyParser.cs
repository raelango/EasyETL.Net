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
using System.Xml.Xsl;
using System.ComponentModel;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("Json")]
    public class JsonEasyParser : MultipleLineEasyParser
    {
        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            string s = txtReader.ReadToEnd();
            xDoc.LoadXml(((XmlDocument)JsonConvert.DeserializeXmlNode(s,RootNodeName)).OuterXml);
            return xDoc;
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict["parsertype"] = "Json";
            return resultDict;
        }
    }
}
