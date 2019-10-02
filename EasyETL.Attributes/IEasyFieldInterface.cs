using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Attributes
{
    public interface IEasyFieldInterface
    {
        bool IsFieldSettingsComplete();
        void LoadFieldSettings(Dictionary<string, string> settingsDictionary);
        void SaveFieldSettingsToXmlNode(XmlNode parentNode);
    }
}
