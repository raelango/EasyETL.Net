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
            base.ReadSettings(xNode);
            Fields = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (EasyFieldType == null)
            {
                XmlNodeList fieldNodes = xNode.SelectNodes("field");
                foreach (XmlNode fieldNode in fieldNodes)
                {
                    string AttributeName = (fieldNode.Attributes.GetNamedItem("name") == null) ? String.Empty : fieldNode.Attributes.GetNamedItem("name").Value;
                    string AttributeValue = (fieldNode.Attributes.GetNamedItem("value") == null) ? fieldNode.InnerText : fieldNode.Attributes.GetNamedItem("value").Value;
                    switch (AttributeName.ToLower())
                    {
                        case "id":
                            ActionID = AttributeValue; break;
                        case "name":
                            ActionName = AttributeValue; break;
                        case "classname":
                            ClassName = AttributeValue; break;
                    }
                    if (String.IsNullOrWhiteSpace(ClassName))
                        EasyFieldType = null;
                    else
                        EasyFieldType = GetClassOf(ClassName);
                }
            }
            if (EasyFieldType != null) Fields = EasyFieldType.LoadFieldSettings(xNode);
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
            if (Fields == null) return;
            foreach (KeyValuePair<string, string> kvPair in Fields)
            {
                XmlElement fieldNode = xNode.OwnerDocument.CreateElement("field");
                fieldNode.SetAttribute("name", kvPair.Key);
                string keyValue = kvPair.Value;
                if (keyValue == null) keyValue = "";
                if (keyValue.Contains(Environment.NewLine))
                    fieldNode.InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes(keyValue));
                else
                    fieldNode.SetAttribute("value", keyValue);
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
            ReadSettingsFromDictionary();
            if (EasyFieldType == null) return null;
            object instance = Activator.CreateInstance(EasyFieldType);            
            ((IEasyFieldInterface)instance).LoadFieldSettings(Fields);
            return instance;
        }

    }
}
