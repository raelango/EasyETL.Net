using EasyETL.Xml.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLParser : EasyETLEasyField
    {
        public override Type GetClassOf(string className)
        {
            if (EasyETLEnvironment.Parsers.Find(m => m.DisplayName == className) != null) return EasyETLEnvironment.Parsers.Find(m => m.DisplayName == className).Class;
            return null;
        }

        public AbstractEasyParser CreateParser()
        {
            return (AbstractEasyParser)CreateInstance();
        }
    }
}
