namespace WindViewer.Forms.EntityEditors
{
    partial class DungeonMapEditor
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldMapSpaceX = new System.Windows.Forms.NumericUpDown();
            this.fieldMapSpaceY = new System.Windows.Forms.NumericUpDown();
            this.fieldMapSpaceScale = new System.Windows.Forms.NumericUpDown();
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldMapSpaceX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldMapSpaceY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldMapSpaceScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldMapSpaceX, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldMapSpaceY, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldMapSpaceScale, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(170, 340);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Unknown 1:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 26);
            this.label3.TabIndex = 4;
            this.label3.Text = "Map Space Scale:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Map Space Y:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map Space X:";
            // 
            // fieldMapSpaceX
            // 
            this.fieldMapSpaceX.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldMapSpaceX.DecimalPlaces = 4;
            this.fieldMapSpaceX.Location = new System.Drawing.Point(88, 3);
            this.fieldMapSpaceX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldMapSpaceX.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldMapSpaceX.Name = "fieldMapSpaceX";
            this.fieldMapSpaceX.Size = new System.Drawing.Size(79, 20);
            this.fieldMapSpaceX.TabIndex = 1;
            this.fieldMapSpaceX.ValueChanged += new System.EventHandler(this.fieldMapSpaceX_ValueChanged);
            // 
            // fieldMapSpaceY
            // 
            this.fieldMapSpaceY.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldMapSpaceY.DecimalPlaces = 4;
            this.fieldMapSpaceY.Location = new System.Drawing.Point(88, 29);
            this.fieldMapSpaceY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldMapSpaceY.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldMapSpaceY.Name = "fieldMapSpaceY";
            this.fieldMapSpaceY.Size = new System.Drawing.Size(79, 20);
            this.fieldMapSpaceY.TabIndex = 3;
            this.fieldMapSpaceY.ValueChanged += new System.EventHandler(this.fieldMapSpaceY_ValueChanged);
            // 
            // fieldMapSpaceScale
            // 
            this.fieldMapSpaceScale.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldMapSpaceScale.DecimalPlaces = 4;
            this.fieldMapSpaceScale.Location = new System.Drawing.Point(88, 55);
            this.fieldMapSpaceScale.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldMapSpaceScale.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldMapSpaceScale.Name = "fieldMapSpaceScale";
            this.fieldMapSpaceScale.Size = new System.Drawing.Size(79, 20);
            this.fieldMapSpaceScale.TabIndex = 5;
            this.fieldMapSpaceScale.ValueChanged += new System.EventHandler(this.fieldMapSpaceScale_ValueChanged);
            // 
            // fieldUnknown1
            // 
            this.fieldUnknown1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown1.DecimalPlaces = 4;
            this.fieldUnknown1.Location = new System.Drawing.Point(88, 81);
            this.fieldUnknown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldUnknown1.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldUnknown1.Name = "fieldUnknown1";
            this.fieldUnknown1.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown1.TabIndex = 7;
            this.fieldUnknown1.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // DungeonMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DungeonMapEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.DungeonMapEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldMapSpaceX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldMapSpaceY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldMapSpaceScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown fieldMapSpaceX;
        private System.Windows.Forms.NumericUpDown fieldMapSpaceY;
        private System.Windows.Forms.NumericUpDown fieldMapSpaceScale;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
    }
}
