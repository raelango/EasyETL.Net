using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EasyETL.Listeners;
using EasyETL.Parsers;
using EasyETL.DataSets;
using EasyETL.Writers;

namespace EasyETL
{
    public class EasyETLJob : IDisposable
    {
        public EasyDataSet Data = null;

        public List<JobListener> Listeners = new List<JobListener>();

        public List<DatasetParser> DatasetParsers = new List<DatasetParser>();

        public List<DatasetWriter> Loaders = new List<DatasetWriter>();

        public event EventHandler<RowReadEventArgs> RowReadAndProcessed;

        public event EventHandler<JobDataChangedEventArgs> DataChanged;               

        public void Start()
        {
            foreach (DatasetParser extractor in DatasetParsers)
            {
                extractor.LineReadAndProcessed += Extractor_LineReadAndProcessed;
                extractor.Parse();
            }
            BuildMasterDataSet();
            foreach (JobListener listener in Listeners)
            {
                listener.OnTriggered += Listener_OnTriggered;
                listener.Start();
            }

            foreach (DatasetWriter loader in Loaders)
            {
                loader.Write(Data);
            }
        }

        void Extractor_LineReadAndProcessed(object sender, RowReadEventArgs e)
        {
            RowReadAndProcessed?.Invoke(this, e);
        }

        void Listener_OnTriggered(object sender, ListenerTriggeredEventArgs e)
        {
            if (e.Data.ContainsKey("AdditionalContent"))
            {
                string addnlData = e.Data["AdditionalContent"].ToString();
                foreach (DatasetParser extractor in DatasetParsers)
                {
                    extractor.ProcessRowObject(addnlData);
                }
                BuildMasterDataSet();
                RaiseDataChangedEvent();
            }
        }

        void RaiseDataChangedEvent()
        {
            EventHandler<JobDataChangedEventArgs> handler = DataChanged;
            if (handler != null)
            {
                JobDataChangedEventArgs e = new JobDataChangedEventArgs();
                handler(this, e);
            }
        }

        private void BuildMasterDataSet()
        {
            if (DatasetParsers.Count == 1)
            {
                Data = DatasetParsers.First().Data;
            }
            else
            {
                Data = new RegexDataSet();
                foreach (DatasetParser extractor in DatasetParsers)
                {
                    Data.Merge(extractor.Data);
                }
            }
        }

        public void Stop()
        {
            foreach (JobListener listener in Listeners)
            {
                listener.Stop();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Data.Dispose();
                Listeners.Clear();
                DatasetParsers.Clear();
                Loaders.Clear();
            }
        }
    }


    public class JobDataChangedEventArgs : EventArgs
    {
    }

}
