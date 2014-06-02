namespace WindViewer.Forms.EntityEditors
{
    partial class PlayerEditor
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.fieldRotZ = new System.Windows.Forms.NumericUpDown();
            this.fieldRotY = new System.Windows.Forms.NumericUpDown();
            this.fieldRotX = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldName = new System.Windows.Forms.TextBox();
            this.fieldEventIndex = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fieldUnkn1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldSpawnType = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldRoomNumber = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fieldPosZ = new System.Windows.Forms.NumericUpDown();
            this.fieldPosY = new System.Windows.Forms.NumericUpDown();
            this.fieldPosX = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRotZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRotY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldEventIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnkn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldSpawnType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRoomNumber)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPosX)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.88235F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.11765F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldEventIndex, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldUnkn1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.fieldSpawnType, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.fieldRoomNumber, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(170, 320);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel2.Controls.Add(this.fieldRotZ);
            this.panel2.Controls.Add(this.fieldRotY);
            this.panel2.Controls.Add(this.fieldRotX);
            this.panel2.Location = new System.Drawing.Point(95, 210);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(75, 80);
            this.panel2.TabIndex = 15;
            // 
            // fieldRotZ
            // 
            this.fieldRotZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldRotZ.Location = new System.Drawing.Point(3, 55);
            this.fieldRotZ.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldRotZ.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldRotZ.Name = "fieldRotZ";
            this.fieldRotZ.Size = new System.Drawing.Size(70, 20);
            this.fieldRotZ.TabIndex = 14;
            this.fieldRotZ.ThousandsSeparator = true;
            this.fieldRotZ.ValueChanged += new System.EventHandler(this.fieldRot_ValueChanged);
            // 
            // fieldRotY
            // 
            this.fieldRotY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldRotY.Location = new System.Drawing.Point(3, 29);
            this.fieldRotY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldRotY.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldRotY.Name = "fieldRotY";
            this.fieldRotY.Size = new System.Drawing.Size(70, 20);
            this.fieldRotY.TabIndex = 13;
            this.fieldRotY.ThousandsSeparator = true;
            this.fieldRotY.ValueChanged += new System.EventHandler(this.fieldRot_ValueChanged);
            // 
            // fieldRotX
            // 
            this.fieldRotX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldRotX.Location = new System.Drawing.Point(3, 3);
            this.fieldRotX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldRotX.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldRotX.Name = "fieldRotX";
            this.fieldRotX.Size = new System.Drawing.Size(70, 20);
            this.fieldRotX.TabIndex = 12;
            this.fieldRotX.ThousandsSeparator = true;
            this.fieldRotX.ValueChanged += new System.EventHandler(this.fieldRot_ValueChanged);
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
            this.fieldName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldName.Location = new System.Drawing.Point(98, 3);
            this.fieldName.Name = "fieldName";
            this.fieldName.Size = new System.Drawing.Size(69, 20);
            this.fieldName.TabIndex = 1;
            this.fieldName.TextChanged += new System.EventHandler(this.fieldName_TextChanged);
            // 
            // fieldEventIndex
            // 
            this.fieldEventIndex.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldEventIndex.Location = new System.Drawing.Point(98, 29);
            this.fieldEventIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldEventIndex.Name = "fieldEventIndex";
            this.fieldEventIndex.Size = new System.Drawing.Size(69, 20);
            this.fieldEventIndex.TabIndex = 3;
            this.fieldEventIndex.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldEventIndex.ValueChanged += new System.EventHandler(this.fieldEventIndex_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Event Index";
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
            // fieldUnkn1
            // 
            this.fieldUnkn1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnkn1.Location = new System.Drawing.Point(98, 55);
            this.fieldUnkn1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldUnkn1.Name = "fieldUnkn1";
            this.fieldUnkn1.Size = new System.Drawing.Size(69, 20);
            this.fieldUnkn1.TabIndex = 5;
            this.fieldUnkn1.ValueChanged += new System.EventHandler(this.fieldUnkn1_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Spawn Type:";
            // 
            // fieldSpawnType
            // 
            this.fieldSpawnType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldSpawnType.Location = new System.Drawing.Point(98, 81);
            this.fieldSpawnType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldSpawnType.Name = "fieldSpawnType";
            this.fieldSpawnType.Size = new System.Drawing.Size(69, 20);
            this.fieldSpawnType.TabIndex = 7;
            this.fieldSpawnType.ValueChanged += new System.EventHandler(this.fieldSpawnType_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Room Number:";
            // 
            // fieldRoomNumber
            // 
            this.fieldRoomNumber.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldRoomNumber.Location = new System.Drawing.Point(98, 107);
            this.fieldRoomNumber.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldRoomNumber.Name = "fieldRoomNumber";
            this.fieldRoomNumber.Size = new System.Drawing.Size(69, 20);
            this.fieldRoomNumber.TabIndex = 9;
            this.fieldRoomNumber.ValueChanged += new System.EventHandler(this.fieldRoomNumber_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel1.Controls.Add(this.fieldPosZ);
            this.panel1.Controls.Add(this.fieldPosY);
            this.panel1.Controls.Add(this.fieldPosX);
            this.panel1.Location = new System.Drawing.Point(95, 130);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(75, 80);
            this.panel1.TabIndex = 13;
            // 
            // fieldPosZ
            // 
            this.fieldPosZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldPosZ.Location = new System.Drawing.Point(3, 55);
            this.fieldPosZ.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldPosZ.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldPosZ.Name = "fieldPosZ";
            this.fieldPosZ.Size = new System.Drawing.Size(70, 20);
            this.fieldPosZ.TabIndex = 14;
            this.fieldPosZ.ThousandsSeparator = true;
            this.fieldPosZ.ValueChanged += new System.EventHandler(this.fieldPos_ValueChanged);
            // 
            // fieldPosY
            // 
            this.fieldPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldPosY.Location = new System.Drawing.Point(3, 29);
            this.fieldPosY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldPosY.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldPosY.Name = "fieldPosY";
            this.fieldPosY.Size = new System.Drawing.Size(70, 20);
            this.fieldPosY.TabIndex = 13;
            this.fieldPosY.ThousandsSeparator = true;
            this.fieldPosY.ValueChanged += new System.EventHandler(this.fieldPos_ValueChanged);
            // 
            // fieldPosX
            // 
            this.fieldPosX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldPosX.Location = new System.Drawing.Point(3, 3);
            this.fieldPosX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldPosX.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldPosX.Name = "fieldPosX";
            this.fieldPosX.Size = new System.Drawing.Size(70, 20);
            this.fieldPosX.TabIndex = 12;
            this.fieldPosX.ThousandsSeparator = true;
            this.fieldPosX.ValueChanged += new System.EventHandler(this.fieldPos_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 135);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Spawn Pos:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 215);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Spawn Rot:";
            // 
            // PlayerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PlayerEditor";
            this.Size = new System.Drawing.Size(170, 320);
            this.Load += new System.EventHandler(this.PlayerEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldRotZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRotY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldEventIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnkn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldSpawnType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRoomNumber)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldPosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPosX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fieldName;
        private System.Windows.Forms.NumericUpDown fieldEventIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown fieldUnkn1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown fieldSpawnType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown fieldRoomNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown fieldPosZ;
        private System.Windows.Forms.NumericUpDown fieldPosY;
        private System.Windows.Forms.NumericUpDown fieldPosX;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown fieldRotZ;
        private System.Windows.Forms.NumericUpDown fieldRotY;
        private System.Windows.Forms.NumericUpDown fieldRotX;
        private System.Windows.Forms.Label label7;
    }
}
