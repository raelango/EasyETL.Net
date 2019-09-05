using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    public class JsonDatasetWriter : FileDatasetWriter
    {

        public JsonDatasetWriter()
            : base()
        {
        }

        public JsonDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public JsonDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet,fileName)
        {
        }

        public override string BuildOutputString()
        {
            return base.BuildOutputString();
        }

        public override string BuildHeaderString()
        {
            return "{" + Environment.NewLine;
        }

        public override string BuildTableHeaderString(DataTable dt)
        {
            return "\"" + dt.TableName + "\" : [" + Environment.NewLine;
        }

        public override string BuildRowString(DataRow dr)
        {
            string returnStr = "{" + Environment.NewLine;
            foreach (DataColumn dc in dr.Table.Columns)
            {
                returnStr += "\"" + GetColumnName(dc) + "\":\"" + dr[dc].ToString() + "\"";
                if (dc.Ordinal < (dr.Table.Columns.Count - 1))
                {
                    returnStr += ",";
                }
                returnStr += Environment.NewLine;
            }
            returnStr += "}" + Environment.NewLine;
            return returnStr;
        }

        public override string RowDelimeter(bool lastRow)
        {
            return lastRow ? String.Empty:"," + Environment.NewLine;
        }

        public override string TableDelimeter(bool lastTable)
        {
            return lastTable ? String.Empty : "," + Environment.NewLine;
        }


        public override string BuildTableFooterString(DataTable dt)
        {
            return "]" + Environment.NewLine ;
        }

        public override string BuildFooterString()
        {
            return "}" + Environment.NewLine;
        }

    }
}
