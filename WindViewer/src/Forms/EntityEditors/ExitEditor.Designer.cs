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
            this.fieldDestName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fieldSpawnIndex = new System.Windows.Forms.NumericUpDown();
            this.fieldDestRoomIndex = new System.Windows.Forms.NumericUpDown();
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.fieldPadding = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldSpawnIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldDestRoomIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldDestName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldSpawnIndex, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldDestRoomIndex, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldPadding, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
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
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Destination Name:";
            // 
            // fieldDestName
            // 
            this.fieldDestName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldDestName.Location = new System.Drawing.Point(88, 3);
            this.fieldDestName.Name = "fieldDestName";
            this.fieldDestName.Size = new System.Drawing.Size(79, 20);
            this.fieldDestName.TabIndex = 1;
            this.fieldDestName.TextChanged += new System.EventHandler(this.fieldDestName_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Spawn Index:";
            // 
            // fieldSpawnIndex
            // 
            this.fieldSpawnIndex.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldSpawnIndex.Location = new System.Drawing.Point(88, 29);
            this.fieldSpawnIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldSpawnIndex.Name = "fieldSpawnIndex";
            this.fieldSpawnIndex.Size = new System.Drawing.Size(79, 20);
            this.fieldSpawnIndex.TabIndex = 3;
            this.fieldSpawnIndex.ValueChanged += new System.EventHandler(this.fieldSpawnIndex_ValueChanged);
            // 
            // fieldDestRoomIndex
            // 
            this.fieldDestRoomIndex.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldDestRoomIndex.Location = new System.Drawing.Point(88, 55);
            this.fieldDestRoomIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldDestRoomIndex.Name = "fieldDestRoomIndex";
            this.fieldDestRoomIndex.Size = new System.Drawing.Size(79, 20);
            this.fieldDestRoomIndex.TabIndex = 4;
            this.fieldDestRoomIndex.ValueChanged += new System.EventHandler(this.fieldDestRoomIndex_ValueChanged);
            // 
            // fieldUnknown1
            // 
            this.fieldUnknown1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown1.Location = new System.Drawing.Point(88, 81);
            this.fieldUnknown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldUnknown1.Name = "fieldUnknown1";
            this.fieldUnknown1.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown1.TabIndex = 5;
            this.fieldUnknown1.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // fieldPadding
            // 
            this.fieldPadding.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPadding.Location = new System.Drawing.Point(88, 107);
            this.fieldPadding.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldPadding.Name = "fieldPadding";
            this.fieldPadding.Size = new System.Drawing.Size(79, 20);
            this.fieldPadding.TabIndex = 6;
            this.fieldPadding.ValueChanged += new System.EventHandler(this.fieldPadding_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 26);
            this.label3.TabIndex = 7;
            this.label3.Text = "Destination Room Index:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Unknown1:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Padding:";
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
            ((System.ComponentModel.ISupportInitialize)(this.fieldSpawnIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldDestRoomIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fieldDestName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown fieldSpawnIndex;
        private System.Windows.Forms.NumericUpDown fieldDestRoomIndex;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
        private System.Windows.Forms.NumericUpDown fieldPadding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
