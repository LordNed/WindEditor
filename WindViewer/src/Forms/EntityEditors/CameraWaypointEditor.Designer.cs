namespace WindViewer.Forms.EntityEditors
{
    partial class CameraWaypointEditor
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
            this.fieldPadding = new System.Windows.Forms.NumericUpDown();
            this.fieldPosition = new WindViewer.Forms.EntityEditors.PositionField();
            this.fieldRotation = new WindViewer.Forms.EntityEditors.PositionField();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.fieldRotation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldPadding, 1, 2);
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
            this.label3.Location = new System.Drawing.Point(3, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Padding:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Rotation:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position";
            // 
            // fieldPadding
            // 
            this.fieldPadding.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPadding.Location = new System.Drawing.Point(88, 143);
            this.fieldPadding.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.fieldPadding.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.fieldPadding.Name = "fieldPadding";
            this.fieldPadding.Size = new System.Drawing.Size(79, 20);
            this.fieldPadding.TabIndex = 5;
            this.fieldPadding.ValueChanged += new System.EventHandler(this.fieldPadding_ValueChanged);
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
            // fieldRotation
            // 
            this.fieldRotation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldRotation.AutoSize = true;
            this.fieldRotation.BackColor = System.Drawing.SystemColors.Control;
            this.fieldRotation.Location = new System.Drawing.Point(85, 70);
            this.fieldRotation.Margin = new System.Windows.Forms.Padding(0);
            this.fieldRotation.Maximum = 32768F;
            this.fieldRotation.Minimum = -32768F;
            this.fieldRotation.MinimumSize = new System.Drawing.Size(85, 70);
            this.fieldRotation.Name = "fieldRotation";
            this.fieldRotation.Size = new System.Drawing.Size(85, 70);
            this.fieldRotation.TabIndex = 3;
            this.fieldRotation.XValueChanged += new System.EventHandler(this.fieldRotation_XValueChanged);
            this.fieldRotation.YValueChanged += new System.EventHandler(this.fieldRotation_YValueChanged);
            this.fieldRotation.ZValueChanged += new System.EventHandler(this.fieldRotation_ZValueChanged);
            // 
            // CameraWaypointEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CameraWaypointEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.CameraWaypointEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private WindViewer.Forms.EntityEditors.PositionField fieldPosition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private WindViewer.Forms.EntityEditors.PositionField fieldRotation;
        private System.Windows.Forms.NumericUpDown fieldPadding;
    }
}
