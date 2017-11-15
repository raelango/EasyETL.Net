using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using EasyETL.DataSets;
using EasyETL.Listeners;
using EasyETL.Parsers;
using System.Messaging;
using System.Diagnostics;

namespace MSMQListenerSample
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

            Extractor p = new Extractor(txtFileName.Text);
            p.LoadProfile(cmbProfile.Text);
            p.LineReadAndProcessed += p_LineReadAndProcessed;

            RegexDataSet rds = (RegexDataSet)p.Parse();

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
            SetupTimer();
            //LoadData();
            this.UseWaitCursor = false;
            MessageBox.Show(String.Format("Parsed {0} Records successfully and Failed {1} records in {2}  Seconds", dgParsedData.RowCount, rtFailedRecords.Lines.Length, DateTime.Now.Subtract(StartTime).TotalSeconds.ToString()));
        }

        private void SetupTimer()
        {
            EventDataSet eds = new EventDataSet("Application");
            eds.RowReadAndProcessed += eds_RowReadAndProcessed;
            eds.Fill();
            cmbParsedDataSet.Items.Clear();


            foreach (DataTable dt in eds.Tables)
            {
                cmbParsedDataSet.Items.Add(dt.TableName);
            }

            if (cmbParsedDataSet.Items.Count > 0)
            {
                cmbParsedDataSet.Text = cmbParsedDataSet.Items[0].ToString();
                cmbParsedDataSet.SelectedIndex = 0;
            }

            dgParsedData.DataSource = eds;
            //EventLogListener ml = new EventLogListener(this, "Application");
            //ml.OnTriggered += ml_OnTriggered;
            //ml.Start();

            //EventLog myLog = new EventLog("Application", ".");
            //myLog.Source = "VSS";
            //// Write an entry to the log.        
            //myLog.WriteEntry("Writing to event log on " + myLog.MachineName);
            //myLog.Close();
            //System.Messaging.Message mm = new System.Messaging.Message();
            //mm.Label = "Testing Label";
            //mm.Body = "This is testing the body....";

            //System.Messaging.MessageQueue mq = new MessageQueue(".\\ArasuElango");
            //mq.Send(mm);
            
            //FileListener fl = new FileListener(this, txtFileName.Text);
            //fl.OnTriggered += fl_OnTriggered;
            //LoadData();
            //fl.Start();
            //TimerListener list = new TimerListener(this, ListenerDaysOfWeek.Daily, 5,DateTime.Parse("10:55 AM"), DateTime.Parse("11:00 AM"));
            //list.OnTriggered += list_OnTriggered;
            //list.Start();
        }

        void eds_RowReadAndProcessed(object sender, RowReadEventArgs e)
        {
            int result = 0;
            Math.DivRem(e.RowNumber, 1000, out result);
            if (result == 0)
            {
                lblProgressMessage.Text = e.Message + "(" + e.RowNumber.ToString() + ")";
                Application.DoEvents();
            }
        }

        void ml_OnTriggered(object sender, ListenerTriggeredEventArgs e)
        {
            string result = "This is testing";
            Console.WriteLine(result);
        }

        void fl_OnTriggered(object sender, ListenerTriggeredEventArgs e)
        {
            RegexDataSet rds = (RegexDataSet)dgParsedData.DataSource;
            string addnlData = e.Data["AdditionalContent"].ToString();
            if (!String.IsNullOrEmpty(addnlData))
            {
                rds.ParseAndLoadLines(e.Data["AdditionalContent"].ToString());
            }
            dgParsedData.Invoke(new MethodInvoker(() => { dgParsedData.Refresh(); }));
            //Console.WriteLine("Here");
        }

        void list_OnTriggered(object sender, ListenerTriggeredEventArgs e)
        {
            Console.WriteLine("I am here");
        }
    }
}