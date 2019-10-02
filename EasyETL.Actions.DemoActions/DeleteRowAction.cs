using EasyETL;
using EasyETL.Actions;
using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EasyDemoActions
{
    [DisplayName("Delete Record")]
    [Description("This action deletes the record(s)")]
    [EasyField("ConfirmationRequired", "If set to True, this would show a message box", "False", "True|False","True;False")]
    public class DeleteRowAction : AbstractEasyAction
    {
        public override void Execute(params DataRow[] dataRows)
        {
            if (SettingsDictionary.ContainsKey("ConfirmationRequired") && (Convert.ToBoolean(SettingsDictionary["ConfirmationRequired"])))
            {
                if (MessageBox.Show("Should I proceed with Deleting " + dataRows.Length + " records ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            base.Execute(dataRows);
        }

        public override void Execute(params Dictionary<string, string>[] dataDictionaries)
        {
            if (SettingsDictionary.ContainsKey("ConfirmationRequired") && (Convert.ToBoolean(SettingsDictionary["ConfirmationRequired"])))
            {
                if (MessageBox.Show("Should I proceed with Deleting " + dataDictionaries.Length + " records ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            base.Execute(dataDictionaries);
        }

        public override void Execute(params EasyDynamicObject[] dataObjects)
        {
            if (SettingsDictionary.ContainsKey("ConfirmationRequired") && (Convert.ToBoolean(SettingsDictionary["ConfirmationRequired"])))
            {
                if (MessageBox.Show("Should I proceed with Deleting " + dataObjects.Length + " records ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            base.Execute(dataObjects);
        }

        public override void Execute(params XmlNode[] dataNodes)
        {
            if (SettingsDictionary.ContainsKey("ConfirmationRequired") && (Convert.ToBoolean(SettingsDictionary["ConfirmationRequired"])))
            {
                if (MessageBox.Show("Should I proceed with Deleting " + dataNodes.Length + " records ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            base.Execute(dataNodes);
        }

        public override void Execute(XmlNodeList dataNodes)
        {
            if (SettingsDictionary.ContainsKey("ConfirmationRequired") && (Convert.ToBoolean(SettingsDictionary["ConfirmationRequired"])))
            {
                if (MessageBox.Show("Should I proceed with Deleting " + dataNodes.Count + " records ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            base.Execute(dataNodes);
        }



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
