using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyXml.Parsers
{
    public static class EasyParser
    {
        public static IEasyParser GetParser(string parserType)
        {
            switch (parserType.ToUpper())
            {
                case "DELIMITED":
                    return new DelimitedEasyParser();
            }
            return null;
        }
    }
}
