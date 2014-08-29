namespace WindViewer.Editor
{
    partial class SongEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.note1Direc = new System.Windows.Forms.ComboBox();
            this.note2Direc = new System.Windows.Forms.ComboBox();
            this.note3Direc = new System.Windows.Forms.ComboBox();
            this.note4Direc = new System.Windows.Forms.ComboBox();
            this.note5Direc = new System.Windows.Forms.ComboBox();
            this.note6Direc = new System.Windows.Forms.ComboBox();
            this.songSelector = new System.Windows.Forms.ComboBox();
            this.noteCountSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.note6Box = new System.Windows.Forms.PictureBox();
            this.note5Box = new System.Windows.Forms.PictureBox();
            this.note4Box = new System.Windows.Forms.PictureBox();
            this.note3Box = new System.Windows.Forms.PictureBox();
            this.note2Box = new System.Windows.Forms.PictureBox();
            this.note1Box = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.note6Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.note5Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.note4Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.note3Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.note2Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.note1Box)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(417, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.openToolStripMenuItem.Text = "File...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // note1Direc
            // 
            this.note1Direc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.note1Direc.FormattingEnabled = true;
            this.note1Direc.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.note1Direc.Location = new System.Drawing.Point(12, 129);
            this.note1Direc.Name = "note1Direc";
            this.note1Direc.Size = new System.Drawing.Size(60, 21);
            this.note1Direc.TabIndex = 7;
            this.note1Direc.SelectedIndexChanged += new System.EventHandler(this.note1Direc_SelectedIndexChanged);
            // 
            // note2Direc
            // 
            this.note2Direc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.note2Direc.FormattingEnabled = true;
            this.note2Direc.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.note2Direc.Location = new System.Drawing.Point(78, 129);
            this.note2Direc.Name = "note2Direc";
            this.note2Direc.Size = new System.Drawing.Size(60, 21);
            this.note2Direc.TabIndex = 8;
            this.note2Direc.SelectedIndexChanged += new System.EventHandler(this.note2Direc_SelectedIndexChanged);
            // 
            // note3Direc
            // 
            this.note3Direc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.note3Direc.FormattingEnabled = true;
            this.note3Direc.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.note3Direc.Location = new System.Drawing.Point(144, 129);
            this.note3Direc.Name = "note3Direc";
            this.note3Direc.Size = new System.Drawing.Size(60, 21);
            this.note3Direc.TabIndex = 9;
            this.note3Direc.SelectedIndexChanged += new System.EventHandler(this.note3Direc_SelectedIndexChanged);
            // 
            // note4Direc
            // 
            this.note4Direc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.note4Direc.FormattingEnabled = true;
            this.note4Direc.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "Empty"});
            this.note4Direc.Location = new System.Drawing.Point(210, 129);
            this.note4Direc.Name = "note4Direc";
            this.note4Direc.Size = new System.Drawing.Size(60, 21);
            this.note4Direc.TabIndex = 10;
            this.note4Direc.SelectedIndexChanged += new System.EventHandler(this.note4Direc_SelectedIndexChanged);
            // 
            // note5Direc
            // 
            this.note5Direc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.note5Direc.FormattingEnabled = true;
            this.note5Direc.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "Empty"});
            this.note5Direc.Location = new System.Drawing.Point(276, 129);
            this.note5Direc.Name = "note5Direc";
            this.note5Direc.Size = new System.Drawing.Size(60, 21);
            this.note5Direc.TabIndex = 11;
            this.note5Direc.SelectedIndexChanged += new System.EventHandler(this.note5Direc_SelectedIndexChanged);
            // 
            // note6Direc
            // 
            this.note6Direc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.note6Direc.FormattingEnabled = true;
            this.note6Direc.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "Empty"});
            this.note6Direc.Location = new System.Drawing.Point(342, 129);
            this.note6Direc.Name = "note6Direc";
            this.note6Direc.Size = new System.Drawing.Size(60, 21);
            this.note6Direc.TabIndex = 12;
            this.note6Direc.SelectedIndexChanged += new System.EventHandler(this.note6Direc_SelectedIndexChanged);
            // 
            // songSelector
            // 
            this.songSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.songSelector.FormattingEnabled = true;
            this.songSelector.Location = new System.Drawing.Point(12, 36);
            this.songSelector.Name = "songSelector";
            this.songSelector.Size = new System.Drawing.Size(80, 21);
            this.songSelector.TabIndex = 13;
            this.songSelector.SelectedIndexChanged += new System.EventHandler(this.songSelector_SelectedIndexChanged);
            // 
            // noteCountSelector
            // 
            this.noteCountSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.noteCountSelector.FormattingEnabled = true;
            this.noteCountSelector.Items.AddRange(new object[] {
            "3",
            "4",
            "6"});
            this.noteCountSelector.Location = new System.Drawing.Point(170, 36);
            this.noteCountSelector.Name = "noteCountSelector";
            this.noteCountSelector.Size = new System.Drawing.Size(80, 21);
            this.noteCountSelector.TabIndex = 14;
            this.noteCountSelector.SelectedIndexChanged += new System.EventHandler(this.noteCountSelector_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Note Count:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Start.dol";
            // 
            // note6Box
            // 
            this.note6Box.Location = new System.Drawing.Point(342, 63);
            this.note6Box.Name = "note6Box";
            this.note6Box.Size = new System.Drawing.Size(60, 60);
            this.note6Box.TabIndex = 6;
            this.note6Box.TabStop = false;
            // 
            // note5Box
            // 
            this.note5Box.Location = new System.Drawing.Point(276, 63);
            this.note5Box.Name = "note5Box";
            this.note5Box.Size = new System.Drawing.Size(60, 60);
            this.note5Box.TabIndex = 5;
            this.note5Box.TabStop = false;
            // 
            // note4Box
            // 
            this.note4Box.Location = new System.Drawing.Point(210, 63);
            this.note4Box.Name = "note4Box";
            this.note4Box.Size = new System.Drawing.Size(60, 60);
            this.note4Box.TabIndex = 4;
            this.note4Box.TabStop = false;
            // 
            // note3Box
            // 
            this.note3Box.Location = new System.Drawing.Point(144, 63);
            this.note3Box.Name = "note3Box";
            this.note3Box.Size = new System.Drawing.Size(60, 60);
            this.note3Box.TabIndex = 3;
            this.note3Box.TabStop = false;
            // 
            // note2Box
            // 
            this.note2Box.Location = new System.Drawing.Point(78, 63);
            this.note2Box.Name = "note2Box";
            this.note2Box.Size = new System.Drawing.Size(60, 60);
            this.note2Box.TabIndex = 2;
            this.note2Box.TabStop = false;
            // 
            // note1Box
            // 
            this.note1Box.Location = new System.Drawing.Point(12, 63);
            this.note1Box.Name = "note1Box";
            this.note1Box.Size = new System.Drawing.Size(60, 60);
            this.note1Box.TabIndex = 1;
            this.note1Box.TabStop = false;
            // 
            // SongEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 164);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.noteCountSelector);
            this.Controls.Add(this.songSelector);
            this.Controls.Add(this.note6Direc);
            this.Controls.Add(this.note5Direc);
            this.Controls.Add(this.note4Direc);
            this.Controls.Add(this.note3Direc);
            this.Controls.Add(this.note2Direc);
            this.Controls.Add(this.note1Direc);
            this.Controls.Add(this.note6Box);
            this.Controls.Add(this.note5Box);
            this.Controls.Add(this.note4Box);
            this.Controls.Add(this.note3Box);
            this.Controls.Add(this.note2Box);
            this.Controls.Add(this.note1Box);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SongEditor";
            this.Text = "Song Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.note6Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.note5Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.note4Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.note3Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.note2Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.note1Box)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.PictureBox note1Box;
        private System.Windows.Forms.PictureBox note2Box;
        private System.Windows.Forms.PictureBox note3Box;
        private System.Windows.Forms.PictureBox note4Box;
        private System.Windows.Forms.PictureBox note5Box;
        private System.Windows.Forms.PictureBox note6Box;
        private System.Windows.Forms.ComboBox note1Direc;
        private System.Windows.Forms.ComboBox note2Direc;
        private System.Windows.Forms.ComboBox note3Direc;
        private System.Windows.Forms.ComboBox note4Direc;
        private System.Windows.Forms.ComboBox note5Direc;
        private System.Windows.Forms.ComboBox note6Direc;
        private System.Windows.Forms.ComboBox songSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.ComboBox noteCountSelector;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}

