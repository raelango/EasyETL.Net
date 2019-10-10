using EasyETL.Xml.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLDatasource : EasyETLEasyField
    {
        public override Type GetClassOf(string className)
        {
            if (EasyETLEnvironment.Datasources.Find(m => m.DisplayName == className) != null) return EasyETLEnvironment.Datasources.Find(m => m.DisplayName == className).Class;
            return null;
        }

        public DatasourceEasyParser CreateDatasource()
        {
            return (DatasourceEasyParser)CreateInstance();
        }
    }
}
