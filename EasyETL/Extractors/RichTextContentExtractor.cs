using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Extractors
{
    [DisplayName("Rich Text (RTF)")]
    public class RichTextContentExtractor : AbstractContentExtractor
    {
        public override Stream GetStream(Stream inStream)
        {
            inStream.Position = 0;
            using (StreamReader sr = new StreamReader(inStream, Encoding.UTF8))
            {
                string contents = sr.ReadToEnd();
                string result = RichTextStripper.StripRichTextFormat(contents);
                return new MemoryStream(Encoding.UTF8.GetBytes(result));
            }
        }
    }
}
