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
        XslCompiledTransform xsl = null;
        Stopwatch stopWatch = new Stopwatch();
        public EasyETLXmlDocument ConfigXmlDocument = null;
        public EasyETLClient ClientConfiguration = null;

        public int intAutoNumber = 0;
        public string SettingsPath = "";
        public string SettingsFileName = "";


        public void LoadSettingsFromXml(string settingsPath)
        {
            if (String.IsNullOrWhiteSpace(SettingsFileName)) return;
            if (!File.Exists(SettingsFileName)) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            string clientName = settingsPath.Split('\\')[0];
            string etlName = settingsPath.Split('\\')[2];
            SettingsPath = "//clients/client[@name='" + clientName + "']/etls/etl[@name='" + etlName + "']";

            #region load profiles
            cmbParserProfile.Items.Clear();
            cbTextExtractor.Items.Clear();
            if (ConfigXmlDocument != null)
            {
                if (ClientConfiguration == null) ClientConfiguration = ConfigXmlDocument.Clients.Find(w => w.ClientName == clientName);

                foreach (ClassMapping extractorMapping in EasyETLEnvironment.Extractors)
                {
                    cbTextExtractor.Items.Add(extractorMapping.DisplayName);
                }

                foreach (ClassMapping classMapping in EasyETLEnvironment.Parsers)
                {
                    if ((classMapping.Class.GetEasyFields().Count() == 0) || (!classMapping.Class.GetEasyFields().Select(p=>p.Value.DefaultValue).Contains(null))) cmbParserProfile.Items.Add(classMapping.DisplayName);
                }

                foreach (EasyETLParser easyETLParser in ClientConfiguration.Parsers)
                {
                    cmbParserProfile.Items.Add(easyETLParser.ActionName);
                }
            }
            #endregion


            #region load all available actions
            chkAvailableActions.Items.Clear();
            fpActions.Controls.Clear();
            //dctActionClassMapping.Clear();
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
                    //dctActionClassMapping.Add(actionName, cMapping);
                }
            }

            XmlNodeList actionNodes = xDoc.SelectNodes("//clients/client[@name='" + clientName + "']/actions/action");

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
                    XmlNode defaultActionNode = actionsNode.SelectSingleNode("defaultaction");
                    if ((defaultActionNode != null) && (defaultActionNode.Attributes.GetNamedItem("name") != null))
                    {
                        cmbDefaultAction.Text = defaultActionNode.Attributes.GetNamedItem("name").Value;
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
                    chkUseXsltTemplate.Checked = false;
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
                                case "usexslt":
                                    chkUseXsltTemplate.Checked = Convert.ToBoolean(xAttr.Value); break;
                                case "xsltfilename":
                                    txtXsltFileName.Text = xAttr.Value; break;
                            }
                        }
                        txtTransformText.Text = afterLoadNode.InnerText;
                    }
                    XmlNode datasetConversionNode = transformationNode.SelectSingleNode("datasetconversion");
                    rbUseDatasetLoad.Checked = true;
                    if (datasetConversionNode != null)
                    {
                        foreach (XmlAttribute xAttr in datasetConversionNode.Attributes)
                        {
                            switch (xAttr.Name.ToLower())
                            {
                                case "usedefault":
                                    rbUseDatasetLoad.Checked = Convert.ToBoolean(xAttr.Value); break;
                                case "usecustom":
                                    rbUseCustomTableLoad.Checked = Convert.ToBoolean(xAttr.Value); break;
                            }
                        }
                        txtNodeMapping.Text = datasetConversionNode.InnerText;
                    }
                }
                #endregion

                #region ParserOptions Load
                XmlNode parseOptionsNode = xNode.SelectSingleNode("parseoptions");
                if ((parseOptionsNode !=null) && (parseOptionsNode.Attributes.GetNamedItem("profilename") != null))
                {
                    cmbParserProfile.Text = parseOptionsNode.Attributes.GetNamedItem("profilename").Value;
                }
                #endregion
                #region Output Settings Load

                XmlNode exportsNode = xNode.SelectSingleNode("exports");
                XmlNodeList exportNodes = null;
                if (exportsNode != null) exportNodes = exportsNode.SelectNodes("export");
                if (exportNodes != null)
                {

                    foreach (XmlNode exportNode in exportNodes)
                    {
                        if (exportNode.Attributes.GetNamedItem("name") != null)
                        {
                            int loadIndex = chkAvailableExports.Items.IndexOf(exportNode.Attributes.GetNamedItem("name").Value);
                            bool toCheck = !((exportNode.Attributes.GetNamedItem("enabled") != null) && (!Convert.ToBoolean(exportNode.Attributes.GetNamedItem("enabled").Value)));
                            if (loadIndex >= 0) chkAvailableExports.SetItemChecked(loadIndex, toCheck);
                        }
                    }
                }
                #endregion

                #region Permissions Settings
                btnSaveSettings.Visible = false;
                XmlNode permissionsNode = xNode.SelectSingleNode("permissions");
                if (permissionsNode != null)
                {
                    foreach (XmlAttribute xAttr in permissionsNode.Attributes)
                    {
                        switch (xAttr.Name.ToLower())
                        {
                            case "canviewsettings":
                                ToggleDisplayConfigurationSection(Convert.ToBoolean(xAttr.Value));
                                break;
                            case "caneditsettings":
                                chkCanEditConfiguration.Checked = Convert.ToBoolean(xAttr.Value); break;
                            case "displaysettingsonload":
                                chkShowConfigurationOnLoad.Checked = Convert.ToBoolean(xAttr.Value);
                                ToggleDisplayConfigurationSection(chkShowConfigurationOnLoad.Checked);
                                break;
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
                    btnSaveSettings.Visible = chkCanEditConfiguration.Checked;
                    pnlExport.Visible = chkCanExportData.Checked;
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
                            case "endpoint":
                                cmbEndpoint.Text = xAttr.Value; break;
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
                            case "datasource":
                                cmbDatasource.Text = xAttr.Value; break;
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
                //foreach (KeyValuePair<string, string> kvPair in dctActionFieldSettings[actionName])
                //{
                //    aea.SettingsDictionary.Add(kvPair.Key, kvPair.Value);
                //}
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

        private void LoadParseOptionsFromNode(XmlNode optionsNode)
        {
            foreach (XmlAttribute xAttr in optionsNode.Attributes)
            {
                switch (xAttr.Name.ToLower())
                {
                    case "parsertype":
                        cmbFileType.SelectedItem = xAttr.Value;
                        cmbFileType_SelectedIndexChanged(this, null);
                        break;
                    case "delimiter":
                        string delimiter = xAttr.Value;
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
            if ((cmbEndpoint.SelectedItem != null) && (ClientConfiguration.Endpoints.Find(ep=>ep.ActionName == cmbEndpoint.SelectedItem.ToString()) !=null))
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
                extractor =   (AbstractContentExtractor)Activator.CreateInstance(EasyETLEnvironment.Extractors.Find(e => e.DisplayName == cbTextExtractor.Text).Class);
            }
            try
            {
                xsl = null;
                EasyXmlDocument xDoc = new EasyXmlDocument();
                xDoc.OnProgress += xDoc_OnProgress;
                //xDoc.OnRowAdd += xDoc_OnRowAdd;
                //xDoc.OnError += xDoc_OnError;
                if (tabDataSource.SelectedTab == tabDatasourceDatabase)
                {
                    DatasourceEasyParser dbep = null;
                    if ((cmbDatasource.SelectedItem != null) && (ClientConfiguration.Datasources.Find(ds => ds.ActionName == cmbDatasource.SelectedItem.ToString()) !=null))
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
                if (!String.IsNullOrWhiteSpace(txtXPathQuery.Text))
                {
                    xDoc = xDoc.ApplyFilter(txtXPathQuery.Text);
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
            catch
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
            cmbFileType.SelectedIndex = 0;
            cmbDatasource.SelectedItem = 0;
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
            string exportName = cmbExport.SelectedItem.ToString();
            if ((String.IsNullOrWhiteSpace(exportName)) || (ClientConfiguration.Writers.Find(w=>w.ActionName == exportName) == null)) return;
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
                string parentPath = SettingsPath.Remove(SettingsPath.IndexOf("/etls/etl") + 5);
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
                    datasourceNode.SetAttribute("endpoint", cmbEndpoint.Text);
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
                    datasourceNode.SetAttribute("datasource", cmbDatasource.Text);
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

            if (!String.IsNullOrWhiteSpace(cmbDefaultAction.Text))
            {
                XmlElement defaultActionNode = xDoc.CreateElement("defaultaction");
                defaultActionNode.SetAttribute("name", cmbDefaultAction.Text);
                actionsNode.AppendChild(defaultActionNode);
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
            afterLoadNode.SetAttribute("usexslt", chkUseXsltTemplate.Checked.ToString());
            afterLoadNode.SetAttribute("xsltfilename", txtXsltFileName.Text);
            afterLoadNode.InnerText = txtTransformText.Text;
            transformationNode.AppendChild(duringLoadNode);
            transformationNode.AppendChild(afterLoadNode);
            XmlElement datasetConversionNode = xDoc.CreateElement("datasetconversion");
            datasetConversionNode.SetAttribute("usedefault", rbUseDatasetLoad.Checked.ToString());
            datasetConversionNode.SetAttribute("usecustom", rbUseCustomTableLoad.Checked.ToString());
            datasetConversionNode.InnerText = txtNodeMapping.Text;
            transformationNode.AppendChild(datasetConversionNode);
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
            //#region Actions section
            XmlElement exportsNode = xDoc.CreateElement("exports");
            foreach (var cic in chkAvailableExports.CheckedItems)
            {
                XmlElement exportNode = xDoc.CreateElement("export");
                exportNode.SetAttribute("name", cic.ToString());
                exportNode.SetAttribute("enabled", "True");
                exportsNode.AppendChild(exportNode);
            }
            xNode.AppendChild(exportsNode);
            #endregion

            #region Permissions Settings
            XmlElement permissionsNode = xDoc.CreateElement("permissions");
            permissionsNode.SetAttribute("role", "owner");
            permissionsNode.SetAttribute("canviewsettings", (tableLayoutPanel1.RowStyles[0].Height > 0).ToString());
            permissionsNode.SetAttribute("displaysettingsonload", chkShowConfigurationOnLoad.Checked.ToString());
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
