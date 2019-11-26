using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJobDatasetTransformation : EasyETLConfiguration
    {
        public bool UseCustom;
        public bool UseDefault;
        public string[] SettingsCommands;

        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);
            string settings = xNode.InnerText;
            SettingsCommands = settings.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override void ReadSettingsFromDictionary()
        {
            UseCustom = Convert.ToBoolean(GetSetting("UseCustom", "False"));
            UseDefault = Convert.ToBoolean(GetSetting("UseDefault", "True"));
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("UseCustom", UseCustom.ToString());
            SetSetting("UseDefault", UseCustom.ToString());
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
            xNode.InnerText = String.Join(Environment.NewLine, SettingsCommands);
        }
    }
}
