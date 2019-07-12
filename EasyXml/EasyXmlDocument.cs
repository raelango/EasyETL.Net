using EasyXml.Parsers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyXml
{
    public class EasyXmlDocument : XmlDocument
    {
        IEasyParser Parser = null;

        public void Load(Stream inStream, IEasyParser parser)
        {
            Parser = parser;
            Load(inStream);
        }

        public override void Load(Stream inStream)
        {
            if (Parser == null)
            {
                base.Load(inStream);
            }
            else
            {
                Parser.Load(inStream, this);
            }
        }

        public void Load(string filename, IEasyParser parser)
        {
            Parser = parser;
            Load(filename);
        }

        public override void Load(string filename)
        {
            if (Parser == null)
            {
                base.Load(filename);
            }
            else
            {
                Parser.Load(filename, this);
            }
        }


        public void Load(TextReader txtReader, IEasyParser parser)
        {
            Parser = parser;
            Load(txtReader);
        }

        public override void Load(TextReader txtReader)
        {
            if (Parser == null)
            {
                base.Load(txtReader);
            }
            else
            {
                Parser.Load(txtReader, this);
            }
        }

        public void LoadStr(string contents, IEasyParser parser)
        {
            Parser = parser;
            Parser.LoadStr(contents, this);
        }

        public DataSet ToDataSet(string xPathFilter = "")
        {
            DataSet ds = new DataSet();
            string xmlContents;
            if (String.IsNullOrWhiteSpace(xPathFilter))
                xmlContents = this.OuterXml;
            else
            {
                XmlDocument outXmlDoc = new XmlDocument();
                foreach (XmlNode xNode in this.SelectNodes(xPathFilter))
                {
                    outXmlDoc.ImportNode(xNode.Clone(), true);
                }
                xmlContents = outXmlDoc.OuterXml;
            }
            if (!String.IsNullOrWhiteSpace(xmlContents))
            {
                ds.ReadXml(new StringReader(xmlContents));
            }
            return ds;
        }

        public DataTable ToDataTable(int tableIndex = 0)
        {
            DataSet ds = ToDataSet();
            return (ds != null && ds.Tables.Count > tableIndex) ? ds.Tables[tableIndex] : new DataTable();
        }

        public DataTable ToDataTable(string tableName)
        {
            DataSet ds = ToDataSet();
            return (ds != null && ds.Tables.Contains(tableName)) ? ds.Tables[tableName] : new DataTable();
        }

        public EasyXmlDocument ApplyFilter(string xPathFilter)
        {
            EasyXmlDocument outXmlDoc = new EasyXmlDocument();
            if (String.IsNullOrWhiteSpace(xPathFilter))
                outXmlDoc = this;
            else
            {
                foreach (XmlNode xNode in this.SelectNodes(xPathFilter))
                {
                    outXmlDoc.ImportNode(xNode.Clone(), true);
                }
            }
            return outXmlDoc;
        }

        public string Beautify()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                this.Save(writer);
            }
            return sb.ToString();
        }

        public XmlDocument Transform(string xslFileName)
        {
            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(xslFileName);
            StringBuilder xmlSB = new StringBuilder();
            XmlWriter xWriter = XmlWriter.Create(xmlSB);
            xsl.Transform(this, null, xWriter);
            LoadXml(xmlSB.ToString());
            return this;
        }


        public XmlDocument Transform(string[] settingsCommands)
        {
            if (this.DocumentElement.Name == null) return this;
            if (settingsCommands.Length == 0) return this;
            string rootElementName = this.FirstChild.Name;
            string rowElementName = this.FirstChild.FirstChild.Name;
            bool bTransformRequired = false;
            Dictionary<string, string> dctAdditions = new Dictionary<string, string>();
            List<string> lstRemoveCommands = new List<string>();
            List<string> lstFilters = new List<string>();
            Dictionary<string, string> dctRenames = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            List<string> lstRowElements = new List<string>();

            StringBuilder xslSB = new StringBuilder();
            Dictionary<string, string> dctSortOrders = new Dictionary<string, string>();
            string strSortOrder = String.Empty;
            foreach (string settingCommand in settingsCommands)
            {
                string[] settings = settingCommand.Split(' ');
                switch (settings[0].ToUpper())
                {
                    case "ADD":
                        dctAdditions.Add(settings[1], settingCommand.Substring(4 + settings[1].Length + 1));
                        bTransformRequired = true;
                        break;
                    case "HIDE":
                    case "REMOVE":
                        lstRemoveCommands.Add(settings[1]);
                        bTransformRequired = true;
                        break;
                    case "RENAME":
                        dctRenames.Add(settings[1], settings[2]);
                        bTransformRequired = true;
                        break;
                    case "FILTER":
                        lstFilters.Add(settingCommand.Substring(6).Trim());
                        bTransformRequired = true;
                        break;
                    case "SORT":
                    case "SORTBY":
                    case "ORDER":
                    case "ORDERBY":
                        strSortOrder = settingCommand.Substring(settings[0].Length + 1).Trim();
                        dctSortOrders.Add(rowElementName, strSortOrder);
                        bTransformRequired = true;
                        break;
                    case "GO":
                    case "EXECUTE":
                        if (bTransformRequired || rowElementName.Contains("=>"))
                        {
                            BuildXsltString(rootElementName, rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                            strSortOrder = String.Empty;
                            TransformXml(rootElementName, lstRowElements, xslSB, dctSortOrders);
                            xslSB = new StringBuilder();
                            rootElementName = this.FirstChild.Name;
                            rowElementName = this.FirstChild.FirstChild.Name;
                            bTransformRequired = false;
                        }
                        break;
                }

                if (settingCommand.Trim()[0] == '[')
                {
                    if (bTransformRequired || rowElementName.Contains("=>"))
                    {
                        BuildXsltString(rootElementName, rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                        strSortOrder = String.Empty;
                    }
                    rowElementName = settingCommand.Trim().Substring(1, settingCommand.Trim().Length - 2);
                    bTransformRequired = false;
                }

            }

            if (bTransformRequired || rowElementName.Contains("=>"))
            {
                BuildXsltString(rootElementName, rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                TransformXml(rootElementName, lstRowElements, xslSB, dctSortOrders);
            }

            return this;
        }

        private XmlDocument TransformXml(string rootElementName, List<string> lstRowElements, StringBuilder xslSB, Dictionary<string,string> dctSortOrders)
        {
            StringBuilder rootSB = new StringBuilder();

            rootSB.AppendLine("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            rootSB.AppendLine("<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">");
            rootSB.AppendLine("<xsl:output method=\"xml\" indent=\"yes\" omit-xml-declaration=\"yes\"/>");

            rootSB.AppendLine("<xsl:template match=\"@*|node()\">");
            rootSB.AppendLine("<xsl:copy>");
            rootSB.AppendLine("<xsl:apply-templates select=\"@*|node()\">");
            foreach (KeyValuePair<string, string> kvSortOrder in dctSortOrders)
            {
                string templateName = kvSortOrder.Key;
                string sortOrder = kvSortOrder.Value;
                string sortDetails = String.Empty;
                if (sortOrder.Contains(' ')) {
                    sortDetails = sortOrder.Substring(sortOrder.IndexOf(' ')).Trim();
                    sortOrder = sortOrder.Substring(0, sortOrder.IndexOf(' ')).Trim() ;
                }
                if (templateName.Contains("=>"))
                {
                    templateName = templateName.Substring(templateName.IndexOf("=>") + 2).Trim();
                }
                //rootSB.AppendLine("<xsl:sort select=\"" + templateName + "/" + sortOrder + "\" " + sortDetails + " />");
                rootSB.AppendLine("<xsl:sort select=\"" + sortOrder + "\" " + sortDetails + " />");
            }
            rootSB.AppendLine("</xsl:apply-templates>");
            rootSB.AppendLine("</xsl:copy>");
            rootSB.AppendLine("</xsl:template>");

            rootSB.AppendLine(xslSB.ToString());
            rootSB.AppendLine("</xsl:stylesheet>");

            XslCompiledTransform xsl = new XslCompiledTransform();
            try
            {
                xsl.Load(XmlReader.Create(new StringReader(rootSB.ToString())));
                StringBuilder xmlSB = new StringBuilder();
                XmlWriterSettings xwSettings = new XmlWriterSettings();
                xwSettings.OmitXmlDeclaration = true;
                xwSettings.ConformanceLevel = ConformanceLevel.Auto;
                XmlWriter xWriter = XmlWriter.Create(xmlSB, xwSettings);
                xsl.Transform(this, null, xWriter);
                LoadXml(xmlSB.ToString());
            }
            catch
            {
                return this;
            }

            return this;
        }

        private void BuildXsltString(string rootElementName, string rowElementName, Dictionary<string, string> dctAdditions, List<string> lstRemoveCommands, List<string> lstFilters, Dictionary<string, string> dctRenames, List<string> lstRowElements, StringBuilder xslSB)
        {
            string newRowElementName = rowElementName;
            if (rowElementName.Contains("=>"))
            {
                newRowElementName = rowElementName.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[1];
                rowElementName = rowElementName.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            if ((this.SelectSingleNode(rootElementName) == null) || (this.SelectSingleNode(rootElementName).SelectSingleNode(rowElementName) == null)) return;

            lstRowElements.Add(rowElementName);
            string xslString = String.Empty;

            foreach (XmlNode xNode in this.SelectSingleNode(rootElementName).SelectSingleNode(rowElementName).ChildNodes)
            {
                if (!lstRemoveCommands.Contains(xNode.Name, StringComparer.CurrentCultureIgnoreCase))
                {
                    string columnName = xNode.Name;
                    if (dctRenames.ContainsKey(columnName)) columnName = dctRenames[columnName];
                    xslString += String.Format("<{0}><xsl:value-of select=\"{1}\"/></{0}>", columnName, xNode.Name);
                    xslString += Environment.NewLine;
                }
            }

            foreach (KeyValuePair<string, string> kvPair in dctAdditions)
            {
                switch (kvPair.Value.Trim().ToUpper())
                {
                    case "AUTONUMBER":
                        xslString += String.Format("<{0}><xsl:number {1}/></{0}>", kvPair.Key, kvPair.Value.Substring(10));
                        xslString += Environment.NewLine;
                        break;
                    default:
                        xslString += String.Format("<{0}><xsl:value-of select=\"{1}\"/></{0}>", kvPair.Key, kvPair.Value);
                        xslString += Environment.NewLine;
                        break;
                }
            }



            if (lstFilters.Count == 0)
            {
                xslSB.AppendLine("<xsl:template match=\"" + rowElementName + "\">");
                xslSB.AppendLine("<xsl:element name=\"" + newRowElementName + "\">");
                xslSB.AppendLine(xslString);
                xslSB.AppendLine("</xsl:element>");
                xslSB.AppendLine("</xsl:template>");
            }
            else
            {
                foreach (string strFilter in lstFilters)
                {
                    xslSB.AppendLine("<xsl:template match=\"" + rowElementName + "[" + strFilter + "]\">");
                    xslSB.AppendLine("<xsl:element name=\"" + newRowElementName + "\">");
                    xslSB.AppendLine(xslString);
                    xslSB.AppendLine("</xsl:element>");
                    xslSB.AppendLine("</xsl:template>");
                }
            }
            dctAdditions.Clear();
            lstFilters.Clear();
            dctRenames.Clear();
        }

    }
}
