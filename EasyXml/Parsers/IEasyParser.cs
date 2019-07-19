using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyXml.Parsers
{
    public interface IEasyParser
    {
        XmlDocument Load(Stream inStream, XmlDocument xDoc = null, XslCompiledTransform xslt = null);
        XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null, XslCompiledTransform xslt = null);
        XmlDocument Load(string filename, XmlDocument xDoc = null, XslCompiledTransform xslt = null);
        XmlDocument LoadStr(string strToLoad, XmlDocument xDoc = null, XslCompiledTransform xslt = null);
    }
}
