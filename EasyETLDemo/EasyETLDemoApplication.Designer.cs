namespace EasyXmlSample
{
    partial class EasyETLDemoApplication
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.tvClients = new System.Windows.Forms.TreeView();
            this.ClientContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmNewClient = new System.Windows.Forms.ToolStripMenuItem();
            this.cmClone = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRename = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.MainTablControl = new System.Windows.Forms.TabControl();
            this.ActionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.amOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.amCreateNew = new System.Windows.Forms.ToolStripMenuItem();
            this.amClone = new System.Windows.Forms.ToolStripMenuItem();
            this.amMoveTo = new System.Windows.Forms.ToolStripMenuItem();
            this.amRename = new System.Windows.Forms.ToolStripMenuItem();
            this.amDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.amClose = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionTypeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.atNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.ClientContextMenuStrip.SuspendLayout();
            this.ActionContextMenuStrip.SuspendLayout();
            this.ActionTypeContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 538);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1108, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // MainContainer
            // 
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.tvClients);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.MainTablControl);
            this.MainContainer.Size = new System.Drawing.Size(1108, 538);
            this.MainContainer.SplitterDistance = 268;
            this.MainContainer.TabIndex = 4;
            // 
            // tvClients
            // 
            this.tvClients.AllowDrop = true;
            this.tvClients.ContextMenuStrip = this.ClientContextMenuStrip;
            this.tvClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvClients.LabelEdit = true;
            this.tvClients.Location = new System.Drawing.Point(0, 0);
            this.tvClients.Name = "tvClients";
            this.tvClients.Size = new System.Drawing.Size(268, 538);
            this.tvClients.TabIndex = 0;
            this.tvClients.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvClients_BeforeLabelEdit);
            this.tvClients.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvClients_AfterLabelEdit);
            this.tvClients.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvClients_AfterSelect);
            this.tvClients.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvClients_NodeMouseClick);
            this.tvClients.DoubleClick += new System.EventHandler(this.tvClients_DoubleClick);
            this.tvClients.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvClients_KeyDown);
            // 
            // ClientContextMenuStrip
            // 
            this.ClientContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmOpen,
            this.cmNewClient,
            this.cmClone,
            this.cmRename,
            this.cmDelete});
            this.ClientContextMenuStrip.Name = "ClientContextMenuStrip";
            this.ClientContextMenuStrip.Size = new System.Drawing.Size(170, 114);
            // 
            // cmOpen
            // 
            this.cmOpen.Name = "cmOpen";
            this.cmOpen.Size = new System.Drawing.Size(169, 22);
            this.cmOpen.Text = "Open";
            this.cmOpen.Click += new System.EventHandler(this.cmOpen_Click);
            // 
            // cmNewClient
            // 
            this.cmNewClient.Name = "cmNewClient";
            this.cmNewClient.Size = new System.Drawing.Size(169, 22);
            this.cmNewClient.Text = "Create New Client";
            this.cmNewClient.Click += new System.EventHandler(this.cmNewClient_Click);
            // 
            // cmClone
            // 
            this.cmClone.Name = "cmClone";
            this.cmClone.Size = new System.Drawing.Size(169, 22);
            this.cmClone.Text = "Clone";
            this.cmClone.Click += new System.EventHandler(this.cmClone_Click);
            // 
            // cmRename
            // 
            this.cmRename.Name = "cmRename";
            this.cmRename.Size = new System.Drawing.Size(169, 22);
            this.cmRename.Text = "Rename";
            this.cmRename.Click += new System.EventHandler(this.cmRename_Click);
            // 
            // cmDelete
            // 
            this.cmDelete.Name = "cmDelete";
            this.cmDelete.Size = new System.Drawing.Size(169, 22);
            this.cmDelete.Text = "Delete";
            this.cmDelete.Click += new System.EventHandler(this.cmDelete_Click);
            // 
            // MainTablControl
            // 
            this.MainTablControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTablControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainTablControl.Location = new System.Drawing.Point(0, 0);
            this.MainTablControl.Name = "MainTablControl";
            this.MainTablControl.SelectedIndex = 0;
            this.MainTablControl.Size = new System.Drawing.Size(836, 538);
            this.MainTablControl.TabIndex = 0;
            // 
            // ActionContextMenuStrip
            // 
            this.ActionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.amOpen,
            this.amCreateNew,
            this.amClone,
            this.amMoveTo,
            this.amRename,
            this.amDelete,
            this.amClose});
            this.ActionContextMenuStrip.Name = "ActionContextMenuStrip";
            this.ActionContextMenuStrip.Size = new System.Drawing.Size(136, 158);
            // 
            // amOpen
            // 
            this.amOpen.Name = "amOpen";
            this.amOpen.Size = new System.Drawing.Size(135, 22);
            this.amOpen.Text = "Open";
            this.amOpen.Click += new System.EventHandler(this.amOpen_Click);
            // 
            // amCreateNew
            // 
            this.amCreateNew.Name = "amCreateNew";
            this.amCreateNew.Size = new System.Drawing.Size(135, 22);
            this.amCreateNew.Text = "Create New";
            this.amCreateNew.Click += new System.EventHandler(this.amCreateNew_Click);
            // 
            // amClone
            // 
            this.amClone.Name = "amClone";
            this.amClone.Size = new System.Drawing.Size(135, 22);
            this.amClone.Text = "Clone";
            this.amClone.Click += new System.EventHandler(this.amClone_Click);
            // 
            // amMoveTo
            // 
            this.amMoveTo.Name = "amMoveTo";
            this.amMoveTo.Size = new System.Drawing.Size(135, 22);
            this.amMoveTo.Text = "Move to ...";
            this.amMoveTo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.amMoveTo_DropDownItemClicked);
            // 
            // amRename
            // 
            this.amRename.Name = "amRename";
            this.amRename.Size = new System.Drawing.Size(135, 22);
            this.amRename.Text = "Rename";
            this.amRename.Click += new System.EventHandler(this.amRename_Click);
            // 
            // amDelete
            // 
            this.amDelete.Name = "amDelete";
            this.amDelete.Size = new System.Drawing.Size(135, 22);
            this.amDelete.Text = "Delete";
            this.amDelete.Click += new System.EventHandler(this.amDelete_Click);
            // 
            // amClose
            // 
            this.amClose.Name = "amClose";
            this.amClose.Size = new System.Drawing.Size(135, 22);
            this.amClose.Text = "Close";
            this.amClose.Click += new System.EventHandler(this.amClose_Click);
            // 
            // ActionTypeContextMenuStrip
            // 
            this.ActionTypeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.atNewToolStripMenuItem});
            this.ActionTypeContextMenuStrip.Name = "contextMenuStrip1";
            this.ActionTypeContextMenuStrip.Size = new System.Drawing.Size(181, 48);
            // 
            // atNewToolStripMenuItem
            // 
            this.atNewToolStripMenuItem.Name = "atNewToolStripMenuItem";
            this.atNewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.atNewToolStripMenuItem.Text = "Create New";
            this.atNewToolStripMenuItem.Click += new System.EventHandler(this.atNewToolStripMenuItem_Click);
            // 
            // EasyETLDemoApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 560);
            this.Controls.Add(this.MainContainer);
            this.Controls.Add(this.statusStrip);
            this.IsMdiContainer = true;
            this.Name = "EasyETLDemoApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EasyETLApplication - Demonstrate the ETL capabilities of easy components";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EasyETLDemoApplication_FormClosing);
            this.Load += new System.EventHandler(this.EasyETLDemoApplication_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.ClientContextMenuStrip.ResumeLayout(false);
            this.ActionContextMenuStrip.ResumeLayout(false);
            this.ActionTypeContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer MainContainer;
        private System.Windows.Forms.TreeView tvClients;
        private System.Windows.Forms.TabControl MainTablControl;
        private System.Windows.Forms.ContextMenuStrip ClientContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cmNewClient;
        private System.Windows.Forms.ContextMenuStrip ActionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem amOpen;
        private System.Windows.Forms.ToolStripMenuItem amCreateNew;
        private System.Windows.Forms.ToolStripMenuItem amClone;
        private System.Windows.Forms.ToolStripMenuItem amMoveTo;
        private System.Windows.Forms.ToolStripMenuItem amRename;
        private System.Windows.Forms.ToolStripMenuItem amDelete;
        private System.Windows.Forms.ToolStripMenuItem amClose;
        private System.Windows.Forms.ToolStripMenuItem cmOpen;
        private System.Windows.Forms.ToolStripMenuItem cmClone;
        private System.Windows.Forms.ToolStripMenuItem cmRename;
        private System.Windows.Forms.ToolStripMenuItem cmDelete;
        private System.Windows.Forms.ContextMenuStrip ActionTypeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem atNewToolStripMenuItem;
    }
}



