using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindViewer.Forms.Dialogs
{
    public partial class NewWorldspaceDialog : Form
    {
        private string _workingDir;
        public NewWorldspaceDialog()
        {
            InitializeComponent();
            _workingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Application.ProductName);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://github.com/pho/WindViewer/wiki/Terms-to-Know");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void dirName_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox) sender;
            string dirPath = Path.Combine(_workingDir, tb.Text + ".wrkDir");
            if (Directory.Exists(dirPath))
            {
                okButton.Enabled = false;
                tb.ForeColor = Color.Red;
            }
            else
            {
                okButton.Enabled = tb.Text.Length > 0;
                tb.ForeColor = Color.Black;
            }

        }
    }
}
