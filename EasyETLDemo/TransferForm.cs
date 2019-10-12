using EasyETL;
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
using System.Xml;

namespace EasyXmlSample
{
    public partial class TransferForm : Form
    {
        AbstractEasyEndpoint SourceEndpoint = null;
        AbstractEasyEndpoint TargetEndpoint = null;
        XmlDocument configXmlDocument = null;
        XmlNode configNode = null;
        public string XmlFileName;
        string clientName = "";
        string transferName = "";
        List<string> lstEndpoints = new List<string>();
        public TransferForm()
        {
            InitializeComponent();
        }

        public void LoadSettingsFromXml(XmlNode xmlNode)
        {
            configNode = xmlNode;
            configXmlDocument = xmlNode.OwnerDocument;
            clientName = xmlNode.ParentNode.ParentNode.Attributes.GetNamedItem("name").Value;
            transferName = xmlNode.Attributes.GetNamedItem("name").Value;
            LoadAllEndPoints();
            foreach (XmlAttribute xAttr in xmlNode.Attributes) {
                switch (xAttr.Name.ToLower())
                {
                    case "sourceendpoint":
                        if (cmbSource.Items.Contains(xAttr.Value)) cmbSource.Text = xAttr.Value;
                        SetSourceEndpoint();
                        break;
                    case "targetendpoint":
                        if (cmbDestination.Items.Contains(xAttr.Value)) cmbDestination.Text = xAttr.Value;
                        SetDestinationEndpoint();
                        break;
                    case "sourcefilter":
                        txtSourceFilter.Text = xAttr.Value; break;
                    case "targetfilter":
                        txtDestinationFilter.Text = xAttr.Value; break;
                }
            }
            PopulateSourceList();
            PopulateTargetList();
        }

        public void LoadAllEndPoints()
        {
            lstEndpoints.Clear();
            cmbDestination.Items.Clear();
            cmbSource.Items.Clear();
            XmlNode endpointsNode = configXmlDocument.SelectSingleNode("//clients/client[@name='" + clientName + "']/endpoints");
            if (endpointsNode == null) return;
            foreach (XmlNode endpointNode in endpointsNode)
            {
                if (endpointNode.Attributes.GetNamedItem("name") != null) lstEndpoints.Add(endpointNode.Attributes.GetNamedItem("name").Value);
            }
            PopulateSourceEndPoint();
            PopulateDestinationEndPoint();
        }

        public void PopulateSourceEndPoint() {
            foreach (string endpoint in lstEndpoints)
            {
                //if ((cmbDestination.SelectedItem != null) && (cmbDestination.SelectedItem.ToString().Equals(endpoint))) {
                //    cmbSource.Items.Remove(endpoint);
                //}
                //else
                //{
                    if (!cmbSource.Items.Contains(endpoint)) cmbSource.Items.Add(endpoint);
                //}
            }
            if ((cmbSource.SelectedItem == null) && (cmbSource.Items.Count >0)) cmbSource.SelectedIndex = 0;
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
            AbstractFileEasyEndpoint endpoint = null;
            XmlNode endpointNode = configXmlDocument.SelectSingleNode("//clients/client[@name='" + clientName + "']/endpoints/endpoint[@name='" + endpointName + "']");
            if (endpointNode != null) {

                ClassMapping[] endpointClasses = ReflectionUtils.LoadClassesFromLibrary(typeof(AbstractFileEasyEndpoint));
                Type classType = endpointClasses.First(f => f.DisplayName == endpointNode.Attributes.GetNamedItem("classname").Value).Class;
                endpoint = (AbstractFileEasyEndpoint)Activator.CreateInstance(classType);
                foreach (XmlNode childNode in endpointNode.SelectNodes("field"))
                {
                    endpoint.LoadSetting(childNode.Attributes.GetNamedItem("name").Value, childNode.Attributes.GetNamedItem("value").Value);
                }
            }
            return endpoint;
        }

        private void lstSourceFileList_SelectedValueChanged(object sender, EventArgs e)
        {
            btnTransfer.Enabled = (lstSourceFileList.SelectedItems.Count > 0) && (SourceEndpoint != null) && (TargetEndpoint !=null);
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if ((SourceEndpoint !=null) && (TargetEndpoint !=null)) {
                foreach (string fileName in lstSourceFileList.SelectedItems) {
                    SourceEndpoint.CopyTo(TargetEndpoint, fileName);
                }
                PopulateTargetList();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(XmlFileName)) return;
            if (configNode == null) return;
            configXmlDocument = new XmlDocument();
            configXmlDocument.Load(XmlFileName);
            configNode = configXmlDocument.SelectSingleNode("//clients/client[@name='" + clientName + "']/transfers/transfer[@name='" + transferName + "']");
            SetConfiguration("sourceendpoint",cmbSource.Text);
            SetConfiguration("sourcefilter", txtSourceFilter.Text);
            SetConfiguration("targetendpoint", cmbDestination.Text);
            SetConfiguration("targetfilter", txtDestinationFilter.Text);
            configXmlDocument.Save(XmlFileName);
            btnClose_Click(this, null);
        }

        private void SetConfiguration(string attrName, string attrValue)
        {
            attrName = attrName.ToLower();
            XmlAttribute xAttr = (XmlAttribute)configNode.Attributes.GetNamedItem(attrName);
            if (xAttr == null)
            {
                xAttr = configXmlDocument.CreateAttribute(attrName);
                configNode.Attributes.Append(xAttr);
            }
            xAttr.Value = attrValue;
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
