using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Parsers
{
    public enum EasyDatabaseConnectionType
    {
        edctOledb,
        edctODBC,
        edctSQL,
        edctQuickbooks
    }

    [DisplayName("Database Source")]
    [EasyField("ConnectionType", "Database Connection Type", "ODBC", "", "ODBC;OleDb;SQL")]
    [EasyField("ConnectionString", "Connection String")]
    [EasyField("Query", "Query to Execute")]
    public class DatabaseEasyParser : DatasourceEasyParser
    {
        public string ConnectionString;
        public string Query;
        public EasyDatabaseConnectionType ConnectionType;
        public IDbConnection Connection;
        public bool IsConnected;

        public DatabaseEasyParser()
        {
            ConnectionType = EasyDatabaseConnectionType.edctODBC;
        }
        public DatabaseEasyParser(EasyDatabaseConnectionType connType, string connString = "")
        {
            ConnectionType = connType;
            ConnectionString = connString;
        }

        public override XmlDocument Load(string queryToExecute = "", XmlDocument xDoc = null)
        {
            if (String.IsNullOrWhiteSpace(queryToExecute)) queryToExecute = Query;

            if (String.IsNullOrWhiteSpace(queryToExecute))
            {
                foreach (string tableName in GetTables())
                {
                    queryToExecute += "SELECT * FROM [" + tableName + "];";
                }
            }

            if (!IsFieldSettingsComplete()) return null;
            return LoadFromQuery(queryToExecute, xDoc);
        }

        public XmlDocument LoadFromQuery(string sqlToExecute, XmlDocument xDoc = null)
        {
            DataSet ds = new DataSet();
            IDbDataAdapter adapter = null;
            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }

            int tableIndex = 1;
            foreach (string individualSQL in sqlToExecute.Split(';'))
            {
                try
                {

                    if (!String.IsNullOrWhiteSpace(individualSQL))
                    {
                        switch (ConnectionType)
                        {
                            case EasyDatabaseConnectionType.edctODBC:
                                adapter = new OdbcDataAdapter(individualSQL, new OdbcConnection(ConnectionString));
                                break;
                            case EasyDatabaseConnectionType.edctOledb:
                                adapter = new OleDbDataAdapter(individualSQL, new OleDbConnection(ConnectionString));
                                break;
                            case EasyDatabaseConnectionType.edctSQL:
                                adapter = new SqlDataAdapter(individualSQL, new SqlConnection(ConnectionString));
                                break;
                        }
                        if (adapter != null)
                        {
                            string dataTableName = "Table_" + (char)(tableIndex + 64);
                            //Let us attempt to parse the table name from the SQL 
                            if (individualSQL.IndexOf(" from ", StringComparison.InvariantCultureIgnoreCase) > 0)
                            {
                                dataTableName = individualSQL.Substring(individualSQL.IndexOf(" from ", StringComparison.InvariantCultureIgnoreCase) + 6);
                                dataTableName = dataTableName.TrimStart('[', '\'');
                                dataTableName = dataTableName.TrimEnd(']', '\'', '$');
                                if (dataTableName.Contains(']'))
                                    dataTableName = dataTableName.Substring(dataTableName.IndexOf(']') + 1).Trim();
                                dataTableName = dataTableName.Replace(' ', '_');
                            }
                            DataSet individualDataSet = new DataSet();
                            adapter.Fill(individualDataSet);
                            DataTable individualDataTable = individualDataSet.Tables[0].Copy();
                            individualDataTable.TableName = dataTableName;
                            ds.Tables.Add(individualDataTable);
                            individualDataSet = null;
                        }
                        adapter = null;
                    }
                }
                catch
                {

                }
                tableIndex++;
            }
            xDoc.LoadXml(ds.GetXml());
            return xDoc;
        }

        public override bool IsFieldSettingsComplete()
        {
            return !String.IsNullOrWhiteSpace(ConnectionString);
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            EstablishConnection();
            return IsConnected;
        }

        private void EstablishConnection()
        {
            if (Connection == null)
            {
                switch (ConnectionType)
                {
                    case EasyDatabaseConnectionType.edctODBC:
                        Connection = new OdbcConnection(ConnectionString);
                        break;
                    case EasyDatabaseConnectionType.edctOledb:
                        Connection = new OleDbConnection(ConnectionString);
                        break;
                    case EasyDatabaseConnectionType.edctSQL:
                        Connection = new SqlConnection(ConnectionString);
                        break;
                }
            }
            if ((Connection != null) && (Connection.State != ConnectionState.Open))
            {
                Connection.Open();
            }
            IsConnected = (Connection != null) && (Connection.State == ConnectionState.Open);
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            switch (fieldName.ToLower())
            {
                case "connectionstring":
                    ConnectionString = fieldValue; break;
                case "connectiontype":
                    ConnectionType = (EasyDatabaseConnectionType)Enum.Parse(typeof(EasyDatabaseConnectionType), "edct" + fieldValue); break;
                case "query":
                    Query = fieldValue; break;
            }
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            resultDict.Add("connectionstring", ConnectionString);
            resultDict.Add("connectiontype", ConnectionType.ToString().Substring(4));
            resultDict.Add("query", Query);
            return resultDict;
        }

        public List<string> GetTables()
        {
            EstablishConnection();
            //DbCommand command = ((DBCommand)Connection).CreateCommand();
            DataTable schema = ((DbConnection)Connection).GetSchema("Tables");
            List<string> TableNames = new List<string>();
            foreach (DataRow row in schema.Rows)
            {
                if (row["TABLE_TYPE"].ToString().Equals("TABLE", StringComparison.InvariantCultureIgnoreCase)) TableNames.Add(row["TABLE_NAME"].ToString());
            }
            return TableNames;

        }

    }
}
