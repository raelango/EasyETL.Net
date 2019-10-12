using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic;
using System.IO;
using System.ComponentModel;
using EasyETL.Attributes;

namespace EasyETL.Endpoint
{
    [DisplayName("FTP Server")]
    [EasyProperty("HasFiles", "True")]
    [EasyProperty("CanStream", "True")]
    [EasyProperty("CanRead", "True")]
    [EasyProperty("CanWrite", "True")]
    [EasyProperty("CanList", "True")]
    [EasyProperty("CanListen", "False")]
    [EasyField("FTP Address","FTP Server Address (DNS or IP)")]
    [EasyField("Port Number","Port Number to connect","21","[0-9]+")]
    [EasyField("User ID","Login User Name")]
    [EasyField("User Password","Login Password","","","",true)]
     public class FtpEasyEndpoint : AbstractFileEasyEndpoint, IDisposable
    {
        public FtpClient FTPControl = null;
        public int PortNumber = 21;
        public NetworkCredential Credentials = null;
        public X509CertificateCollection Certificates = null;
        public string FTPAddress = String.Empty;
        public string UserID = String.Empty;
        public string Password = String.Empty;

        public FtpEasyEndpoint()
        {

        }
        public FtpEasyEndpoint(string ftpAddress, string userID, string password, int port = 21, params X509Certificate[] certificates)
        {
            FTPAddress = ftpAddress;
            PortNumber = port;
            if (!String.IsNullOrWhiteSpace(userID) && !String.IsNullOrWhiteSpace(password))
            {
                Credentials = GetCredential(userID, password);
            }
            Certificates = new X509CertificateCollection(certificates);
            EstablishFTPConnection();
        }

        #region public properties
        public string CurrentDirectory
        {
            get
            {
                EstablishFTPConnection();
                return FTPControl.GetWorkingDirectory();
            }
            set { 
                EstablishFTPConnection(); 
                FTPControl.SetWorkingDirectory(value); 
            }
        }
        #endregion

        #region Public overriden methods


        public override string[] GetList(string filter = "*.*")
        {
            List<string> resultList = new List<string>();
            try
            {
                EstablishFTPConnection();
                FtpListItem[] ftpListItems = FTPControl.GetListing();
                foreach (FtpListItem ftpListItem in ftpListItems)
                {
                    string fileName = ftpListItem.Name;
                    if (Operators.LikeString(fileName, filter, CompareMethod.Text))
                    {
                        resultList.Add(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return resultList.ToArray();
        }

        public override Stream GetStream(string fileName)
        {
            try
            {
                return new MemoryStream(Read(fileName));
            }
            catch
            {
                return null;
            }
        }

        public override byte[] Read(string fileName)
        {
            try
            {
                EstablishFTPConnection();
                byte[] contents;
                FTPControl.Download(out contents, fileName);
                return contents;
            }
            catch
            {
                return null;
            }
        }

        public override bool Write(string fileName, byte[] data)
        {
            try
            {
                FtpExists overwriteMode = FtpExists.Skip;
                if (Overwrite) overwriteMode = FtpExists.Overwrite;
                EstablishFTPConnection();
                FTPControl.Upload(data, fileName, overwriteMode);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool FileExists(string fileName)
        {
            EstablishFTPConnection();
            return FTPControl.FileExists(fileName);
        }

        public override bool Delete(string fileName)
        {
            try
            {
                EstablishFTPConnection();
                FTPControl.DeleteFile(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool IsFieldSettingsComplete()
        {
            return !String.IsNullOrWhiteSpace(FTPAddress);
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string,string> settingDict = base.GetSettingsAsDictionary();
            settingDict.Add("FTP Address", FTPAddress);
            settingDict.Add("Port Number", PortNumber.ToString());
            settingDict.Add("User ID", UserID);
            settingDict.Add("User Password", Password);
            return settingDict;
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "ftp address":
                    FTPAddress = fieldValue; break;
                case "port number":
                    PortNumber = Convert.ToInt16(fieldValue); break;
                case "user id":
                    UserID = fieldValue; break;
                case "user password":
                    Password = fieldValue; break;
            }
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            try
            {
                EstablishFTPConnection();
                return FTPControl.IsConnected;
            }
            catch {
                return false;
            }
        }

        #endregion

        #region Private methods
        private void EstablishFTPConnection()
        {
            if ((FTPControl == null) || (!FTPControl.IsConnected))
            {
                if (String.IsNullOrWhiteSpace(UserID) && String.IsNullOrWhiteSpace(Password))
                    Credentials = CredentialCache.DefaultNetworkCredentials;
                else
                    Credentials = GetCredential(UserID, Password);
                FTPControl = new FtpClient(FTPAddress, PortNumber, Credentials);
                if (Certificates !=null) FTPControl.ClientCertificates.AddRange(Certificates);
                FTPControl.Connect();
                FTPControl.DownloadDataType = FtpDataType.Binary;
                FTPControl.UploadDataType = FtpDataType.Binary;
            }
        }
        #endregion

        
        public void Dispose()
        {
            if (FTPControl != null) { FTPControl.Dispose(); }
            FTPControl = null;
            Credentials = null;
            Certificates = null;
            FTPAddress = String.Empty;
        }
    }
}
