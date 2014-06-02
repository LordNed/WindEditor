using System.Diagnostics;
using System.Windows.Forms;

namespace WindViewer.Forms.EntityEditors
{
    public partial class UnsupportedEntity : UserControl
    {
        public UnsupportedEntity()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/LordNed/WindEditor/wiki/Property-Editor-Status");
        }
    }
}
