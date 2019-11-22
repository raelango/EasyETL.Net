using EasyETL.Endpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLEndpoint : EasyETLEasyField
    {
        public override Type GetClassOf(string className)
        {
            if (EasyETLEnvironment.Endpoints.Find(m => m.DisplayName == className) !=null) return EasyETLEnvironment.Endpoints.Find(m => m.DisplayName == className).Class;
            return null;
        }

        public AbstractEasyEndpoint CreateEndpoint()
        {
            return (AbstractEasyEndpoint)CreateInstance();
        }

    }
}
