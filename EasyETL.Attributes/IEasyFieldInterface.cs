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
        bool CanFunction();
    }

    public static class EasyFieldInterfaceExtensions
    {
        public static void SaveFieldSettingsToXmlNode(this XmlNode parentNode, Dictionary<string, string> SettingsDictionary)
        {
            foreach (KeyValuePair<string, string> kvPair in SettingsDictionary)
            {
                XmlElement xNode = parentNode.OwnerDocument.CreateElement("field");
                xNode.SetAttribute("name", kvPair.Key);
                if (kvPair.Value.Contains(Environment.NewLine))
                    xNode.InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes(kvPair.Value));
                else
                    xNode.SetAttribute("value", kvPair.Value);
                parentNode.AppendChild(xNode);
            }
        }

        public static Dictionary<string, string> LoadFieldSettings(this Type classType, XmlNode parentNode) 
        {
            Dictionary<string, string> resultDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            Dictionary<string, EasyFieldAttribute> easyFields = classType.GetEasyFields();
            foreach (KeyValuePair<string,EasyFieldAttribute> kvEFA in easyFields)
            {
                XmlNode efaNode = parentNode.SelectSingleNode("field[translate(@name,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')='" + kvEFA.Key.ToUpper() +"']");
                if (efaNode == null)
                    resultDictionary.Add(kvEFA.Key, kvEFA.Value.DefaultValue);
                else
                {
                    if (efaNode.Attributes.GetNamedItem("value") == null)
                        resultDictionary.Add(kvEFA.Key, Encoding.UTF8.GetString(Convert.FromBase64String(efaNode.InnerText)));
                    else
                        resultDictionary.Add(kvEFA.Key, efaNode.Attributes.GetNamedItem("value").Value);
                }
            }
            return resultDictionary;
        }

        public static Dictionary<string,string> EncryptPasswords(this Dictionary<string,string> sourceDictionary, Dictionary<string,EasyFieldAttribute> easyFieldDictionary)
        {
            Dictionary<string, string> resultDictionary = new Dictionary<string, string>(sourceDictionary);
            foreach (KeyValuePair<string,EasyFieldAttribute> easyFieldPair in easyFieldDictionary)
            {
                if (easyFieldPair.Value.IsPassword && resultDictionary.ContainsKey(easyFieldPair.Key))
                {
                    resultDictionary[easyFieldPair.Key] = resultDictionary[easyFieldPair.Key].Encrypt256();
                }
            }
            return resultDictionary;
        }


        public static Dictionary<string, string> DecryptPasswords(this Dictionary<string, string> sourceDictionary, Dictionary<string, EasyFieldAttribute> easyFieldDictionary)
        {
            Dictionary<string, string> resultDictionary = new Dictionary<string, string>(sourceDictionary);
            foreach (KeyValuePair<string, EasyFieldAttribute> easyFieldPair in easyFieldDictionary)
            {
                if (easyFieldPair.Value.IsPassword && resultDictionary.ContainsKey(easyFieldPair.Key))
                {
                    resultDictionary[easyFieldPair.Key] = resultDictionary[easyFieldPair.Key].Decrypt256();
                }
            }
            return resultDictionary;
        }

    }
}
