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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyETLDemoApplication));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.MoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.CloneToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MoveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RenameToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.CloseToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.ClientContextMenuStrip.SuspendLayout();
            this.ActionContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.viewMenu,
            this.toolsMenu,
            this.helpMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1108, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.CloneToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator5,
            this.MoveToolStripMenuItem,
            this.renameStripMenuItem,
            this.DeleteToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.CloseToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.ShowNewForm);
            // 
            // CloneToolStripMenuItem
            // 
            this.CloneToolStripMenuItem.Name = "CloneToolStripMenuItem";
            this.CloneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.CloneToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.CloneToolStripMenuItem.Text = "Clone";
            this.CloneToolStripMenuItem.Click += new System.EventHandler(this.CloneToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenFile);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(160, 6);
            // 
            // MoveToolStripMenuItem
            // 
            this.MoveToolStripMenuItem.Name = "MoveToolStripMenuItem";
            this.MoveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.MoveToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.MoveToolStripMenuItem.Text = "Move";
            this.MoveToolStripMenuItem.Click += new System.EventHandler(this.MoveToolStripMenuItem_Click);
            // 
            // renameStripMenuItem
            // 
            this.renameStripMenuItem.Name = "renameStripMenuItem";
            this.renameStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F2)));
            this.renameStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.renameStripMenuItem.Text = "Rename";
            this.renameStripMenuItem.Click += new System.EventHandler(this.renameStripMenuItem_Click);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.DeleteToolStripMenuItem.Text = "Delete";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.CloseToolStripMenuItem.Text = "&Close";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(160, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolsStripMenuItem_Click);
            // 
            // viewMenu
            // 
            this.viewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBarToolStripMenuItem,
            this.statusBarToolStripMenuItem});
            this.viewMenu.Name = "viewMenu";
            this.viewMenu.Size = new System.Drawing.Size(44, 20);
            this.viewMenu.Text = "&View";
            // 
            // toolBarToolStripMenuItem
            // 
            this.toolBarToolStripMenuItem.Checked = true;
            this.toolBarToolStripMenuItem.CheckOnClick = true;
            this.toolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolBarToolStripMenuItem.Name = "toolBarToolStripMenuItem";
            this.toolBarToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.toolBarToolStripMenuItem.Text = "&Toolbar";
            this.toolBarToolStripMenuItem.Click += new System.EventHandler(this.ToolBarToolStripMenuItem_Click);
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.Checked = true;
            this.statusBarToolStripMenuItem.CheckOnClick = true;
            this.statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.statusBarToolStripMenuItem.Text = "&Status Bar";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.StatusBarToolStripMenuItem_Click);
            // 
            // toolsMenu
            // 
            this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsMenu.Name = "toolsMenu";
            this.toolsMenu.Size = new System.Drawing.Size(46, 20);
            this.toolsMenu.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator8,
            this.aboutToolStripMenuItem});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(44, 20);
            this.helpMenu.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("indexToolStripMenuItem.Image")));
            this.indexToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("searchToolStripMenuItem.Image")));
            this.searchToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(165, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.aboutToolStripMenuItem.Text = "&About ... ...";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.CloneToolStripButton,
            this.openToolStripButton,
            this.toolStripSeparator3,
            this.MoveToolStripButton,
            this.RenameToolStripButton,
            this.DeleteToolStripButton,
            this.saveToolStripButton,
            this.CloseToolStripButton,
            this.toolStripSeparator1,
            this.ExitToolStripButton,
            this.toolStripSeparator2});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1108, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "New";
            this.newToolStripButton.Click += new System.EventHandler(this.ShowNewForm);
            // 
            // CloneToolStripButton
            // 
            this.CloneToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CloneToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("CloneToolStripButton.Image")));
            this.CloneToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CloneToolStripButton.Name = "CloneToolStripButton";
            this.CloneToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.CloneToolStripButton.Text = "Clone this Node";
            this.CloneToolStripButton.Click += new System.EventHandler(this.CloneToolStripButton_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "Open";
            this.openToolStripButton.Click += new System.EventHandler(this.OpenFile);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // MoveToolStripButton
            // 
            this.MoveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveToolStripButton.Image")));
            this.MoveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveToolStripButton.Name = "MoveToolStripButton";
            this.MoveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.MoveToolStripButton.Text = "Move to Another client";
            this.MoveToolStripButton.Click += new System.EventHandler(this.MoveToolStripButton_Click);
            // 
            // RenameToolStripButton
            // 
            this.RenameToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RenameToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RenameToolStripButton.Image")));
            this.RenameToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RenameToolStripButton.Name = "RenameToolStripButton";
            this.RenameToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.RenameToolStripButton.Text = "<Rename>";
            this.RenameToolStripButton.Click += new System.EventHandler(this.RenameToolStripButton_Click);
            // 
            // DeleteToolStripButton
            // 
            this.DeleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteToolStripButton.Image")));
            this.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteToolStripButton.Name = "DeleteToolStripButton";
            this.DeleteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteToolStripButton.Text = "Delete";
            this.DeleteToolStripButton.Click += new System.EventHandler(this.DeleteToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // CloseToolStripButton
            // 
            this.CloseToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CloseToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseToolStripButton.Image")));
            this.CloseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CloseToolStripButton.Name = "CloseToolStripButton";
            this.CloseToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.CloseToolStripButton.ToolTipText = "Close Window";
            this.CloseToolStripButton.Click += new System.EventHandler(this.CloseToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ExitToolStripButton
            // 
            this.ExitToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExitToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ExitToolStripButton.Image")));
            this.ExitToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExitToolStripButton.Name = "ExitToolStripButton";
            this.ExitToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.ExitToolStripButton.Text = "Quit Application";
            this.ExitToolStripButton.Click += new System.EventHandler(this.ExitToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            this.MainContainer.Location = new System.Drawing.Point(0, 49);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.tvClients);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.MainTablControl);
            this.MainContainer.Size = new System.Drawing.Size(1108, 489);
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
            this.tvClients.Size = new System.Drawing.Size(268, 489);
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
            this.MainTablControl.Size = new System.Drawing.Size(836, 489);
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
            this.ActionContextMenuStrip.Size = new System.Drawing.Size(181, 180);
            // 
            // amOpen
            // 
            this.amOpen.Name = "amOpen";
            this.amOpen.Size = new System.Drawing.Size(180, 22);
            this.amOpen.Text = "Open";
            this.amOpen.Click += new System.EventHandler(this.amOpen_Click);
            // 
            // amCreateNew
            // 
            this.amCreateNew.Name = "amCreateNew";
            this.amCreateNew.Size = new System.Drawing.Size(180, 22);
            this.amCreateNew.Text = "Create New";
            this.amCreateNew.Click += new System.EventHandler(this.amCreateNew_Click);
            // 
            // amClone
            // 
            this.amClone.Name = "amClone";
            this.amClone.Size = new System.Drawing.Size(180, 22);
            this.amClone.Text = "Clone";
            this.amClone.Click += new System.EventHandler(this.amClone_Click);
            // 
            // amMoveTo
            // 
            this.amMoveTo.Name = "amMoveTo";
            this.amMoveTo.Size = new System.Drawing.Size(180, 22);
            this.amMoveTo.Text = "Move to ...";
            this.amMoveTo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.amMoveTo_DropDownItemClicked);
            // 
            // amRename
            // 
            this.amRename.Name = "amRename";
            this.amRename.Size = new System.Drawing.Size(180, 22);
            this.amRename.Text = "Rename";
            this.amRename.Click += new System.EventHandler(this.amRename_Click);
            // 
            // amDelete
            // 
            this.amDelete.Name = "amDelete";
            this.amDelete.Size = new System.Drawing.Size(180, 22);
            this.amDelete.Text = "Delete";
            this.amDelete.Click += new System.EventHandler(this.amDelete_Click);
            // 
            // amClose
            // 
            this.amClose.Name = "amClose";
            this.amClose.Size = new System.Drawing.Size(180, 22);
            this.amClose.Text = "Close";
            this.amClose.Click += new System.EventHandler(this.amClose_Click);
            // 
            // EasyETLDemoApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 560);
            this.Controls.Add(this.MainContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "EasyETLDemoApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EasyETLApplication - Demonstrate the ETL capabilities of easy components";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.ClientContextMenuStrip.ResumeLayout(false);
            this.ActionContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenu;
        private System.Windows.Forms.ToolStripMenuItem toolBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsMenu;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer MainContainer;
        private System.Windows.Forms.TreeView tvClients;
        private System.Windows.Forms.TabControl MainTablControl;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem renameStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton RenameToolStripButton;
        private System.Windows.Forms.ToolStripButton DeleteToolStripButton;
        private System.Windows.Forms.ToolStripButton CloseToolStripButton;
        private System.Windows.Forms.ToolStripButton ExitToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem MoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton MoveToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem CloneToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton CloneToolStripButton;
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
    }
}



