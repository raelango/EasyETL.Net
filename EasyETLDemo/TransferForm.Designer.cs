namespace EasyXmlSample
{
    partial class TransferForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSource = new System.Windows.Forms.ComboBox();
            this.cmbDestination = new System.Windows.Forms.ComboBox();
            this.txtSourceFilter = new System.Windows.Forms.TextBox();
            this.txtDestinationFilter = new System.Windows.Forms.TextBox();
            this.lstSourceFileList = new System.Windows.Forms.ListBox();
            this.lstDestinationFileList = new System.Windows.Forms.ListBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.03738F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.96262F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 591F));
            this.tableLayoutPanel1.Controls.Add(this.lstDestinationFileList, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDestinationFilter, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbDestination, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbSource, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtSourceFilter, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lstSourceFileList, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnTransfer, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1127, 500);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(465, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(538, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(586, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destination";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbSource
            // 
            this.cmbSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(3, 53);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(465, 21);
            this.cmbSource.TabIndex = 2;
            this.cmbSource.SelectedIndexChanged += new System.EventHandler(this.cmbSource_SelectedIndexChanged);
            // 
            // cmbDestination
            // 
            this.cmbDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestination.FormattingEnabled = true;
            this.cmbDestination.Location = new System.Drawing.Point(538, 53);
            this.cmbDestination.Name = "cmbDestination";
            this.cmbDestination.Size = new System.Drawing.Size(586, 21);
            this.cmbDestination.TabIndex = 3;
            this.cmbDestination.SelectedIndexChanged += new System.EventHandler(this.cmbDestination_SelectedIndexChanged);
            // 
            // txtSourceFilter
            // 
            this.txtSourceFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSourceFilter.Location = new System.Drawing.Point(35, 95);
            this.txtSourceFilter.Margin = new System.Windows.Forms.Padding(35, 5, 35, 5);
            this.txtSourceFilter.Name = "txtSourceFilter";
            this.txtSourceFilter.Size = new System.Drawing.Size(401, 20);
            this.txtSourceFilter.TabIndex = 4;
            this.txtSourceFilter.Text = "*.*";
            // 
            // txtDestinationFilter
            // 
            this.txtDestinationFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDestinationFilter.Location = new System.Drawing.Point(570, 95);
            this.txtDestinationFilter.Margin = new System.Windows.Forms.Padding(35, 5, 35, 5);
            this.txtDestinationFilter.Name = "txtDestinationFilter";
            this.txtDestinationFilter.Size = new System.Drawing.Size(522, 20);
            this.txtDestinationFilter.TabIndex = 5;
            this.txtDestinationFilter.Text = "*.*";
            // 
            // lstSourceFileList
            // 
            this.lstSourceFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSourceFileList.FormattingEnabled = true;
            this.lstSourceFileList.Location = new System.Drawing.Point(3, 133);
            this.lstSourceFileList.Name = "lstSourceFileList";
            this.lstSourceFileList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSourceFileList.Size = new System.Drawing.Size(465, 323);
            this.lstSourceFileList.TabIndex = 6;
            this.lstSourceFileList.SelectedValueChanged += new System.EventHandler(this.lstSourceFileList_SelectedValueChanged);
            // 
            // lstDestinationFileList
            // 
            this.lstDestinationFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDestinationFileList.FormattingEnabled = true;
            this.lstDestinationFileList.Location = new System.Drawing.Point(538, 133);
            this.lstDestinationFileList.Name = "lstDestinationFileList";
            this.lstDestinationFileList.Size = new System.Drawing.Size(586, 323);
            this.lstDestinationFileList.TabIndex = 7;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTransfer.Enabled = false;
            this.btnTransfer.Location = new System.Drawing.Point(474, 283);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(58, 23);
            this.btnTransfer.TabIndex = 8;
            this.btnTransfer.Text = "==>";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(989, 462);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(135, 35);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save and Close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(3, 462);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(122, 35);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Cancel Changes";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // TransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1127, 500);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TransferForm";
            this.Text = "TransferForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDestinationFilter;
        private System.Windows.Forms.ComboBox cmbDestination;
        private System.Windows.Forms.ComboBox cmbSource;
        private System.Windows.Forms.TextBox txtSourceFilter;
        private System.Windows.Forms.ListBox lstDestinationFileList;
        private System.Windows.Forms.ListBox lstSourceFileList;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}