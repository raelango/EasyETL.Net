using EasyETL.Attributes;
using EasyETL.Endpoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            EventHandler<RowWrittenEventArgs> handler = RowWritten;
            if (handler != null)
            {
                handler(this, e);
            }
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
            return false;
        }

        public void LoadFieldSettings(Dictionary<string, string> settingsDictionary)
        {
            foreach (KeyValuePair<string, string> kvPair in settingsDictionary)
            {
                LoadSetting(kvPair.Key, kvPair.Value);
            }
        }

        public void SaveFieldSettingsToXmlNode(System.Xml.XmlNode parentNode)
        {
            Dictionary<string, string> settingsDict = GetSettingsAsDictionary();
            foreach (KeyValuePair<string, string> kvPair in settingsDict)
            {
                XmlElement xNode = parentNode.OwnerDocument.CreateElement("field");
                xNode.SetAttribute("name", kvPair.Key);
                xNode.SetAttribute("value", kvPair.Value);
                parentNode.AppendChild(xNode);
            }
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
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            resultDict.Add("includeheader", PrintTableHeader.ToString());
            return resultDict;
        }


    }
}
