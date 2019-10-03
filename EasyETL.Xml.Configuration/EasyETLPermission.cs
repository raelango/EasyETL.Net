using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLPermission : EasyETLConfiguration
    {
        public bool CanViewSettings;
        public bool CanEditSettings;
        public bool CanAddData;
        public bool CanEditData;
        public bool CanDeleteData;
        public bool CanExportData;

        public override void ReadSettingsFromDictionary()
        {
            CanViewSettings = Convert.ToBoolean(GetSetting("CanViewSettings", CanViewSettings.ToString()));
            CanEditSettings = Convert.ToBoolean(GetSetting("CanEditSettings", CanEditSettings.ToString()));
            CanAddData = Convert.ToBoolean(GetSetting("CanAddData", CanAddData.ToString()));
            CanEditData = Convert.ToBoolean(GetSetting("CanEditData", CanEditData.ToString()));
            CanDeleteData = Convert.ToBoolean(GetSetting("CanDeleteData", CanDeleteData.ToString()));
            CanExportData = Convert.ToBoolean(GetSetting("CanExportData", CanExportData.ToString()));
        }

        public override void WriteSettingsToDictionary()
        {
            AttributesDictionary = new Dictionary<string, string>();
            SetSetting("CanViewSettings", CanViewSettings.ToString());
            SetSetting("CanEditSettings", CanEditSettings.ToString());
            SetSetting("CanAddData", CanAddData.ToString());
            SetSetting("CanEditData", CanEditData.ToString());
            SetSetting("CanDeleteData", CanDeleteData.ToString());
            SetSetting("CanExportData", CanExportData.ToString());
        }
    }
}
