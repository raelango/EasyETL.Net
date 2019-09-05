using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace EasyETL.Listeners
{
    public class MailListener : JobListener, IDisposable
    {
        public SmtpClient MailClient = null;
        public MailListener()
        {
            MailClient = new SmtpClient();
        }
    }
}
