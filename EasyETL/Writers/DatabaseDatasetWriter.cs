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
using System.ComponentModel;
using System.Data.Common;
using EasyETL.Attributes;

namespace EasyETL.Writers
{
    [DisplayName("Database Writer")]
    [EasyField("ConnectionType", "Database Connection Type", "Odbc", "", "Odbc;OleDb;Sql")]
    [EasyField("ConnectionString", "Connection String")]
    [EasyField("TableName", "Name of database Table")]
    [EasyField("UniqueKeyColumns", "Provide all columns that form a unique key (separate using semicolon)")]
    [EasyField("InsertCommand", "Insert Command")]
    [EasyField("UpdateCommand", "Update Command")]
    public class DatabaseDatasetWriter : DatasetWriter, IDisposable
    {
        public DbConnection _connection = null;
        public string _connString = String.Empty;
        public string _tableName = String.Empty;
        public string _insertCommand = String.Empty;
        public string _updateCommand = String.Empty;
        public string _uniqueKeyColumns = String.Empty;
        public DatabaseType _dbType = DatabaseType.Odbc;

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

        public DatabaseDatasetWriter(DataSet dataSet, string connString, string insertCommand, string updateCommand = "", string tableName = "")
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
            if (_dataSet == null) throw new EasyLoaderException("Dataset is null");

            if (_dataSet.Tables.Count == 0) throw new EasyLoaderException("Dataset has no tables");

            if (String.IsNullOrWhiteSpace(_tableName)) _tableName = _dataSet.Tables[0].TableName;

            if (!_dataSet.Tables.Contains(_tableName)) throw new EasyLoaderException("Dataset does not contain table <<" + _tableName + ">>");

            if (_dataSet.Tables[_tableName].Rows.Count == 0) throw new EasyLoaderException("The table <<" + _tableName + ">> in the dataset is empty");

            EstablishDatabaseConnection();
            SetCommands(String.IsNullOrWhiteSpace(_insertCommand), String.IsNullOrWhiteSpace(_updateCommand));

            int rowNumber = 0;
            foreach (DataRow dataRow in _dataSet.Tables[_tableName].Rows)
            {
                try
                {
                    if ((dataRow.RowState == DataRowState.Added) || (dataRow.RowState == DataRowState.Modified))
                    {
                        WriteRowToDatabase(dataRow, rowNumber);
                        dataRow.AcceptChanges();
                    }
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

        public virtual void SetCommands(bool bIns, bool bUpd)
        {
            if ((!bIns) && (!bUpd)) return;
            if ((_dataSet != null) && String.IsNullOrWhiteSpace(_insertCommand) && (_connection.State == ConnectionState.Open))
            {
                List<string> AllColumns = GetColumnsOfCollection("Columns");
                List<string> matchedColumns = new List<string>();
                foreach (string columnName in AllColumns)
                {
                    if (_dataSet.Tables[0].Columns.Contains(columnName)) matchedColumns.Add(columnName);
                }
                if (bIns) {
                    _insertCommand = "";
                    string nameCommand = "";
                    string valueCommand = "";
                    foreach (string columnName in matchedColumns) {
                        if (!String.IsNullOrWhiteSpace(nameCommand)) nameCommand += ", ";
                        if (!String.IsNullOrWhiteSpace(valueCommand)) valueCommand += ", ";
                        nameCommand += "[" + columnName + "]";
                        valueCommand += "'{" + columnName + "}'";
                    }
                    _insertCommand = "INSERT INTO [" + _tableName + "] (" + nameCommand + ") VALUES (" + valueCommand + ")"; 
                }
                if (bUpd) {
                    _updateCommand = "";
                    string whereCommand = "";
                    List<string> uniqueColumns =  new List<string>(_uniqueKeyColumns.Split(';').AsEnumerable());
                    foreach (string columnName in matchedColumns) {
                        if (uniqueColumns.Contains(columnName)) {
                            if (!String.IsNullOrWhiteSpace(whereCommand)) whereCommand += " AND ";
                            whereCommand += "[" + columnName + "] = '{" + columnName + "}'";
                        }
                        else {
                            if (!String.IsNullOrWhiteSpace(_updateCommand)) _updateCommand += ", ";
                            _updateCommand += "[" + columnName + "] = '{" + columnName + "}'";
                        }
                    }
                    if (whereCommand.Length > 0)
                        _updateCommand = "Update [" + _tableName + "] Set " + _updateCommand + " WHERE " + whereCommand;
                    else
                        _updateCommand = "";
                }
            }

        }

        public List<string> GetColumnsOfCollection(string collectionName)
        {
            List<string> result = new List<string>();
            DbCommand command = _connection.CreateCommand();
            string[] restrictions = new string[] { null, null, _tableName };
            DataTable table = _connection.GetSchema(collectionName, restrictions);

            if (string.IsNullOrEmpty(_tableName)) throw new Exception("Table name must be set.");

            foreach (DataRow row in table.Rows)
            {
                result.Add(row["column_name"].ToString());
            }

            return result;
        }


        public virtual void SetUpdateCommand()
        {
        }

        public virtual void EstablishDatabaseConnection()
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

        public override bool IsFieldSettingsComplete()
        {
            return base.IsFieldSettingsComplete() && !String.IsNullOrWhiteSpace(_connString) && ((!String.IsNullOrWhiteSpace(_tableName) && !String.IsNullOrWhiteSpace(_uniqueKeyColumns)) || (!String.IsNullOrWhiteSpace(_insertCommand) && !String.IsNullOrWhiteSpace(_updateCommand)));
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            EstablishDatabaseConnection();
            if ((_connection != null) && (_connection.State == ConnectionState.Open))
            {
                CloseDatabaseConnection();
                return true;
            }
            return false;
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("ConnectionType", _dbType.ToString());
            resultDict.Add("ConnectionString", _connString);
            resultDict.Add("TableName", _tableName);
            resultDict.Add("UniqueKeyColumns", _uniqueKeyColumns);
            resultDict.Add("InsertCommand", _insertCommand);
            resultDict.Add("UpdateCommand", _updateCommand);
            return resultDict;
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "connectiontype":
                    _dbType = (DatabaseType)Enum.Parse(typeof(DatabaseType), fieldValue); break;
                case "connectionstring":
                    _connString = fieldValue; break;
                case "tablename":
                    _tableName = fieldValue; break;
                case "uniquekeycolumns":
                    _uniqueKeyColumns = fieldValue; break;
                case "insertcommand":
                    _insertCommand = fieldValue; break;
                case "updatecommand":
                    _updateCommand = fieldValue; break;
            }
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
                if (!String.IsNullOrWhiteSpace(_updateCommand))
                {
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

                if (bInsertRecord)
                {
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
            Write(connString, insertCommand, updateCommand);
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


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection = null;
                }
            }
        }
    }
}
