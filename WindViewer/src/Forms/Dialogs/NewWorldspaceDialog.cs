using System;
using System.Windows.Forms;

namespace WindViewer.Forms.Dialogs
{
    public partial class NewWorldspaceDialog : Form
    {
        public NewWorldspaceDialog()
        {
            InitializeComponent();
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
            okButton.Enabled = tb.Text.Length > 0;
        }
    }
}
