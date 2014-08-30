namespace WindViewer.Editor
{
    partial class TextColorEditorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorIndexPicker = new System.Windows.Forms.DomainUpDown();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.textTestBox = new System.Windows.Forms.TextBox();
            this.colorBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // colorIndexPicker
            // 
            this.colorIndexPicker.Location = new System.Drawing.Point(13, 13);
            this.colorIndexPicker.Name = "colorIndexPicker";
            this.colorIndexPicker.ReadOnly = true;
            this.colorIndexPicker.Size = new System.Drawing.Size(120, 20);
            this.colorIndexPicker.TabIndex = 0;
            this.colorIndexPicker.Text = "domainUpDown1";
            this.colorIndexPicker.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colorIndexPicker.SelectedItemChanged += new System.EventHandler(this.colorIndexPicker_SelectedItemChanged);
            // 
            // textTestBox
            // 
            this.textTestBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.textTestBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textTestBox.Location = new System.Drawing.Point(140, 13);
            this.textTestBox.Multiline = true;
            this.textTestBox.Name = "textTestBox";
            this.textTestBox.Size = new System.Drawing.Size(132, 104);
            this.textTestBox.TabIndex = 2;
            this.textTestBox.Text = "The quick brown fox jumps over the lazy dog.";
            // 
            // colorBox
            // 
            this.colorBox.BackColor = System.Drawing.SystemColors.Control;
            this.colorBox.Location = new System.Drawing.Point(13, 40);
            this.colorBox.Name = "colorBox";
            this.colorBox.Size = new System.Drawing.Size(120, 77);
            this.colorBox.TabIndex = 3;
            this.colorBox.TabStop = false;
            this.colorBox.Click += new System.EventHandler(this.colorBox_Click);
            // 
            // TextColorEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 129);
            this.Controls.Add(this.colorBox);
            this.Controls.Add(this.textTestBox);
            this.Controls.Add(this.colorIndexPicker);
            this.Name = "TextColorEditorForm";
            this.Text = "Color Editor";
            ((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DomainUpDown colorIndexPicker;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox textTestBox;
        public System.Windows.Forms.PictureBox colorBox;
    }
}