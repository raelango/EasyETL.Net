using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using EasyETL.DataSets;


namespace EasyXml.Parsers
{
    public class TemplateEasyParser : AbstractEasyParser
    {
        public string RowSeparator = String.Empty;
        private string _templateString = "";
        public string RegexString = String.Empty;

        public string TemplateString { get { return _templateString; } set { _templateString = value; RegexString = String.Empty; } }

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

            if (String.IsNullOrWhiteSpace(TemplateString)) return xDoc;
            if (String.IsNullOrWhiteSpace(RowSeparator)) RowSeparator = Environment.NewLine;
            string regexPattern = GetRegexPattern();

            Regex regex = new Regex(regexPattern, RegexOptions.Compiled);
            FieldNames = regex.GetGroupNames().Where(w => w.Trim('0', '1', '2', '3', '4', '5', '6', '7', '8', '9') != "").ToArray();

            int lineNumber = 0;
            string subStr = txtReader.ReadLine();
            while (subStr != null)
            {
                lineNumber++;
                if (regex.IsMatch(subStr))
                {
                    Match match = regex.Match(subStr);
                    XmlElement rowNode = xDoc.CreateElement(RowNodeName);
                    foreach (string fieldName in FieldNames)
                    {
                        XmlElement colNode = xDoc.CreateElement(fieldName);
                        colNode.InnerText = match.Groups[fieldName].Value;
                        rowNode.AppendChild(colNode);
                    }
                    rootNode.AppendChild(rowNode);
                }
                else
                {
                    Exceptions.Add(new MalformedLineException(subStr, lineNumber));
                }
                subStr = txtReader.ReadLine();
            }

            return xDoc;
        }

        private string GetRegexPattern()
        {            
            if (String.IsNullOrWhiteSpace(RegexString + TemplateString)) return String.Empty;

            if (String.IsNullOrWhiteSpace(RegexString))
            {
                #region break down the template string into parts
                List<string> templateParts = new List<string>();
                string strTemp = String.Empty;
                foreach (string subPart in TemplateString.Split(']'))
                {
                    string columnName = String.Empty;
                    if (subPart.Contains('['))
                    {
                        strTemp += subPart.Substring(0, subPart.IndexOf('['));
                        if (!String.IsNullOrEmpty(strTemp))
                        {
                            templateParts.Add(strTemp);
                        }
                        templateParts.Add(subPart.Substring(subPart.IndexOf('[')) + ']');
                        strTemp = String.Empty;
                    }
                }
                if (!String.IsNullOrEmpty(strTemp)) templateParts.Add(strTemp + ']');
                #endregion 

                #region build the regex string from the parts using regex column builder
                RegexColumnBuilder rcb = new RegexColumnBuilder();
                string prefix = string.Empty;
                for (int index = 0; index < templateParts.Count; index++)
                {
                    if ((templateParts[index].StartsWith("[")) && (templateParts[index].EndsWith("]")))
                    {
                        //This is a column...
                        string colName = templateParts[index].Trim('[', ']');
                        string colRegex = ".*";
                        if (colName.Contains(':')) {
                            string colFormat = colName.Substring(colName.IndexOf(':') + 1);
                            if ((colFormat.StartsWith("(")) && (colFormat.EndsWith(")")))
                            {
                                //this is the regex format to be used...
                                colRegex = colFormat.Replace('(', '[').Replace(')', ']') + "+";
                            }
                            else
                            {
                                int colWidth =0;
                                if (Int32.TryParse(colFormat,out colWidth)) {
                                    //This is fixed width
                                    colRegex = ".{" + colWidth.ToString() + "}";
                                }
                            }
                            colName = colName.Substring(0, colName.IndexOf(':'));
                        } 
                        rcb.AddColumn(colName, colRegex, "", prefix);
                        prefix = String.Empty;
                    }
                    else
                    {
                        //this is a constant....
                        prefix += templateParts[index];
                    }
                }
                RegexString = rcb.CreateRegularExpressionString();
                #endregion 
            }

            return RegexString;
        }
  
    }
}
