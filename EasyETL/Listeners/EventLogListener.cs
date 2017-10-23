using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EasyETL.Listeners
{
    public class EventLogListener : JobListener, IDisposable
    {
        EventLog  _eventLog = null;

        string _logName = String.Empty;
        string _sourceName = String.Empty;
        string _machineName = String.Empty;

        public EventLogListener(object caller, string logName, string sourceName = "", string machineName = ".")
            : base(caller)
        {
            _logName = logName;
            _sourceName = sourceName;
            _machineName = machineName;
        }

        
        #region Public override operations
        public override bool StartOperations()
        {
            if (base.StartOperations())
            {
                if (String.IsNullOrEmpty(_sourceName))
                {
                    _eventLog = new EventLog(_logName , _machineName);
                }
                else
                {
                    _eventLog = new EventLog(_logName, _machineName, _sourceName);
                }
                _eventLog.EntryWritten += EventLogWritten;
                _eventLog.EnableRaisingEvents = true;
                return true;
            }
            return false;
        }

        void EventLogWritten(object sender, EntryWrittenEventArgs e)
        {
            DataToPass["EventLogEntry"] = e.Entry;
            DataToPass["Index"] = e.Entry.Index;
            DataToPass["Message"] = e.Entry.Message;
            DataToPass["EventID"] = e.Entry.InstanceId;
            DataToPass["EventType"] = e.Entry.EntryType.ToString();
            DataToPass["Category"] = e.Entry.Category; 

            DataToPass["User"] = e.Entry.UserName;
            DataToPass["Source"] = e.Entry.Source;
            DataToPass["Machine"] = e.Entry.MachineName;
            DataToPass["TimeCreated"] = e.Entry.TimeGenerated;
            DataToPass["TimeWritten"] = e.Entry.TimeWritten;


            TriggerEvent();
        }

        public override bool Stop()
        {
            if (base.Stop())
            {
                _eventLog.EnableRaisingEvents = false;
                _eventLog.EntryWritten -= EventLogWritten;
                _eventLog = null;
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if (_eventLog != null)
            {
                _eventLog.EnableRaisingEvents = false;
                _eventLog.EntryWritten -= EventLogWritten;
                _eventLog = null;
            }
        }
        #endregion
    }
}
