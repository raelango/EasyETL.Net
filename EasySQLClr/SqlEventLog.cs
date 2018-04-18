using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace EasySQLClr
{
    public class SqlEventLog
    {

        public void WriteEventLog(string eventMessage, string logName = "", string eventSource = "", string eventType = "", int eventID = 101, short eventCategoryID = 1)
        {
            if (String.IsNullOrEmpty(eventType)) eventType = "Information";
            if (String.IsNullOrEmpty(logName)) logName = "Application";
            if (String.IsNullOrEmpty(eventSource)) eventSource = Assembly.GetEntryAssembly().GetName().Name;

            EventLogEntryType elEntryType = EventLogEntryType.Information;
            if (!Enum.TryParse<EventLogEntryType>(eventType, out elEntryType))
            {
                elEntryType = EventLogEntryType.Information;
            }
            using (EventLog eventLog = new EventLog(logName))
            {
                eventLog.Source = eventSource;
                eventLog.WriteEntry(eventMessage, elEntryType, eventID, eventCategoryID);
            }
        }


    }
}
