namespace WindViewer.Forms.EntityEditors
{
    partial class PathEditor
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldNumPoints = new System.Windows.Forms.NumericUpDown();
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.fieldUnknown2 = new System.Windows.Forms.NumericUpDown();
            this.fieldUnknown3 = new System.Windows.Forms.NumericUpDown();
            this.fieldPadding = new System.Windows.Forms.NumericUpDown();
            this.fieldEntryOffset = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldNumPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldEntryOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldNumPoints, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldPadding, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.fieldEntryOffset, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(170, 340);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 26);
            this.label6.TabIndex = 10;
            this.label6.Text = "First Entry Offset:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Padding:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Unknown 3:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Unknown 2:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Unknown 1:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Num Points:";
            // 
            // fieldNumPoints
            // 
            this.fieldNumPoints.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldNumPoints.Location = new System.Drawing.Point(88, 3);
            this.fieldNumPoints.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldNumPoints.Name = "fieldNumPoints";
            this.fieldNumPoints.Size = new System.Drawing.Size(79, 20);
            this.fieldNumPoints.TabIndex = 1;
            this.fieldNumPoints.ValueChanged += new System.EventHandler(this.fieldNumPoints_ValueChanged);
            // 
            // fieldUnknown1
            // 
            this.fieldUnknown1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown1.Location = new System.Drawing.Point(88, 29);
            this.fieldUnknown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldUnknown1.Name = "fieldUnknown1";
            this.fieldUnknown1.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown1.TabIndex = 3;
            this.fieldUnknown1.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // fieldUnknown2
            // 
            this.fieldUnknown2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown2.Location = new System.Drawing.Point(88, 55);
            this.fieldUnknown2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldUnknown2.Name = "fieldUnknown2";
            this.fieldUnknown2.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown2.TabIndex = 5;
            this.fieldUnknown2.ValueChanged += new System.EventHandler(this.fieldUnknown2_ValueChanged);
            // 
            // fieldUnknown3
            // 
            this.fieldUnknown3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown3.Location = new System.Drawing.Point(88, 81);
            this.fieldUnknown3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldUnknown3.Name = "fieldUnknown3";
            this.fieldUnknown3.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown3.TabIndex = 7;
            this.fieldUnknown3.ValueChanged += new System.EventHandler(this.fieldUnknown3_ValueChanged);
            // 
            // fieldPadding
            // 
            this.fieldPadding.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPadding.Location = new System.Drawing.Point(88, 107);
            this.fieldPadding.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldPadding.Name = "fieldPadding";
            this.fieldPadding.Size = new System.Drawing.Size(79, 20);
            this.fieldPadding.TabIndex = 9;
            this.fieldPadding.ValueChanged += new System.EventHandler(this.fieldPadding_ValueChanged);
            // 
            // fieldEntryOffset
            // 
            this.fieldEntryOffset.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldEntryOffset.Location = new System.Drawing.Point(88, 133);
            this.fieldEntryOffset.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.fieldEntryOffset.Name = "fieldEntryOffset";
            this.fieldEntryOffset.Size = new System.Drawing.Size(79, 20);
            this.fieldEntryOffset.TabIndex = 11;
            this.fieldEntryOffset.ValueChanged += new System.EventHandler(this.fieldEntryOffset_ValueChanged);
            // 
            // PathEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PathEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.PathEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldNumPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldEntryOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown fieldNumPoints;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
        private System.Windows.Forms.NumericUpDown fieldUnknown2;
        private System.Windows.Forms.NumericUpDown fieldUnknown3;
        private System.Windows.Forms.NumericUpDown fieldPadding;
        private System.Windows.Forms.NumericUpDown fieldEntryOffset;
    }
}
