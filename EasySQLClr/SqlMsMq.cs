using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace EasySQLClr
{
    public class SqlMsMq
    {

        public void SendMessage(string queuePath,string messageLabel, string messageBody)
        {
            if (MessageQueue.Exists(queuePath))
            {
                using (MessageQueue mq = new MessageQueue(queuePath))
                {
                    Message mm = new Message();
                    mm.Label = messageLabel;
                    mm.Body = messageBody;
                    mq.Send(mm);
                }
            }
        }
    }
}
