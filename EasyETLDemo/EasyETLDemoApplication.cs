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
            LoadConfigurationDocument();
            foreach (EasyETLClient client in configXmlDocument.Clients)
            {
                TreeNode clientNode = tvClients.Nodes.Add(client.ClientID, client.ClientName);
                TreeNode actionsNode = clientNode.Nodes.Add("actions");
                TreeNode datasourcesNode = clientNode.Nodes.Add("datasources");
                TreeNode exportsNode = clientNode.Nodes.Add("exports");
                TreeNode endpointsNode = clientNode.Nodes.Add("endpoints");
                TreeNode parsersNode = clientNode.Nodes.Add("parsers");
                TreeNode transfersNode = clientNode.Nodes.Add("transfers");
                TreeNode etlsNode = clientNode.Nodes.Add("etls");
                foreach (EasyETLAction action in client.Actions) actionsNode.Nodes.Add(action.ActionID, action.ActionName);
                foreach (EasyETLDatasource datasource in client.Datasources) datasourcesNode.Nodes.Add(datasource.ActionID, datasource.ActionName);
                foreach (EasyETLWriter writer in client.Writers) exportsNode.Nodes.Add(writer.ActionID, writer.ActionName);
                foreach (EasyETLEndpoint endpoint in client.Endpoints) endpointsNode.Nodes.Add(endpoint.ActionID, endpoint.ActionName);
                foreach (EasyETLParser parser in client.Parsers) parsersNode.Nodes.Add(parser.ActionID, parser.ActionName);
                foreach (EasyETLTransfer transfer in client.Transfers) transfersNode.Nodes.Add(transfer.TransferName, transfer.TransferName);
                foreach (EasyETLJobConfiguration job in client.ETLs) etlsNode.Nodes.Add(job.ETLID, job.ETLName);
            }
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


        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
            if (configXmlDocument.GetClientConfiguration(node.Text) != null) configXmlDocument.GetClientConfiguration(node.Text).ClientName = newLabel;
            if (node.FullPath.Contains('\\'))
            {
                string clientName = node.FullPath.Split('\\')[0];
                string nodeCategory = node.FullPath.Split('\\')[1];
                string nodeName = node.FullPath.Split('\\')[2];
                string nodeLabel = newLabel;
                EasyETLClient selectedClientConfiguration = configXmlDocument.GetClientConfiguration(clientName);
                if (selectedClientConfiguration != null)
                {
                    switch (nodeCategory.ToLower())
                    {
                        case "actions":
                            EasyETLAction action = selectedClientConfiguration.Actions.Find(a => a.ActionName == nodeName);
                            if (action == null)
                            {
                                action = new EasyETLAction() { ActionID = newLabel, ActionName = newLabel };
                                selectedClientConfiguration.Actions.Add(action);
                            }
                            action.ActionName = newLabel;
                            break;
                        case "datasources":
                            EasyETLDatasource datasource = selectedClientConfiguration.Datasources.Find(d => d.ActionName == nodeName);
                            if (datasource == null)
                            {
                                datasource = new EasyETLDatasource() { ActionID = newLabel, ActionName = newLabel };
                                selectedClientConfiguration.Datasources.Add(datasource);
                            }
                            datasource.ActionName = newLabel;
                            break;
                        case "exports":
                            EasyETLWriter writer = selectedClientConfiguration.Writers.Find(w => w.ActionName == nodeName);
                            if (writer == null)
                            {
                                writer = new EasyETLWriter() { ActionID = newLabel, ActionName = newLabel };
                                selectedClientConfiguration.Writers.Add(writer);
                            }
                            writer.ActionName = newLabel;
                            break;
                        case "endpoints":
                            EasyETLEndpoint endpoint = selectedClientConfiguration.Endpoints.Find(ep => ep.ActionName == nodeName);
                            if (endpoint == null)
                            {
                                endpoint = new EasyETLEndpoint() { ActionID = newLabel, ActionName = newLabel };
                                selectedClientConfiguration.Endpoints.Add(endpoint);
                            }
                            endpoint.ActionName = newLabel;
                            break;
                        case "parsers":
                            EasyETLParser parser = selectedClientConfiguration.Parsers.Find(p => p.ActionName == nodeName);
                            if (parser == null)
                            {
                                parser = new EasyETLParser() { ActionID = newLabel, ActionName = newLabel };
                                selectedClientConfiguration.Parsers.Add(parser);
                            }
                            parser.ActionName = newLabel;
                            break;
                        case "transfers":
                            EasyETLTransfer transfer = selectedClientConfiguration.Transfers.Find(t => t.TransferName == nodeName);
                            if (transfer == null)
                            {
                                transfer = new EasyETLTransfer() { TransferName = newLabel };
                                selectedClientConfiguration.Transfers.Add(transfer);
                            }
                            transfer.TransferName = newLabel;
                            break;
                        case "etls":
                            EasyETLJobConfiguration etl = selectedClientConfiguration.ETLs.Find(e => e.ETLName == nodeName);
                            if (etl == null)
                            {
                                etl = new EasyETLJobConfiguration() { ETLID = newLabel, ETLName = newLabel };
                                selectedClientConfiguration.ETLs.Add(etl);
                            }
                            etl.ETLName = newLabel;
                            break;
                    }
                }
            }
            configXmlDocument.Save();
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
            if (tvClients.SelectedNode == null) return;
            foreach (TabPage p in MainTablControl.TabPages)
            {
                if (p.Text == tvClients.SelectedNode.FullPath)
                {
                    MainTablControl.SelectedTab = p;
                    return;
                }
            }

            Form currentForm = null;
            if (tvClients.SelectedNode.Parent == null)
            {
                //this is the top level node...
                EasyETLClient selectClientConfiguration = configXmlDocument.GetClientConfiguration(tvClients.SelectedNode.Text);
                if (selectClientConfiguration != null)
                {
                    ClientSettingsConfiguration cForm = new ClientSettingsConfiguration
                    {
                        clientConfiguration = selectClientConfiguration,
                        configDocument = configXmlDocument
                    };
                    cForm.LoadConfiguration();
                    currentForm = cForm;
                }
            }

            if ((tvClients.SelectedNode.Parent != null) && (tvClients.SelectedNode.Parent.Parent != null))
            {
                string clientName = tvClients.SelectedNode.Parent.Parent.Text;
                string nodeName = tvClients.SelectedNode.Text;
                if ("actions;exports;datasources;endpoints;parsers;etls;transfers".Split(';').Contains(tvClients.SelectedNode.Parent.Text))
                {
                    switch (tvClients.SelectedNode.Parent.Text)
                    {
                        case "etls":
                            ETLForm mForm = new ETLForm
                            {
                                ConfigXmlDocument = configXmlDocument,
                                ClientName = clientName,
                                ETLName = nodeName
                            };
                            mForm.LoadControls();
                            mForm.LoadSettingsFromXml();
                            currentForm = mForm;
                            break;
                        case "transfers":
                            currentForm = null;
                            TransferForm tForm = new TransferForm()
                            {
                                ConfigurationDocument = configXmlDocument,
                                ClientName = clientName,
                                TransferName = nodeName
                            };
                            tForm.LoadSettingsFromXml();
                            currentForm = tForm;
                            break;
                        case "actions":
                        case "exports":
                        case "endpoints":
                        case "datasources":
                        case "parsers":
                            ClassConfigurationForm aForm = new ClassConfigurationForm
                            {
                                ClassType = tvClients.SelectedNode.Parent.Text.TrimEnd('s'),
                                ConfigXmlDocument = configXmlDocument,
                                ClientName = clientName,
                                ActionName = nodeName
                            };
                            switch (tvClients.SelectedNode.Parent.Text)
                            {
                                case "actions":
                                    aForm.BaseClassType = typeof(AbstractEasyAction);
                                    aForm.lstClasses = EasyETLEnvironment.Actions;
                                    break;
                                case "exports":
                                    aForm.BaseClassType = typeof(DatasetWriter);
                                    aForm.lstClasses = EasyETLEnvironment.Writers;
                                    break;
                                case "endpoints":
                                    aForm.BaseClassType = typeof(AbstractEasyEndpoint);
                                    aForm.lstClasses = EasyETLEnvironment.Endpoints;
                                    break;
                                case "datasources":
                                    aForm.BaseClassType = typeof(DatasourceEasyParser);
                                    aForm.lstClasses = EasyETLEnvironment.Datasources;
                                    break;
                                case "parsers":
                                    aForm.BaseClassType = typeof(ContentEasyParser);
                                    aForm.lstClasses = EasyETLEnvironment.Parsers;
                                    break;
                                default:
                                    break;
                            }
                            aForm.LoadFormControls();
                            aForm.LoadSettingsFromConfig();
                            //aForm.LoadSettingsFromXml(tvClients.SelectedNode.FullPath);
                            currentForm = aForm;
                            break;

                    }
                }
            }
            if (currentForm != null)
            {
                currentForm.TopLevel = false;
                currentForm.MdiParent = this;
                MainTablControl.TabPages.Add(tvClients.SelectedNode.FullPath);
                TabPage newTabPage = MainTablControl.TabPages[MainTablControl.TabCount - 1];
                newTabPage.Controls.Add(currentForm);
                currentForm.Dock = DockStyle.Fill;
                currentForm.Show();
                currentForm.FormBorderStyle = FormBorderStyle.None;
                MainTablControl.SelectedTab = newTabPage;
            }
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
            switch (e.Node.Level)
            {
                case 0:
                    tvClients.ContextMenuStrip = ClientContextMenuStrip;
                    break;
                case 1:
                    tvClients.ContextMenuStrip = ActionTypeContextMenuStrip;
                    atNewToolStripMenuItem.Text = "Create New " + e.Node.Text.TrimEnd('s');
                    break;
                case 2:
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
                    amMoveTo.Visible = amMoveTo.DropDownItems.Count > 0;

                    //Change text of create new to the relevant action
                    amCreateNew.Text = "Create New " + e.Node.Parent.Text.TrimEnd('s');
                    tvClients.ContextMenuStrip = ActionContextMenuStrip;
                    break;

            }
        }

        private void cmNewClient_Click(object sender, EventArgs e)
        {
            if ((tvClients.SelectedNode == null) || (tvClients.SelectedNode.Parent == null))
            {
                string newPath = "Client_" + tvClients.Nodes.Count + 1;
                EasyETLClient newClient = new EasyETLClient();
                newClient.ClientID = newPath;
                newClient.ClientName = newPath;
                configXmlDocument.Clients.Add(newClient);
                configXmlDocument.Save();
                LoadConfiguration(newPath);
            }
        }

        private void cmClone_Click(object sender, EventArgs e)
        {
            string newPath = "";
            if (!tvClients.SelectedNode.FullPath.Contains('\\'))
            {
                //This is a client
                EasyETLClient client = configXmlDocument.GetClientConfiguration(tvClients.SelectedNode.Text);
                if (client != null)
                {
                    newPath = "Copy Of " + client.ClientName;
                    EasyETLClient newClient = new EasyETLClient();
                    XmlElement clientElement = configXmlDocument.CreateElement("client");
                    client.WriteSettings(clientElement);
                    newClient.ReadSettings(clientElement);
                    newClient.AttributesDictionary = new Dictionary<string, string>(client.AttributesDictionary);
                    newClient.ClientID = newPath;
                    newClient.ClientName = newPath;
                    configXmlDocument.Clients.Add(newClient);
                }
            }
            else
            {
                if (tvClients.SelectedNode.FullPath.Split('\\').Length == 3)
                {
                    //This is leaf node...
                    string clientName = tvClients.SelectedNode.FullPath.Split('\\')[0];
                    string nodeCategory = tvClients.SelectedNode.FullPath.Split('\\')[1];
                    string nodeName = tvClients.SelectedNode.FullPath.Split('\\')[2];
                    string newNodeName = "Copy Of " + nodeName;
                    newPath = clientName + '\\' + nodeCategory + '\\' + newNodeName;
                    EasyETLClient selectedClientConfiguration = configXmlDocument.GetClientConfiguration(clientName);
                    if (selectedClientConfiguration != null)
                    {
                        XmlElement element = null;
                        switch (nodeCategory.ToLower())
                        {
                            case "actions":
                                EasyETLAction action = selectedClientConfiguration.Actions.Find(a => a.ActionName == nodeName);
                                EasyETLAction newAction = new EasyETLAction();
                                element = configXmlDocument.CreateElement("action");
                                action.WriteSettings(element);
                                newAction.ReadSettings(element);
                                newAction.ActionName = newNodeName;
                                newAction.ActionID = newNodeName;
                                selectedClientConfiguration.Actions.Add(newAction);
                                break;
                            case "datasources":
                                EasyETLDatasource datasource = selectedClientConfiguration.Datasources.Find(d => d.ActionName == nodeName);
                                EasyETLDatasource newdatasource = new EasyETLDatasource();
                                element = configXmlDocument.CreateElement("datasource");
                                datasource.WriteSettings(element);
                                newdatasource.ReadSettings(element);
                                newdatasource.ActionName = newNodeName;
                                newdatasource.ActionID = newNodeName;
                                selectedClientConfiguration.Datasources.Add(newdatasource);
                                break;
                            case "exports":
                                EasyETLWriter writer = selectedClientConfiguration.Writers.Find(w => w.ActionName == nodeName);
                                EasyETLWriter newwriter = new EasyETLWriter();
                                element = configXmlDocument.CreateElement("export");
                                writer.WriteSettings(element);
                                newwriter.ReadSettings(element);
                                newwriter.ActionName = newNodeName;
                                newwriter.ActionID = newNodeName;
                                selectedClientConfiguration.Writers.Add(newwriter);
                                break;
                            case "endpoints":
                                EasyETLEndpoint endpoint = selectedClientConfiguration.Endpoints.Find(ep => ep.ActionName == nodeName);
                                EasyETLEndpoint newendpoint = new EasyETLEndpoint();
                                element = configXmlDocument.CreateElement("endpoint");
                                endpoint.WriteSettings(element);
                                newendpoint.ReadSettings(element);
                                newendpoint.ActionName = newNodeName;
                                newendpoint.ActionID = newNodeName;
                                selectedClientConfiguration.Endpoints.Add(newendpoint);
                                break;
                            case "parsers":
                                EasyETLParser parser = selectedClientConfiguration.Parsers.Find(p => p.ActionName == nodeName);
                                EasyETLParser newparser = new EasyETLParser();
                                element = configXmlDocument.CreateElement("parser");
                                parser.WriteSettings(element);
                                newparser.ReadSettings(element);
                                newparser.ActionName = newNodeName;
                                newparser.ActionID = newNodeName;
                                selectedClientConfiguration.Parsers.Add(newparser);
                                break;
                            case "transfers":
                                EasyETLTransfer transfer = selectedClientConfiguration.Transfers.Find(t => t.TransferName == nodeName);
                                EasyETLTransfer newtransfer = new EasyETLTransfer();
                                element = configXmlDocument.CreateElement("transfer");
                                transfer.WriteSettings(element);
                                newtransfer.ReadSettings(element);
                                newtransfer.TransferName = newNodeName;
                                selectedClientConfiguration.Transfers.Add(newtransfer);
                                break;
                            case "etls":
                                EasyETLJobConfiguration etl = selectedClientConfiguration.ETLs.Find(et => et.ETLName == nodeName);
                                EasyETLJobConfiguration newetl = new EasyETLJobConfiguration();
                                element = configXmlDocument.CreateElement("etl");
                                etl.WriteSettings(element);
                                newetl.ReadSettings(element);
                                newetl.ETLID = newNodeName;
                                newetl.ETLName = newNodeName;
                                selectedClientConfiguration.ETLs.Add(newetl);
                                break;
                        }

                    }
                }
            }
            configXmlDocument.Save();
            LoadConfiguration(newPath);
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

                string xPath = "//clients/client[@name='" + node.FullPath + "']";
                if (node.FullPath.Contains('\\'))
                {
                    string clientName = node.FullPath.Split('\\')[0];
                    string nodeCategory = node.FullPath.Split('\\')[1];
                    string nodeName = node.FullPath.Split('\\')[2];
                    xPath = "//clients/client[@name='" + clientName + "']/" + nodeCategory + "/" + nodeCategory.TrimEnd('s') + "[@name='" + nodeName + "']";
                }
                string selectedNodePath = "";
                if (node.NextNode != null) selectedNodePath = node.NextNode.FullPath;
                if (node.PrevNode != null) selectedNodePath = node.PrevNode.FullPath;
                if ((selectedNodePath == "") && (node.Parent != null)) selectedNodePath = node.Parent.FullPath;

                if (!tvClients.SelectedNode.FullPath.Contains('\\'))
                {
                    //This is a client
                    EasyETLClient client = configXmlDocument.GetClientConfiguration(tvClients.SelectedNode.Text);
                    if (client != null) configXmlDocument.Clients.Remove(client);
                    configXmlDocument.Save();
                    LoadConfiguration(selectedNodePath);
                }
                else
                {
                    if (tvClients.SelectedNode.FullPath.Split('\\').Length == 3)
                    {
                        //This is leaf node...
                        string clientName = tvClients.SelectedNode.FullPath.Split('\\')[0];
                        string nodeCategory = tvClients.SelectedNode.FullPath.Split('\\')[1];
                        string nodeName = tvClients.SelectedNode.FullPath.Split('\\')[2];
                        EasyETLClient selectedClientConfiguration = configXmlDocument.GetClientConfiguration(clientName);
                        if (selectedClientConfiguration != null)
                        {
                            switch (nodeCategory.ToLower())
                            {
                                case "actions":
                                    EasyETLAction action = selectedClientConfiguration.Actions.Find(a => a.ActionName == nodeName);
                                    selectedClientConfiguration.Actions.Remove(action);
                                    break;
                                case "datasources":
                                    EasyETLDatasource datasource = selectedClientConfiguration.Datasources.Find(d => d.ActionName == nodeName);
                                    selectedClientConfiguration.Datasources.Remove(datasource);
                                    break;
                                case "exports":
                                    EasyETLWriter writer = selectedClientConfiguration.Writers.Find(w => w.ActionName == nodeName);
                                    selectedClientConfiguration.Writers.Remove(writer);
                                    break;
                                case "endpoints":
                                    EasyETLEndpoint endpoint = selectedClientConfiguration.Endpoints.Find(ep => ep.ActionName == nodeName);
                                    selectedClientConfiguration.Endpoints.Remove(endpoint);
                                    break;
                                case "parsers":
                                    EasyETLParser parser = selectedClientConfiguration.Parsers.Find(p => p.ActionName == nodeName);
                                    selectedClientConfiguration.Parsers.Remove(parser);
                                    break;
                                case "transfers":
                                    EasyETLTransfer transfer = selectedClientConfiguration.Transfers.Find(t => t.TransferName == nodeName);
                                    selectedClientConfiguration.Transfers.Remove(transfer);
                                    break;
                                case "etls":
                                    EasyETLJobConfiguration etl = selectedClientConfiguration.ETLs.Find(et => et.ETLName == nodeName);
                                    selectedClientConfiguration.ETLs.Remove(etl);
                                    break;
                            }
                        }
                        configXmlDocument.Save();
                        LoadConfiguration(selectedNodePath);
                    }
                }
            }
        }

        private void amOpen_Click(object sender, EventArgs e)
        {
            tvClients_DoubleClick(this, null);
        }

        private void amCreateNew_Click(object sender, EventArgs e)
        {
            string newNodeName;
            if ((tvClients.SelectedNode != null) && (tvClients.SelectedNode.Parent != null))
            {
                TreeNode categoryNode = tvClients.SelectedNode;
                if (clientCategories.Split(';').Contains(tvClients.SelectedNode.Parent.Text)) categoryNode = tvClients.SelectedNode.Parent;
                int nodeCount = categoryNode.Nodes.Count + 1;
                newNodeName = categoryNode.Parent.Name + "_" + categoryNode.Text.TrimEnd('s') + "_" + nodeCount.ToString();
                while (categoryNode.Nodes[newNodeName] != null)
                {
                    nodeCount++;
                    newNodeName = categoryNode.Parent.Name + "_" + categoryNode.Text.TrimEnd('s') + "_" + nodeCount.ToString();
                }
            }
            else
            {
                newNodeName = "Client_" + (configXmlDocument.Clients.Count + 1).ToString();
            }
            if (!String.IsNullOrWhiteSpace(newNodeName))
            {
                TreeNode tNode = tvClients.SelectedNode.Nodes.Add(newNodeName, newNodeName);
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
            string selectedPath = selectedClient + '\\' + originalActionType + '\\' + originalActionName; 
            if (!String.IsNullOrWhiteSpace(selectedClient))
            {
                EasyETLClient oldClient = configXmlDocument.GetClientConfiguration(originalClientName);
                EasyETLClient newClient = configXmlDocument.GetClientConfiguration(selectedClient);
                if ((oldClient != null) && (newClient != null))
                {
                    switch (originalActionType.ToLower())
                    {
                        case "actions":
                            EasyETLAction action = oldClient.Actions.Find(a => a.ActionName == originalActionName);
                            oldClient.Actions.Remove(action);
                            newClient.Actions.Add(action);
                            break;
                        case "datasources":
                            EasyETLDatasource datasource = oldClient.Datasources.Find(d => d.ActionName == originalActionName);
                            oldClient.Datasources.Remove(datasource);
                            newClient.Datasources.Add(datasource);
                            break;
                        case "exports":
                            EasyETLWriter writer = oldClient.Writers.Find(w => w.ActionName == originalActionName);
                            oldClient.Writers.Remove(writer);
                            newClient.Writers.Add(writer);
                            break;
                        case "endpoints":
                            EasyETLEndpoint endpoint = oldClient.Endpoints.Find(ep => ep.ActionName == originalActionName);
                            oldClient.Endpoints.Remove(endpoint);
                            newClient.Endpoints.Add(endpoint);
                            break;
                        case "parsers":
                            EasyETLParser parser = oldClient.Parsers.Find(p => p.ActionName == originalActionName);
                            oldClient.Parsers.Remove(parser);
                            newClient.Parsers.Add(parser);
                            break;
                        case "transfers":
                            EasyETLTransfer transfer = oldClient.Transfers.Find(t => t.TransferName == originalActionName);
                            oldClient.Transfers.Remove(transfer);
                            newClient.Transfers.Add(transfer);
                            break;
                        case "etls":
                            EasyETLJobConfiguration etl = oldClient.ETLs.Find(et => et.ETLName == originalActionName);
                            oldClient.ETLs.Remove(etl);
                            newClient.ETLs.Add(etl);
                            break;
                    }
                    configXmlDocument.Save();
                    LoadConfiguration(selectedPath);
                }
            }
        }

        private void EasyETLDemoApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure to close the application ?", "Close Application ?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
        }

        private void cmOpen_Click(object sender, EventArgs e)
        {
            tvClients_DoubleClick(this, null);
        }

        private void EasyETLDemoApplication_Load(object sender, EventArgs e)
        {

        }

        public void LoadConfigurationDocument()
        {
            configXmlDocument = new EasyETLXmlDocument();
            configXmlDocument.Load(xmlFileName);
            configXmlDocument.AddGlobalValue("XmlTemplatePath");
            configXmlDocument.AddGlobalValue("WordTemplatePath");
        }

        private void atNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amCreateNew_Click(this, null);
        }
    }
}
