using ol= Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using EasyETL.Attributes;

namespace EasyETL.Actions.Outlook
{
    [DisplayName("Send EMail using Outlook")]
    [Description("This opens up the outlook email client with prepopulated contents")]
    [EasyField("Recipient", "Email Addresses of the recipient(s).  Separate multiple email adddresses with ';'")]
    [EasyField("CC", "Email Addresses to be copied.  Separate multiple email adddresses with ';'")]
    [EasyField("BCC", "Email Addresses to be blind copied.  Separate multiple email adddresses with ';'")]
    [EasyField("Subject", "Subject of the email.")]
    [EasyField("Body", @"Body of the email.  Can be html content too.  Alternatively, you can specify a file name like @C:\Temp\Template.html")]
    [EasyField("IndividualEmails", "Should we send one email per row?", "False", "True|False", "True;False")]
    [EasyField("AutoSend", "Should we automatically send the email?", "False", "True|False", "True;False")]
    public class OutlookSendEmailAction : EmailAction
    {
        public bool AutoSend = false;

        public override void LoadSettings()
        {
            base.LoadSettings();
            AutoSend = (SettingsDictionary.ContainsKey("AutoSend") ? Convert.ToBoolean(SettingsDictionary["AutoSend"]) : false);
        }

        public override void SendEmail(string recipient, string cc, string bcc, string subject, string body)
        {
            ol.Application olApp = new ol.Application();
            ol.MailItem eMail = (ol.MailItem)olApp.CreateItem(ol.OlItemType.olMailItem);
            eMail.To = recipient;
            eMail.CC = cc;
            eMail.BCC = bcc;
            eMail.Subject = subject;
            eMail.BodyFormat = (body.StartsWith("<html", StringComparison.CurrentCultureIgnoreCase)) ? ol.OlBodyFormat.olFormatHTML : ol.OlBodyFormat.olFormatPlain;
            if (eMail.BodyFormat == ol.OlBodyFormat.olFormatHTML) eMail.HTMLBody = body;
            if (eMail.BodyFormat == ol.OlBodyFormat.olFormatPlain) eMail.Body = body;
            if (AutoSend)
                eMail.Send();
            else
            {
                eMail.Save();
                eMail.Display(false);
            }
            Marshal.ReleaseComObject(eMail);
            eMail = null;
        }
    }
}
