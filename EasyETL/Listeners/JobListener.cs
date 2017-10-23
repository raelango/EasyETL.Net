using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Dynamic;

namespace EasyETL.Listeners
{

    public class ListenerTriggeredEventArgs : EventArgs
    {
        public JobListener Listener;
        public object Originator;
        public DateTime TimeTriggered;
        public Dictionary<string,object> Data;
    }

    public abstract class JobListener 
    {

        public event EventHandler<ListenerTriggeredEventArgs> OnTriggered;

        private object _originator = null;
        int _triggeredCount = 0;
        protected Dictionary<string, object> DataToPass = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

        public JobListener()
        {
            _originator = null;
        }

        public JobListener(object caller)
        {
            _originator = caller;
        }

        public bool Start()
        {
            if (PreStartOperations())
            {
                if (StartOperations())
                {
                    if (PostStartOperations())
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        public virtual bool Stop()
        {
            if (PreStopOperations()) {
                if (StopOperations()) {
                    if (PostStopOperations()) {
                        return true;
                    }
                }

            }
            return false;
        }

        public void TriggerEvent()
        {
            EventHandler<ListenerTriggeredEventArgs> handler = OnTriggered;
            if (handler != null)
            {
                ListenerTriggeredEventArgs e = new ListenerTriggeredEventArgs() { Listener = this, Originator = _originator, TimeTriggered = DateTime.Now, Data = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase) };
                SetListenerSpecificData(e);
                handler(this, e);
            }
        }

        public virtual void SetListenerSpecificData(ListenerTriggeredEventArgs eventArgs) {
            _triggeredCount += 1;
            eventArgs.Data["RunID"] = _triggeredCount;
            foreach (KeyValuePair<string, object> kvPair in DataToPass)
            {
                eventArgs.Data[kvPair.Key] = kvPair.Value;
            }
        }

        public virtual bool PreStartOperations()
        {
            return true;
        }

        public virtual bool StartOperations()
        {
            return true;
        }

        public virtual bool PostStartOperations()
        {
            return true;
        }

        public virtual bool PreStopOperations()
        {
            return true;
        }

        public virtual bool StopOperations()
        {
            return true;
        }

        public virtual bool PostStopOperations()
        {
            return true;
        }

    }

}
