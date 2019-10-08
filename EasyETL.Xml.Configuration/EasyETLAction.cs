using EasyETL.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLAction : EasyETLEasyField
    {
        public override Type GetClassOf(string className)
        {
            if (EasyETLEnvironment.Actions.Find(m => m.DisplayName == className) != null) return EasyETLEnvironment.Actions.Find(m => m.DisplayName == className).Class;
            return null;
        }

        public AbstractEasyAction CreateAction()
        {
            return (AbstractEasyAction)CreateInstance();
        }
    }
}
