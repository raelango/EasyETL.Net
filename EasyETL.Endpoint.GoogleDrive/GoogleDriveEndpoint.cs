using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.ComponentModel;
using EasyETL.Attributes;


namespace EasyETL.Endpoint.GoogleDrive
{
    [DisplayName("Google Drive")]
    [EasyProperty("CanListen", "True")]
    [EasyField("ApplicationName","Name of the application")]
    [EasyField("Credentials","The JSON format credentials")]
    public class GoogleDriveEndpoint : AbstractFileEasyEndpoint
    {
        public string ApplicationName = String.Empty;
        public string Credentials = String.Empty;
        DriveService GoogleDriveService = null;

        public GoogleDriveEndpoint()
        {

        }

        public GoogleDriveEndpoint(string applicationName)
        {
            ApplicationName = applicationName;
            InitializeGoogleDriveService();
        }

        private void InitializeGoogleDriveService()
        {
            if (GoogleDriveService != null) return;
            string[] Scopes = { DriveService.Scope.DriveReadonly };
            UserCredential credential;
            using (Stream stream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(Credentials)))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            GoogleDriveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.ApplicationName
            });

        }

        #region public overriden methods

        public override bool IsFieldSettingsComplete()
        {
            return base.IsFieldSettingsComplete() && (!String.IsNullOrWhiteSpace(ApplicationName)) && (!String.IsNullOrWhiteSpace(Credentials));
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            InitializeGoogleDriveService();
            return (GoogleDriveService != null);
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "applicationname":
                    ApplicationName = fieldValue; break;
                case "credentials":
                    Credentials = fieldValue; break;
            }
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string,string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("ApplicationName", ApplicationName);
            resultDict.Add("Credentials", Credentials);
            return resultDict;
        }


        public override string[] GetList(string filter = "*.*")
        {
            InitializeGoogleDriveService();
            List<string> fileList = new List<string>();
            if (GoogleDriveService == null) return fileList.ToArray();
            FilesResource.ListRequest lstRequest = GoogleDriveService.Files.List();
            lstRequest.PageSize = null;
            lstRequest.Fields = "nextPageToken, files(id, name)";
            lstRequest.Q = filter;
            IList<Google.Apis.Drive.v3.Data.File> lstFiles = lstRequest.Execute().Files;
            if ((lstFiles != null) && (lstFiles.Count > 0))
            {
                foreach (Google.Apis.Drive.v3.Data.File file in lstFiles)
                {
                    fileList.Add(file.Name);
                }
            }
            return fileList.ToArray();
        }

        public string GetFileID(string fileName)
        {
            Google.Apis.Drive.v3.Data.File file = GetFile(fileName);
            if (file == null) return String.Empty;
            return file.Id;
        }

        public Google.Apis.Drive.v3.Data.File GetFile(string fileName)
        {
            InitializeGoogleDriveService();
            FilesResource.ListRequest lstRequest = GoogleDriveService.Files.List();
            lstRequest.Q = "name=" + fileName;
            lstRequest.PageSize = 1;
            IList<Google.Apis.Drive.v3.Data.File> lstFiles = lstRequest.Execute().Files;
            if ((lstFiles == null) || (lstFiles.Count == 0)) return null;
            return lstFiles.First();
        }

        public override Stream GetStream(string fileName)
        {
            Google.Apis.Drive.v3.Data.File file = GetFile(fileName);
            if (file == null) return null;
            Google.Apis.Drive.v3.FilesResource.GetRequest request = GoogleDriveService.Files.Get(file.Id);
            MemoryStream memoryStream = new MemoryStream();
            request.Download(memoryStream);
            return memoryStream;
        }

        public override byte[] Read(string fileName)
        {
            Stream fileStream = GetStream(fileName);
            if (fileStream != null)
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return Encoding.ASCII.GetBytes(reader.ReadToEnd());
                }
            }
            return null;
        }

        public override bool Write(string fileName, byte[] data)
        {
            try
            {
                if ((!FileExists(fileName)) || Overwrite)
                {
                    string contentType = "image/jpeg";
                    using (MemoryStream memoryStream = new MemoryStream(data))
                    {
                        Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File() { Name = fileName };
                        FilesResource.CreateMediaUpload request = GoogleDriveService.Files.Create(file, memoryStream, contentType);
                        request.Fields = "id";
                        request.Upload();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override bool FileExists(string fileName)
        {
            try
            {
                InitializeGoogleDriveService();
            }
            catch
            {
                return false;
            }
            return false;
        }

        public override bool Delete(string fileName)
        {
            try
            {
                string fileID = GetFileID(fileName);
                if (!String.IsNullOrWhiteSpace(fileID)) GoogleDriveService.Files.Delete(fileID).Execute();
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

    }
}
