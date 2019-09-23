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
        bool HasFiles { get; } //Ports, Databases are not files
        bool CanStream { get; } //Ports -- NetworkStream, files -- FileStream, Database -- No
        bool CanRead { get; }
        bool CanWrite { get; }
        bool CanList { get; }
        bool Overwrite { get; }
        bool CanListen { get; }
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
