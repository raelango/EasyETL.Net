using EasyETL.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLClient : EasyETLConfiguration
    {
        public string ClientName;
        public string ClientID;

        public List<EasyETLAction> Actions = new List<EasyETLAction>();
        public List<EasyETLDatasource> Datasources = new List<EasyETLDatasource>();
        public List<EasyETLWriter> Writers = new List<EasyETLWriter>();
        public List<EasyETLEndpoint> Endpoints = new List<EasyETLEndpoint>();

        public List<EasyETLJob> ETLs = new List<EasyETLJob>();

        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);
            Actions = new List<EasyETLAction>();
            foreach (XmlNode childNode in xNode.SelectNodes("actions/action"))
            {
                EasyETLAction etlAction = new EasyETLAction();
                etlAction.ReadSettings(childNode);
                Actions.Add(etlAction);
            }

            Datasources = new List<EasyETLDatasource>();
            foreach (XmlNode childNode in xNode.SelectNodes("datasources/datasource"))
            {
                EasyETLDatasource etlDatasource = new EasyETLDatasource();
                etlDatasource.ReadSettings(childNode);
                Datasources.Add(etlDatasource);
            }

            Writers = new List<EasyETLWriter>();
            foreach (XmlNode childNode in xNode.SelectNodes("exports/export"))
            {
                EasyETLWriter etlWriter = new EasyETLWriter();
                etlWriter.ReadSettings(childNode);
                Writers.Add(etlWriter);
            }

            Endpoints = new List<EasyETLEndpoint>();
            foreach (XmlNode childNode in xNode.SelectNodes("endpoints/endpoint"))
            {
                EasyETLEndpoint etlEndpoint = new EasyETLEndpoint();
                etlEndpoint.ReadSettings(childNode);
                Endpoints.Add(etlEndpoint);
            }

            ETLs = new List<EasyETLJob>();
            foreach (XmlNode childNode in xNode.SelectNodes("etls/etl"))
            {
                EasyETLJob etlJob = new EasyETLJob();
                etlJob.ReadSettings(childNode);
                ETLs.Add(etlJob);
            }

        }

        public override void ReadSettingsFromDictionary()
        {
            ClientName = GetSetting("name");
            ClientID = GetSetting("id");
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("name", ClientName);
            SetSetting("id", ClientID);
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
            //Write all actions
            XmlElement actionsNode = xNode.OwnerDocument.CreateElement("actions");
            xNode.AppendChild(actionsNode);
            foreach (EasyETLAction action in Actions)
            {
                XmlElement actionNode = xNode.OwnerDocument.CreateElement("action");
                actionsNode.AppendChild(actionNode);
                action.WriteSettings(actionNode);
            }

            //Write all Data sources
            XmlElement datasourcesNode = xNode.OwnerDocument.CreateElement("datasources");
            xNode.AppendChild(datasourcesNode);
            foreach (EasyETLDatasource datasource in Datasources)
            {
                XmlElement datasourceNode = xNode.OwnerDocument.CreateElement("datasource");
                datasourcesNode.AppendChild(datasourceNode);
                datasource.WriteSettings(datasourceNode);
            }

            //Write all actions
            XmlElement writersNode = xNode.OwnerDocument.CreateElement("exports");
            xNode.AppendChild(writersNode);
            foreach (EasyETLWriter writer in Writers)
            {
                XmlElement writerNode = xNode.OwnerDocument.CreateElement("export");
                writersNode.AppendChild(writerNode);
                writer.WriteSettings(writerNode);
            }

            //Write all actions
            XmlElement endpointsNode = xNode.OwnerDocument.CreateElement("endpoints");
            xNode.AppendChild(endpointsNode);
            foreach (EasyETLEndpoint endpoint in Endpoints)
            {
                XmlElement endpointNode = xNode.OwnerDocument.CreateElement("endpoint");
                endpointsNode.AppendChild(endpointNode);
                endpoint.WriteSettings(endpointNode);
            }

        }

    }
}
