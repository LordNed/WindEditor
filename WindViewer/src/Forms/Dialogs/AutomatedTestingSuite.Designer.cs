namespace WindViewer.Forms.Dialogs
{
    partial class AutomatedTestingSuite
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
            this.textSourceDir = new System.Windows.Forms.TextBox();
            this.btnSourceDirBrowse = new System.Windows.Forms.Button();
            this.btnDestinationDirBrowse = new System.Windows.Forms.Button();
            this.textDestinationDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose your res\\Stage\\ dir:";
            // 
            // textSourceDir
            // 
            this.textSourceDir.Location = new System.Drawing.Point(12, 25);
            this.textSourceDir.Name = "textSourceDir";
            this.textSourceDir.Size = new System.Drawing.Size(367, 20);
            this.textSourceDir.TabIndex = 1;
            this.textSourceDir.TextChanged += new System.EventHandler(this.textSourceDir_TextChanged);
            // 
            // btnSourceDirBrowse
            // 
            this.btnSourceDirBrowse.Location = new System.Drawing.Point(385, 22);
            this.btnSourceDirBrowse.Name = "btnSourceDirBrowse";
            this.btnSourceDirBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSourceDirBrowse.TabIndex = 2;
            this.btnSourceDirBrowse.Text = "Browse...";
            this.btnSourceDirBrowse.UseVisualStyleBackColor = true;
            this.btnSourceDirBrowse.Click += new System.EventHandler(this.btnSourceDirBrowse_Click);
            // 
            // btnDestinationDirBrowse
            // 
            this.btnDestinationDirBrowse.Location = new System.Drawing.Point(385, 61);
            this.btnDestinationDirBrowse.Name = "btnDestinationDirBrowse";
            this.btnDestinationDirBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnDestinationDirBrowse.TabIndex = 5;
            this.btnDestinationDirBrowse.Text = "Browse...";
            this.btnDestinationDirBrowse.UseVisualStyleBackColor = true;
            this.btnDestinationDirBrowse.Click += new System.EventHandler(this.btnDestinationDirBrowse_Click);
            // 
            // textDestinationDir
            // 
            this.textDestinationDir.Location = new System.Drawing.Point(12, 64);
            this.textDestinationDir.Name = "textDestinationDir";
            this.textDestinationDir.Size = new System.Drawing.Size(367, 20);
            this.textDestinationDir.TabIndex = 4;
            this.textDestinationDir.TextChanged += new System.EventHandler(this.textDestinationDir_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Choose your output dir:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(304, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(385, 123);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 90);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(447, 23);
            this.progressBar.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Status:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(58, 128);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(52, 13);
            this.statusLabel.TabIndex = 10;
            this.statusLabel.Text = "Waiting...";
            // 
            // AutomatedTestingSuite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 158);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDestinationDirBrowse);
            this.Controls.Add(this.textDestinationDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSourceDirBrowse);
            this.Controls.Add(this.textSourceDir);
            this.Controls.Add(this.label1);
            this.Name = "AutomatedTestingSuite";
            this.Text = "AutomatedTestingSuite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textSourceDir;
        private System.Windows.Forms.Button btnSourceDirBrowse;
        private System.Windows.Forms.Button btnDestinationDirBrowse;
        private System.Windows.Forms.TextBox textDestinationDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label statusLabel;
    }
}