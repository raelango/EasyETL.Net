using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJobExport : EasyETLConfiguration
    {
        public string ExportName;
        public bool IsEnabled;
        public bool IsDefault;

        public override void ReadSettingsFromDictionary()
        {
            ExportName = GetSetting("name");
            IsEnabled = Convert.ToBoolean(GetSetting("Enabled", "False"));
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("Enabled", IsEnabled.ToString());
            SetSetting("Name", ExportName);
        }
    }
}
