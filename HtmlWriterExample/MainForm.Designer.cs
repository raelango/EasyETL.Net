namespace HtmlWriterSample
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
            this.ofdBox = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblProgressMessage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProfile = new System.Windows.Forms.ComboBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ofdButton = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbParsedDataSet = new System.Windows.Forms.ComboBox();
            this.dgParsedData = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtFailedRecords = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgParsedData)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdBox
            // 
            this.ofdBox.FileName = "openFileDialog1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.tabControl1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1121, 721);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblProgressMessage);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbProfile);
            this.panel1.Controls.Add(this.btnReload);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ofdButton);
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1112, 61);
            this.panel1.TabIndex = 11;
            // 
            // lblProgressMessage
            // 
            this.lblProgressMessage.AutoSize = true;
            this.lblProgressMessage.Location = new System.Drawing.Point(503, 9);
            this.lblProgressMessage.Name = "lblProgressMessage";
            this.lblProgressMessage.Size = new System.Drawing.Size(0, 13);
            this.lblProgressMessage.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(764, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Parse Profile:";
            // 
            // cmbProfile
            // 
            this.cmbProfile.FormattingEnabled = true;
            this.cmbProfile.Location = new System.Drawing.Point(839, 28);
            this.cmbProfile.Name = "cmbProfile";
            this.cmbProfile.Size = new System.Drawing.Size(171, 21);
            this.cmbProfile.TabIndex = 10;
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(1016, 26);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 7;
            this.btnReload.Text = "&Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please select the File Name (or Connection String):";
            // 
            // ofdButton
            // 
            this.ofdButton.Location = new System.Drawing.Point(682, 26);
            this.ofdButton.Name = "ofdButton";
            this.ofdButton.Size = new System.Drawing.Size(75, 23);
            this.ofdButton.TabIndex = 9;
            this.ofdButton.Text = "&Browse";
            this.ofdButton.UseVisualStyleBackColor = true;
            this.ofdButton.Click += new System.EventHandler(this.ofdButton_Click_1);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(4, 26);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(659, 20);
            this.txtFileName.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(3, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1119, 653);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.dgParsedData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1111, 627);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Parsed Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnExport);
            this.panel2.Controls.Add(this.cmbParsedDataSet);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1105, 28);
            this.panel2.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(294, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(91, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export To HTML";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cmbParsedDataSet
            // 
            this.cmbParsedDataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParsedDataSet.FormattingEnabled = true;
            this.cmbParsedDataSet.Location = new System.Drawing.Point(4, 4);
            this.cmbParsedDataSet.Name = "cmbParsedDataSet";
            this.cmbParsedDataSet.Size = new System.Drawing.Size(274, 21);
            this.cmbParsedDataSet.TabIndex = 0;
            this.cmbParsedDataSet.SelectedIndexChanged += new System.EventHandler(this.cmbParsedDataSet_SelectedIndexChanged);
            // 
            // dgParsedData
            // 
            this.dgParsedData.AllowUserToAddRows = false;
            this.dgParsedData.AllowUserToDeleteRows = false;
            this.dgParsedData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgParsedData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgParsedData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgParsedData.Location = new System.Drawing.Point(3, 34);
            this.dgParsedData.Name = "dgParsedData";
            this.dgParsedData.ReadOnly = true;
            this.dgParsedData.RowHeadersVisible = false;
            this.dgParsedData.Size = new System.Drawing.Size(1105, 590);
            this.dgParsedData.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rtFailedRecords);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1111, 627);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Errors";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rtFailedRecords
            // 
            this.rtFailedRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtFailedRecords.Location = new System.Drawing.Point(3, 3);
            this.rtFailedRecords.Name = "rtFailedRecords";
            this.rtFailedRecords.Size = new System.Drawing.Size(1105, 621);
            this.rtFailedRecords.TabIndex = 4;
            this.rtFailedRecords.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 721);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "HTML Writer Sample";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgParsedData)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button ofdButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgParsedData;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rtFailedRecords;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbParsedDataSet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbProfile;
        private System.Windows.Forms.Label lblProgressMessage;
        private System.Windows.Forms.Button btnExport;
    }
}

