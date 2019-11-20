using EasyETL.Extractors;
using EasyETL.Xml.Parsers;
using EasyETL.Xml.XsltExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Xml
{
    public class EasyXmlDocument : XmlDocument
    {
        AbstractEasyParser Parser = null;
        public XslCompiledTransform LastTransformer = null;
        public string XsltExtensionNamespace = "easy";
        public string LastTransformerTemplate = String.Empty;
        public event EventHandler<XmlNodeChangedEventArgs> OnRowAdd;
        public event EventHandler<EasyParserExceptionEventArgs> OnError;
        public event EventHandler<EasyParserProgressEventArgs> OnProgress;

        public override XmlNode Clone()
        {
            EasyXmlDocument ezDoc = new EasyXmlDocument();
            XmlNode xNode = ezDoc.ImportNode(this.DocumentElement, true);
            ezDoc.AppendChild(xNode);
            return ezDoc;
        }


        public void Load(Stream inStream, AbstractEasyParser parser, IContentExtractor extractor = null)
        {
            Parser = parser;
            if (extractor != null) inStream = extractor.GetStream(inStream);
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
                Parser.OnRowAdd += OnRowAdd;
                Parser.OnError += OnError;
                Parser.OnProgress += OnProgress;
                Parser.Load(inStream, this);
                Parser.OnRowAdd -= OnRowAdd;
                Parser.OnError -= OnError;
                Parser.OnProgress -= OnProgress;
            }
            Transform();
        }

        public void Load(string filename, AbstractEasyParser parser, IContentExtractor extractor = null)
        {
            Parser = parser;
            if (extractor != null)
            {
                Stream inStream = extractor.GetStream(filename);
                Load(inStream);
            }
            else
            {
                Load(filename);
            }
        }

        public override void Load(string filename)
        {
            if (Parser == null)
            {
                base.Load(filename);
            }
            else
            {
                Parser.OnRowAdd += OnRowAdd;
                Parser.OnError += OnError;
                Parser.OnProgress += OnProgress;
                Parser.Load(filename, this);
                Parser.OnRowAdd -= OnRowAdd;
                Parser.OnError -= OnError;
                Parser.OnProgress -= OnProgress;
            }
            Transform();
        }


        public void Load(TextReader txtReader, AbstractEasyParser parser, IContentExtractor extractor = null)
        {
            Parser = parser;
            if (extractor != null) txtReader = extractor.GetTextReader(txtReader);
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
                Parser.OnRowAdd += OnRowAdd;
                Parser.OnError += OnError;
                Parser.OnProgress += OnProgress;
                Parser.Load(txtReader, this);
                Parser.OnRowAdd -= OnRowAdd;
                Parser.OnError -= OnError;
                Parser.OnProgress -= OnProgress;
            }
            Transform();
        }

        public void LoadStr(string contents, AbstractEasyParser parser, IContentExtractor extractor = null)
        {
            Parser = parser;
            if (extractor != null)
            {
                using (TextReader tr = new StringReader(contents))
                {
                    TextReader extractedContents = extractor.GetTextReader(tr);
                    contents = extractedContents.ReadToEnd();
                }
            }
            if (Parser != null)
            {
                Parser.OnRowAdd += OnRowAdd;
                Parser.OnError += OnError;
                Parser.OnProgress += OnProgress;
                Parser.LoadStr(contents, this);
                Parser.OnRowAdd -= OnRowAdd;
                Parser.OnError -= OnError;
                Parser.OnProgress -= OnProgress;
            }
            else
            {
                LoadXml(contents);
            }
            Transform();
        }

        public DataSet ToDataSet(string xPathFilter = "")
        {
            DataSet ds = new DataSet();
            if (String.IsNullOrWhiteSpace(xPathFilter)) {
                ds.ReadXml(new XmlNodeReader(this));
            }
            else
            {
                XmlDocument outXmlDoc = new XmlDocument();
                foreach (XmlNode xNode in this.SelectNodes(xPathFilter))
                {
                    outXmlDoc.ImportNode(xNode.Clone(), true);
                }
                ds.ReadXml(new XmlNodeReader(outXmlDoc));
            }
            return ds;
        }

        public DataTable ToDataTable(int tableIndex = 0)
        {
            using (DataSet ds = ToDataSet())
            {
                return (ds != null && ds.Tables.Count > tableIndex) ? ds.Tables[tableIndex] : new DataTable();
            }
        }

        public DataTable ToDataTable(string tableName)
        {
            using (DataSet ds = ToDataSet())
            {
                return (ds != null && ds.Tables.Contains(tableName)) ? ds.Tables[tableName] : new DataTable();
            }
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
            using (var ms = new MemoryStream())
            {
                using (var x = new XmlTextWriter(ms, new UTF8Encoding(false)) { Formatting = Formatting.Indented, IndentChar = ' ', Namespaces=false })
                {
                    this.Save(x);
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        ms.Position = 0;
                        string resultText = sr.ReadToEnd();
                        return resultText;
                    }
                }
            }
        }

        public StreamReader BeautifyReaderStream(XmlDocument xDoc = null)
        {
            MemoryStream ms = new MemoryStream();
            using (var x = new XmlTextWriter(ms, Encoding.UTF8) { Formatting = Formatting.Indented, IndentChar = ' ' })
            {
                if (xDoc != null)
                {
                    xDoc.Save(x);
                }
                else
                {
                    this.Save(x);
                }
                return new StreamReader(ms);
            }
        }


        public XmlDocument Transform()
        {
            if (LastTransformer != null)
            {
                StringBuilder xmlSB = new StringBuilder();
                XmlWriter xWriter = XmlWriter.Create(xmlSB);
                LastTransformer.Transform(this, null, xWriter);
                LoadXml(xmlSB.ToString());
            }
            return this;
        }

        public XmlDocument Transform(string xslFileName)
        {
            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(xslFileName);
            LastTransformer = xsl;
            return Transform();
        }


        public XmlNode Transform(string[] settingsCommands, string xsltExtensionNamespace = "easy")
        {
            XsltExtensionNamespace = xsltExtensionNamespace;
            LastTransformerTemplate = String.Empty;
            if (this.DocumentElement.Name == null) return this;
            if (settingsCommands.Length == 0) return this;
            string rootElementName = string.Empty;
            string rowElementName = string.Empty;
            if (this.FirstChild != null) rootElementName = this.FirstChild.Name;
            if (this.FirstChild.FirstChild != null) rowElementName = this.FirstChild.FirstChild.Name;

            if (String.IsNullOrWhiteSpace(rootElementName)) return this;
            if (String.IsNullOrWhiteSpace(rowElementName)) return this;

            bool bTransformRequired = false;
            Dictionary<string, string> dctAdditions = new Dictionary<string, string>();
            List<string> lstRemoveCommands = new List<string>();
            List<string> lstFilters = new List<string>();
            Dictionary<string, string> dctRenames = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            List<string> lstRowElements = new List<string>();

            StringBuilder xslSB = new StringBuilder();
            Dictionary<string, string> dctSortOrders = new Dictionary<string, string>();
            foreach (string settingCommand in settingsCommands)
            {
                string[] settings = settingCommand.Split(' ');
                string strSortOrder;
                switch (settings[0].ToUpper())
                {
                    case "ADD":
                        if (settingCommand.Length > (settings[1].Length + 4))
                        {
                            dctAdditions.Add(settings[1], settingCommand.Substring(4 + settings[1].Length + 1));
                            bTransformRequired = true;
                        }
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
                            TransformXml(xslSB, dctSortOrders);
                            xslSB = new StringBuilder();
                            rootElementName = String.Empty;
                            if (this.FirstChild != null) rootElementName = this.FirstChild.Name;
                            rowElementName = String.Empty;
                            if (this.FirstChild.FirstChild != null) rowElementName = this.FirstChild.FirstChild.Name;
                            if (String.IsNullOrWhiteSpace(rootElementName)) return this;
                            if (String.IsNullOrWhiteSpace(rowElementName)) return this;
                            bTransformRequired = false;
                        }
                        break;
                }
                if (!String.IsNullOrWhiteSpace(settingCommand.Trim()))
                {


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

            }

            if (bTransformRequired || rowElementName.Contains("=>"))
            {
                BuildXsltString(rootElementName, rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                TransformXml(xslSB, dctSortOrders);
            }

            return this;
        }


        private XmlDocument TransformXml(StringBuilder xslSB, Dictionary<string, string> dctSortOrders)
        {
            try
            {
                XsltArgumentList xal = new XsltArgumentList();
                xal.AddExtensionObject(XsltExtensions.XsltStringExtensions.Namespace, new XsltExtensions.XsltStringExtensions()); //This is to make the "easy" extension functions available...
                XslCompiledTransform xsl = GetCompiledTransform(xslSB, dctSortOrders);
                StringBuilder xmlSB = new StringBuilder();
                XmlWriterSettings xwSettings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    ConformanceLevel = ConformanceLevel.Auto
                };
                XmlWriter xWriter = XmlWriter.Create(xmlSB, xwSettings);
                xsl.Transform(this, xal, xWriter);
                LoadXml(xmlSB.ToString());
                dctSortOrders.Clear();
            }
            catch
            {
                return this;
            }

            return this;
        }

        public XslCompiledTransform GetCompiledTransform(StringBuilder xslSB, Dictionary<string, string> dctSortOrders)
        {
            StringBuilder rootSB = new StringBuilder();

            rootSB.AppendLine("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            rootSB.AppendLine("<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:" + XsltExtensionNamespace + "=\"http://EasyXsltExtensions/1.0\">");
            rootSB.AppendLine("<xsl:output method=\"xml\" indent=\"yes\" omit-xml-declaration=\"yes\"/>");
            rootSB.AppendLine("<xsl:template match=\"@*|node()\">");
            rootSB.AppendLine("<xsl:copy>");
            rootSB.AppendLine("<xsl:apply-templates select=\"@*|node()\">");
            foreach (KeyValuePair<string, string> kvSortOrder in dctSortOrders)
            {
                string templateName = kvSortOrder.Key;
                string sortOrder = kvSortOrder.Value;
                string sortDetails = String.Empty;
                if (sortOrder.Contains(' '))
                {
                    sortDetails = sortOrder.Substring(sortOrder.IndexOf(' ')).Trim();
                    sortOrder = sortOrder.Substring(0, sortOrder.IndexOf(' ')).Trim();
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

            rootSB.Append(xslSB.ToString());
            rootSB.AppendLine("</xsl:stylesheet>");

            if (!String.IsNullOrWhiteSpace(LastTransformerTemplate)) LastTransformerTemplate += Environment.NewLine + Environment.NewLine + Environment.NewLine;
            LastTransformerTemplate += rootSB.ToString();


            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(XmlReader.Create(new StringReader(rootSB.ToString())));
            return xsl;
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
                    xslSB.Append(xslString);
                    xslSB.AppendLine("</xsl:element>");
                    xslSB.AppendLine("</xsl:template>");
                }
                xslSB.AppendLine("<xsl:template match=\"" + rowElementName + "\"/>");
            }
            dctAdditions.Clear();
            lstFilters.Clear();
            dctRenames.Clear();
        }

    }
}
