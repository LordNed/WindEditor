using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using FolderSelect;
using JWC;
using OpenTK;
using WindViewer.Editor;
using WindViewer.FileFormats;
using WindViewer.Forms.Dialogs;
using WindViewer.Forms.EntityEditors;
using WindViewer.src.Forms;

namespace WindViewer.Forms
{
    public partial class MainEditor : Form
    {
        //Currently loaded Worldspace Project. Null if no project loaded.
        private WorldspaceProject _loadedWorldspaceProject;

        //Currently selected Entity data file for Worldspace Project. Null if none selected.
        private WindWakerEntityData _selectedEntityFile;
        private EditorHelpers.EntityLayer _selectedEntityLayer;

        //Events
        public static event Action<WindWakerEntityData> SelectedEntityFileChanged;
        public static event Action<WindWakerEntityData.BaseChunk> SelectedEntityChanged;
        public static event Action<WorldspaceProject> WorldspaceProjectLoaded;

        //Misc
        private MruStripMenu _mruMenu;
        private string _mruRegKey = "SOFTWARE\\Wind Viewer";
        private bool _glControlInitalized;

        private EditorCore _editorCore;

        public MainEditor()
        {
            //Initialize the WinForm
            InitializeComponent();
            KeyPreview = true;
            
            _mruMenu = new MruStripMenu(mruList, OnMruClickedHandler, _mruRegKey + "\\MRU", 6);
            _loadedWorldspaceProject = null;

            // Register a handler for WorldspaceProjectLoaded that sets the Window's title.
            WorldspaceProjectLoaded += OnWorldSpaceProjectLoaded;

            glControl.KeyDown += HandleEventKeyDown;
            glControl.KeyUp += HandleEventKeyUp;
            glControl.MouseDown += HandleEventMouseDown;
            glControl.MouseMove += HandleEventMouseMove;
            glControl.MouseUp += HandleEventMouseUp;
            glControl.Resize += Display.Internal_EventResize;

            glControl.Load += (sender, args) =>
            {
                // Hook Application Idle to force rendering while no events are happening.
                Application.Idle += HandleApplicationIdle;

                Console.WriteLine("glContro loaded!");
                // Initialize our core once the glControl has been created as initalization
                // requires a glContext.
                _editorCore = new EditorCore();
            };

            // Hook the glControl's Paint function (which is called every frame thanks to above)
            glControl.Paint += (sender, args) => RenderFrame();
        }

        private void MainEditor_Load(object sender, EventArgs e)
        {
            // Check to see if they've set up user prefs before.
            if (string.IsNullOrEmpty(Properties.Settings.Default.rootDiskDir))
            {
                MessageBox.Show(
                    "Application paths have not been configured. Not all tools will function. Please configure via Tools->Options.",
                    "Please Set Editor Paths", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region Event Handlers

        private void HandleEventKeyDown(object sender, KeyEventArgs e)
        {
            if (_editorCore == null)
                return;

            _editorCore.InputSetKeyState(e.KeyCode, true);
        }

        private void HandleEventKeyUp(object sender, KeyEventArgs e)
        {
            if (_editorCore == null)
                return;

            _editorCore.InputSetKeyState(e.KeyCode, false);
        }

        private void HandleEventMouseDown(object sender, MouseEventArgs e)
        {
            if (_editorCore == null)
                return;

            _editorCore.InputSetMouseBtnState(e.Button, true);
        }

        private void HandleEventMouseMove(object sender, MouseEventArgs e)
        {
            if (_editorCore == null)
                return;

            _editorCore.InputSetMousePos(new Vector2(e.X, e.Y));
        }

        private void HandleEventMouseUp(object sender, MouseEventArgs e)
        {
            if (_editorCore == null)
                return;

            _editorCore.InputSetMouseBtnState(e.Button, false);
        }

        private void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (glControl.IsIdle)
            {
                RenderFrame();
            }
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

            _loadedWorldspaceProject = new WorldspaceProject();
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

            // ToDo: Ask the user if they want to save.
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow popup = new AboutWindow();
            popup.Show();
        }

        /// <summary>
        /// Opens a Utility for converting Big-Endian floats from Hexadecimal to Float and back.
        /// </summary>
        private void floatConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FloatConverter popup = new FloatConverter();
            popup.Show(this);
        }

        #endregion

        void RenderFrame()
        {
            if (_editorCore == null)
                return;

            _editorCore.ProcessFrame();
            glControl.SwapBuffers();
        }
        
        private void UpdateEntityTreeview()
        {
            toolStripStatusLabel1.Text = "Updating Entity Treeview...";

            EntityTreeview.SuspendLayout();
            EntityTreeview.BeginUpdate();
            EntityTreeview.Nodes.Clear();

            //Early out in case we just unloaded a project.
            if (_loadedWorldspaceProject == null || _selectedEntityFile == null)
            {
                EntityTreeview.ResumeLayout();
                EntityTreeview.EndUpdate();
                return;
            }

            foreach (var kvPair in _selectedEntityFile.GetAllChunks())
            {
                //This is the top-level grouping, ie: "Doors". We don't know the name yet though.
                TreeNode topLevelNode = EntityTreeview.Nodes.Add("ChunkHeader");
                TreeNode topLevelNodeLayer = null;
                int i = 0;

                foreach (var chunk in kvPair.Value)
                {
                    if (chunk.ChunkLayer != EditorHelpers.EntityLayer.DefaultLayer && chunk.ChunkLayer != _selectedEntityLayer)
                        continue;

                    TreeNode curParentNode = topLevelNode;

                    //If it's a non-default layer we want to put them under a different TLN
                    if (chunk.ChunkLayer != EditorHelpers.EntityLayer.DefaultLayer)
                    {
                        if (topLevelNodeLayer == null)
                            topLevelNodeLayer = EntityTreeview.Nodes.Add("ChunkHeaderLayer");
                        topLevelNodeLayer.Text = "[" + chunk.ChunkName.ToUpper() + "] " + chunk.ChunkDescription + " [" + EditorHelpers.LayerIdToString(chunk.ChunkLayer) + "]";
                        topLevelNodeLayer.BackColor = EditorHelpers.LayerIdToColor(chunk.ChunkLayer);
                        curParentNode = topLevelNodeLayer;
                    }
                    else
                    {
                        topLevelNode.Text = "[" + chunk.ChunkName.ToUpper() + "] " + chunk.ChunkDescription;
                    }

                    string displayName = string.Empty;
                    //Now generate the name for our current node. If it doesn't have a DisplayName attribute then we'll just
                    //use an index, otherwise we'll use the display name + index.
                    foreach (var field in chunk.GetType().GetFields())
                    {
                        DisplayName dispNameAttribute =
                            (DisplayName)Attribute.GetCustomAttribute(field, typeof(DisplayName));
                        if (dispNameAttribute != null)
                        {
                            var value = field.GetValue(chunk);
                            displayName = (value ?? "").ToString();
                        }
                    }

                    TreeNode newNode = curParentNode.Nodes.Add("[" + i + "] " + displayName);
                    newNode.Tag = chunk;


                    if (chunk.ChunkLayer != EditorHelpers.EntityLayer.DefaultLayer)
                        newNode.BackColor = EditorHelpers.LayerIdToColor(chunk.ChunkLayer);
                    i++;
                }

                //Some maps only have copies of things on sub-layers and not the main
                //layer, so we're going to remove the main layer one if it hasn't been
                //used.
                if (topLevelNode.Text == "ChunkHeader")
                    EntityTreeview.Nodes.Remove(topLevelNode);
            }

            EntityTreeview.EndUpdate();
            EntityTreeview.ResumeLayout();
            toolStripStatusLabel1.Text = "Completed.";
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

                    if (archiveFile is WindWakerEntityData && _selectedEntityFile == null)
                    {
                        _selectedEntityFile = (WindWakerEntityData)archiveFile;
                        UpdateLayersView(); //Updates the Entityview for us.

                        if (SelectedEntityFileChanged != null)
                            SelectedEntityFileChanged((WindWakerEntityData)archiveFile);
                    }
                }
            }

            ProjectTreeview.ResumeLayout();
            ProjectTreeview.EndUpdate();
        }

        private void ProjectTreeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!(e.Node.Tag is WindWakerEntityData))
                return;

            if (SelectedEntityFileChanged != null)
            {
                var entData = (WindWakerEntityData)e.Node.Tag;
                _selectedEntityFile = entData;


                UpdateLayersView(); //Updates the Entity view for us.
                SelectedEntityFileChanged(entData); //Broadcast event.
            }
        }



        private void UpdateLayersView()
        {
            LayersListBox.SuspendLayout();
            LayersListBox.BeginUpdate();
            LayersListBox.Items.Clear();

            //Early out if the worldspace project is null (ie: We just unloaded the project)
            if (_loadedWorldspaceProject == null)
            {
                LayersListBox.EndUpdate();
                LayersListBox.ResumeLayout();

                return;
            }

            WindWakerEntityData entData = _selectedEntityFile;
            List<EditorHelpers.EntityLayer> validLayers = new List<EditorHelpers.EntityLayer>();

            foreach (var kvPair in entData.GetAllChunks())
            {
                foreach (WindWakerEntityData.BaseChunk chunk in kvPair.Value)
                {
                    if (validLayers.Contains(chunk.ChunkLayer))
                        continue;

                    validLayers.Add(chunk.ChunkLayer);
                }
            }

            for (int i = validLayers.Count - 1; i >= 0; i--)
            {
                LayersListBox.Items.Add(EditorHelpers.LayerIdToString(validLayers[i]));
            }

            //Select the Default layer by uh... default.
            if (LayersListBox.Items.Count > 0)
                LayersListBox.SetSelected(LayersListBox.Items.Count - 1, true);

            LayersListBox.ResumeLayout();
            LayersListBox.EndUpdate();
        }

        private void LayersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedEntityLayer = EditorHelpers.ConvertStringToLayerId((string)((ListBox)sender).SelectedItem);
            UpdateEntityTreeview();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save our MRU File List
            _mruMenu.SaveToRegistry(_mruRegKey + "\\MRU");

            //ToDo: Ask if the user wants to save.
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
        private void exportArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportArchivesForWorldspaceProject();
        }
        private void unloadWorldspaceProject_Click(object sender, EventArgs e)
        {
            UnloadLoadedWorldspaceProject();

            unloadWorldspaceProjectToolStripMenuItem.Enabled = false;
        }

        private void automatedTestSuiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutomatedTestingSuite ats = new AutomatedTestingSuite(this);
            ats.Show();
        }
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var settings = new SettingsDialog();
            settings.Show();
        }

        private void EntityTreeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
            WindWakerEntityData.BaseChunk chunk = e.Node.Tag as WindWakerEntityData.BaseChunk;
            if (chunk != null)
            {
                //Temp
                e.Node.ContextMenuStrip = contextEntityTreeRoot;
                contextEntityTreeRoot.Tag = chunk;


                //Find the Editor Type attribute.
                EntEditorType editorType = null;

                if (chunk.GetType().IsDefined(typeof(EntEditorType), true))
                {
                    editorType = (EntEditorType)chunk.GetType().GetCustomAttributes(typeof(EntEditorType), false)[0];
                }

                Type editType = null;
                if (editorType == null)
                {
                    editType = typeof(UnsupportedEntity);
                }
                else
                {
                    editType = editorType.EditorType();
                }
                UserControl obj = Activator.CreateInstance(editType) as UserControl;


                obj.Dock = DockStyle.Fill;

                PropertiesBox.SuspendLayout();
                //Dispose of the control manually right now so that they un-register their event
                //handler to MainForm::SelectedEntityChanged
                foreach (Control control in PropertiesBox.Controls)
                {
                    control.Dispose();
                }
                PropertiesBox.Controls.Clear();
                PropertiesBox.Controls.Add(obj);
                PropertiesBox.ResumeLayout(true);

                if (editorType != null && MainSplitter.Panel2.Width < editorType.MinEditorWidth)
                    MainSplitter.SplitterDistance = MainSplitter.Width - editorType.MinEditorWidth;

                if (SelectedEntityChanged != null)
                    SelectedEntityChanged(chunk);
            }
        }

        private void exportChunksOfTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                BinaryWriter stream = new BinaryWriter(fs);

                WindWakerEntityData.BaseChunk chunk = (WindWakerEntityData.BaseChunk)contextEntityTreeRoot.Tag;
                chunk.WriteData(stream);

                fs.Flush();
                fs.Close();
            }
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

        

        public void UnloadLoadedWorldspaceProject()
        {
            _loadedWorldspaceProject = null;
            _selectedEntityFile = null;
            _selectedEntityLayer = EditorHelpers.EntityLayer.DefaultLayer;
            //_renderer.OnSceneUnload();
            UpdateProjectFolderTreeview();
            UpdateEntityTreeview();
            UpdateLayersView();
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

        
        private void OnWorldSpaceProjectLoaded(WorldspaceProject worldspaceProject)
        {
            // Modify the title of the WinForm UI to reflect the currently loaded Worldspace Project.
            Text = string.Format("Wind Editor ({0} - {1})", worldspaceProject.Name, worldspaceProject.ProjectFilePath);
        }


    }
}
