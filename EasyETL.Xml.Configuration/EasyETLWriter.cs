using EasyETL.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLWriter : EasyETLEasyField
    {
        public override Type GetClassOf(string className)
        {
            return EasyETLEnvironment.Writers.Find(m => m.DisplayName == className).Class;
        }

        public DatasetWriter CreateWriter()
        {
            return (DatasetWriter)CreateInstance();
        }
    }
}
