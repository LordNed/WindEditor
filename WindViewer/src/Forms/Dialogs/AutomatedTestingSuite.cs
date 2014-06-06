using System.IO;
using System.Windows.Forms;

namespace WindViewer.Forms.Dialogs
{
    public partial class AutomatedTestingSuite : Form
    {
        public AutomatedTestingSuite()
        {
            InitializeComponent();
        }

        private void textSourceDir_TextChanged(object sender, System.EventArgs e)
        {
            UpdateStartButton();
        }

        private void textDestinationDir_TextChanged(object sender, System.EventArgs e)
        {
            UpdateStartButton();
        }

        private void UpdateStartButton()
        {
            if (Directory.Exists(textSourceDir.Text))
            {
                btnStart.Enabled = true;
            }
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            //Create the directory if it doesn't exist already.
            Directory.CreateDirectory(textDestinationDir.Text);

            //ToDo:
            //Invoke MainForm's New From Archives on all archives folder's in \Stage\
            //Copy to output dir, unpack, delete archives. (Create a .wrkDir!)
            //Skip the archive if the folder already exists.
            //Update Status to say unpacking.
            //Set progress bar max to #dirs, update after each unpack.
            //Update status to say Load testing
            //start loading each file one by one by invoking mainforms loader.
        }
    }
}
