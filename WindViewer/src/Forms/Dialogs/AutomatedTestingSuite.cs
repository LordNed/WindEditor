using System.IO;
using System.Windows.Forms;
using WindViewer.Editor;
using FolderSelect;
using System;
using WindViewer.FileFormats;
using System.Collections.Generic;
using System.Reflection;

namespace WindViewer.Forms.Dialogs
{
    public partial class AutomatedTestingSuite : Form
    {
        private MainEditor _mainEditor;

        //For testing
        private string _outputDir;
        private int _numDifferences;

        public AutomatedTestingSuite(MainEditor editor)
        {
            InitializeComponent();
            _mainEditor = editor;

            //ToDo: We should really use a BackgroundWorker class/thread and do the
            //work there, then push progress updates to the UI via the ReportProgress
            //method. This will keep the UI (and the cancel button xD) responsive.
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
            btnStart.Enabled = Directory.Exists(textSourceDir.Text);
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            btnCancel.Enabled = false;
            btnCancel.Refresh(); //Hack

            //Create the directory if it doesn't exist already.
            Directory.CreateDirectory(textDestinationDir.Text);

            string[] archiveFolders = Directory.GetDirectories(textSourceDir.Text);
            if(archiveFolders.Length == 0)
            {
                statusLabel.Text = "No sub-directories found, check source dir!";
                return;
            }

            Console.WriteLine("Hold onto your hats, crunching {0} folders!", archiveFolders.Length);
            statusLabel.Text = "Unpacking archives...";
            statusLabel.Refresh(); //Hack
            progressBar.Maximum = archiveFolders.Length;
            progressBar.Refresh(); //Hack

            foreach (string subFolder in archiveFolders)
            {
                string[] files = Directory.GetFiles(subFolder, "*.arc");
                string folderName = new System.IO.DirectoryInfo(subFolder).Name;

                string folderFilepath = textDestinationDir.Text;
                if (Directory.Exists(folderFilepath + "\\\\" +  folderName + ".wrkDir"))
                {
                    Console.WriteLine("Folder {0} already unpacked, skipping...", subFolder);
                    continue;
                }

                if (files.Length == 0)
                {
                    Console.WriteLine("No archive found in subfolder {0}, skipping...", subFolder);
                    continue;
                }
                
                MainEditor.CreateWorkingDirFromArchive(files, folderName, folderFilepath);
                progressBar.Value++;
                progressBar.Refresh();
            }

            string[] extractedProjects = Directory.GetDirectories(textDestinationDir.Text);

            statusLabel.Text = "Extraction complete. Beginning tests on " + extractedProjects.Length + "...";
            statusLabel.Refresh(); //Hack
            progressBar.Maximum = extractedProjects.Length;
            progressBar.Value = 0;

            _outputDir = textDestinationDir.Text;
            File.Delete(_outputDir + "//results.txt");

            foreach (string projDir in extractedProjects)
            {
                _mainEditor.OpenFileFromWorkingDir(projDir, true);

                progressBar.Value++;
                progressBar.Refresh(); //Hack
            }

            Console.WriteLine("Automated tests completed.");
            statusLabel.Text = "Completed.";
            progressBar.Value = 0;
            btnCancel.Enabled = true;
        }

        private void btnSourceDirBrowse_Click(object sender, System.EventArgs e)
        {
            FolderSelectDialog ofd = new FolderSelectDialog();
            ofd.Title = "Navigate to a folder that contains LoZ Content";

            string workingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
            ofd.InitialDirectory = workingDir;

            if (ofd.ShowDialog(this.Handle))
            {
                textSourceDir.Text = ofd.FileName;
            }
        }

        private void btnDestinationDirBrowse_Click(object sender, EventArgs e)
        {
            FolderSelectDialog ofd = new FolderSelectDialog();
            ofd.Title = "Navigate to output folder...";

            string workingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
            ofd.InitialDirectory = workingDir;

            if (ofd.ShowDialog(this.Handle))
            {
                textDestinationDir.Text = ofd.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AutomatedTestingSuite_Load(object sender, EventArgs e)
        {
            MainEditor.WorldspaceProjectLoaded += OnWorldspaceProjectLoaded;
        }

        private void AutomatedTestingSuite_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainEditor.WorldspaceProjectLoaded -= OnWorldspaceProjectLoaded;
        }

        private void OnWorldspaceProjectLoaded(WorldspaceProject project)
        {
            PerformTestsForWorldspaceProject(project);

            _mainEditor.UnloadLoadedWorldspaceProject();
        }

        private void PerformTestsForWorldspaceProject(WorldspaceProject project)
        {
            throw new NotImplementedException();
        }
    }
}
