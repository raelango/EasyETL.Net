﻿using EasyETL;
using EasyETL.Actions;
using EasyETL.DataSets;
using EasyETL.Extractors;
using EasyETL.Writers;
using EasyETL.Xml;
using EasyETL.Xml.Parsers;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace EasyXmlSample
{

    public partial class ETLForm : Form
    {
        bool actionInProgress = false;
        EasyXmlDocument ezDoc = null;
        XslCompiledTransform xsl = null;
        Stopwatch stopWatch = new Stopwatch();
        List<ExportSettings> AllExportSettings = new List<ExportSettings>();
        public Dictionary<string, ClassMapping> dctClassMapping = new Dictionary<string, ClassMapping>();
        public Dictionary<string, Dictionary<string, string>> dctActionFieldSettings = new Dictionary<string, Dictionary<string, string>>();

        public int intAutoNumber = 0;
        public string SettingsPath = "";
        public string SettingsFileName = "";


        public void LoadSettingsFromXml(string settingsPath)
        {
            if (String.IsNullOrWhiteSpace(SettingsFileName)) return;
            if (!File.Exists(SettingsFileName)) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            #region load profiles
            cmbParserProfile.Items.Clear();
            XmlNodeList profileNodes = xDoc.SelectNodes("//profiles/profile");
            foreach (XmlNode profileNode in profileNodes)
            {
                cmbParserProfile.Items.Add(profileNode.Attributes["profilename"].Value);
            }
            #endregion




            string clientName = settingsPath.Split('\\')[0];
            string etlName = settingsPath.Split('\\')[2];
            SettingsPath = "//clients/client[@name='" + clientName + "']/etls/etl[@name='" + etlName + "']";

            chkAvailableActions.Items.Clear();
            fpActions.Controls.Clear();
            XmlNodeList actionNodes = xDoc.SelectNodes("//clients/client[@name='" + clientName + "']/actions/action");
            dctClassMapping.Clear();
            dctActionFieldSettings.Clear();
            string actionFolderName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Actions");
            foreach (XmlNode actionNode in actionNodes)
            {
                string actionName = actionNode.Attributes.GetNamedItem("name").Value;
                chkAvailableActions.Items.Add(actionName);
                Button btnControl = new Button();
                btnControl.Text = actionName;
                btnControl.Click += btnControl_Click;
                fpActions.Controls.Add(btnControl);

                string libraryName = actionNode.Attributes.GetNamedItem("libraryname").Value;
                string className = actionNode.Attributes.GetNamedItem("classname").Value;
                ClassMapping cMapping = ReflectionUtils.LoadClassFromLibrary(Path.Combine(actionFolderName, libraryName + ".dll"), typeof(AbstractEasyAction), className);

                if (cMapping != null)
                {
                    dctClassMapping.Add(actionName, cMapping);
                    Dictionary<string, string> actionDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
                    dctActionFieldSettings.Add(actionName, actionDictionary);
                    XmlNodeList actionFieldNodeList = actionNode.SelectNodes("field");
                    foreach (XmlNode actionFieldNode in actionFieldNodeList)
                    {
                        actionDictionary.Add(actionFieldNode.Attributes.GetNamedItem("name").Value, actionFieldNode.Attributes.GetNamedItem("value").Value);
                    }

                }


            }


            XmlNode xNode = xDoc.SelectSingleNode(SettingsPath);
            if (xNode != null)
            {
                #region Actions Load
                XmlNode actionsNode = xNode.SelectSingleNode("actions");
                actionNodes = null;
                if (actionsNode != null) actionNodes = actionsNode.SelectNodes("action");
                if (actionNodes != null)
                {

                    foreach (XmlNode actionNode in actionNodes)
                    {
                        if (actionNode.Attributes.GetNamedItem("name") != null)
                        {
                            int loadIndex = chkAvailableActions.Items.IndexOf(actionNode.Attributes.GetNamedItem("name").Value);
                            bool toCheck = true;
                            if ((actionNode.Attributes.GetNamedItem("enabled") != null) && (!Convert.ToBoolean(actionNode.Attributes.GetNamedItem("enabled").Value)))
                            {
                                toCheck = false;
                            }
                            if (loadIndex >= 0) chkAvailableActions.SetItemChecked(loadIndex, toCheck);
                        }
                    }
                }
                DisplayActionButtonsAsNeeded();
                #endregion

                #region Transformations Load
                XmlNode transformationNode = xNode.SelectSingleNode("transformations");
                if (transformationNode != null)
                {
                    XmlNode duringLoadNode = transformationNode.SelectSingleNode("duringload");
                    if (duringLoadNode != null)
                    {
                        foreach (XmlAttribute xAttr in duringLoadNode.Attributes)
                        {
                            switch (xAttr.Name.ToLower())
                            {
                                case "profilename":
                                    cbOnLoadProfiles.Text = xAttr.Value; break;
                                case "savefilename":
                                    txtOnLoadFileName.Text = xAttr.Value; break;
                            }
                        }
                        txtOnLoadContents.Text = duringLoadNode.InnerText;
                    }
                    XmlNode afterLoadNode = transformationNode.SelectSingleNode("afterload");
                    if (afterLoadNode != null)
                    {
                        foreach (XmlAttribute xAttr in afterLoadNode.Attributes)
                        {
                            switch (xAttr.Name.ToLower())
                            {
                                case "profilename":
                                    cbTransformProfiles.Text = xAttr.Value; break;
                                case "savefilename":
                                    txtTransformFileName.Text = xAttr.Value; break;
                            }
                        }
                        txtTransformText.Text = afterLoadNode.InnerText;
                    }
                }
                #endregion

                #region Parse Options Load
                XmlNode optionsNode = xNode.SelectSingleNode("parseoptions");
                if (optionsNode != null)
                {
                    if (optionsNode.Attributes["profilename"] != null)
                    {
                        cmbParserProfile.Text = optionsNode.Attributes["profilename"].Value;
                    }

                    LoadParseOptionsFromNode(optionsNode);
                }
                #endregion

                #region Output Settings Load
                XmlNodeList outputNodes = xNode.SelectNodes("output");
                AllExportSettings.Clear();
                foreach (XmlNode dataNode in outputNodes)
                {
                    ExportSettings eSettings = new ExportSettings() { SettingName = "" };
                    foreach (XmlAttribute xAttr in dataNode.Attributes)
                    {
                        switch (xAttr.Name.ToLower())
                        {
                            case "name":
                                eSettings.SettingName = xAttr.Value; break;
                            case "exportformat":
                                eSettings.ExportFormat = xAttr.Value; break;
                            case "exportfilename":
                                eSettings.ExportFileName = xAttr.Value; break;
                            case "templatefilename":
                                eSettings.TemplateFileName = xAttr.Value; break;
                            case "includeheader":
                                eSettings.IncludeTableHeader = Convert.ToBoolean(xAttr.Value); break;
                        }
                    }
                    AllExportSettings.Add(eSettings);
                }
                RefreshExportSettingsCombo();
                //if ((dataNode != null) && (dataNode.Attributes["exportformat"] != null)) cmbDestination.SelectedItem = dataNode.Attributes["exportformat"].Value;
                #endregion

                #region Permissions Settings
                XmlNode permissionsNode = xNode.SelectSingleNode("permissions");
                if (permissionsNode != null)
                {
                    foreach (XmlAttribute xAttr in permissionsNode.Attributes)
                    {
                        switch (xAttr.Name.ToLower())
                        {
                            case "canviewsettings":
                                tableLayoutPanel1.RowStyles[0].Height = tabSource.Height + 6;
                                if (!Convert.ToBoolean(xAttr.Value)) tableLayoutPanel1.RowStyles[0].Height = 0;
                                break;
                            case "caneditsettings":
                                chkCanEditConfiguration.Checked = Convert.ToBoolean(xAttr.Value); break;
                            case "canadddata":
                                chkCanAddData.Checked = Convert.ToBoolean(xAttr.Value); break;
                            case "caneditdata":
                                chkCanEditData.Checked = Convert.ToBoolean(xAttr.Value); break;
                            case "candeletedata":
                                chkCanDeleteData.Checked = Convert.ToBoolean(xAttr.Value); break;
                            case "canexportdata":
                                chkCanExportData.Checked = Convert.ToBoolean(xAttr.Value); break;
                        }
                    }
                }
                #endregion

                #region Datasource Node Load
                XmlNode datasourceNode = xNode.SelectSingleNode("datasource");
                if (datasourceNode != null)
                {

                    foreach (XmlAttribute xAttr in datasourceNode.Attributes)
                    {
                        switch (xAttr.Name.ToLower())
                        {
                            case "sourcetype":
                                tabDataSource.SelectTab("tabDatasource" + xAttr.Value); break;
                            case "filename":
                                txtFileName.Text = xAttr.Value; break;
                            case "usetextextractor":
                                chkUseTextExtractor.Checked = Convert.ToBoolean(xAttr.Value); break;
                            case "textextractor":
                                cbTextExtractor.Text = xAttr.Value; break;
                            case "hasmaxrows":
                                chkHasMaxRows.Checked = Convert.ToBoolean(xAttr.Value); break;
                            case "maxrows":
                                nudMaxRows.Value = Convert.ToInt64(xAttr.Value); break;
                            case "connectionstring":
                                txtDatabaseConnectionString.Text = xAttr.Value; break;
                            case "connectiontype":
                                cmbDatabaseConnectionType.Text = xAttr.Value; break;
                        }
                    }
                    if (datasourceNode.SelectSingleNode("textcontents") != null)
                    {
                        txtTextContents.Text = datasourceNode.SelectSingleNode("textcontents").InnerText;
                    }
                    if (datasourceNode.SelectSingleNode("query") != null)
                    {
                        txtDatabaseQuery.Text = datasourceNode.SelectSingleNode("query").InnerText;
                    }
                }
                #endregion

                if (xNode.Attributes.GetNamedItem("autorefresh") != null)
                {
                    chkAutoRefresh.Checked = Boolean.Parse(xNode.Attributes.GetNamedItem("autorefresh").Value);
                }
                else
                {
                    chkAutoRefresh.Checked = false;
                }
            }

        }

        private void DisplayActionButtonsAsNeeded()
        {
            foreach (Control c in fpActions.Controls)
            {
                if (c is Button)
                {
                    Button b = (Button)c;
                    b.Visible = chkAvailableActions.CheckedItems.Contains(b.Text);
                }
            }
        }

        void btnControl_Click(object sender, EventArgs e)
        {
            actionInProgress = true;
            string actionName = ((Button)sender).Text;
            if (dctClassMapping[actionName] != null)
            {
                AbstractEasyAction aea = (AbstractEasyAction)Activator.CreateInstance(dctClassMapping[actionName].Class);
                foreach (KeyValuePair<string, string> kvPair in dctActionFieldSettings[actionName])
                {
                    aea.SettingsDictionary.Add(kvPair.Key, kvPair.Value);
                }
                ProgressForm pForm = new ProgressForm();
                //pForm.TopLevel = false;
                pForm.TopMost = true;
                //splitContainer2.Panel2.Controls.Add(pForm);
                pForm.Show(splitContainer2.Panel2);
                pForm.MaximumItems = dataGrid.SelectedRows.Count;
                pForm.ActionName = actionName;
                int currentIndex = 0;
                foreach (DataGridViewRow dgvRow in dataGrid.SelectedRows)
                {
                    currentIndex++;
                    pForm.SetCurrentIndex(currentIndex);
                    Application.DoEvents();
                    aea.Execute(((DataRowView)dgvRow.DataBoundItem).Row);
                }
                //splitContainer2.Panel2.Controls.Remove(pForm);
                pForm.Close();
            }
            actionInProgress = false;
            EnableActionButtonsAsNeeded();
        }


        private void LoadParseOptionsFromNode(XmlNode optionsNode)
        {
            string delimiter = "";
            foreach (XmlAttribute xAttr in optionsNode.Attributes)
            {
                switch (xAttr.Name.ToLower())
                {
                    case "parsertype":
                        cmbFileType.SelectedItem = xAttr.Value;
                        cmbFileType_SelectedIndexChanged(this, null);
                        break;
                    case "delimiter":
                        delimiter = xAttr.Value;
                        Control c = this.Controls.Find("rbDelimiter" + delimiter, true)[0];
                        ((RadioButton)c).Checked = true;
                        break;
                    case "hasheader":
                        cbHeaderRow.Checked = Convert.ToBoolean(xAttr.Value);
                        break;
                    case "separator":
                        txtCustomDelimiter.Text = xAttr.InnerText; break;
                }
            }
            if (optionsNode.SelectSingleNode("comments") != null)
            {
                if (cmbFileType.SelectedItem.ToString() == "Delimited")
                    txtDelimitedComments.Text = optionsNode.SelectSingleNode("comments").InnerText;
                else
                    txtFixedWidthComments.Text = optionsNode.SelectSingleNode("comments").InnerText;
            }

            if (optionsNode.SelectSingleNode("xpath") != null)
            {
                txtXPathQuery.Text = optionsNode.SelectSingleNode("xpath").InnerText;
            }

            if (optionsNode.SelectSingleNode("template") != null)
            {
                txtTemplateString.Text = optionsNode.SelectSingleNode("template").InnerText;
            }

            XmlNodeList widthList = optionsNode.SelectNodes("width");
            if (widthList != null)
            {
                lstFixedColumnWidths.Items.Clear();
                foreach (XmlNode widthNode in widthList)
                {
                    lstFixedColumnWidths.Items.Add(Convert.ToInt16(widthNode.InnerText));
                }
            }
        }

        public ETLForm()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            ofd.CheckFileExists = true;
            ofd.FileName = txtFileName.Text;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = ofd.FileName;
                txtExportFileName.Text = Path.ChangeExtension(txtFileName.Text, GetExportPathExtension());
                //LoadDataToGridView();
            }
        }

        public void LoadDataToGridView()
        {
            if (stopWatch.IsRunning) stopWatch.Stop();
            if ((tabDataSource.SelectedTab == tabDatasourceText) && (txtTextContents.TextLength == 0)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceFile) && (txtFileName.TextLength == 0)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceFile) && !File.Exists(txtFileName.Text)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceDatabase) && (String.IsNullOrWhiteSpace(txtDatabaseConnectionString.Text) || String.IsNullOrWhiteSpace(txtDatabaseQuery.Text))) return;
            stopWatch.Restart();
            intAutoNumber = 0;
            this.UseWaitCursor = true;
            Application.DoEvents();
            lblRecordCount.Text = "";
            txtRegexContents.Text = "";
            cmbTableName.Items.Clear();
            IContentExtractor extractor = null;
            if (chkUseTextExtractor.Checked)
            {
                switch (cbTextExtractor.Text)
                {
                    case "PDF":
                        extractor = new PDFContentExtractor();
                        break;
                    case "Word":
                        extractor = new WordContentExtractor();
                        break;
                }
            }
            try
            {
                xsl = null;
                EasyXmlDocument xDoc = new EasyXmlDocument();
                xDoc.OnProgress += xDoc_OnProgress;
                //xDoc.OnRowAdd += xDoc_OnRowAdd;
                xDoc.OnError += xDoc_OnError;
                if (tabDataSource.SelectedTab == tabDatasourceDatabase)
                {
                    DatabaseEasyParser dbep = null;
                    switch (cmbDatabaseConnectionType.Text)
                    {
                        case "Sql":
                            dbep = new DatabaseEasyParser(EasyDatabaseConnectionType.edctSQL, txtDatabaseConnectionString.Text);
                            break;
                        case "Oledb":
                            dbep = new DatabaseEasyParser(EasyDatabaseConnectionType.edctOledb, txtDatabaseConnectionString.Text);
                            break;
                        case "Odbc":
                            dbep = new DatabaseEasyParser(EasyDatabaseConnectionType.edctODBC, txtDatabaseConnectionString.Text);
                            break;
                    }
                    xDoc.LoadXml(dbep.LoadFromQuery(txtDatabaseQuery.Text).OuterXml);
                }
                else
                {
                    AbstractEasyParser ep = null;
                    switch (cmbFileType.Text)
                    {
                        case "Html":
                            ep = new HtmlEasyParser();
                            break;
                        case "Delimited":
                            string delimiter = "";
                            if (rbDelimiterComma.Checked) delimiter = ",";
                            if (rbDelimiterSemicolon.Checked) delimiter = ";";
                            if (rbDelimiterSpace.Checked) delimiter = " ";
                            if (rbDelimiterTab.Checked) delimiter = "\t";
                            if (rbDelimiterCustom.Checked) delimiter = txtCustomDelimiter.Text;
                            ep = new DelimitedEasyParser(cbHeaderRow.Checked) { RowNodeName = "record" };
                            if (!String.IsNullOrEmpty(delimiter))
                            {
                                ((DelimitedEasyParser)ep).Delimiters.Add(delimiter);
                            }
                            if (txtDelimitedComments.Text.Trim().Length > 0)
                            {
                                ((DelimitedEasyParser)ep).SetCommentTokens(txtDelimitedComments.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                            }
                            break;
                        case "HL7":
                            ep = new HL7EasyParser();
                            break;
                        case "Fixed Width":
                            ep = new FixedWidthEasyParser(false, lstFixedColumnWidths.Items.Cast<int>().ToArray());
                            if (txtFixedWidthComments.Text.Trim().Length > 0)
                            {
                                ((FixedWidthEasyParser)ep).SetCommentTokens(txtFixedWidthComments.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                            }
                            break;
                        case "Json":
                            ep = new JsonEasyParser();
                            break;
                        case "Template":
                            if (String.IsNullOrWhiteSpace(txtTemplateString.Text)) txtTemplateString.Text = "[Contents]";
                            ep = new TemplateEasyParser();
                            ((TemplateEasyParser)ep).Templates = txtTemplateString.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            ((TemplateEasyParser)ep).LoadStr("");
                            StringBuilder sb = new StringBuilder();
                            foreach (string strRegex in ((TemplateEasyParser)ep).lstRegex.Keys)
                            {
                                sb.AppendLine(strRegex);
                            }
                            txtRegexContents.Text = sb.ToString();
                            break;
                        case "HtmlTable":
                            ep = new HtmlTableEasyParser();
                            break;
                    }
                    ep.ProgressInterval = 100;
                    if (chkHasMaxRows.Checked) ep.MaxRecords = Convert.ToInt64(nudMaxRows.Value);
                    if (!String.IsNullOrWhiteSpace(txtOnLoadContents.Text))
                        ep.OnLoadSettings = txtOnLoadContents.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    if ((tabDataSource.SelectedTab == tabDatasourceFile))
                        xDoc.Load(txtFileName.Text, ep, extractor);
                    else
                        xDoc.LoadStr(txtTextContents.Text, ep, extractor);
                    txtExceptions.Text = "";
                    if ((ep != null) && (ep.Exceptions.Count > 0))
                    {
                        MessageBox.Show("There were " + ep.Exceptions.Count + " Exceptions while loading the document");
                        foreach (MalformedLineException mep in ep.Exceptions)
                        {
                            txtExceptions.Text += String.Format("(Line {0} - {1}", mep.LineNumber, mep.Message) + Environment.NewLine;
                        }
                    }
                }
                ezDoc = xDoc;
                try
                {
                    TransformDataFromEzDoc();
                }
                catch
                {
                    MessageBox.Show("Error Transforming document...");
                }

            }
            catch
            {
                MessageBox.Show("Error loading contents...");
            }
            this.UseWaitCursor = false;
        }

        void xDoc_OnProgress(object sender, EasyParserProgressEventArgs e)
        {
            lblRecordCount.Text = "(" + e.LineNumber.ToString() + ") Records Processed....";
            Application.DoEvents();
        }

        void xDoc_OnError(object sender, EasyParserExceptionEventArgs e)
        {
            MessageBox.Show("Error <<" + e.Exception.Message + ">> in line number (" + e.Exception.LineNumber.ToString() + ")");
        }

        void xDoc_OnRowAdd(object sender, XmlNodeChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtOnLoadContents.Text)) return;
            XmlNode rowNode = e.Node;
            if (xsl == null)
            {
                xsl = rowNode.GetCompiledTransform(txtOnLoadContents.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            }
            rowNode = rowNode.TransformXml(xsl);
            e.Node.InnerXml = (rowNode == null) ? String.Empty : rowNode.InnerXml;
        }

        private void TransformDataFromEzDoc()
        {
            if (!stopWatch.IsRunning) stopWatch.Restart();
            lblRecordCount.Text = "";
            cmbTableName.Items.Clear();
            EasyXmlDocument xDoc = (EasyXmlDocument)ezDoc;
            try
            {
                xDoc = (EasyXmlDocument)ezDoc.Clone();
                if (!String.IsNullOrWhiteSpace(txtTransformText.Text))
                {
                    xDoc.Transform(txtTransformText.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
                }
                if (!String.IsNullOrWhiteSpace(txtXPathQuery.Text))
                {
                    xDoc = xDoc.ApplyFilter(txtXPathQuery.Text);
                }
                txtXPathContents.Text = xDoc.LastTransformerTemplate;
            }
            catch
            {
                xDoc = (EasyXmlDocument)ezDoc;
                MessageBox.Show("Unable to transform....");
            }

            try
            {
                txtXmlContents.Text = xDoc.Beautify();
            }
            catch
            {
            }

            DataSet ds = null;
            fpActions.Visible = false;
            try
            {
                ds = xDoc.ToDataSet();
                if (ds != null)
                {
                    fpActions.Visible = true;
                    foreach (DataTable table in ds.Tables)
                    {
                        cmbTableName.Items.Add(table.TableName);
                    }
                }
                dataGrid.DataSource = ds;
                EnableExportButtonIfNeeded();
                if (cmbTableName.Items.Count > 0)
                {
                    cmbTableName.SelectedIndex = 0;
                    dataGrid.DataMember = cmbTableName.Text;
                    lblRecordCount.Text = "(No Records)";
                    if (dataGrid.RowCount > 0) lblRecordCount.Text = dataGrid.RowCount + " Record(s)";
                }
            }
            catch
            {

            }

            stopWatch.Stop();
            lblRecordCount.Text += Environment.NewLine + String.Format("[{0}:{1}:{2}.{3}]", stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();
        }

        private void EnableExportButtonIfNeeded()
        {
            DataSet ds = (DataSet)dataGrid.DataSource;
            btnExport.Enabled = ((ds != null) && (ds.Tables.Count > 0)) && (cmbExport.SelectedItem != null) && (!String.IsNullOrWhiteSpace(cmbExport.SelectedItem.ToString()));
        }

        public void LoadControls()
        {
            cmbFileType.SelectedIndex = 0;
            cmbDatabaseConnectionType.SelectedIndex = 0;
            btnRefreshOnLoadProfiles_Click(this, null);
            btnTransformProfilesLoad_Click(this, null);
        }

        private void cmbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDelimited.Visible = false;
            grpFixedFileOptions.Visible = false;
            grpHtmlOptions.Visible = false;
            grpTemplate.Visible = false;
            switch (cmbFileType.Text)
            {
                case "Delimited":
                    cmbDelimited.Visible = true;
                    break;
                case "Fixed Width":
                    grpFixedFileOptions.Visible = true;
                    grpFixedFileOptions.Top = cmbDelimited.Top;
                    grpFixedFileOptions.Left = cmbDelimited.Left;
                    break;
                case "Html":
                    grpHtmlOptions.Visible = true;
                    grpHtmlOptions.Top = cmbDelimited.Top;
                    grpHtmlOptions.Left = cmbDelimited.Left;
                    break;
                case "Template":
                    grpTemplate.Visible = true;
                    grpTemplate.Top = cmbDelimited.Top;
                    grpTemplate.Left = cmbDelimited.Left;
                    break;

            }
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void rbDelimiterCustom_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomDelimiter.Enabled = rbDelimiterCustom.Checked;
            if (txtCustomDelimiter.Enabled) txtCustomDelimiter.Focus();
        }

        private void cmbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGrid.DataMember = cmbTableName.Text;
            lblRecordCount.Text = "(No Records)";
            if (dataGrid.RowCount > 0)
            {
                lblRecordCount.Text = dataGrid.RowCount + " Record(s)";
            }
        }

        private void lstFixedColumnWidths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(lstFixedColumnWidths.Text)) nupColumnWidth.Value = Int16.Parse(lstFixedColumnWidths.Text);
            btnRemove.Visible = !String.IsNullOrWhiteSpace(lstFixedColumnWidths.Text);
            btnUpdate.Visible = btnRemove.Visible;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstFixedColumnWidths.Text == String.Empty)
                lstFixedColumnWidths.Items.Add(Int32.Parse(nupColumnWidth.Value.ToString()));
            else
                lstFixedColumnWidths.Items.Insert(lstFixedColumnWidths.SelectedIndex, Int32.Parse(nupColumnWidth.Value.ToString()));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int pos = lstFixedColumnWidths.SelectedIndex;
            lstFixedColumnWidths.Items.RemoveAt(pos);
            lstFixedColumnWidths.Items.Insert(pos, Int32.Parse(nupColumnWidth.Value.ToString()));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lstFixedColumnWidths.Items.RemoveAt(lstFixedColumnWidths.SelectedIndex);
        }


        private void txtTextContents_Leave(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void cbTransformProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTransformFileName.Text = cbTransformProfiles.Text;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNode transformNode = xDoc.SelectSingleNode("//transforms/transform[@profilename='" + txtTransformFileName.Text + "']");
            if (transformNode != null) txtTransformText.Text = transformNode.InnerText;
        }

        private void btnTransformProfilesLoad_Click(object sender, EventArgs e)
        {
            cbTransformProfiles.Items.Clear();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNodeList transformNodeList = xDoc.SelectNodes("//transforms/transform");
            foreach (XmlNode xNode in transformNodeList)
            {
                cbTransformProfiles.Items.Add(xNode.Attributes["profilename"].Value);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (cmbExport.SelectedItem == null) return;
            if (String.IsNullOrWhiteSpace(cmbExport.SelectedItem.ToString())) return;
            DataSet rds = (DataSet)dataGrid.DataSource;
            DatasetWriter dsw = null;
            ExportSettings eSettings = AllExportSettings.Find(m => m.SettingName == cmbExport.SelectedItem.ToString());
            if (eSettings == null)
            {
                MessageBox.Show("Please select a valid export type");
                return;
            }
            switch (eSettings.ExportFormat.ToUpper())
            {
                case "CSV":
                    dsw = new DelimitedDatasetWriter(rds, eSettings.ExportFileName) { Delimiter = ',', IncludeHeaders = true, IncludeQuotes = true };
                    break;
                case "TAB":
                    dsw = new DelimitedDatasetWriter(rds, eSettings.ExportFileName) { Delimiter = '\t', IncludeHeaders = true, IncludeQuotes = true };
                    break;
                case "HTML":
                    dsw = new HtmlDatasetWriter(rds, eSettings.ExportFileName, eSettings.TemplateFileName);
                    break;
                case "WORD":
                    dsw = new OfficeDatasetWriter(rds, eSettings.ExportFileName, eSettings.TemplateFileName);
                    break;
                case "EXCEL":
                    dsw = new OfficeDatasetWriter(rds, eSettings.ExportFileName, eSettings.TemplateFileName) { DestinationType = OfficeFileType.ExcelWorkbook };
                    break;
                case "MAILMERGE-WORD":
                    dsw = new OfficeMailMergeDatasetWriter(rds, eSettings.ExportFileName, eSettings.TemplateFileName);
                    break;
                case "XML":
                    dsw = new XmlDatasetWriter(rds, eSettings.ExportFileName, eSettings.TemplateFileName);
                    break;
                case "PDF":
                    dsw = new PDFDatasetWriter(rds, eSettings.ExportFileName);
                    break;
                case "JSON":
                    dsw = new JsonDatasetWriter(rds, eSettings.ExportFileName);
                    break;
            }
            dsw.PrintHeader = eSettings.IncludeTableHeader;
            dsw.Write();
            MessageBox.Show("Saved file in " + Path.GetDirectoryName(eSettings.ExportFileName));
            Process.Start(Path.GetDirectoryName(eSettings.ExportFileName));
            //Process.Start(txtExportFileName.Text);
        }

        private void btnTransformSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtTransformFileName.Text))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(SettingsFileName);
                XmlNode transformsNode = xDoc.SelectSingleNode("//transforms");
                if (transformsNode == null)
                {
                    transformsNode = xDoc.CreateElement("transforms");
                    xDoc.DocumentElement.AppendChild(transformsNode);
                }
                XmlElement transformNode = (XmlElement)xDoc.SelectSingleNode("//transforms/transform[@profilename='" + txtTransformFileName.Text + "']");
                if (transformNode == null)
                {
                    transformNode = xDoc.CreateElement("transform");
                    transformNode.SetAttribute("profilename", txtTransformFileName.Text);
                    transformsNode.AppendChild(transformNode);
                }
                transformNode.InnerText = txtTransformText.Text;
                xDoc.Save(SettingsFileName);
                if (!cbTransformProfiles.Items.Contains(txtTransformFileName.Text)) cbTransformProfiles.Items.Add(txtTransformFileName.Text);
                cbTransformProfiles.SelectedText = txtOnLoadFileName.Text;
            }

        }

        private void txtTransformText_Leave(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) TransformDataFromEzDoc();
        }

        private void chkUseTextExtractor_CheckedChanged(object sender, EventArgs e)
        {
            cbTextExtractor.Visible = chkUseTextExtractor.Checked;
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void btnRefreshOnLoadProfiles_Click(object sender, EventArgs e)
        {
            cbOnLoadProfiles.Items.Clear();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNodeList transformNodeList = xDoc.SelectNodes("//transforms/transform");
            foreach (XmlNode xNode in transformNodeList)
            {
                cbOnLoadProfiles.Items.Add(xNode.Attributes["profilename"].Value);
            }
        }

        private void btnOnLoadSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtOnLoadFileName.Text))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(SettingsFileName);
                XmlNode transformsNode = xDoc.SelectSingleNode("//transforms");
                if (transformsNode == null)
                {
                    transformsNode = xDoc.CreateElement("transforms");
                    xDoc.DocumentElement.AppendChild(transformsNode);
                }
                XmlElement transformNode = (XmlElement)xDoc.SelectSingleNode("//transforms/transform[@profilename='" + txtParserProfileSaveFileName.Text + "']");
                if (transformNode == null)
                {
                    transformNode = xDoc.CreateElement("transform");
                    transformNode.SetAttribute("profilename", txtOnLoadFileName.Text);
                    transformsNode.AppendChild(transformNode);
                }
                transformNode.InnerText = txtOnLoadContents.Text;
                xDoc.Save(SettingsFileName);
                if (!cbOnLoadProfiles.Items.Contains(txtOnLoadFileName.Text)) cbOnLoadProfiles.Items.Add(txtOnLoadFileName.Text);
                cbOnLoadProfiles.SelectedText = txtOnLoadFileName.Text;
            }
        }

        private void cbOnLoadProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOnLoadFileName.Text = cbOnLoadProfiles.Text;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNode transformNode = xDoc.SelectSingleNode("//transforms/transform[@profilename='" + txtOnLoadFileName.Text + "']");
            if (transformNode != null) txtOnLoadContents.Text = transformNode.InnerText;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
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
            if (xNode == null)
            {
                string etlName = SettingsPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[3];
                etlName = System.Text.RegularExpressions.Regex.Match(etlName, "name='(?<ETLName>.*?)'").Groups["ETLName"].Value;
                XmlElement xElement = xDoc.CreateElement("etl");
                xElement.SetAttribute("name", etlName);
                string parentPath = SettingsPath.Remove(SettingsPath.IndexOf("/etls/etl")+5);
                xDoc.SelectSingleNode(parentPath).AppendChild(xElement);
                xNode = xDoc.SelectSingleNode(SettingsPath);
            }
            
            xNode.InnerText = "";
            XmlAttribute autoRefresh = xDoc.CreateAttribute("autorefresh");
            autoRefresh.Value = chkAutoRefresh.Checked.ToString();
            xNode.Attributes.Append(autoRefresh);
            #region Datasource Section
            XmlElement datasourceNode = xDoc.CreateElement("datasource");
            datasourceNode.SetAttribute("sourcetype", tabDataSource.SelectedTab.Text);
            switch (tabDataSource.SelectedTab.Text)
            {
                case "File":
                    datasourceNode.SetAttribute("usetextextractor", chkUseTextExtractor.Checked.ToString());
                    datasourceNode.SetAttribute("textextractor", cbTextExtractor.Text);
                    datasourceNode.SetAttribute("hasmaxrows", chkHasMaxRows.Checked.ToString());
                    datasourceNode.SetAttribute("maxrows", nudMaxRows.Value.ToString());
                    datasourceNode.SetAttribute("filename", txtFileName.Text);
                    break;
                case "Text":
                    XmlElement textContents = xDoc.CreateElement("textcontents");
                    textContents.InnerText = txtTextContents.Text;
                    datasourceNode.AppendChild(textContents);
                    break;
                case "Database":
                    datasourceNode.SetAttribute("connectionstring", txtDatabaseConnectionString.Text);
                    datasourceNode.SetAttribute("connectiontype", cmbDatabaseConnectionType.Text);
                    XmlElement queryNode = xDoc.CreateElement("query");
                    queryNode.InnerText = txtDatabaseQuery.Text;
                    datasourceNode.AppendChild(queryNode);
                    break;
            }
            xNode.AppendChild(datasourceNode);
            #endregion

            #region Actions section
            XmlElement actionsNode = xDoc.CreateElement("actions");
            foreach (var cic in chkAvailableActions.CheckedItems)
            {
                XmlElement actionNode = xDoc.CreateElement("action");
                actionNode.SetAttribute("name", cic.ToString());
                actionNode.SetAttribute("enabled", "True");
                actionsNode.AppendChild(actionNode);
            }
            xNode.AppendChild(actionsNode);
            #endregion

            #region Transformations section
            XmlElement transformationNode = xDoc.CreateElement("transformations");
            XmlElement duringLoadNode = xDoc.CreateElement("duringload");
            duringLoadNode.SetAttribute("profilename", cbOnLoadProfiles.Text);
            duringLoadNode.SetAttribute("savefilename", txtOnLoadFileName.Text);
            duringLoadNode.InnerText = txtOnLoadContents.Text;
            XmlElement afterLoadNode = xDoc.CreateElement("afterload");
            afterLoadNode.SetAttribute("profilename", cbTransformProfiles.Text);
            afterLoadNode.SetAttribute("savefilename", txtTransformFileName.Text);
            afterLoadNode.InnerText = txtTransformText.Text;
            transformationNode.AppendChild(duringLoadNode);
            transformationNode.AppendChild(afterLoadNode);
            xNode.AppendChild(transformationNode);
            #endregion

            #region Parse Options section
            XmlElement optionsNode = xDoc.CreateElement("parseoptions");
            if (!String.IsNullOrWhiteSpace(cmbParserProfile.Text))
            {
                optionsNode.SetAttribute("profilename", cmbParserProfile.Text);
            }
            GetCurrentParserSettings(optionsNode);
            xNode.AppendChild(optionsNode);
            #endregion

            #region Export Settings
            foreach (ExportSettings eSettings in AllExportSettings)
            {
                XmlElement dataNode = xDoc.CreateElement("output");
                dataNode.SetAttribute("name", eSettings.SettingName);
                dataNode.SetAttribute("exportformat", eSettings.ExportFormat);
                dataNode.SetAttribute("exportfilename", eSettings.ExportFileName);
                dataNode.SetAttribute("templatefilename", eSettings.TemplateFileName);
                dataNode.SetAttribute("includeheader", eSettings.IncludeTableHeader.ToString());
                xNode.AppendChild(dataNode);
            }
            #endregion

            #region Permissions Settings
            XmlElement permissionsNode = xDoc.CreateElement("permissions");
            permissionsNode.SetAttribute("role", "owner");
            permissionsNode.SetAttribute("canviewsettings", (tableLayoutPanel1.RowStyles[0].Height > 0).ToString());
            permissionsNode.SetAttribute("caneditsettings", chkCanEditConfiguration.Checked.ToString());
            permissionsNode.SetAttribute("canadddata", chkCanAddData.Checked.ToString());
            permissionsNode.SetAttribute("caneditdata", chkCanEditData.Checked.ToString());
            permissionsNode.SetAttribute("candeletedata", chkCanDeleteData.Checked.ToString());
            permissionsNode.SetAttribute("canexportdata", chkCanExportData.Checked.ToString());
            xNode.AppendChild(permissionsNode);
            #endregion

            xDoc.Save(SettingsFileName);
        }

        private void GetCurrentParserSettings(XmlElement optionsNode)
        {
            XmlDocument xDoc = optionsNode.OwnerDocument;
            optionsNode.SetAttribute("parsertype", cmbFileType.Text);
            switch (cmbFileType.Text)
            {
                case "Delimited":
                    string delimiter = "AutoDetect";
                    if (rbDelimiterComma.Checked) delimiter = "Comma";
                    if (rbDelimiterSemicolon.Checked) delimiter = "Semicolon";
                    if (rbDelimiterSpace.Checked) delimiter = "Space";
                    if (rbDelimiterTab.Checked) delimiter = "Tab";
                    if (rbDelimiterCustom.Checked) delimiter = "Custom";
                    string separator = String.Empty;
                    if (rbDelimiterCustom.Checked) separator = txtCustomDelimiter.Text;
                    optionsNode.SetAttribute("delimiter", delimiter);
                    optionsNode.SetAttribute("separator", separator);
                    optionsNode.SetAttribute("hasheader", cbHeaderRow.Checked.ToString());
                    XmlElement dCommentsElement = xDoc.CreateElement("comments");
                    dCommentsElement.InnerText = txtDelimitedComments.Text;
                    optionsNode.AppendChild(dCommentsElement);
                    break;
                case "Fixed Width":
                    XmlElement fCommentsElement = xDoc.CreateElement("comments");
                    fCommentsElement.InnerText = txtFixedWidthComments.Text;
                    optionsNode.AppendChild(fCommentsElement);
                    foreach (int i in lstFixedColumnWidths.Items.Cast<int>())
                    {
                        XmlElement widthNode = xDoc.CreateElement("width");
                        widthNode.InnerText = i.ToString();
                        optionsNode.AppendChild(widthNode);
                    }
                    break;
                case "Html":
                    XmlElement htmlElement = xDoc.CreateElement("xpath");
                    htmlElement.InnerText = txtXPathQuery.Text;
                    break;
                case "Template":
                    XmlElement templateElement = xDoc.CreateElement("template");
                    templateElement.InnerText = txtTemplateString.Text;
                    optionsNode.AppendChild(templateElement);
                    break;
            }
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

        private void tabSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabSource.SelectedTab == tabParseOptions) && (tabDataSource.SelectedTab == tabDatasourceDatabase))
            {
                tabSource.SelectedTab = tabTransformationOptions;
            }
        }

        private void txtParserProfileSaveFileName_TextChanged(object sender, EventArgs e)
        {
            btnSaveParserProfile.Enabled = !String.IsNullOrWhiteSpace(txtParserProfileSaveFileName.Text);
        }

        private void btnSaveParserProfile_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNode profilesNode = xDoc.SelectSingleNode("//profiles");
            if (profilesNode == null)
            {
                profilesNode = xDoc.CreateElement("profiles");
                xDoc.DocumentElement.AppendChild(profilesNode);
            }
            XmlElement profileNode = (XmlElement)xDoc.SelectSingleNode("//profiles/profile[@profilename='" + txtParserProfileSaveFileName.Text + "']");
            if (profileNode == null)
            {
                profileNode = xDoc.CreateElement("profile");
                profileNode.SetAttribute("profilename", txtParserProfileSaveFileName.Text);
                profilesNode.AppendChild(profileNode);
            }
            profileNode.InnerText = "";
            GetCurrentParserSettings(profileNode);

            xDoc.Save(SettingsFileName);
            btnSaveParserProfile.Enabled = false;
            if (!cmbParserProfile.Items.Contains(txtParserProfileSaveFileName.Text)) cmbParserProfile.Items.Add(txtParserProfileSaveFileName.Text);
            cmbParserProfile.Text = txtParserProfileSaveFileName.Text;
        }

        private void cmbParserProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(cmbParserProfile.Text))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(SettingsFileName);
                XmlNode profileNode = xDoc.SelectSingleNode("//profiles/profile[@profilename='" + cmbParserProfile.Text + "']");
                if (profileNode != null)
                {
                    txtParserProfileSaveFileName.Text = cmbParserProfile.Text;
                    LoadParseOptionsFromNode(profileNode);
                }
            }
        }

        private void btnExportFileName_Click(object sender, EventArgs e)
        {
            ofd.CheckFileExists = false;
            ofd.AddExtension = true;
            ofd.DefaultExt = GetExportPathExtension();
            ofd.FileName = txtExportFileName.Text;
            if (ofd.ShowDialog() == DialogResult.OK) txtExportFileName.Text = ofd.FileName;
        }

        private string GetExportPathExtension()
        {
            switch (cmbExportDestination.Text.ToUpper())
            {
                case "WORD":
                case "MAILMERGE-WORD":
                    return "docx";
                case "EXCEL":
                    return "xlsx";
                case "TAB":
                    return "txt";
                default:
                    return cmbExportDestination.Text.ToLower();
            }
        }

        private void cmbDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtExportFileName.Text)) txtExportFileName.Text = txtFileName.Text;
            txtExportFileName.Text = Path.ChangeExtension(txtExportFileName.Text, GetExportPathExtension());
            pnlExportXsltDetails.Visible = ((cmbExportDestination.Text == "XML") || (cmbExportDestination.Text == "EXCEL") || (cmbExportDestination.Text == "WORD") || (cmbExportDestination.Text == "HTML") || (cmbExportDestination.Text == "MAILMERGE-WORD"));
            if (!File.Exists(txtExportTemplateFileName.Text)) txtExportTemplateFileName.Text = "";
        }

        private void btnExportXsltFileName_Click(object sender, EventArgs e)
        {
            ofd.CheckFileExists = true;
            ofd.DefaultExt = "xslt";
            if (ofd.ShowDialog() == DialogResult.OK) txtExportTemplateFileName.Text = ofd.FileName;
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void nudMaxRows_ValueChanged(object sender, EventArgs e)
        {
            chkHasMaxRows.Checked = true;
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void chkHasMaxRows_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void txtTextContents_TextChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void chkFixedFirstRowHasFieldNames_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void txtCustomDelimiter_TextChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void chkCanAddData_CheckedChanged(object sender, EventArgs e)
        {
            dataGrid.AllowUserToAddRows = chkCanAddData.Checked;
            dataGrid.RowHeadersVisible = (!dataGrid.ReadOnly || dataGrid.AllowUserToAddRows || dataGrid.AllowUserToDeleteRows);
        }

        private void chkCanEditData_CheckedChanged(object sender, EventArgs e)
        {
            dataGrid.ReadOnly = !chkCanEditData.Checked;
            dataGrid.EditMode = DataGridViewEditMode.EditOnF2;
            dataGrid.RowHeadersVisible = (!dataGrid.ReadOnly || dataGrid.AllowUserToAddRows || dataGrid.AllowUserToDeleteRows);
        }

        private void chkCanDeleteData_CheckedChanged(object sender, EventArgs e)
        {
            dataGrid.AllowUserToDeleteRows = chkCanDeleteData.Checked;
            dataGrid.RowHeadersVisible = (!dataGrid.ReadOnly || dataGrid.AllowUserToAddRows || dataGrid.AllowUserToDeleteRows);
        }

        private void chkCanExportData_CheckedChanged(object sender, EventArgs e)
        {
            pnlExport.Visible = chkCanExportData.Checked;
        }

        private void btnSaveExportSettings_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtExportSettingName.Text))
            {
                ExportSettings eSettings = AllExportSettings.Find(m => m.SettingName == txtExportSettingName.Text);
                if (eSettings == null)
                {
                    eSettings = new ExportSettings() { SettingName = txtExportSettingName.Text };
                    AllExportSettings.Add(eSettings);
                }
                eSettings.ExportFormat = cmbExportDestination.SelectedItem.ToString();
                eSettings.ExportFileName = txtExportFileName.Text;
                eSettings.TemplateFileName = txtExportTemplateFileName.Text;
                eSettings.IncludeTableHeader = chkIncludeTableHeader.Checked;
            }
            RefreshExportSettingsCombo();
        }

        private void RefreshExportSettingsCombo()
        {
            cmbExportSettings.Items.Clear();
            cmbExport.Items.Clear();
            foreach (ExportSettings eSetting in AllExportSettings)
            {
                cmbExportSettings.Items.Add(eSetting.SettingName);
                cmbExport.Items.Add(eSetting.SettingName);
            }
        }

        private void cmbExportSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExportSettings.SelectedItem != null)
            {
                ExportSettings eSettings = AllExportSettings.Find(m => m.SettingName == cmbExportSettings.SelectedItem.ToString());
                if (eSettings != null)
                {
                    grpExportSettings.Text = eSettings.SettingName;
                    txtExportSettingName.Text = eSettings.SettingName;
                    txtExportFileName.Text = eSettings.ExportFileName;
                    txtExportTemplateFileName.Text = eSettings.TemplateFileName;
                    cmbExportDestination.Text = eSettings.ExportFormat;
                    chkIncludeTableHeader.Checked = eSettings.IncludeTableHeader;
                }
                else
                {
                    txtExportSettingName.Text = "";
                    txtExportFileName.Text = "";
                    txtExportTemplateFileName.Text = "";
                    cmbExportDestination.Text = "";
                    chkIncludeTableHeader.Checked = true;
                }
            }
        }

        private void btnExportDeleteSetting_Click(object sender, EventArgs e)
        {
            ExportSettings eSettings = AllExportSettings.Find(m => m.SettingName == txtExportSettingName.Text);
            if (eSettings != null) AllExportSettings.Remove(eSettings);
            txtExportSettingName.Text = "";
            txtExportFileName.Text = "";
            txtExportTemplateFileName.Text = "";
            cmbExportDestination.Text = "";
            RefreshExportSettingsCombo();
        }

        private void chkCanEditConfiguration_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveSettings.Visible = chkCanEditConfiguration.Checked;
            chkAutoRefresh.Visible = chkCanEditConfiguration.Checked;
            if (!chkAutoRefresh.Visible) chkAutoRefresh.Checked = true;
        }

        private void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableExportButtonIfNeeded();
        }

        private void cmbExport_TextChanged(object sender, EventArgs e)
        {
            EnableExportButtonIfNeeded();
        }

        private void chkAvailableActions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            foreach (Control c in fpActions.Controls)
            {
                if (c is Button)
                {
                    Button b = (Button)c;
                    b.Visible = (b.Text == chkAvailableActions.Items[e.Index].ToString()) && (e.NewValue == CheckState.Checked);
                }
            }
        }

        private void EnableActionButtonsAsNeeded()
        {
            if (actionInProgress) return;
            foreach (Control c in fpActions.Controls)
            {
                if (c is Button)
                {
                    Button b = (Button)c;
                    fpActions.Visible = true;
                    if (dctClassMapping.ContainsKey(b.Text) && (dctClassMapping[b.Text] != null))
                    {
                        AbstractEasyAction ea = (AbstractEasyAction)Activator.CreateInstance(dctClassMapping[b.Text].Class);
                        foreach (KeyValuePair<string, string> kvPair in dctActionFieldSettings[b.Text])
                        {
                            ea.SettingsDictionary.Add(kvPair.Key, kvPair.Value);
                        }

                        foreach (DataGridViewRow dRow in dataGrid.SelectedRows)
                        {
                            DataRow row = ((DataRowView)dRow.DataBoundItem).Row;
                            if (!ea.CanExecute(row))
                            {
                                b.Enabled = false;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            fpActions.Visible = false;
            if (dataGrid.SelectedRows.Count == 0) return;
            EnableActionButtonsAsNeeded();
        }

    }


    public class ExportSettings
    {
        public string SettingName;
        public string ExportFormat;
        public string ExportFileName;
        public string TemplateFileName;
        public bool IncludeTableHeader;
    }

}
