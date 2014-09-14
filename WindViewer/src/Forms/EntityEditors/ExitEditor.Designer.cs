namespace WindViewer.Forms.EntityEditors
{
    partial class ExitEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldStageName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fieldSpawnID = new System.Windows.Forms.NumericUpDown();
            this.fieldRoomID = new System.Windows.Forms.NumericUpDown();
            this.fieldFadeoutID = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldSpawnID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRoomID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldFadeoutID)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldStageName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldSpawnID, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldRoomID, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldFadeoutID, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(170, 320);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stage Name:";
            // 
            // fieldStageName
            // 
            this.fieldStageName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldStageName.Location = new System.Drawing.Point(88, 3);
            this.fieldStageName.Name = "fieldStageName";
            this.fieldStageName.Size = new System.Drawing.Size(79, 20);
            this.fieldStageName.TabIndex = 1;
            this.fieldStageName.TextChanged += new System.EventHandler(this.fieldDestName_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Spawn ID:";
            // 
            // fieldSpawnID
            // 
            this.fieldSpawnID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldSpawnID.Location = new System.Drawing.Point(88, 29);
            this.fieldSpawnID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldSpawnID.Name = "fieldSpawnID";
            this.fieldSpawnID.Size = new System.Drawing.Size(79, 20);
            this.fieldSpawnID.TabIndex = 3;
            this.fieldSpawnID.ValueChanged += new System.EventHandler(this.fieldSpawnIndex_ValueChanged);
            // 
            // fieldRoomID
            // 
            this.fieldRoomID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldRoomID.Location = new System.Drawing.Point(88, 55);
            this.fieldRoomID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldRoomID.Name = "fieldRoomID";
            this.fieldRoomID.Size = new System.Drawing.Size(79, 20);
            this.fieldRoomID.TabIndex = 4;
            this.fieldRoomID.ValueChanged += new System.EventHandler(this.fieldDestRoomIndex_ValueChanged);
            // 
            // fieldFadeoutID
            // 
            this.fieldFadeoutID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldFadeoutID.Location = new System.Drawing.Point(88, 81);
            this.fieldFadeoutID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldFadeoutID.Name = "fieldFadeoutID";
            this.fieldFadeoutID.Size = new System.Drawing.Size(79, 20);
            this.fieldFadeoutID.TabIndex = 5;
            this.fieldFadeoutID.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Room ID:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Fadeout ID:";
            // 
            // ExitEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ExitEditor";
            this.Size = new System.Drawing.Size(170, 320);
            this.Load += new System.EventHandler(this.ExitEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldSpawnID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRoomID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldFadeoutID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fieldStageName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown fieldSpawnID;
        private System.Windows.Forms.NumericUpDown fieldRoomID;
        private System.Windows.Forms.NumericUpDown fieldFadeoutID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
