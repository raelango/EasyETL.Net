using EasyETL;
using EasyETL.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyDemoActions
{
    [DisplayName("Delete Record")]
    [Description("This action deletes the record(s)")]
    [EasyField("ConfirmationRequired", "If set to True, this would show a message box", "False", "True|False","True;False")]
    public class DeleteRowAction : AbstractEasyAction
    {
        public override void Execute(XmlNode dataNode)
        {
            dataNode.RemoveAll();
        }
        public override void Execute(EasyDynamicObject dataObject)
        {
            dataObject.Properties.Clear();
        }
        public override void Execute(Dictionary<string, string> dataDictionary)
        {
            dataDictionary.Clear();
        }

        public override void Execute(DataRow dataRow)
        {
            dataRow.Delete();
        }

    }
}
