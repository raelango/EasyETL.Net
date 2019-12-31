using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("IIS Log")]
    [EasyField("ServerName", "Name of the IIS Log Server")]
    [EasyField("SiteName", "Name of the Website.  Leave empty to load all sites")]
    [EasyField("StartDate", "Start date to filter records.  Leave Empty to filter without start date")]
    [EasyField("EndDate", "End date to filter records.  Leave Empty to filter without end date")]
    [EasyField("TableName", "Name of the table", "log")]
    public class IISLogEasyParser : FixedWidthEasyParser
    {
        public string ServerName;
        public List<String> SiteNames;
        public DateTime? StartDate;
        public DateTime? EndDate;
        public IISLogEasyParser(string serverName, params string[] siteNames)
        {
            ServerName = serverName;
            SiteNames = new List<string>();
            foreach (string siteName in siteNames)
            {
                SiteNames.Add(siteName);
            }
        }

        public override XmlDocument Load(XmlDocument xDoc, params string[] filenames)
        {

            if (filenames.Length == 0)
            {

                filenames = SiteNames.ToArray();
            }
            return base.Load(xDoc, filenames);
        }
    }
}
