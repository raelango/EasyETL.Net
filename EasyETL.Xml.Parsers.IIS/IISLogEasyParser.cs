using EasyETL.Attributes;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        public IISLogEasyParser()
        {
            ServerName = "";
            SiteNames = new List<string>();
            StartDate = null;
            EndDate = null;
        }


        public IISLogEasyParser(string serverName, params string[] siteNames)
        {
            ServerName = serverName;
            SiteNames = new List<string>();
            if (siteNames.Length > 0)
            {
                foreach (string siteName in siteNames)
                    SiteNames.Add(siteName);
            }
            else
            {
                using (ServerManager serverManager = String.IsNullOrWhiteSpace(serverName) ? new ServerManager() : ServerManager.OpenRemote(serverName))
                {
                    foreach (Site site in serverManager.Sites)
                        SiteNames.Add(site.Name);
                }
            }
        }

        public override XmlDocument Load(XmlDocument xDoc, params string[] filenames)
        {

            if (filenames.Length == 0)
            {
                using (ServerManager serverManager = String.IsNullOrWhiteSpace(ServerName) ? new ServerManager() : ServerManager.OpenRemote(ServerName))
                {
                    List<string> logFiles = new List<string>();
                    foreach (string siteName in SiteNames)
                    {
                        Site site = serverManager.Sites[siteName];
                        if (site != null)
                        {
                            SiteLogFile siteLogFile = site.LogFile;
                            foreach (string logFileName in Directory.EnumerateFiles(siteLogFile.Directory, "*.log", SearchOption.TopDirectoryOnly))
                            {
                                if ((StartDate.HasValue) && (StartDate.Value <= File.GetCreationTime(logFileName)) &&
                                    (EndDate.HasValue) && (EndDate.Value >= File.GetLastWriteTime(logFileName)))
                                {
                                    logFiles.Add(logFileName);
                                }
                            }
                        }
                    }
                }
            }
            return (filenames.Length > 0) ? base.Load(xDoc, filenames) : xDoc;
        }

        public override XmlDocument Load(string filename, XmlDocument xDoc = null)
        {
            if (String.IsNullOrWhiteSpace(filename)) filename = ServerName;
            if ((!File.Exists(filename)) && (SiteNames.Count >0))
            {
                Load(xDoc);
            }
            return base.Load(filename, xDoc);
        }
    }
}
