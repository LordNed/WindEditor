namespace WindViewer.Forms.EntityEditors
{
    partial class RoomPosEditor
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
            this.fieldUnknown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldXPos = new System.Windows.Forms.NumericUpDown();
            this.fieldYPos = new System.Windows.Forms.NumericUpDown();
            this.fieldYRot = new System.Windows.Forms.NumericUpDown();
            this.fieldRoomId = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldXPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYRot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRoomId)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.fieldUnknown1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldXPos, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.fieldYPos, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.fieldYRot, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.fieldRoomId, 1, 3);
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
            // fieldUnknown1
            // 
            this.fieldUnknown1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldUnknown1.Location = new System.Drawing.Point(88, 107);
            this.fieldUnknown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldUnknown1.Name = "fieldUnknown1";
            this.fieldUnknown1.Size = new System.Drawing.Size(79, 20);
            this.fieldUnknown1.TabIndex = 9;
            this.fieldUnknown1.ValueChanged += new System.EventHandler(this.fieldUnknown1_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Unknown 1:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Room Id:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y Rotation:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y Translation:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X Translation:";
            // 
            // fieldXPos
            // 
            this.fieldXPos.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldXPos.Location = new System.Drawing.Point(88, 3);
            this.fieldXPos.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldXPos.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldXPos.Name = "fieldXPos";
            this.fieldXPos.Size = new System.Drawing.Size(79, 20);
            this.fieldXPos.TabIndex = 1;
            this.fieldXPos.ValueChanged += new System.EventHandler(this.fieldXPos_ValueChanged);
            // 
            // fieldYPos
            // 
            this.fieldYPos.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldYPos.Location = new System.Drawing.Point(88, 29);
            this.fieldYPos.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fieldYPos.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.fieldYPos.Name = "fieldYPos";
            this.fieldYPos.Size = new System.Drawing.Size(79, 20);
            this.fieldYPos.TabIndex = 3;
            this.fieldYPos.ValueChanged += new System.EventHandler(this.fieldYPos_ValueChanged);
            // 
            // fieldYRot
            // 
            this.fieldYRot.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldYRot.Location = new System.Drawing.Point(88, 55);
            this.fieldYRot.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.fieldYRot.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.fieldYRot.Name = "fieldYRot";
            this.fieldYRot.Size = new System.Drawing.Size(79, 20);
            this.fieldYRot.TabIndex = 5;
            this.fieldYRot.ValueChanged += new System.EventHandler(this.fieldYRot_ValueChanged);
            // 
            // fieldRoomId
            // 
            this.fieldRoomId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fieldRoomId.Location = new System.Drawing.Point(88, 81);
            this.fieldRoomId.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fieldRoomId.Name = "fieldRoomId";
            this.fieldRoomId.Size = new System.Drawing.Size(79, 20);
            this.fieldRoomId.TabIndex = 7;
            this.fieldRoomId.ValueChanged += new System.EventHandler(this.fieldRoomId_ValueChanged);
            // 
            // RoomPosEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RoomPosEditor";
            this.Size = new System.Drawing.Size(170, 340);
            this.Load += new System.EventHandler(this.RoomPosEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldUnknown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldXPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldYRot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldRoomId)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown fieldUnknown1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown fieldXPos;
        private System.Windows.Forms.NumericUpDown fieldYPos;
        private System.Windows.Forms.NumericUpDown fieldYRot;
        private System.Windows.Forms.NumericUpDown fieldRoomId;
    }
}
