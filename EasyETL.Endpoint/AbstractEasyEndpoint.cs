using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Endpoint
{
    public abstract class AbstractEasyEndpoint : IEasyEndpoint
    {
        public virtual bool HasFiles
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool CanStream
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool CanList
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool CanListen
        {
            get { throw new NotImplementedException();  }
        }

        public virtual bool Overwrite
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual Stream GetStream(string fileName)
        {
            throw new NotImplementedException();
        }

        public virtual string[] GetList(string filter)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] Read(string fileName)
        {
            throw new NotImplementedException();
        }

        public virtual void CopyTo(IEasyEndpoint destEasyEndPoint, params string[] fileNames)
        {
            if (!destEasyEndPoint.CanWrite) return;
            if (!HasFiles) return;
            foreach (string strFileName in fileNames)
            {
                byte[] fileBytes = Read(strFileName);
                if (fileBytes != null) destEasyEndPoint.Write(strFileName, fileBytes);
            }
        }

        public virtual bool Write(string fileName, byte[] data)
        {
            throw new NotImplementedException();
        }

        public virtual bool FileExists(string fileName)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(string fileName)
        {
            throw new NotImplementedException();
        }

        public SecureString GetSecureString(string inputStr)
        {

            SecureString secureString = new SecureString();
            foreach (char c in inputStr)
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }

        public NetworkCredential GetCredential(string userName, string password)
        {
            if (!String.IsNullOrWhiteSpace(userName) && !String.IsNullOrWhiteSpace(password))
            {
                SecureString sPassword = GetSecureString(password);
                string strDomain = ".";
                if (userName.Contains('\\'))
                {
                    strDomain = userName.Split('\\')[0];
                    userName = userName.Split('\\')[1];
                }
                //if (userName.Contains('@'))
                //{
                //    strDomain = userName.Split('@')[1];
                //    userName = userName.Split('@')[0];
                //}
                return new NetworkCredential(userName, sPassword, strDomain);
            }
            return null;
        }
    }
}
