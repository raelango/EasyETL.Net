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

namespace EasyDemoActions
{
    [DisplayName("Set Field Status")]
    [Description("This sets the status based on the variable code")]
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

        public override void Execute(System.Xml.XmlNode dataNode)
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
