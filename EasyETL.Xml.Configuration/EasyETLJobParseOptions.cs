using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{

    //this implementation is incomplete....
    public class EasyETLJobParseOptions : EasyETLConfiguration
    {
        public string ProfileName;

        public override void ReadSettingsFromDictionary()
        {
            ProfileName = GetSetting("profilename");
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("profilename", ProfileName);
        }
    }
}
