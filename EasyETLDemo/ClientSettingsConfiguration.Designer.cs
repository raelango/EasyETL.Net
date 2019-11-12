namespace EasyXmlSample
{
    partial class ClientSettingsConfiguration
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
            this.lstAvailableFields = new System.Windows.Forms.ListBox();
            this.dgConfiguration = new System.Windows.Forms.DataGridView();
            this.dgcFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcDefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcClientSpecificValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgConfiguration)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lstAvailableFields, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgConfiguration, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(594, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client Specific Configuration:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(603, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "Available Configuration Fields";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstAvailableFields
            // 
            this.lstAvailableFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAvailableFields.FormattingEnabled = true;
            this.lstAvailableFields.Location = new System.Drawing.Point(603, 83);
            this.lstAvailableFields.Name = "lstAvailableFields";
            this.lstAvailableFields.Size = new System.Drawing.Size(194, 364);
            this.lstAvailableFields.TabIndex = 2;
            this.lstAvailableFields.DoubleClick += new System.EventHandler(this.lstAvailableFields_DoubleClick);
            // 
            // dgConfiguration
            // 
            this.dgConfiguration.AllowUserToAddRows = false;
            this.dgConfiguration.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgConfiguration.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcFieldName,
            this.dgcDefaultValue,
            this.dgcClientSpecificValue});
            this.dgConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgConfiguration.Location = new System.Drawing.Point(3, 83);
            this.dgConfiguration.Name = "dgConfiguration";
            this.dgConfiguration.Size = new System.Drawing.Size(594, 364);
            this.dgConfiguration.TabIndex = 3;
            this.dgConfiguration.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgConfiguration_UserDeletedRow);
            // 
            // dgcFieldName
            // 
            this.dgcFieldName.HeaderText = "Field Name";
            this.dgcFieldName.Name = "dgcFieldName";
            this.dgcFieldName.ReadOnly = true;
            // 
            // dgcDefaultValue
            // 
            this.dgcDefaultValue.HeaderText = "Default Value";
            this.dgcDefaultValue.Name = "dgcDefaultValue";
            this.dgcDefaultValue.ReadOnly = true;
            // 
            // dgcClientSpecificValue
            // 
            this.dgcClientSpecificValue.HeaderText = "Client Specific Value";
            this.dgcClientSpecificValue.Name = "dgcClientSpecificValue";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(633, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(164, 44);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 41);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(85, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 41);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ClientSettingsConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientSettingsConfiguration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ClientSettingsConfiguration";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgConfiguration)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstAvailableFields;
        private System.Windows.Forms.DataGridView dgConfiguration;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcDefaultValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcClientSpecificValue;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}