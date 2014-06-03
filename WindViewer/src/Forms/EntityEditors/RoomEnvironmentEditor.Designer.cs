namespace WindViewer.src.Forms.EntityEditors
{
    partial class RoomEnvironmentEditor
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
            this.fieldTimePassage = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.fieldWind = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldLighting = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldUnknown2 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldTimePassage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldWind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLighting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldTimePassage, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldWind, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldLighting, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown2, 1, 4);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(170, 340);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Passage Mode:";
            // 
            // fieldTimePassage
            // 
            this.fieldTimePassage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldTimePassage.Location = new System.Drawing.Point(88, 3);
            this.fieldTimePassage.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldTimePassage.Name = "fieldTimePassage";
            this.fieldTimePassage.Size = new System.Drawing.Size(79, 20);
            this.fieldTimePassage.TabIndex = 1;
            this.fieldTimePassage.ValueChanged += new System.EventHandler(this.fieldTimePassage_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Wind:";
            // 
            // fieldWind
            // 
            this.fieldWind.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldWind.Location = new System.Drawing.Point(88, 29);
            this.fieldWind.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldWind.Name = "fieldWind";
            this.fieldWind.Size = new System.Drawing.Size(79, 20);
            this.fieldWind.TabIndex = 3;
            this.fieldWind.ValueChanged += new System.EventHandler(this.fieldWind_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Unknown 1:";
            // 
            // fieldUnknown1
            // 
            this.fieldUnknown1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown1.Location = new System.Drawing.Point(88, 55);
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
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Lighting Type:";
            // 
            // fieldLighting
            // 
            this.fieldLighting.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldLighting.Location = new System.Drawing.Point(88, 81);
            this.fieldLighting.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldLighting.Name = "fieldLighting";
            this.fieldLighting.Size = new System.Drawing.Size(79, 20);
            this.fieldLighting.TabIndex = 7;
            this.fieldLighting.ValueChanged += new System.EventHandler(this.fieldLighting_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Unknown 2:";
            // 
            // fieldUnknown2
            // 
            this.fieldUnknown2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown2.DecimalPlaces = 2;
            this.fieldUnknown2.Location = new System.Drawing.Point(88, 107);
            this.fieldUnknown2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldUnknown2.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldUnknown2.Name = "fieldUnknown2";
            this.fieldUnknown2.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown2.TabIndex = 9;
            this.fieldUnknown2.Value = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldUnknown2.ValueChanged += new System.EventHandler(this.fieldUnknown2_ValueChanged);
            // 
            // RoomEnvironmentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RoomEnvironmentEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.RoomEnvironmentEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldTimePassage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldWind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLighting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown fieldTimePassage;
        private System.Windows.Forms.NumericUpDown fieldWind;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
        private System.Windows.Forms.NumericUpDown fieldLighting;
        private System.Windows.Forms.NumericUpDown fieldUnknown2;
    }
}
