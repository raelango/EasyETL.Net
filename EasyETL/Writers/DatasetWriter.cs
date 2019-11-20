using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace EasyETL.Writers
{

    public class RowWrittenEventArgs : EventArgs
    {
        public int RowNumber { get; set; }
        public DataRow Row;
        public string TableName;

    }

    public abstract class DatasetWriter : IEasyFieldInterface
    {
        protected DataSet _dataSet = null;
        protected string outputString = String.Empty;
        public bool PrintHeader = true;
        public bool PrintFooter = true;
        public bool PrintTableHeader = true;
        public bool PrintTableFooter = true;

        public event EventHandler<RowWrittenEventArgs> RowWritten;

        public DatasetWriter()
        {
            _dataSet = null;
        }

        public DatasetWriter(DataSet dataSet)
        {
            _dataSet = dataSet;
        }

        public virtual void Write()
        {
            if (_dataSet != null)
            {
                outputString = BuildOutputString();
            }
            else
            {
                throw new EasyLoaderException("Dataset is null");
            }
        }

        public void Write(DataSet dataSet)
        {
            _dataSet = dataSet;
            Write();
        }

        public virtual string BuildOutputString()
        {
            if (_dataSet != null)
            {
                outputString = String.Empty;
                if (PrintHeader) outputString = BuildHeaderString();
                foreach (DataTable dt in _dataSet.Tables)
                {
                    if (PrintTableHeader) outputString += BuildTableHeaderString(dt);
                    int rowNumber = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        outputString += BuildRowString(dr);
                        outputString += RowDelimiter(dt.Rows[dt.Rows.Count - 1].Equals(dr));
                        OnRowWritten(new RowWrittenEventArgs() { RowNumber = rowNumber, Row = dr, TableName = dt.TableName });
                        rowNumber++;
                    }
                    if (PrintTableFooter) outputString += BuildTableFooterString(dt);
                    outputString += TableDelimiter(_dataSet.Tables[_dataSet.Tables.Count - 1].Equals(dt));
                }
                if (PrintFooter) outputString += BuildFooterString();
            }
            return outputString;
        }

        public virtual string BuildHeaderString()
        {
            return "Tables = " + _dataSet.Tables.Count.ToString() + Environment.NewLine;
        }

        public virtual string BuildTableHeaderString(DataTable dt)
        {
            return dt.TableName + " = " + dt.Rows.Count.ToString() + Environment.NewLine;
        }

        public virtual string BuildRowString(DataRow dr)
        {
            return String.Empty;
        }

        public virtual string RowDelimiter(bool lastRow)
        {
            return String.Empty;
        }

        public virtual string TableDelimiter(bool lastTable)
        {
            return String.Empty;
        }

        public virtual string BuildTableFooterString(DataTable dt)
        {
            return String.Empty;
        }

        public virtual string BuildFooterString()
        {
            return String.Empty;
        }

        protected virtual void OnRowWritten(RowWrittenEventArgs e)
        {
            RowWritten?.Invoke(this, e);
        }

        protected string GetColumnName(DataColumn dColumn)
        {
            return String.IsNullOrWhiteSpace(dColumn.Caption) ? dColumn.ColumnName : dColumn.Caption;
        }


        public virtual bool IsFieldSettingsComplete()
        {
            return true;
        }

        public virtual bool CanFunction()
        {
            return IsFieldSettingsComplete();
        }

        public void LoadFieldSettings(Dictionary<string, string> settingsDictionary)
        {
            foreach (KeyValuePair<string, string> kvPair in settingsDictionary.DecryptPasswords(this.GetEasyFields()))
            {
                LoadSetting(kvPair.Key,kvPair.Value);
            }
        }

        public void SaveFieldSettingsToXmlNode(XmlNode parentNode)
        {
            parentNode.SaveFieldSettingsToXmlNode(GetSettingsAsDictionary());
        }

        public virtual void LoadSetting(string fieldName, string fieldValue)
        {
            switch (fieldName.ToLower())
            {
                case "includeheader":
                    PrintTableHeader = Convert.ToBoolean(fieldValue); break;
            }
        }

        public virtual Dictionary<string, string> GetSettingsAsDictionary()
        {
            return new Dictionary<string, string>
            {
                { "includeheader", PrintTableHeader.ToString() }
            };
        }


    }
}
