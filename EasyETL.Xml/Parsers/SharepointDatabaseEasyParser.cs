using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("Sharepoint Documents")]
    [EasyField("DocumentsURL", "The full URL to access the sharepoint documents")]
    [EasyField("ListID", "This is usually a guid","","([0-9A-Za-z]{8})-([0-9A-Za-z]{4})-([0-9A-Za-z]{4})-([0-9A-Za-z]{4})-([0-9A-Za-z]{12})","")]
    [EasyField("Query", "Query to Execute")]
    public class SharepointDatabaseEasyParser : DatabaseEasyParser
    {
        public string DocumentsSiteURL = String.Empty;
        public string ListID = String.Empty;
        public SharepointDatabaseEasyParser() : base(EasyDatabaseConnectionType.edctOledb) { }
        public SharepointDatabaseEasyParser(string connString) : base(EasyDatabaseConnectionType.edctOledb, connString) { }

        public override bool IsFieldSettingsComplete()
        {
            ConnectionString = String.Empty;
            if ((!String.IsNullOrWhiteSpace(DocumentsSiteURL)) && (!String.IsNullOrWhiteSpace(ListID)))
            {
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;WSS;IMEX=1;RetrieveIds=Yes;DATABASE=" + DocumentsSiteURL + ";LIST={" + ListID + "};";
            }
            return base.IsFieldSettingsComplete();
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "documentsurl":
                    DocumentsSiteURL = fieldValue; break;
                case "listid":
                    ListID = fieldValue; break;
            }
        }


        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("documentsurl", DocumentsSiteURL);
            resultDict.Add("listid", ListID);
            return resultDict;
        }

    }
}
