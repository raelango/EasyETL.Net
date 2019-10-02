using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Writers
{
    [DisplayName("XML Writer")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("TemplateFileName", "Name of template file (XSLT File) to use. Leave Empty for no template file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    public class XmlDatasetWriter : FileDatasetWriter
    {
        private string xsltFileName;
        public XmlDatasetWriter()
            : base()
        {
        }

        public XmlDatasetWriter(DataSet dataSet, string filename = "", string XsltFileName = "")
            : base(dataSet, filename)
        {
            xsltFileName = XsltFileName;
        }

        public virtual void Write(DataSet dataSet, string fileName = "", string XsltFileName = "")
        {
            xsltFileName = XsltFileName;
            _dataSet = dataSet;
            Write(fileName);
        }

        public override string BuildOutputString()
        {
            _dataSet.DataSetName = "xml";
            string outputString = _dataSet.GetXml();
            if ((!String.IsNullOrEmpty(xsltFileName)) && (File.Exists(xsltFileName)))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(outputString);
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xsltFileName);
                StringBuilder resultString = new StringBuilder();
                StringWriter sWriter = new StringWriter(resultString);
                xslt.Transform(xDoc, null, sWriter);
                sWriter.Close();
                outputString = resultString.ToString();
            }
            return outputString;
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "templatefilename":
                    xsltFileName = fieldValue; break;
            }
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> settingsDict = base.GetSettingsAsDictionary();
            settingsDict.Add("templatefilename", xsltFileName);
            return settingsDict;
        }

    }
}
