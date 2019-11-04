using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("Excel File")]
    [EasyField("FileName", "Excel Full File Name (Including Path)")]
    [EasyField("Provider", "Driver Provider Name", "Microsoft.ACE.OLEDB.12.0")]
    [EasyField("ContainsHeader", "Does the excel file data contain the columns in first row?","True","","True;False")]
    [EasyField("Query", "Query to Execute")]
    public class ExcelDatabaseEasyParser : DatabaseEasyParser
    {
        public string ExcelFileName = String.Empty;
        public string ExcelVersion = "12.0";
        public string ProviderName = "Microsoft.ACE.OLEDB.12.0";
        public bool HasHeader = true;
        public ExcelDatabaseEasyParser() : base(EasyDatabaseConnectionType.edctOledb) { }
        public ExcelDatabaseEasyParser(string connString) : base(EasyDatabaseConnectionType.edctOledb, connString) { }

        public override bool IsFieldSettingsComplete()
        {
            ConnectionString = String.Empty;
            if ((!String.IsNullOrWhiteSpace(ExcelFileName)) && (File.Exists(ExcelFileName)))
            {
                ExcelVersion = (Path.GetExtension(ExcelFileName).Length == 4) ? "8.0" : "12.0";
                ConnectionString = "Provider=" + ProviderName + "; Data Source=" + ExcelFileName + ";Extended Properties=\"Excel " + ExcelVersion + ";HDR=" + (HasHeader ? "YES" : "NO") + "\"";
            }
            return base.IsFieldSettingsComplete();
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "filename":
                    ExcelFileName = fieldValue; break;
                case "provider":
                    ProviderName = fieldValue; break;
                case "containsheader":
                    HasHeader = Convert.ToBoolean(fieldValue); break;
            }
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("filename", ExcelFileName);
            resultDict.Add("provider", ProviderName);
            resultDict.Add("containsheader", HasHeader.ToString());
            return resultDict;
        }

    }
}
