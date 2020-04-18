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
            if (xNode.SelectSingleNode("datasource") != null) Datasource.ReadSettings(xNode.SelectSingleNode("datasource"));

            Actions = new List<EasyETLJobAction>();
            foreach (XmlNode actionNode in xNode.SelectNodes("actions/action"))
            {
                EasyETLJobAction easyETLJobAction = new EasyETLJobAction();
                easyETLJobAction.ReadSettings(actionNode);
                Actions.Add(easyETLJobAction);
            }

            ParseOptions = new EasyETLJobParseOptions();
            if (xNode.SelectSingleNode("parseoptions") != null) ParseOptions.ReadSettings(xNode.SelectSingleNode("parseoptions"));


            Transformations = new EasyETLJobTransformations();
            if (xNode.SelectSingleNode("transformations") != null) Transformations.ReadSettings(xNode.SelectSingleNode("transformations"));

            Exports = new List<EasyETLJobExport>();
            foreach (XmlNode exportNode in xNode.SelectNodes("exports/export"))
            {
                EasyETLJobExport easyETLJobExport = new EasyETLJobExport();
                easyETLJobExport.ReadSettings(exportNode);
                Exports.Add(easyETLJobExport);
            }

            DefaultPermission = new EasyETLPermission();
            if (xNode.SelectSingleNode("permissions") != null) DefaultPermission.ReadSettings(xNode.SelectSingleNode("permissions"));

            Permissions = new List<EasyETLPermission>();
            foreach (XmlNode permissionNode in xNode.SelectNodes("permissions/permission"))
            {
                EasyETLPermission permission = new EasyETLPermission() { CanViewSettings = true, CanEditSettings = true, CanExportData = true };
                permission.ReadSettings(permissionNode);
                Permissions.Add(permission);
            }

            if (Permissions.Count == 0) Permissions.Add(new EasyETLPermission() { CanViewSettings = true, CanEditSettings = true, CanExportData = true });
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

            XmlElement actionsElement = xNode.OwnerDocument.CreateElement("actions");
            xNode.AppendChild(actionsElement);
            foreach (EasyETLJobAction eTLJobAction in Actions)
            {
                XmlElement actionElement = xNode.OwnerDocument.CreateElement("action");
                actionsElement.AppendChild(actionElement);
                eTLJobAction.WriteSettings(actionElement);
            }

            XmlElement parseElement = xNode.OwnerDocument.CreateElement("parseoptions");
            xNode.AppendChild(parseElement);
            ParseOptions.WriteSettings(parseElement);

            XmlElement transformationsElement = xNode.OwnerDocument.CreateElement("transformations");
            xNode.AppendChild(transformationsElement);
            Transformations.WriteSettings(transformationsElement);


            XmlElement exportsElement = xNode.OwnerDocument.CreateElement("exports");
            xNode.AppendChild(exportsElement);
            foreach (EasyETLJobExport jobExport in Exports)
            {
                XmlElement exportElement = xNode.OwnerDocument.CreateElement("export");
                exportsElement.AppendChild(exportElement);
                jobExport.WriteSettings(exportElement);
            }


            XmlElement permissionsElement = xNode.OwnerDocument.CreateElement("permissions");
            xNode.AppendChild(permissionsElement);
            DefaultPermission.WriteSettings(permissionsElement);

            foreach (EasyETLPermission permission in Permissions)
            {
                XmlElement permissionElement = xNode.OwnerDocument.CreateElement("permission");
                permissionsElement.AppendChild(permissionElement);
                permission.WriteSettings(permissionElement);
            }

        }
    }
}
