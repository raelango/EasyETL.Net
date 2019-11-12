using EasyETL.Actions;
using EasyETL.Endpoint;
using EasyETL.Writers;
using EasyETL.Xml.Configuration;
using EasyETL.Xml.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EasyXmlSample
{
    public partial class EasyETLDemoApplication : Form
    {
        private string xmlFileName;
        private EasyETLXmlDocument configXmlDocument = null;
        private string clientCategories = "actions;datasources;exports;endpoints;parsers;transfers;etls";
        public EasyETLDemoApplication()
        {
            InitializeComponent();
            LoadConfiguration();
        }

        public void FocusClientsTreeView()
        {
            tvClients.Focus();
        }

        private void LoadConfiguration(string visibleNodeFullpath = "")
        {
            tvClients.Nodes.Clear();
            xmlFileName = Path.Combine(Environment.CurrentDirectory, Properties.Settings.Default.ConfigurationFilePath);
            if (!File.Exists(xmlFileName))
            {
                using (StreamWriter sw = File.CreateText(xmlFileName))
                {
                    sw.WriteLine("<config><clients></clients><transforms></transforms><profiles></profiles></config>");
                }

            }
            configXmlDocument = new EasyETLXmlDocument();
            configXmlDocument.Load(xmlFileName);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlFileName);
            AddTreeViewChildNodes(tvClients.Nodes, xDoc.SelectSingleNode("//clients"));
            xDoc = null;
            if (!String.IsNullOrWhiteSpace(visibleNodeFullpath))
            {
                TreeNodeCollection parentNodes = tvClients.Nodes;
                TreeNode tn = null;
                foreach (string nodePath in visibleNodeFullpath.Split('\\'))
                {
                    if (tn != null) parentNodes = tn.Nodes;
                    foreach (TreeNode subtreeNode in parentNodes)
                    {
                        if (subtreeNode.Text.Equals(nodePath)) tn = subtreeNode;
                    }
                }
                if (tn != null)
                {
                    tn.EnsureVisible();
                    tvClients.SelectedNode = tn;
                }
            }
        }

        // Add the children of this XML node 
        // to this child nodes collection.
        private void AddTreeViewChildNodes(
            TreeNodeCollection parent_nodes, XmlNode xml_node, int currentDepth = 0)
        {
            foreach (XmlNode child_node in xml_node.ChildNodes)
            {
                // Make the new TreeView node.
                string clientid = child_node.Name;
                string clientname = child_node.Name;
                if ((child_node.Attributes != null) && (child_node.Attributes["id"] != null))
                {
                    clientid = child_node.Attributes["id"].Value;
                    clientname = child_node.Attributes["name"].Value;
                }
                else
                {
                    if (child_node.ParentNode.Name == "client")
                    {
                        clientid = child_node.Name + "_" + child_node.ParentNode.Attributes["id"].Value;
                    }
                    clientname = child_node.Name;
                }
                TreeNode new_node = parent_nodes.Add(clientid, clientname);

                // Recursively make this node's descendants.
                if (currentDepth < 2) AddTreeViewChildNodes(new_node.Nodes, child_node, currentDepth + 1);

                // If this is a leaf node, make sure it's visible.
                //if (new_node.Nodes.Count == 0) new_node.EnsureVisible();
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            cmNewClient_Click(this, null);
        }

        private void OpenFile(object sender, EventArgs e)
        {
            tvClients_DoubleClick(this, null);
        }

       
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void tvClients_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.CancelEdit) return;
            if (String.IsNullOrWhiteSpace(e.Label)) return;
            if (e.Node.Text != e.Label)
            {
                SaveXmlFile(e.Node, e.Label);
            }
        }

        private void SaveXmlFile(TreeNode node, string newLabel)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlFileName);
            XmlNode xNode = xDoc.SelectSingleNode("//clients/client[@name='" + node.Text + "']");
            if (xNode != null)
            {
                xNode.Attributes["name"].Value = newLabel;
            }
            else
            {
                if (node.FullPath.Contains('\\'))
                {
                    string clientName = node.FullPath.Split('\\')[0];
                    string nodeCategory = node.FullPath.Split('\\')[1];
                    string nodeName = node.FullPath.Split('\\')[2];
                    string nodeLabel = newLabel;
                    xNode = xDoc.SelectSingleNode("//clients/client[@name='" + clientName + "']/" + nodeCategory + "/" + nodeCategory.TrimEnd('s') + "[@name='" + node.Name + "']");
                    if (xNode != null)
                    {
                        xNode.Attributes["name"].Value = newLabel;
                    }
                    else
                    {
                        xNode = xDoc.SelectSingleNode("//clients/client[@name='" + clientName + "']/" + nodeCategory);
                        XmlElement xElement = xDoc.CreateElement(nodeCategory.TrimEnd('s'));
                        xElement.SetAttribute("id", nodeLabel);
                        xElement.SetAttribute("name", nodeLabel);
                        xNode.AppendChild(xElement);
                    }
                }
                else
                {
                    string nodeType = "client";
                    XmlElement childNode = xDoc.CreateElement(nodeType);
                    childNode.SetAttribute("id", node.Name);
                    childNode.SetAttribute("name", node.Text);
                    foreach (string clientCategory in clientCategories.Split(';'))
                    {
                        XmlElement ccNode = xDoc.CreateElement(clientCategory);
                        childNode.AppendChild(ccNode);
                    }

                    xDoc.SelectSingleNode("//clients").AppendChild(childNode);
                }
            }
            xDoc.Save(xmlFileName);
            configXmlDocument = new EasyETLXmlDocument();
            configXmlDocument.Load(xmlFileName);
        }

        private void tvClients_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            e.CancelEdit = !CanAllowEdit(selectedNode);
        }

        private bool CanAllowEdit(TreeNode selectedNode)
        {
            bool allowEdit = false;
            if (selectedNode.Parent == null) allowEdit = true;
            if ((!allowEdit) && (selectedNode.Nodes.Count == 0) && (selectedNode.Parent != null) && (selectedNode.Parent.Parent != null)) allowEdit = true;
            if (allowEdit)
            {
                foreach (TabPage p in MainTablControl.TabPages)
                {
                    if (p.Text == tvClients.SelectedNode.FullPath)
                    {
                        MessageBox.Show("This configuration is open.  Please close window before attempting to rename.");
                        allowEdit = false;
                        break;
                    }
                }
            }
            return allowEdit;
        }

        private void tvClients_DoubleClick(object sender, EventArgs e)
        {
            if ((tvClients.SelectedNode != null) && (tvClients.SelectedNode.Parent != null) && (tvClients.SelectedNode.Parent.Parent != null))
            {
                foreach (TabPage p in MainTablControl.TabPages)
                {
                    if (p.Text == tvClients.SelectedNode.FullPath)
                    {
                        MainTablControl.SelectedTab = p;
                        return;
                    }
                }
                string clientName = tvClients.SelectedNode.Parent.Parent.Text;
                string nodeName = tvClients.SelectedNode.Text;
                if ("actions;exports;datasources;endpoints;parsers;etls;transfers".Split(';').Contains(tvClients.SelectedNode.Parent.Text))
                {
                    MainTablControl.TabPages.Add(tvClients.SelectedNode.FullPath);
                    TabPage newTabPage = MainTablControl.TabPages[MainTablControl.TabCount - 1];
                    Form currentForm = null;
                    switch (tvClients.SelectedNode.Parent.Text)
                    {
                        case "etls":
                            ETLForm mForm = new ETLForm();
                            mForm.SettingsFileName = xmlFileName;
                            mForm.LoadControls();
                            mForm.LoadSettingsFromXml(tvClients.SelectedNode.FullPath);
                            currentForm = mForm;
                            break;
                        case "transfers":
                            TransferForm tForm = new TransferForm();
                            tForm.XmlFileName = xmlFileName;
                            tForm.LoadSettingsFromXml(configXmlDocument.SelectSingleNode("//clients/client[@name='" + clientName + "']/transfers/transfer[@name='" + nodeName + "']"));
                            currentForm = tForm;
                            break;
                        case "actions":
                        case "exports":
                        case "endpoints":
                        case "datasources":
                        case "parsers":
                            ClassConfigurationForm aForm = new ClassConfigurationForm();
                            aForm.ClassType = tvClients.SelectedNode.Parent.Text.TrimEnd('s');
                            switch (tvClients.SelectedNode.Parent.Text)
                            {
                                case "actions":
                                    aForm.BaseClassType = typeof(AbstractEasyAction);
                                    break;
                                case "exports":
                                    aForm.BaseClassType = typeof(DatasetWriter);
                                    break;
                                case "endpoints":
                                    aForm.BaseClassType = typeof(AbstractEasyEndpoint);
                                    break;
                                case "datasources":
                                    aForm.BaseClassType = typeof(DatasourceEasyParser);
                                    break;
                                case "parsers":
                                    aForm.BaseClassType = typeof(ContentEasyParser);
                                    break;
                                default:
                                    break;
                            }
                            aForm.SettingsFileName = xmlFileName;
                            aForm.LoadFormControls();
                            aForm.LoadSettingsFromXml(tvClients.SelectedNode.FullPath);
                            currentForm = aForm;
                            break;

                    }
                    if (currentForm != null)
                    {
                        currentForm.TopLevel = false;
                        currentForm.MdiParent = this;
                        newTabPage.Controls.Add(currentForm);
                        currentForm.Dock = DockStyle.Fill;
                        currentForm.Show();
                        currentForm.FormBorderStyle = FormBorderStyle.None;
                        MainTablControl.SelectedTab = newTabPage;
                    }
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (MainTablControl.SelectedTab != null)
            {
                ((ETLForm)MainTablControl.SelectedTab.Controls[0]).SaveSettingsToXmlFile();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton_Click(this, null);
        }

        private void tvClients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Delete)
            {

            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cmDelete_Click(this, null);
        }

        private void renameStripMenuItem_Click(object sender, EventArgs e)
        {
            cmRename_Click(this, null);
        }

        private void RenameToolStripButton_Click(object sender, EventArgs e)
        {
            renameStripMenuItem_Click(this, null);
        }

        private void DeleteToolStripButton_Click(object sender, EventArgs e)
        {
            DeleteToolStripMenuItem_Click(this, null);
        }

        private void CloseToolStripButton_Click(object sender, EventArgs e)
        {
            CloseToolStripMenuItem_Click(this, null);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amClose_Click(this, null);
        }

        private void ExitToolStripButton_Click(object sender, EventArgs e)
        {
            ExitToolsStripMenuItem_Click(this, null);
        }

        private void MoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amMoveTo_DropDownItemClicked(this, null);
        }

        private void MoveToolStripButton_Click(object sender, EventArgs e)
        {
            MoveToolStripMenuItem_Click(this, null);
        }

        private void CloneToolStripButton_Click(object sender, EventArgs e)
        {
            CloneToolStripMenuItem_Click(this, null);
        }

        private void CloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cmClone_Click(this, null);
        }

        private void tvClients_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        private void tvClients_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tvClients.ContextMenu = null;
            if (e.Node.Level == 0) tvClients.ContextMenuStrip = ClientContextMenuStrip;
            if (e.Node.Level == 2)
            {
                //this is a action level node... let us initialize and assign the ActionContextMenuStrip

                //Let us populate the move to client list
                amMoveTo.DropDownItems.Clear();
                string CurrentClient = tvClients.SelectedNode.Parent.Parent.Text;
                string CurrentActionType = tvClients.SelectedNode.Parent.Text;
                foreach (TreeNode clientNode in tvClients.Nodes)
                {

                    if (clientNode.Text != CurrentClient)
                    {
                        TreeNode actionTypeNode = null;
                        if (clientNode.Nodes.Cast<TreeNode>().Where(r => r.Text == CurrentActionType).Count() > 0) actionTypeNode = clientNode.Nodes.Cast<TreeNode>().Where(r => r.Text == CurrentActionType).ToArray()[0];
                        if (actionTypeNode != null)
                        {
                            if (actionTypeNode.Nodes.Cast<TreeNode>().Where(r => r.Text == tvClients.SelectedNode.Text).Count() == 0)
                                amMoveTo.DropDownItems.Add(clientNode.Text);
                        }

                    }
                }

                //Change text of create new to the relevant action
                amCreateNew.Text = "Create New " + e.Node.Parent.Text;
                tvClients.ContextMenuStrip = ActionContextMenuStrip;
            }
        }

        private void cmNewClient_Click(object sender, EventArgs e)
        {
            TreeNode tNode = null;
            int nodeCount = 0;
            if ((tvClients.SelectedNode == null) || (tvClients.SelectedNode.Parent == null))
            {
                nodeCount = tvClients.Nodes.Count + 1;
                while (tvClients.Nodes[nodeCount.ToString()] != null) nodeCount++;
                tNode = tvClients.Nodes.Add(nodeCount.ToString(), "Client" + nodeCount.ToString());
                SaveXmlFile(tNode, tNode.Text);
                LoadConfiguration(tNode.FullPath);
            }
        }

        private void cmClone_Click(object sender, EventArgs e)
        {
            if (tvClients.SelectedNode != null)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlFileName);
                TreeNode node = tvClients.SelectedNode;
                string xPath = "//clients/client[@name='" + node.FullPath + "']";

                string clientName = node.FullPath;
                string nodeCategory = "";
                string nodeName = node.Text;
                string newNodeName = "Copy of " + nodeName;
                string newNodePath = newNodeName;
                if (node.FullPath.Contains('\\'))
                {
                    clientName = node.FullPath.Split('\\')[0];
                    nodeCategory = node.FullPath.Split('\\')[1];
                    nodeName = node.FullPath.Split('\\')[2];
                    xPath = "//clients/client[@name='" + clientName + "']/" + nodeCategory + "/" + nodeCategory.TrimEnd('s') + "[@name='" + nodeName + "']";
                    newNodePath = clientName + '\\' + nodeCategory + '\\' + newNodeName;
                }
                XmlNode xNode = xDoc.SelectSingleNode(xPath);
                XmlNode cloneNode = xNode.CloneNode(true);
                cloneNode.Attributes.GetNamedItem("name").Value = newNodeName;
                cloneNode.Attributes.GetNamedItem("id").Value = newNodeName;
                xNode.ParentNode.AppendChild(cloneNode);
                xDoc.Save(xmlFileName);
                LoadConfiguration(newNodePath);
            }
        }

        private void cmRename_Click(object sender, EventArgs e)
        {
            if ((tvClients.SelectedNode != null) && (CanAllowEdit(tvClients.SelectedNode))) tvClients.SelectedNode.BeginEdit();
        }

        private void cmDelete_Click(object sender, EventArgs e)
        {
            bool allowDelete = (tvClients.SelectedNode != null) && ((tvClients.SelectedNode.Parent == null) || (tvClients.SelectedNode.Nodes.Count == 0));
            TreeNode node = tvClients.SelectedNode;
            if (allowDelete)
            {
                foreach (TabPage p in MainTablControl.TabPages)
                {
                    if (p.Text == node.FullPath)
                    {
                        MessageBox.Show("This configuration is open.  Please close window before attempting to delete the node.");
                        allowDelete = false;
                        break;
                    }
                }
            }

            if ((allowDelete) && (MessageBox.Show("Deleted nodes cannot be recovered.  Are you sure to delete ?", "Deleting Configuration", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlFileName);
                string xPath = "//clients/client[@name='" + node.FullPath + "']";
                if (node.FullPath.Contains('\\'))
                {
                    string clientName = node.FullPath.Split('\\')[0];
                    string nodeCategory = node.FullPath.Split('\\')[1];
                    string nodeName = node.FullPath.Split('\\')[2];
                    xPath = "//clients/client[@name='" + clientName + "']/" + nodeCategory + "/" + nodeCategory.TrimEnd('s') + "[@name='" + node.Text + "']";
                }
                string selectedNodePath = "";
                if (node.NextNode != null) selectedNodePath = node.NextNode.FullPath;
                if (node.PrevNode != null) selectedNodePath = node.PrevNode.FullPath;
                XmlNode xNode = xDoc.SelectSingleNode(xPath);
                if (xNode != null)
                {
                    xNode.ParentNode.RemoveChild(xNode);
                    xDoc.Save(xmlFileName);
                    LoadConfiguration(selectedNodePath);
                }
            }
        }

        private void amOpen_Click(object sender, EventArgs e)
        {
            tvClients_DoubleClick(this, null);
        }

        private void amCreateNew_Click(object sender, EventArgs e)
        {
            TreeNode tNode = null;
            int nodeCount = 0;
            if ((tvClients.SelectedNode != null) && (tvClients.SelectedNode.Parent != null))
            {
                TreeNode categoryNode = tvClients.SelectedNode;
                if (clientCategories.Split(';').Contains(tvClients.SelectedNode.Parent.Text)) categoryNode = tvClients.SelectedNode.Parent;
                nodeCount = categoryNode.Nodes.Count + 1;
                string newNodeName = categoryNode.Parent.Name + "_" + categoryNode.Text.TrimEnd('s') + "_" + nodeCount.ToString();
                while (categoryNode.Nodes[newNodeName] != null)
                {
                    nodeCount++;
                    newNodeName = categoryNode.Parent.Name + "_" + categoryNode.Text.TrimEnd('s') + "_" + nodeCount.ToString();
                }
                tNode = tvClients.SelectedNode.Nodes.Add(newNodeName, newNodeName);
                SaveXmlFile(tNode, tNode.Text);
                LoadConfiguration(tNode.FullPath);
            }
        }

        private void amClone_Click(object sender, EventArgs e)
        {
            cmClone_Click(this, null);
        }


        private void amRename_Click(object sender, EventArgs e)
        {
            cmRename_Click(this, null);
        }

        private void amDelete_Click(object sender, EventArgs e)
        {
            cmDelete_Click(this, null);
        }

        private void amClose_Click(object sender, EventArgs e)
        {
            if (MainTablControl.SelectedTab != null)
            {
                MainTablControl.TabPages.Remove(MainTablControl.SelectedTab);
                FocusClientsTreeView();
            }
        }

        private void amMoveTo_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e == null) return;
            string selectedClient = e.ClickedItem.Text;
            string originalPath = tvClients.SelectedNode.FullPath;
            string originalClientName = originalPath.Split('\\')[0];
            string originalActionType = originalPath.Split('\\')[1];
            string originalActionName = originalPath.Split('\\')[2];
            if (!String.IsNullOrWhiteSpace(selectedClient))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlFileName);

                XmlNode xNode = xDoc.SelectSingleNode("//clients/client[@name='" + originalClientName + "']/" + originalActionType + "/" + originalActionType.Trim('s') + "[@name='" + originalActionName + "']");
                if (xNode != null)
                {
                    XmlNode newParentNode = xDoc.SelectSingleNode("//clients/client[@name='" + selectedClient + "']/" + originalActionType);
                    if (newParentNode != null)
                    {
                        newParentNode.AppendChild(xNode);
                    }
                }
                xDoc.Save(xmlFileName);
                LoadConfiguration(selectedClient + '\\' + originalPath.Split('\\')[1] + '\\' + originalPath.Split('\\')[2]);
            }
        }
    }
}
