using EasyETL;
using EasyETL.Endpoint;
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
using System.Xml;

namespace EasyXmlSample
{
    public partial class TransferForm : Form
    {
        AbstractEasyEndpoint SourceEndpoint = null;
        AbstractEasyEndpoint TargetEndpoint = null;
        public XmlDocument configXmlDocument = null;
        public EasyETLXmlDocument ConfigurationDocument = null;
        public EasyETLClient ClientConfiguration = null;
        public EasyETLTransfer TransferConfiguration = null;
        public string ClientName = "";
        public string TransferName = "";
        List<string> lstEndpoints = new List<string>();
        public TransferForm()
        {
            InitializeComponent();
        }

        public void LoadSettingsFromXml()
        {
            ClientConfiguration = ConfigurationDocument.Clients.Find(c => c.ClientName == ClientName);
            TransferConfiguration = ClientConfiguration.Transfers.Find(t => t.TransferName == TransferName);
            LoadAllEndPoints();
            cmbSource.Text = TransferConfiguration.SourceEndpointName;
            cmbDestination.Text = TransferConfiguration.TargetEndpointName;
            txtSourceFilter.Text = TransferConfiguration.SourceFilter;
            txtDestinationFilter.Text = TransferConfiguration.TargetFilter;
            SetSourceEndpoint();
            SetDestinationEndpoint();
            PopulateSourceList();
            PopulateTargetList();
        }

        public void LoadAllEndPoints()
        {
            lstEndpoints.Clear();
            cmbDestination.Items.Clear();
            cmbSource.Items.Clear();
            foreach (EasyETLEndpoint easyETLEndpoint in ClientConfiguration.Endpoints)
            {
                lstEndpoints.Add(easyETLEndpoint.ActionName);
            }
            PopulateSourceEndPoint();
            PopulateDestinationEndPoint();
        }

        public void PopulateSourceEndPoint()
        {
            foreach (string endpoint in lstEndpoints)
            {
                if (!cmbSource.Items.Contains(endpoint)) cmbSource.Items.Add(endpoint);
            }
            if ((cmbSource.SelectedItem == null) && (cmbSource.Items.Count > 0)) cmbSource.SelectedIndex = 0;
        }

        public void PopulateDestinationEndPoint()
        {
            foreach (string endpoint in lstEndpoints)
            {
                if ((cmbSource.SelectedItem != null) && (cmbSource.SelectedItem.ToString().Equals(endpoint)))
                {
                    cmbDestination.Items.Remove(endpoint);
                }
                else
                {
                    if (!cmbDestination.Items.Contains(endpoint)) cmbDestination.Items.Add(endpoint);
                }
            }
            if ((cmbDestination.SelectedItem == null) && (cmbDestination.Items.Count > 0)) cmbDestination.SelectedIndex = 0;
        }

        private void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSourceEndpoint();
            PopulateDestinationEndPoint();
        }

        private void SetSourceEndpoint()
        {
            SourceEndpoint = GetEndPoint(cmbSource.Text);
            PopulateSourceList();
        }

        private void PopulateSourceList()
        {
            if (SourceEndpoint != null)
            {
                lstSourceFileList.Items.Clear();
                lstSourceFileList.Items.AddRange(SourceEndpoint.GetList(txtSourceFilter.Text));
            }
        }

        private void cmbDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDestinationEndpoint();
            PopulateSourceEndPoint();
        }

        private void SetDestinationEndpoint()
        {
            TargetEndpoint = GetEndPoint(cmbDestination.Text);
            PopulateTargetList();
        }

        private void PopulateTargetList()
        {
            if (TargetEndpoint != null)
            {
                lstDestinationFileList.Items.Clear();
                lstDestinationFileList.Items.AddRange(TargetEndpoint.GetList(txtDestinationFilter.Text));

            }
        }

        private AbstractEasyEndpoint GetEndPoint(string endpointName)
        {
            return ClientConfiguration.Endpoints.Find(e => e.ActionName == endpointName).CreateEndpoint();
        }

        private void lstSourceFileList_SelectedValueChanged(object sender, EventArgs e)
        {
            btnTransfer.Enabled = (lstSourceFileList.SelectedItems.Count > 0) && (SourceEndpoint != null) && (TargetEndpoint != null);
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if ((SourceEndpoint != null) && (TargetEndpoint != null))
            {
                foreach (string fileName in lstSourceFileList.SelectedItems)
                {
                    SourceEndpoint.CopyTo(TargetEndpoint, fileName);
                }
                PopulateTargetList();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TransferConfiguration.SourceFilter = txtSourceFilter.Text;
            TransferConfiguration.TargetFilter = txtDestinationFilter.Text;
            TransferConfiguration.SourceEndpointName = cmbSource.Text;
            TransferConfiguration.TargetEndpointName = cmbDestination.Text;
            ConfigurationDocument.Save();
            btnClose_Click(this, null);
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

        private void txtSourceFilter_Leave(object sender, EventArgs e)
        {
            PopulateSourceList();
        }

        private void txtDestinationFilter_Leave(object sender, EventArgs e)
        {
            PopulateTargetList();
        }
    }
}
