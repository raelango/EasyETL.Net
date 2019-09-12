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
        public Dictionary<string,string> lstRegex = new Dictionary<string,string>();

        public List<string> lstTemplates = new List<string>();

        public string[] Templates { get { return lstTemplates.ToArray() ; } set { lstTemplates.Clear(); lstTemplates.AddRange(value.AsEnumerable()); lstRegex.Clear(); } }

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

            if (Templates.Length == 0) return xDoc;
            if (String.IsNullOrWhiteSpace(RowSeparator)) RowSeparator = Environment.NewLine;
            string[] regexPattern = GetRegexPattern();
            Dictionary<Regex,string> dctRegex = new Dictionary<Regex, string>();
            foreach (string strRegexPattern in regexPattern)
            {
                Regex regex = new Regex(strRegexPattern, RegexOptions.Compiled);
                dctRegex.Add(regex, lstRegex[strRegexPattern]);
            }


            int lineNumber = 0;
            string subStr = txtReader.ReadLine();
            int exceptionCount =0;
            int rowCount = 0;
            while (subStr != null)
            {
                lineNumber++;
                UpdateProgress(lineNumber);
                bool matched = false;
                foreach ( KeyValuePair<Regex,string> kvRegex in dctRegex)
                {
                    if (kvRegex.Key.IsMatch(subStr))
                    {
                        matched = true;
                        FieldNames = kvRegex.Key.GetGroupNames().Where(w => w.Trim('0', '1', '2', '3', '4', '5', '6', '7', '8', '9') != "").ToArray();
                        if (FieldNames.Length > 0)
                        {
                            Match match = kvRegex.Key.Match(subStr);
                            string rowName = kvRegex.Value;
                            XmlElement rowNode = xDoc.CreateElement(rowName);
                            foreach (string fieldName in FieldNames)
                            {
                                XmlElement colNode = xDoc.CreateElement(fieldName);
                                colNode.InnerText = match.Groups[fieldName].Value;
                                rowNode.AppendChild(colNode);
                            }
                            AddRow(xDoc, rowNode);
                            if ((rowNode != null) && (rowNode.HasChildNodes))
                            {
                                rootNode.AppendChild(rowNode);
                                rowCount++;
                            }
                        }
                        break;
                    }
                }
                if (!matched)
                {
                    Exceptions.Add(new MalformedLineException(subStr, lineNumber));
                    exceptionCount++;
                    RaiseException(Exceptions.Last());
                    if (exceptionCount > MaximumErrorsToAbort) break;
                }
                if (rowCount >= MaxRecords) break;
                subStr = txtReader.ReadLine();
            }

            UpdateProgress(lineNumber, true);
            return xDoc;
        }

        private string[] GetRegexPattern()
        {            
            if ((lstRegex.Count + lstTemplates.Count) == 0) return new List<string>().ToArray();

            if (lstTemplates.Count > lstRegex.Count)
            {
                lstRegex.Clear();
                foreach (string strTemplate in lstTemplates)
                {
                    #region break down the template string into parts
                    List<string> templateParts = new List<string>();
                    string strTemplateWithoutRowName = strTemplate;
                    string rowName = RowNodeName;
                    if ((strTemplate.StartsWith("[[")) && (strTemplate.Contains("]]")))
                    {
                        rowName = strTemplate.Substring(0, strTemplate.IndexOf("]]"));
                        rowName = rowName.Trim('[');
                        strTemplateWithoutRowName = strTemplate.Substring(strTemplate.IndexOf("]]") + 2);
                    }


                    string strTemp = String.Empty;
                    foreach (string subPart in strTemplateWithoutRowName.Split(']'))
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
                        else
                        {
                            strTemp = subPart;
                        }
                    }
                    if (!String.IsNullOrEmpty(strTemp))
                    {
                        if (strTemp.StartsWith("[")) strTemp += ']';
                        templateParts.Add(strTemp);
                    }
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

                    string strRegex = prefix;
                    if (rcb.Columns.Count >0) strRegex = rcb.CreateRegularExpressionString();
                    lstRegex.Add(strRegex, rowName);
                    #endregion 

                }
            }

            return lstRegex.Keys.ToArray();
        }
  
    }
}
