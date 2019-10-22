using EasyETL;
using EasyETL.Actions;
using EasyETL.Attributes;
using EasyETL.Writers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EasyXmlSample
{
    public partial class ClassConfigurationForm : Form
    {
        public string SettingsPath = "";
        public string SettingsFileName = "";
        public string ActionsFolder = "";
        public Type BaseClassType = null;
        public string ClassType = "action";

        public List<string> lstLibraries = new List<string>();
        public List<ClassMapping> lstClasses = new List<ClassMapping>();
        public ClassMapping SelectedClassMapping = null;
        public EasyFieldAttribute SelectedFieldAttribute = null;
        public Dictionary<string, string> SelectedClassSettings = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        public string SelectedDescription;
        public string SelectedProperties;

        public void LoadFormControls()
        {
            ActionsFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Actions");
            lstLibraries = new List<string>(ReflectionUtils.LoadAllLibrariesWithClass(BaseClassType, ActionsFolder));
            cmbClassName.Items.Clear();
            lstClasses = new List<ClassMapping>(ReflectionUtils.LoadClassesFromLibrary(BaseClassType).AsEnumerable());
            foreach (ClassMapping cMapping in lstClasses)
            {
                cmbClassName.Items.Add(cMapping.DisplayName);
            }
            lblSettingsComplete.Width = panel2.Width;
        }

        public void LoadSettingsFromXml(string settingsPath)
        {
            if (String.IsNullOrWhiteSpace(SettingsFileName)) return;
            if (!File.Exists(SettingsFileName)) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);

            string clientName = settingsPath.Split('\\')[0];
            string className = settingsPath.Split('\\')[2];
            SettingsPath = "//clients/client[@name='" + clientName + "']/" + ClassType + "s/" + ClassType + "[@name='" + className + "']";
            XmlNode xNode = xDoc.SelectSingleNode(SettingsPath);
            if (xNode != null)
            {
                foreach (XmlAttribute xAttr in xNode.Attributes)
                {
                    switch (xAttr.Name.ToLower())
                    {
                        case "name":
                            txtActionName.Text = xAttr.Value; break;
                        case "classname":
                            cmbClassName.SelectedItem = xAttr.Value;
                            break;
                    }
                }

                if (SelectedClassMapping != null) SelectedClassSettings = SelectedClassMapping.Class.LoadFieldSettings(xNode);
                RefreshGridView();
                UpdateColorOfLabel();
            }

        }

        public ClassConfigurationForm()
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
            if (cmbClassName.SelectedItem == null) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNode xNode = xDoc.SelectSingleNode(SettingsPath);
            xNode.InnerText = "";

            XmlAttribute classAttr = xDoc.CreateAttribute("classname");
            classAttr.Value = cmbClassName.SelectedItem.ToString();
            xNode.Attributes.Append(classAttr);


            if (SelectedClassSettings != null)
            {
                if (typeof(IEasyFieldInterface).IsAssignableFrom(SelectedClassMapping.Class))
                {
                    IEasyFieldInterface efiObject = (IEasyFieldInterface)Activator.CreateInstance(SelectedClassMapping.Class);
                    efiObject.LoadFieldSettings(SelectedClassSettings);
                    if (!efiObject.IsFieldSettingsComplete())
                    {
                        MessageBox.Show(this, "The field settings are incomplete.  Please correct before saving.", "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                    efiObject.SaveFieldSettingsToXmlNode(xNode);
                }
                else
                {
                    MessageBox.Show(this, "This control has not implemented the EasyFieldInterface.", "Information", MessageBoxButtons.OK);
                    foreach (KeyValuePair<string, string> kvPair in SelectedClassSettings)
                    {
                        if (!String.IsNullOrWhiteSpace(kvPair.Value))
                        {
                            XmlElement fieldNode = xDoc.CreateElement("field");
                            fieldNode.SetAttribute("name", kvPair.Key);
                            fieldNode.SetAttribute("value", kvPair.Value);
                            xNode.AppendChild(fieldNode);
                        }
                    }
                }
            }

            xDoc.Save(SettingsFileName);
            btnCloseWindow_Click(this, null);
        }

        private void btnCloseWindow_Click(object sender, EventArgs e)
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


        private void cmbClassName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedClassSettings.Clear();
            lblProperties.Text = "";
            if (cmbClassName.SelectedItem == null)
            {
                lblLibraryName.Text = "";
                SelectedClassMapping = null;
            }
            else
            {
                SelectedClassMapping = lstClasses.Find(cm => cm.DisplayName == cmbClassName.SelectedItem.ToString());
            }
            if (SelectedClassMapping != null)
            {
                lblLibraryName.Text = SelectedClassMapping.Assembly.Location;

                lblClassDescription.Text = lstClasses.Find(cm => cm.DisplayName == cmbClassName.SelectedItem.ToString()).Description;

                foreach (EasyFieldAttribute efAttribute in SelectedClassMapping.Class.GetEasyFields().Values)
                {
                    SelectedClassSettings.Add(efAttribute.FieldName, efAttribute.DefaultValue);
                }
                foreach (KeyValuePair<string, string> kvPair in SelectedClassMapping.Class.GetEasyProperties())
                {
                    lblProperties.Text += kvPair.Key + "=" + kvPair.Value + Environment.NewLine;
                }

            }
            RefreshGridView();
            UpdateColorOfLabel();
        }

        private void RefreshGridView()
        {
            if ((SelectedClassMapping == null) || (SelectedClassSettings == null) || (SelectedClassSettings.Keys.Count == 0)) return;
            Dictionary<string, EasyFieldAttribute> easyFields = SelectedClassMapping.Class.GetEasyFields();
            DataTable gridDataTable = new DataTable();
            gridDataTable.Columns.Add("Field Name");
            gridDataTable.Columns.Add("Field Value");

            foreach (KeyValuePair<string, EasyFieldAttribute> kvEasyField in easyFields)
            {
                DataRow gridRow = gridDataTable.NewRow();
                gridRow["Field Name"] = kvEasyField.Key;
                if (SelectedClassSettings.ContainsKey(kvEasyField.Key)) gridRow["Field Value"] = (kvEasyField.Value.IsPassword) ? "<<<****Password****>>>" : SelectedClassSettings[kvEasyField.Key];
                gridDataTable.Rows.Add(gridRow);
            }
            gvFieldValues.DataSource = gridDataTable;
        }

        private void UpdateColorOfLabel()
        {
            lblSettingsComplete.BackColor = Color.Transparent;

            if (SelectedClassMapping != null)
            {
                if (typeof(IEasyFieldInterface).IsAssignableFrom(SelectedClassMapping.Class))
                {
                    try
                    {
                        IEasyFieldInterface efiObject = (IEasyFieldInterface)Activator.CreateInstance(SelectedClassMapping.Class);
                        efiObject.LoadFieldSettings(SelectedClassSettings);
                        lblSettingsComplete.BackColor = efiObject.CanFunction() ? Color.Green : Color.Red;
                        efiObject = null;
                        return;
                    }
                    catch
                    {

                    }
                }
            }
            lblSettingsComplete.BackColor = Color.Red;
        }

        private void ClassConfigurationForm_ResizeEnd(object sender, EventArgs e)
        {
            lblSettingsComplete.Width = panel2.Width;
        }

        private void gvFieldValues_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((gvFieldValues.Rows.Count > e.RowIndex) && (gvFieldValues.Rows[e.RowIndex] != null))
            {
                string fieldName = gvFieldValues[0, e.RowIndex].Value.ToString();
                string fieldValue = SelectedClassSettings.ContainsKey(fieldName) ? SelectedClassSettings[fieldName] : String.Empty;
                SelectedFieldAttribute = SelectedClassMapping.Class.GetEasyField(fieldName);
                if (SelectedFieldAttribute != null)
                {
                    string fieldProperties = "";
                    foreach (KeyValuePair<string, string> kvPair in SelectedClassMapping.Class.GetEasyProperties())
                    {
                        fieldProperties += kvPair.Key + "=" + kvPair.Value + Environment.NewLine;
                    }
                    if (String.IsNullOrEmpty(fieldValue)) fieldValue = SelectedFieldAttribute.DefaultValue;
                    FieldSettingModificationForm fsmForm = new FieldSettingModificationForm();
                    fsmForm.SetFields(txtActionName.Text, fieldValue, SelectedFieldAttribute);
                    fsmForm.ShowDialog(this);
                    SelectedClassSettings[fieldName] = fsmForm.FieldValue;
                    RefreshGridView();
                    UpdateColorOfLabel();
                }
            }
        }


    }
}
