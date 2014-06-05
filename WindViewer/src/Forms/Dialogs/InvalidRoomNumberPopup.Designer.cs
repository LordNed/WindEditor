namespace WindViewer.Forms
{
    partial class InvalidRoomNumberPopup
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
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.numDesc = new System.Windows.Forms.Label();
            this.roomNumberSelector = new System.Windows.Forms.NumericUpDown();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.roomNumberSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Location = new System.Drawing.Point(12, 9);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(276, 41);
            this.DescriptionLabel.TabIndex = 0;
            this.DescriptionLabel.Text = "Failed to automatically determine room number from file name. Expected: Room<x>.a" +
    "rc or R<xx>_00, got: blahblahbllah.arc";
            this.DescriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // numDesc
            // 
            this.numDesc.AutoSize = true;
            this.numDesc.Location = new System.Drawing.Point(12, 55);
            this.numDesc.Name = "numDesc";
            this.numDesc.Size = new System.Drawing.Size(144, 13);
            this.numDesc.TabIndex = 1;
            this.numDesc.Text = "Enter Manual Room Number:";
            // 
            // roomNumberSelector
            // 
            this.roomNumberSelector.Location = new System.Drawing.Point(168, 53);
            this.roomNumberSelector.Name = "roomNumberSelector";
            this.roomNumberSelector.Size = new System.Drawing.Size(120, 20);
            this.roomNumberSelector.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(108, 86);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Ok";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // InvalidRoomNumberPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 121);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.roomNumberSelector);
            this.Controls.Add(this.numDesc);
            this.Controls.Add(this.DescriptionLabel);
            this.Name = "InvalidRoomNumberPopup";
            this.Text = "Invalid Room Number";
            ((System.ComponentModel.ISupportInitialize)(this.roomNumberSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label numDesc;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.NumericUpDown roomNumberSelector;
        private System.Windows.Forms.Label DescriptionLabel;

    }
}