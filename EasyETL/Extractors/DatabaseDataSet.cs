using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;




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

    }
}
