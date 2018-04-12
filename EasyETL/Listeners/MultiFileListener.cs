using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EasyETL.Listeners
{
    public class MultiFileListener : FolderListener
    {
        Dictionary<string, long> dictFilePositions = new Dictionary<string, long>(StringComparer.CurrentCultureIgnoreCase);
        #region constructors 
        public MultiFileListener(object caller, string folderName, string filter = "*.*", bool monitorSubFolders = false, WatcherChangeTypes monitorChangeTypes = WatcherChangeTypes.All, NotifyFilters notificationFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size)
            : base(caller, folderName, filter, monitorSubFolders, monitorChangeTypes, notificationFilter)
        {
            dictFilePositions = new Dictionary<string, long>(StringComparer.CurrentCultureIgnoreCase);
        }
        #endregion

        public override void SetListenerSpecificData(ListenerTriggeredEventArgs eventArgs)
        {
            base.SetListenerSpecificData(eventArgs);
            string FullFileName = DataToPass["ImpactedFullFileOrFolderName"].ToString();
            if (File.Exists(FullFileName)) {
                if (!dictFilePositions.ContainsKey(FullFileName))
                {
                    dictFilePositions.Add(FullFileName, 0);
                }
                using (FileStream _textReader = new FileStream(FullFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    _textReader.Seek(dictFilePositions[FullFileName], SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(_textReader))
                    {
                        eventArgs.Data["AdditionalContent"] = sr.ReadToEnd();
                        dictFilePositions[FullFileName] = _textReader.Position;
                    }

                }
            }
            else {
                dictFilePositions.Remove(FullFileName);
            }
        }
    }
}
