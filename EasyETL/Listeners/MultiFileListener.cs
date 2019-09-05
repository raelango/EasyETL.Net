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
        public Dictionary<string, long> FilePositions = new Dictionary<string, long>(StringComparer.CurrentCultureIgnoreCase);
        #region constructors 
        public MultiFileListener(object caller, string folderName, string filter = "*.*", bool monitorSubFolders = false, WatcherChangeTypes monitorChangeTypes = WatcherChangeTypes.All, NotifyFilters notificationFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size)
            : base(caller, folderName, filter, monitorSubFolders, monitorChangeTypes, notificationFilter)
        {
            FilePositions = new Dictionary<string, long>(StringComparer.CurrentCultureIgnoreCase);
        }
        #endregion

        public override void SetListenerSpecificData(ListenerTriggeredEventArgs eventArgs)
        {
            base.SetListenerSpecificData(eventArgs);
            string FullFileName = DataToPass["ImpactedFullFileOrFolderName"].ToString();
            if (File.Exists(FullFileName)) {
                if (!FilePositions.ContainsKey(FullFileName))
                {
                    FilePositions.Add(FullFileName, 0);
                }
                using (FileStream _textReader = new FileStream(FullFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    _textReader.Seek(FilePositions[FullFileName], SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(_textReader))
                    {
                        eventArgs.Data["AdditionalContent"] = sr.ReadToEnd();
                        FilePositions[FullFileName] = _textReader.Position;
                    }
                }
            }
            else {
                FilePositions.Remove(FullFileName);
            }
        }
    }
}
