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
        public string ParserType;
        public string Delimiter;
        public string Separator;
        public bool HasHeader;
        public string Comments;
        public List<int> Widths = new List<int>();
        public string TemplateString;

        public override void ReadSettingsFromDictionary()
        {
            ProfileName = GetSetting("profilename");
            ParserType = GetSetting("parsertype","Delimited");

        }

        public override void WriteSettingsToDictionary()
        {
            base.WriteSettingsToDictionary();
        }

        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
        }
    }
}
