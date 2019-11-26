using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJobTransformations : EasyETLConfiguration
    {
        public EasyETLJobDuringLoadTransformation DuringLoad = new EasyETLJobDuringLoadTransformation();
        public EasyETLJobAfterLoadTransformation AfterLoad = new EasyETLJobAfterLoadTransformation();
        public EasyETLJobDatasetTransformation Dataset = new EasyETLJobDatasetTransformation();

        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);
            if (xNode.SelectSingleNode("duringload") != null) DuringLoad.ReadSettings(xNode.SelectSingleNode("duringload"));
            if (xNode.SelectSingleNode("afterload") != null) AfterLoad.ReadSettings(xNode.SelectSingleNode("afterload"));
            if (xNode.SelectSingleNode("datasetconversion") != null) Dataset.ReadSettings(xNode.SelectSingleNode("datasetconversion"));
        }

        public override void ReadSettingsFromDictionary()
        {
            DuringLoad.ReadSettingsFromDictionary();
            AfterLoad.ReadSettingsFromDictionary();
            Dataset.ReadSettingsFromDictionary();
        }

        public override void WriteSettingsToDictionary()
        {
            DuringLoad.WriteSettingsToDictionary();
            AfterLoad.WriteSettingsToDictionary();
            Dataset.WriteSettingsToDictionary();
        }

        public override void WriteSettings(XmlNode xNode)
        {
            base.WriteSettings(xNode);
            XmlElement duringLoadNode = xNode.OwnerDocument.CreateElement("duringload");
            XmlElement afterLoadNode = xNode.OwnerDocument.CreateElement("afterload");
            XmlElement datasetNode = xNode.OwnerDocument.CreateElement("datasetconversion");
            xNode.AppendChild(duringLoadNode);
            xNode.AppendChild(afterLoadNode);
            xNode.AppendChild(datasetNode);
            DuringLoad.WriteSettings(duringLoadNode);
            AfterLoad.WriteSettings(afterLoadNode);
            Dataset.WriteSettings(datasetNode);
        }
    }
}
