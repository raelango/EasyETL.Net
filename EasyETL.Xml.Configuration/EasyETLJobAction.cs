using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJobAction : EasyETLConfiguration
    {
        public string ActionName;
        public bool IsEnabled;
        public bool IsDefault;

        public override void ReadSettingsFromDictionary()
        {
            ActionName = GetSetting("name");
            IsEnabled = Convert.ToBoolean(GetSetting("Enabled", "False"));
            IsDefault = Convert.ToBoolean(GetSetting("Default", "False"));
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("Default", IsDefault.ToString());
            SetSetting("Enabled", IsEnabled.ToString());
            SetSetting("Name", ActionName);
        }
    }
}
