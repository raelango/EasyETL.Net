using EasyETL.Endpoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    public abstract class FileDatasetWriter : DatasetWriter
    {
        protected string _fileName = String.Empty;
        protected IEasyEndpoint endpoint = null;
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
            _fileName = PopulatedName(_fileName);
            if (!String.IsNullOrWhiteSpace(_fileName))
            {
                SaveContentToFile(BuildOutputString());
            }
        }

        public virtual void Write(string fileName, IEasyEndpoint epoint = null)
        {
            _fileName = fileName;
            endpoint = epoint;
            Write();
        }

        public string PopulatedName(string fileName)
        {
            MatchCollection matches = Regex.Matches(fileName, @"\[(?<Var>.*?)\]");
            foreach (Match match in matches)
            {
                string fieldName = match.Groups["Var"].Value;
                if ((_dataSet != null) && (_dataSet.Tables.Count == 1) && (_dataSet.Tables[0].Columns[fieldName] != null) && (_dataSet.Tables[0].Rows.Count > 0))
                {
                    fileName = fileName.Replace('[' + fieldName + ']', _dataSet.Tables[0].Rows[0][fieldName].ToString());
                }
                else
                {
                    fileName = fileName.Replace('[' + fieldName + ']', DateTime.Now.ToString(fieldName));
                }
            }
            return fileName;
            //throw new NotImplementedException();
        }

        public virtual void Write(DataSet dataSet, string fileName = "", IEasyEndpoint epoint = null)
        {
            _dataSet = dataSet;
            Write(fileName, epoint);
        }

        public virtual void SaveContentToFile(string contentToWrite)
        {
            if (endpoint == null)
            {

                using (StreamWriter outputFile = new StreamWriter(_fileName, false))
                {
                    outputFile.Write(contentToWrite);
                }
            }
            else
            {
                endpoint.Write(_fileName, ASCIIEncoding.ASCII.GetBytes(contentToWrite));
            }
        }    
    }
}
