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

    }
}
