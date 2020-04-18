using EasyETL.Endpoint;
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
    public partial class EndpointFilesForm : Form
    {
        AbstractEasyEndpoint Endpoint = null;
        public string FileName = String.Empty;
        
        public EndpointFilesForm()
        {
            InitializeComponent();
        }



        public void LoadEndPoint(AbstractFileEasyEndpoint aep, string endpointName)
        {
            Endpoint = aep;
            lblEndpointName.Text = endpointName;
        }

        private void btnLoadList_Click(object sender, EventArgs e)
        {
            lbFiles.Items.Clear();
            if (Endpoint == null) return;
            lbFiles.Items.AddRange(Endpoint.GetList(txtSearchFilter.Text));
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            FileName = lbFiles.Text;
            DialogResult = DialogResult.OK;
        }

        private void txtSearchFilter_Enter(object sender, EventArgs e)
        {
            btnLoadList.NotifyDefault(true);
            this.AcceptButton = btnLoadList;
        }

        private void lbFiles_Enter(object sender, EventArgs e)
        {
            btnSelect.NotifyDefault(true);
            this.AcceptButton = btnSelect;
        }

        private void lbFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnSelect_Click(null, null);
        }
    }
}
