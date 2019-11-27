using EasyETL;
using EasyETL.Actions;
using EasyETL.Attributes;
using EasyETL.Endpoint;
using EasyETL.Extractors;
using EasyETL.Writers;
using EasyETL.Xml;
using EasyETL.Xml.Configuration;
using EasyETL.Xml.Parsers;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace EasyXmlSample
{

    public partial class ETLForm : Form
    {
        bool actionInProgress = false;
        EasyXmlDocument ezDoc = null;
        Stopwatch stopWatch = new Stopwatch();
        public EasyETLXmlDocument ConfigXmlDocument = null;
        public string ClientName;
        public string ETLName;
        public EasyETLClient ClientConfiguration = null;
        public EasyETLJobConfiguration JobConfiguration = null;

        public int intAutoNumber = 0;


        public void LoadSettingsFromXml()
        {

            #region load profiles
            cmbParserProfile.Items.Clear();
            cbTextExtractor.Items.Clear();
            ClientConfiguration = ConfigXmlDocument.Clients.Find(c => c.ClientName == ClientName);
            JobConfiguration = ClientConfiguration.ETLs.Find(etl => etl.ETLName == ETLName);
            foreach (ClassMapping extractorMapping in EasyETLEnvironment.Extractors)
            {
                cbTextExtractor.Items.Add(extractorMapping.DisplayName);
            }

            foreach (ClassMapping classMapping in EasyETLEnvironment.Parsers)
            {
                if ((classMapping.Class.GetEasyFields().Count() == 0) || (!classMapping.Class.GetEasyFields().Select(p => p.Value.DefaultValue).Contains(null))) cmbParserProfile.Items.Add(classMapping.DisplayName);
            }

            foreach (EasyETLParser easyETLParser in ClientConfiguration.Parsers)
            {
                cmbParserProfile.Items.Add(easyETLParser.ActionName);
            }
            #endregion


            #region load all available actions
            chkAvailableActions.Items.Clear();
            fpActions.Controls.Clear();
            foreach (EasyETLAction etlAction in ClientConfiguration.Actions)
            {
                string actionName = etlAction.ActionName;
                string className = etlAction.ClassName;
                if (EasyETLEnvironment.Actions.Where(a => a.DisplayName == className).Count() > 0)
                {
                    ClassMapping cMapping = EasyETLEnvironment.Actions.Where(a => a.DisplayName == className).First();
                    chkAvailableActions.Items.Add(etlAction.ActionName);
                    Button btnControl = new Button
                    {
                        AutoSize = true,
                        Text = etlAction.ActionName
                    };
                    btnControl.Click += btnControl_Click;
                    fpActions.Controls.Add(btnControl);
                }
            }

            #endregion

            #region load all available exports

            chkAvailableExports.Items.Clear();
            cmbExport.Items.Clear();

            foreach (EasyETLWriter easyETLWriter in ClientConfiguration.Writers)
            {
                string actionName = easyETLWriter.ActionName;
                string className = easyETLWriter.ClassName;
                ClassMapping exportClassMapping = EasyETLEnvironment.Writers.Find(w => w.DisplayName == className);
                if (exportClassMapping != null)
                {
                    chkAvailableExports.Items.Add(actionName);
                }
            }
            #endregion

            #region load all available data sources
            cmbDatasource.Items.Clear();
            foreach (EasyETLDatasource easyETLDatasource in ClientConfiguration.Datasources)
                cmbDatasource.Items.Add(easyETLDatasource.ActionName);
            #endregion

            #region load all available endpoints
            cmbEndpoint.Items.Clear();
            foreach (EasyETLEndpoint easyETLEndpoint in ClientConfiguration.Endpoints) cmbEndpoint.Items.Add(easyETLEndpoint.ActionName);
            #endregion


            #region Actions Load
            foreach (EasyETLJobAction easyETLJobAction in JobConfiguration.Actions)
            {
                int loadIndex = chkAvailableActions.Items.IndexOf(easyETLJobAction.ActionName);
                bool toCheck = (easyETLJobAction.IsEnabled);
                if (loadIndex >= 0) chkAvailableActions.SetItemChecked(loadIndex, toCheck);
                if (easyETLJobAction.IsDefault) cmbDefaultAction.Text = easyETLJobAction.ActionName;
            }

            DisplayActionButtonsAsNeeded();
            #endregion

            #region Transformations Load
            cbOnLoadProfiles.Text = JobConfiguration.Transformations.DuringLoad.SaveProfileName;
            txtOnLoadFileName.Text = JobConfiguration.Transformations.DuringLoad.SaveFileName;
            txtOnLoadContents.Text = String.Join(Environment.NewLine, JobConfiguration.Transformations.DuringLoad.SettingsCommands);

            chkUseXsltTemplate.Checked = JobConfiguration.Transformations.AfterLoad.UseXslt;
            cbTransformProfiles.Text = JobConfiguration.Transformations.AfterLoad.SaveProfileName;
            txtTransformFileName.Text = JobConfiguration.Transformations.AfterLoad.SaveFileName;
            txtXsltFileName.Text = JobConfiguration.Transformations.AfterLoad.XsltFileName;
            txtTransformText.Text = String.Join(Environment.NewLine, JobConfiguration.Transformations.AfterLoad.SettingsCommands);

            rbUseDatasetLoad.Checked = JobConfiguration.Transformations.Dataset.UseDefault;
            rbUseCustomTableLoad.Checked = JobConfiguration.Transformations.Dataset.UseCustom;
            txtNodeMapping.Text = String.Join(Environment.NewLine, JobConfiguration.Transformations.Dataset.SettingsCommands);

            #endregion

            #region ParserOptions Load
            cmbParserProfile.Text = JobConfiguration.ParseOptions.ProfileName;
            #endregion

            #region Output Settings Load

            foreach (EasyETLJobExport easyETLJobExport in JobConfiguration.Exports)
            {
                int loadIndex = chkAvailableExports.Items.IndexOf(easyETLJobExport.ExportName);
                bool toCheck = (easyETLJobExport.IsEnabled);
                if (loadIndex >= 0) chkAvailableExports.SetItemChecked(loadIndex, toCheck);
            }
            #endregion

            #region Permissions Settings
            btnSaveSettings.Visible = false;
            EasyETLPermission etlPermission = JobConfiguration.DefaultPermission;
            chkCanEditConfiguration.Checked = etlPermission.CanEditSettings;
            chkShowConfigurationOnLoad.Checked = etlPermission.DisplaySettingsOnLoad;
            chkCanAddData.Checked = etlPermission.CanAddData;
            chkCanEditData.Checked = etlPermission.CanEditData;
            chkCanDeleteData.Checked = etlPermission.CanDeleteData;
            chkCanExportData.Checked = etlPermission.CanExportData;

            ToggleDisplayConfigurationSection(chkShowConfigurationOnLoad.Checked && etlPermission.CanViewSettings);

            chkAutoRefresh.Visible = chkCanEditConfiguration.Checked;
            btnSaveSettings.Visible = chkCanEditConfiguration.Checked;
            pnlExport.Visible = chkCanExportData.Checked;

            #endregion

            #region Datasource Node Load
            tabDataSource.SelectTab("tabDatasource" + JobConfiguration.Datasource.SourceType);
            cmbEndpoint.Text = JobConfiguration.Datasource.Endpoint;
            txtFileName.Text = JobConfiguration.Datasource.FileName;
            chkUseTextExtractor.Checked = JobConfiguration.Datasource.UseTextExtractor;
            cbTextExtractor.Text = JobConfiguration.Datasource.TextExtractor;
            chkHasMaxRows.Checked = JobConfiguration.Datasource.HasMaxRows;
            nudMaxRows.Value = JobConfiguration.Datasource.MaxRows;
            cmbDatasource.Text = JobConfiguration.Datasource.DataSource;
            txtTextContents.Text = JobConfiguration.Datasource.TextContents;
            txtDatabaseQuery.Text = JobConfiguration.Datasource.Query;

            #endregion

            chkAutoRefresh.Checked = JobConfiguration.AutoRefresh; // (xNode.Attributes.GetNamedItem("autorefresh") == null) ? false : Boolean.Parse(xNode.Attributes.GetNamedItem("autorefresh").Value);

        }

        private void DisplayActionButtonsAsNeeded()
        {
            foreach (Control c in fpActions.Controls)
            {
                if (c is Button b)
                {
                    b.Visible = chkAvailableActions.CheckedItems.Contains(b.Text);
                }
            }
        }

        void btnControl_Click(object sender, EventArgs e)
        {
            actionInProgress = true;
            string actionName = ((Button)sender).Text;
            if (ClientConfiguration.Actions.Find(a => a.ActionName == actionName) != null)
            {
                AbstractEasyAction aea = ClientConfiguration.Actions.Find(a => a.ActionName == actionName).CreateAction();

                List<DataRow> dataRows = new List<DataRow>();
                foreach (DataGridViewRow dgvRow in dataGrid.SelectedRows)
                {
                    if (dgvRow.DataBoundItem != null) dataRows.Add(((DataRowView)dgvRow.DataBoundItem).Row);
                }
                if (dataRows.Count == 0)
                {
                    if (dataGrid.SelectedCells.Count > 0)
                    {
                        List<int> lstRowIndex = new List<int>();
                        foreach (DataGridViewCell cell in dataGrid.SelectedCells)
                        {
                            if (!lstRowIndex.Contains(cell.RowIndex)) lstRowIndex.Add(cell.RowIndex);
                        }
                        foreach (int rowIndex in lstRowIndex)
                        {
                            if (dataGrid.Rows[rowIndex].DataBoundItem != null) dataRows.Add(((DataRowView)dataGrid.Rows[rowIndex].DataBoundItem).Row);
                        }
                    }
                }
                pbProgress.Maximum = dataRows.Count;
                aea.OnProgress += aea_OnProgress;
                pbProgress.Visible = true;
                lblProgress.Visible = true;
                aea.Execute(dataRows.ToArray());
                lblProgress.Visible = false;
                pbProgress.Visible = false;
            }
            actionInProgress = false;
            EnableActionButtonsAsNeeded();
        }

        void aea_OnProgress(object sender, EasyActionProgressEventArgs e)
        {
            pbProgress.Value = e.CurrentIndex;
            double dblProgress = (e.CurrentIndex * 1.0) / pbProgress.Maximum;
            lblProgress.Text = dblProgress.ToString("P");
            Application.DoEvents();
        }

        public ETLForm()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(cmbEndpoint.Text)) return;

            AbstractFileEasyEndpoint endpoint = GetEndpoint();
            if (endpoint == null) return;
            if (!endpoint.CanFunction()) return;

            using (EndpointFilesForm epfForm = new EndpointFilesForm())
            {
                epfForm.LoadEndPoint(endpoint, cmbEndpoint.Text);
                if (epfForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFileName.Text = epfForm.FileName;
                }
            }

        }

        private AbstractFileEasyEndpoint GetEndpoint()
        {
            AbstractFileEasyEndpoint endpoint = null;
            if ((cmbEndpoint.SelectedItem != null) && (ClientConfiguration.Endpoints.Find(ep => ep.ActionName == cmbEndpoint.SelectedItem.ToString()) != null))
            {
                endpoint = (AbstractFileEasyEndpoint)ClientConfiguration.Endpoints.Find(ep => ep.ActionName == cmbEndpoint.SelectedItem.ToString()).CreateEndpoint();
            }
            return endpoint;
        }

        public void LoadDataToGridView()
        {
            if (stopWatch.IsRunning) stopWatch.Stop();
            if ((tabDataSource.SelectedTab == tabDatasourceText) && (txtTextContents.TextLength == 0)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceFile) && (txtFileName.TextLength == 0)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceDatabase) && (String.IsNullOrWhiteSpace(cmbDatasource.Text))) return;
            AbstractFileEasyEndpoint endpoint = null;
            if (tabDataSource.SelectedTab == tabDatasourceFile)
            {
                endpoint = GetEndpoint();
                if (endpoint == null) return;
                if (endpoint.GetList(txtFileName.Text).Length == 0) return;
            }
            stopWatch.Restart();
            intAutoNumber = 0;
            this.UseWaitCursor = true;
            Application.DoEvents();
            lblRecordCount.Text = "";
            txtRegexContents.Text = "";
            cmbTableName.Items.Clear();
            AbstractContentExtractor extractor = null;
            if ((chkUseTextExtractor.Checked) && (EasyETLEnvironment.Extractors.Find(e => e.DisplayName == cbTextExtractor.Text) != null))
            {
                extractor = (AbstractContentExtractor)Activator.CreateInstance(EasyETLEnvironment.Extractors.Find(e => e.DisplayName == cbTextExtractor.Text).Class);
            }
            try
            {
                EasyXmlDocument xDoc = new EasyXmlDocument();
                xDoc.OnProgress += xDoc_OnProgress;
                if (tabDataSource.SelectedTab == tabDatasourceDatabase)
                {
                    DatasourceEasyParser dbep = null;
                    if ((cmbDatasource.SelectedItem != null) && (ClientConfiguration.Datasources.Find(ds => ds.ActionName == cmbDatasource.SelectedItem.ToString()) != null))
                    {
                        dbep = ClientConfiguration.Datasources.Find(ds => ds.ActionName == cmbDatasource.SelectedItem.ToString()).CreateDatasource();
                        if (chkHasMaxRows.Checked) dbep.MaxRecords = Convert.ToInt64(nudMaxRows.Value);
                        xDoc.LoadXml(dbep.Load(txtDatabaseQuery.Text).OuterXml);
                    }
                }
                else
                {
                    AbstractEasyParser ep = null;
                    if (ClientConfiguration.Parsers.Where(p => p.ActionName == cmbParserProfile.Text).Count() > 0)
                    {
                        ep = ClientConfiguration.Parsers.Find(p => p.ActionName == cmbParserProfile.Text).CreateParser();
                    }

                    if ((ep == null) && (EasyETLEnvironment.Parsers.Where(p => p.DisplayName == cmbParserProfile.Text).Count() > 0))
                    {
                        ClassMapping parserClassMapping = EasyETLEnvironment.Parsers.Where(p => p.DisplayName == cmbParserProfile.Text).First();
                        ep = (AbstractEasyParser)Activator.CreateInstance(parserClassMapping.Class);
                    }

                    if (ep != null)
                    {
                        ep.ProgressInterval = 100;
                        if (chkHasMaxRows.Checked) ep.MaxRecords = Convert.ToInt64(nudMaxRows.Value);
                        if (!String.IsNullOrWhiteSpace(txtOnLoadContents.Text))
                            ep.OnLoadSettings = txtOnLoadContents.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        if ((tabDataSource.SelectedTab == tabDatasourceFile))
                        {
                            Stream fileStream = endpoint.GetStream(txtFileName.Text);
                            if (extractor != null) extractor.FileName = txtFileName.Text;
                            xDoc.Load(fileStream, ep, extractor);
                        }
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
                }
                ezDoc = xDoc;
                try
                {
                    TransformDataFromEzDoc();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Transforming document." + Environment.NewLine + ex.Message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading contents..." + Environment.NewLine + ex.Message);
            }
            endpoint = null;
            this.UseWaitCursor = false;
        }

        void xDoc_OnProgress(object sender, EasyParserProgressEventArgs e)
        {
            lblRecordCount.Text = "(" + e.LineNumber.ToString() + ") Records Processed....";
            Application.DoEvents();
        }

        private void TransformDataFromEzDoc()
        {
            if (!stopWatch.IsRunning) stopWatch.Restart();
            lblRecordCount.Text = "";
            cmbTableName.Items.Clear();
            EasyXmlDocument xDoc;
            try
            {
                xDoc = (EasyXmlDocument)ezDoc.Clone();
                if (!String.IsNullOrWhiteSpace(txtTransformText.Text))
                {
                    xDoc.Transform(txtTransformText.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None), Properties.Settings.Default.XsltExtensionNamespace);
                }
                txtXPathContents.Text = xDoc.LastTransformerTemplate;
                if (chkUseXsltTemplate.Checked && File.Exists(txtXsltFileName.Text))
                {
                    //Transform the xml using the xslt file
                    ezDoc.LoadXml(xDoc.Transform(txtXsltFileName.Text).OuterXml);
                }
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
            catch (Exception ex)
            {
            }

            fpActions.Visible = false;
            try
            {

                DataSet ds;
                if (rbUseDatasetLoad.Checked)
                {
                    ds = xDoc.ToDataSet();
                }
                else
                {
                    //we need to use custom load
                    ds = new DataSet();
                    foreach (string strNodeMapping in txtNodeMapping.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string tableName = strNodeMapping;
                        string nodePath = strNodeMapping;
                        if (strNodeMapping.Contains('='))
                        {
                            tableName = strNodeMapping.Substring(0, strNodeMapping.IndexOf('='));
                            nodePath = strNodeMapping.Substring(strNodeMapping.IndexOf('=') + 1);
                        }
                        DataTable dataTable = ds.Tables.Add(tableName);
                        XmlNodeList xmlNodeList = xDoc.SelectNodes(nodePath);
                        if ((xmlNodeList != null) && (xmlNodeList.Count > 0))
                        {
                            foreach (XmlNode columnNode in xmlNodeList[0])
                                dataTable.Columns.Add(columnNode.Name);
                            foreach (XmlNode xmlNode in xmlNodeList)
                            {
                                DataRow dataRow = dataTable.NewRow();
                                foreach (XmlNode columnNode in xmlNode)
                                {
                                    dataRow[columnNode.Name] = columnNode.InnerText;
                                }
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }
                }
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
            cmbDatasource.SelectedItem = 0;
            btnRefreshOnLoadProfiles_Click(this, null);
            btnTransformProfilesLoad_Click(this, null);
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

        private void txtTextContents_Leave(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked) LoadDataToGridView();
        }

        private void cbTransformProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTransformFileName.Text = cbTransformProfiles.Text;
            EasyETLTransform transform = ConfigXmlDocument.Transforms.Find(t => t.ProfileName == txtTransformFileName.Text);
            if (transform != null) txtTransformText.Text = String.Join(Environment.NewLine, transform.SettingsCommands);
        }

        private void btnTransformProfilesLoad_Click(object sender, EventArgs e)
        {
            cbTransformProfiles.Items.Clear();
            foreach (EasyETLTransform transform in ConfigXmlDocument.Transforms)
            {
                cbTransformProfiles.Items.Add(transform.ProfileName);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (cmbExport.SelectedItem == null) return;
            string exportName = cmbExport.SelectedItem.ToString();
            if ((String.IsNullOrWhiteSpace(exportName)) || (ClientConfiguration.Writers.Find(w => w.ActionName == exportName) == null)) return;
            DataSet rds = (DataSet)dataGrid.DataSource;
            DatasetWriter dsw = ClientConfiguration.Writers.Find(w => w.ActionName == exportName).CreateWriter();
            if (!dsw.IsFieldSettingsComplete())
            {
                MessageBox.Show("The configuration of this export setting is incomplete.");
                return;
            }

            dsw.Write(rds);
            if (dsw is FileDatasetWriter)
            {
                string exportFileName = ((FileDatasetWriter)dsw).ExportFileName;
                MessageBox.Show("Saved file as " + exportFileName);
                Process.Start(exportFileName);
            }
        }

        private void btnTransformSave_Click(object sender, EventArgs e)
        {
            EasyETLTransform transform = ConfigXmlDocument.Transforms.Find(t => t.ProfileName == txtTransformFileName.Text);
            if (transform == null)
            {
                transform = new EasyETLTransform() { ProfileName = txtTransformFileName.Text };
                ConfigXmlDocument.Transforms.Add(transform);
            }
            transform.SettingsCommands = txtTransformText.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            ConfigXmlDocument.Save();
            if (!cbTransformProfiles.Items.Contains(transform.ProfileName)) cbTransformProfiles.Items.Add(transform.ProfileName);
            cbTransformProfiles.SelectedText = String.Join(Environment.NewLine, transform.SettingsCommands);

            //if (!String.IsNullOrWhiteSpace(txtTransformFileName.Text))
            //{

            //    XmlDocument xDoc = new XmlDocument();
            //    xDoc.Load(SettingsFileName);
            //    XmlNode transformsNode = xDoc.SelectSingleNode("//transforms");
            //    if (transformsNode == null)
            //    {
            //        transformsNode = xDoc.CreateElement("transforms");
            //        xDoc.DocumentElement.AppendChild(transformsNode);
            //    }
            //    XmlElement transformNode = (XmlElement)xDoc.SelectSingleNode("//transforms/transform[@profilename='" + txtTransformFileName.Text + "']");
            //    if (transformNode == null)
            //    {
            //        transformNode = xDoc.CreateElement("transform");
            //        transformNode.SetAttribute("profilename", txtTransformFileName.Text);
            //        transformsNode.AppendChild(transformNode);
            //    }
            //    transformNode.InnerText = txtTransformText.Text;
            //    xDoc.Save(SettingsFileName);
            //    if (!cbTransformProfiles.Items.Contains(txtTransformFileName.Text)) cbTransformProfiles.Items.Add(txtTransformFileName.Text);
            //    cbTransformProfiles.SelectedText = txtOnLoadFileName.Text;
            //}

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
            foreach (EasyETLTransform transform in ConfigXmlDocument.Transforms)
            {
                cbOnLoadProfiles.Items.Add(transform.ProfileName);
            }
        }

        private void cbOnLoadProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOnLoadFileName.Text = cbOnLoadProfiles.Text;
            EasyETLTransform transform = ConfigXmlDocument.Transforms.Find(t => t.ProfileName == txtOnLoadFileName.Text);
            if (transform != null) txtOnLoadContents.Text = String.Join(Environment.NewLine, transform.SettingsCommands);
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettingsToXmlFile();
        }

        public void SaveSettingsToXmlFile()
        {
            
            JobConfiguration.AutoRefresh = chkAutoRefresh.Checked;
            #region Datasource Section
            JobConfiguration.Datasource.SourceType = tabDataSource.SelectedTab.Text;

            switch (tabDataSource.SelectedTab.Text)
            {
                case "File":
                    JobConfiguration.Datasource.Endpoint = cmbEndpoint.Text;
                    JobConfiguration.Datasource.UseTextExtractor = chkUseTextExtractor.Checked;
                    JobConfiguration.Datasource.TextExtractor = cbTextExtractor.Text;
                    JobConfiguration.Datasource.HasMaxRows = chkHasMaxRows.Checked;
                    JobConfiguration.Datasource.MaxRows = Convert.ToInt32(nudMaxRows.Value);
                    JobConfiguration.Datasource.FileName = txtFileName.Text;
                    break;
                case "Text":
                    JobConfiguration.Datasource.TextContents = txtTextContents.Text;
                    break;
                case "Database":
                    JobConfiguration.Datasource.DataSource = cmbDatasource.Text;
                    JobConfiguration.Datasource.Query = txtDatabaseQuery.Text;
                    break;
            }
            #endregion

            #region Actions section
            JobConfiguration.Actions.Clear();
            foreach (var cic in chkAvailableActions.CheckedItems)
            {
                EasyETLJobAction action = new EasyETLJobAction() { ActionName = cic.ToString(), IsEnabled = true, IsDefault = (cic.ToString() == cmbDefaultAction.Text) };
                JobConfiguration.Actions.Add(action);
            }
            #endregion

            #region Transformations section
            JobConfiguration.Transformations.DuringLoad.SaveProfileName = cbOnLoadProfiles.Text;
            JobConfiguration.Transformations.DuringLoad.SaveFileName = txtOnLoadFileName.Text;
            JobConfiguration.Transformations.DuringLoad.SettingsCommands = txtOnLoadContents.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            JobConfiguration.Transformations.AfterLoad.SaveProfileName = cbTransformProfiles.Text;
            JobConfiguration.Transformations.AfterLoad.SaveFileName = txtTransformFileName.Text;
            JobConfiguration.Transformations.AfterLoad.SettingsCommands = txtTransformText.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            JobConfiguration.Transformations.AfterLoad.UseXslt = chkUseXsltTemplate.Checked;
            JobConfiguration.Transformations.AfterLoad.XsltFileName = txtXsltFileName.Text;

            JobConfiguration.Transformations.Dataset.UseDefault = rbUseDatasetLoad.Checked;
            JobConfiguration.Transformations.Dataset.UseCustom = rbUseCustomTableLoad.Checked;
            JobConfiguration.Transformations.Dataset.SettingsCommands = txtNodeMapping.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            #endregion

            #region Parse Options section
            JobConfiguration.ParseOptions.ProfileName = cmbParserProfile.Text;
            #endregion

            #region Export Settings
            JobConfiguration.Exports.Clear();
            foreach (var cic in chkAvailableExports.CheckedItems)
            {
                EasyETLJobExport export = new EasyETLJobExport() { ExportName = cic.ToString(), IsEnabled = true};
                JobConfiguration.Exports.Add(export);
            }
            #endregion

            #region Permissions Settings
            JobConfiguration.DefaultPermission.Role = "owner";
            JobConfiguration.DefaultPermission.CanViewSettings = tableLayoutPanel1.RowStyles[0].Height > 0;
            JobConfiguration.DefaultPermission.DisplaySettingsOnLoad = chkShowConfigurationOnLoad.Checked;
            JobConfiguration.DefaultPermission.CanEditSettings = chkShowConfigurationOnLoad.Checked;
            JobConfiguration.DefaultPermission.CanAddData = chkCanAddData.Checked;
            JobConfiguration.DefaultPermission.CanEditData = chkCanEditData.Checked;
            JobConfiguration.DefaultPermission.CanDeleteData = chkCanDeleteData.Checked;
            JobConfiguration.DefaultPermission.CanExportData = chkCanExportData.Checked;
            #endregion

            ConfigXmlDocument.Save();
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

        private void chkCanEditConfiguration_CheckedChanged(object sender, EventArgs e)
        {
            chkAutoRefresh.Visible = chkCanEditConfiguration.Checked;
            if (!chkAutoRefresh.Visible) chkAutoRefresh.Checked = true;
            if (chkCanEditConfiguration.Checked) btnSaveSettings.Visible = true;
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
                if ((c is Button) && (((Button)c).Text == chkAvailableActions.Items[e.Index].ToString()))
                {
                    Button b = (Button)c;
                    b.Visible = (e.NewValue == CheckState.Checked);
                }
            }
            if (e.NewValue == CheckState.Checked)
            {
                cmbDefaultAction.Items.Add(chkAvailableActions.Items[e.Index].ToString());
            }
            else
            {
                cmbDefaultAction.Items.Remove(chkAvailableActions.Items[e.Index].ToString());
            }
        }

        private void EnableActionButtonsAsNeeded(DataRow dataRow = null)
        {
            if (actionInProgress) return;
            List<DataRow> dataRows = null;
            foreach (Control c in fpActions.Controls)
            {
                if (c is Button b)
                {
                    fpActions.Visible = true;
                    b.Enabled = false;
                    if ((b.Visible) && (ClientConfiguration.Actions.Find(a => a.ActionName == b.Text) != null))
                    {
                        if (dataRows == null)
                        {
                            dataRows = new List<DataRow>();
                            if (dataRow == null)
                            {
                                foreach (DataGridViewRow dRow in dataGrid.SelectedRows)
                                {
                                    if (dRow.DataBoundItem != null) dataRows.Add(((DataRowView)dRow.DataBoundItem).Row);
                                }
                                if (dataRows.Count == 0)
                                {
                                    if (dataGrid.SelectedCells.Count > 0)
                                    {
                                        List<int> lstRowIndex = new List<int>();
                                        foreach (DataGridViewCell cell in dataGrid.SelectedCells)
                                        {
                                            if (!lstRowIndex.Contains(cell.RowIndex)) lstRowIndex.Add(cell.RowIndex);
                                        }
                                        foreach (int rowIndex in lstRowIndex)
                                        {
                                            if (dataGrid.Rows[rowIndex].DataBoundItem != null) dataRows.Add(((DataRowView)dataGrid.Rows[rowIndex].DataBoundItem).Row);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dataRows.Add(dataRow);
                            }
                        }
                        AbstractEasyAction ea = ClientConfiguration.Actions.Where(a => a.ActionName == b.Text).First().CreateAction();
                        //(AbstractEasyAction)Activator.CreateInstance(dctActionClassMapping[b.Text].Class);
                        //foreach (KeyValuePair<string, string> kvPair in dctActionFieldSettings[b.Text])
                        //{
                        //    ea.SettingsDictionary.Add(kvPair.Key, kvPair.Value);
                        //}
                        b.Enabled = ea.CanExecute(dataRows.ToArray());
                    }
                }
            }
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            fpActions.Visible = false;
            if ((dataGrid.SelectedRows.Count == 0) && (dataGrid.SelectedCells.Count == 0)) return;
            EnableActionButtonsAsNeeded();
        }

        private void dataGrid_DoubleClick(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(cmbDefaultAction.Text)) return;
            foreach (Control c in fpActions.Controls)
            {
                if ((c.Visible) && (c.Enabled) && (c is Button) && (c.Text == cmbDefaultAction.Text))
                {
                    btnControl_Click(c, null);
                }
            }

        }

        private void btnShowSettings_Click(object sender, EventArgs e)
        {
            ToggleDisplayConfigurationSection(true);
        }

        private void ToggleDisplayConfigurationSection(bool bShow)
        {
            if (bShow)
            {
                btnShowSettings.Visible = false;
                tableLayoutPanel1.RowStyles[0].Height = 232;
            }
            else
            {
                btnShowSettings.Visible = true;
                tableLayoutPanel1.RowStyles[0].Height = 1;
            }
        }

        private void btnHideSettings_Click(object sender, EventArgs e)
        {
            ToggleDisplayConfigurationSection(false);
        }

        private void chkAvailableExports_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                cmbExport.Items.Add(chkAvailableExports.Items[e.Index].ToString());
            }
            else
            {
                cmbExport.Items.Remove(chkAvailableExports.Items[e.Index].ToString());
            }
        }

        private void dataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGrid_DoubleClick(this, null);
        }

        private void chkUseXsltTemplate_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowseXsltFileName.Enabled = chkUseXsltTemplate.Checked;
        }

        private void btnBrowseXsltFileName_Click(object sender, EventArgs e)
        {
            ofd.FileName = txtXsltFileName.Text;
            if (ofd.ShowDialog(this) == DialogResult.OK)
                txtXsltFileName.Text = ofd.FileName;
        }

        private void rbUseCustomTableLoad_CheckedChanged(object sender, EventArgs e)
        {
            txtNodeMapping.Enabled = rbUseCustomTableLoad.Checked;
        }
    }


}
