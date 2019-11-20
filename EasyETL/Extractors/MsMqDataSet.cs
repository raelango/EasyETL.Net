using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Data;

namespace EasyETL.DataSets
{
    public class MsMqDataSet : EasyDataSet 
    {
        MessageQueue messageQueue = null;
        public MsMqDataSet(string queueName)
        {
            messageQueue = new MessageQueue(queueName);
        }

        public override void Fill()
        {
            CreateTableStructure();
        }

        public override void CreateTableStructure()
        {
            if (Tables.Count >0) {
                Tables.Clear();
            }
            DataTable dt = Tables.Add(messageQueue.QueueName);
            dt.Columns.Add("ArrivedTime");
            dt.Columns.Add("Body");
            dt.Columns.Add("Label");
            dt.Columns.Add("MessageType");
            dt.Columns.Add("Priority");
            dt.Columns.Add("SenderID");
            dt.Columns.Add("SenderVersion");
            dt.Columns.Add("SentTime");
            dt.Columns.Add("SourceMachine");
            dt.Columns.Add("TransactionID");
        }

        public override void ProcessRowObject(object row)
        {
            if (row is Dictionary<string, object> Data)
            {
                if (Data.ContainsKey("Message"))
                {
                    row = Data["Message"];
                }
            }

            if (row is Message msg)
            {
                DataRow dr = Tables[0].NewRow();
                dr["ArrivedTime"] = msg.ArrivedTime;
                dr["Body"] = msg.Body.ToString();
                dr["Label"] = msg.Label;
                dr["MessageType"] = msg.MessageType.ToString();
                dr["Priority"] = msg.Priority.ToString();
                dr["SenderID"] = msg.SenderId.ToString();
                dr["SenderVersion"] = msg.SenderVersion.ToString();
                dr["SentTime"] = msg.SentTime.ToString();
                dr["SourceMachine"] = msg.SourceMachine;
                dr["TransactionID"] = msg.TransactionId;
                Tables[0].Rows.Add(dr);
            }
        }


    }
}
