using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FolderSelect;

namespace WindViewer.src.Forms.Dialogs
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderSelectDialog fsd = new FolderSelectDialog();
            fsd.InitialDirectory = "C:\\";
            fsd.Title = "Choose the root dir of your WindWaker ISO extract.";
            if(fsd.ShowDialog())
            {
                textDir.Text = fsd.FileName;
                UpdateButtonStates();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.rootDiskDir = textDir.Text;
            Properties.Settings.Default.Save();
            Close();
        }

        private void textDir_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            btnOk.Enabled = Directory.Exists(textDir.Text);
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            textDir.Text = Properties.Settings.Default.rootDiskDir;
            UpdateButtonStates();
        }
    }
}
