using EasyETL.Attributes;
using EasyETL.Xml.Parsers;
using QBXMLRP2Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Endpoint.Quickbooks
{
    [DisplayName("Quickbooks Database")]
    [EasyField("ApplicationID", "ID of the calling Application")]
    [EasyField("ApplicationName", "Name of the calling Application")]
    [EasyField("QuickbookFileName", "Full Path of the quickbooks database (*.QBW)")]
    [EasyField("ItemType", "Type of the Quickbooks Item that we are querying","Company","","Company;Account;Deposit;Bill;Barcode;Item;Inventory;Invoice;InvoicePayment;BillPayment;Customer;Employee")]
    public class QuickbooksDatabaseEasyParser : DatabaseEasyParser
    {
        public string ApplicationID;
        public string ApplicationName;
        public string QuickbookFileName;
        public QuickbooksItemType ItemType;
        public RequestProcessor2 QBProcessor = null;
        public string SessionID = String.Empty;
        public QuickbooksDatabaseEasyParser() : base(EasyDatabaseConnectionType.edctQuickbooks) { }

        public override bool IsFieldSettingsComplete()
        {
            if ((String.IsNullOrWhiteSpace(ApplicationID)) && (String.IsNullOrWhiteSpace(ApplicationName))) return false;
            return ((!String.IsNullOrWhiteSpace(QuickbookFileName)) && (File.Exists(QuickbookFileName)));
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            resultDict.Add("applicationid", ApplicationID);
            resultDict.Add("applicationname", ApplicationName);
            resultDict.Add("quickbookfilename", QuickbookFileName);
            resultDict.Add("itemtype", ItemType.ToString());
            return resultDict;
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "applicationid":
                    ApplicationID = fieldValue;break;
                case "applicationname":
                    ApplicationName = fieldValue; break;
                case "quickbookfilename":
                    QuickbookFileName = fieldValue; break;
                case "itemtype":
                    ItemType = (QuickbooksItemType)Enum.Parse(typeof(QuickbooksItemType),fieldValue); break;
            }
        }

        public override XmlDocument Load(string queryToExecute = "", XmlDocument xDoc = null)
        {
            ConnectToQuickBooks();
            if ((QBProcessor == null) || String.IsNullOrWhiteSpace(SessionID)) return null;
            string resultXML = QBProcessor.ProcessRequest(SessionID, QBInputRequests.GetAllItems(ItemType));
            if (xDoc == null) xDoc = new XmlDocument();
            xDoc.LoadXml(resultXML);
            return xDoc;
        }

        private void ConnectToQuickBooks() {
            if ((QBProcessor !=null) && (!String.IsNullOrWhiteSpace(SessionID)))  return;

            if (QBProcessor == null)
            {
                SessionID = String.Empty;
                //QBProcessor is null.. 
                if ((File.Exists(QuickbookFileName)) && (!String.IsNullOrWhiteSpace(ApplicationName)))
                {
                    QBProcessor = new RequestProcessor2();
                    QBProcessor.OpenConnection(ApplicationID, ApplicationName);
                }
            }

            if (QBProcessor !=null) {
                //Session ID is Null or empty .. let us establish the session
                SessionID = QBProcessor.BeginSession(QuickbookFileName, QBFileMode.qbFileOpenMultiUser);
                return;
            }
        }
    }
}
