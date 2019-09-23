using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Endpoint
{
    public abstract class AbstractFileEasyEndpoint : AbstractEasyEndpoint
    {
        public override bool Overwrite { get; set; }

        public override bool HasFiles
        {
            get { return true; }
        }

        public override bool CanStream
        {
            get { return true; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanList
        {
            get { return true; }
        }

    }
}
