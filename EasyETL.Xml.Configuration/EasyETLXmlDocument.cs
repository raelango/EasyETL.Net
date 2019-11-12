using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLXmlDocument : XmlDocument
    {
        public List<EasyETLClient> Clients = new List<EasyETLClient>();
        public Dictionary<string, string> GlobalValues = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        public void SetGlobalValue(string fieldName, string fieldValue)
        {
            AddGlobalValue(fieldName);
            GlobalValues[fieldName] = fieldValue;
        }

        public void AddGlobalValue(string fieldName)
        {
            if (!GlobalValues.ContainsKey(fieldName)) GlobalValues.Add(fieldName, String.Empty);
        }

        public override void Load(string filename)
        {
            base.Load(filename);

            Clients = new List<EasyETLClient>();
            foreach (XmlNode clientNode in SelectNodes("//clients/client"))
            {
                EasyETLClient etlClient = new EasyETLClient();
                etlClient.ReadSettings(clientNode);
                Clients.Add(etlClient);
            }

            //Let us load the global values from attributes of clients node
            XmlNode clientConfigNode = SelectSingleNode("clients");
            GlobalValues = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            if (clientConfigNode != null)
            {
                foreach (XmlAttribute xAttr in clientConfigNode.Attributes)
                    GlobalValues.Add(xAttr.Name, xAttr.Value);
            }
        }

        public override void Save(string filename)
        {
            this.InnerXml = "";
            XmlElement clientsNode = this.CreateElement("clients");
            foreach (KeyValuePair<string, string> kvPair in GlobalValues)
            {
                XmlAttribute xAttr = this.CreateAttribute(kvPair.Key);
                xAttr.Value = kvPair.Value;
                clientsNode.Attributes.Append(xAttr);
            }
            this.AppendChild(clientsNode);

            foreach (EasyETLClient client in Clients)
            {
                XmlElement clientNode = this.CreateElement("client");
                clientsNode.AppendChild(clientNode);
                client.WriteSettings(clientNode);
            }

            base.Save(filename);
        }

        public EasyETLClient GetClientConfiguration(string clientName)
        {
            return Clients.Where(c => c.ClientName.Equals(clientName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        }

    }
}
