using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using EasyETL.Utils;

namespace EasyETL.Xml.XsltExtensions
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
            return (inputStr.IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) >= 0);
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
            return DateTime.TryParse(inputStr, out _);
        }

        public string Trim(string inputStr)
        {
            return inputStr.Trim();
        }

        public string Replace(string inputStr, string searchStr, string replaceStr)
        {
            return inputStr.Replace(searchStr, replaceStr);
        }

        public string Deidentify(string inputStr, string inputType)
        {
            if (String.IsNullOrWhiteSpace(inputType)) inputType = "detect";
            return inputStr.DeidentifyData(inputType);
        }

        public string Today(string format)
        {
            return DateTime.Today.ToString(format);
        }

        public string Now(string format)
        {
            return DateTime.Now.ToString(format);
        }

        public string FormatDate(string date, string format)
        {
            if (IsDate(date)) return DateTime.Parse(date).ToString(format);
            return "";
        }

        public string ParseDate(string date, string format, string newformat)
        {
            DateTime result;
            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) return result.ToString(newformat);
            return "";
        }

        public string GeoCodingData(string address, string xpath)
        {
            //checkout https://developers.google.com/maps/documentation/geocoding/intro?hl=en_US
            try
            {
                xpath = "GeocodeResponse/result/geometry/" + xpath;
                string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}", Uri.EscapeDataString(address), "AIzaSyBU6LR7AsTe5cRyyNbKDL7trA65XHFSfnQ");
                string xmlContent = String.Empty;
                using (WebResponse wr = WebRequest.Create(requestUri).GetResponse())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        xmlContent = sr.ReadToEnd();
                    }
                }
                if (!String.IsNullOrWhiteSpace(xmlContent))
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlContent);
                    if (xDoc.SelectSingleNode(xpath) != null) return xDoc.SelectSingleNode(xpath).InnerXml;
                }
            }
            catch
            {
            }
            return "";
        }

    }
}
