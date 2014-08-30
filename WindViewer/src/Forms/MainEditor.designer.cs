namespace WindViewer.Forms
{
    partial class MainEditor
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
            this.glControl = new OpenTK.GLControl();
            this.MainSplitter = new System.Windows.Forms.SplitContainer();
            this.leftColumGameSplit = new System.Windows.Forms.SplitContainer();
            this.EntitiesProjectSplit = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EntityTreeview = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ProjectTreeview = new System.Windows.Forms.TreeView();
            this.PropertiesLayerSplit = new System.Windows.Forms.SplitContainer();
            this.PropertiesBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LayersListBox = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.contextEntityTreeRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.actorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerSpawnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shipSpawnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exportChunksOfTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllChunksOfTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFromArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openWorldspaceDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mruList = new JWC.MruStripMenu.MruMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportArchivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.unloadWorldspaceProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floatConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automatedTestSuiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.issueTrackerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainArea = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitter)).BeginInit();
            this.MainSplitter.Panel1.SuspendLayout();
            this.MainSplitter.Panel2.SuspendLayout();
            this.MainSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftColumGameSplit)).BeginInit();
            this.leftColumGameSplit.Panel1.SuspendLayout();
            this.leftColumGameSplit.Panel2.SuspendLayout();
            this.leftColumGameSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EntitiesProjectSplit)).BeginInit();
            this.EntitiesProjectSplit.Panel1.SuspendLayout();
            this.EntitiesProjectSplit.Panel2.SuspendLayout();
            this.EntitiesProjectSplit.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesLayerSplit)).BeginInit();
            this.PropertiesLayerSplit.Panel1.SuspendLayout();
            this.PropertiesLayerSplit.Panel2.SuspendLayout();
            this.PropertiesLayerSplit.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextEntityTreeRoot.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.mainArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.glControl.BackColor = System.Drawing.Color.Transparent;
            this.glControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.glControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(965, 629);
            this.glControl.TabIndex = 1;
            this.glControl.VSync = true;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            // 
            // MainSplitter
            // 
            this.MainSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.MainSplitter.Location = new System.Drawing.Point(3, 3);
            this.MainSplitter.Name = "MainSplitter";
            // 
            // MainSplitter.Panel1
            // 
            this.MainSplitter.Panel1.Controls.Add(this.leftColumGameSplit);
            // 
            // MainSplitter.Panel2
            // 
            this.MainSplitter.Panel2.Controls.Add(this.PropertiesLayerSplit);
            this.MainSplitter.Panel2MinSize = 170;
            this.MainSplitter.Size = new System.Drawing.Size(1356, 629);
            this.MainSplitter.SplitterDistance = 1182;
            this.MainSplitter.TabIndex = 2;
            // 
            // leftColumGameSplit
            // 
            this.leftColumGameSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftColumGameSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.leftColumGameSplit.Location = new System.Drawing.Point(0, 0);
            this.leftColumGameSplit.Name = "leftColumGameSplit";
            // 
            // leftColumGameSplit.Panel1
            // 
            this.leftColumGameSplit.Panel1.Controls.Add(this.EntitiesProjectSplit);
            // 
            // leftColumGameSplit.Panel2
            // 
            this.leftColumGameSplit.Panel2.Controls.Add(this.glControl);
            this.leftColumGameSplit.Size = new System.Drawing.Size(1182, 629);
            this.leftColumGameSplit.SplitterDistance = 213;
            this.leftColumGameSplit.TabIndex = 2;
            // 
            // EntitiesProjectSplit
            // 
            this.EntitiesProjectSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntitiesProjectSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.EntitiesProjectSplit.Location = new System.Drawing.Point(0, 0);
            this.EntitiesProjectSplit.Name = "EntitiesProjectSplit";
            this.EntitiesProjectSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // EntitiesProjectSplit.Panel1
            // 
            this.EntitiesProjectSplit.Panel1.Controls.Add(this.groupBox3);
            // 
            // EntitiesProjectSplit.Panel2
            // 
            this.EntitiesProjectSplit.Panel2.Controls.Add(this.groupBox2);
            this.EntitiesProjectSplit.Size = new System.Drawing.Size(213, 629);
            this.EntitiesProjectSplit.SplitterDistance = 303;
            this.EntitiesProjectSplit.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EntityTreeview);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(213, 303);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Entities";
            // 
            // EntityTreeview
            // 
            this.EntityTreeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntityTreeview.Location = new System.Drawing.Point(3, 16);
            this.EntityTreeview.Name = "EntityTreeview";
            this.EntityTreeview.Size = new System.Drawing.Size(207, 284);
            this.EntityTreeview.TabIndex = 0;
            this.EntityTreeview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EntityTreeview_AfterSelect);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ProjectTreeview);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 322);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Project View";
            // 
            // ProjectTreeview
            // 
            this.ProjectTreeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectTreeview.Location = new System.Drawing.Point(3, 16);
            this.ProjectTreeview.Name = "ProjectTreeview";
            this.ProjectTreeview.Size = new System.Drawing.Size(207, 303);
            this.ProjectTreeview.TabIndex = 0;
            this.ProjectTreeview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ProjectTreeview_AfterSelect);
            // 
            // PropertiesLayerSplit
            // 
            this.PropertiesLayerSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesLayerSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.PropertiesLayerSplit.Location = new System.Drawing.Point(0, 0);
            this.PropertiesLayerSplit.Name = "PropertiesLayerSplit";
            this.PropertiesLayerSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // PropertiesLayerSplit.Panel1
            // 
            this.PropertiesLayerSplit.Panel1.Controls.Add(this.PropertiesBox);
            // 
            // PropertiesLayerSplit.Panel2
            // 
            this.PropertiesLayerSplit.Panel2.Controls.Add(this.groupBox1);
            this.PropertiesLayerSplit.Panel2.Controls.Add(this.panel1);
            this.PropertiesLayerSplit.Size = new System.Drawing.Size(170, 629);
            this.PropertiesLayerSplit.SplitterDistance = 361;
            this.PropertiesLayerSplit.TabIndex = 1;
            // 
            // PropertiesBox
            // 
            this.PropertiesBox.AutoSize = true;
            this.PropertiesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesBox.Location = new System.Drawing.Point(0, 0);
            this.PropertiesBox.Name = "PropertiesBox";
            this.PropertiesBox.Size = new System.Drawing.Size(170, 361);
            this.PropertiesBox.TabIndex = 0;
            this.PropertiesBox.TabStop = false;
            this.PropertiesBox.Text = "Properties";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LayersListBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 235);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Layers";
            // 
            // LayersListBox
            // 
            this.LayersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayersListBox.FormattingEnabled = true;
            this.LayersListBox.Location = new System.Drawing.Point(3, 16);
            this.LayersListBox.Name = "LayersListBox";
            this.LayersListBox.Size = new System.Drawing.Size(164, 216);
            this.LayersListBox.TabIndex = 0;
            this.LayersListBox.SelectedIndexChanged += new System.EventHandler(this.LayersListBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 235);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(170, 29);
            this.panel1.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.AutoSize = true;
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(92, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "Remove";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(3, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "Add";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // contextEntityTreeRoot
            // 
            this.contextEntityTreeRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.exportChunksOfTypeToolStripMenuItem,
            this.deleteAllChunksOfTypeToolStripMenuItem});
            this.contextEntityTreeRoot.Name = "contextMenuStrip1";
            this.contextEntityTreeRoot.Size = new System.Drawing.Size(211, 70);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actorToolStripMenuItem,
            this.playerSpawnToolStripMenuItem,
            this.shipSpawnToolStripMenuItem,
            this.toolStripSeparator5});
            this.toolStripMenuItem3.Enabled = false;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(210, 22);
            this.toolStripMenuItem3.Text = "&Add...";
            // 
            // actorToolStripMenuItem
            // 
            this.actorToolStripMenuItem.Name = "actorToolStripMenuItem";
            this.actorToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.actorToolStripMenuItem.Text = "&Actor";
            // 
            // playerSpawnToolStripMenuItem
            // 
            this.playerSpawnToolStripMenuItem.Name = "playerSpawnToolStripMenuItem";
            this.playerSpawnToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.playerSpawnToolStripMenuItem.Text = "&Player Spawn";
            // 
            // shipSpawnToolStripMenuItem
            // 
            this.shipSpawnToolStripMenuItem.Name = "shipSpawnToolStripMenuItem";
            this.shipSpawnToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.shipSpawnToolStripMenuItem.Text = "&Ship Spawn";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(141, 6);
            // 
            // exportChunksOfTypeToolStripMenuItem
            // 
            this.exportChunksOfTypeToolStripMenuItem.Name = "exportChunksOfTypeToolStripMenuItem";
            this.exportChunksOfTypeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.exportChunksOfTypeToolStripMenuItem.Text = "&Export Chunks of Type...";
            this.exportChunksOfTypeToolStripMenuItem.Click += new System.EventHandler(this.exportChunksOfTypeToolStripMenuItem_Click);
            // 
            // deleteAllChunksOfTypeToolStripMenuItem
            // 
            this.deleteAllChunksOfTypeToolStripMenuItem.Name = "deleteAllChunksOfTypeToolStripMenuItem";
            this.deleteAllChunksOfTypeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.deleteAllChunksOfTypeToolStripMenuItem.Text = "&Delete All Chunks of Type";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 658);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1362, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(281, 17);
            this.toolStripStatusLabel1.Text = "Load a project via File->Open  from Worldspace Dir!";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1362, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.newFromArchiveToolStripMenuItem,
            this.toolStripSeparator1,
            this.openWorldspaceDirToolStripMenuItem,
            this.mruList,
            this.toolStripSeparator2,
            this.saveAllToolStripMenuItem,
            this.exportArchivesToolStripMenuItem,
            this.toolStripSeparator3,
            this.unloadWorldspaceProjectToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.newToolStripMenuItem.Text = "&New...";
            // 
            // newFromArchiveToolStripMenuItem
            // 
            this.newFromArchiveToolStripMenuItem.Name = "newFromArchiveToolStripMenuItem";
            this.newFromArchiveToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.newFromArchiveToolStripMenuItem.Text = "&New from &Archive...";
            this.newFromArchiveToolStripMenuItem.Click += new System.EventHandler(this.newFromArchiveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // openWorldspaceDirToolStripMenuItem
            // 
            this.openWorldspaceDirToolStripMenuItem.Name = "openWorldspaceDirToolStripMenuItem";
            this.openWorldspaceDirToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.openWorldspaceDirToolStripMenuItem.Text = "&Open Worldspace Dir";
            this.openWorldspaceDirToolStripMenuItem.Click += new System.EventHandler(this.openWorldspaceDirToolStripMenuItem_Click);
            // 
            // mruList
            // 
            this.mruList.Filename = "";
            this.mruList.Name = "mruList";
            this.mruList.Size = new System.Drawing.Size(190, 22);
            this.mruList.Tag = "";
            this.mruList.Text = "Open &Recent Dirs...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Enabled = false;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.saveAllToolStripMenuItem.Text = "&Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // exportArchivesToolStripMenuItem
            // 
            this.exportArchivesToolStripMenuItem.Enabled = false;
            this.exportArchivesToolStripMenuItem.Name = "exportArchivesToolStripMenuItem";
            this.exportArchivesToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exportArchivesToolStripMenuItem.Text = "&Export Archives (*.arc)";
            this.exportArchivesToolStripMenuItem.Click += new System.EventHandler(this.exportArchivesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
            // 
            // unloadWorldspaceProjectToolStripMenuItem
            // 
            this.unloadWorldspaceProjectToolStripMenuItem.Enabled = false;
            this.unloadWorldspaceProjectToolStripMenuItem.Name = "unloadWorldspaceProjectToolStripMenuItem";
            this.unloadWorldspaceProjectToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.unloadWorldspaceProjectToolStripMenuItem.Text = "&Unload All";
            this.unloadWorldspaceProjectToolStripMenuItem.Click += new System.EventHandler(this.unloadWorldspaceProject_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.floatConverterToolStripMenuItem,
            this.automatedTestSuiteToolStripMenuItem,
            this.optionsToolStripMenuItem1});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // floatConverterToolStripMenuItem
            // 
            this.floatConverterToolStripMenuItem.Name = "floatConverterToolStripMenuItem";
            this.floatConverterToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.floatConverterToolStripMenuItem.Text = "&Float Converter...";
            this.floatConverterToolStripMenuItem.Click += new System.EventHandler(this.floatConverterToolStripMenuItem_Click);
            // 
            // automatedTestSuiteToolStripMenuItem
            // 
            this.automatedTestSuiteToolStripMenuItem.Name = "automatedTestSuiteToolStripMenuItem";
            this.automatedTestSuiteToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.automatedTestSuiteToolStripMenuItem.Text = "&Automated Test Suite...";
            this.automatedTestSuiteToolStripMenuItem.Click += new System.EventHandler(this.automatedTestSuiteToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            this.optionsToolStripMenuItem1.Text = "&Options...";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wikiToolStripMenuItem,
            this.issueTrackerToolStripMenuItem,
            this.toolStripSeparator4,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // wikiToolStripMenuItem
            // 
            this.wikiToolStripMenuItem.Name = "wikiToolStripMenuItem";
            this.wikiToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.wikiToolStripMenuItem.Text = "&Wiki";
            this.wikiToolStripMenuItem.Click += new System.EventHandler(this.wikiToolStripMenuItem_Click);
            // 
            // issueTrackerToolStripMenuItem
            // 
            this.issueTrackerToolStripMenuItem.Name = "issueTrackerToolStripMenuItem";
            this.issueTrackerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.issueTrackerToolStripMenuItem.Text = "&Issue Tracker";
            this.issueTrackerToolStripMenuItem.Click += new System.EventHandler(this.issueTrackerToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // mainArea
            // 
            this.mainArea.Controls.Add(this.MainSplitter);
            this.mainArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainArea.Location = new System.Drawing.Point(0, 24);
            this.mainArea.Name = "mainArea";
            this.mainArea.Padding = new System.Windows.Forms.Padding(3, 3, 3, 24);
            this.mainArea.Size = new System.Drawing.Size(1362, 656);
            this.mainArea.TabIndex = 0;
            // 
            // MainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 680);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainArea);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wind Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainEditor_Load);
            this.MainSplitter.Panel1.ResumeLayout(false);
            this.MainSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitter)).EndInit();
            this.MainSplitter.ResumeLayout(false);
            this.leftColumGameSplit.Panel1.ResumeLayout(false);
            this.leftColumGameSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftColumGameSplit)).EndInit();
            this.leftColumGameSplit.ResumeLayout(false);
            this.EntitiesProjectSplit.Panel1.ResumeLayout(false);
            this.EntitiesProjectSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EntitiesProjectSplit)).EndInit();
            this.EntitiesProjectSplit.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.PropertiesLayerSplit.Panel1.ResumeLayout(false);
            this.PropertiesLayerSplit.Panel1.PerformLayout();
            this.PropertiesLayerSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesLayerSplit)).EndInit();
            this.PropertiesLayerSplit.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextEntityTreeRoot.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainArea.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl;
        private System.Windows.Forms.SplitContainer MainSplitter;
        private System.Windows.Forms.SplitContainer leftColumGameSplit;
        private System.Windows.Forms.SplitContainer PropertiesLayerSplit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox LayersListBox;
        private System.Windows.Forms.GroupBox PropertiesBox;
        private System.Windows.Forms.SplitContainer EntitiesProjectSplit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView EntityTreeview;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView ProjectTreeview;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFromArchiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openWorldspaceDirToolStripMenuItem;
        private System.Windows.Forms.Panel mainArea;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private JWC.MruStripMenu.MruMenuItem mruList;
        private System.Windows.Forms.ToolStripMenuItem exportArchivesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unloadWorldspaceProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floatConverterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wikiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem issueTrackerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextEntityTreeRoot;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem actorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerSpawnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shipSpawnToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem exportChunksOfTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllChunksOfTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem automatedTestSuiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
    }
}