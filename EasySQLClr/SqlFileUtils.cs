using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasySQLClr
{
    public class SqlFileUtils
    {
        public byte[] GetFileContents(string strFileName, long startPosition = 0, long maxBytes = 0)
        {
            byte[] buff = null;
            if (File.Exists(strFileName))
            {
                FileStream fs = new FileStream(strFileName,
                                               FileMode.Open,
                                               FileAccess.Read);
                fs.Seek(startPosition, SeekOrigin.Begin);
                using (BinaryReader br = new BinaryReader(fs))
                {
                    if (maxBytes == 0)
                    {
                        maxBytes = new FileInfo(strFileName).Length - startPosition;
                    }
                    buff = br.ReadBytes((int)maxBytes);
                }
            }
            return buff;
        }
    }
}
