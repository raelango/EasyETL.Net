using EasyETL.Actions;
using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public abstract class EasyETLEasyField : EasyETLConfiguration
    {
        
        public string ActionID;
        public string ActionName;
        public string ClassName;
        public Type EasyFieldType;
        public Dictionary<string, string> Fields;

        public override void ReadSettings(XmlNode xNode)
        {
            Fields = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            base.ReadSettings(xNode);
            XmlNodeList fieldNodes = xNode.SelectNodes("field");
            foreach (XmlNode fieldNode in fieldNodes)
            {
                string AttributeName = (fieldNode.Attributes.GetNamedItem("name") == null) ? String.Empty : fieldNode.Attributes.GetNamedItem("name").Value;
                string AttributeValue = (fieldNode.Attributes.GetNamedItem("value") == null) ? String.Empty : fieldNode.Attributes.GetNamedItem("value").Value;
                if (!String.IsNullOrWhiteSpace(AttributeName)) {
                    if (!Fields.ContainsKey(AttributeName)) 
                        Fields.Add(AttributeName,AttributeValue);
                    else 
                        Fields[AttributeName] = AttributeValue;

                }
            }
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
            foreach (KeyValuePair<string, string> kvPair in Fields)
            {
                XmlElement fieldNode = xNode.OwnerDocument.CreateElement("field");
                fieldNode.SetAttribute("name", kvPair.Key);
                fieldNode.SetAttribute("value", kvPair.Value);
                xNode.AppendChild(fieldNode);
            }
        }

        public override void ReadSettingsFromDictionary()
        {
            base.ReadSettingsFromDictionary();
            ActionID = GetSetting("id");
            ActionName = GetSetting("name");
            ClassName = GetSetting("classname");
            if (String.IsNullOrWhiteSpace(ClassName))
                EasyFieldType = null;
            else
                EasyFieldType = GetClassOf(ClassName);
        }

        public virtual Type GetClassOf(string className) {
            return null;
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("id", ActionID);
            SetSetting("name", ActionName);
            SetSetting("classname", ClassName);
        }

        public object CreateInstance() 
        {
            if (EasyFieldType == null) return null;
            object instance = Activator.CreateInstance(EasyFieldType);            
            ((IEasyFieldInterface)instance).LoadFieldSettings(Fields);
            return instance;
        }

    }
}
