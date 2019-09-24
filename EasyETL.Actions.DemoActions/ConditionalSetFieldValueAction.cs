using EasyETL;
using EasyETL.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace EasyDemoActions
{
    [DisplayName("Conditional Set Field Value")]
    [Description("This sets the value of field mentioned in <<FieldName>> to value specified in <<NewFieldValue>> if the current value matches <<CurrentFieldValue>>")]
    [EasyField("FieldName", "This is the name of the column.")]
    [EasyField("CurrentFieldValue", "This is the value expected to be in the <<FieldName>> column.  Supports Regex match and ignores case")]
    [EasyField("NewFieldValue", "This is the new value to be set in the <<FieldName>> column.")]
    public class ConditionalSetFieldValueAction : AbstractEasyAction
    {
        public override bool CanExecute(DataRow dataRow)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            return CanExecute(dataRow[fieldName].ToString(), currentFieldValue);
        }

        private bool CanExecute(string input, string pattern)
        {
            if ((!pattern.StartsWith("^")) && (!pattern.EndsWith("$"))) {
                pattern = "^(" + pattern + ")$";
            }
            return Regex.IsMatch(input,pattern, RegexOptions.IgnoreCase);
        }

        public override bool CanExecute(Dictionary<string, string> dataDictionary)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            return CanExecute(dataDictionary[fieldName], currentFieldValue);
        }

        public override bool CanExecute(EasyDynamicObject dataObject)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            return CanExecute(dataObject.Properties[fieldName].ToString(), currentFieldValue);
        }

        public override bool CanExecute(XmlNode dataNode)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            return CanExecute(dataNode.SelectSingleNode(fieldName).InnerText, currentFieldValue);
        }


        public override void Execute(Dictionary<string, string> dataDictionary)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            string newFieldValue = SettingsDictionary["NewFieldValue"];
            if (CanExecute(dataDictionary[fieldName], currentFieldValue))
                dataDictionary[fieldName] = newFieldValue;
        }

        public override void Execute(EasyDynamicObject dataObject)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            string newFieldValue = SettingsDictionary["NewFieldValue"];
            if (CanExecute(dataObject.Properties[fieldName].ToString(), currentFieldValue))
                dataObject.Properties[fieldName] = newFieldValue;
        }

        public override void Execute(DataRow dataRow)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            string newFieldValue = SettingsDictionary["NewFieldValue"];
            if (CanExecute(dataRow[fieldName].ToString(), currentFieldValue)) dataRow[fieldName] = newFieldValue;
        }

        public override void Execute(XmlNode dataNode)
        {
            string fieldName = SettingsDictionary["FieldName"];
            string currentFieldValue = SettingsDictionary["CurrentFieldValue"];
            string newFieldValue = SettingsDictionary["NewFieldValue"];
            if (CanExecute(dataNode.SelectSingleNode(fieldName).InnerText, currentFieldValue)) dataNode.SelectSingleNode(fieldName).InnerText = newFieldValue;
        }
    }
}
