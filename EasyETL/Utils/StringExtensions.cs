using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyETL.Utils
{
    public static class StringExtensions
    {

        public static string DeidentifyData(this string inputStr, string inputType = "detect")
        {
            string resultData = inputStr;
            //Please see https://www.hhs.gov/hipaa/for-professionals/privacy/special-topics/de-identification/index.html#standard for de-identification of data...
            //This function uses "Safe Harbor" method.
            inputType = inputType.ToLower();
            if ((inputType == "detect") && (MatchesPattern(inputStr, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"))) inputType = "phone";
            if ((inputType == "detect") && (MatchesPattern(inputStr, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)"))) inputType = "email";
            if ((inputType == "detect") && (MatchesPattern(inputStr, @"^\d{5}(?:[-\s]\d{4})?$"))) inputType = "zipcode";
            if ((inputType == "detect") && (MatchesPattern(inputStr, @"\d{1,3}.?\d{0,3}\s[a-zA-Z]{2,30}\s[a-zA-Z]{2,15}"))) inputType = "address";
            if ((inputType == "detect") && (MatchesPattern(inputStr, @"\d{3}-\d{2}-\d{4}"))) inputType = "ssn";
            if ((inputType == "detect") && (IPAddress.TryParse(inputStr, out IPAddress ipAddress))) inputType = "ipaddress";

            if ((inputType == "detect") && (DateTime.TryParse(inputStr, out DateTime dateTime))) inputType = "date";
            if ((inputType == "detect") && (new CreditCardValidator(inputStr).IsValid)) inputType = "creditcard";
            if ((inputType == "detect") && (MatchesPattern(inputStr, @"[A-Za-z]+(\s+[A-Za-z]+)+", @"[A-Za-z]+"))) inputType = "name";

            switch (inputType)
            {
                case "name":
                    if (MatchesPattern(inputStr, @"[A-Za-z]+(\s+[A-Za-z]+)+", @"[A-Za-z]+"))
                    {
                        //This is a name.. let us deidenfity...
                        if (resultData.Length > 3) resultData = resultData.Substring(0, 3) + new String('*', resultData.Length - 3);
                        return resultData;
                    }
                    break;
                case "phone":
                    if (MatchesPattern(inputStr, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}")) return inputStr.Substring(0, inputStr.Length - 4) + "****";
                    break;
                case "email":
                    if (MatchesPattern(inputStr, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)"))
                    {
                        //This is a email address
                        resultData = "";
                        foreach (char thisChar in inputStr)
                        {
                            resultData += ".@-_".Contains(thisChar) ? thisChar : '*';
                        }
                        return resultData;
                    }
                    break;
                case "zip":
                case "zipcode":
                    if (MatchesPattern(inputStr, @"^\d{5}(?:[-\s]\d{4})?$")) return resultData.Substring(0, 3) + "**";
                    break;
                case "address":
                    if (MatchesPattern(inputStr, @"\d{1,3}.?\d{0,3}\s[a-zA-Z]{2,30}\s[a-zA-Z]{2,15}"))
                    {
                        resultData = "";
                        //This is a US street address
                        foreach (string partData in inputStr.Split(' '))
                        {
                            if (!String.IsNullOrWhiteSpace(resultData)) resultData += " ";
                            if (partData.Length > 2)
                                resultData += partData.Substring(0, 2) + new String('*', partData.Length - 2);
                            else
                                resultData += partData;
                        }
                        return resultData;
                    }
                    break;
                case "ssn":
                    if (MatchesPattern(inputStr, @"\d{3}-\d{2}-\d{4}")) return resultData.Substring(0, 4) + "**-****";
                    break;
                case "creditcard":
                    if (new CreditCardValidator(inputStr).IsValid)
                    {
                        int digitsMasked = 0;
                        resultData = "";
                        foreach (char c in inputStr.Reverse())
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
                    break;
                case "date":
                case "datetime":
                    if (DateTime.TryParse(inputStr, out DateTime dateOutput))
                    {
                        //This is a valid date.. we should mask everything except year...
                        string yearPart = String.Empty;
                        foreach (string part in inputStr.Split('-', ' ', '/'))
                        {
                            short intPart;
                            if (Int16.TryParse(part, out intPart))
                            {
                                //this is an integer and hence, could be an year..
                                if (part.Length > 2)
                                {
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
                            int yearPartIndex = inputStr.IndexOf(yearPart);

                            resultData = "";
                            for (int charIndex = 0; charIndex < inputStr.Length; charIndex++)
                            {
                                resultData += (((charIndex < yearPartIndex) || (charIndex > (yearPartIndex + yearPart.Length))) && (Char.IsDigit(inputStr[charIndex]))) ? '1' : inputStr[charIndex];
                            }
                            return resultData;
                        }
                    }
                    break;
                case "ipaddress":
                    if (IPAddress.TryParse(inputStr, out IPAddress ip)) return Regex.Replace(inputStr, "[0-9]", "*");
                    break;
                case "uri":
                case "url":
                    if (Uri.TryCreate(inputStr, UriKind.Absolute, out Uri uriResult))
                    {
                        //This is a valid URL
                        resultData = inputStr.Substring(0, inputStr.IndexOf('.'));
                        resultData += new string('*', inputStr.Length - resultData.Length);
                        return resultData;
                    }
                    break;
                case "detect":
                    break;
            }


            return resultData;
        }

        public static bool MatchesPattern(this string input, params string[] patterns)
        {
            foreach (string pattern in patterns)
            {
                if (Regex.IsMatch(input, "^" + pattern + "$")) return true;
            }
            return false;
        }

    }
}
