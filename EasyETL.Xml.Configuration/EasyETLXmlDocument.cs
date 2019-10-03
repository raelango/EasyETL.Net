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
        }

        public override void Save(string filename)
        {
            this.InnerXml = "";
            XmlElement clientsNode = this.CreateElement("clients");
            this.AppendChild(clientsNode);
            foreach (EasyETLClient client in Clients)
            {
                XmlElement clientNode = this.CreateElement("client");
                clientsNode.AppendChild(clientNode);
                client.WriteSettings(clientNode);
            }
            base.Save(filename);
        }

    }
}
