namespace WindViewer.Forms.EntityEditors
{
    partial class PositionField
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
            this.fieldY = new System.Windows.Forms.NumericUpDown();
            this.fieldX = new System.Windows.Forms.NumericUpDown();
            this.fieldZ = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldZ)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.fieldY, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldX, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldZ, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(85, 70);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // fieldY
            // 
            this.fieldY.DecimalPlaces = 2;
            this.fieldY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldY.Location = new System.Drawing.Point(0, 24);
            this.fieldY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.fieldY.Name = "fieldY";
            this.fieldY.Size = new System.Drawing.Size(85, 20);
            this.fieldY.TabIndex = 2;
            this.fieldY.ThousandsSeparator = true;
            this.fieldY.ValueChanged += new System.EventHandler(this.fieldY_ValueChanged);
            // 
            // fieldX
            // 
            this.fieldX.DecimalPlaces = 2;
            this.fieldX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldX.Location = new System.Drawing.Point(0, 0);
            this.fieldX.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.fieldX.Name = "fieldX";
            this.fieldX.Size = new System.Drawing.Size(85, 20);
            this.fieldX.TabIndex = 1;
            this.fieldX.ThousandsSeparator = true;
            this.fieldX.ValueChanged += new System.EventHandler(this.fieldX_ValueChanged);
            // 
            // fieldZ
            // 
            this.fieldZ.DecimalPlaces = 2;
            this.fieldZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldZ.Location = new System.Drawing.Point(0, 48);
            this.fieldZ.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.fieldZ.Name = "fieldZ";
            this.fieldZ.Size = new System.Drawing.Size(85, 20);
            this.fieldZ.TabIndex = 0;
            this.fieldZ.ThousandsSeparator = true;
            this.fieldZ.ValueChanged += new System.EventHandler(this.fieldZ_ValueChanged);
            // 
            // PositionField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(85, 70);
            this.Name = "PositionField";
            this.Size = new System.Drawing.Size(85, 70);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldZ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown fieldY;
        private System.Windows.Forms.NumericUpDown fieldX;
        private System.Windows.Forms.NumericUpDown fieldZ;

    }
}
