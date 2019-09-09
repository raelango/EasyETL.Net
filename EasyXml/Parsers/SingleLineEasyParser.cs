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
        public string[] CommentTokens;

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
            int errorCount = 0;
            int rowCount = 0;
            while (!txtFieldParser.EndOfData)
            {
                UpdateProgress(txtFieldParser.LineNumber);
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
                        XmlNode childNode = ConvertFieldsToXmlNode(xDoc, fieldValues);
                        if ((childNode != null) && (childNode.HasChildNodes))
                        {
                            childNode = xDoc.ImportNode(childNode, true);
                            rootNode.AppendChild(childNode);
                            rowCount++;
                            if (rowCount >= MaxRecords) break;
                        }
                    }
                    catch (MalformedLineException mex)
                    {
                        Exceptions.Add(mex);
                        errorCount++;
                        RaiseException(mex);
                        if (errorCount > MaximumErrorsToAbort) break;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        RaiseException(new MalformedLineException(ex.Message, ex));
                        if (errorCount > MaximumErrorsToAbort) break;
                    }
                }
            }
            return xDoc;
        }

        public void SetCommentTokens(params string[] commentTokens)
        {
            CommentTokens = commentTokens;
            if (txtFieldParser != null)
            {
                txtFieldParser.CommentTokens = commentTokens;
            }
        }

        public virtual void NewRow(string[] fieldValues)
        {
            RowNodeName = "row";
            FieldPrefix = "Field_";
        }
        
    }
}
