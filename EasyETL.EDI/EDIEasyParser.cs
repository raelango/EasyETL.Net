using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OopFactory.X12.Parsing;
using OopFactory.X12.Parsing.Model;
using EasyETL.Xml.Parsers;
using System.Xml;
using System.IO;
using System.ComponentModel;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("EDI")]
    public class EDIEasyParser : MultipleLineEasyParser
    {
        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            #region setup the rootNode
            XmlNode rootNode;
            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }
            if (xDoc.DocumentElement == null)
            {
                rootNode = xDoc.CreateElement(RootNodeName);
                xDoc.AppendChild(rootNode);
            }

            rootNode = xDoc.DocumentElement;
            #endregion
            
            X12Parser parser = new X12Parser(false); 
            List<Interchange> interchanges = parser.ParseMultiple(txtReader.ReadToEnd());
            foreach (Interchange interchange in interchanges)
            {
                string xmlContent = interchange.Serialize(true);
                XmlDocument interchangeDocument = new XmlDocument();
                interchangeDocument.LoadXml(xmlContent);
                foreach (XmlNode interchangeNode in interchangeDocument.SelectNodes("Interchange"))
                {
                    XmlNode childNode = xDoc.ImportNode(interchangeNode,true);
                    rootNode.AppendChild(childNode);
                }
            }

            return xDoc;
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict["parsertype"] = "EDI";
            return resultDict;
        }
    }
}
