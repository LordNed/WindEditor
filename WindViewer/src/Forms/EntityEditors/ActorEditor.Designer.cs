namespace WindViewer.Forms.EntityEditors
{
    partial class ActorEditor
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldName = new System.Windows.Forms.TextBox();
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.fieldRpatIndex = new System.Windows.Forms.NumericUpDown();
            this.fieldUnknown2 = new System.Windows.Forms.NumericUpDown();
            this.fieldBehaviorType = new System.Windows.Forms.NumericUpDown();
            this.fieldEnemyNumber = new System.Windows.Forms.NumericUpDown();
            this.fieldPosition = new WindViewer.Forms.EntityEditors.PositionField();
            this.fieldRotation = new WindViewer.Forms.EntityEditors.PositionField();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRpatIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldBehaviorType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldEnemyNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldRpatIndex, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldBehaviorType, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.fieldPosition, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.fieldRotation, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.fieldEnemyNumber, 1, 7);
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
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 26);
            this.label8.TabIndex = 14;
            this.label8.Text = "Enemy Number:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Rotation:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Position:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Behavior Type:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Unknown 2:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "RPAT Index:";
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
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // fieldName
            // 
            this.fieldName.Location = new System.Drawing.Point(88, 3);
            this.fieldName.Name = "fieldName";
            this.fieldName.Size = new System.Drawing.Size(79, 20);
            this.fieldName.TabIndex = 1;
            this.fieldName.TextChanged += new System.EventHandler(this.fieldName_TextChanged);
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
            this.fieldUnknown1.TabIndex = 3;
            this.fieldUnknown1.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // fieldRpatIndex
            // 
            this.fieldRpatIndex.Location = new System.Drawing.Point(88, 55);
            this.fieldRpatIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldRpatIndex.Name = "fieldRpatIndex";
            this.fieldRpatIndex.Size = new System.Drawing.Size(79, 20);
            this.fieldRpatIndex.TabIndex = 5;
            this.fieldRpatIndex.ValueChanged += new System.EventHandler(this.fieldRpatIndex_ValueChanged);
            // 
            // fieldUnknown2
            // 
            this.fieldUnknown2.Location = new System.Drawing.Point(88, 81);
            this.fieldUnknown2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldUnknown2.Name = "fieldUnknown2";
            this.fieldUnknown2.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown2.TabIndex = 7;
            this.fieldUnknown2.ValueChanged += new System.EventHandler(this.fieldUnknown2_ValueChanged);
            // 
            // fieldBehaviorType
            // 
            this.fieldBehaviorType.Location = new System.Drawing.Point(88, 107);
            this.fieldBehaviorType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldBehaviorType.Name = "fieldBehaviorType";
            this.fieldBehaviorType.Size = new System.Drawing.Size(79, 20);
            this.fieldBehaviorType.TabIndex = 9;
            this.fieldBehaviorType.ValueChanged += new System.EventHandler(this.fieldBehaviorType_ValueChanged);
            // 
            // fieldEnemyNumber
            // 
            this.fieldEnemyNumber.Location = new System.Drawing.Point(88, 273);
            this.fieldEnemyNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fieldEnemyNumber.Name = "fieldEnemyNumber";
            this.fieldEnemyNumber.Size = new System.Drawing.Size(79, 20);
            this.fieldEnemyNumber.TabIndex = 15;
            this.fieldEnemyNumber.ValueChanged += new System.EventHandler(this.fieldEnemyNumber_ValueChanged);
            // 
            // fieldPosition
            // 
            this.fieldPosition.AutoSize = true;
            this.fieldPosition.BackColor = System.Drawing.SystemColors.Control;
            this.fieldPosition.Location = new System.Drawing.Point(85, 130);
            this.fieldPosition.Margin = new System.Windows.Forms.Padding(0);
            this.fieldPosition.Maximum = 1000000F;
            this.fieldPosition.Minimum = -1000000F;
            this.fieldPosition.MinimumSize = new System.Drawing.Size(85, 70);
            this.fieldPosition.Name = "fieldPosition";
            this.fieldPosition.Size = new System.Drawing.Size(85, 70);
            this.fieldPosition.TabIndex = 11;
            this.fieldPosition.XValueChanged += new System.EventHandler(this.fieldPosition_XValueChanged);
            this.fieldPosition.YValueChanged += new System.EventHandler(this.fieldPosition_YValueChanged);
            this.fieldPosition.ZValueChanged += new System.EventHandler(this.fieldPosition_ZValueChanged);
            // 
            // fieldRotation
            // 
            this.fieldRotation.AutoSize = true;
            this.fieldRotation.BackColor = System.Drawing.SystemColors.Control;
            this.fieldRotation.Location = new System.Drawing.Point(85, 200);
            this.fieldRotation.Margin = new System.Windows.Forms.Padding(0);
            this.fieldRotation.Maximum = 32767F;
            this.fieldRotation.Minimum = -32768F;
            this.fieldRotation.MinimumSize = new System.Drawing.Size(85, 70);
            this.fieldRotation.Name = "fieldRotation";
            this.fieldRotation.Size = new System.Drawing.Size(85, 70);
            this.fieldRotation.TabIndex = 13;
            this.fieldRotation.XValueChanged += new System.EventHandler(this.fieldRotation_XValueChanged);
            this.fieldRotation.YValueChanged += new System.EventHandler(this.fieldRotation_YValueChanged);
            this.fieldRotation.ZValueChanged += new System.EventHandler(this.fieldRotation_ZValueChanged);
            // 
            // ActorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ActorEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.ActorEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRpatIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldBehaviorType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldEnemyNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fieldName;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
        private System.Windows.Forms.NumericUpDown fieldRpatIndex;
        private System.Windows.Forms.NumericUpDown fieldUnknown2;
        private System.Windows.Forms.NumericUpDown fieldBehaviorType;
        private PositionField fieldPosition;
        private PositionField fieldRotation;
        private System.Windows.Forms.NumericUpDown fieldEnemyNumber;
    }
}
