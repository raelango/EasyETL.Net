using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Xml;




namespace EasyETL.DataSets
{
    public enum DatabaseType
    {
        Odbc,
        OleDb,
        Sql
    }

    public class DatabaseDataSet : EasyDataSet
    {

        private string _connString = String.Empty;
        private string _sqlString = String.Empty;
        private DatabaseType _dbType = DatabaseType.Odbc;

        public DatabaseDataSet(DatabaseType dbType, string connectionString, string sqlString = "")
        {
            _dbType = dbType;
            _connString = connectionString;
            if (!String.IsNullOrWhiteSpace(sqlString))
            {
                _sqlString = sqlString;
                Fill();
            }
        }

        public virtual void Fill(DatabaseType dbType, string connectionString, string sqlString)
        {
            _dbType = dbType;
            _connString = connectionString;
            Fill(sqlString);
        }

        public virtual void Fill(string sqlString)
        {
            _sqlString = sqlString;
            Fill();
        }

        public override void Fill()
        {
            this.Tables.Clear();
            if (String.IsNullOrWhiteSpace(_sqlString))
            {
                throw new ApplicationException("SQL command is missing");
            }

            if (String.IsNullOrWhiteSpace(_connString))
            {
                throw new ApplicationException("Connection string is missing");
            }
            
            switch (_dbType)
            {
                case DatabaseType.Odbc:
                    using (OdbcConnection conn = new OdbcConnection(_connString))
                    {
                        conn.Open();
                        using (OdbcDataAdapter oAdapter = new OdbcDataAdapter(_sqlString, conn))
                        {
                            oAdapter.Fill(this);
                        }
                    };
                    break;
                case DatabaseType.OleDb:
                    using (OleDbConnection conn = new OleDbConnection(_connString))
                    {
                        conn.Open();
                        using (OleDbDataAdapter oAdapter = new OleDbDataAdapter(_sqlString, conn))
                        {
                            oAdapter.Fill(this);
                        }
                    };
                    break;
                case DatabaseType.Sql:
                    using (SqlConnection conn = new SqlConnection(_connString))
                    {
                        conn.Open();
                        using (SqlDataAdapter oAdapter = new SqlDataAdapter(_sqlString, conn))
                        {
                            oAdapter.Fill(this);
                        }
                    };
                    break;
            }

        }

        public override void LoadProfileSettings(XmlNode xNode)
        {
            foreach (XmlAttribute xAttr in xNode.Attributes)
            {
                switch (xAttr.Name.ToUpper()) {
                    case "TYPE":
                        _dbType = (DatabaseType)Enum.Parse(typeof(DatabaseType), xAttr.Value,true);
                        break;
                    case "SQL":
                    case "SQLSTRING":
                        _sqlString = xAttr.Value;
                        break;
                    case "CONNECTIONSTRING":
                    case "CONNSTRING":
                        _connString = xAttr.Value;
                        break;
                }
            }
        }

        public override string GetPropertiesAsXml(string nodeName)
        {
            return "<" + nodeName + " Type='" + _dbType.ToString() + "' ConnectionString='" + _connString + "' SQL='" + _sqlString + "' />";
        }

    }
}
