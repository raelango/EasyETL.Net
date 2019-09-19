using EasyETL;
using EasyETL.Actions;
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
    public partial class ActionForm : Form
    {
        public string SettingsPath = "";
        public string SettingsFileName = "";
        public string ActionsFolder = "";


        public List<string> lstLibraries = new List<string>();
        public List<ClassMapping> lstClasses = new List<ClassMapping>();
        public ClassMapping SelectedClassMapping = null;
        public EasyFieldAttribute SelectedFieldAttribute = null;
        public Dictionary<string, string> SelectedClassSettings = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        public void LoadFormControls()
        {
            ActionsFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Actions");
            lstLibraries =  new List<string>(ReflectionUtils.LoadAllLibrariesWithClass(ActionsFolder,typeof(AbstractEasyAction)));
            cmbLibraryName.Items.Clear();
            cmbLibraryName.Items.AddRange(lstLibraries.ToArray());
        }

        public void LoadSettingsFromXml(string settingsPath)
        {
            if (String.IsNullOrWhiteSpace(SettingsFileName)) return;
            if (!File.Exists(SettingsFileName)) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);

            string clientName = settingsPath.Split('\\')[0];
            string actionName = settingsPath.Split('\\')[2];
            SettingsPath = "//clients/client[@name='" + clientName + "']/actions/action[@name='" + actionName + "']";
            XmlNode xNode = xDoc.SelectSingleNode(SettingsPath);
            if (xNode != null)
            {
                foreach (XmlAttribute xAttr in xNode.Attributes)
                {
                    switch (xAttr.Name.ToLower())
                    {
                        case "name":
                            txtActionName.Text = xAttr.Value; break;
                        case "libraryname":
                            cmbLibraryName.SelectedItem = xAttr.Value; break;
                        case "classname":
                            cmbClassName.SelectedItem = xAttr.Value; break;
                    }
                }

                SelectedClassSettings.Clear();
                XmlNodeList fieldNodeList = xNode.SelectNodes("field");
                foreach (XmlNode fieldNode in fieldNodeList)
                {
                    SelectedClassSettings.Add(fieldNode.Attributes["name"].Value, fieldNode.Attributes["value"].Value);
                }

            }

        }
        
        public ActionForm()
        {
            InitializeComponent();
        }

        private void btnSaveAction_Click(object sender, EventArgs e)
        {
            SaveSettingsToXmlFile();
        }

        public void SaveSettingsToXmlFile()
        {
            if (String.IsNullOrWhiteSpace(SettingsFileName)) return;
            if (!File.Exists(SettingsFileName)) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNode xNode = xDoc.SelectSingleNode(SettingsPath);
            xNode.InnerText = "";
            xNode.Attributes.RemoveNamedItem("libraryname");
            XmlAttribute libraryAttr = xDoc.CreateAttribute("libraryname");
            libraryAttr.Value = cmbLibraryName.SelectedItem.ToString();

            xNode.Attributes.Append(libraryAttr);

            XmlAttribute classAttr = xDoc.CreateAttribute("classname");
            classAttr.Value = cmbClassName.SelectedItem.ToString();
            xNode.Attributes.Append(classAttr);

            if (SelectedClassSettings != null)
            {
                foreach (KeyValuePair<string, string> kvPair in SelectedClassSettings)
                {
                    if (!String.IsNullOrWhiteSpace(kvPair.Value))
                    {
                        XmlElement fieldNode = xDoc.CreateElement("field");
                        fieldNode.SetAttribute("name",kvPair.Key);
                        fieldNode.SetAttribute("value",kvPair.Value);
                        xNode.AppendChild(fieldNode);
                    }
                }
            }

            xDoc.Save(SettingsFileName);
        }

        private void btnCloseWindow_Click(object sender, EventArgs e)
        {
            if ((this.Parent != null) && (this.Parent is TabPage))
            {
                TabPage tpage = (TabPage)this.Parent;
                TabControl tcontrol = (TabControl)tpage.Parent;
                tcontrol.TabPages.Remove(tpage);
            }
        }

        private void cmbLibraryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbClassName.Items.Clear();
            lstClasses = new List<ClassMapping>(ReflectionUtils.LoadClassesFromLibrary(Path.Combine(ActionsFolder,cmbLibraryName.SelectedItem.ToString() + ".dll"),typeof(AbstractEasyAction)).AsEnumerable());
            foreach (ClassMapping cMapping in lstClasses) {
                cmbClassName.Items.Add(cMapping.DisplayName);
            }
        }

        private void cmbClassName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedClassSettings.Clear();
            if (cmbClassName.SelectedItem == null) 
                SelectedClassMapping = null;
            else 
                SelectedClassMapping = lstClasses.Find(cm => cm.DisplayName == cmbClassName.SelectedItem.ToString());
            if (SelectedClassMapping != null)
            {
                lblClassDescription.Text = lstClasses.Find(cm=>cm.DisplayName == cmbClassName.SelectedItem.ToString()).Description;
                lstFields.Items.Clear();
                foreach (EasyFieldAttribute efAttribute in SelectedClassMapping.Class.GetCustomAttributes(typeof(EasyFieldAttribute), true))
                {
                    lstFields.Items.Add(efAttribute.FieldName);
                    SelectedClassSettings.Add(efAttribute.FieldName, efAttribute.DefaultValue);
                }
            }
        }

        private void lstFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblFieldDescription.Text = "";
            pnlField.Visible = false;
            if (lstFields.SelectedItem == null) return;
            if (String.IsNullOrWhiteSpace(lstFields.SelectedItem.ToString())) return;
            foreach (EasyFieldAttribute efAttribute in SelectedClassMapping.Class.GetCustomAttributes(typeof(EasyFieldAttribute), true))
            {
                if (efAttribute.FieldName == lstFields.SelectedItem.ToString())
                {
                    SelectedFieldAttribute = efAttribute;
                    break;
                }
            }
            if (SelectedFieldAttribute != null)
            {
                pnlField.Visible = true;
                lblFieldDescription.Text = SelectedFieldAttribute.FieldDescription;
                lblField.Text = "Value of [" + SelectedFieldAttribute.FieldName + "]";
                cmbField.Items.Clear();
                cmbField.Items.AddRange(SelectedFieldAttribute.PossibleValues.ToArray());
                cmbField.Text = SelectedClassSettings[lstFields.SelectedItem.ToString()];
            }
        }

        private void btnSaveField_Click(object sender, EventArgs e)
        {
            if ((cmbField.Items.Count > 1) && (cmbField.SelectedItem == null)) {
                MessageBox.Show("Please select a valid value");
                return;
            }
            SelectedClassSettings[lstFields.SelectedItem.ToString()] = cmbField.Text;
        }


    }
}
