using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Endpoint
{
    public abstract class AbstractFileEasyEndpoint : AbstractEasyEndpoint, IEasyFieldInterface
    {
        public override bool Overwrite { get; set; }


        public virtual bool IsFieldSettingsComplete()
        {
             throw new NotImplementedException();
        }

        public void LoadFieldSettings(Dictionary<string, string> settingsDictionary)
        {
            foreach (KeyValuePair<string, string> kvPair in settingsDictionary)
            {
                LoadSetting(kvPair.Key, kvPair.Value);
            }
        }

        public void SaveFieldSettingsToXmlNode(XmlNode parentNode)
        {
            Dictionary<string, string> settingsDict = GetSettingsAsDictionary();
            foreach (KeyValuePair<string, string> kvPair in settingsDict)
            {
                XmlElement xNode = parentNode.OwnerDocument.CreateElement("field");
                xNode.SetAttribute("name",kvPair.Key);
                xNode.SetAttribute("value", kvPair.Value);
                parentNode.AppendChild(xNode);
            }
        }


        public virtual void LoadSetting(string fieldName, string fieldValue)
        {
            switch (fieldName.ToLower())
            {
                case "overwrite":
                    Overwrite = Convert.ToBoolean(fieldValue); break;
            }
        }

        public virtual Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            resultDict.Add("overwrite", Overwrite.ToString());
            return resultDict;
        }
        
    }
}
