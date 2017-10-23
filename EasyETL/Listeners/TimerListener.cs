using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace EasyETL.Listeners
{

    public class TimerListener : JobListener, IDisposable
    {
        #region properties 
        public List<DateTime> TimesToRun = new List<DateTime>();
        public ListenerDaysOfWeek WeekDaysToRun =  ListenerDaysOfWeek.Daily;
        public List<int> DaysToRun = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public List<int> MonthsToRun = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }

            set
            { 
                _startTime = value;
                _endTime = DateTime.Parse("11:59 PM");
            }
        }

        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }

            set
            {
                while (value < DateTime.Now)
                {
                    value = value.AddDays(1);
                }
                _endTime = value;
            }
        }
        #endregion

        #region private methods
        private DateTime _startTime;
        private DateTime _endTime;

        Timer _timer = null;
        #endregion private methods

        #region constructors
        public TimerListener(object caller, int intSeconds, DateTime? startTime = null, DateTime? endTime = null) : base(caller) {
            InitTimer(intSeconds,startTime,endTime);
            
        }

        public TimerListener(object caller, string weekDays, int intSeconds, DateTime? startTime = null, DateTime? endTime = null)
            : base(caller)
        {
            InitTimer(intSeconds, startTime, endTime);
            SetWeekDays(weekDays);
        }

        public TimerListener(object caller, ListenerDaysOfWeek weekDays, int intSeconds, DateTime? startTime = null, DateTime? endTime = null)
            : base(caller)
        {
            InitTimer(intSeconds, startTime, endTime);
            WeekDaysToRun = weekDays;
        }

        public TimerListener(object caller, params DateTime[] triggerTime)
            : base(caller)
        {
            InitTimer(1);
            TimesToRun.AddRange(triggerTime);
        }

        public TimerListener(object caller, string weekDays, params DateTime[] triggerTime) : base(caller)
        {
            InitTimer(1);
            TimesToRun.AddRange(triggerTime);
            SetWeekDays(weekDays);
        }

        public TimerListener(object caller, ListenerDaysOfWeek weekDays, params DateTime[] triggerTime) : base(caller)
        {
            InitTimer(1);
            TimesToRun.AddRange(triggerTime);
            WeekDaysToRun = weekDays;
        }
        #endregion 

        #region Overridden methods
        public override bool StartOperations()
        {
            if (base.StartOperations())
            {
                _timer.Start();
                return true;
            }
            return false;
        }

        public override bool StopOperations()
        {
            if (base.StopOperations())
            {
                _timer.Stop();
                return true;
            }
            return false;
        }

        public override void SetListenerSpecificData(ListenerTriggeredEventArgs eventArgs)
        {
            base.SetListenerSpecificData(eventArgs);
            eventArgs.Data["DayOfWeek"] = DateTime.Now.DayOfWeek.ToString();
        }
        #endregion

        #region Public methods
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }
        #endregion

        #region private methods
        private void InitTimer(int intSeconds, DateTime? startTime = null, DateTime? endTime = null)
        {
            _timer = new Timer(intSeconds * 1000);
            TimesToRun = new List<DateTime>();
            SetWeekDays(true);
            _timer.Elapsed += _timer_Elapsed;
            if (startTime == null)
            {
                startTime = DateTime.Parse("12:00 AM");
            }
            if (endTime == null)
            {
                endTime = DateTime.Parse("11:59 PM");
            }
            StartTime = startTime.Value;
            EndTime = endTime.Value; 
        }

        private void SetWeekDays(bool enabled)
        {
            WeekDaysToRun = enabled ? ListenerDaysOfWeek.Daily : ListenerDaysOfWeek.Nodays;
        }

        private void SetWeekDays(string weekDays)
        {
            bool enabled = true;
            if (weekDays.StartsWith("NOT ", StringComparison.CurrentCultureIgnoreCase))
            {
                enabled = false;
                weekDays = weekDays.Remove(0, 4);
            }
            SetWeekDays(!enabled);
            ListenerDaysOfWeek currentDayOfWeek = ListenerDaysOfWeek.Nodays;
            MatchCollection matches = Regex.Matches(weekDays, "(?<WeekDay>\\w+)");
            foreach (Match match in matches)
            {
                string weekDay = match.Groups["WeekDay"].Value;
                if (new char[] { 'M', 'W', 'F', 'm', 'w', 'f' }.Contains(weekDay[0]))
                {
                    switch (weekDay[0])
                    {
                        case 'm':
                        case 'M':
                            currentDayOfWeek = ListenerDaysOfWeek.Monday;
                            break;
                        case 'W':
                        case 'w':
                            currentDayOfWeek = ListenerDaysOfWeek.Wednesday;
                            break;
                        case 'F':
                        case 'f':
                            currentDayOfWeek = ListenerDaysOfWeek.Friday;
                            break;
                    }
                }
                if (weekDay.Length > 1)
                {
                    if (new string[] { "TU", "TH", "SU", "SA" }.Contains(weekDay.Substring(0, 2).ToUpper()))
                    {
                        switch (weekDay.Substring(0, 2).ToUpper())
                        {
                            case "TU":
                                currentDayOfWeek = ListenerDaysOfWeek.Tuesday;
                                break;
                            case "TH":
                                currentDayOfWeek = ListenerDaysOfWeek.Thursday;
                                break;
                            case "SU":
                                currentDayOfWeek = ListenerDaysOfWeek.Sunday;
                                break;
                            case "SA":
                                currentDayOfWeek = ListenerDaysOfWeek.Saturday;
                                break;
                        }
                    }
                }
                if (enabled)
                {
                    WeekDaysToRun |= currentDayOfWeek;
                }
                else
                {
                    WeekDaysToRun &= ~currentDayOfWeek;
                }
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            while (DateTime.Now > EndTime)
            {
                StartTime = StartTime.AddDays(1);
                EndTime = EndTime.AddDays(1);
            }
            bool triggerNow = ((DateTime.Now >= StartTime) && (DateTime.Now <= EndTime) && (DaysToRun.Contains(DateTime.Today.Day)) && (MonthsToRun.Contains(DateTime.Today.Month)));

            if (triggerNow && (TimesToRun.Count > 0))
            {
                triggerNow = false;
                DateTime[] timesToRun = TimesToRun.ToArray();
                foreach (DateTime dateTime in timesToRun)
                {
                    if (dateTime < DateTime.Now)
                    {
                        triggerNow = true;
                        DateTime nextExecTime = dateTime.AddDays(1);
                        while (nextExecTime < DateTime.Now)
                        {
                            nextExecTime = nextExecTime.AddDays(1);
                        }
                        TimesToRun.Remove(dateTime);
                        TimesToRun.Add(nextExecTime);
                    }
                }
            }

            if (triggerNow)
            {
                triggerNow = WeekDaysToRun.HasFlag((ListenerDaysOfWeek)Enum.Parse(typeof(ListenerDaysOfWeek), DateTime.Today.DayOfWeek.ToString()));
            }

            if (triggerNow)
            {
                TriggerEvent();
            }
            _timer.Enabled = true;
        }
        #endregion 

    }

    [Flags]
    public enum ListenerDaysOfWeek
    {
        Nodays = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64,
        Daily = Sunday + Monday + Tuesday + Wednesday + Thursday + Friday + Saturday,
        Weekdays = Monday + Tuesday + Wednesday + Thursday + Friday,
        Weekends = Sunday + Saturday
    }

}
