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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("[SCOOB] Scaleable Objects");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("[0] LinkRM");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("[SCLS] Exits", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("[0] KNOB00D");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("[TGDR] Door", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("model.bdl");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("model1.bdl");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("model3.bdl");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("bdl", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("model1.btk");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("btk", new System.Windows.Forms.TreeNode[] {
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("room.dzb");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("dzb", new System.Windows.Forms.TreeNode[] {
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Room44", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode11,
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Outset", new System.Windows.Forms.TreeNode[] {
            treeNode14});
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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.mainArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(1018, 632);
            this.glControl.TabIndex = 1;
            this.glControl.VSync = true;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyDown);
            this.glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyUp);
            // 
            // MainSplitter
            // 
            this.MainSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.MainSplitter.Location = new System.Drawing.Point(0, 0);
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
            this.MainSplitter.Size = new System.Drawing.Size(1409, 632);
            this.MainSplitter.SplitterDistance = 1235;
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
            this.leftColumGameSplit.Size = new System.Drawing.Size(1235, 632);
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
            this.EntitiesProjectSplit.Size = new System.Drawing.Size(213, 632);
            this.EntitiesProjectSplit.SplitterDistance = 306;
            this.EntitiesProjectSplit.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EntityTreeview);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(213, 306);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Entities";
            // 
            // EntityTreeview
            // 
            this.EntityTreeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntityTreeview.Location = new System.Drawing.Point(3, 16);
            this.EntityTreeview.Name = "EntityTreeview";
            treeNode1.Name = "Node0";
            treeNode1.Text = "[SCOOB] Scaleable Objects";
            treeNode2.Name = "Node4";
            treeNode2.Text = "[0] LinkRM";
            treeNode3.Name = "Node1";
            treeNode3.Text = "[SCLS] Exits";
            treeNode4.Name = "Node3";
            treeNode4.Text = "[0] KNOB00D";
            treeNode5.Name = "Node2";
            treeNode5.Text = "[TGDR] Door";
            this.EntityTreeview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode5});
            this.EntityTreeview.Size = new System.Drawing.Size(207, 287);
            this.EntityTreeview.TabIndex = 0;
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
            treeNode6.Name = "Node3";
            treeNode6.Text = "model.bdl";
            treeNode7.Name = "Node4";
            treeNode7.Text = "model1.bdl";
            treeNode8.Name = "Node5";
            treeNode8.Text = "model3.bdl";
            treeNode9.Name = "Node2";
            treeNode9.Text = "bdl";
            treeNode10.Name = "Node7";
            treeNode10.Text = "model1.btk";
            treeNode11.Name = "Node6";
            treeNode11.Text = "btk";
            treeNode12.Name = "Node9";
            treeNode12.Text = "room.dzb";
            treeNode13.Name = "Node8";
            treeNode13.Text = "dzb";
            treeNode14.Name = "Node1";
            treeNode14.Text = "Room44";
            treeNode15.Name = "Node0";
            treeNode15.Text = "Outset";
            this.ProjectTreeview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode15});
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
            this.PropertiesLayerSplit.Size = new System.Drawing.Size(170, 632);
            this.PropertiesLayerSplit.SplitterDistance = 364;
            this.PropertiesLayerSplit.TabIndex = 1;
            // 
            // PropertiesBox
            // 
            this.PropertiesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesBox.Location = new System.Drawing.Point(0, 0);
            this.PropertiesBox.Name = "PropertiesBox";
            this.PropertiesBox.Size = new System.Drawing.Size(170, 364);
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
            this.LayersListBox.Items.AddRange(new object[] {
            "Layer 0",
            "Layer 1",
            "Layer 2",
            "Layer 3",
            "Layer 4",
            "Layer 5",
            "Layer 6",
            "Layer 7",
            "Layer 8",
            "Layer 9",
            "Layer A",
            "Layer B",
            "Layer C",
            "Layer D",
            "Layer E",
            "Layer F"});
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 658);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1409, 22);
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
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1409, 24);
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
            this.toolStripMenuItem1,
            this.toolStripSeparator3,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.newToolStripMenuItem.Text = "&New...";
            // 
            // newFromArchiveToolStripMenuItem
            // 
            this.newFromArchiveToolStripMenuItem.Enabled = false;
            this.newFromArchiveToolStripMenuItem.Name = "newFromArchiveToolStripMenuItem";
            this.newFromArchiveToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.newFromArchiveToolStripMenuItem.Text = "&New from &Archive...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // openWorldspaceDirToolStripMenuItem
            // 
            this.openWorldspaceDirToolStripMenuItem.Name = "openWorldspaceDirToolStripMenuItem";
            this.openWorldspaceDirToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openWorldspaceDirToolStripMenuItem.Text = "&Open Worldspace Dir";
            this.openWorldspaceDirToolStripMenuItem.Click += new System.EventHandler(this.openWorldspaceDirToolStripMenuItem_Click);
            // 
            // mruList
            // 
            this.mruList.Filename = "";
            this.mruList.Name = "mruList";
            this.mruList.Size = new System.Drawing.Size(194, 22);
            this.mruList.Tag = "";
            this.mruList.Text = "&Recent Dirs...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(191, 6);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Enabled = false;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.saveAllToolStripMenuItem.Text = "&Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.toolStripMenuItem1.Text = "&Export Archives (*.rarc)";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(194, 22);
            this.toolStripMenuItem2.Text = "&Unload All";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mainArea
            // 
            this.mainArea.Controls.Add(this.MainSplitter);
            this.mainArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainArea.Location = new System.Drawing.Point(0, 24);
            this.mainArea.Name = "mainArea";
            this.mainArea.Padding = new System.Windows.Forms.Padding(0, 0, 0, 24);
            this.mainArea.Size = new System.Drawing.Size(1409, 656);
            this.mainArea.TabIndex = 0;
            // 
            // MainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1409, 680);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainArea);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainEditor";
            this.Text = "Wind Editor";
            this.Load += new System.EventHandler(this.TestLayout_Load);
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
            this.PropertiesLayerSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesLayerSplit)).EndInit();
            this.PropertiesLayerSplit.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}