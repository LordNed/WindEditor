namespace WindViewer.src.Forms.EntityEditors
{
    partial class InteriorLightEditor
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fieldColorR = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldColorG = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldColorB = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.fieldColorA = new System.Windows.Forms.NumericUpDown();
            this.fieldRadius = new WindViewer.Forms.EntityEditors.PositionField();
            this.fieldPosition = new WindViewer.Forms.EntityEditors.PositionField();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorA)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.fieldRadius, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldPosition, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldColorR, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldColorG, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldColorB, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.fieldColorA, 1, 5);
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
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Radius:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Color R:";
            // 
            // fieldColorR
            // 
            this.fieldColorR.Location = new System.Drawing.Point(88, 143);
            this.fieldColorR.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldColorR.Name = "fieldColorR";
            this.fieldColorR.Size = new System.Drawing.Size(79, 20);
            this.fieldColorR.TabIndex = 5;
            this.fieldColorR.ValueChanged += new System.EventHandler(this.fieldColorR_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Color G:";
            // 
            // fieldColorG
            // 
            this.fieldColorG.Location = new System.Drawing.Point(88, 169);
            this.fieldColorG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldColorG.Name = "fieldColorG";
            this.fieldColorG.Size = new System.Drawing.Size(79, 20);
            this.fieldColorG.TabIndex = 7;
            this.fieldColorG.ValueChanged += new System.EventHandler(this.fieldColorG_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 198);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Color B:";
            // 
            // fieldColorB
            // 
            this.fieldColorB.Location = new System.Drawing.Point(88, 195);
            this.fieldColorB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldColorB.Name = "fieldColorB";
            this.fieldColorB.Size = new System.Drawing.Size(79, 20);
            this.fieldColorB.TabIndex = 9;
            this.fieldColorB.ValueChanged += new System.EventHandler(this.fieldColorB_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Color A:";
            // 
            // fieldColorA
            // 
            this.fieldColorA.Location = new System.Drawing.Point(88, 221);
            this.fieldColorA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldColorA.Name = "fieldColorA";
            this.fieldColorA.Size = new System.Drawing.Size(79, 20);
            this.fieldColorA.TabIndex = 11;
            this.fieldColorA.ValueChanged += new System.EventHandler(this.fieldColorA_ValueChanged);
            // 
            // fieldRadius
            // 
            this.fieldRadius.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldRadius.AutoSize = true;
            this.fieldRadius.BackColor = System.Drawing.SystemColors.Control;
            this.fieldRadius.Location = new System.Drawing.Point(85, 70);
            this.fieldRadius.Margin = new System.Windows.Forms.Padding(0);
            this.fieldRadius.Maximum = 100000F;
            this.fieldRadius.Minimum = -100000F;
            this.fieldRadius.MinimumSize = new System.Drawing.Size(85, 70);
            this.fieldRadius.Name = "fieldRadius";
            this.fieldRadius.Size = new System.Drawing.Size(85, 70);
            this.fieldRadius.TabIndex = 3;
            this.fieldRadius.XValueChanged += new System.EventHandler(this.fieldRadius_XValueChanged);
            this.fieldRadius.YValueChanged += new System.EventHandler(this.fieldRadius_YValueChanged);
            this.fieldRadius.ZValueChanged += new System.EventHandler(this.fieldRadius_ZValueChanged);
            // 
            // fieldPosition
            // 
            this.fieldPosition.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPosition.AutoSize = true;
            this.fieldPosition.BackColor = System.Drawing.SystemColors.Control;
            this.fieldPosition.Location = new System.Drawing.Point(85, 0);
            this.fieldPosition.Margin = new System.Windows.Forms.Padding(0);
            this.fieldPosition.Maximum = 100000F;
            this.fieldPosition.Minimum = -100000F;
            this.fieldPosition.MinimumSize = new System.Drawing.Size(85, 70);
            this.fieldPosition.Name = "fieldPosition";
            this.fieldPosition.Size = new System.Drawing.Size(85, 70);
            this.fieldPosition.TabIndex = 1;
            this.fieldPosition.XValueChanged += new System.EventHandler(this.fieldPosition_XValueChanged);
            this.fieldPosition.YValueChanged += new System.EventHandler(this.fieldPosition_YValueChanged);
            this.fieldPosition.ZValueChanged += new System.EventHandler(this.fieldPosition_ZValueChanged);
            // 
            // InteriorLightEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "InteriorLightEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.InteriorLightEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldColorA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private WindViewer.Forms.EntityEditors.PositionField fieldPosition;
        private System.Windows.Forms.Label label2;
        private WindViewer.Forms.EntityEditors.PositionField fieldRadius;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown fieldColorR;
        private System.Windows.Forms.NumericUpDown fieldColorG;
        private System.Windows.Forms.NumericUpDown fieldColorB;
        private System.Windows.Forms.NumericUpDown fieldColorA;
    }
}
