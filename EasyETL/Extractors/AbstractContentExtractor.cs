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
        public string FileName = String.Empty;
        public virtual Stream GetStream(Stream inStream)
        {
            throw new NotImplementedException();
        }

        public virtual TextReader GetTextReader(TextReader txtReader)
        {

            MemoryStream ms = new MemoryStream((Encoding.Default.GetBytes(txtReader.ReadToEnd())));
            return new StreamReader(ms);
        }

        public virtual Stream GetStream(string filename)
        {
            FileName = filename;
            using (FileStream fs = new FileStream(filename,FileMode.Open))
            {
                return GetStream(fs);
            }
        }

        public virtual TextReader GetTextReader(string filename)
        {
            FileName = filename;
            using (StreamReader sr = File.OpenText(filename))
            {
                return GetTextReader(sr);
            }
        }
    }
}
