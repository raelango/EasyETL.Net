using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Writers
{
    public class XmlDatasetWriter : FileDatasetWriter
    {
        private string xsltFileName;
        public XmlDatasetWriter()
            : base()
        {
        }

        public XmlDatasetWriter(DataSet dataSet, string XsltFileName = "", string filename = "")
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
    }
}
