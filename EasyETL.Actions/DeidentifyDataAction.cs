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
            string resultData = fullData;
            //Please see https://www.hhs.gov/hipaa/for-professionals/privacy/special-topics/de-identification/index.html#standard for de-identification of data...
            //This function uses "Safe Harbor" method.
            if (MatchesPattern(fullData, @"[A-Za-z]+(\s+[A-Za-z]+)+", @"[A-Za-z]+"))
            {
                //This is a name.. let us deidenfity...
                if (resultData.Length >3 ) resultData = resultData.Substring(0, 3) + new String('*', resultData.Length - 3);
                return resultData;
            }
            if (MatchesPattern(fullData,@"\d{5}")) {
                //this is a zip code....
                return resultData.Substring(0,3) + "**";
            }

            if (MatchesPattern(fullData,@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}")) {
                //This is a telephone number or fax number
                return fullData.Substring(0, fullData.Length - 4) + "****";
            }

            if (MatchesPattern(fullData, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)"))
            {
                //This is a email address
                resultData = "";
                foreach (char thisChar in fullData) 
                {
                    resultData += ".@-_".Contains(thisChar) ? thisChar : '*';
                }
                return resultData;
            }

            if (MatchesPattern(fullData, @"\d{3}-\d{2}-\d{4}"))
            {
                //This is a social security number
                return resultData.Substring(0, 4) + "**-****";
            }

            if (MatchesPattern(fullData, @"\d{1,3}.?\d{0,3}\s[a-zA-Z]{2,30}\s[a-zA-Z]{2,15}"))
            {
                resultData = "";
                //This is a US street address
                foreach (string partData in fullData.Split(' '))
                {
                    if (!String.IsNullOrWhiteSpace(resultData)) resultData += " ";
                    if (partData.Length > 2)
                        resultData += partData.Substring(0, 2) + new String('*', partData.Length - 2);
                    else
                        resultData += partData;
                }
                return resultData;
            }

            CreditCardValidator ccValidator = new CreditCardValidator(fullData);
            if (ccValidator.IsValid)
            {
                int digitsMasked = 0;
                resultData = "";
                foreach (char c in fullData.Reverse())
                {
                    if (Char.IsDigit(c) && (digitsMasked < 8))
                    {
                        resultData = "*" + resultData;
                        digitsMasked++;
                    }
                    else
                        resultData = c + resultData;
                }
                return resultData;
            }

            DateTime dateOutput;
            if (DateTime.TryParse(fullData, out dateOutput))
            {
                //This is a valid date.. we should mask everything except year...
                string yearPart = String.Empty;
                foreach (string part in fullData.Split('-', ' ', '/'))
                {
                    short intPart;
                    if (Int16.TryParse(part,out intPart)) {
                        //this is an integer and hence, could be an year..
                        if (part.Length > 2) {
                            yearPart = part;
                        }
                        else
                        {
                            if (intPart == dateOutput.Year)
                            {
                                yearPart = part;
                            }
                        }
                        if (!String.IsNullOrWhiteSpace(yearPart)) break;
                    }
                }

                if (yearPart.Length > 0)
                {
                    int yearPartIndex = fullData.IndexOf(yearPart);

                    resultData = "";
                    for (int charIndex =0; charIndex < fullData.Length; charIndex ++) {
                        resultData += (((charIndex < yearPartIndex) || (charIndex > (yearPartIndex + yearPart.Length))) && (Char.IsDigit(fullData[charIndex]))) ? '*' : fullData[charIndex];
                    }
                    return resultData;
                }
            }


            IPAddress ip;
            if (IPAddress.TryParse(fullData,out ip))
            {
                //This is a valid IP Address
                return Regex.Replace(fullData,"[0-9]","*");
            }

            Uri uriResult;
            if (Uri.TryCreate(fullData, UriKind.Absolute, out uriResult))
            {
                //This is a valid URL
                resultData = fullData.Substring(0, fullData.IndexOf('.'));
                resultData += new string('*', fullData.Length - resultData.Length);
                return resultData;
            }


            return resultData;
        }

        private bool MatchesPattern(string input, params string[] patterns)
        {
            foreach (string pattern in patterns)
            {
                if (Regex.IsMatch(input, "^" + pattern + "$")) return true;
            }
            return false;
        }


    }
}
