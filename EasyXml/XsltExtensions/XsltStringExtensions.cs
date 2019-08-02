using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyXml.XsltExtensions
{
    public class XsltStringExtensions
    {
        public const string Namespace = "http://EasyXsltExtensions/1.0";
        public string Upper(string inputStr)
        {
            return inputStr.ToUpper();
        }

        public string Lower(string inputStr)
        {
            return inputStr.ToLower();
        }

        public string Proper(string inputStr)
        {
            string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(inputStr.ToLower());
            return result;
        }

        public int Length(string inputStr)
        {
            return inputStr.Length;
        }

        public string Left(string inputStr, int numChars)
        {
            return inputStr.Substring(0, numChars);
        }

        public string Right(string inputStr, int numChars)
        {
            return inputStr.Substring(inputStr.Length - numChars);
        }

        public bool Contains(string inputStr, string searchStr)
        {
            return (inputStr.IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) >=0);
        }

        public bool IsEmpty(string inputStr)
        {
            return String.IsNullOrWhiteSpace(inputStr);
        }

        public bool IsNumber(string inputStr)
        {
            inputStr = Trim(inputStr);
            bool result = !Regex.IsMatch(inputStr, "([^\\d,.]+)");
            return result;
        }

        public bool IsDate(string inputStr)
        {
            DateTime dateTime;
            return DateTime.TryParse(inputStr, out dateTime);
        }

        public string Trim(string inputStr)
        {
            return inputStr.Trim();
        }

        public string Replace(string inputStr, string searchStr, string replaceStr)
        {
            return inputStr.Replace(searchStr, replaceStr);
        }

    }
}
