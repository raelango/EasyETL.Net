using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public interface IEasyETLConfiguration
    {
        void ReadSettings(XmlNode xNode);
        void WriteSettings(XmlNode xNode);
    }
}
