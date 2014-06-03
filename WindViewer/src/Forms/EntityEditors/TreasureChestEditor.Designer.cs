namespace WindViewer.Forms.EntityEditors
{
    partial class TreasureChestEditor
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
            this.fieldPadding = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldChestName = new System.Windows.Forms.TextBox();
            this.fieldChestType = new System.Windows.Forms.NumericUpDown();
            this.fieldPosition = new WindViewer.Forms.EntityEditors.PositionField();
            this.fieldUnknown2 = new System.Windows.Forms.NumericUpDown();
            this.fieldYRotation = new System.Windows.Forms.NumericUpDown();
            this.fieldChestItem = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldChestType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYRotation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldChestItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.fieldPadding, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldChestName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldChestType, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldPosition, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.fieldYRotation, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.fieldChestItem, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
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
            // fieldPadding
            // 
            this.fieldPadding.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPadding.Location = new System.Drawing.Point(88, 229);
            this.fieldPadding.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldPadding.Name = "fieldPadding";
            this.fieldPadding.Size = new System.Drawing.Size(79, 20);
            this.fieldPadding.TabIndex = 15;
            this.fieldPadding.ValueChanged += new System.EventHandler(this.fieldPadding_ValueChanged);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Padding:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Chest Item:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Y Rotation:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Unknown 2:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Chest Type:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // fieldChestName
            // 
            this.fieldChestName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldChestName.Location = new System.Drawing.Point(88, 3);
            this.fieldChestName.Name = "fieldChestName";
            this.fieldChestName.Size = new System.Drawing.Size(79, 20);
            this.fieldChestName.TabIndex = 1;
            this.fieldChestName.TextChanged += new System.EventHandler(this.fieldChestName_TextChanged);
            // 
            // fieldChestType
            // 
            this.fieldChestType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldChestType.Location = new System.Drawing.Point(88, 55);
            this.fieldChestType.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldChestType.Name = "fieldChestType";
            this.fieldChestType.Size = new System.Drawing.Size(79, 20);
            this.fieldChestType.TabIndex = 3;
            this.fieldChestType.ValueChanged += new System.EventHandler(this.fieldChestType_ValueChanged);
            // 
            // fieldPosition
            // 
            this.fieldPosition.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldPosition.AutoSize = true;
            this.fieldPosition.BackColor = System.Drawing.SystemColors.Control;
            this.fieldPosition.Location = new System.Drawing.Point(85, 78);
            this.fieldPosition.Margin = new System.Windows.Forms.Padding(0);
            this.fieldPosition.Maximum = 1000000F;
            this.fieldPosition.Minimum = -1000000F;
            this.fieldPosition.MinimumSize = new System.Drawing.Size(85, 70);
            this.fieldPosition.Name = "fieldPosition";
            this.fieldPosition.Size = new System.Drawing.Size(85, 70);
            this.fieldPosition.TabIndex = 5;
            this.fieldPosition.XValueChanged += new System.EventHandler(this.fieldPosition_XValueChanged);
            this.fieldPosition.YValueChanged += new System.EventHandler(this.fieldPosition_YValueChanged);
            this.fieldPosition.ZValueChanged += new System.EventHandler(this.fieldPosition_ZValueChanged);
            // 
            // fieldUnknown2
            // 
            this.fieldUnknown2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown2.Location = new System.Drawing.Point(88, 151);
            this.fieldUnknown2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldUnknown2.Name = "fieldUnknown2";
            this.fieldUnknown2.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown2.TabIndex = 7;
            this.fieldUnknown2.ValueChanged += new System.EventHandler(this.fieldUnknown2_ValueChanged);
            // 
            // fieldYRotation
            // 
            this.fieldYRotation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldYRotation.Location = new System.Drawing.Point(88, 177);
            this.fieldYRotation.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldYRotation.Name = "fieldYRotation";
            this.fieldYRotation.Size = new System.Drawing.Size(79, 20);
            this.fieldYRotation.TabIndex = 9;
            this.fieldYRotation.ValueChanged += new System.EventHandler(this.fieldYRotation_ValueChanged);
            // 
            // fieldChestItem
            // 
            this.fieldChestItem.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldChestItem.Location = new System.Drawing.Point(88, 203);
            this.fieldChestItem.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldChestItem.Name = "fieldChestItem";
            this.fieldChestItem.Size = new System.Drawing.Size(79, 20);
            this.fieldChestItem.TabIndex = 11;
            this.fieldChestItem.ValueChanged += new System.EventHandler(this.fieldChestItem_ValueChanged);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Position:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Unknown 1:";
            // 
            // fieldUnknown1
            // 
            this.fieldUnknown1.Location = new System.Drawing.Point(88, 29);
            this.fieldUnknown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldUnknown1.Name = "fieldUnknown1";
            this.fieldUnknown1.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown1.TabIndex = 13;
            this.fieldUnknown1.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // TreasureChestEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TreasureChestEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.TreasureChestEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPadding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldChestType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYRotation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldChestItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fieldChestName;
        private System.Windows.Forms.NumericUpDown fieldChestType;
        private PositionField fieldPosition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown fieldUnknown2;
        private System.Windows.Forms.NumericUpDown fieldYRotation;
        private System.Windows.Forms.NumericUpDown fieldChestItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
        private System.Windows.Forms.NumericUpDown fieldPadding;
        private System.Windows.Forms.Label label8;
    }
}
