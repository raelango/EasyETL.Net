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
        public RegexDataSet Data = new RegexDataSet();

        public List<JobListener> Listeners = new List<JobListener>();

        public List<Extractor> Extractors = new List<Extractor>();

        public List<DatasetWriter> Loaders = new List<DatasetWriter>();

        public event EventHandler<LinesReadEventArgs> LineReadAndProcessed;

        public event EventHandler<JobDataChangedEventArgs> DataChanged;               

        public void Run()
        {
            foreach (Extractor extractor in Extractors)
            {
                extractor.LineReadAndProcessed += extractor_LineReadAndProcessed;
                extractor.Parse();
            }
            BuildMasterDataSet();
            foreach (JobListener listener in Listeners)
            {
                listener.OnTriggered += listener_OnTriggered;
                listener.Start();
            }
        }

        void extractor_LineReadAndProcessed(object sender, LinesReadEventArgs e)
        {
            EventHandler<LinesReadEventArgs> handler = LineReadAndProcessed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void listener_OnTriggered(object sender, ListenerTriggeredEventArgs e)
        {
            if (e.Data.ContainsKey("AdditionalContent"))
            {
                string addnlData = e.Data["AdditionalContent"].ToString();
                foreach (Extractor extractor in Extractors)
                {
                    extractor.ParseAndLoadLines(addnlData);
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
            if (Extractors.Count == 1)
            {
                Data = Extractors.First().Data;
            }
            else
            {
                Data = new RegexDataSet();
                foreach (Extractor extractor in Extractors)
                {
                    Data.Merge(extractor.Data);
                }
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
                Extractors.Clear();
                Loaders.Clear();
            }
        }
    }


    public class JobDataChangedEventArgs : EventArgs
    {
    }

}
