using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEndpoint
{
    interface IEasyEndpoint
    {
        bool HasFiles { get; } //Ports, Databases are not files
        bool CanStream { get; } //Ports -- NetworkStream, files -- FileStream, Database -- No
        bool CanRead { get; }
        bool CanWrite { get;  }
        bool CanList { get;  }

        Stream GetStream(string fileName);
        string[] GetList(string filter);
        byte[] Copy(string fileName);
        bool Save(string fileName, byte[] data);
    }
}
