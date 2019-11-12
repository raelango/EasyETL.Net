using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public abstract class EasyETLConfiguration : IEasyETLConfiguration
    {
        public Dictionary<string, string> AttributesDictionary = new Dictionary<string, string>();
        public virtual void ReadSettings(XmlNode xNode)
        {
            LoadAttributesToDictionary(xNode);
            if ((!xNode.HasChildNodes) && (!AttributesDictionary.ContainsKey(xNode.Name))) AttributesDictionary.Add(xNode.Name, xNode.InnerXml);
            ReadSettingsFromDictionary();
        }

        public virtual void ReadSettingsFromDictionary()
        {
        }

        public virtual void WriteSettingsToDictionary()
        {
        }

        public virtual void WriteSettings(XmlNode xNode)
        {
            WriteSettingsToDictionary();
            SaveAttributesToNode(xNode);
        }

        private void LoadAttributesToDictionary(XmlNode xNode)
        {
            AttributesDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (XmlAttribute xAttr in xNode.Attributes)
            {
                AttributesDictionary.Add(xAttr.Name, xAttr.Value);
            }
        }

        private void SaveAttributesToNode(XmlNode xNode)
        {
            foreach (KeyValuePair<string, string> kvPair in AttributesDictionary)
            {
                if (!kvPair.Key.Equals(xNode.Name))
                {
                    XmlAttribute xAttr = (XmlAttribute)xNode.Attributes.GetNamedItem(kvPair.Key);
                    if (xAttr == null) {
                        xAttr = xNode.OwnerDocument.CreateAttribute(kvPair.Key);
                        xNode.Attributes.Append(xAttr);
                    }
                    xAttr.Value = kvPair.Value;
                }
            }
        }

        public string GetSetting(string settingName, string defaultValue = "")
        {
            if (!AttributesDictionary.ContainsKey(settingName)) return defaultValue;
            return AttributesDictionary[settingName];
        }

        public void SetSetting(string settingName, string settingValue)
        {
            if (AttributesDictionary.ContainsKey(settingName))
                AttributesDictionary[settingName] = settingValue;
            else 
                AttributesDictionary.Add(settingName, settingValue);
        }

    }
}
