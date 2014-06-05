using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindViewer.src.Forms
{
    partial class AboutWindow : Form
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutWindow_Load(object sender, EventArgs e)
        {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(appVersion.Build).AddSeconds(appVersion.Revision * 2);

            labelVersion.Text = "[Build: " + buildDate.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}
