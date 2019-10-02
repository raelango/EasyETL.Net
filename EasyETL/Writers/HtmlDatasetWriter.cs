using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.ComponentModel;
using EasyETL.Attributes;

namespace EasyETL.Writers
{
    [DisplayName("HTML Writer")]
    [EasyField("IncludeHeader", "Include Table Header", "True", "", "True;False")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("TemplateFileName", "Name of template file to use. Leave Empty for no template file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    public class HtmlDatasetWriter : FileDatasetWriter
    {
        public string TemplateFileName = String.Empty;
        public Dictionary<string, string> DocProperties = new Dictionary<string, string>();

        public HtmlDatasetWriter()
            : base()
        {
        }

        public HtmlDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public HtmlDatasetWriter(DataSet dataSet, string fileName, string templateFileName = "")
            : base(dataSet, fileName)
        {
            TemplateFileName = templateFileName;
        }

        public override string BuildOutputString()
        {
            return base.BuildOutputString();
        }

        public override string BuildHeaderString()
        {
            string returnStr = "<HTML>" + Environment.NewLine;
            if (!String.IsNullOrWhiteSpace(TemplateFileName) && (File.Exists(TemplateFileName)))
            {
                string htmlContents = File.ReadAllText(TemplateFileName);
                returnStr += GetHtmlHeader(htmlContents) + Environment.NewLine;
                returnStr += GetBodyContents(htmlContents) + Environment.NewLine;
            }
            else
            {
                returnStr += "<HEAD>" + Environment.NewLine;
                returnStr += "</HEAD>" + Environment.NewLine;
                returnStr += "<BODY>" + Environment.NewLine;
            }
            return returnStr;
        }

        private string GetBodyContents(string htmlContents)
        {
            int headEndPosition = htmlContents.IndexOf("</body>", StringComparison.CurrentCultureIgnoreCase);
            if (headEndPosition >= 0)
            {
                htmlContents = htmlContents.Substring(0, headEndPosition);
            }
            int headStartPosition = htmlContents.IndexOf("<body>", StringComparison.CurrentCultureIgnoreCase);
            if (headStartPosition < 1) headStartPosition = htmlContents.IndexOf("<body ", StringComparison.CurrentCultureIgnoreCase);
            if (headStartPosition > 0)
            {
                htmlContents = htmlContents.Substring(headStartPosition);
            }
            else
            {
                htmlContents = "<body>";
            }
            return htmlContents;
        }

        private string GetHtmlHeader(string htmlContents)
        {
            int headEndPosition = htmlContents.IndexOf("</head>", StringComparison.CurrentCultureIgnoreCase);
            if ( headEndPosition >= 0)
            {
                htmlContents = htmlContents.Substring(0, headEndPosition);
            }
            int headStartPosition = htmlContents.IndexOf("<head>",StringComparison.CurrentCultureIgnoreCase);
            if (headStartPosition < 1) headStartPosition = htmlContents.IndexOf("<head ", StringComparison.CurrentCultureIgnoreCase);
            if (headStartPosition > 0)
            {
                htmlContents = htmlContents.Substring(headStartPosition);
                htmlContents += "</head>";
            }
            else
            {
                htmlContents = "<head></head>";
            }
            return htmlContents;
        }

        public override string BuildTableHeaderString(DataTable dt)
        {
            string returnStr = "<Table width='100%' ID='" + dt.TableName +"'>" + Environment.NewLine;

            if (PrintTableHeader)
            {
                returnStr += "<THEAD>" + Environment.NewLine;
                foreach (DataColumn dc in dt.Columns)
                {
                    returnStr += "<TH>" + GetColumnName(dc) + "</TH>" + Environment.NewLine;
                }
                returnStr += "</THEAD>" + Environment.NewLine;
            }
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

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "templatefilename":
                    TemplateFileName = fieldValue; break;
            }
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string,string> settingsDict = base.GetSettingsAsDictionary();
            settingsDict.Add("templatefilename", TemplateFileName);
            return settingsDict;
        }

    }
}
