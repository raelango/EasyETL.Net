namespace EasyXmlSample
{
    partial class ClassConfigurationForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveAction = new System.Windows.Forms.Button();
            this.btnCloseWindow = new System.Windows.Forms.Button();
            this.txtActionName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblProperties = new System.Windows.Forms.Label();
            this.lblLibraryName = new System.Windows.Forms.Label();
            this.pnlField = new System.Windows.Forms.Panel();
            this.btnSaveField = new System.Windows.Forms.Button();
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.lblField = new System.Windows.Forms.Label();
            this.lblFieldDescription = new System.Windows.Forms.Label();
            this.lstFields = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblClassDescription = new System.Windows.Forms.Label();
            this.cmbClassName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlField.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(591, 485);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveAction);
            this.panel1.Controls.Add(this.btnCloseWindow);
            this.panel1.Controls.Add(this.txtActionName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 34);
            this.panel1.TabIndex = 0;
            // 
            // btnSaveAction
            // 
            this.btnSaveAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAction.Location = new System.Drawing.Point(370, 0);
            this.btnSaveAction.Name = "btnSaveAction";
            this.btnSaveAction.Size = new System.Drawing.Size(96, 34);
            this.btnSaveAction.TabIndex = 3;
            this.btnSaveAction.Text = "Save Action";
            this.btnSaveAction.UseVisualStyleBackColor = true;
            this.btnSaveAction.Click += new System.EventHandler(this.btnSaveAction_Click);
            // 
            // btnCloseWindow
            // 
            this.btnCloseWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseWindow.Location = new System.Drawing.Point(472, 0);
            this.btnCloseWindow.Name = "btnCloseWindow";
            this.btnCloseWindow.Size = new System.Drawing.Size(113, 34);
            this.btnCloseWindow.TabIndex = 2;
            this.btnCloseWindow.Text = "Close Window";
            this.btnCloseWindow.UseVisualStyleBackColor = true;
            this.btnCloseWindow.Click += new System.EventHandler(this.btnCloseWindow_Click);
            // 
            // txtActionName
            // 
            this.txtActionName.Location = new System.Drawing.Point(105, 9);
            this.txtActionName.Name = "txtActionName";
            this.txtActionName.ReadOnly = true;
            this.txtActionName.Size = new System.Drawing.Size(249, 20);
            this.txtActionName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Action Name:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblProperties);
            this.panel2.Controls.Add(this.lblLibraryName);
            this.panel2.Controls.Add(this.pnlField);
            this.panel2.Controls.Add(this.lblFieldDescription);
            this.panel2.Controls.Add(this.lstFields);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lblClassDescription);
            this.panel2.Controls.Add(this.cmbClassName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(588, 439);
            this.panel2.TabIndex = 1;
            // 
            // lblProperties
            // 
            this.lblProperties.Location = new System.Drawing.Point(367, 294);
            this.lblProperties.Name = "lblProperties";
            this.lblProperties.Size = new System.Drawing.Size(218, 83);
            this.lblProperties.TabIndex = 10;
            // 
            // lblLibraryName
            // 
            this.lblLibraryName.AutoSize = true;
            this.lblLibraryName.Location = new System.Drawing.Point(105, 88);
            this.lblLibraryName.Name = "lblLibraryName";
            this.lblLibraryName.Size = new System.Drawing.Size(38, 13);
            this.lblLibraryName.TabIndex = 9;
            this.lblLibraryName.Text = "Library";
            // 
            // pnlField
            // 
            this.pnlField.Controls.Add(this.btnSaveField);
            this.pnlField.Controls.Add(this.cmbField);
            this.pnlField.Controls.Add(this.lblField);
            this.pnlField.Location = new System.Drawing.Point(370, 131);
            this.pnlField.Name = "pnlField";
            this.pnlField.Size = new System.Drawing.Size(215, 160);
            this.pnlField.TabIndex = 8;
            // 
            // btnSaveField
            // 
            this.btnSaveField.Location = new System.Drawing.Point(81, 86);
            this.btnSaveField.Name = "btnSaveField";
            this.btnSaveField.Size = new System.Drawing.Size(75, 23);
            this.btnSaveField.TabIndex = 2;
            this.btnSaveField.Text = "Save Field Value";
            this.btnSaveField.UseVisualStyleBackColor = true;
            this.btnSaveField.Click += new System.EventHandler(this.btnSaveField_Click);
            // 
            // cmbField
            // 
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Location = new System.Drawing.Point(16, 29);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(190, 21);
            this.cmbField.TabIndex = 1;
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            this.lblField.Location = new System.Drawing.Point(13, 12);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(37, 13);
            this.lblField.TabIndex = 0;
            this.lblField.Text = "Value:";
            // 
            // lblFieldDescription
            // 
            this.lblFieldDescription.Location = new System.Drawing.Point(108, 294);
            this.lblFieldDescription.Name = "lblFieldDescription";
            this.lblFieldDescription.Size = new System.Drawing.Size(246, 83);
            this.lblFieldDescription.TabIndex = 7;
            // 
            // lstFields
            // 
            this.lstFields.FormattingEnabled = true;
            this.lstFields.Location = new System.Drawing.Point(108, 131);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(246, 160);
            this.lstFields.Sorted = true;
            this.lstFields.TabIndex = 6;
            this.lstFields.SelectedIndexChanged += new System.EventHandler(this.lstFields_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Fields:";
            // 
            // lblClassDescription
            // 
            this.lblClassDescription.AutoSize = true;
            this.lblClassDescription.Location = new System.Drawing.Point(105, 45);
            this.lblClassDescription.Name = "lblClassDescription";
            this.lblClassDescription.Size = new System.Drawing.Size(32, 13);
            this.lblClassDescription.TabIndex = 4;
            this.lblClassDescription.Text = "Class";
            // 
            // cmbClassName
            // 
            this.cmbClassName.FormattingEnabled = true;
            this.cmbClassName.Location = new System.Drawing.Point(108, 21);
            this.cmbClassName.Name = "cmbClassName";
            this.cmbClassName.Size = new System.Drawing.Size(246, 21);
            this.cmbClassName.Sorted = true;
            this.cmbClassName.TabIndex = 3;
            this.cmbClassName.SelectedIndexChanged += new System.EventHandler(this.cmbClassName_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Class Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Library Name:";
            // 
            // ClassConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 485);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ClassConfigurationForm";
            this.Text = "ActionForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlField.ResumeLayout(false);
            this.pnlField.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtActionName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCloseWindow;
        private System.Windows.Forms.Button btnSaveAction;
        private System.Windows.Forms.ComboBox cmbClassName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblClassDescription;
        private System.Windows.Forms.ListBox lstFields;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblFieldDescription;
        private System.Windows.Forms.Panel pnlField;
        private System.Windows.Forms.Button btnSaveField;
        private System.Windows.Forms.ComboBox cmbField;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.Label lblLibraryName;
        private System.Windows.Forms.Label lblProperties;
    }
}