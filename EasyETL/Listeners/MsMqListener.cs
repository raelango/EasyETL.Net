using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace EasyETL.Listeners
{
    public class MsMqListener : JobListener, IDisposable 
    {
        MessageQueue _mQueue = null;
        string _queueName = String.Empty;

        public MsMqListener(object caller, string queueName)
            : base(caller)
        {
            _queueName = queueName;
        }

        void MessageReceived(object sender, ReceiveCompletedEventArgs e)
        {
            Message msg = _mQueue.EndReceive(e.AsyncResult);
            DataToPass["Message"] = msg;
            DataToPass["Label"] = msg.Label;
            DataToPass["MessageType"] = msg.MessageType.ToString();
            DataToPass["Body"] = msg.Body;
            TriggerEvent();
            _mQueue.BeginReceive();
        }

        #region Public override operations
        public override bool StartOperations()
        {
            if (base.StartOperations())
            {
                if (MessageQueue.Exists(_queueName))
                    _mQueue = new MessageQueue(_queueName);
                else
                    _mQueue = MessageQueue.Create(_queueName);
                _mQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) }); 
                _mQueue.ReceiveCompleted += MessageReceived;
                _mQueue.BeginReceive();
                return true;
            }
            return false;
        }

        public override bool Stop()
        {
            if (base.Stop())
            {
                _mQueue.ReceiveCompleted -= MessageReceived;
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if (_mQueue != null)
            {
                _mQueue.Close();
                _mQueue = null;
            }
        }
        #endregion
    }
}
