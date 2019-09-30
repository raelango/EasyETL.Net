using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    [DisplayName("Delimited Data Writer")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("Delimiter", "Specify the delimiter to be used", ",", ".")]
    [EasyField("IncludeHeaders","Include the table names in the output?","True","","True;False")]
    [EasyField("IncludeQuotes", "Surround the Field Name and Values by quotes?", "True", "", "True;False")]
    public class DelimitedDatasetWriter : FileDatasetWriter
    {
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
            if (PrintTableHeader)
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
