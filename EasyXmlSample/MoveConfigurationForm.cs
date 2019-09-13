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
    public partial class MoveConfigurationForm : Form
    {
        public string SettingsFileName;
        public string OriginalPath;

        public MoveConfigurationForm()
        {
            InitializeComponent();
        }

        public void LoadClients(string currentClientName)
        {
            lstClients.Items.Clear();
            if (File.Exists(SettingsFileName))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(SettingsFileName);
                XmlNodeList clientList = xDoc.SelectNodes("//clients/client");
                foreach (XmlNode clientNode in clientList)
                {
                    string clientName = clientNode.Attributes.GetNamedItem("name").Value;
                    if (!clientName.Equals(currentClientName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        lstClients.Items.Add(clientName);
                    }
                }
            }
            lstClients.ClearSelected();
        }

        public void SetMoveLabel(string moveLabelText)
        {
            lblMove.Text = moveLabelText;
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(lstClients.SelectedItem.ToString()))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(SettingsFileName);

                XmlNode xNode = xDoc.SelectSingleNode("//clients/client[@name='" + OriginalPath.Split('\\')[0] + "']/" + OriginalPath.Split('\\')[1] + "/" + OriginalPath.Split('\\')[1].Trim('s') + "[@name='" + OriginalPath.Split('\\')[2] + "']");
                if (xNode != null)
                {
                    XmlNode newParentNode = xDoc.SelectSingleNode("//clients/client[@name='" + lstClients.SelectedItem.ToString() + "']/" + OriginalPath.Split('\\')[1]);
                    if (newParentNode != null) newParentNode.AppendChild(xNode);
                }
                xDoc.Save(SettingsFileName);
                this.Close();
            }
        }
    }
}
