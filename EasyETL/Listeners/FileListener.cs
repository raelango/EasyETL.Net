using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EasyETL.Listeners
{
    public class FileListener : FolderListener
    {
        FileStream _textReader;

        public FileListener(object caller, string fileName)
            : base(caller, Path.GetDirectoryName(fileName), Path.GetFileName(fileName))
        {
            _textReader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _textReader.Seek(0, SeekOrigin.End);
        }

        public override void SetListenerSpecificData(ListenerTriggeredEventArgs eventArgs)
        {
            base.SetListenerSpecificData(eventArgs);
            eventArgs.Data["AdditionalContent"] = new StreamReader(_textReader).ReadToEnd();
            _textReader.Seek(0, SeekOrigin.End);
        }
    }
}
