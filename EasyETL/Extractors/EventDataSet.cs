using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;

namespace EasyETL.DataSets
{
    public class EventDataSet : EasyDataSet
    {
        EventLog  _eventLog = null;
        string _logName = String.Empty;
        string _sourceName = String.Empty;
        string _machineName = String.Empty;

        public EventDataSet(string logName, string sourceName ="", string machineName = ".")
        {
            _logName = logName;
            _sourceName = sourceName;
            _machineName = machineName;
        }

        public override void Fill()
        {
            if (String.IsNullOrEmpty(_sourceName))
            {
                _eventLog = new EventLog(_logName, _machineName);
            }
            else
            {
                _eventLog = new EventLog(_logName, _machineName, _sourceName);
            }

            CreateTableStructure();
            foreach (EventLogEntry elEntry in _eventLog.Entries)
            {
                ProcessRowObject(elEntry);
            }
        }

        public override void CreateTableStructure()
        {
            if (Tables.Count > 0)
            {
                Tables.Clear();
            }
            DataTable dt = Tables.Add(((_machineName !=".") ? _machineName + "_" : String.Empty ) + _logName + (!String.IsNullOrWhiteSpace(_sourceName) ? "_" + _sourceName : String.Empty ) );
            dt.Columns.Add("Index");
            dt.Columns.Add("Category");
            dt.Columns.Add("CategoryID");
            dt.Columns.Add("EntryType");
            dt.Columns.Add("Message");
            dt.Columns.Add("MachineName");
            dt.Columns.Add("Application");
            dt.Columns.Add("TimeCreated");
            dt.Columns.Add("TimeWritten");
            dt.Columns.Add("UserName");
        }

        public override void ProcessRowObject(object row)
        {
            if (row is Dictionary<string, object> Data)
            {
                if (Data.ContainsKey("EventLogEntry"))
                {
                    row = Data["EventLogEntry"];
                }
            }

            if (row is EventLogEntry elogEntry)
            {
                DataRow dr = Tables[0].NewRow();
                dr["Index"] = elogEntry.Index.ToString();
                dr["Category"] = elogEntry.Category;
                dr["CategoryID"] = elogEntry.CategoryNumber.ToString();
                dr["EntryType"] = elogEntry.EntryType.ToString();
                dr["Message"] = elogEntry.Message;
                dr["MachineName"] = elogEntry.MachineName;
                dr["Application"] = elogEntry.Source;
                dr["TimeCreated"] = elogEntry.TimeGenerated.ToString();
                dr["TimeWritten"] = elogEntry.TimeWritten.ToString();
                dr["UserName"] = elogEntry.UserName;
                Tables[0].Rows.Add(dr);
                SendMessageToCallingApplicationHandler(Tables[0].Rows.Count, "Importing Entry");
            }
        }


    }
}
