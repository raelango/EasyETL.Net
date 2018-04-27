using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;

namespace EasyETL.DataSets
{
    public class RowReadEventArgs : EventArgs
    {
        public int RowNumber { get; set; }
        public string Message { get; set; }
        public object RowData { get; set; }
    }

    public abstract class EasyDataSet : DataSet 
    {

        /// <summary>
        ///     The name the datatable in the dataset should get
        ///     or the name of the datatable to use when a dataset is
        ///     provided
        /// </summary>
        public string TableName = "Table1";

        /// <summary>
        ///     Lines in the text file that did not match
        ///     the regular expression
        /// </summary>
        public List<string> MisReads { get; protected set; }

        public event EventHandler<RowReadEventArgs> RowReadAndProcessed;

        #region public methods
        public virtual void LoadProfileSettings(XmlNode xNode)
        {

        }


        public virtual string GetPropertiesAsXml(string nodeName)
        {
            return String.Empty;
        }

        public virtual void Fill()
        {
            CreateTableStructure();
        }

        public virtual void CreateTableStructure()
        {

        }

        public virtual void ProcessRowObject(object row)
        {

        }

        #endregion



        #region protected methods
        protected void SendMessageToCallingApplicationHandler(int lineNumber, string message, object rowData = null)
        {
            RowReadEventArgs lrEventArgs = new RowReadEventArgs();
            lrEventArgs.RowNumber = lineNumber;
            lrEventArgs.Message = message;
            lrEventArgs.RowData = rowData;
            OnLineReadAndProcessed(lrEventArgs);
        }

        protected virtual void OnLineReadAndProcessed(RowReadEventArgs e)
        {
            EventHandler<RowReadEventArgs> handler = RowReadAndProcessed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion    
    }
}
