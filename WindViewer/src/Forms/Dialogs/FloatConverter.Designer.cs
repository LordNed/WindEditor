namespace WindViewer.Forms
{
    partial class FloatConverter
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
            this.label1 = new System.Windows.Forms.Label();
            this.floatValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hexValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Floating Point:";
            // 
            // floatValue
            // 
            this.floatValue.Location = new System.Drawing.Point(219, 12);
            this.floatValue.Name = "floatValue";
            this.floatValue.Size = new System.Drawing.Size(100, 20);
            this.floatValue.TabIndex = 1;
            this.floatValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.floatValue.TextChanged += new System.EventHandler(this.floatValue_Changed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hexadecimal:";
            // 
            // hexValue
            // 
            this.hexValue.Location = new System.Drawing.Point(219, 38);
            this.hexValue.Name = "hexValue";
            this.hexValue.Size = new System.Drawing.Size(100, 20);
            this.hexValue.TabIndex = 3;
            this.hexValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hexValue.TextChanged += new System.EventHandler(this.hexValue_Changed);
            // 
            // FloatConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 72);
            this.Controls.Add(this.hexValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.floatValue);
            this.Controls.Add(this.label1);
            this.Name = "FloatConverter";
            this.Text = "Float Converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox floatValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hexValue;
    }
}