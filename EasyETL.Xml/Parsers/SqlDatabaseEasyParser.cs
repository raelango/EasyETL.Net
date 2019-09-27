using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("SQL Server Database")]
    [EasyField("ServerName", "SQL Server Name")]
    [EasyField("DatabaseName", "Database Name")]
    [EasyField("Query", "Query to Execute")]
    [EasyField("SQLUserName", "SQL User Name (Leave Empty to use Integrated Security)")]
    [EasyField("SQLPassword", "SQL User Password (Leave Empty to use Integrated Security)")]
    public class SqlDatabaseEasyParser : DatabaseEasyParser
    {
        public string ServerName;
        public string DatabaseName;
        public string SQLUserName;
        public string SQLPassword;
        public SqlDatabaseEasyParser() : base(EasyDatabaseConnectionType.edctSQL) { }
        public SqlDatabaseEasyParser(string connString) : base(EasyDatabaseConnectionType.edctSQL, connString) { }

        public override bool IsFieldSettingsComplete()
        {
            ConnectionString = String.Empty;
            if ((!String.IsNullOrWhiteSpace(ServerName)) && (!String.IsNullOrWhiteSpace(DatabaseName))) {
                ConnectionString = "Server=" + ServerName + ";Database=" + DatabaseName + ";";
                if (String.IsNullOrWhiteSpace(SQLUserName) && String.IsNullOrWhiteSpace(SQLPassword)) {
                    ConnectionString += "Trusted_Connection=True;";
                }
                else {
                    ConnectionString += "User Id=" + SQLUserName + ";Password=" + SQLPassword + ";";
                }
            }
            return base.IsFieldSettingsComplete();
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "servername":
                    ServerName = fieldValue; break;
                case "databasename":
                    DatabaseName = fieldValue; break;
                case "sqlusername":
                    SQLUserName = fieldValue; break;
                case "sqlpassword":
                    SQLPassword = fieldValue; break;
            }
        }


        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("servername", ServerName);
            resultDict.Add("databasename", DatabaseName);
            resultDict.Add("sqlusername", SQLUserName);
            resultDict.Add("sqlpassword", SQLPassword);
            return resultDict;
        }

    }
}
