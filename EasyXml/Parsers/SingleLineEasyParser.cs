using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyXml.Parsers
{
    public abstract class SingleLineEasyParser : AbstractEasyParser
    {
        public bool FirstRowHasFieldNames = true;
        public TextFieldParser txtFieldParser;
        public FieldType ParserFieldType = FieldType.Delimited;

        protected XmlDocument GetXmlDocument(XmlNode xmlNode = null, XmlDocument xDoc = null)
        {
            bool bFirstRow = true;
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
            if (xmlNode != null)
            {
                rootNode.AppendChild(xmlNode);
            }
            while (!txtFieldParser.EndOfData)
            {
                bool skipRow = false;
                if (FirstRowHasFieldNames && bFirstRow)
                {
                    SetFieldNames(txtFieldParser.ReadFields());
                    skipRow = true;
                }
                bFirstRow = false;

                if (!skipRow)
                {
                    try
                    {
                        string[] fieldValues = txtFieldParser.ReadFields();
                        NewRow(fieldValues);
                        rootNode.AppendChild(ConvertFieldsToXmlNode(xDoc, fieldValues));
                    }
                    catch (MalformedLineException mex)
                    {
                        Exceptions.Add(mex);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return xDoc;
        }

        public virtual void NewRow(string[] fieldValues)
        {
            RowNodeName = "row";
            FieldPrefix = "Field_";
        }
        
    }
}
