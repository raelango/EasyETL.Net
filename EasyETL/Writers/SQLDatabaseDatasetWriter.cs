using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    [DisplayName("SQLServer Database Writer")]
    [EasyField("ServerName", "SQL Server Name")]
    [EasyField("DatabaseName", "Database Name")]
    [EasyField("SQLUserName", "SQL User Name (Leave Empty to use Integrated Security)")]
    [EasyField("SQLPassword", "SQL User Password (Leave Empty to use Integrated Security)","","","",true)]
    [EasyField("TableName", "Name of database Table")]
    [EasyField("UniqueKeyColumns","Provide all columns that form a unique key (separate using semicolon)")]
    [EasyField("InsertCommand", "Insert Command")]
    [EasyField("UpdateCommand", "Update Command")]
    public class SQLDatabaseDatasetWriter : DatabaseDatasetWriter
    {
        public string ServerName;
        public string DatabaseName;
        public string SQLUserName;
        public string SQLPassword;
        public SQLDatabaseDatasetWriter() : base() { }

        public override bool IsFieldSettingsComplete()
        {
            _connString  = String.Empty;
            if ((!String.IsNullOrWhiteSpace(ServerName)) && (!String.IsNullOrWhiteSpace(DatabaseName))) {
                _connString = "Server=" + ServerName + ";Database=" + DatabaseName + ";";
                if (String.IsNullOrWhiteSpace(SQLUserName) && String.IsNullOrWhiteSpace(SQLPassword)) {
                    _connString += "Trusted_Connection=True;";
                }
                else {
                    _connString += "User Id=" + SQLUserName + ";Password=" + SQLPassword + ";";
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
