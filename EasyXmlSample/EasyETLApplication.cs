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
    public partial class EasyETLApplication : Form
    {
        private int childFormNumber = 0;
        private string xmlFileName;
        public EasyETLApplication()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients(string visibleNodeFullpath = "")
        {
            tvClients.Nodes.Clear();
            xmlFileName = Path.Combine(Environment.CurrentDirectory, "clients.xml");
            if (!File.Exists(xmlFileName))
            {
                using (StreamWriter sw = File.CreateText(xmlFileName))
                {
                    sw.WriteLine("<clients></clients>");
                }

            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlFileName);
            AddTreeViewChildNodes(tvClients.Nodes, xDoc.DocumentElement);
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
                if (child_node.Attributes["id"] != null)
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
                if (currentDepth < 2)  AddTreeViewChildNodes(new_node.Nodes, child_node, currentDepth + 1 );

                // If this is a leaf node, make sure it's visible.
                //if (new_node.Nodes.Count == 0) new_node.EnsureVisible();
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            TreeNode tNode = null;
            if ((tvClients.SelectedNode == null) || (tvClients.SelectedNode.Parent == null))
            {
                tNode = tvClients.Nodes.Add((tvClients.Nodes.Count + 1).ToString(), "Client" + (tvClients.Nodes.Count + 1).ToString());
            }
            else
            {
                TreeNode categoryNode = tvClients.SelectedNode;
                if ("endpoints;transfers;etls".Split(';').Contains(tvClients.SelectedNode.Parent.Text)) categoryNode = tvClients.SelectedNode.Parent;
                if (tNode == null) tNode = tvClients.SelectedNode.Nodes.Add(categoryNode.Parent.Name + "_" + categoryNode.Text.TrimEnd('s') + "_" + (categoryNode.Nodes.Count + 1).ToString());
            }
            if (tNode != null)
            {
                SaveXmlFile(tNode, tNode.Text);
                LoadClients(tNode.FullPath);
            }
        }

        private void OpenFile(object sender, EventArgs e)
        {
            tvClients_DoubleClick(this, null);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
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
            XmlNode xNode = xDoc.SelectSingleNode("//clients/client[@id='" + node.Name + "']");
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
                    xNode = xDoc.SelectSingleNode("//clients/client[@name='" + clientName + "']/" + nodeCategory + "/" + nodeCategory.TrimEnd('s') + "[@id='" + node.Name + "']");
                    if (xNode != null)
                    {
                        xNode.Attributes["name"].Value = newLabel;
                    }
                    else
                    {
                        xNode = xDoc.SelectSingleNode("//clients/client[@name='" + clientName + "']/" + nodeCategory);
                        XmlElement xElement = xDoc.CreateElement(nodeCategory.TrimEnd('s'));
                        xElement.SetAttribute("id", nodeName);
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
                    XmlElement epNode = xDoc.CreateElement("endpoints");
                    XmlElement trNode = xDoc.CreateElement("transfers");
                    XmlElement etlNode = xDoc.CreateElement("etls");
                    childNode.AppendChild(epNode);
                    childNode.AppendChild(trNode);
                    childNode.AppendChild(etlNode);

                    xDoc.DocumentElement.AppendChild(childNode);
                }
            }
            xDoc.Save(xmlFileName);
            //LoadClients();
            //throw new NotImplementedException();
        }

        private void tvClients_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            bool allowEdit = false;
            if (e.Node.Parent == null) allowEdit = true;
            if ((!allowEdit) && (e.Node.Nodes != null)) allowEdit = true;
            e.CancelEdit = !allowEdit;
        }

        private void tvClients_DoubleClick(object sender, EventArgs e)
        {

            if ((tvClients.SelectedNode != null) && (tvClients.SelectedNode.Parent != null) && (tvClients.SelectedNode.Parent.Text == "etls") && (tvClients.SelectedNode.Parent.Parent != null))
            {
                foreach (TabPage p in MainTablControl.TabPages)
                {
                    if (p.Text == tvClients.SelectedNode.FullPath)
                    {
                        MainTablControl.SelectedTab = p;
                        return;
                    }
                }
                MainTablControl.TabPages.Add(tvClients.SelectedNode.FullPath);
                TabPage newTabPage = MainTablControl.TabPages[MainTablControl.TabCount - 1];
                MainForm mForm = new MainForm();
                mForm.TopLevel = false;
                mForm.MdiParent = this;
                mForm.FormBorderStyle = FormBorderStyle.None;
                mForm.LoadControls();
                mForm.LoadSettingsFromXml(xmlFileName, tvClients.SelectedNode.FullPath);
                mForm.LoadDataToGridView();
                newTabPage.Controls.Add(mForm);
                mForm.WindowState = FormWindowState.Maximized;
                mForm.Show();
                MainTablControl.SelectedTab = newTabPage;
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (MainTablControl.SelectedTab != null)
            {
                ((MainForm)MainTablControl.SelectedTab.Controls[0]).SaveSettingsToXmlFile();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton_Click(this, null);
        }

    }
}
