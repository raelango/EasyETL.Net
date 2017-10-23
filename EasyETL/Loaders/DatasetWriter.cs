using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Loaders
{
    public abstract class DatasetWriter
    { 
        protected DataSet _dataSet = null;
        protected string outputString = String.Empty;
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

        public virtual string BuildOutputString()
        {
            outputString = BuildHeaderString();
            foreach (DataTable dt in _dataSet.Tables)
            {
                outputString += BuildTableHeaderString(dt) ;
                foreach (DataRow dr in dt.Rows)
                {
                    outputString += BuildRowString(dr);
                    outputString += RowDelimeter(dt.Rows[dt.Rows.Count - 1].Equals(dr));
                }
                outputString += BuildTableFooterString(dt);
                outputString += TableDelimeter(_dataSet.Tables[_dataSet.Tables.Count -1].Equals(dt));
            }
            outputString += BuildFooterString();
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

        public virtual string RowDelimeter(bool lastRow)
        {
            return String.Empty;
        }

        public virtual string TableDelimeter(bool lastTable)
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

    }
}
