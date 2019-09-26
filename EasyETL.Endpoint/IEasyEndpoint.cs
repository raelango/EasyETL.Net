using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Endpoint
{
    public interface IEasyEndpoint
    {
        #region Properties
        bool Overwrite { get; }
        #endregion

        #region Methods
        Stream GetStream(string fileName);
        string[] GetList(string filter);
        byte[] Read(string fileName);
        void CopyTo(IEasyEndpoint destEasyEndPoint, params string[] fileNames);
        bool Write(string fileName, byte[] data);
        bool FileExists(string fileName);
        bool Delete(string fileName);
        #endregion

    }
}
