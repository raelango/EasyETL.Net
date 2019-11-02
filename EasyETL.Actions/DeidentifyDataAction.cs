using EasyETL.Utils;
using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyETL.Actions
{
    [DisplayName("Deidentify Data")]
    [Description("Deidentify data from columns")]
    [EasyField("DataFields", "The name of the column(s) to be deidentified.  Separate multiple columns with ';'")]
    public class DeidentifyDataAction : AbstractEasyAction
    {
        public string DataFields;

        public override bool IsFieldSettingsComplete()
        {
            LoadSettings();
            return !String.IsNullOrWhiteSpace(DataFields) ;
        }

        private void LoadSettings()
        {
            DataFields = (SettingsDictionary.ContainsKey("DataFields") ? SettingsDictionary["DataFields"] : "");
        }

        public override bool CanExecute(Dictionary<string, string> dataDictionary)
        {
            if (!IsFieldSettingsComplete()) return false;
            foreach (string argument in DataFields.Split(';'))
            {
                if (!String.IsNullOrWhiteSpace(argument))
                {
                    string fieldName = argument.Trim('[', ']');
                    if (!dataDictionary.ContainsKey(fieldName)) return false;
                }
            }
            return true;
        }

        public override void Execute(Dictionary<string, string> dataDictionary)
        {
            if (CanExecute(dataDictionary))
            {
                foreach (string dataField in DataFields.Split(';'))
                {
                    string fieldName = dataField.Trim('[', ']');
                    dataDictionary[fieldName] = DeidentifyData(dataDictionary[fieldName]);
                }
            }
        }

        public override void Execute(DataRow dataRow)
        {
            Dictionary<string, string> dataDictionary = GetPropertiesFromDataRow(dataRow);
            if (!CanExecute(dataDictionary)) return;
            Execute(dataDictionary);
            foreach (string dataField in DataFields.Split(';'))
            {
                dataRow[dataField] = dataDictionary[dataField];
            }
        }

        private string DeidentifyData(string fullData)
        {
            return fullData.DeidentifyData();
        }
    }
}
