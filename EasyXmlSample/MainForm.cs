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

namespace EasyXmlSample
{
    public partial class MainForm : Form
    {
        EasyXmlDocument ezDoc = null;
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

        private void LoadDataToGridView()
        {
            if ((tabDataSource.SelectedTab == tabDatasourceText) && (txtTextContents.TextLength == 0)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceFile) && (txtFileName.TextLength == 0)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceFile) && !File.Exists(txtFileName.Text)) return;
            if ((tabDataSource.SelectedTab == tabDatasourceDatabase) && (String.IsNullOrWhiteSpace(txtDatabaseConnectionString.Text) || String.IsNullOrWhiteSpace(txtDatabaseQuery.Text))) return;
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
                EasyXmlDocument xDoc = new EasyXmlDocument();
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
                            break;
                        case "HL7":
                            ep = new HL7EasyParser();
                            break;
                        case "Fixed Width":
                            ep = new FixedWidthEasyParser(false, lstFixedColumnWidths.Items.Cast<int>().ToArray());
                            break;
                        case "Json":
                            ep = new JsonEasyParser();
                            break;
                        case "Template":
                            if (String.IsNullOrWhiteSpace(txtTemplateString.Text)) txtTemplateString.Text = "[Contents]";
                            ep = new TemplateEasyParser() { TemplateString = txtTemplateString.Text };
                            ((TemplateEasyParser)ep).LoadStr("");
                            txtRegexContents.Text = ((TemplateEasyParser)ep).RegexString;
                            break;
                        case "HtmlTable":
                            ep = new HtmlTableEasyParser();
                            break;
                    }
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
                TransformDataFromEzDoc();

            }
            catch
            {
                MessageBox.Show("Error loading contents...");
            }
            ProgressBar.Visible = false;
            this.UseWaitCursor = false;
        }

        private void TransformDataFromEzDoc()
        {
            lblRecordCount.Text = "";
            cmbTableName.Items.Clear();
            EasyXmlDocument xDoc = (EasyXmlDocument)ezDoc.Clone();
            if (!String.IsNullOrWhiteSpace(cbTransformProfiles.Text))
            {
                xDoc.Transform(txtTransformText.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            }

            if (!String.IsNullOrWhiteSpace(txtXPathQuery.Text))
            {
                xDoc = xDoc.ApplyFilter(txtXPathQuery.Text);
            }
            txtXmlContents.Text = xDoc.Beautify();
            txtXPathContents.Text = xDoc.LastTransformerTemplate;

            DataSet ds = null;
            if (grpHtmlOptions.Visible)
            {
                ds = xDoc.ToDataSet(txtXPathQuery.Text);
            }
            else
            {
                ds = xDoc.ToDataSet();
            }

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

        void xDoc_NodeInserted(object sender, XmlNodeChangedEventArgs e)
        {
            StatusBarLabel.Text = "Inserted " + e.Node.Name;
            Application.DoEvents();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbFileType.SelectedIndex = 0;
            cmbDatabaseConnectionType.SelectedIndex = 0;
            ProgressBar.Visible = false;
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
                    break;
                case "Fixed Width":
                    grpFixedFileOptions.Visible = true;
                    break;
                case "Html":
                    grpHtmlOptions.Visible = true;
                    break;
                case "Template":
                    grpTemplate.Visible = true;
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

        private void txtDatabaseQuery_Leave(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void txtDatabaseConnectionString_Leave(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void cmbDatabaseConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void tabDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            grpLoadOptions.Visible = (tabDataSource.SelectedTab != tabDatasourceDatabase);
        }

        private void cbHeaderRow_CheckedChanged(object sender, EventArgs e)
        {
            LoadDataToGridView();
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

        private void cbOnLoadTransformationProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ofd.CheckFileExists = false;
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
                        dsw = new XmlDatasetWriter(rds,"" , ofd.FileName);
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

        private void txtTemplateString_Leave(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void chkUseTextExtractor_CheckedChanged(object sender, EventArgs e)
        {
            cbTextExtractor.Visible = chkUseTextExtractor.Checked;
            LoadDataToGridView();
        }

        private void cbTextExtractor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

    }
}
