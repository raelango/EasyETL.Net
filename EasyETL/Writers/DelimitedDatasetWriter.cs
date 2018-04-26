using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    public class DelimitedDatasetWriter : FileDatasetWriter
    {
        public bool IncludeHeaders = true;
        public char Delimiter = ',';
        public bool IncludeQuotes = true;

        public DelimitedDatasetWriter()
            : base()
        {
        }

        public DelimitedDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public DelimitedDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet, fileName)
        {
        }

        public override string BuildHeaderString()
        {
            return String.Empty;
        }

        public override string BuildTableHeaderString(DataTable dt)
        {
            string returnStr = String.Empty;
            if (IncludeHeaders)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    returnStr += (IncludeQuotes ? "\"":"") + GetColumnName(dc) + (IncludeQuotes ? "\"":"");
                    if (dc.Ordinal < (dt.Columns.Count - 1))
                    {
                        returnStr += Delimiter;
                    }
                    else
                    {
                        returnStr += Environment.NewLine;
                    }
                }
            }
            return returnStr;
        }

        public override string BuildRowString(DataRow dr)
        {
            string returnStr = String.Empty;
            foreach (DataColumn dc in dr.Table.Columns)
            {
                returnStr += (IncludeQuotes ? "\"" : "") + dr[dc].ToString() + (IncludeQuotes ? "\"" : "");
                if (dc.Ordinal < (dr.Table.Columns.Count - 1))
                {
                    returnStr += Delimiter;
                }
                else
                {
                    returnStr += Environment.NewLine;
                }
            }
            return returnStr;
        }


    }
}
