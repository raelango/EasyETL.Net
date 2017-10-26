using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace EasyETL.Writers
{
    public class HtmlDatasetWriter : FileDatasetWriter
    {
        public Dictionary<string, string> DocProperties = new Dictionary<string, string>();

        public HtmlDatasetWriter()
            : base()
        {
        }

        public HtmlDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public HtmlDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet, fileName)
        {
        }

        public override string BuildOutputString()
        {
            return base.BuildOutputString();
        }

        public override string BuildHeaderString()
        {
            string returnStr = "<HTML>" + Environment.NewLine;
            returnStr += "<HEAD>" + Environment.NewLine;
            returnStr += "</HEAD>" + Environment.NewLine;
            returnStr += "<BODY>" + Environment.NewLine;
            return returnStr;
        }

        public override string BuildTableHeaderString(DataTable dt)
        {
            string returnStr = "<Table width='100%' ID='" + dt.TableName +"'>" + Environment.NewLine;

            returnStr += "<THEAD>" + Environment.NewLine;
            foreach (DataColumn dc in dt.Columns)
            {
                returnStr += "<TH>" + dc.ColumnName + "</TH>" + Environment.NewLine;
            }
            returnStr += "</THEAD>" + Environment.NewLine;
            return returnStr;
        }

        public override string BuildRowString(DataRow dr)
        {
            string returnStr = "<TR>" + Environment.NewLine;
            foreach (DataColumn dc in dr.Table.Columns)
            {
                returnStr += "<TD>" + WebUtility.HtmlEncode(dr[dc].ToString()) + "</TD>" + Environment.NewLine;
            }
            returnStr += "</TR>" + Environment.NewLine;
            return returnStr;
        }

        public override string BuildTableFooterString(DataTable dt)
        {
            return "</TABLE>" + Environment.NewLine;
        }

        public override string BuildFooterString()
        {
            return "</BODY>" + Environment.NewLine + "</HTML>" + Environment.NewLine;
        }

    }
}
