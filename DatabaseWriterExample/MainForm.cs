using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using EasyETL.DataSets;
using EasyETL.Listeners;
using EasyETL.Parsers;
using EasyETL.Writers;
using System.Messaging;
using System.Diagnostics;

namespace DatabaseWriterSample
{
    public partial class MainForm : Form
    {
        public int intUpdates = 0;
        public int intInserts = 0;
        public int intErrors = 0;
        DatabasePropertiesForm dpForm = new DatabasePropertiesForm();

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

            Extractor p = new Extractor(txtFileName.Text);
            p.LoadProfile(cmbProfile.Text);
            p.LineReadAndProcessed += p_LineReadAndProcessed;

            RegexDataSet rds = p.Parse();

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

        private void p_LineReadAndProcessed(object sender, LinesReadEventArgs e)
        {
            int result = 0;
            Math.DivRem(e.LineNumber, 1000, out result);
            if (result == 0)
            {
                lblProgressMessage.Text = e.Message + "(" + e.LineNumber.ToString() + ")";
                Application.DoEvents();
            }
        }

        private void ofdButton_Click_1(object sender, EventArgs e)
        {
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
            if (dpForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RegexDataSet rds = (RegexDataSet)dgParsedData.DataSource;
                DatabaseDatasetWriter dsw = new DatabaseDatasetWriter((DatabaseType) Enum.Parse(typeof(DatabaseType),dpForm.DatabaseType), rds, dpForm.ConnectionString, dpForm.InsertCommand, dpForm.UpdateCommand, cmbParsedDataSet.Text);
                intUpdates = 0;
                intInserts = 0;
                intErrors = 0;

                dsw.RowInserted += dsw_RowInserted;
                dsw.RowUpdated += dsw_RowUpdated;
                dsw.RowErrored += dsw_RowErrored;
                dsw.Write();
                MessageBox.Show(String.Format("{0} Inserts, {1} Updates and {2} Errors.", intInserts, intUpdates, intErrors));
            }
        }

        void dsw_RowErrored(object sender, RowWrittenEventArgs e)
        {
            intErrors ++;
            MessageBox.Show("Error importing Row " + e.RowNumber.ToString() + ", Table Name = " + e.TableName + ", error = " + e.Row.RowError);
        }

        void dsw_RowUpdated(object sender, RowWrittenEventArgs e)
        {
            intUpdates ++;
        }

        void dsw_RowInserted(object sender, RowWrittenEventArgs e)
        {
            intInserts ++;
        }
    }
}