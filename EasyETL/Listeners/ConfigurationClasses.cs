using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Listeners
{
    public class MonitorFolderCollection : List<MonitorFolderSettings> { }

    [Serializable]
    public class MonitorFolderSettings 
    {
        public string FolderName {get; set;}
        public List<string> Exclusions = new List<string>();
        public string ProcessorType {get; set;}
        public bool DeleteFileAfterProcessing {get;set;}

    }
}
