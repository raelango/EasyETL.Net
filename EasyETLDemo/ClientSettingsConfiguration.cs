using EasyETL.Xml.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyXmlSample
{
    public partial class ClientSettingsConfiguration : Form
    {

        public EasyETLClient clientConfiguration;
        public EasyETLXmlDocument configDocument;
        public ClientSettingsConfiguration()
        {
            InitializeComponent();
        }

        public void LoadConfiguration()
        {
            lstAvailableFields.Items.Clear();
            lstAvailableFields.Items.AddRange(configDocument.GlobalValues.Keys.ToArray());

            dgConfiguration.Rows.Clear();
            foreach (KeyValuePair<string, string> keyValuePair in clientConfiguration.AttributesDictionary)
            {
                if (configDocument.GlobalValues.ContainsKey(keyValuePair.Key))
                {
                    int dgvIndex = dgConfiguration.Rows.Add();
                    dgConfiguration.Rows[dgvIndex].Cells[0].Value = keyValuePair.Key;
                    dgConfiguration.Rows[dgvIndex].Cells[1].Value = configDocument.GlobalValues[keyValuePair.Key];
                    dgConfiguration.Rows[dgvIndex].Cells[2].Value = keyValuePair.Value;
                    lstAvailableFields.Items.Remove(keyValuePair.Key);
                }
            }
        }

        private void lstAvailableFields_DoubleClick(object sender, EventArgs e)
        {
            if ((lstAvailableFields.SelectedItem != null) && (!String.IsNullOrWhiteSpace(lstAvailableFields.SelectedItem.ToString())))
            {
                string selectedItem = lstAvailableFields.SelectedItem.ToString();
                int dgvIndex = dgConfiguration.Rows.Add();
                dgConfiguration.Rows[dgvIndex].Cells[0].Value = selectedItem;
                dgConfiguration.Rows[dgvIndex].Cells[1].Value = configDocument.GlobalValues[selectedItem];
                dgConfiguration.Rows[dgvIndex].Cells[2].Value = "";
                lstAvailableFields.Items.Remove(selectedItem);
            }
        }

        private void dgConfiguration_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            lstAvailableFields.Items.Add(e.Row.Cells[0].Value);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if ((this.Parent != null) && (this.Parent is TabPage))
            {
                TabPage tpage = (TabPage)this.Parent;
                TabControl tcontrol = (TabControl)tpage.Parent;
                EasyETLDemoApplication app = ((EasyETLDemoApplication)(tcontrol.FindForm()));
                tcontrol.TabPages.Remove(tpage);
                app.FocusClientsTreeView();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (string strConfig in lstAvailableFields.Items)
            {
                if (clientConfiguration.AttributesDictionary.ContainsKey(strConfig)) clientConfiguration.AttributesDictionary.Remove(strConfig);
            }
            foreach (DataGridViewRow dgvRow in dgConfiguration.Rows)
            {
                clientConfiguration.SetSetting(dgvRow.Cells[0].Value.ToString(), dgvRow.Cells[2].Value.ToString());
            }
            btnClose_Click(this, null);
        }
    }
}
