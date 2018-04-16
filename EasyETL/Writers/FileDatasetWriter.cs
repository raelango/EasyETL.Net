using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    public abstract class FileDatasetWriter : DatasetWriter
    {
        protected string _fileName = String.Empty;

        public FileDatasetWriter()
            : base()
        {
            _fileName = String.Empty;
        }

        public FileDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
            _fileName = String.Empty;
        }

        public FileDatasetWriter(DataSet dataSet, string fileName) : base(dataSet) {
            _fileName = fileName;
        }

        public override void Write()
        {
            if (!String.IsNullOrWhiteSpace(_fileName))
            {
                SaveContentToFile(BuildOutputString());
            }
        }

        public virtual void Write(string fileName)
        {
            _fileName = fileName;
            Write();
        }

        public virtual void Write(DataSet dataSet, string fileName = "")
        {
            _dataSet = dataSet;
            Write(fileName);
        }

        public virtual void SaveContentToFile(string contentToWrite)
        {
            using (StreamWriter outputFile = new StreamWriter(_fileName,false))
            {
                outputFile.Write(contentToWrite);
            }
        }    
    }
}
