using EasyETL;
using EasyETL.Actions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ComponentModel;
using System.Xml;
using EasyETL.Attributes;

namespace EasyDemoActions
{
    [DisplayName("Set Field Value")]
    [Description("This sets the value of fields mentioned in <<FieldName>> to values specified in <<FieldValue>>")]
    [EasyField("FieldName", "This is the name of the column. use ';' to separate multiple columns")]
    [EasyField("FieldValue","This is the value of the column.  use ';' to separate multiple columnns.  Please note that the columns in FieldName should match the columns in FieldValue")]
    public class SetFieldValueAction : AbstractEasyAction
    {
        public override void Execute(Dictionary<string, string> dataDictionary)
        {
            string[] fieldNames = SettingsDictionary["FieldName"].Split(';');
            string[] fieldValues = SettingsDictionary["FieldValue"].Split(';');
            if (fieldNames.Length != fieldValues.Length) return;
            for (int fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
            {
                dataDictionary[fieldNames[fieldIndex]] = fieldValues[fieldIndex];
            }
        }

        public override void Execute(EasyDynamicObject dataObject)
        {
            string[] fieldNames = SettingsDictionary["FieldName"].Split(';');
            string[] fieldValues = SettingsDictionary["FieldValue"].Split(';');
            if (fieldNames.Length != fieldValues.Length) return;
            for (int fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
            {
                dataObject.Properties[fieldNames[fieldIndex]] = fieldValues[fieldIndex];
            }
        }

        public override void Execute(DataRow dataRow)
        {
            string[] fieldNames = SettingsDictionary["FieldName"].Split(';');
            string[] fieldValues = SettingsDictionary["FieldValue"].Split(';');
            if (fieldNames.Length != fieldValues.Length) return;
            for (int fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
            {
                dataRow[fieldNames[fieldIndex]] = fieldValues[fieldIndex];
            }
        }

        public override void Execute(XmlNode dataNode)
        {
            string[] fieldNames = SettingsDictionary["FieldName"].Split(';');
            string[] fieldValues = SettingsDictionary["FieldValue"].Split(';');
            if (fieldNames.Length != fieldValues.Length) return;
            for (int fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
            {
                if (dataNode.SelectSingleNode(fieldNames[fieldIndex]) != null)
                {
                    dataNode.SelectSingleNode(fieldNames[fieldIndex]).InnerText = fieldValues[fieldIndex];
                }
            }
        }

    }
}
