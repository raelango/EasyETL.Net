using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJobConfiguration : EasyETLConfiguration
    {
        public string ETLName;
        public string ETLID;
        public bool AutoRefresh;

        public EasyETLJobDatasource Datasource = new EasyETLJobDatasource();

        public List<EasyETLJobAction> Actions = new List<EasyETLJobAction>();

        public EasyETLJobParseOptions ParseOptions = new EasyETLJobParseOptions();

        public List<EasyETLJobExport> Exports = new List<EasyETLJobExport>();

        public EasyETLJobTransformations Transformations = new EasyETLJobTransformations();

        public List<EasyETLPermission> Permissions = new List<EasyETLPermission>();
        public EasyETLPermission DefaultPermission = new EasyETLPermission();

        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);

            Datasource = new EasyETLJobDatasource();
            if (xNode.SelectSingleNode("datasource") !=null) Datasource.ReadSettings(xNode.SelectSingleNode("datasource"));

            Actions = new List<EasyETLJobAction>();
            foreach (XmlNode actionNode in xNode.SelectNodes("actions/action"))
            {
                EasyETLJobAction easyETLJobAction = new EasyETLJobAction();
                easyETLJobAction.ReadSettings(actionNode);
                Actions.Add(easyETLJobAction);
            }

            ParseOptions = new EasyETLJobParseOptions();
            if (xNode.SelectSingleNode("parseoptions") != null) ParseOptions.ReadSettings(xNode.SelectSingleNode("parseoptions"));

            Exports = new List<EasyETLJobExport>();
            foreach (XmlNode exportNode in xNode.SelectNodes("exports/export"))
            {
                EasyETLJobExport easyETLJobExport = new EasyETLJobExport();
                easyETLJobExport.ReadSettings(exportNode);
                Exports.Add(easyETLJobExport);
            }

            Transformations = new EasyETLJobTransformations();
            if (xNode.SelectNodes("transformations") != null) Transformations.ReadSettings(xNode.SelectSingleNode("transformations"));

            DefaultPermission = new EasyETLPermission();
            if (xNode.SelectSingleNode("permissions") != null) DefaultPermission.ReadSettings(xNode.SelectSingleNode("permissions"));

            Permissions = new List<EasyETLPermission>();
            foreach (XmlNode permissionNode in xNode.SelectNodes("permissions/permission"))
            {
                EasyETLPermission permission = new EasyETLPermission();
                permission.ReadSettings(permissionNode);
                Permissions.Add(permission);
            }

        }

        public override void ReadSettingsFromDictionary()
        {
            ETLName = GetSetting("name");
            ETLID = GetSetting("id");
            AutoRefresh = Convert.ToBoolean(GetSetting("AutoRefresh", "False"));

        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("name", ETLName);
            SetSetting("id", ETLID);
            SetSetting("autorefresh", AutoRefresh.ToString());            
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
            XmlElement dsElement = xNode.OwnerDocument.CreateElement("datasource");
            xNode.AppendChild(dsElement);
            Datasource.WriteSettings(dsElement);
        }
    }
}
