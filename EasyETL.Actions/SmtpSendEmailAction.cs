using EasyETL.Attributes;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace EasyETL.Actions
{
    [DisplayName("Send EMail using SMTP")]
    [Description("This sends email using Smtp Server")]
    [EasyField("SmtpServer", "SMTP Host Name","smtp.live.com","","smtp.live.com;smtp.gmail.com")]
    [EasyField("SmtpPort", "SMTP Port Number", "587","\\d+")]
    [EasyField("SenderAccount", "Sender email address.")]
    [EasyField("SenderAccountPassword", "Sender Password to authenticate.","","","",true)]
    [EasyField("Recipient", "Email Addresses of the recipient(s).  Separate multiple email adddresses with ';'")]
    [EasyField("CC", "Email Addresses to be copied.  Separate multiple email adddresses with ';'")]
    [EasyField("BCC", "Email Addresses to be blind copied.  Separate multiple email adddresses with ';'")]
    [EasyField("Subject", "Subject of the email.")]
    [EasyField("Body", @"Body of the email.  Can be html content too.  Alternatively, you can specify a file name like @C:\Temp\Template.html")]
    [EasyField("IndividualEmails", "Should we send one email per row?", "False", "True|False", "True;False")]
    [EasyField("UseSSL", "Should we use SSL for SMTP Connection?", "True", "True|False", "True;False")]
    [EasyField("UseDefaultCredentials", "Should we use Default Credentials?", "True", "True|False", "True;False")]

    public class SmtpSendEmailAction : EmailAction
    {
        public string SmtpServerName;
        public int SmtpPort;
        public string SenderAccount;
        public string SenderAccountPassword;
        public bool UseSSL;
        public bool UseDefaultCredentials;

        public override bool IsFieldSettingsComplete()
        {
            if (!base.IsFieldSettingsComplete()) return false;
            if (String.IsNullOrWhiteSpace(SmtpServerName)) return false;
            if (String.IsNullOrWhiteSpace(SenderAccount) && UseDefaultCredentials)
                SenderAccount = Environment.UserName;
            if (!UseDefaultCredentials && String.IsNullOrWhiteSpace(SenderAccountPassword)) return false;
            return true; 
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            return true;
        }

        public override void LoadSettings()
        {
            base.LoadSettings();
            SenderAccount = (SettingsDictionary.ContainsKey("SenderAccount") ? SettingsDictionary["SenderAccount"]:"");
            SenderAccountPassword = (SettingsDictionary.ContainsKey("SenderAccountPassword") ? SettingsDictionary["SenderAccountPassword"] : "");
            SmtpServerName = (SettingsDictionary.ContainsKey("SmtpServer") ? SettingsDictionary["SmtpServer"] : "");
            SmtpPort = (SettingsDictionary.ContainsKey("SmtpPort") ? Convert.ToInt16(SettingsDictionary["SmtpPort"]) : 587);
            UseSSL = (SettingsDictionary.ContainsKey("UseSSL") ? Convert.ToBoolean(SettingsDictionary["UseSSL"]) : true);
            UseDefaultCredentials = (SettingsDictionary.ContainsKey("UseDefaultCredentials") ? Convert.ToBoolean(SettingsDictionary["UseDefaultCredentials"]) : true);
        }

        public override void SendEmail(string recipient, string cc, string bcc, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(SenderAccount)
            };
            mailMessage.To.Add(recipient.Replace(';', ','));
            if (!String.IsNullOrWhiteSpace(cc)) mailMessage.CC.Add(cc.Replace(';', ','));
            if (!String.IsNullOrWhiteSpace(bcc)) mailMessage.Bcc.Add(bcc.Replace(';', ','));
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = body.StartsWith("<html", StringComparison.CurrentCultureIgnoreCase);
            mailMessage.Body = body;

            using (SmtpClient smtpClient = new SmtpClient(SmtpServerName))
            {
                smtpClient.Port = SmtpPort;
                smtpClient.UseDefaultCredentials = UseDefaultCredentials;
                if (!UseDefaultCredentials)
                {
                    smtpClient.Credentials = new NetworkCredential(SenderAccount, SenderAccountPassword);
                }
                smtpClient.EnableSsl = UseSSL;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
