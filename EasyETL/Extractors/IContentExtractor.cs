using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Extractors
{
    public interface IContentExtractor
    {
        Stream GetStream(Stream inStream);
        TextReader GetTextReader(TextReader txtReader);
        Stream GetStream(string filename);
        TextReader GetTextReader(string filename);
    }
}
