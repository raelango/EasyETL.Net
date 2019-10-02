using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("Access Database File")]
    [EasyField("FileName", "Access Full File Name (Including Path)")]
    [EasyField("Password", "Database Password (Leave Empty for no password)")]
    [EasyField("Query", "Query to Execute")]
    public class AccessDatabaseEasyParser : DatabaseEasyParser
    {
        public string AccessFileName = String.Empty;
        public string AccessPassword = String.Empty;
        public AccessDatabaseEasyParser() : base(EasyDatabaseConnectionType.edctOledb) { }
        public AccessDatabaseEasyParser(string connString) : base(EasyDatabaseConnectionType.edctOledb, connString) { }

        public override bool IsFieldSettingsComplete()
        {
            ConnectionString = String.Empty;
            if ((!String.IsNullOrWhiteSpace(AccessFileName)) && (File.Exists(AccessFileName)))
            {
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + AccessFileName + ";";
                if (String.IsNullOrWhiteSpace(AccessPassword))
                    ConnectionString += "Persist Security Info=False;";
                else
                    ConnectionString += "Jet OLEDB:Database Password=" + AccessPassword + ";";
            }
            return base.IsFieldSettingsComplete();
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "filename":
                    AccessFileName = fieldValue; break;
                case "password":
                    AccessPassword = fieldValue; break;
            }
        }


        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("filename", AccessFileName);
            resultDict.Add("password", AccessPassword);
            return resultDict;
        }

    }
}
