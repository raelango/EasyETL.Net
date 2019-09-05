using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Utils
{
    public static class Configuration
    {
        public static XmlNode GetProfileNode(string profileName)
        {
            return GetXmlNode("profiles.xml", "profiles/" + profileName);
        }

        public static XmlNode GetDatasetNode(string datasetName)
        {
            return GetXmlNode("DataSets.xml", "datasets/" + datasetName);
        }

        public static XmlNode GetDataTableNode(string dataTableName)
        {
            return GetXmlNode("DataTables.xml", "datatables/" + dataTableName);
        }

        private static XmlNode GetXmlNode(string fileName, string nodePath)
        {
            XmlNode node = null;
            try
            {

                if ( (!String.IsNullOrWhiteSpace(fileName) && File.Exists(fileName)) && (!String.IsNullOrWhiteSpace(nodePath)))
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(fileName);
                    node = xDoc.SelectSingleNode(nodePath);
                }
            }
            catch
            {
                node = null;
            }
            return node;
        }

    }
}
