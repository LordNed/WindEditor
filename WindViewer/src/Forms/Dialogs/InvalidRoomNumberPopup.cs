using System;
using System.Windows.Forms;

namespace WindViewer.Forms
{
    public partial class InvalidRoomNumberPopup : Form
    {
        public InvalidRoomNumberPopup()
        {
            InitializeComponent();
        }

        public void SetFailedRoomDescription(string failedRoomName)
        {
            DescriptionLabel.Text = "Failed to automatically determine room number from file name. Expected: Room<x>.arc or R<xx>_00, got: " +
            failedRoomName + ".arc.";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
