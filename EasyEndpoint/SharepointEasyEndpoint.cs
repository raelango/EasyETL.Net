using SP = Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Security;
using System.Net;

namespace EasyEndpoint
{
    public class SharepointEasyEndpoint : AbstractFileEasyEndpoint, IDisposable
    {

        string SiteName = String.Empty;
        ICredentials Credentials = null;
        string LibraryName = String.Empty;
        SP.ClientContext _clientContext = null;
        SP.Web _web = null;
        SP.Site _site = null;

        public SharepointEasyEndpoint(string siteName, string userName, string password, string libraryName, bool overwriteFiles = true)
        {
            SiteName = siteName;
            if (userName.Contains('@'))
            {
                Credentials = new SP.SharePointOnlineCredentials(userName, GetSecureString(password));
            }
            else
            {
                Credentials = GetCredential(userName, password);
            }

            LibraryName = libraryName;
            Overwrite = overwriteFiles;
        }

        #region Public overriden methods
        public override string[] GetList(string filter = "*.*")
        {
            List<string> resultList = new List<string>();
            try
            {
                SP.ListItemCollection spListItems = SharepointList.GetItems(SP.CamlQuery.CreateAllItemsQuery(20, "Title"));
                SharepointClientContext.Load(spListItems);
                SharepointClientContext.ExecuteQuery();
                foreach (SP.ListItem spListItem in spListItems)
                {
                    string fileName = spListItem.FieldValues["FileLeafRef"].ToString();
                    if (Operators.LikeString(fileName, filter, CompareMethod.Text))
                    {
                        resultList.Add(fileName);
                    }
                }
            }
            catch
            {
            }
            return resultList.ToArray();
        }

        public override Stream GetStream(string fileName)
        {
            try
            {
                SP.File spFile;
                if (TryGetFileByServerRelativeUrl(_web, _web.ServerRelativeUrl + "/" + LibraryName + "/" + fileName, out spFile))
                {
                    SharepointClientContext.Load(spFile);
                    SharepointClientContext.ExecuteQuery();
                    SP.FileInformation fileInfo = SP.File.OpenBinaryDirect(_clientContext, spFile.ServerRelativeUrl);
                    return fileInfo.Stream;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public override byte[] Read(string fileName)
        {
            try
            {
                Stream stream = GetStream(fileName);
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
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
                SharepointClientContext.Load(SharepointList.RootFolder);
                SharepointClientContext.ExecuteQuery();

                string fileUrl = String.Format("{0}/{1}", SharepointList.RootFolder.ServerRelativeUrl, fileName);
                Microsoft.SharePoint.Client.File.SaveBinaryDirect(_clientContext, fileUrl, new MemoryStream(data, false), true);
                _clientContext.ExecuteQuery(); //Uploaded .. but still checked out...


                //Load the FieldCollection from the list...
                SP.FieldCollection fileFields = SharepointList.Fields;
                _clientContext.Load(fileFields);
                _clientContext.ExecuteQuery();

                SP.File uploadedFile = _web.GetFileByServerRelativeUrl(fileUrl);
                SP.ListItem newFileListItem = uploadedFile.ListItemAllFields;
                newFileListItem.Update();
                _clientContext.ExecuteQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool FileExists(string fileName)
        {
            SP.File spFile;
            return TryGetFileByServerRelativeUrl(_web, _web.ServerRelativeUrl + "/" + LibraryName + "/" + fileName, out spFile);
        }

        public override bool Delete(string fileName)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        #endregion

        #region Private Methods

        private static bool TryGetFileByServerRelativeUrl(SP.Web web, string serverRelativeUrl, out SP.File file)
        {
            var ctx = web.Context;
            try
            {
                file = web.GetFileByServerRelativeUrl(serverRelativeUrl);
                ctx.Load(file);
                ctx.ExecuteQuery();
                return file.Exists;
            }
            catch
            {
                file = null;
                return false;
            }
        }

        #endregion

        #region Sharepoint specific properties
        public SP.ClientContext SharepointClientContext
        {
            get
            {
                if (_clientContext == null)
                {
                    //Initialize and load the Client Context...
                    _clientContext = new SP.ClientContext(SiteName);
                    _clientContext.Credentials = Credentials;
                    _clientContext.ExecuteQuery();

                    //Load the Web
                    _web = _clientContext.Web;
                    _clientContext.Load(_web);
                    _clientContext.ExecuteQuery();

                    //Load the Site....
                    _site = _clientContext.Site;
                    _clientContext.Load(_site);
                    _clientContext.ExecuteQuery();
                }
                return _clientContext;
            }
        }

        public SP.Web SharepointWeb
        {
            get
            {
                if (_web == null) _web = SharepointClientContext.Web;
                return _web;
            }
        }

        public SP.Site SharepointSite
        {
            get
            {
                if (_site == null) _site = SharepointClientContext.Site;
                return _site;
            }
        }

        public SP.List SharepointList
        {
            get
            {
                try
                {
                    SP.ListCollection spLists = SharepointWeb.Lists;
                    SharepointClientContext.Load(spLists);
                    SharepointClientContext.ExecuteQuery();

                    SP.List spList = spLists.GetByTitle(LibraryName);
                    SharepointClientContext.Load(spList);
                    SharepointClientContext.ExecuteQuery();
                    return spList;
                }
                catch
                {
                    return null;
                }
            }
        }
        
        #endregion

        public void Dispose()
        {
            if (_site != null) _site = null;
            if (_web != null) _web = null;
            if (_clientContext != null) _clientContext = null; 
            Credentials = null;
        }
    }
}
