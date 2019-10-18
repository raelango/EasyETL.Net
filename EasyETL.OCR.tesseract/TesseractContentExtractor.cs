using EasyETL.Extractors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing;
using EasyETL.Utils;

namespace EasyETL.OCR.Tesseract
{
    [DisplayName("Tesseract OCR")]
    public class TesseractContentExtractor : AbstractContentExtractor
    {
        public override Stream GetStream(Stream inStream)
        {
            string content = String.Empty;
            string tessDataPath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath), "tessdata");
            using (TesseractEngine tesseractEngine = new TesseractEngine(tessDataPath, "eng"))
            {
                using (Image image = Image.FromStream(inStream))
                {
                    using (Bitmap bitmap = new Bitmap(image))
                    {
                        using (Page page = tesseractEngine.Process(bitmap))
                        {
                            content = page.GetText();
                        }

                    }
                }
            }
            return new MemoryStream(Encoding.Default.GetBytes(content));
        }
    }
}
