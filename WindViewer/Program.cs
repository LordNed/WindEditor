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
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            Application.Run(new MainEditor());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
