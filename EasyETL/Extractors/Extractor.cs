using EasyETL.DataSets;
using EasyETL.Utils;
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
    public class Extractor : IDisposable
    {
        public string FileToParse = String.Empty;
        public XmlNode ProfileNode = null;
        public string ParserType = String.Empty;

        public EasyDataSet Data = null;
        public event EventHandler<RowReadEventArgs> LineReadAndProcessed;

        public Extractor(string fileToParse, XmlNode profileNode = null)
        {
            FileToParse = fileToParse;
            ProfileNode = profileNode;
        }

        public void LoadProfile(string profileName)
        {
            ProfileNode = Configuration.GetProfileNode(profileName) ;
        }

        public static EasyDataSet Parse(string parseFileName) {
            return new RegexDataSet(parseFileName);
        }

        public EasyDataSet Parse()
        {
            Data = new RegexDataSet();
            string parserType = ParserType;
            string connString = String.Empty;
            string sqlString = String.Empty;
            string msmqName = String.Empty;
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
                            connString =  xAttr.Value;
                            connString = connString.Replace("[FileName]", FileToParse);
                            break;
                        case "SQLSTRING":
                        case "SQL":
                            sqlString = xAttr.Value;
                            break;
                        case "MSMQNAME":
                        case "NAME":
                        case "QUEUENAME":
                        case "QUEUE":
                        case "MSMQ":
                            msmqName = xAttr.Value;
                            break;
                    }
                }
            }

            switch (parserType.ToUpper())
            {
                case "JSON":
                    Data = new JsonDataSet();
                    break;
                case "HTML":
                    Data = new HtmlDataSet();
                    break;
                case "EXCEL":
                    Data = new ExcelDataSet();
                    break;
                case "ODBC":
                case "OLEDB":
                case "SQL":
                    Data = new DatabaseDataSet((DatabaseType) Enum.Parse(typeof(DatabaseType),parserType,true), connString, sqlString);
                    break;
                case "MSMQ":
                    Data = new MsMqDataSet(msmqName);
                    break;
                case "XML":
                    Data = new XmlDataSet();
                    break;
                case "DELIMITED":
                    Data = new DelimitedDataSet();
                    break;
                case "HL7":
                    Data = new HL7DataSet();
                    break;
                default:
                    Data = new RegexDataSet();
                    break;
            }
            Data.LoadProfileSettings(ProfileNode);
            Data.RowReadAndProcessed += resultDataSet_RowReadAndProcessed;
            if ((!String.IsNullOrWhiteSpace(FileToParse)) && (File.Exists(FileToParse))  ) {
                if (Data is EasyDataSet)
                {
                    ((EasyDataSet)Data).Fill(FileToParse);
                }
            }
            return Data;
        }

        public void ProcessRowObject(object row)
        {
            Data.ProcessRowObject(row);
        }

        void resultDataSet_RowReadAndProcessed(object sender, RowReadEventArgs e)
        {
            EventHandler<RowReadEventArgs> handler = LineReadAndProcessed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ProfileNode = null;
                Data.Dispose();
                Data = null;
            }
        }

    }


}
