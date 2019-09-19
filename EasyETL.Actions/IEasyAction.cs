using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Actions
{
    public interface IEasyAction
    {
        bool CanExecute(params Dictionary<string,string>[] dataDictionaries);
        bool CanExecute(params XmlNode[] dataNodes);
        bool CanExecute(params EasyDynamicObject[] dataObjects);
        bool CanExecute(params DataRow[] dataRows);

        bool CanExecute(Dictionary<string, string> dataDictionary);
        bool CanExecute(XmlNode dataNode);
        bool CanExecute(XmlNodeList dataNodes);
        bool CanExecute(EasyDynamicObject dataObject);
        bool CanExecute(DataRow dataRow);

        void Execute(params Dictionary<string, string>[] dataDictionaries);
        void Execute(params XmlNode[] dataNodes);
        void Execute(params EasyDynamicObject[] dataObjects);
        void Execute(params DataRow[] dataRows);

        void Execute(Dictionary<string, string> dataDictionary);
        void Execute(XmlNode dataNode);
        void Execute(XmlNodeList dataNodes);
        void Execute(EasyDynamicObject dataObject);
        void Execute(DataRow dataRow);

    }
}
