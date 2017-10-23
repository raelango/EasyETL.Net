using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EasyETL.Listeners
{
    public class FolderListener : JobListener
    {
        public NotifyFilters NotificationFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
        public WatcherChangeTypes MonitorChangeTypes = WatcherChangeTypes.All;
        public string Filter = "*.*";
        public bool SubFolders = false;

        FileSystemWatcher _folderWatcher = null;

        #region constructors
        public FolderListener(object caller, string folderName, string filter = "*.*", bool monitorSubFolders = false, WatcherChangeTypes monitorChangeTypes = WatcherChangeTypes.All, NotifyFilters notificationFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size)
            : base(caller)
        {
            _folderWatcher = new FileSystemWatcher(folderName, filter);
            _folderWatcher.IncludeSubdirectories = monitorSubFolders;
            MonitorChangeTypes = monitorChangeTypes;
            NotificationFilter = notificationFilter;

        }
        #endregion

        public override bool StartOperations()
        {
            if (base.StartOperations())
            {

                _folderWatcher.NotifyFilter = NotificationFilter;

                if (MonitorChangeTypes.HasFlag(WatcherChangeTypes.Created))
                    _folderWatcher.Created += ChangeReceived;

                if (MonitorChangeTypes.HasFlag(WatcherChangeTypes.Renamed))
                    _folderWatcher.Renamed += ChangeReceived;

                if (MonitorChangeTypes.HasFlag(WatcherChangeTypes.Deleted))
                    _folderWatcher.Deleted += ChangeReceived;

                if (MonitorChangeTypes.HasFlag(WatcherChangeTypes.Changed))
                    _folderWatcher.Changed += ChangeReceived;                

                _folderWatcher.EnableRaisingEvents = true;
                return true;
            }
            return false;
        }

        public void ChangeReceived(object sender, FileSystemEventArgs e)
        {
            DataToPass["ImpactedFullFileOrFolderName"] = e.FullPath;
            DataToPass["NatureOfChange"] = e.ChangeType.ToString();
            DataToPass["ImpactedFileOrFolderName"] = e.Name;
            DataToPass["TimeOfEvent"] = DateTime.Now;
            TriggerEvent();
        }

        public override bool StopOperations()
        {
            if (base.StopOperations())
            {
                _folderWatcher.EnableRaisingEvents = false;
                _folderWatcher.Created -= ChangeReceived;
                _folderWatcher.Renamed -= ChangeReceived;
                _folderWatcher.Deleted -= ChangeReceived;
                _folderWatcher.Changed -= ChangeReceived;
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_folderWatcher != null)
                {
                    _folderWatcher.Dispose();
                    _folderWatcher.EnableRaisingEvents = false;
                }
                _folderWatcher = null;
            }
        }
    }
}
