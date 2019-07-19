using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security;
using System.Xml.Xsl;

namespace EasyXml.Parsers
{
    public abstract class AbstractEasyParser : IEasyParser
    {
        public string[] FieldNames = null;
        public string RootNodeName = "data";
        public string RowNodeName = "row";
        public string FieldPrefix = "Field_";
        public XslCompiledTransform OnLoadTransformer = null;

        public virtual string ConvertFieldsToXml12(string[] fieldValues) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<" + RowNodeName + ">");
            int colIndex = 0;
            foreach (string field in fieldValues)
            {
                string fieldName = (FieldNames != null && FieldNames.Length >= colIndex) ? FieldNames[colIndex] : FieldPrefix + colIndex.ToString();
                sb.AppendLine(String.Format("<{0}>{1}</{0}>", fieldName, SecurityElement.Escape(field)));
                colIndex++;
            }
            sb.AppendLine("</" + RowNodeName + ">");
            return sb.ToString();
        }

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

            if (OnLoadTransformer != null)
            {
                StringBuilder xmlSB = new StringBuilder();
                XmlWriterSettings xwSettings = new XmlWriterSettings();
                xwSettings.OmitXmlDeclaration = true;
                xwSettings.ConformanceLevel = ConformanceLevel.Auto;
                XmlWriter xWriter = XmlWriter.Create(xmlSB, xwSettings);
                OnLoadTransformer.Transform(rowNode,null,xWriter);
                XmlDocument newXDoc = new XmlDocument();
                newXDoc.LoadXml(xmlSB.ToString());
                rowNode = newXDoc.DocumentElement;
            }

            return rowNode;
        }

        public virtual void SetFieldNames(params string[] fieldNames)
        {
            FieldNames = fieldNames;
        }

        public virtual XmlDocument LoadStr(string strToLoad, XmlDocument xDoc = null, XslCompiledTransform xslt = null)
        {
            if (xslt != null)
            {
                OnLoadTransformer = xslt;
            }
            return Load(new StringReader(strToLoad), xDoc);
        }

        public virtual XmlDocument Load(string filename, XmlDocument xDoc = null, XslCompiledTransform xslt = null)
        {
            if (xslt != null)
            {
                OnLoadTransformer = xslt;
            }
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return Load(fs, xDoc);
            }
        }

        public virtual XmlDocument Load(Stream inStream, XmlDocument xDoc = null, XslCompiledTransform xslt = null)
        {
            if (xslt != null)
            {
                OnLoadTransformer = xslt;
            }
            using (StreamReader sr = new StreamReader(inStream))
            {
                return Load(sr, xDoc);
            }
        }

        public virtual XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null, XslCompiledTransform xslt = null)
        {
            throw new NotImplementedException();
        }

    }
}
