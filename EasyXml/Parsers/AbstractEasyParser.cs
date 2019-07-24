using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security;
using System.Xml.Xsl;
using Microsoft.VisualBasic.FileIO;

namespace EasyXml.Parsers
{
    public abstract class AbstractEasyParser : IEasyParser
    {
        public string[] FieldNames = null;
        public string RootNodeName = "data";
        public string RowNodeName = "row";
        public string FieldPrefix = "Field_";
        public List<MalformedLineException> Exceptions = new List<MalformedLineException>();

        public virtual XmlNode ConvertFieldsToXmlNode(XmlDocument xDoc, string[] fieldValues)
        {
            XmlElement rowNode = xDoc.CreateElement(RowNodeName);
            int colIndex = 0;
            foreach (string field in fieldValues)
            {
                string fieldName = (FieldNames != null && FieldNames.Length >= colIndex) ? FieldNames[colIndex] : FieldPrefix + colIndex.ToString();
                XmlElement colNode = xDoc.CreateElement(fieldName);
                colNode.InnerText = field;
                rowNode.AppendChild(colNode);
                colIndex++;
            }

            return rowNode;
        }

        public virtual void SetFieldNames(params string[] fieldNames)
        {
            FieldNames = fieldNames;
        }

        public virtual XmlDocument LoadStr(string strToLoad, XmlDocument xDoc = null)
        {
            return Load(new StringReader(strToLoad), xDoc);
        }

        public virtual XmlDocument Load(string filename, XmlDocument xDoc = null)
        {

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return Load(fs, xDoc);
            }
        }

        public virtual XmlDocument Load(Stream inStream, XmlDocument xDoc = null)
        {
            using (StreamReader sr = new StreamReader(inStream))
            {
                return Load(sr, xDoc);
            }
        }

        public virtual XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            throw new NotImplementedException();
        }

    }
}
