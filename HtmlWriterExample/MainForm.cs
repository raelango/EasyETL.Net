using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using EasyETL.DataSets;
using EasyETL.Listeners;
using EasyETL.Parsers;
using EasyETL.Writers;
using System.Messaging;
using System.Net;
using System.Diagnostics;

namespace HtmlWriterSample
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbProfile.Items.Clear();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("profiles.xml");
            if (xDoc.SelectSingleNode("profiles") != null)
            {
                foreach (XmlNode xNode in xDoc.SelectSingleNode("profiles").ChildNodes)
                {
                    cmbProfile.Items.Add(xNode.Name);
                }
            }
        }

        private void LoadData()
        {
            lblProgressMessage.Text = "";
            dgParsedData.DataSource = null;

            EasyDataSet rds = null;

            Extractor p = new Extractor(txtFileName.Text);
            p.LoadProfile(cmbProfile.Text);
            p.LineReadAndProcessed += p_LineReadAndProcessed;
            rds = p.Parse();

            string strXml = rds.GetPropertiesAsXml(cmbProfile.Text) ;

            cmbParsedDataSet.Items.Clear();


            foreach (DataTable dt in rds.Tables)
            {
                cmbParsedDataSet.Items.Add(dt.TableName);
            }

            if (cmbParsedDataSet.Items.Count > 0)
            {
                cmbParsedDataSet.Text = cmbParsedDataSet.Items[0].ToString();
                cmbParsedDataSet.SelectedIndex = 0;
            }

            dgParsedData.DataSource = rds;

            rtFailedRecords.Text = (rds.MisReads == null) ? "" : String.Join(Environment.NewLine, rds.MisReads);
            btnExport.Enabled = rds.Tables.Count > 0;
            lblProgressMessage.Text = "";
        }

        private void p_LineReadAndProcessed(object sender, RowReadEventArgs e)
        {
            int result = 0;
            Math.DivRem(e.RowNumber, 1000, out result);
            if (result == 0)
            {
                lblProgressMessage.Text = e.Message + "(" + e.RowNumber.ToString() + ")";
                Application.DoEvents();
            }
        }

        private void ofdButton_Click_1(object sender, EventArgs e)
        {
            ofdBox.FileName = txtFileName.Text;
            ofdBox.CheckFileExists = true;
            if (ofdBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFileName.Text = ofdBox.FileName;
            }
        }

        private void cmbParsedDataSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgParsedData.DataMember = cmbParsedDataSet.Text;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            DateTime StartTime = DateTime.Now;
            LoadData();
            this.UseWaitCursor = false;
            MessageBox.Show(String.Format("Parsed {0} Records successfully and Failed {1} records in {2}  Seconds", dgParsedData.RowCount, rtFailedRecords.Lines.Length, DateTime.Now.Subtract(StartTime).TotalSeconds.ToString()));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ofdBox.CheckFileExists = false;
            if (ofdBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                EasyDataSet rds = (EasyDataSet)dgParsedData.DataSource;
                DatasetWriter dsw = null;
                switch (cmbDestination.Text.ToUpper())
                {
                    case "CSV":
                        dsw = new DelimitedDatasetWriter(rds, ofdBox.FileName) { Delimiter = ',', IncludeHeaders = true, IncludeQuotes = true };
                        break;
                    case "TAB":
                        dsw = new DelimitedDatasetWriter(rds, ofdBox.FileName) { Delimiter = '\t', IncludeHeaders = true, IncludeQuotes = true };
                        break;
                    case "HTML":
                        dsw = new HtmlDatasetWriter(rds, ofdBox.FileName);
                        break;
                    case "WORD":
                        dsw = new OfficeDatasetWriter(rds, ofdBox.FileName);
                        break;
                    case "EXCEL":
                        dsw = new OfficeDatasetWriter(rds, ofdBox.FileName) { DestinationType = OfficeFileType.ExcelWorkbook };
                        break;
                    case "XML":
                        dsw = new XmlDatasetWriter(rds, txtXsltFileName.Text, ofdBox.FileName);
                        break;
                    case "PDF":
                        dsw = new PDFDatasetWriter(rds, ofdBox.FileName);
                        break;
                    case "JSON":
                        dsw = new JsonDatasetWriter(rds, ofdBox.FileName);
                        break;
                }
                dsw.Write();
                MessageBox.Show("Saved file in " + ofdBox.FileName);
            }

        }

        private void cmbDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            XsltPanel.Visible = cmbDestination.Text.ToUpper() == "XML";
        }

        private void xsltButton_Click(object sender, EventArgs e)
        {
            ofdBox.FileName = txtXsltFileName.Text;
            ofdBox.CheckFileExists = true;
            if (ofdBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtXsltFileName.Text = ofdBox.FileName;
            }
        }

    }
}