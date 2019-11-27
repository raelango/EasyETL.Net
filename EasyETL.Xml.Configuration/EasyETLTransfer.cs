using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public class EasyETLTransfer : EasyETLConfiguration
    {
        public string TransferName;
        public string SourceFilter;
        public string SourceEndpointName;
        public string TargetFilter;
        public string TargetEndpointName;
        public override void ReadSettingsFromDictionary()
        {
            TransferName = GetSetting("Name", "");
            SourceFilter = GetSetting("SourceFilter", "*.*");
            SourceEndpointName = GetSetting("SourceEndpoint", "");
            TargetFilter = GetSetting("TargetFilter", "*.*");
            TargetEndpointName = GetSetting("TargetEndpoint", "");
        }

        public override void WriteSettingsToDictionary()
        {
            base.WriteSettingsToDictionary();
            SetSetting("Name", TransferName);
            SetSetting("SourceFilter", SourceFilter);
            SetSetting("SourceEndpoint", SourceEndpointName);
            SetSetting("TargetFilter",TargetFilter);
            SetSetting("TargetEndpoint", TargetEndpointName);
        }
    }
}
