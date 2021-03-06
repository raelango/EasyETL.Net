﻿using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    [DisplayName("Excel Writer")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("IncludeHeader", "Include Table Header", "True", "", "True;False")]
    public class ExcelDatasetWriter : FileDatasetWriter
    {
        public Dictionary<string, string> DocProperties = new Dictionary<string, string>();

        public ExcelDatasetWriter()
            : base()
        {
        }

        public ExcelDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public ExcelDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet,fileName)
        {
        }

        public override string BuildHeaderString()
        {
            string returnStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
            returnStr += "<?mso-application progid=\"Excel.Sheet\"?>" + Environment.NewLine;
            returnStr += "<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\">" + Environment.NewLine;
            returnStr += "<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">" + Environment.NewLine;
            foreach (KeyValuePair<string, string> kvPair in DocProperties)
            {
                returnStr += String.Format("<{0}>{1}</{0}>" + Environment.NewLine, kvPair.Key, kvPair.Value);
            }
            returnStr += "</DocumentProperties>" + Environment.NewLine;
            return returnStr;
        }

        public override string BuildTableHeaderString(DataTable dt)
        {
            string returnStr = "<Worksheet ss:Name=\""  + dt.TableName + "\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">" + Environment.NewLine;
            returnStr += "<Table>" + Environment.NewLine;

            if (PrintTableHeader)
            {
                returnStr += "<Row>" + Environment.NewLine;
                foreach (DataColumn dc in dt.Columns)
                {
                    returnStr += "<Cell><Data ss:Type=\"String\">" + GetColumnName(dc) + "</Data></Cell>" + Environment.NewLine;
                }
                returnStr += "</Row>" + Environment.NewLine;
            }
            return returnStr;
        }

        public override string BuildRowString(DataRow dr)
        {
            string returnStr = "<Row>" + Environment.NewLine;
            foreach (DataColumn dc in dr.Table.Columns)
            {
                returnStr += "<Cell><Data ss:Type=\"String\">" + dr[dc].ToString() + "</Data></Cell>" + Environment.NewLine;
            }
            returnStr += "</Row>" + Environment.NewLine;
            return returnStr;
        }

        public override string BuildTableFooterString(DataTable dt)
        {
            return "</Table>" + Environment.NewLine + "</Worksheet>" + Environment.NewLine;
        }

        public override string BuildFooterString()
        {
            return "</Workbook>" + Environment.NewLine;
        }

    }
}
