using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJobAfterLoadTransformation : EasyETLConfiguration
    {
        public string SaveFileName;
        public string SaveProfileName;
        public string XsltFileName;
        public bool UseXslt;
        public string[] SettingsCommands;

        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);
            string settings = xNode.InnerText;
            SettingsCommands = settings.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override void ReadSettingsFromDictionary()
        {
            SaveFileName = GetSetting("SaveFileName");
            SaveProfileName = GetSetting("ProfileName");
            XsltFileName = GetSetting("XsltFileName");
            UseXslt = Convert.ToBoolean(GetSetting("usexslt", "False"));
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("SaveFileName", SaveFileName);
            SetSetting("ProfileName", SaveProfileName);
            SetSetting("XsltFileName", XsltFileName);
            SetSetting("UseXslt", UseXslt.ToString());
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
            xNode.InnerText = (SettingsCommands == null) ? "" : String.Join(Environment.NewLine, SettingsCommands);
        }
    }
}
