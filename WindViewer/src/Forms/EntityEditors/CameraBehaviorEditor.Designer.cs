namespace WindViewer.src.Forms.EntityEditors
{
    partial class CameraBehaviorEditor
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
            this.fieldCameraType = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fieldRaroIndex = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.fieldPadding1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldPadding2 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldPadding3 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRaroIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding3)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.fieldCameraType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldRaroIndex, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldPadding1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldPadding2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldPadding3, 1, 4);
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
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Camera Type:";
            // 
            // fieldCameraType
            // 
            this.fieldCameraType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldCameraType.Location = new System.Drawing.Point(88, 3);
            this.fieldCameraType.Name = "fieldCameraType";
            this.fieldCameraType.Size = new System.Drawing.Size(79, 20);
            this.fieldCameraType.TabIndex = 1;
            this.fieldCameraType.TextChanged += new System.EventHandler(this.fieldCameraType_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "RARO Index:";
            // 
            // fieldRaroIndex
            // 
            this.fieldRaroIndex.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldRaroIndex.Location = new System.Drawing.Point(88, 29);
            this.fieldRaroIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldRaroIndex.Name = "fieldRaroIndex";
            this.fieldRaroIndex.Size = new System.Drawing.Size(79, 20);
            this.fieldRaroIndex.TabIndex = 3;
            this.fieldRaroIndex.ValueChanged += new System.EventHandler(this.fieldRaroIndex_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Padding:";
            // 
            // fieldPadding1
            // 
            this.fieldPadding1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPadding1.Location = new System.Drawing.Point(88, 55);
            this.fieldPadding1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldPadding1.Name = "fieldPadding1";
            this.fieldPadding1.Size = new System.Drawing.Size(79, 20);
            this.fieldPadding1.TabIndex = 5;
            this.fieldPadding1.ValueChanged += new System.EventHandler(this.fieldPadding1_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Padding";
            // 
            // fieldPadding2
            // 
            this.fieldPadding2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPadding2.Location = new System.Drawing.Point(88, 81);
            this.fieldPadding2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldPadding2.Name = "fieldPadding2";
            this.fieldPadding2.Size = new System.Drawing.Size(79, 20);
            this.fieldPadding2.TabIndex = 7;
            this.fieldPadding2.ValueChanged += new System.EventHandler(this.fieldPadding2_ValueChanged);
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
            // fieldPadding3
            // 
            this.fieldPadding3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPadding3.Location = new System.Drawing.Point(88, 107);
            this.fieldPadding3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldPadding3.Name = "fieldPadding3";
            this.fieldPadding3.Size = new System.Drawing.Size(79, 20);
            this.fieldPadding3.TabIndex = 9;
            this.fieldPadding3.ValueChanged += new System.EventHandler(this.fieldPadding3_ValueChanged);
            // 
            // CameraBehaviorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CameraBehaviorEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.CameraBehaviorEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRaroIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fieldCameraType;
        private System.Windows.Forms.NumericUpDown fieldRaroIndex;
        private System.Windows.Forms.NumericUpDown fieldPadding1;
        private System.Windows.Forms.NumericUpDown fieldPadding2;
        private System.Windows.Forms.NumericUpDown fieldPadding3;
    }
}
