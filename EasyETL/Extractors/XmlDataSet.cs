using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.DataSets
{
    public class XmlDataSet : RegexDataSet
    {
        public override void Fill()
        {
            if (TextFile == null)
                throw new ApplicationException("No stream available to convert to a DataSet");

            SendMessageToCallingApplicationHandler(0, "Loading Xml Contents");
            using (XmlTextReader reader = new XmlTextReader(TextFile))
            {
                this.ReadXml(reader);
            }
        }

    }
}
