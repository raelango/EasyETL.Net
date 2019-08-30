namespace EasyXmlSample
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.grpLoadOptions = new System.Windows.Forms.GroupBox();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.cmbDelimited = new System.Windows.Forms.GroupBox();
            this.txtDelimitedComments = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbHeaderRow = new System.Windows.Forms.CheckBox();
            this.grpDelimiters = new System.Windows.Forms.GroupBox();
            this.rbDelimiterSpace = new System.Windows.Forms.RadioButton();
            this.txtCustomDelimiter = new System.Windows.Forms.TextBox();
            this.rbDelimiterCustom = new System.Windows.Forms.RadioButton();
            this.rbDelimiterSemicolon = new System.Windows.Forms.RadioButton();
            this.rbDelimiterTab = new System.Windows.Forms.RadioButton();
            this.rbDelimiterComma = new System.Windows.Forms.RadioButton();
            this.rbDelimiterAutoDetect = new System.Windows.Forms.RadioButton();
            this.grpFixedFileOptions = new System.Windows.Forms.GroupBox();
            this.lstFixedColumnWidths = new System.Windows.Forms.ListBox();
            this.chkFixedFirstRowHasFieldNames = new System.Windows.Forms.CheckBox();
            this.txtFixedWidthComments = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.nupColumnWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.grpTemplate = new System.Windows.Forms.GroupBox();
            this.txtTemplateString = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.grpHtmlOptions = new System.Windows.Forms.GroupBox();
            this.txtXPathQuery = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grpFieldNames = new System.Windows.Forms.GroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.txtOnLoadFileName = new System.Windows.Forms.TextBox();
            this.btnOnLoadSave = new System.Windows.Forms.Button();
            this.txtOnLoadContents = new System.Windows.Forms.TextBox();
            this.btnRefreshOnLoadProfiles = new System.Windows.Forms.Button();
            this.cbOnLoadProfiles = new System.Windows.Forms.ComboBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTransformFileName = new System.Windows.Forms.TextBox();
            this.btnTransformSave = new System.Windows.Forms.Button();
            this.txtTransformText = new System.Windows.Forms.TextBox();
            this.btnTransformProfilesLoad = new System.Windows.Forms.Button();
            this.cbTransformProfiles = new System.Windows.Forms.ComboBox();
            this.cmbFileType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grpDataSource = new System.Windows.Forms.GroupBox();
            this.tabDataSource = new System.Windows.Forms.TabControl();
            this.tabDatasourceFile = new System.Windows.Forms.TabPage();
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
            this.txtDatabaseQuery = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDatabaseConnectionString = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDatabaseConnectionType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.cmbDestination = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmbTableName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtXmlContents = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtXPathContents = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtRegexContents = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtExceptions = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.grpLoadOptions.SuspendLayout();
            this.cmbDelimited.SuspendLayout();
            this.grpDelimiters.SuspendLayout();
            this.grpFixedFileOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupColumnWidth)).BeginInit();
            this.grpTemplate.SuspendLayout();
            this.grpHtmlOptions.SuspendLayout();
            this.grpFieldNames.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.grpDataSource.SuspendLayout();
            this.tabDataSource.SuspendLayout();
            this.tabDatasourceFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRows)).BeginInit();
            this.tabDatasourceText.SuspendLayout();
            this.tabDatasourceDatabase.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpLoadOptions
            // 
            this.grpLoadOptions.Controls.Add(this.btnRefreshData);
            this.grpLoadOptions.Controls.Add(this.cmbDelimited);
            this.grpLoadOptions.Controls.Add(this.grpFixedFileOptions);
            this.grpLoadOptions.Controls.Add(this.grpTemplate);
            this.grpLoadOptions.Controls.Add(this.grpHtmlOptions);
            this.grpLoadOptions.Controls.Add(this.grpFieldNames);
            this.grpLoadOptions.Controls.Add(this.cmbFileType);
            this.grpLoadOptions.Controls.Add(this.label2);
            this.grpLoadOptions.Location = new System.Drawing.Point(796, 13);
            this.grpLoadOptions.Name = "grpLoadOptions";
            this.grpLoadOptions.Size = new System.Drawing.Size(517, 796);
            this.grpLoadOptions.TabIndex = 4;
            this.grpLoadOptions.TabStop = false;
            this.grpLoadOptions.Text = "Load Options";
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Location = new System.Drawing.Point(411, 761);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(100, 23);
            this.btnRefreshData.TabIndex = 13;
            this.btnRefreshData.Text = "Refresh Data";
            this.btnRefreshData.UseVisualStyleBackColor = true;
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // cmbDelimited
            // 
            this.cmbDelimited.Controls.Add(this.txtDelimitedComments);
            this.cmbDelimited.Controls.Add(this.label11);
            this.cmbDelimited.Controls.Add(this.cbHeaderRow);
            this.cmbDelimited.Controls.Add(this.grpDelimiters);
            this.cmbDelimited.Location = new System.Drawing.Point(14, 65);
            this.cmbDelimited.Name = "cmbDelimited";
            this.cmbDelimited.Size = new System.Drawing.Size(491, 213);
            this.cmbDelimited.TabIndex = 11;
            this.cmbDelimited.TabStop = false;
            this.cmbDelimited.Text = "Delimited File Options:";
            // 
            // txtDelimitedComments
            // 
            this.txtDelimitedComments.Location = new System.Drawing.Point(320, 150);
            this.txtDelimitedComments.Multiline = true;
            this.txtDelimitedComments.Name = "txtDelimitedComments";
            this.txtDelimitedComments.Size = new System.Drawing.Size(138, 54);
            this.txtDelimitedComments.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(182, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(132, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Comment Lines Start With:";
            // 
            // cbHeaderRow
            // 
            this.cbHeaderRow.AutoSize = true;
            this.cbHeaderRow.Checked = true;
            this.cbHeaderRow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHeaderRow.Location = new System.Drawing.Point(6, 155);
            this.cbHeaderRow.Name = "cbHeaderRow";
            this.cbHeaderRow.Size = new System.Drawing.Size(153, 17);
            this.cbHeaderRow.TabIndex = 2;
            this.cbHeaderRow.Text = "First Row Has Field Names";
            this.cbHeaderRow.UseVisualStyleBackColor = true;
            // 
            // grpDelimiters
            // 
            this.grpDelimiters.Controls.Add(this.rbDelimiterSpace);
            this.grpDelimiters.Controls.Add(this.txtCustomDelimiter);
            this.grpDelimiters.Controls.Add(this.rbDelimiterCustom);
            this.grpDelimiters.Controls.Add(this.rbDelimiterSemicolon);
            this.grpDelimiters.Controls.Add(this.rbDelimiterTab);
            this.grpDelimiters.Controls.Add(this.rbDelimiterComma);
            this.grpDelimiters.Controls.Add(this.rbDelimiterAutoDetect);
            this.grpDelimiters.Location = new System.Drawing.Point(6, 19);
            this.grpDelimiters.Name = "grpDelimiters";
            this.grpDelimiters.Size = new System.Drawing.Size(479, 125);
            this.grpDelimiters.TabIndex = 1;
            this.grpDelimiters.TabStop = false;
            this.grpDelimiters.Text = "Delimiter:";
            // 
            // rbDelimiterSpace
            // 
            this.rbDelimiterSpace.AutoSize = true;
            this.rbDelimiterSpace.Location = new System.Drawing.Point(7, 94);
            this.rbDelimiterSpace.Name = "rbDelimiterSpace";
            this.rbDelimiterSpace.Size = new System.Drawing.Size(108, 17);
            this.rbDelimiterSpace.TabIndex = 6;
            this.rbDelimiterSpace.TabStop = true;
            this.rbDelimiterSpace.Text = "Space Separated";
            this.rbDelimiterSpace.UseVisualStyleBackColor = true;
            // 
            // txtCustomDelimiter
            // 
            this.txtCustomDelimiter.Enabled = false;
            this.txtCustomDelimiter.Location = new System.Drawing.Point(366, 93);
            this.txtCustomDelimiter.Name = "txtCustomDelimiter";
            this.txtCustomDelimiter.Size = new System.Drawing.Size(86, 20);
            this.txtCustomDelimiter.TabIndex = 5;
            // 
            // rbDelimiterCustom
            // 
            this.rbDelimiterCustom.AutoSize = true;
            this.rbDelimiterCustom.Location = new System.Drawing.Point(238, 93);
            this.rbDelimiterCustom.Name = "rbDelimiterCustom";
            this.rbDelimiterCustom.Size = new System.Drawing.Size(109, 17);
            this.rbDelimiterCustom.TabIndex = 4;
            this.rbDelimiterCustom.TabStop = true;
            this.rbDelimiterCustom.Text = "Custom Separator";
            this.rbDelimiterCustom.UseVisualStyleBackColor = true;
            // 
            // rbDelimiterSemicolon
            // 
            this.rbDelimiterSemicolon.AutoSize = true;
            this.rbDelimiterSemicolon.Location = new System.Drawing.Point(238, 20);
            this.rbDelimiterSemicolon.Name = "rbDelimiterSemicolon";
            this.rbDelimiterSemicolon.Size = new System.Drawing.Size(126, 17);
            this.rbDelimiterSemicolon.TabIndex = 3;
            this.rbDelimiterSemicolon.TabStop = true;
            this.rbDelimiterSemicolon.Text = "Semicolon Separated";
            this.rbDelimiterSemicolon.UseVisualStyleBackColor = true;
            // 
            // rbDelimiterTab
            // 
            this.rbDelimiterTab.AutoSize = true;
            this.rbDelimiterTab.Location = new System.Drawing.Point(238, 58);
            this.rbDelimiterTab.Name = "rbDelimiterTab";
            this.rbDelimiterTab.Size = new System.Drawing.Size(98, 17);
            this.rbDelimiterTab.TabIndex = 2;
            this.rbDelimiterTab.TabStop = true;
            this.rbDelimiterTab.Text = "TAB Separated";
            this.rbDelimiterTab.UseVisualStyleBackColor = true;
            // 
            // rbDelimiterComma
            // 
            this.rbDelimiterComma.AutoSize = true;
            this.rbDelimiterComma.Location = new System.Drawing.Point(6, 58);
            this.rbDelimiterComma.Name = "rbDelimiterComma";
            this.rbDelimiterComma.Size = new System.Drawing.Size(112, 17);
            this.rbDelimiterComma.TabIndex = 1;
            this.rbDelimiterComma.TabStop = true;
            this.rbDelimiterComma.Text = "Comma Separated";
            this.rbDelimiterComma.UseVisualStyleBackColor = true;
            // 
            // rbDelimiterAutoDetect
            // 
            this.rbDelimiterAutoDetect.AutoSize = true;
            this.rbDelimiterAutoDetect.Checked = true;
            this.rbDelimiterAutoDetect.Location = new System.Drawing.Point(7, 20);
            this.rbDelimiterAutoDetect.Name = "rbDelimiterAutoDetect";
            this.rbDelimiterAutoDetect.Size = new System.Drawing.Size(82, 17);
            this.rbDelimiterAutoDetect.TabIndex = 0;
            this.rbDelimiterAutoDetect.TabStop = true;
            this.rbDelimiterAutoDetect.Text = "Auto Detect";
            this.rbDelimiterAutoDetect.UseVisualStyleBackColor = true;
            // 
            // grpFixedFileOptions
            // 
            this.grpFixedFileOptions.Controls.Add(this.lstFixedColumnWidths);
            this.grpFixedFileOptions.Controls.Add(this.chkFixedFirstRowHasFieldNames);
            this.grpFixedFileOptions.Controls.Add(this.txtFixedWidthComments);
            this.grpFixedFileOptions.Controls.Add(this.label12);
            this.grpFixedFileOptions.Controls.Add(this.btnRemove);
            this.grpFixedFileOptions.Controls.Add(this.btnAdd);
            this.grpFixedFileOptions.Controls.Add(this.btnUpdate);
            this.grpFixedFileOptions.Controls.Add(this.nupColumnWidth);
            this.grpFixedFileOptions.Controls.Add(this.label4);
            this.grpFixedFileOptions.Location = new System.Drawing.Point(14, 59);
            this.grpFixedFileOptions.Name = "grpFixedFileOptions";
            this.grpFixedFileOptions.Size = new System.Drawing.Size(491, 213);
            this.grpFixedFileOptions.TabIndex = 12;
            this.grpFixedFileOptions.TabStop = false;
            this.grpFixedFileOptions.Text = "Fixed File Options:";
            // 
            // lstFixedColumnWidths
            // 
            this.lstFixedColumnWidths.FormattingEnabled = true;
            this.lstFixedColumnWidths.Location = new System.Drawing.Point(15, 40);
            this.lstFixedColumnWidths.Name = "lstFixedColumnWidths";
            this.lstFixedColumnWidths.Size = new System.Drawing.Size(186, 134);
            this.lstFixedColumnWidths.TabIndex = 3;
            // 
            // chkFixedFirstRowHasFieldNames
            // 
            this.chkFixedFirstRowHasFieldNames.AutoSize = true;
            this.chkFixedFirstRowHasFieldNames.Checked = true;
            this.chkFixedFirstRowHasFieldNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFixedFirstRowHasFieldNames.Location = new System.Drawing.Point(15, 180);
            this.chkFixedFirstRowHasFieldNames.Name = "chkFixedFirstRowHasFieldNames";
            this.chkFixedFirstRowHasFieldNames.Size = new System.Drawing.Size(153, 17);
            this.chkFixedFirstRowHasFieldNames.TabIndex = 2;
            this.chkFixedFirstRowHasFieldNames.Text = "First Row Has Field Names";
            this.chkFixedFirstRowHasFieldNames.UseVisualStyleBackColor = true;
            // 
            // txtFixedWidthComments
            // 
            this.txtFixedWidthComments.Location = new System.Drawing.Point(347, 152);
            this.txtFixedWidthComments.Multiline = true;
            this.txtFixedWidthComments.Name = "txtFixedWidthComments";
            this.txtFixedWidthComments.Size = new System.Drawing.Size(138, 54);
            this.txtFixedWidthComments.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(209, 157);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(132, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Comment Lines Start With:";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(222, 100);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(222, 67);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add Column";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(308, 38);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            // 
            // nupColumnWidth
            // 
            this.nupColumnWidth.Location = new System.Drawing.Point(222, 40);
            this.nupColumnWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nupColumnWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nupColumnWidth.Name = "nupColumnWidth";
            this.nupColumnWidth.Size = new System.Drawing.Size(75, 20);
            this.nupColumnWidth.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Column Widths:";
            // 
            // grpTemplate
            // 
            this.grpTemplate.Controls.Add(this.txtTemplateString);
            this.grpTemplate.Controls.Add(this.label10);
            this.grpTemplate.Location = new System.Drawing.Point(20, 63);
            this.grpTemplate.Name = "grpTemplate";
            this.grpTemplate.Size = new System.Drawing.Size(491, 213);
            this.grpTemplate.TabIndex = 10;
            this.grpTemplate.TabStop = false;
            this.grpTemplate.Text = "Template Options:";
            // 
            // txtTemplateString
            // 
            this.txtTemplateString.Location = new System.Drawing.Point(117, 23);
            this.txtTemplateString.Multiline = true;
            this.txtTemplateString.Name = "txtTemplateString";
            this.txtTemplateString.Size = new System.Drawing.Size(362, 62);
            this.txtTemplateString.TabIndex = 7;
            this.txtTemplateString.Leave += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Template String:";
            // 
            // grpHtmlOptions
            // 
            this.grpHtmlOptions.Controls.Add(this.txtXPathQuery);
            this.grpHtmlOptions.Controls.Add(this.label7);
            this.grpHtmlOptions.Location = new System.Drawing.Point(20, 65);
            this.grpHtmlOptions.Name = "grpHtmlOptions";
            this.grpHtmlOptions.Size = new System.Drawing.Size(491, 213);
            this.grpHtmlOptions.TabIndex = 7;
            this.grpHtmlOptions.TabStop = false;
            this.grpHtmlOptions.Text = "Html Options:";
            // 
            // txtXPathQuery
            // 
            this.txtXPathQuery.Location = new System.Drawing.Point(117, 26);
            this.txtXPathQuery.Multiline = true;
            this.txtXPathQuery.Name = "txtXPathQuery";
            this.txtXPathQuery.Size = new System.Drawing.Size(362, 62);
            this.txtXPathQuery.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "XPath Query:";
            // 
            // grpFieldNames
            // 
            this.grpFieldNames.Controls.Add(this.tabControl2);
            this.grpFieldNames.Location = new System.Drawing.Point(20, 284);
            this.grpFieldNames.Name = "grpFieldNames";
            this.grpFieldNames.Size = new System.Drawing.Size(491, 458);
            this.grpFieldNames.TabIndex = 3;
            this.grpFieldNames.TabStop = false;
            this.grpFieldNames.Text = "Transformations";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Location = new System.Drawing.Point(9, 20);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(478, 438);
            this.tabControl2.TabIndex = 6;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.label14);
            this.tabPage6.Controls.Add(this.txtOnLoadFileName);
            this.tabPage6.Controls.Add(this.btnOnLoadSave);
            this.tabPage6.Controls.Add(this.txtOnLoadContents);
            this.tabPage6.Controls.Add(this.btnRefreshOnLoadProfiles);
            this.tabPage6.Controls.Add(this.cbOnLoadProfiles);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(470, 412);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "During Load";
            this.tabPage6.UseVisualStyleBackColor = true;
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
            this.txtOnLoadFileName.Location = new System.Drawing.Point(5, 384);
            this.txtOnLoadFileName.Name = "txtOnLoadFileName";
            this.txtOnLoadFileName.Size = new System.Drawing.Size(380, 20);
            this.txtOnLoadFileName.TabIndex = 22;
            // 
            // btnOnLoadSave
            // 
            this.btnOnLoadSave.Location = new System.Drawing.Point(391, 384);
            this.btnOnLoadSave.Name = "btnOnLoadSave";
            this.btnOnLoadSave.Size = new System.Drawing.Size(75, 23);
            this.btnOnLoadSave.TabIndex = 21;
            this.btnOnLoadSave.Text = "Save";
            this.btnOnLoadSave.UseVisualStyleBackColor = true;
            this.btnOnLoadSave.Click += new System.EventHandler(this.btnOnLoadSave_Click);
            // 
            // txtOnLoadContents
            // 
            this.txtOnLoadContents.Location = new System.Drawing.Point(6, 34);
            this.txtOnLoadContents.Multiline = true;
            this.txtOnLoadContents.Name = "txtOnLoadContents";
            this.txtOnLoadContents.Size = new System.Drawing.Size(460, 344);
            this.txtOnLoadContents.TabIndex = 20;
            // 
            // btnRefreshOnLoadProfiles
            // 
            this.btnRefreshOnLoadProfiles.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshOnLoadProfiles.Image")));
            this.btnRefreshOnLoadProfiles.Location = new System.Drawing.Point(439, 6);
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
            this.cbOnLoadProfiles.Size = new System.Drawing.Size(309, 21);
            this.cbOnLoadProfiles.TabIndex = 18;
            this.cbOnLoadProfiles.SelectedIndexChanged += new System.EventHandler(this.cbOnLoadProfiles_SelectedIndexChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.label8);
            this.tabPage7.Controls.Add(this.txtTransformFileName);
            this.tabPage7.Controls.Add(this.btnTransformSave);
            this.tabPage7.Controls.Add(this.txtTransformText);
            this.tabPage7.Controls.Add(this.btnTransformProfilesLoad);
            this.tabPage7.Controls.Add(this.cbTransformProfiles);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(470, 412);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "After Load";
            this.tabPage7.UseVisualStyleBackColor = true;
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
            this.txtTransformFileName.Location = new System.Drawing.Point(5, 384);
            this.txtTransformFileName.Name = "txtTransformFileName";
            this.txtTransformFileName.Size = new System.Drawing.Size(380, 20);
            this.txtTransformFileName.TabIndex = 16;
            // 
            // btnTransformSave
            // 
            this.btnTransformSave.Location = new System.Drawing.Point(391, 384);
            this.btnTransformSave.Name = "btnTransformSave";
            this.btnTransformSave.Size = new System.Drawing.Size(75, 23);
            this.btnTransformSave.TabIndex = 15;
            this.btnTransformSave.Text = "Save";
            this.btnTransformSave.UseVisualStyleBackColor = true;
            this.btnTransformSave.Click += new System.EventHandler(this.btnTransformSave_Click);
            // 
            // txtTransformText
            // 
            this.txtTransformText.Location = new System.Drawing.Point(6, 34);
            this.txtTransformText.Multiline = true;
            this.txtTransformText.Name = "txtTransformText";
            this.txtTransformText.Size = new System.Drawing.Size(460, 344);
            this.txtTransformText.TabIndex = 14;
            this.txtTransformText.Leave += new System.EventHandler(this.txtTransformText_Leave);
            // 
            // btnTransformProfilesLoad
            // 
            this.btnTransformProfilesLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnTransformProfilesLoad.Image")));
            this.btnTransformProfilesLoad.Location = new System.Drawing.Point(439, 6);
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
            this.cbTransformProfiles.Size = new System.Drawing.Size(309, 21);
            this.cbTransformProfiles.TabIndex = 12;
            this.cbTransformProfiles.SelectedIndexChanged += new System.EventHandler(this.cbTransformProfiles_SelectedIndexChanged);
            // 
            // cmbFileType
            // 
            this.cmbFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileType.FormattingEnabled = true;
            this.cmbFileType.Items.AddRange(new object[] {
            "Delimited ",
            "Fixed Width",
            "HL7",
            "Html",
            "HtmlTable",
            "Json",
            "Template",
            "Xml"});
            this.cmbFileType.Location = new System.Drawing.Point(91, 26);
            this.cmbFileType.Name = "cmbFileType";
            this.cmbFileType.Size = new System.Drawing.Size(252, 21);
            this.cmbFileType.Sorted = true;
            this.cmbFileType.TabIndex = 1;
            this.cmbFileType.SelectedIndexChanged += new System.EventHandler(this.cmbFileType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Type:";
            // 
            // grpDataSource
            // 
            this.grpDataSource.Controls.Add(this.tabDataSource);
            this.grpDataSource.Location = new System.Drawing.Point(5, 8);
            this.grpDataSource.Name = "grpDataSource";
            this.grpDataSource.Size = new System.Drawing.Size(775, 164);
            this.grpDataSource.TabIndex = 7;
            this.grpDataSource.TabStop = false;
            this.grpDataSource.Text = "Data Source:";
            // 
            // tabDataSource
            // 
            this.tabDataSource.Controls.Add(this.tabDatasourceFile);
            this.tabDataSource.Controls.Add(this.tabDatasourceText);
            this.tabDataSource.Controls.Add(this.tabDatasourceDatabase);
            this.tabDataSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDataSource.Location = new System.Drawing.Point(3, 16);
            this.tabDataSource.Name = "tabDataSource";
            this.tabDataSource.SelectedIndex = 0;
            this.tabDataSource.Size = new System.Drawing.Size(769, 145);
            this.tabDataSource.TabIndex = 9;
            this.tabDataSource.SelectedIndexChanged += new System.EventHandler(this.tabDataSource_SelectedIndexChanged);
            // 
            // tabDatasourceFile
            // 
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
            this.tabDatasourceFile.Size = new System.Drawing.Size(761, 119);
            this.tabDatasourceFile.TabIndex = 0;
            this.tabDatasourceFile.Text = "File";
            this.tabDatasourceFile.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(168, 94);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "rows";
            // 
            // nudMaxRows
            // 
            this.nudMaxRows.Location = new System.Drawing.Point(126, 94);
            this.nudMaxRows.Name = "nudMaxRows";
            this.nudMaxRows.Size = new System.Drawing.Size(35, 20);
            this.nudMaxRows.TabIndex = 11;
            this.nudMaxRows.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // chkHasMaxRows
            // 
            this.chkHasMaxRows.AutoSize = true;
            this.chkHasMaxRows.Location = new System.Drawing.Point(7, 94);
            this.chkHasMaxRows.Name = "chkHasMaxRows";
            this.chkHasMaxRows.Size = new System.Drawing.Size(112, 17);
            this.chkHasMaxRows.TabIndex = 10;
            this.chkHasMaxRows.Text = "Stop loading after ";
            this.chkHasMaxRows.UseVisualStyleBackColor = true;
            // 
            // cbTextExtractor
            // 
            this.cbTextExtractor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTextExtractor.FormattingEnabled = true;
            this.cbTextExtractor.Items.AddRange(new object[] {
            "PDF",
            "Word"});
            this.cbTextExtractor.Location = new System.Drawing.Point(145, 70);
            this.cbTextExtractor.Name = "cbTextExtractor";
            this.cbTextExtractor.Size = new System.Drawing.Size(242, 21);
            this.cbTextExtractor.TabIndex = 9;
            this.cbTextExtractor.Visible = false;
            this.cbTextExtractor.SelectedIndexChanged += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // chkUseTextExtractor
            // 
            this.chkUseTextExtractor.AutoSize = true;
            this.chkUseTextExtractor.Location = new System.Drawing.Point(7, 70);
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
            this.btnLoad.Location = new System.Drawing.Point(675, 30);
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
            this.txtFileName.Location = new System.Drawing.Point(6, 32);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(663, 20);
            this.txtFileName.TabIndex = 6;
            // 
            // tabDatasourceText
            // 
            this.tabDatasourceText.Controls.Add(this.txtTextContents);
            this.tabDatasourceText.Location = new System.Drawing.Point(4, 22);
            this.tabDatasourceText.Name = "tabDatasourceText";
            this.tabDatasourceText.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatasourceText.Size = new System.Drawing.Size(761, 119);
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
            this.txtTextContents.Size = new System.Drawing.Size(755, 113);
            this.txtTextContents.TabIndex = 10;
            this.txtTextContents.Leave += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // tabDatasourceDatabase
            // 
            this.tabDatasourceDatabase.Controls.Add(this.txtDatabaseQuery);
            this.tabDatasourceDatabase.Controls.Add(this.label6);
            this.tabDatasourceDatabase.Controls.Add(this.txtDatabaseConnectionString);
            this.tabDatasourceDatabase.Controls.Add(this.label5);
            this.tabDatasourceDatabase.Controls.Add(this.cmbDatabaseConnectionType);
            this.tabDatasourceDatabase.Controls.Add(this.label1);
            this.tabDatasourceDatabase.Location = new System.Drawing.Point(4, 22);
            this.tabDatasourceDatabase.Name = "tabDatasourceDatabase";
            this.tabDatasourceDatabase.Size = new System.Drawing.Size(761, 119);
            this.tabDatasourceDatabase.TabIndex = 2;
            this.tabDatasourceDatabase.Text = "Database";
            this.tabDatasourceDatabase.UseVisualStyleBackColor = true;
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
            // txtDatabaseConnectionString
            // 
            this.txtDatabaseConnectionString.Location = new System.Drawing.Point(113, 3);
            this.txtDatabaseConnectionString.Multiline = true;
            this.txtDatabaseConnectionString.Name = "txtDatabaseConnectionString";
            this.txtDatabaseConnectionString.Size = new System.Drawing.Size(362, 43);
            this.txtDatabaseConnectionString.TabIndex = 17;
            this.txtDatabaseConnectionString.Leave += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Connection String:";
            // 
            // cmbDatabaseConnectionType
            // 
            this.cmbDatabaseConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseConnectionType.FormattingEnabled = true;
            this.cmbDatabaseConnectionType.Items.AddRange(new object[] {
            "Odbc",
            "Oledb",
            "Sql"});
            this.cmbDatabaseConnectionType.Location = new System.Drawing.Point(592, 3);
            this.cmbDatabaseConnectionType.Name = "cmbDatabaseConnectionType";
            this.cmbDatabaseConnectionType.Size = new System.Drawing.Size(158, 21);
            this.cmbDatabaseConnectionType.Sorted = true;
            this.cmbDatabaseConnectionType.TabIndex = 15;
            this.cmbDatabaseConnectionType.SelectedIndexChanged += new System.EventHandler(this.txtTextContents_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(490, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Connection Type:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(5, 178);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 631);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblRecordCount);
            this.tabPage1.Controls.Add(this.cmbDestination);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.btnExport);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.cmbTableName);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Dataset";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Location = new System.Drawing.Point(503, 15);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(169, 25);
            this.lblRecordCount.TabIndex = 14;
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbDestination
            // 
            this.cmbDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestination.FormattingEnabled = true;
            this.cmbDestination.Items.AddRange(new object[] {
            "CSV",
            "TAB",
            "HTML",
            "WORD",
            "EXCEL",
            "XML",
            "PDF",
            "JSON"});
            this.cmbDestination.Location = new System.Drawing.Point(440, 19);
            this.cmbDestination.Name = "cmbDestination";
            this.cmbDestination.Size = new System.Drawing.Size(57, 21);
            this.cmbDestination.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(358, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Export Format:";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(678, 19);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(9, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(744, 551);
            this.dataGridView1.TabIndex = 10;
            // 
            // cmbTableName
            // 
            this.cmbTableName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableName.FormattingEnabled = true;
            this.cmbTableName.Location = new System.Drawing.Point(88, 19);
            this.cmbTableName.Name = "cmbTableName";
            this.cmbTableName.Size = new System.Drawing.Size(246, 21);
            this.cmbTableName.TabIndex = 7;
            this.cmbTableName.SelectedIndexChanged += new System.EventHandler(this.cmbTableName_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Table Name:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtXmlContents);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 605);
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
            this.txtXmlContents.Size = new System.Drawing.Size(761, 599);
            this.txtXmlContents.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtXPathContents);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(767, 605);
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
            this.txtXPathContents.Size = new System.Drawing.Size(767, 605);
            this.txtXPathContents.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtRegexContents);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(767, 605);
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
            this.txtRegexContents.Size = new System.Drawing.Size(767, 605);
            this.txtRegexContents.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtExceptions);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(767, 605);
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
            this.txtExceptions.Size = new System.Drawing.Size(767, 605);
            this.txtExceptions.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBarLabel,
            this.ProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 848);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1325, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusBarLabel
            // 
            this.StatusBarLabel.Name = "StatusBarLabel";
            this.StatusBarLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // ProgressTimer
            // 
            this.ProgressTimer.Enabled = true;
            this.ProgressTimer.Interval = 1000;
            this.ProgressTimer.Tick += new System.EventHandler(this.ProgressTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1325, 870);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.grpDataSource);
            this.Controls.Add(this.grpLoadOptions);
            this.Name = "MainForm";
            this.Text = "Easy Controls Demo Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpLoadOptions.ResumeLayout(false);
            this.grpLoadOptions.PerformLayout();
            this.cmbDelimited.ResumeLayout(false);
            this.cmbDelimited.PerformLayout();
            this.grpDelimiters.ResumeLayout(false);
            this.grpDelimiters.PerformLayout();
            this.grpFixedFileOptions.ResumeLayout(false);
            this.grpFixedFileOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupColumnWidth)).EndInit();
            this.grpTemplate.ResumeLayout(false);
            this.grpTemplate.PerformLayout();
            this.grpHtmlOptions.ResumeLayout(false);
            this.grpHtmlOptions.PerformLayout();
            this.grpFieldNames.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.grpDataSource.ResumeLayout(false);
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
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.GroupBox grpLoadOptions;
        private System.Windows.Forms.ComboBox cmbFileType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpFieldNames;
        private System.Windows.Forms.GroupBox grpDataSource;
        private System.Windows.Forms.GroupBox grpHtmlOptions;
        private System.Windows.Forms.TextBox txtXPathQuery;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmbTableName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtXmlContents;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusBarLabel;
        private System.Windows.Forms.TabControl tabDataSource;
        private System.Windows.Forms.TabPage tabDatasourceFile;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TabPage tabDatasourceText;
        private System.Windows.Forms.TextBox txtTextContents;
        private System.Windows.Forms.TabPage tabDatasourceDatabase;
        private System.Windows.Forms.TextBox txtDatabaseQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDatabaseConnectionString;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDatabaseConnectionType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.Timer ProgressTimer;
        private System.Windows.Forms.ComboBox cmbDestination;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtXPathContents;
        private System.Windows.Forms.GroupBox grpTemplate;
        private System.Windows.Forms.TextBox txtTemplateString;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtRegexContents;
        private System.Windows.Forms.ComboBox cbTextExtractor;
        private System.Windows.Forms.CheckBox chkUseTextExtractor;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txtExceptions;
        private System.Windows.Forms.GroupBox cmbDelimited;
        private System.Windows.Forms.TextBox txtDelimitedComments;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbHeaderRow;
        private System.Windows.Forms.GroupBox grpDelimiters;
        private System.Windows.Forms.RadioButton rbDelimiterSpace;
        private System.Windows.Forms.TextBox txtCustomDelimiter;
        private System.Windows.Forms.RadioButton rbDelimiterCustom;
        private System.Windows.Forms.RadioButton rbDelimiterSemicolon;
        private System.Windows.Forms.RadioButton rbDelimiterTab;
        private System.Windows.Forms.RadioButton rbDelimiterComma;
        private System.Windows.Forms.RadioButton rbDelimiterAutoDetect;
        private System.Windows.Forms.GroupBox grpFixedFileOptions;
        private System.Windows.Forms.ListBox lstFixedColumnWidths;
        private System.Windows.Forms.CheckBox chkFixedFirstRowHasFieldNames;
        private System.Windows.Forms.TextBox txtFixedWidthComments;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.NumericUpDown nupColumnWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudMaxRows;
        private System.Windows.Forms.CheckBox chkHasMaxRows;
        private System.Windows.Forms.Button btnRefreshData;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
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
    }
}

