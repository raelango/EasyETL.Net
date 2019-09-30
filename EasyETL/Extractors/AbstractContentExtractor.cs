using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Extractors
{
    public abstract class AbstractContentExtractor : IContentExtractor
    {
        public virtual Stream GetStream(Stream inStream)
        {
            throw new NotImplementedException();
        }

        public virtual TextReader GetTextReader(TextReader txtReader)
        {
            return new StreamReader(GetStream(new MemoryStream((Encoding.Default.GetBytes(txtReader.ReadToEnd())))));
        }

        public virtual Stream GetStream(string filename)
        {
            return GetStream(new FileStream(filename, FileMode.Open));
        }

        public virtual TextReader GetTextReader(string filename)
        {
            return GetTextReader(File.OpenText(filename));
        }
    }
}
