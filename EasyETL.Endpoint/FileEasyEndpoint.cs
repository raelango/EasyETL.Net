using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Endpoint
{
    [DisplayName("Local Network")]
    [EasyProperty("HasFiles","True")]
    [EasyProperty("CanStream", "True")]
    [EasyProperty("CanRead", "True")]
    [EasyProperty("CanWrite", "True")]
    [EasyProperty("CanList", "True")]
    [EasyProperty("CanListen", "True")]
    [EasyField("FolderName","Name of the Folder")]
    [EasyField("Overwrite", "Can Overwrite files if already present ?","False","True|False","True;False")]
    public class FileEasyEndpoint : AbstractFileEasyEndpoint
    {
        string FolderName = String.Empty;

        public FileEasyEndpoint()
        {
            Overwrite = true;
        }

        public FileEasyEndpoint(string folderName, bool overwriteFiles = true)
        {
            FolderName = Path.GetFullPath(folderName);
            Overwrite = overwriteFiles;
        }

        #region public overriden methods
        public override string[] GetList(string filter = "*.*")
        {
            return Directory.Exists(FolderName) ? Directory.GetFiles(FolderName, filter).Select(file => Path.GetFileName(file)).ToArray() : Array.Empty<string>();
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


        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "foldername":
                    FolderName = fieldValue; break;
                case "overwrite":
                    Overwrite = Convert.ToBoolean(fieldValue);break;
            }
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string,string> settingsDict = base.GetSettingsAsDictionary();
            settingsDict.Add("foldername", FolderName);
            return settingsDict;
        }

        public override bool IsFieldSettingsComplete()
        {
            return (!String.IsNullOrWhiteSpace(FolderName));
        }

        #endregion

        public string FullFileName(string fileName)
        {
            return Path.Combine(FolderName, fileName);
        }

        public override bool CanFunction()
        {
            return IsFieldSettingsComplete() && Directory.Exists(FolderName);
        }
    }
}
