using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Endpoint
{
    public class FileEasyEndpoint : AbstractFileEasyEndpoint
    {
        string FolderName = String.Empty;

        public override bool CanListen
        {
            get
            {
                return true;
            }
        }

        public FileEasyEndpoint(string folderName, bool overwriteFiles = true)
        {
            FolderName = Path.GetFullPath(folderName);
            Overwrite = overwriteFiles;
        }

        #region public overriden methods
        public override  string[] GetList(string filter = "*.*")
        {
            return Directory.GetFiles(FolderName, filter).Select(file=>Path.GetFileName(file)).ToArray();
        }

        public override Stream GetStream(string fileName)
        {
            if (File.Exists(FullFileName(fileName))) return File.OpenRead(FullFileName(fileName));
            return null;
        }

        public override byte[] Read(string fileName)
        {
            if (File.Exists(FullFileName(fileName))) return File.ReadAllBytes(FullFileName(fileName));
            return null;            
        }

        public override bool Write(string fileName, byte[] data)
        {
            try
            {
                if (!File.Exists(FullFileName(fileName)) || Overwrite) File.WriteAllBytes(FullFileName(fileName), data);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override bool FileExists(string fileName)
        {
            return File.Exists(FullFileName(fileName));
        }

        public override bool Delete(string fileName)
        {
            try
            {
                File.Delete(FullFileName(fileName));
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        public string FullFileName(string fileName)
        {
            return Path.Combine(FolderName, fileName);
        }

    }
}
