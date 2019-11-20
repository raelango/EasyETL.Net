using EasyETL.Attributes;
using EasyETL.Xml.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net;
using MailKit;
using MailKit.Security;
using System.Xml;
using MailKit.Search;
using MimeKit;

namespace EasyETL.Xml.EMail
{
    [DisplayName("IMAP EMail Datasource")]
    [EasyField("MailHost", "Server IP or Name")]
    [EasyField("UserName", "User Name to be used to connect to the email host")]
    [EasyField("UserPassword", "Password to be used to login to the mail box", "", "", "", true)]
    [EasyField("MailboxName", "Name of the Mailbox or Folder", "Inbox")]
    [EasyField("Encryption", "What level of encryption to be used", "Auto", "", "Auto;None;SslOnConnect;StartTls;StartTlsWhenAvailable")]
    [EasyField("Query", "Search Query to execute on the folder")]
    [EasyField("IncludeSubFolders", "Should include the items in subfolders", "False", "", "True;False")]
    public class EMailDatasourceEasyParser : DatasourceEasyParser, IDisposable
    {
        public string MailHost;
        public string UserName;
        public string UserPassword;
        public string ClientType = "IMAP";
        public string MailboxName = "Inbox";
        public string Query;
        public bool IncludeSubfolders = false;
        public SecureSocketOptions Encryption = SecureSocketOptions.Auto;
        public MailStore mailStore = null;

        public EMailDatasourceEasyParser() { }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "mailhost":
                    MailHost = fieldValue; break;
                case "username":
                    UserName = fieldValue; break;
                case "userpassword":
                    UserPassword = fieldValue; break;
                case "mailboxname":
                    MailboxName = fieldValue; break;
                case "clienttype":
                    ClientType = fieldValue; break;
                case "encryption":
                    Encryption = (SecureSocketOptions)Enum.Parse(typeof(SecureSocketOptions), fieldValue); break;
                case "query":
                    Query = fieldValue; break;
                case "includesubfolders":
                    IncludeSubfolders = Convert.ToBoolean(fieldValue); break;
            }
        }

        public override bool IsFieldSettingsComplete()
        {
            return base.IsFieldSettingsComplete() && !String.IsNullOrWhiteSpace(MailHost) && !String.IsNullOrWhiteSpace(UserName) && !String.IsNullOrWhiteSpace(UserPassword) && !String.IsNullOrWhiteSpace(MailboxName);
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            ConnectToEMailServer();
            return (mailStore != null) && (mailStore.IsAuthenticated);
        }


        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("mailhost", MailHost);
            resultDict.Add("username", UserName);
            resultDict.Add("userpassword", UserPassword);
            resultDict.Add("mailboxname", MailboxName);
            resultDict.Add("clienttype", ClientType);
            resultDict.Add("encryption", Encryption.ToString());
            resultDict.Add("query", Query);
            resultDict.Add("includesubfolders", IncludeSubfolders.ToString());
            return resultDict;
        }

        public override XmlDocument Load(System.IO.TextReader txtReader, XmlDocument xDoc = null)
        {
            return LoadStr(txtReader.ReadToEnd(), xDoc);
        }

        public override XmlDocument Load(string filename, XmlDocument xDoc = null)
        {
            return LoadStr(filename);
        }

        public override XmlDocument LoadStr(string strToLoad, XmlDocument xDoc = null)
        {
            if (!String.IsNullOrWhiteSpace(strToLoad)) Query = strToLoad;
            ConnectToEMailServer();
            if (mailStore == null) return null;

            #region setup the rootNode
            XmlNode rootNode;
            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }
            if (xDoc.DocumentElement == null)
            {
                rootNode = xDoc.CreateElement(RootNodeName);
                xDoc.AppendChild(rootNode);
            }

            rootNode = xDoc.DocumentElement;
            #endregion


            SearchQuery searchQuery = SearchQuery.All;
            if (!String.IsNullOrWhiteSpace(Query)) searchQuery = SearchQuery.MessageContains(Query) ;
            IMailFolder mailFolder = mailStore.GetFolder(MailboxName);
            mailFolder.Open(FolderAccess.ReadOnly);
            IList<UniqueId> mailIDs = mailFolder.Search(searchQuery);

            int lineNumber = 0;
            int rowCount = 0;

            if (mailIDs.Count > MaxRecords) mailIDs = mailIDs.Take((int)MaxRecords).ToList();

            foreach (UniqueId mailID in mailIDs)
            {
                HeaderList messageHeader = mailFolder.GetHeaders(mailID);
                lineNumber++;
                UpdateProgress(lineNumber);

                XmlElement rowNode = xDoc.CreateElement(RowNodeName);
                AddXmlNodeChild(rowNode, "UniqueID", mailID.Id.ToString());
                foreach (Header msgHeader in messageHeader)
                {
                    AddXmlNodeChild(rowNode, msgHeader.Field, msgHeader.Value);
                }

                AddRow(xDoc, rowNode);
                if ((rowNode != null) && (rowNode.HasChildNodes))
                {
                    rootNode.AppendChild(rowNode);
                    rowCount++;
                }
                if (rowCount >= MaxRecords) break;
            }

            UpdateProgress(lineNumber, true);

            return xDoc;
        }

        private void AddXmlNodeChild(XmlElement parentNode, string nodeName, string nodeValue)
        {

            XmlElement childNode = parentNode.OwnerDocument.CreateElement(nodeName);
            childNode.InnerText = nodeValue;
            parentNode.AppendChild(childNode);
        }

        private void ConnectToEMailServer()
        {
            if ((mailStore !=null) && (mailStore.IsAuthenticated)) return;
            if (mailStore == null) {
                switch (ClientType.ToLower()) {
                    case "imap":
                        mailStore = new MailKit.Net.Imap.ImapClient(); break;
                }
                if (mailStore !=null) mailStore.Connect(MailHost,0,Encryption);
            }
            if (mailStore == null) return;
            if (mailStore.IsConnected) {
                mailStore.Authenticate(UserName,UserPassword);
            }
        }

        private void DisconnectFromEmailServer()
        {
            if (mailStore == null) return;
            if (mailStore.IsConnected) mailStore.Disconnect(true);
            mailStore = null;
        }


        public void Dispose()
        {
            if (mailStore != null) mailStore.Dispose();
            DisconnectFromEmailServer();
        }
    }
}
