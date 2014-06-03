namespace WindViewer.Forms.EntityEditors
{
    partial class ShipSpawnEditor
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldPosition = new WindViewer.Forms.EntityEditors.PositionField();
            this.fieldYRot = new System.Windows.Forms.NumericUpDown();
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYRot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldPosition, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldYRot, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(170, 340);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Unknown 1:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y Rotation:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position:";
            // 
            // fieldPosition
            // 
            this.fieldPosition.AutoSize = true;
            this.fieldPosition.BackColor = System.Drawing.SystemColors.Control;
            this.fieldPosition.Location = new System.Drawing.Point(85, 0);
            this.fieldPosition.Margin = new System.Windows.Forms.Padding(0);
            this.fieldPosition.Maximum = 1000000F;
            this.fieldPosition.Minimum = -1000000F;
            this.fieldPosition.MinimumSize = new System.Drawing.Size(85, 70);
            this.fieldPosition.Name = "fieldPosition";
            this.fieldPosition.Size = new System.Drawing.Size(85, 70);
            this.fieldPosition.TabIndex = 1;
            this.fieldPosition.XValueChanged += new System.EventHandler(this.fieldPosition_XValueChanged);
            this.fieldPosition.YValueChanged += new System.EventHandler(this.fieldPosition_YValueChanged);
            this.fieldPosition.ZValueChanged += new System.EventHandler(this.fieldPosition_ZValueChanged);
            // 
            // fieldYRot
            // 
            this.fieldYRot.Location = new System.Drawing.Point(88, 73);
            this.fieldYRot.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldYRot.Name = "fieldYRot";
            this.fieldYRot.Size = new System.Drawing.Size(79, 20);
            this.fieldYRot.TabIndex = 3;
            this.fieldYRot.ValueChanged += new System.EventHandler(this.fieldYRot_ValueChanged);
            // 
            // fieldUnknown1
            // 
            this.fieldUnknown1.Location = new System.Drawing.Point(88, 99);
            this.fieldUnknown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldUnknown1.Name = "fieldUnknown1";
            this.fieldUnknown1.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown1.TabIndex = 5;
            this.fieldUnknown1.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // ShipSpawnEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ShipSpawnEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.ShipSpawnEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYRot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private WindViewer.Forms.EntityEditors.PositionField fieldPosition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown fieldYRot;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
    }
}
