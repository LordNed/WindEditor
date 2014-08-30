using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using WindViewer.FileFormats;
using WindViewer.Forms;

namespace WindViewer
{
    static class Program
    {
        public static Stopwatch DeltaTimeStopwatch;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DeltaTimeStopwatch = new Stopwatch();

            // Upgrade any user settings from a previous version if possible.
            // This prevents them from being wiped every time they update the
            // tool.
            if (Properties.Settings.Default.shouldUpgradeSettings)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.shouldUpgradeSettings = false;
            }

            Application.Run(new MainEditor());
        }
    }
}
