namespace EasyXmlSample
{
    partial class ETLForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ETLForm));
            this.btnCloseWindow = new System.Windows.Forms.Button();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabDuringLoad = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.txtOnLoadFileName = new System.Windows.Forms.TextBox();
            this.btnOnLoadSave = new System.Windows.Forms.Button();
            this.txtOnLoadContents = new System.Windows.Forms.TextBox();
            this.btnRefreshOnLoadProfiles = new System.Windows.Forms.Button();
            this.cbOnLoadProfiles = new System.Windows.Forms.ComboBox();
            this.tabAfterLoad = new System.Windows.Forms.TabPage();
            this.btnBrowseXsltFileName = new System.Windows.Forms.Button();
            this.txtXsltFileName = new System.Windows.Forms.TextBox();
            this.chkUseXsltTemplate = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTransformFileName = new System.Windows.Forms.TextBox();
            this.btnTransformSave = new System.Windows.Forms.Button();
            this.txtTransformText = new System.Windows.Forms.TextBox();
            this.btnTransformProfilesLoad = new System.Windows.Forms.Button();
            this.cbTransformProfiles = new System.Windows.Forms.ComboBox();
            this.tabDataSetConversion = new System.Windows.Forms.TabPage();
            this.txtNodeMapping = new System.Windows.Forms.TextBox();
            this.rbUseCustomTableLoad = new System.Windows.Forms.RadioButton();
            this.rbUseDatasetLoad = new System.Windows.Forms.RadioButton();
            this.tabDataSource = new System.Windows.Forms.TabControl();
            this.tabDatasourceFile = new System.Windows.Forms.TabPage();
            this.cmbEndpoint = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.nudMaxRows = new System.Windows.Forms.NumericUpDown();
            this.chkHasMaxRows = new System.Windows.Forms.CheckBox();
            this.cbTextExtractor = new System.Windows.Forms.ComboBox();
            this.chkUseTextExtractor = new System.Windows.Forms.CheckBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.tabDatasourceText = new System.Windows.Forms.TabPage();
            this.txtTextContents = new System.Windows.Forms.TextBox();
            this.tabDatasourceDatabase = new System.Windows.Forms.TabPage();
            this.cmbDatasource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDatabaseQuery = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnShowSettings = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.cmbTableName = new System.Windows.Forms.ComboBox();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtXmlContents = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtXPathContents = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtRegexContents = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtExceptions = new System.Windows.Forms.TextBox();
            this.ProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.tabSource = new System.Windows.Forms.TabControl();
            this.tabDataSourceOptions = new System.Windows.Forms.TabPage();
            this.cmbParserProfile = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabTransformationOptions = new System.Windows.Forms.TabPage();
            this.tabActionOptions = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.cmbDefaultAction = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.chkAvailableActions = new System.Windows.Forms.CheckedListBox();
            this.tabExportOptions = new System.Windows.Forms.TabPage();
            this.chkAvailableExports = new System.Windows.Forms.CheckedListBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tabPermissionOptions = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkCanExportData = new System.Windows.Forms.CheckBox();
            this.chkCanDeleteData = new System.Windows.Forms.CheckBox();
            this.chkCanAddData = new System.Windows.Forms.CheckBox();
            this.chkCanEditData = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkShowConfigurationOnLoad = new System.Windows.Forms.CheckBox();
            this.chkCanEditConfiguration = new System.Windows.Forms.CheckBox();
            this.chkAutoRefresh = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.fpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlExport = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbExport = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHideSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.tabControl2.SuspendLayout();
            this.tabDuringLoad.SuspendLayout();
            this.tabAfterLoad.SuspendLayout();
            this.tabDataSetConversion.SuspendLayout();
            this.tabDataSource.SuspendLayout();
            this.tabDatasourceFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRows)).BeginInit();
            this.tabDatasourceText.SuspendLayout();
            this.tabDatasourceDatabase.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabSource.SuspendLayout();
            this.tabDataSourceOptions.SuspendLayout();
            this.tabTransformationOptions.SuspendLayout();
            this.tabActionOptions.SuspendLayout();
            this.tabExportOptions.SuspendLayout();
            this.tabPermissionOptions.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlExport.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCloseWindow
            // 
            this.btnCloseWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseWindow.Location = new System.Drawing.Point(1216, 234);
            this.btnCloseWindow.Name = "btnCloseWindow";
            this.btnCloseWindow.Size = new System.Drawing.Size(93, 49);
            this.btnCloseWindow.TabIndex = 15;
            this.btnCloseWindow.Text = "Close Window";
            this.btnCloseWindow.UseVisualStyleBackColor = true;
            this.btnCloseWindow.Click += new System.EventHandler(this.btnCloseWindow_Click);
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshData.Location = new System.Drawing.Point(1189, 4);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(100, 23);
            this.btnRefreshData.TabIndex = 13;
            this.btnRefreshData.Text = "Load Data";
            this.btnRefreshData.UseVisualStyleBackColor = true;
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabDuringLoad);
            this.tabControl2.Controls.Add(this.tabAfterLoad);
            this.tabControl2.Controls.Add(this.tabDataSetConversion);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1061, 199);
            this.tabControl2.TabIndex = 6;
            // 
            // tabDuringLoad
            // 
            this.tabDuringLoad.Controls.Add(this.label14);
            this.tabDuringLoad.Controls.Add(this.txtOnLoadFileName);
            this.tabDuringLoad.Controls.Add(this.btnOnLoadSave);
            this.tabDuringLoad.Controls.Add(this.txtOnLoadContents);
            this.tabDuringLoad.Controls.Add(this.btnRefreshOnLoadProfiles);
            this.tabDuringLoad.Controls.Add(this.cbOnLoadProfiles);
            this.tabDuringLoad.Location = new System.Drawing.Point(4, 22);
            this.tabDuringLoad.Name = "tabDuringLoad";
            this.tabDuringLoad.Padding = new System.Windows.Forms.Padding(3);
            this.tabDuringLoad.Size = new System.Drawing.Size(1053, 173);
            this.tabDuringLoad.TabIndex = 0;
            this.tabDuringLoad.Text = "During Load";
            this.tabDuringLoad.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 13);
            this.label14.TabIndex = 23;
            this.label14.Text = "Transformation Profile:";
            // 
            // txtOnLoadFileName
            // 
            this.txtOnLoadFileName.Location = new System.Drawing.Point(500, 7);
            this.txtOnLoadFileName.Name = "txtOnLoadFileName";
            this.txtOnLoadFileName.Size = new System.Drawing.Size(195, 20);
            this.txtOnLoadFileName.TabIndex = 22;
            // 
            // btnOnLoadSave
            // 
            this.btnOnLoadSave.Location = new System.Drawing.Point(711, 5);
            this.btnOnLoadSave.Name = "btnOnLoadSave";
            this.btnOnLoadSave.Size = new System.Drawing.Size(75, 23);
            this.btnOnLoadSave.TabIndex = 21;
            this.btnOnLoadSave.Text = "Save";
            this.btnOnLoadSave.UseVisualStyleBackColor = true;
            // 
            // txtOnLoadContents
            // 
            this.txtOnLoadContents.Location = new System.Drawing.Point(6, 34);
            this.txtOnLoadContents.Multiline = true;
            this.txtOnLoadContents.Name = "txtOnLoadContents";
            this.txtOnLoadContents.Size = new System.Drawing.Size(780, 115);
            this.txtOnLoadContents.TabIndex = 20;
            this.txtOnLoadContents.Leave += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // btnRefreshOnLoadProfiles
            // 
            this.btnRefreshOnLoadProfiles.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshOnLoadProfiles.Image")));
            this.btnRefreshOnLoadProfiles.Location = new System.Drawing.Point(264, 7);
            this.btnRefreshOnLoadProfiles.Name = "btnRefreshOnLoadProfiles";
            this.btnRefreshOnLoadProfiles.Size = new System.Drawing.Size(27, 22);
            this.btnRefreshOnLoadProfiles.TabIndex = 19;
            this.btnRefreshOnLoadProfiles.UseVisualStyleBackColor = true;
            this.btnRefreshOnLoadProfiles.Click += new System.EventHandler(this.btnRefreshOnLoadProfiles_Click);
            // 
            // cbOnLoadProfiles
            // 
            this.cbOnLoadProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOnLoadProfiles.FormattingEnabled = true;
            this.cbOnLoadProfiles.Location = new System.Drawing.Point(124, 7);
            this.cbOnLoadProfiles.Name = "cbOnLoadProfiles";
            this.cbOnLoadProfiles.Size = new System.Drawing.Size(134, 21);
            this.cbOnLoadProfiles.TabIndex = 18;
            this.cbOnLoadProfiles.SelectedIndexChanged += new System.EventHandler(this.cbOnLoadProfiles_SelectedIndexChanged);
            // 
            // tabAfterLoad
            // 
            this.tabAfterLoad.Controls.Add(this.btnBrowseXsltFileName);
            this.tabAfterLoad.Controls.Add(this.txtXsltFileName);
            this.tabAfterLoad.Controls.Add(this.chkUseXsltTemplate);
            this.tabAfterLoad.Controls.Add(this.label8);
            this.tabAfterLoad.Controls.Add(this.txtTransformFileName);
            this.tabAfterLoad.Controls.Add(this.btnTransformSave);
            this.tabAfterLoad.Controls.Add(this.txtTransformText);
            this.tabAfterLoad.Controls.Add(this.btnTransformProfilesLoad);
            this.tabAfterLoad.Controls.Add(this.cbTransformProfiles);
            this.tabAfterLoad.Location = new System.Drawing.Point(4, 22);
            this.tabAfterLoad.Name = "tabAfterLoad";
            this.tabAfterLoad.Padding = new System.Windows.Forms.Padding(3);
            this.tabAfterLoad.Size = new System.Drawing.Size(1053, 173);
            this.tabAfterLoad.TabIndex = 1;
            this.tabAfterLoad.Text = "After Load";
            this.tabAfterLoad.UseVisualStyleBackColor = true;
            // 
            // btnBrowseXsltFileName
            // 
            this.btnBrowseXsltFileName.Enabled = false;
            this.btnBrowseXsltFileName.Location = new System.Drawing.Point(555, 150);
            this.btnBrowseXsltFileName.Name = "btnBrowseXsltFileName";
            this.btnBrowseXsltFileName.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseXsltFileName.TabIndex = 20;
            this.btnBrowseXsltFileName.Text = "Browse";
            this.btnBrowseXsltFileName.UseVisualStyleBackColor = true;
            this.btnBrowseXsltFileName.Click += new System.EventHandler(this.btnBrowseXsltFileName_Click);
            // 
            // txtXsltFileName
            // 
            this.txtXsltFileName.Location = new System.Drawing.Point(174, 150);
            this.txtXsltFileName.Name = "txtXsltFileName";
            this.txtXsltFileName.ReadOnly = true;
            this.txtXsltFileName.Size = new System.Drawing.Size(374, 20);
            this.txtXsltFileName.TabIndex = 19;
            // 
            // chkUseXsltTemplate
            // 
            this.chkUseXsltTemplate.AutoSize = true;
            this.chkUseXsltTemplate.Location = new System.Drawing.Point(9, 150);
            this.chkUseXsltTemplate.Name = "chkUseXsltTemplate";
            this.chkUseXsltTemplate.Size = new System.Drawing.Size(158, 17);
            this.chkUseXsltTemplate.TabIndex = 18;
            this.chkUseXsltTemplate.Text = "Transform XML Using XSLT";
            this.chkUseXsltTemplate.UseVisualStyleBackColor = true;
            this.chkUseXsltTemplate.CheckedChanged += new System.EventHandler(this.chkUseXsltTemplate_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Transformation Profile:";
            // 
            // txtTransformFileName
            // 
            this.txtTransformFileName.Location = new System.Drawing.Point(510, 6);
            this.txtTransformFileName.Name = "txtTransformFileName";
            this.txtTransformFileName.Size = new System.Drawing.Size(195, 20);
            this.txtTransformFileName.TabIndex = 16;
            // 
            // btnTransformSave
            // 
            this.btnTransformSave.Location = new System.Drawing.Point(711, 5);
            this.btnTransformSave.Name = "btnTransformSave";
            this.btnTransformSave.Size = new System.Drawing.Size(75, 23);
            this.btnTransformSave.TabIndex = 15;
            this.btnTransformSave.Text = "Save";
            this.btnTransformSave.UseVisualStyleBackColor = true;
            this.btnTransformSave.Click += new System.EventHandler(this.btnTransformSave_Click);
            // 
            // txtTransformText
            // 
            this.txtTransformText.Location = new System.Drawing.Point(9, 33);
            this.txtTransformText.Multiline = true;
            this.txtTransformText.Name = "txtTransformText";
            this.txtTransformText.Size = new System.Drawing.Size(777, 113);
            this.txtTransformText.TabIndex = 14;
            this.txtTransformText.Leave += new System.EventHandler(this.txtTransformText_Leave);
            // 
            // btnTransformProfilesLoad
            // 
            this.btnTransformProfilesLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnTransformProfilesLoad.Image")));
            this.btnTransformProfilesLoad.Location = new System.Drawing.Point(264, 6);
            this.btnTransformProfilesLoad.Name = "btnTransformProfilesLoad";
            this.btnTransformProfilesLoad.Size = new System.Drawing.Size(27, 22);
            this.btnTransformProfilesLoad.TabIndex = 13;
            this.btnTransformProfilesLoad.UseVisualStyleBackColor = true;
            this.btnTransformProfilesLoad.Click += new System.EventHandler(this.btnTransformProfilesLoad_Click);
            // 
            // cbTransformProfiles
            // 
            this.cbTransformProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTransformProfiles.FormattingEnabled = true;
            this.cbTransformProfiles.Location = new System.Drawing.Point(124, 7);
            this.cbTransformProfiles.Name = "cbTransformProfiles";
            this.cbTransformProfiles.Size = new System.Drawing.Size(134, 21);
            this.cbTransformProfiles.TabIndex = 12;
            this.cbTransformProfiles.SelectedIndexChanged += new System.EventHandler(this.cbTransformProfiles_SelectedIndexChanged);
            // 
            // tabDataSetConversion
            // 
            this.tabDataSetConversion.Controls.Add(this.txtNodeMapping);
            this.tabDataSetConversion.Controls.Add(this.rbUseCustomTableLoad);
            this.tabDataSetConversion.Controls.Add(this.rbUseDatasetLoad);
            this.tabDataSetConversion.Location = new System.Drawing.Point(4, 22);
            this.tabDataSetConversion.Name = "tabDataSetConversion";
            this.tabDataSetConversion.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataSetConversion.Size = new System.Drawing.Size(1053, 173);
            this.tabDataSetConversion.TabIndex = 2;
            this.tabDataSetConversion.Text = "XML to Dataset Conversion";
            this.tabDataSetConversion.UseVisualStyleBackColor = true;
            // 
            // txtNodeMapping
            // 
            this.txtNodeMapping.Enabled = false;
            this.txtNodeMapping.Location = new System.Drawing.Point(7, 55);
            this.txtNodeMapping.Multiline = true;
            this.txtNodeMapping.Name = "txtNodeMapping";
            this.txtNodeMapping.Size = new System.Drawing.Size(429, 112);
            this.txtNodeMapping.TabIndex = 2;
            // 
            // rbUseCustomTableLoad
            // 
            this.rbUseCustomTableLoad.AutoSize = true;
            this.rbUseCustomTableLoad.Location = new System.Drawing.Point(7, 31);
            this.rbUseCustomTableLoad.Name = "rbUseCustomTableLoad";
            this.rbUseCustomTableLoad.Size = new System.Drawing.Size(117, 17);
            this.rbUseCustomTableLoad.TabIndex = 1;
            this.rbUseCustomTableLoad.Text = "Use Node Mapping";
            this.rbUseCustomTableLoad.UseVisualStyleBackColor = true;
            this.rbUseCustomTableLoad.CheckedChanged += new System.EventHandler(this.rbUseCustomTableLoad_CheckedChanged);
            // 
            // rbUseDatasetLoad
            // 
            this.rbUseDatasetLoad.AutoSize = true;
            this.rbUseDatasetLoad.Checked = true;
            this.rbUseDatasetLoad.Location = new System.Drawing.Point(7, 7);
            this.rbUseDatasetLoad.Name = "rbUseDatasetLoad";
            this.rbUseDatasetLoad.Size = new System.Drawing.Size(139, 17);
            this.rbUseDatasetLoad.TabIndex = 0;
            this.rbUseDatasetLoad.TabStop = true;
            this.rbUseDatasetLoad.Text = "Convert XML to Dataset";
            this.rbUseDatasetLoad.UseVisualStyleBackColor = true;
            // 
            // tabDataSource
            // 
            this.tabDataSource.Controls.Add(this.tabDatasourceFile);
            this.tabDataSource.Controls.Add(this.tabDatasourceText);
            this.tabDataSource.Controls.Add(this.tabDatasourceDatabase);
            this.tabDataSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDataSource.Location = new System.Drawing.Point(3, 3);
            this.tabDataSource.Name = "tabDataSource";
            this.tabDataSource.SelectedIndex = 0;
            this.tabDataSource.Size = new System.Drawing.Size(1055, 193);
            this.tabDataSource.TabIndex = 9;
            // 
            // tabDatasourceFile
            // 
            this.tabDatasourceFile.Controls.Add(this.cmbParserProfile);
            this.tabDatasourceFile.Controls.Add(this.cmbEndpoint);
            this.tabDatasourceFile.Controls.Add(this.label15);
            this.tabDatasourceFile.Controls.Add(this.label5);
            this.tabDatasourceFile.Controls.Add(this.label18);
            this.tabDatasourceFile.Controls.Add(this.label13);
            this.tabDatasourceFile.Controls.Add(this.nudMaxRows);
            this.tabDatasourceFile.Controls.Add(this.chkHasMaxRows);
            this.tabDatasourceFile.Controls.Add(this.cbTextExtractor);
            this.tabDatasourceFile.Controls.Add(this.chkUseTextExtractor);
            this.tabDatasourceFile.Controls.Add(this.btnLoad);
            this.tabDatasourceFile.Controls.Add(this.txtFileName);
            this.tabDatasourceFile.Location = new System.Drawing.Point(4, 22);
            this.tabDatasourceFile.Name = "tabDatasourceFile";
            this.tabDatasourceFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatasourceFile.Size = new System.Drawing.Size(1047, 167);
            this.tabDatasourceFile.TabIndex = 0;
            this.tabDatasourceFile.Text = "File";
            this.tabDatasourceFile.UseVisualStyleBackColor = true;
            // 
            // cmbEndpoint
            // 
            this.cmbEndpoint.FormattingEnabled = true;
            this.cmbEndpoint.Location = new System.Drawing.Point(72, 18);
            this.cmbEndpoint.Name = "cmbEndpoint";
            this.cmbEndpoint.Size = new System.Drawing.Size(189, 21);
            this.cmbEndpoint.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Endpoint :";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 51);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(57, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "File Name:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(178, 122);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "rows";
            // 
            // nudMaxRows
            // 
            this.nudMaxRows.Location = new System.Drawing.Point(126, 123);
            this.nudMaxRows.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudMaxRows.Name = "nudMaxRows";
            this.nudMaxRows.Size = new System.Drawing.Size(53, 20);
            this.nudMaxRows.TabIndex = 11;
            this.nudMaxRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMaxRows.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudMaxRows.ValueChanged += new System.EventHandler(this.nudMaxRows_ValueChanged);
            // 
            // chkHasMaxRows
            // 
            this.chkHasMaxRows.AutoSize = true;
            this.chkHasMaxRows.Checked = true;
            this.chkHasMaxRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHasMaxRows.Location = new System.Drawing.Point(7, 123);
            this.chkHasMaxRows.Name = "chkHasMaxRows";
            this.chkHasMaxRows.Size = new System.Drawing.Size(112, 17);
            this.chkHasMaxRows.TabIndex = 10;
            this.chkHasMaxRows.Text = "Stop loading after ";
            this.chkHasMaxRows.UseVisualStyleBackColor = true;
            this.chkHasMaxRows.CheckedChanged += new System.EventHandler(this.chkHasMaxRows_CheckedChanged);
            // 
            // cbTextExtractor
            // 
            this.cbTextExtractor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTextExtractor.FormattingEnabled = true;
            this.cbTextExtractor.Location = new System.Drawing.Point(126, 96);
            this.cbTextExtractor.Name = "cbTextExtractor";
            this.cbTextExtractor.Size = new System.Drawing.Size(242, 21);
            this.cbTextExtractor.TabIndex = 9;
            this.cbTextExtractor.Visible = false;
            this.cbTextExtractor.SelectedIndexChanged += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // chkUseTextExtractor
            // 
            this.chkUseTextExtractor.AutoSize = true;
            this.chkUseTextExtractor.Location = new System.Drawing.Point(7, 96);
            this.chkUseTextExtractor.Name = "chkUseTextExtractor";
            this.chkUseTextExtractor.Size = new System.Drawing.Size(114, 17);
            this.chkUseTextExtractor.TabIndex = 8;
            this.chkUseTextExtractor.Text = "Use Text Extractor";
            this.chkUseTextExtractor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkUseTextExtractor.UseVisualStyleBackColor = true;
            this.chkUseTextExtractor.CheckedChanged += new System.EventHandler(this.chkUseTextExtractor_CheckedChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(675, 68);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Browse File";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(6, 70);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(663, 20);
            this.txtFileName.TabIndex = 6;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // tabDatasourceText
            // 
            this.tabDatasourceText.Controls.Add(this.txtTextContents);
            this.tabDatasourceText.Location = new System.Drawing.Point(4, 22);
            this.tabDatasourceText.Name = "tabDatasourceText";
            this.tabDatasourceText.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatasourceText.Size = new System.Drawing.Size(1047, 167);
            this.tabDatasourceText.TabIndex = 1;
            this.tabDatasourceText.Text = "Text";
            this.tabDatasourceText.UseVisualStyleBackColor = true;
            // 
            // txtTextContents
            // 
            this.txtTextContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTextContents.Location = new System.Drawing.Point(3, 3);
            this.txtTextContents.Multiline = true;
            this.txtTextContents.Name = "txtTextContents";
            this.txtTextContents.Size = new System.Drawing.Size(1041, 161);
            this.txtTextContents.TabIndex = 10;
            this.txtTextContents.TextChanged += new System.EventHandler(this.txtTextContents_TextChanged);
            this.txtTextContents.Leave += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // tabDatasourceDatabase
            // 
            this.tabDatasourceDatabase.Controls.Add(this.cmbDatasource);
            this.tabDatasourceDatabase.Controls.Add(this.label1);
            this.tabDatasourceDatabase.Controls.Add(this.txtDatabaseQuery);
            this.tabDatasourceDatabase.Controls.Add(this.label6);
            this.tabDatasourceDatabase.Location = new System.Drawing.Point(4, 22);
            this.tabDatasourceDatabase.Name = "tabDatasourceDatabase";
            this.tabDatasourceDatabase.Size = new System.Drawing.Size(1047, 167);
            this.tabDatasourceDatabase.TabIndex = 2;
            this.tabDatasourceDatabase.Text = "Database";
            this.tabDatasourceDatabase.UseVisualStyleBackColor = true;
            // 
            // cmbDatasource
            // 
            this.cmbDatasource.FormattingEnabled = true;
            this.cmbDatasource.Location = new System.Drawing.Point(113, 14);
            this.cmbDatasource.Name = "cmbDatasource";
            this.cmbDatasource.Size = new System.Drawing.Size(362, 21);
            this.cmbDatasource.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Data Source:";
            // 
            // txtDatabaseQuery
            // 
            this.txtDatabaseQuery.Location = new System.Drawing.Point(113, 55);
            this.txtDatabaseQuery.Multiline = true;
            this.txtDatabaseQuery.Name = "txtDatabaseQuery";
            this.txtDatabaseQuery.Size = new System.Drawing.Size(362, 62);
            this.txtDatabaseQuery.TabIndex = 19;
            this.txtDatabaseQuery.Leave += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "SQL Query:";
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 289);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1306, 578);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1298, 552);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Dataset";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnShowSettings);
            this.splitContainer2.Panel1.Controls.Add(this.lblProgress);
            this.splitContainer2.Panel1.Controls.Add(this.pbProgress);
            this.splitContainer2.Panel1.Controls.Add(this.cmbTableName);
            this.splitContainer2.Panel1.Controls.Add(this.lblRecordCount);
            this.splitContainer2.Panel1.Controls.Add(this.btnRefreshData);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGrid);
            this.splitContainer2.Size = new System.Drawing.Size(1292, 546);
            this.splitContainer2.SplitterDistance = 32;
            this.splitContainer2.TabIndex = 15;
            // 
            // btnShowSettings
            // 
            this.btnShowSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowSettings.Location = new System.Drawing.Point(1068, 4);
            this.btnShowSettings.Name = "btnShowSettings";
            this.btnShowSettings.Size = new System.Drawing.Size(115, 23);
            this.btnShowSettings.TabIndex = 17;
            this.btnShowSettings.Text = "Show Configuration";
            this.btnShowSettings.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowSettings.UseVisualStyleBackColor = true;
            this.btnShowSettings.Visible = false;
            this.btnShowSettings.Click += new System.EventHandler(this.btnShowSettings_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(968, 10);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(33, 13);
            this.lblProgress.TabIndex = 16;
            this.lblProgress.Text = "100%";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgress.Visible = false;
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(862, 4);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(100, 23);
            this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbProgress.TabIndex = 15;
            this.pbProgress.Visible = false;
            // 
            // cmbTableName
            // 
            this.cmbTableName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableName.FormattingEnabled = true;
            this.cmbTableName.Location = new System.Drawing.Point(94, 6);
            this.cmbTableName.Name = "cmbTableName";
            this.cmbTableName.Size = new System.Drawing.Size(246, 21);
            this.cmbTableName.TabIndex = 7;
            this.cmbTableName.SelectedIndexChanged += new System.EventHandler(this.cmbTableName_SelectedIndexChanged);
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Location = new System.Drawing.Point(369, 4);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(265, 25);
            this.lblRecordCount.TabIndex = 14;
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Table Name:";
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToOrderColumns = true;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.Size = new System.Drawing.Size(1292, 510);
            this.dataGrid.TabIndex = 10;
            this.dataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_CellContentDoubleClick);
            this.dataGrid.SelectionChanged += new System.EventHandler(this.dataGrid_SelectionChanged);
            this.dataGrid.DoubleClick += new System.EventHandler(this.dataGrid_DoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtXmlContents);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1298, 552);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Xml";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtXmlContents
            // 
            this.txtXmlContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXmlContents.Location = new System.Drawing.Point(3, 3);
            this.txtXmlContents.Multiline = true;
            this.txtXmlContents.Name = "txtXmlContents";
            this.txtXmlContents.Size = new System.Drawing.Size(1292, 546);
            this.txtXmlContents.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtXPathContents);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1298, 552);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "XPath";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtXPathContents
            // 
            this.txtXPathContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXPathContents.Location = new System.Drawing.Point(0, 0);
            this.txtXPathContents.Multiline = true;
            this.txtXPathContents.Name = "txtXPathContents";
            this.txtXPathContents.Size = new System.Drawing.Size(1298, 552);
            this.txtXPathContents.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtRegexContents);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1298, 552);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Regex";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtRegexContents
            // 
            this.txtRegexContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRegexContents.Location = new System.Drawing.Point(0, 0);
            this.txtRegexContents.Multiline = true;
            this.txtRegexContents.Name = "txtRegexContents";
            this.txtRegexContents.Size = new System.Drawing.Size(1298, 552);
            this.txtRegexContents.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtExceptions);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1298, 552);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Exceptions";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtExceptions
            // 
            this.txtExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExceptions.Location = new System.Drawing.Point(0, 0);
            this.txtExceptions.Multiline = true;
            this.txtExceptions.Name = "txtExceptions";
            this.txtExceptions.Size = new System.Drawing.Size(1298, 552);
            this.txtExceptions.TabIndex = 0;
            // 
            // tabSource
            // 
            this.tabSource.Controls.Add(this.tabDataSourceOptions);
            this.tabSource.Controls.Add(this.tabTransformationOptions);
            this.tabSource.Controls.Add(this.tabActionOptions);
            this.tabSource.Controls.Add(this.tabExportOptions);
            this.tabSource.Controls.Add(this.tabPermissionOptions);
            this.tabSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSource.Location = new System.Drawing.Point(3, 3);
            this.tabSource.Name = "tabSource";
            this.tabSource.SelectedIndex = 0;
            this.tabSource.Size = new System.Drawing.Size(1069, 225);
            this.tabSource.TabIndex = 8;
            // 
            // tabDataSourceOptions
            // 
            this.tabDataSourceOptions.Controls.Add(this.tabDataSource);
            this.tabDataSourceOptions.Location = new System.Drawing.Point(4, 22);
            this.tabDataSourceOptions.Name = "tabDataSourceOptions";
            this.tabDataSourceOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataSourceOptions.Size = new System.Drawing.Size(1061, 199);
            this.tabDataSourceOptions.TabIndex = 0;
            this.tabDataSourceOptions.Text = "Data Source";
            this.tabDataSourceOptions.UseVisualStyleBackColor = true;
            // 
            // cmbParserProfile
            // 
            this.cmbParserProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParserProfile.FormattingEnabled = true;
            this.cmbParserProfile.Location = new System.Drawing.Point(498, 97);
            this.cmbParserProfile.Name = "cmbParserProfile";
            this.cmbParserProfile.Size = new System.Drawing.Size(252, 21);
            this.cmbParserProfile.Sorted = true;
            this.cmbParserProfile.TabIndex = 3;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(408, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Parser Profile:";
            // 
            // tabTransformationOptions
            // 
            this.tabTransformationOptions.Controls.Add(this.tabControl2);
            this.tabTransformationOptions.Location = new System.Drawing.Point(4, 22);
            this.tabTransformationOptions.Name = "tabTransformationOptions";
            this.tabTransformationOptions.Size = new System.Drawing.Size(1061, 199);
            this.tabTransformationOptions.TabIndex = 2;
            this.tabTransformationOptions.Text = "Transformations";
            this.tabTransformationOptions.UseVisualStyleBackColor = true;
            // 
            // tabActionOptions
            // 
            this.tabActionOptions.Controls.Add(this.label24);
            this.tabActionOptions.Controls.Add(this.cmbDefaultAction);
            this.tabActionOptions.Controls.Add(this.label23);
            this.tabActionOptions.Controls.Add(this.label22);
            this.tabActionOptions.Controls.Add(this.chkAvailableActions);
            this.tabActionOptions.Location = new System.Drawing.Point(4, 22);
            this.tabActionOptions.Name = "tabActionOptions";
            this.tabActionOptions.Size = new System.Drawing.Size(1061, 199);
            this.tabActionOptions.TabIndex = 5;
            this.tabActionOptions.Text = "Actions";
            this.tabActionOptions.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(349, 74);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(367, 13);
            this.label24.TabIndex = 4;
            this.label24.Text = "This action would be triggered when the user does a double click on the grid";
            // 
            // cmbDefaultAction
            // 
            this.cmbDefaultAction.FormattingEnabled = true;
            this.cmbDefaultAction.Location = new System.Drawing.Point(352, 34);
            this.cmbDefaultAction.Name = "cmbDefaultAction";
            this.cmbDefaultAction.Size = new System.Drawing.Size(229, 21);
            this.cmbDefaultAction.TabIndex = 3;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(349, 15);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(77, 13);
            this.label23.TabIndex = 2;
            this.label23.Text = "Default Action:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 15);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(91, 13);
            this.label22.TabIndex = 1;
            this.label22.Text = "Available Actions:";
            // 
            // chkAvailableActions
            // 
            this.chkAvailableActions.FormattingEnabled = true;
            this.chkAvailableActions.Location = new System.Drawing.Point(6, 34);
            this.chkAvailableActions.Name = "chkAvailableActions";
            this.chkAvailableActions.Size = new System.Drawing.Size(314, 154);
            this.chkAvailableActions.TabIndex = 0;
            this.chkAvailableActions.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkAvailableActions_ItemCheck);
            // 
            // tabExportOptions
            // 
            this.tabExportOptions.Controls.Add(this.chkAvailableExports);
            this.tabExportOptions.Controls.Add(this.label20);
            this.tabExportOptions.Location = new System.Drawing.Point(4, 22);
            this.tabExportOptions.Name = "tabExportOptions";
            this.tabExportOptions.Size = new System.Drawing.Size(1061, 199);
            this.tabExportOptions.TabIndex = 3;
            this.tabExportOptions.Text = "Export";
            this.tabExportOptions.UseVisualStyleBackColor = true;
            // 
            // chkAvailableExports
            // 
            this.chkAvailableExports.FormattingEnabled = true;
            this.chkAvailableExports.Location = new System.Drawing.Point(9, 49);
            this.chkAvailableExports.Name = "chkAvailableExports";
            this.chkAvailableExports.Size = new System.Drawing.Size(260, 139);
            this.chkAvailableExports.TabIndex = 22;
            this.chkAvailableExports.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkAvailableExports_ItemCheck);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 15);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(91, 13);
            this.label20.TabIndex = 21;
            this.label20.Text = "Available Exports:";
            // 
            // tabPermissionOptions
            // 
            this.tabPermissionOptions.Controls.Add(this.groupBox2);
            this.tabPermissionOptions.Controls.Add(this.groupBox1);
            this.tabPermissionOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPermissionOptions.Name = "tabPermissionOptions";
            this.tabPermissionOptions.Size = new System.Drawing.Size(1061, 199);
            this.tabPermissionOptions.TabIndex = 4;
            this.tabPermissionOptions.Text = "Permissions";
            this.tabPermissionOptions.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkCanExportData);
            this.groupBox2.Controls.Add(this.chkCanDeleteData);
            this.groupBox2.Controls.Add(this.chkCanAddData);
            this.groupBox2.Controls.Add(this.chkCanEditData);
            this.groupBox2.Location = new System.Drawing.Point(492, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(495, 166);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Access:";
            // 
            // chkCanExportData
            // 
            this.chkCanExportData.AutoSize = true;
            this.chkCanExportData.Checked = true;
            this.chkCanExportData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCanExportData.Location = new System.Drawing.Point(18, 124);
            this.chkCanExportData.Name = "chkCanExportData";
            this.chkCanExportData.Size = new System.Drawing.Size(104, 17);
            this.chkCanExportData.TabIndex = 3;
            this.chkCanExportData.Text = "Can Export Data";
            this.chkCanExportData.UseVisualStyleBackColor = true;
            this.chkCanExportData.CheckedChanged += new System.EventHandler(this.chkCanExportData_CheckedChanged);
            // 
            // chkCanDeleteData
            // 
            this.chkCanDeleteData.AutoSize = true;
            this.chkCanDeleteData.Location = new System.Drawing.Point(18, 87);
            this.chkCanDeleteData.Name = "chkCanDeleteData";
            this.chkCanDeleteData.Size = new System.Drawing.Size(105, 17);
            this.chkCanDeleteData.TabIndex = 2;
            this.chkCanDeleteData.Text = "Can Delete Data";
            this.chkCanDeleteData.UseVisualStyleBackColor = true;
            this.chkCanDeleteData.CheckedChanged += new System.EventHandler(this.chkCanDeleteData_CheckedChanged);
            // 
            // chkCanAddData
            // 
            this.chkCanAddData.AutoSize = true;
            this.chkCanAddData.Location = new System.Drawing.Point(18, 20);
            this.chkCanAddData.Name = "chkCanAddData";
            this.chkCanAddData.Size = new System.Drawing.Size(93, 17);
            this.chkCanAddData.TabIndex = 1;
            this.chkCanAddData.Text = "Can Add Data";
            this.chkCanAddData.UseVisualStyleBackColor = true;
            this.chkCanAddData.CheckedChanged += new System.EventHandler(this.chkCanAddData_CheckedChanged);
            // 
            // chkCanEditData
            // 
            this.chkCanEditData.AutoSize = true;
            this.chkCanEditData.Location = new System.Drawing.Point(18, 54);
            this.chkCanEditData.Name = "chkCanEditData";
            this.chkCanEditData.Size = new System.Drawing.Size(92, 17);
            this.chkCanEditData.TabIndex = 0;
            this.chkCanEditData.Text = "Can Edit Data";
            this.chkCanEditData.UseVisualStyleBackColor = true;
            this.chkCanEditData.CheckedChanged += new System.EventHandler(this.chkCanEditData_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkShowConfigurationOnLoad);
            this.groupBox1.Controls.Add(this.chkCanEditConfiguration);
            this.groupBox1.Location = new System.Drawing.Point(18, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration Access:";
            // 
            // chkShowConfigurationOnLoad
            // 
            this.chkShowConfigurationOnLoad.AutoSize = true;
            this.chkShowConfigurationOnLoad.Checked = true;
            this.chkShowConfigurationOnLoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowConfigurationOnLoad.Location = new System.Drawing.Point(7, 43);
            this.chkShowConfigurationOnLoad.Name = "chkShowConfigurationOnLoad";
            this.chkShowConfigurationOnLoad.Size = new System.Drawing.Size(169, 17);
            this.chkShowConfigurationOnLoad.TabIndex = 1;
            this.chkShowConfigurationOnLoad.Text = "Display Configuration On Load";
            this.chkShowConfigurationOnLoad.UseVisualStyleBackColor = true;
            // 
            // chkCanEditConfiguration
            // 
            this.chkCanEditConfiguration.AutoSize = true;
            this.chkCanEditConfiguration.Checked = true;
            this.chkCanEditConfiguration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCanEditConfiguration.Location = new System.Drawing.Point(7, 20);
            this.chkCanEditConfiguration.Name = "chkCanEditConfiguration";
            this.chkCanEditConfiguration.Size = new System.Drawing.Size(131, 17);
            this.chkCanEditConfiguration.TabIndex = 0;
            this.chkCanEditConfiguration.Text = "Can Edit Configuration";
            this.chkCanEditConfiguration.UseVisualStyleBackColor = true;
            this.chkCanEditConfiguration.CheckedChanged += new System.EventHandler(this.chkCanEditConfiguration_CheckedChanged);
            // 
            // chkAutoRefresh
            // 
            this.chkAutoRefresh.AutoSize = true;
            this.chkAutoRefresh.Checked = true;
            this.chkAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoRefresh.Location = new System.Drawing.Point(13, 22);
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkAutoRefresh.Size = new System.Drawing.Size(207, 17);
            this.chkAutoRefresh.TabIndex = 16;
            this.chkAutoRefresh.Text = "Refresh Data on configuration change";
            this.chkAutoRefresh.UseVisualStyleBackColor = true;
            this.chkAutoRefresh.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.Controls.Add(this.fpActions, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlExport, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCloseWindow, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabSource, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1312, 870);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // fpActions
            // 
            this.fpActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpActions.Location = new System.Drawing.Point(3, 234);
            this.fpActions.Name = "fpActions";
            this.fpActions.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpActions.Size = new System.Drawing.Size(1069, 49);
            this.fpActions.TabIndex = 19;
            this.fpActions.Visible = false;
            // 
            // pnlExport
            // 
            this.pnlExport.Controls.Add(this.btnExport);
            this.pnlExport.Controls.Add(this.cmbExport);
            this.pnlExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExport.Location = new System.Drawing.Point(1078, 234);
            this.pnlExport.Name = "pnlExport";
            this.pnlExport.Size = new System.Drawing.Size(132, 49);
            this.pnlExport.TabIndex = 17;
            this.pnlExport.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(0, 26);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(132, 23);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cmbExport
            // 
            this.cmbExport.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbExport.FormattingEnabled = true;
            this.cmbExport.Location = new System.Drawing.Point(0, 0);
            this.cmbExport.Name = "cmbExport";
            this.cmbExport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbExport.Size = new System.Drawing.Size(132, 21);
            this.cmbExport.TabIndex = 15;
            this.cmbExport.SelectedIndexChanged += new System.EventHandler(this.cmbExport_SelectedIndexChanged);
            this.cmbExport.TextChanged += new System.EventHandler(this.cmbExport_TextChanged);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.btnHideSettings);
            this.panel1.Controls.Add(this.btnSaveSettings);
            this.panel1.Controls.Add(this.chkAutoRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1078, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 225);
            this.panel1.TabIndex = 19;
            // 
            // btnHideSettings
            // 
            this.btnHideSettings.Location = new System.Drawing.Point(13, 195);
            this.btnHideSettings.Name = "btnHideSettings";
            this.btnHideSettings.Size = new System.Drawing.Size(209, 23);
            this.btnHideSettings.TabIndex = 19;
            this.btnHideSettings.Text = "Hide Configuration Section";
            this.btnHideSettings.UseVisualStyleBackColor = true;
            this.btnHideSettings.Click += new System.EventHandler(this.btnHideSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(13, 167);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(209, 23);
            this.btnSaveSettings.TabIndex = 18;
            this.btnSaveSettings.Text = "Save Configuration";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Visible = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            // 
            // ETLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 870);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ETLForm";
            this.Text = "Easy Controls Demo Application";
            this.tabControl2.ResumeLayout(false);
            this.tabDuringLoad.ResumeLayout(false);
            this.tabDuringLoad.PerformLayout();
            this.tabAfterLoad.ResumeLayout(false);
            this.tabAfterLoad.PerformLayout();
            this.tabDataSetConversion.ResumeLayout(false);
            this.tabDataSetConversion.PerformLayout();
            this.tabDataSource.ResumeLayout(false);
            this.tabDatasourceFile.ResumeLayout(false);
            this.tabDatasourceFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRows)).EndInit();
            this.tabDatasourceText.ResumeLayout(false);
            this.tabDatasourceText.PerformLayout();
            this.tabDatasourceDatabase.ResumeLayout(false);
            this.tabDatasourceDatabase.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabSource.ResumeLayout(false);
            this.tabDataSourceOptions.ResumeLayout(false);
            this.tabTransformationOptions.ResumeLayout(false);
            this.tabActionOptions.ResumeLayout(false);
            this.tabActionOptions.PerformLayout();
            this.tabExportOptions.ResumeLayout(false);
            this.tabExportOptions.PerformLayout();
            this.tabPermissionOptions.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlExport.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.ComboBox cmbTableName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtXmlContents;
        private System.Windows.Forms.TabControl tabDataSource;
        private System.Windows.Forms.TabPage tabDatasourceFile;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TabPage tabDatasourceText;
        private System.Windows.Forms.TextBox txtTextContents;
        private System.Windows.Forms.TabPage tabDatasourceDatabase;
        private System.Windows.Forms.TextBox txtDatabaseQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer ProgressTimer;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtXPathContents;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtRegexContents;
        private System.Windows.Forms.ComboBox cbTextExtractor;
        private System.Windows.Forms.CheckBox chkUseTextExtractor;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txtExceptions;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudMaxRows;
        private System.Windows.Forms.CheckBox chkHasMaxRows;
        private System.Windows.Forms.Button btnRefreshData;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabDuringLoad;
        private System.Windows.Forms.TabPage tabAfterLoad;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTransformFileName;
        private System.Windows.Forms.Button btnTransformSave;
        private System.Windows.Forms.TextBox txtTransformText;
        private System.Windows.Forms.Button btnTransformProfilesLoad;
        private System.Windows.Forms.ComboBox cbTransformProfiles;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtOnLoadFileName;
        private System.Windows.Forms.Button btnOnLoadSave;
        private System.Windows.Forms.TextBox txtOnLoadContents;
        private System.Windows.Forms.Button btnRefreshOnLoadProfiles;
        private System.Windows.Forms.ComboBox cbOnLoadProfiles;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnCloseWindow;
        private System.Windows.Forms.TabControl tabSource;
        private System.Windows.Forms.TabPage tabDataSourceOptions;
        private System.Windows.Forms.TabPage tabTransformationOptions;
        private System.Windows.Forms.TabPage tabExportOptions;
        private System.Windows.Forms.ComboBox cmbParserProfile;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chkAutoRefresh;
        private System.Windows.Forms.TabPage tabActionOptions;
        private System.Windows.Forms.TabPage tabPermissionOptions;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCanExportData;
        private System.Windows.Forms.CheckBox chkCanDeleteData;
        private System.Windows.Forms.CheckBox chkCanAddData;
        private System.Windows.Forms.CheckBox chkCanEditData;
        private System.Windows.Forms.CheckBox chkCanEditConfiguration;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Panel pnlExport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ComboBox cmbExport;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.CheckedListBox chkAvailableActions;
        private System.Windows.Forms.FlowLayoutPanel fpActions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cmbDefaultAction;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnHideSettings;
        private System.Windows.Forms.Button btnShowSettings;
        private System.Windows.Forms.ComboBox cmbDatasource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox chkAvailableExports;
        private System.Windows.Forms.ComboBox cmbEndpoint;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkUseXsltTemplate;
        private System.Windows.Forms.Button btnBrowseXsltFileName;
        private System.Windows.Forms.TextBox txtXsltFileName;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.TabPage tabDataSetConversion;
        private System.Windows.Forms.TextBox txtNodeMapping;
        private System.Windows.Forms.RadioButton rbUseCustomTableLoad;
        private System.Windows.Forms.RadioButton rbUseDatasetLoad;
        private System.Windows.Forms.CheckBox chkShowConfigurationOnLoad;
    }
}

