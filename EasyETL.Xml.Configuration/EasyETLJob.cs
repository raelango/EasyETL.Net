using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJob : EasyETLConfiguration
    {
        public string ETLName;
        public string ETLID;
        public bool AutoRefresh;

        public EasyETLJobDatasource Datasource = new EasyETLJobDatasource();

        public List<EasyETLPermission> Permissions = new List<EasyETLPermission>();
        public EasyETLPermission DefaultPermission = new EasyETLPermission();

        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);

            Datasource = new EasyETLJobDatasource();
            if (xNode.SelectSingleNode("datasource") !=null) Datasource.ReadSettings(xNode.SelectSingleNode("datasource"));

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
