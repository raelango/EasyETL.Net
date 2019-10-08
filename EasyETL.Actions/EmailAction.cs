using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyETL.Actions
{
    [DisplayName("Send EMail")]
    [Description("This opens up the default email client with prepopulated contents")]
    [EasyField("Recipient", "Email Addresses of the recipient(s).  Separate multiple email adddresses with ';'")]
    [EasyField("CC", "Email Addresses to be copied.  Separate multiple email adddresses with ';'")]
    [EasyField("BCC", "Email Addresses to be blind copied.  Separate multiple email adddresses with ';'")]
    [EasyField("Subject", "Subject of the email.")]
    [EasyField("Body", @"Body of the email.  Can be html content too.  Alternatively, you can specify a file name like @C:\Temp\Template.html")]
    [EasyField("IndividualEmails", "Should we send one email per row?", "False", "True|False", "True;False")]
    public class EmailAction : AbstractEasyAction
    {
        public string Recipient;
        public string CC;
        public string BCC;
        public string Subject;
        public string Body;
        public bool IndividualEmails = false;

        public override bool IsFieldSettingsComplete()
        {
            LoadSettings();
            return (!String.IsNullOrWhiteSpace(Recipient) && !String.IsNullOrWhiteSpace(Subject) && !String.IsNullOrWhiteSpace(Body));
        }

        public virtual void LoadSettings()
        {
            Recipient = (SettingsDictionary.ContainsKey("Recipient") ? SettingsDictionary["Recipient"] : "");
            CC = (SettingsDictionary.ContainsKey("CC") ? SettingsDictionary["CC"] : "");
            BCC = (SettingsDictionary.ContainsKey("BCC") ? SettingsDictionary["BCC"] : "");
            Subject = (SettingsDictionary.ContainsKey("Subject") ? SettingsDictionary["Subject"] : "");
            Body = (SettingsDictionary.ContainsKey("Body") ? SettingsDictionary["Body"] : "");
            if (Body.StartsWith("@"))
            {
                if (File.Exists(Body.Substring(1))) Body = File.ReadAllText(Body.Substring(1));
            }
            IndividualEmails = (SettingsDictionary.ContainsKey("IndividualEmails") ? Convert.ToBoolean(SettingsDictionary["IndividualEmails"]) : false);
        }

        public override bool CanExecute(Dictionary<string, string> dataDictionary)
        {
            return IsFieldSettingsComplete();
        }

        public override void Execute(params DataRow[] dataRows)
        {
            if (!IsFieldSettingsComplete()) return;
            if (!CanExecute(dataRows)) return;
            if (dataRows.Length ==0) return;
            if ((IndividualEmails) || (dataRows.Length == 1))
            {
                base.Execute(dataRows);
            }
            else
            {
                string recipient = Recipient;
                string cc = CC;
                string bcc = BCC;
                string subject = Subject;
                string body = Body;
                subject = Regex.Replace(subject, "\\[RowCount\\]", dataRows.Length.ToString(), RegexOptions.IgnoreCase);
                body = Regex.Replace(body, "\\[RowCount\\]", dataRows.Length.ToString(), RegexOptions.IgnoreCase);
                body = GetPopulatedBody(body, dataRows);
                SendEmail(recipient, cc, bcc, subject, body);            
            }
        }

        public virtual void SendEmail(string recipient, string cc, string bcc, string subject, string body)
        {

            if (String.IsNullOrWhiteSpace(cc)) cc = " ";
            if (String.IsNullOrWhiteSpace(bcc)) bcc = " ";
            if (String.IsNullOrWhiteSpace(recipient)) recipient = " ";
            string processCommand = String.Format("mailto:{0}?subject={1}&cc={2}&bcc={3}&body={4}", recipient, subject, cc, bcc, body);
            Process.Start(processCommand);
        }

        public virtual string GetPopulatedBody(string body, DataRow[] dataRows)
        {
            bool isHtml = body.StartsWith("<html", StringComparison.CurrentCultureIgnoreCase);
            string tableName = dataRows[0].Table.TableName;
            if (body.IndexOf("[[" + dataRows[0].Table.TableName + "]]", StringComparison.CurrentCultureIgnoreCase) < 0) tableName = "data";
            if (body.IndexOf("[[" + tableName + "]]", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                StringBuilder sbDataTable = new StringBuilder();
                if (isHtml)
                {
                    sbDataTable.AppendLine("<table name='" + tableName + "' width='100%'>");
                    sbDataTable.AppendLine("<thead>");
                }
                foreach (DataColumn dataColumn in dataRows[0].Table.Columns)
                {
                    if (isHtml)
                    {
                        sbDataTable.AppendLine("<th>");
                        sbDataTable.AppendLine(dataColumn.ColumnName);
                        sbDataTable.AppendLine("</th>");
                    }
                    else 
                        sbDataTable.Append(dataColumn.ColumnName + '\t');
                }
                sbDataTable.AppendLine();
                if (isHtml) sbDataTable.AppendLine("<thead>");
                foreach (DataRow dataRow in dataRows)
                {
                    if (isHtml) sbDataTable.AppendLine("<tr>");
                    foreach (DataColumn dataColumn in dataRows[0].Table.Columns)
                    {
                        if (isHtml)
                        {
                            sbDataTable.AppendLine("<td>");
                            sbDataTable.AppendLine(dataRow[dataColumn.ColumnName].ToString());
                            sbDataTable.AppendLine("</td>");
                        }
                        else 
                            sbDataTable.Append(dataRow[dataColumn.ColumnName].ToString() + '\t');
                    }
                    sbDataTable.AppendLine();
                    if (isHtml) sbDataTable.AppendLine("</tr>");
                }
                if (isHtml) sbDataTable.AppendLine("</table>");
                body = Regex.Replace(body, "\\[\\[" + tableName + "\\]\\]", sbDataTable.ToString(), RegexOptions.IgnoreCase);
            }
            return body;
        }

        public override void Execute(Dictionary<string, string> dataDictionary)
        {
            if (CanExecute(dataDictionary))
            {                
                string recipient = Recipient;
                string cc = CC;
                string bcc = BCC;
                string subject = Subject;
                string body = Body;
                foreach (KeyValuePair<string, string> kvPair in dataDictionary)
                {
                    recipient = Regex.Replace(recipient, "\\[" + kvPair.Key + "\\]", kvPair.Value, RegexOptions.IgnoreCase);
                    cc = Regex.Replace(cc, "\\[" + kvPair.Key + "\\]", kvPair.Value, RegexOptions.IgnoreCase);
                    bcc = Regex.Replace(bcc, "\\[" + kvPair.Key + "\\]", kvPair.Value, RegexOptions.IgnoreCase);
                    subject = Regex.Replace(subject, "\\[" + kvPair.Key + "\\]", kvPair.Value, RegexOptions.IgnoreCase);
                    body = Regex.Replace(body, "\\[" + kvPair.Key + "\\]", kvPair.Value, RegexOptions.IgnoreCase);
                }

                SendEmail(recipient, cc, bcc, subject, body);            
            }
        }
    }
}
