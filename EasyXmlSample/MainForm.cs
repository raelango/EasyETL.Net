using EasyETL.DataSets;
using EasyETL.Extractors;
using EasyETL.Writers;
using EasyXml;
using EasyXml.Parsers;
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
    public partial class MainForm : Form
    {
        EasyXmlDocument ezDoc = null;
        XslCompiledTransform xsl = null;
        Stopwatch stopWatch = new Stopwatch();
        public int intAutoNumber = 0;
        public string SettingsPath = "";
        public string SettingsFileName = "";


        public void LoadSettingsFromXml(string settingsFileName, string settingsPath)
        {
            SettingsFileName = settingsFileName;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            string clientName = settingsPath.Split('\\')[0];
            string etlName = settingsPath.Split('\\')[2];
            SettingsPath = "//clients/client[@name='" + clientName + "']/etls/etl[@name='" + etlName + "']";
            XmlNode xNode = xDoc.SelectSingleNode(SettingsPath);
            if (xNode != null)
            {
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
                string delimiter = "";
                XmlNode optionsNode = xNode.SelectSingleNode("parseoptions");
                if (optionsNode != null) { 
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
                            Control c = this.Controls.Find("rbDelimiter" + delimiter,true)[0];
                            ((CheckBox)c).Checked = true;
                            break;
                        case "hasheader":
                            cbHeaderRow.Checked = Convert.ToBoolean(xAttr.Value);
                            break;
                        case "separator":
                            txtCustomDelimiter.Text = xAttr.InnerText; break;
                    }
                    if (optionsNode.SelectSingleNode("comments") != null)
                    {
                        if (delimiter == "Delimited")
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

                    XmlNodeList widthList = xNode.SelectNodes("width");
                    if (widthList != null)
                    {
                        lstFixedColumnWidths.Items.Clear();
                        foreach (XmlNode widthNode in widthList)
                        {
                            lstFixedColumnWidths.Items.Add(Convert.ToInt16(widthNode.InnerText));
                        }
                    }


                }
                }
                #endregion

                #region Output Settings Load
                XmlNode dataNode = xNode.SelectSingleNode("output");
                if ((dataNode != null) && (dataNode.Attributes["exportformat"] != null)) cmbDestination.SelectedItem = dataNode.Attributes["exportformat"].Value;
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
            }

        }

        public MainForm()
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
                LoadDataToGridView();
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
            ProgressBar.Visible = true;
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
                //xDoc.OnRowAdd += xDoc_OnRowAdd;
                xDoc.OnError += xDoc_OnError;
                if (tabDataSource.SelectedTab == tabDatasourceDatabase)
                {
                    DatabaseEasyParser dbep = null;
                    switch (cmbDatabaseConnectionType.Text.TrimEnd())
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
                    switch (cmbFileType.Text.TrimEnd())
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
            ProgressBar.Visible = false;
            this.UseWaitCursor = false;
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
            try
            {
                ds = xDoc.ToDataSet();
                if (ds != null)
                {
                    foreach (DataTable table in ds.Tables)
                    {
                        cmbTableName.Items.Add(table.TableName);
                    }
                }
                dataGridView1.DataSource = ds;
                if (cmbTableName.Items.Count > 0)
                {
                    cmbTableName.SelectedIndex = 0;
                    dataGridView1.DataMember = cmbTableName.Text;
                    lblRecordCount.Text = "(No Records)";
                    if (dataGridView1.RowCount > 0) lblRecordCount.Text = dataGridView1.RowCount + " Record(s)";
                }
            }
            catch
            {

            }

            stopWatch.Stop();
            lblRecordCount.Text += Environment.NewLine + String.Format("[{0}:{1}:{2}.{3}]", stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();
        }

        public void LoadControls()
        {
            cmbFileType.SelectedIndex = 0;
            cmbDatabaseConnectionType.SelectedIndex = 0;
            ProgressBar.Visible = false;
            btnRefreshOnLoadProfiles_Click(this, null);
            btnTransformProfilesLoad_Click(this, null);
        }

        private void cmbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDelimited.Visible = false;
            grpFixedFileOptions.Visible = false;
            grpHtmlOptions.Visible = false;
            grpTemplate.Visible = false;
            switch (cmbFileType.Text.TrimEnd())
            {
                case "Delimited":
                    cmbDelimited.Visible = true;
                    cmbDelimited.BringToFront();
                    break;
                case "Fixed Width":
                    grpFixedFileOptions.Visible = true;
                    grpFixedFileOptions.BringToFront();
                    break;
                case "Html":
                    grpHtmlOptions.Visible = true;
                    grpHtmlOptions.Top = grpFixedFileOptions.Top;
                    grpHtmlOptions.Left = grpFixedFileOptions.Left;
                    break;
                case "Template":
                    grpTemplate.Visible = true;
                    txtTemplateString.Visible = true;
                    grpTemplate.Top = grpFixedFileOptions.Top;
                    grpTemplate.Left = grpFixedFileOptions.Left;
                    break;

            }
            LoadDataToGridView();
        }

        private void rbDelimiterCustom_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomDelimiter.Enabled = rbDelimiterCustom.Checked;
            if (txtCustomDelimiter.Enabled) txtCustomDelimiter.Focus();
        }

        private void cmbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataMember = cmbTableName.Text;
            lblRecordCount.Text = "(No Records)";
            if (dataGridView1.RowCount > 0)
            {
                lblRecordCount.Text = dataGridView1.RowCount + " Record(s)";
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
            LoadDataToGridView();
        }

        private void tabDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            grpLoadOptions.Visible = (tabDataSource.SelectedTab != tabDatasourceDatabase);
        }


        private void cbTransformProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTransformFileName.Text = cbTransformProfiles.Text;
            txtTransformText.Text = File.ReadAllText(Path.Combine(Application.StartupPath, cbTransformProfiles.Text + ".transforms"));
            TransformDataFromEzDoc();
        }

        private void btnTransformProfilesLoad_Click(object sender, EventArgs e)
        {
            cbTransformProfiles.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(Application.StartupPath, "*.transforms"))
            {
                cbTransformProfiles.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (ProgressBar.Visible)
            {
                if (ProgressBar.Value == 100) ProgressBar.Value = 0;
                ProgressBar.Value += 1;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ofd.CheckFileExists = false;
            ofd.FileName = Path.ChangeExtension(ofd.FileName, cmbDestination.Text);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DataSet rds = (DataSet)dataGridView1.DataSource;
                DatasetWriter dsw = null;
                switch (cmbDestination.Text.ToUpper())
                {
                    case "CSV":
                        dsw = new DelimitedDatasetWriter(rds, ofd.FileName) { Delimiter = ',', IncludeHeaders = true, IncludeQuotes = true };
                        break;
                    case "TAB":
                        dsw = new DelimitedDatasetWriter(rds, ofd.FileName) { Delimiter = '\t', IncludeHeaders = true, IncludeQuotes = true };
                        break;
                    case "HTML":
                        dsw = new HtmlDatasetWriter(rds, ofd.FileName);
                        break;
                    case "WORD":
                        dsw = new OfficeDatasetWriter(rds, ofd.FileName);
                        break;
                    case "EXCEL":
                        dsw = new OfficeDatasetWriter(rds, ofd.FileName) { DestinationType = OfficeFileType.ExcelWorkbook };
                        break;
                    case "XML":
                        dsw = new XmlDatasetWriter(rds, "", ofd.FileName);
                        break;
                    case "PDF":
                        dsw = new PDFDatasetWriter(rds, ofd.FileName);
                        break;
                    case "JSON":
                        dsw = new JsonDatasetWriter(rds, ofd.FileName);
                        break;
                }
                dsw.Write();
                MessageBox.Show("Saved file in " + ofd.FileName);
                Process.Start(ofd.FileName);
            }
        }

        private void btnTransformSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtTransformFileName.Text))
            {
                File.WriteAllText(Path.Combine(Application.StartupPath, txtTransformFileName.Text + ".transforms"), txtTransformText.Text);
                if (!cbTransformProfiles.Items.Contains(txtTransformFileName.Text)) cbTransformProfiles.Items.Add(txtTransformFileName.Text);
                cbTransformProfiles.SelectedText = txtTransformFileName.Text;
            }
        }

        private void txtTransformText_Leave(object sender, EventArgs e)
        {
            TransformDataFromEzDoc();
        }

        private void chkUseTextExtractor_CheckedChanged(object sender, EventArgs e)
        {
            cbTextExtractor.Visible = chkUseTextExtractor.Checked;
            LoadDataToGridView();
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void btnRefreshOnLoadProfiles_Click(object sender, EventArgs e)
        {
            cbOnLoadProfiles.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(Application.StartupPath, "*.transforms"))
            {
                cbOnLoadProfiles.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        private void btnOnLoadSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtOnLoadFileName.Text))
            {
                File.WriteAllText(Path.Combine(Application.StartupPath, txtOnLoadFileName.Text + ".transforms"), txtOnLoadContents.Text);
                if (!cbOnLoadProfiles.Items.Contains(txtOnLoadFileName.Text)) cbOnLoadProfiles.Items.Add(txtOnLoadFileName.Text);
                cbOnLoadProfiles.SelectedText = txtOnLoadFileName.Text;
            }
        }

        private void cbOnLoadProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOnLoadFileName.Text = cbOnLoadProfiles.Text;
            txtOnLoadContents.Text = File.ReadAllText(Path.Combine(Application.StartupPath, cbOnLoadProfiles.Text + ".transforms"));
            LoadDataToGridView();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettingsToXmlFile();
        }

        private void SaveSettingsToXmlFile()
        {
            if (String.IsNullOrWhiteSpace(SettingsFileName)) return;
            if (!File.Exists(SettingsFileName)) return;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(SettingsFileName);
            XmlNode xNode = xDoc.SelectSingleNode(SettingsPath);
            xNode.InnerText = "";
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
            optionsNode.SetAttribute("parsertype", cmbFileType.Text);
            switch (cmbFileType.Text)
            {
                case "Delimited":
                    string delimiter = "AutoDetect";
                    if (rbDelimiterComma.Checked) delimiter  = "Comma";
                    if (rbDelimiterSemicolon.Checked) delimiter = "Semicolon";
                    if (rbDelimiterSpace.Checked) delimiter = "Space";
                    if (rbDelimiterTab.Checked) delimiter = "Tab";
                    if (rbDelimiterCustom.Checked) delimiter = "Custom";
                    string separator = String.Empty;
                    if (rbDelimiterCustom.Checked) separator = txtCustomDelimiter.Text;
                    optionsNode.SetAttribute("delimiter",delimiter);
                    optionsNode.SetAttribute("separator",separator);
                    optionsNode.SetAttribute("hasheader",cbHeaderRow.Checked.ToString());
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
            xNode.AppendChild(optionsNode);
            #endregion

            #region Output Settings 
            XmlElement dataNode = xDoc.CreateElement("output");
            dataNode.SetAttribute("exportformat", cmbDestination.Text);
            xNode.AppendChild(dataNode);
            #endregion

            xDoc.Save(SettingsFileName);
        }

        private void btnCloseWindow_Click(object sender, EventArgs e)
        {
            if ( (this.Parent != null) && (this.Parent is TabPage))
            {
                TabPage tpage = (TabPage)this.Parent;
                TabControl tcontrol = (TabControl)tpage.Parent;
                tcontrol.TabPages.Remove(tpage);
            }
        }

    }
}
