using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FolderSelect;
using JWC;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Math;
using WindViewer.Editor;
using WindViewer.Editor.Renderer;
using WindViewer.Editor.Tools;
using WindViewer.Forms.Dialogs;
using WindViewer.src.Forms;

namespace WindViewer.Forms
{
    public partial class MainEditor : Form
    {
        //Currently loaded Worldspace Project. Null if no project loaded.
        private WorldspaceProject _loadedWorldspaceProject;

        private Camera _camera;
        private Camera _camera2;

        //Rendering stuffs
        //ToDo: Listify
        private J3DRenderer _renderer;
        private DebugRenderer _debugRenderer;
        private List<Camera> _cameras;

        private List<IEditorTool> _editorTools;


        //Events
        public static event Action<WorldspaceProject> WorldspaceProjectLoaded;

        //Misc
        private MruStripMenu _mruMenu;
        private string _mruRegKey = "SOFTWARE\\Wind Viewer";
        private bool _glControlInitalized;

        //Framerate Independent Camera Movement
        public static float DeltaTime;
        public static float Time;

        public MainEditor()
        {
            //Initialize the WinForm
            InitializeComponent();
            KeyPreview = true;

            _mruMenu = new MruStripMenu(mruList, OnMruClickedHandler, _mruRegKey + "\\MRU", 6);

            //Register some event handlers instead of using the designer because they point to a different class.
            glControl.Paint += this.glControl_Paint;
            glControl.KeyDown += Input.Internal_EventKeyDown;
            glControl.KeyUp += Input.Internal_EventKeyUp;
            glControl.MouseDown += Input.Internal_EventMouseDown;
            glControl.MouseMove += Input.Internal_EventMouseMove;
            glControl.MouseUp += Input.Internal_EventMouseUp;
            glControl.Resize += Display.Internal_EventResize;
        }

        private void MainEditor_Load(object sender, EventArgs e)
        {
            _loadedWorldspaceProject = null;

            _camera = new Camera();
            _camera.ClearColor = Color.DarkSlateGray;
            //_camera.Transform.Position = new Vector3(500, 500, 500);
            //_camera.Transform.LookAt(Vector3.Zero);
            _cameras = new List<Camera>();
            _cameras.Add(_camera);

            _editorTools = new List<IEditorTool>();
            _editorTools.Add(new MovementGizmo());

            //Add our renderers to the list 
            _renderer = new J3DRenderer();
            _renderer.Initialize();

            _debugRenderer = new DebugRenderer();
            _debugRenderer.Initialize();
            _editorTools.Add(_debugRenderer);

            _glControlInitalized = true;
        }


        #region GLControl
        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl.IsIdle)
            {
                RenderFrame();
            }
        }

        void glControl_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        void glControl_Paint(object sender, PaintEventArgs e)
        {
            RenderFrame();
        }

        #endregion

        #region Toolstrip Callbacks
        public void OpenFileFromWorkingDir(string workDir, bool surpressMRU = false)
        {
            //Iterate through the sub folders (dzb, dzr, bdl, etc.) and construct an appropriate data
            //structure for each one out of it. Then stick them all in a WorldspaceProject and save that
            //into our list of open projects. Then we can operate out of the WorldspaceProject references
            //and save and stuff.

            //Scan loaded projects to make sure we haven't already loaded it.
            if (_loadedWorldspaceProject != null)
            {
                Console.WriteLine("Trying to open new worldspacedir while one is open. Unloading!");
                UnloadLoadedWorldspaceProject();
            }
            toolStripStatusLabel1.Text = "Loading Worldspace Project...";
            saveAllToolStripMenuItem.Enabled = true;
            exportArchivesToolStripMenuItem.Enabled = true;
            unloadWorldspaceProjectToolStripMenuItem.Enabled = true;

            _loadedWorldspaceProject = new Editor.WindWaker.Project();
            _loadedWorldspaceProject.LoadFromDirectory(workDir);

            UpdateProjectFolderTreeview();

            if (!surpressMRU)
                _mruMenu.AddFile(_loadedWorldspaceProject.ProjectFilePath);

            if (WorldspaceProjectLoaded != null)
                WorldspaceProjectLoaded(_loadedWorldspaceProject);
        }

        /// <summary>
        /// Callback handler for opening an existing project.
        /// </summary>
        private void openWorldspaceDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderSelectDialog ofd = new FolderSelectDialog();
            ofd.Title = "Navigate to a folder that ends in .wrkDir";

            string workingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
            ofd.InitialDirectory = workingDir;

            if (ofd.ShowDialog(this.Handle))
            {
                //Ensure that the selected directory ends in ".wrkDir". If it doesn't, I don't want to figure out what happens.
                if (ofd.FileName.EndsWith(".wrkDir"))
                {
                    OpenFileFromWorkingDir(ofd.FileName);
                }
                else
                {
                    Console.WriteLine("Error: Select a folder that ends in .wrkDir!");
                }
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_loadedWorldspaceProject != null)
                _loadedWorldspaceProject.SaveAllArchives();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnMruClickedHandler(int number, string filename)
        {
            _mruMenu.SetFirstFile(number);

            if (Directory.Exists(filename))
            {
                OpenFileFromWorkingDir(filename);
            }
            else
            {
                MessageBox.Show("Selected file not found, removing from list.", "Missing File", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _mruMenu.RemoveFile(filename);
            }
        }

        /// <summary>
        /// Open the link to our Wiki which has more information about file formats, their usages, etc.
        /// Launches the default web browser on the users computer.
        /// </summary>
        private void wikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/LordNed/WindEditor/wiki");
        }

        private void issueTrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/LordNed/WindEditor/issues");
        }

        /// <summary>
        /// Opens a Utility for converting Big-Endian floats from Hexadecimal to Float and back.
        /// </summary>
        private void floatConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FloatConverter popup = new FloatConverter();
            popup.Show(this);
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*PreferencesWindow popup = new PreferencesWindow();
            popup.Show(this);*/
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow popup = new AboutWindow();
            popup.Show();
        }
        #endregion

        void RenderFrame()
        {
            if (!_glControlInitalized)
                return;

            DeltaTime = Program.DeltaTimeStopwatch.Elapsed.Milliseconds / 1000f;
            Time += DeltaTime;
            Program.DeltaTimeStopwatch.Restart();

            foreach (IEditorTool tool in _editorTools)
            {
                tool.Update();
            }

            DebugRenderer.DrawWireCube(Vector3.Zero, Color.DarkRed, Quaternion.Identity, new Vector3(10, 10, 10));

            GL.Enable(EnableCap.ScissorTest);
            foreach (var camera in _cameras)
            {

                GL.Viewport((int)(camera.Rect.X * Display.Width), (int)(camera.Rect.Y * Display.Height), camera.PixelWidth, camera.PixelHeight);
                GL.Scissor((int)(camera.Rect.X * Display.Width), (int)(camera.Rect.Y * Display.Height), camera.PixelWidth, camera.PixelHeight);
                

                //Actual render stuff
                GL.ClearColor(camera.ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                

                //ToDo: Put these in a list...
                _renderer.Render(camera);
                _debugRenderer.Render(camera);

                
            }
            GL.Disable(EnableCap.ScissorTest);

            foreach (IEditorTool tool in _editorTools)
            {
                tool.PostRenderUpdate();
            }


            //ToDo: This should be moved inside the camera, camera should be an IEditorTool
            if (Input.GetKey(Keys.W))
                _camera.Move(0f, 0f, 1);
            if (Input.GetKey(Keys.S))
                _camera.Move(0f, 0f, -1);
            if (Input.GetKey(Keys.A))
                _camera.Move(1, 0f, 0f);
            if (Input.GetKey(Keys.D))
                _camera.Move(-1, 0f, 0f);

            if (Input.GetMouseButton(1))
            {
                _camera.Rotate(Input.MouseDelta.X, Input.MouseDelta.Y);
            }

            if (Input.GetKeyDown(Keys.Q))
            {
                Ray mouseRay = _camera.ViewportPointToRay(Input.MousePosition);
                float distance;
                Vector3 point;

                bool bIntersects = Physics.RayVsPlane(mouseRay, new Plane(Vector3.Zero, Vector3.UnitY),
                    out distance, out point);

                Console.WriteLine("Intersects: {0}, Distance: {1} At: {2}", bIntersects, distance , point);

            }

            Input.Internal_UpdateInputState();
            glControl.SwapBuffers();
        }

        private void UpdateProjectFolderTreeview()
        {
            ProjectTreeview.BeginUpdate();
            ProjectTreeview.SuspendLayout();
            ProjectTreeview.Nodes.Clear();

            //Early out if the worldspace project is null (ie: We just unloaded the project)
            if (_loadedWorldspaceProject == null)
            {
                ProjectTreeview.EndUpdate();
                ProjectTreeview.ResumeLayout();
                return;
            }

            foreach (ZArchive archive in _loadedWorldspaceProject.GetAllArchives())
            {
                TreeNode arcRoot = ProjectTreeview.Nodes.Add(archive.Name, archive.Name);
                foreach (BaseArchiveFile archiveFile in archive.GetAllFiles())
                {
                    //Multiple files can share the same folder, so either find a node with the existing folder name or make a new one if it doesn't exist yet.
                    TreeNode folderNode = !arcRoot.Nodes.ContainsKey(archiveFile.FolderName) ? arcRoot.Nodes.Add(archiveFile.FolderName, archiveFile.FolderName) : arcRoot.Nodes.Find(archiveFile.FolderName, false)[0];

                    TreeNode fileName = folderNode.Nodes.Add(archiveFile.FileName);
                    fileName.Tag = archiveFile; //Store a reference to the archive file so we can get it later.
                }
            }

            ProjectTreeview.ResumeLayout();
            ProjectTreeview.EndUpdate();
        }



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save our MRU File List
            _mruMenu.SaveToRegistry(_mruRegKey + "\\MRU");

            //ToDo: Ask if the user wants to save when closing the application.
        }

        private void newFromArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] filePaths = EditorHelpers.ShowOpenFileDialog("Wind Waker Archives (*.arc; *.rarc)|*.arc; *.rarc|All Files (*.*)|*.*", true);

            //A canceled OFD returns an empty array.
            if (filePaths.Length == 0)
                return;

            //A canceled CWDRFA returns an empty string
            string workDir = CreateWorkingDirFromArchive(filePaths);
            if (workDir == string.Empty)
                return;

            OpenFileFromWorkingDir(workDir);
        }

        /// <summary>
        /// This creates a new "Working Dir" for a project (ie: "My Documents\Wind Viewer\MiniHyo.wrkDir"). It is the equivelent
        /// of setting up a project directory for new files. 
        /// </summary>
        /// <param name="archiveFilePaths">Archive to use as the base content to place in the WrkDir.</param>
        /// <returns></returns>
        public static string CreateWorkingDirFromArchive(string[] archiveFilePaths, string workDirName = "", string outputFolder = "")
        {
            //We're going to extract each file to the working directory.
            string workingDir = outputFolder;
            if (outputFolder == "")
            {
                outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                 Application.ProductName);
            }

            //Instead of guessing at what map this was originally, we're going to just ask the user
            //because the original names are in Romaji and they probably want their working folders
            //to be in English.
            string worldspaceName = workDirName;
            if (workDirName == "")
            {
                NewWorldspaceDialog dialog = new NewWorldspaceDialog();

                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.Cancel)
                    return string.Empty;

                worldspaceName = dialog.dirName.Text;
            }




            workingDir = Path.Combine(outputFolder, worldspaceName + ".wrkDir");
            foreach (string filePath in archiveFilePaths)
            {
                string arcExtractorFileName = Path.Combine(Application.StartupPath, "ExternalTools/arcExtract.exe");
                string folderName = Path.Combine(workingDir, Path.GetFileNameWithoutExtension(filePath));

                //ArcExtractor can't seem to output to a specified location, so we're going to cheat
                //here, and copy the arc over, then extract it, and then delete it. Yay!
                string newFileName = Path.Combine(folderName, Path.GetFileName(filePath));
                Directory.CreateDirectory(folderName);
                File.Copy(filePath, newFileName);


                Console.WriteLine("Invoking external tool arcExtract on {0}", filePath);
                ProcessStartInfo arcExtract = new ProcessStartInfo(arcExtractorFileName);
                arcExtract.WorkingDirectory = folderName;
                arcExtract.WindowStyle = ProcessWindowStyle.Hidden;
                arcExtract.Arguments = "\"" + newFileName + "\"";
                Process.Start(arcExtract);
            }

            //HackHack: The process does funny things and if we delete the file
            //immediately it doesn't extract + it never raises Exited events properly.
            System.Threading.Thread.Sleep(1000);


            //Delete the falsely copied archives
            foreach (string filePath in archiveFilePaths)
            {
                string folderName = Path.Combine(workingDir, Path.GetFileNameWithoutExtension(filePath));
                string newFileName = Path.Combine(folderName, Path.GetFileName(filePath));
                File.Delete(newFileName);
            }

            return workingDir;
        }

        private void exportArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportArchivesForWorldspaceProject();
        }


        public void UnloadLoadedWorldspaceProject()
        {
            _loadedWorldspaceProject = null;
            _renderer.OnSceneUnload();
            UpdateProjectFolderTreeview();
        }


        private void ExportArchivesForWorldspaceProject()
        {
            string[] archiveFilePaths = Directory.GetDirectories(_loadedWorldspaceProject.ProjectFilePath);
            foreach (string filePath in archiveFilePaths)
            {
                string arcPackerFileName = Path.Combine(Application.StartupPath, "ExternalTools/arcPack.exe");
                string yazCompressFileName = Path.Combine(Application.StartupPath, "ExternalTools/yaz0enc.exe");

                //We're going to archive all of the folders in the project dir, under the assumption that people
                //haven't been fucking around with them manually. >:|
                Console.WriteLine("Invoking external tool arcPack on {0}", filePath);
                ProcessStartInfo arcPack = new ProcessStartInfo(arcPackerFileName);
                arcPack.WindowStyle = ProcessWindowStyle.Hidden;
                arcPack.Arguments = "\"" + filePath + "\"";
                Process.Start(arcPack);


                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (fileName.ToLower() == "stage")
                {
                    System.Threading.Thread.Sleep(100);
                    Console.WriteLine("Invoking external tool yaz0enc on {0}", filePath);
                    ProcessStartInfo yazCompress = new ProcessStartInfo(yazCompressFileName);
                    yazCompress.WindowStyle = ProcessWindowStyle.Hidden;
                    yazCompress.Arguments = "\"" + filePath + ".arc\"";
                    Process.Start(yazCompress);
                }
            }

            //Wait for all of the archiving processes to finish.
            Process[] archiveProcesses = Process.GetProcessesByName("arcPack");
            if (archiveProcesses.Length > 0)
            {
                Console.WriteLine("Archives still being created, waiting...");
                while (archiveProcesses.Length > 0)
                {
                    System.Threading.Thread.Sleep(100);
                    archiveProcesses = Process.GetProcessesByName("arcPack");
                }
                Console.WriteLine("arcPack processes complete.");
            }


            //We're going to delete the old Stage.arc and rename the Yaz0 compressed one for easier/less confusing inclusion.
            string stageFilePath = Path.Combine(_loadedWorldspaceProject.ProjectFilePath, "Stage.arc");
            string compressedStageFilePath = stageFilePath + ".yaz0";
            if (File.Exists(stageFilePath) && File.Exists(compressedStageFilePath))
            {
                Process[] processes = Process.GetProcessesByName("yaz0enc");
                if (processes.Length > 0)
                {
                    Console.WriteLine("Stage.arc is still being compressed, waiting...");
                    while (!processes[0].HasExited)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    Console.WriteLine("yaz0enc compression complete. Replacing Stage.arc with compressed file.");
                }

                FileInfo fInfo = new FileInfo(stageFilePath);
                fInfo.Replace(compressedStageFilePath, null);
            }

            Console.WriteLine("Project Export completed.");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            UnloadLoadedWorldspaceProject();

            unloadWorldspaceProjectToolStripMenuItem.Enabled = false;
        }

        private void automatedTestSuiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutomatedTestingSuite ats = new AutomatedTestingSuite(this);
            ats.Show();
        }
    }
}
