using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EasyETL.DataSets;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace EasyETL.Writers
{
    public class DatabaseDatasetWriter : DatasetWriter 
    {
        IDbConnection _connection = null;
        string _connString = String.Empty;
        string _tableName = String.Empty;
        string _insertCommand = String.Empty;
        string _updateCommand = string.Empty;
        DatabaseType _dbType = DatabaseType.Sql;

        public event EventHandler<RowWrittenEventArgs> RowInserted;
        public event EventHandler<RowWrittenEventArgs> RowUpdated;
        public event EventHandler<RowWrittenEventArgs> RowErrored;


        #region constructors
        public DatabaseDatasetWriter()
            : base()
        {
            _dbType = DatabaseType.Sql;
            _connString = String.Empty;
            _tableName = String.Empty;
            _insertCommand = String.Empty;
            _updateCommand = string.Empty;
        }

        public DatabaseDatasetWriter(DatabaseType dbType)
            : base()
        {
            _dbType = dbType;
            _connString = String.Empty;
            _tableName = String.Empty;
            _insertCommand = String.Empty;
            _updateCommand = string.Empty;
        }

        public DatabaseDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
            _dbType = DatabaseType.Sql;
            _connString = String.Empty;
            _tableName = String.Empty;
            _insertCommand = String.Empty;
            _updateCommand = string.Empty;
        }

        public DatabaseDatasetWriter(DataSet dataSet, string connString, string insertCommand, string updateCommand = "",string tableName = "")
            : base(dataSet)
        {
            _dbType = DatabaseType.Sql;
            _connString = connString;
            _tableName = tableName;
            _insertCommand = insertCommand;
            _updateCommand = updateCommand;
        }

        public DatabaseDatasetWriter(DatabaseType dbType, DataSet dataSet, string connString, string insertCommand, string updateCommand = "", string tableName = "")
            : base(dataSet)
        {
            _dbType = dbType;
            _connString = connString;
            _tableName = tableName;
            _insertCommand = insertCommand;
            _updateCommand = updateCommand;
        }
        #endregion 

        #region public methods
        public override void Write()
        {
            if (_dataSet == null)
            {
                throw new EasyLoaderException("Dataset is null"); 
            }
            
            if (_dataSet.Tables.Count == 0)
            {
                throw new EasyLoaderException("Dataset has no tables");
            }

            if (String.IsNullOrWhiteSpace(_tableName))
            {
                _tableName = _dataSet.Tables[0].TableName;
            }

            if (!_dataSet.Tables.Contains(_tableName))
            {
                throw new EasyLoaderException("Dataset does not contain table <<" + _tableName + ">>");
            }

            if (_dataSet.Tables[_tableName].Rows.Count == 0)
            {
                throw new EasyLoaderException("The table <<" + _tableName + ">> in the dataset is empty");
            }

            EstablishDatabaseConnection();

            int rowNumber = 0;
            foreach (DataRow dataRow in _dataSet.Tables[_tableName].Rows)
            {
                try
                {
                    WriteRowToDatabase(dataRow, rowNumber);
                }
                catch (Exception e)
                {
                    dataRow.RowError = e.Message;
                    OnRowErrored(new RowWrittenEventArgs() { Row = dataRow, RowNumber = rowNumber, TableName = _tableName });
                }
                rowNumber++;
            }

            CloseDatabaseConnection();
        }

        public void EstablishDatabaseConnection()
        {
            switch (_dbType)
            {
                case DatabaseType.Odbc:
                    _connection = new OdbcConnection(_connString);
                    break;
                case DatabaseType.OleDb:
                    _connection = new OleDbConnection(_connString);
                    break;
                case DatabaseType.Sql:
                    _connection = new SqlConnection(_connString);
                    break;
            }
            _connection.Open();
        }

        public void CloseDatabaseConnection()
        {
            _connection.Close();
            _connection = null;
        }

        public virtual void WriteRowToDatabase(DataRow row, int rowNumber)
        {

            using (IDbCommand cmd = _connection.CreateCommand())
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                cmd.CommandType = CommandType.Text;

                List<IDbDataParameter> parameters = new List<IDbDataParameter>();
                foreach (DataColumn dColumn in row.Table.Columns)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = "@" + dColumn.ColumnName;
                    parameter.Value = String.IsNullOrWhiteSpace(row[dColumn].ToString()) ? DBNull.Value : row[dColumn];
                    parameters.Add(parameter);
                }

                bool bInsertRecord = true;
                cmd.Parameters.Clear();
                if (!String.IsNullOrWhiteSpace(_updateCommand)) {
                    cmd.CommandText = GetUpdateCommand(row);
                    if (cmd.CommandText.Contains('@'))
                    {
                        foreach (IDbDataParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    bInsertRecord = cmd.ExecuteNonQuery() == 0;
                    if (!bInsertRecord)
                    {
                        OnRowUpdated(new RowWrittenEventArgs() { Row = row, RowNumber = rowNumber, TableName = _tableName });
                    }
                    cmd.Parameters.Clear();
                }

                if (bInsertRecord) {
                    cmd.CommandText = GetInsertCommand(row);
                    if (cmd.CommandText.Contains('@'))
                    {
                        foreach (IDbDataParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    cmd.ExecuteNonQuery();
                    OnRowInserted(new RowWrittenEventArgs() { Row = row, RowNumber = rowNumber, TableName = _tableName });
                }

            }
        }

        public virtual void Write(string connString, string insertCommand, string updateCommand = "", string tableName = "")
        {
            _connString = connString;
            _insertCommand = insertCommand;
            _updateCommand = updateCommand;
            Write();
        }

        public virtual void Write(DatabaseType dbType, string connString, string insertCommand, string updateCommand = "", string tableName = "")
        {
            _dbType = dbType;
            _connString = connString;
            _insertCommand = insertCommand;
            _updateCommand = updateCommand;
            Write();
        }


        public virtual void Write(DataSet dataSet, string connString, string insertCommand, string updateCommand = "", string tableName = "")
        {
            _dataSet = dataSet;
            Write(connString,insertCommand,updateCommand);
        }

        public virtual void Write(DatabaseType dbType, DataSet dataSet, string connString, string insertCommand, string updateCommand = "", string tableName = "")
        {
            _dbType = dbType;
            Write(dataSet, connString, insertCommand, updateCommand);
        }
        #endregion

        #region protected methods
        protected virtual void OnRowInserted(RowWrittenEventArgs e)
        {
            EventHandler<RowWrittenEventArgs> handler = RowInserted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRowUpdated(RowWrittenEventArgs e)
        {
            EventHandler<RowWrittenEventArgs> handler = RowUpdated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRowErrored(RowWrittenEventArgs e)
        {
            EventHandler<RowWrittenEventArgs> handler = RowErrored;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        
        #endregion 
        #region private methods
        private string GetUpdateCommand(DataRow row)
        {
            return GetCommand(_updateCommand, row);
        }

        private string GetInsertCommand(DataRow row)
        {
            return GetCommand(_insertCommand, row);
        }

        private string GetCommand(string strCommand, DataRow row)
        {
            string resultStr = strCommand;
            foreach (DataColumn dColumn in row.Table.Columns)
            {
                resultStr = resultStr.Replace("{" + dColumn.ColumnName + "}", row[dColumn].ToString());
                resultStr = resultStr.Replace("{" + dColumn.Ordinal.ToString() + "}", row[dColumn].ToString());
            }
            return resultStr;
        }
        #endregion
    }
}
