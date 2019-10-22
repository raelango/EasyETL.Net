using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.Xml;

namespace EasyETL.Endpoint
{
    public abstract class AbstractFileEasyEndpoint : AbstractEasyEndpoint, IEasyFieldInterface
    {
        public override bool Overwrite { get; set; }


        public virtual bool IsFieldSettingsComplete()
        {
            return true;
        }

        public void LoadFieldSettings(Dictionary<string, string> settingsDictionary)
        {
            foreach (KeyValuePair<string, string> kvPair in settingsDictionary.DecryptPasswords(this.GetEasyFields()))
            {
                LoadSetting(kvPair.Key, kvPair.Value);
            }
        }

        public void SaveFieldSettingsToXmlNode(XmlNode parentNode)
        {
            parentNode.SaveFieldSettingsToXmlNode(GetSettingsAsDictionary());
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

        public virtual bool CanFunction()
        {
            return false;
        }
        
    }
}
