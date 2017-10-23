using EasyETL.DataSets;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Parsers
{
    public class Extractor
    {
        public string FileToParse = String.Empty;
        public XmlNode ProfileNode = null;
        public string ParserType = String.Empty;

        public event EventHandler<LinesReadEventArgs> LineReadAndProcessed;

        public Extractor(string fileToParse, XmlNode profileNode = null)
        {
            FileToParse = fileToParse;
            ProfileNode = profileNode;
        }

        public void LoadProfile(string profileName)
        {
            ProfileNode = null;
            if (!String.IsNullOrWhiteSpace(profileName))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("profiles.xml");
                ProfileNode = xDoc.SelectSingleNode("profiles/" + profileName);
            }
        }

        public RegexDataSet Parse()
        {
            RegexDataSet resultDataSet = null;
            string parserType = ParserType;
            string connString = String.Empty;
            string sqlString = String.Empty;
            if (ProfileNode == null)
            {
                if (String.IsNullOrEmpty(parserType))
                {
                    if (File.Exists(FileToParse))
                    {
                        switch (Path.GetExtension(FileToParse).ToUpper())
                        {
                            case ".XLS":
                            case ".XLSX":
                                parserType = "EXCEL";
                                break;
                            case ".JSON":
                                parserType = "JSON";
                                break;
                            case ".HTM":
                            case ".HTML":
                                parserType = "HTML";
                                break;
                            case ".XML":
                                parserType = "XML";
                                break;
                        }
                    }
                }
            }
            else
            {
                foreach (XmlAttribute xAttr in ProfileNode.Attributes)
                {
                    switch (xAttr.Name.ToUpper())
                    {
                        case "PARSER":
                        case "TYPE":
                            parserType = xAttr.Value;
                            break;
                        case "CONNECTIONSTRING":
                        case "CONNSTRING":
                            connString = xAttr.Value;
                            break;
                        case "SQLSTRING":
                        case "SQL":
                            sqlString = xAttr.Value;
                            break;
                    }
                }
            }
            switch (parserType.ToUpper())
            {
                case "JSON":
                    resultDataSet = new JsonDataSet();
                    break;
                case "HTML":
                    resultDataSet = new HtmlDataSet();
                    break;
                case "EXCEL":
                    resultDataSet = new ExcelDataSet();
                    break;
                case "ODBC":
                case "OLEDB":
                case "SQL":
                    resultDataSet = new DatabaseDataSet((DatabaseType) Enum.Parse(typeof(DatabaseType),parserType,true), connString, sqlString);
                    break;
                case "XML":
                    resultDataSet = new XmlDataSet();
                    break;
                default:
                    resultDataSet = new RegexDataSet();
                    break;
            }
            resultDataSet.LoadProfileSettings(ProfileNode);
            resultDataSet.LineReadAndProcessed += resultDataSet_LineReadAndProcessed;
            resultDataSet.Fill(FileToParse);
            return resultDataSet;
        }

        void resultDataSet_LineReadAndProcessed(object sender, LinesReadEventArgs e)
        {
            EventHandler<LinesReadEventArgs> handler = LineReadAndProcessed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnLineReadAndProcessed(LinesReadEventArgs e)
        {
        }

    }


}
