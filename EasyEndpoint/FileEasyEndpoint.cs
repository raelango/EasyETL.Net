using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEndpoint
{
    public class FileEasyEndpoint : IEasyEndpoint
    {
        string FolderName = String.Empty;
        public FileEasyEndpoint(string folderName)
        {
            FolderName = Path.GetFullPath(folderName);
        }

        public bool HasFiles
        {
            get { return true; }
        }

        public bool CanStream
        {
            get { return true; }
        }

        public bool CanRead
        {
            get { return true; }
        }

        public bool CanWrite
        {
            get { return true; }
        }

        public bool CanList
        {
            get { return true; }
        }

        public string[] GetList(string filter = "*.*")
        {
            return Directory.GetFiles(FolderName, filter);
        }

        public Stream GetStream(string fileName)
        {
            string fullFileName = Path.Combine(FolderName, fileName);
            if (File.Exists(fullFileName)) return File.OpenRead(fullFileName);
            return null;
        }

        public byte[] Copy(string fileName)
        {
            string fullFileName = Path.Combine(FolderName, fileName);
            if (File.Exists(fullFileName)) return File.ReadAllBytes(fullFileName);
            return null;            
        }

        public bool Save(string fileName, byte[] data)
        {
            try
            {
                string fullFileName = Path.Combine(FolderName, fileName);
                File.WriteAllBytes(fullFileName, data);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
