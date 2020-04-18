using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLJobDatasource : EasyETLConfiguration
    {
        public string SourceType;
        public bool UseTextExtractor;
        public string TextExtractor;
        public bool HasMaxRows;
        public int MaxRows =0;
        public string FileName;
        public string DataSource;
        public string Endpoint;
        //The below properties are child nodes
        public string TextContents;
        public string Query;


        public override void ReadSettings(XmlNode xNode)
        {
            base.ReadSettings(xNode);
            TextContents = "";
            if (xNode.SelectSingleNode("textcontents") != null) TextContents = xNode.SelectSingleNode("textcontents").InnerText;
            Query = "";
            if (xNode.SelectSingleNode("query") != null) Query = xNode.SelectSingleNode("query").InnerText;
        }

        public override void ReadSettingsFromDictionary()
        {
            SourceType = GetSetting("SourceType");
            UseTextExtractor = Convert.ToBoolean(GetSetting("UseTextExtractor", "False"));
            TextExtractor = GetSetting("TextExtractor");
            HasMaxRows = Convert.ToBoolean(GetSetting("HasMaxRows", "True"));
            MaxRows = Convert.ToInt32(GetSetting("MaxRows", "20"));
            FileName = GetSetting("FileName");
            DataSource = GetSetting("Datasource");
            Endpoint = GetSetting("Endpoint");
        }

        public override void WriteSettingsToDictionary()
        {
            SetSetting("SourceType", SourceType);
            SetSetting("UseTextExtractor", UseTextExtractor.ToString());
            SetSetting("TextExtractor", TextExtractor);
            SetSetting("HasMaxRows", HasMaxRows.ToString());
            SetSetting("MaxRows", MaxRows.ToString());
            SetSetting("FileName", FileName);
            SetSetting("DataSource", DataSource);
            SetSetting("Endpoint", Endpoint);
        }

        public override void WriteSettings(XmlNode xNode)
        {
            
            base.WriteSettings(xNode);
            XmlElement tcElement = xNode.OwnerDocument.CreateElement("textcontents");
            tcElement.InnerText = TextContents;
            xNode.AppendChild(tcElement);

            XmlElement qElement = xNode.OwnerDocument.CreateElement("query");
            qElement.InnerText = Query;
            xNode.AppendChild(qElement);
        }

    }
}
