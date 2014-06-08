using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FolderSelect;
using JWC;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor;
using WindViewer.Editor.Renderer;
using WindViewer.Editor.Tools;
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

        private Camera _camera;

        //Rendering stuffs
        private List<IRenderer> _renderers;
        private IRenderer _collisionRenderer;

        //Editor stuffs
        private List<IEditorTool> _editorTools; 

        //Events
        public static event Action<WindWakerEntityData> SelectedEntityFileChanged;
        public static event Action<WindWakerEntityData.BaseChunk> SelectedEntityChanged;
        public static event Action<WorldspaceProject> WorldspaceProjectLoaded;

        //Misc
        private MruStripMenu _mruMenu;
        private string _mruRegKey = "SOFTWARE\\Wind Viewer";
        private bool _glControlInitalized;

        //Framerate Independent Camera Movement
        public static float DeltaTime;

        public MainEditor()
        {
            //Initialize the WinForm
            InitializeComponent();

            _mruMenu = new MruStripMenu(mruList, OnMruClickedHandler, _mruRegKey + "\\MRU", 6);

            //Editor fails to auto-center on primary screen for some users.
            //CenterToScreen();

            SelectedEntityFileChanged += delegate(WindWakerEntityData data)
            {
                Console.WriteLine("Changing Ent file...");
            };
        }

        private void MainEditor_Load(object sender, EventArgs e)
        {
            _loadedWorldspaceProject = null;

            _camera = new Camera();
            _renderers = new List<IRenderer>();
            _editorTools = new List<IEditorTool>();


            //Add our renderers to the list 
            _collisionRenderer = new GLRenderer();
            DebugRenderer dbgRender = new DebugRenderer();
            _renderers.Add(_collisionRenderer);
            _renderers.Add(dbgRender);

            //Then add them to the tool list so they get updates
            _editorTools.Add(dbgRender);
            _editorTools.Add(_collisionRenderer);

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

        void glControl_Resize(object sender, EventArgs e)
        {
            if (!_glControlInitalized)
                return;

            glControl.Invalidate();
        }



        void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            EditorHelpers.KeysDown[e.KeyValue] = true;

           Console.WriteLine("cam pos: " + _camera.transform.Position);
        }

        void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            EditorHelpers.KeysDown[e.KeyValue] = false;
        }

        void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    EditorHelpers.MouseState.LDown = true;
                    break;
                case MouseButtons.Right:
                    EditorHelpers.MouseState.RDown = true;
                    break;
                case MouseButtons.Middle:
                    EditorHelpers.MouseState.MDown = true;
                    break;
            }

            EditorHelpers.MouseState.Center = new Vector2(e.X, e.Y);
        }

        void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 newMousePos = new Vector2(e.X, e.Y);
            Vector2 delta = newMousePos - EditorHelpers.MouseState.Center;
            EditorHelpers.MouseState.Center = newMousePos;

            EditorHelpers.MouseState.Delta = delta;

            if (EditorHelpers.MouseState.LDown)
            {
                _camera.Rotate(-delta.X, delta.Y);
            }
        }

        void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    EditorHelpers.MouseState.LDown = false;
                    break;
                case MouseButtons.Right:
                    EditorHelpers.MouseState.RDown = false;
                    break;
                case MouseButtons.Middle:
                    EditorHelpers.MouseState.MDown = false;
                    break;
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
                throw new Exception(
                    "Tried to load second WorldspaceProject. Unloading of the first one isn't implemented yet!");
            }
            toolStripStatusLabel1.Text = "Loading Worldspace Project...";
            saveAllToolStripMenuItem.Enabled = true;
            exportArchivesToolStripMenuItem.Enabled = true;
            unloadWorldspaceProjectToolStripMenuItem.Enabled = true;

            _loadedWorldspaceProject = new WorldspaceProject();
            _loadedWorldspaceProject.LoadFromDirectory(workDir);
            
            UpdateProjectFolderTreeview();

            if(!surpressMRU)
                _mruMenu.AddFile(_loadedWorldspaceProject.ProjectFilePath);

            //Temp
            foreach (ZArchive archive in _loadedWorldspaceProject.GetAllArchives())
            {
                StaticCollisionModel scm = archive.GetFileByType<StaticCollisionModel>();
                if(scm!=null)
                    _collisionRenderer.AddRenderable(scm.Renderable);
            }

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
            if(_loadedWorldspaceProject != null)
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
            Program.DeltaTimeStopwatch.Restart();
            toolStripStatusLabel1.Text = (1 / DeltaTime).ToString("00") + " fps.";

            foreach (IEditorTool tool in _editorTools)
            {
                tool.PreUpdate();
            }

            //Hack...
            if (_loadedWorldspaceProject != null)
            {
                
                foreach (var archive in _loadedWorldspaceProject.GetAllArchives())
                {
                    WindWakerEntityData entData = archive.GetFileByType<WindWakerEntityData>();
                    if (entData != null)
                    {
                        foreach (var kvp in entData.GetAllChunks())
                        {
                            foreach (WindWakerEntityData.BaseChunk chunk in kvp.Value)
                            {
                                var spatial = chunk as WindWakerEntityData.BaseChunkSpatial;
                                if (spatial != null)
                                {
                                    Vector3 flippedZ = spatial.Transform.Position;
                                    flippedZ.X = -flippedZ.X;
                                    flippedZ.Z = -flippedZ.Z;
                                    DebugRenderer.DrawWireCube(flippedZ);
                                }
                            }
                        }
                    }
                }
            }

            foreach (IEditorTool tool in _editorTools)
            {
                tool.LateUpdate();
            }


            //Actual render stuff
            GL.ClearColor(Color.LightSeaGreen);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            foreach (IRenderer renderer in _renderers)
            {
                renderer.Render(_camera, (float)glControl.Width / (float)glControl.Height);
            }


            if (EditorHelpers.KeysDown[(int)Keys.W])
                _camera.Move(0f, 0f, 1);
            if (EditorHelpers.KeysDown[(int)Keys.S])
                _camera.Move(0f, 0f, -1);
            if (EditorHelpers.KeysDown[(int)Keys.A])
                _camera.Move(1, 0f, 0f);
            if (EditorHelpers.KeysDown[(int)Keys.D])
                _camera.Move(-1, 0f, 0f);

            glControl.SwapBuffers();
        }


        private void UpdateEntityTreeview()
        {
            Stopwatch timer = Stopwatch.StartNew();
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
                    if(chunk.ChunkLayer != EditorHelpers.EntityLayer.DefaultLayer && chunk.ChunkLayer != _selectedEntityLayer)
                        continue;

                    TreeNode curParentNode = topLevelNode;

                    //If it's a non-default layer we want to put them under a different TLN
                    if (chunk.ChunkLayer != EditorHelpers.EntityLayer.DefaultLayer)
                    {
                        if (topLevelNodeLayer == null)
                            topLevelNodeLayer = EntityTreeview.Nodes.Add("ChunkHeaderLayer");
                        topLevelNodeLayer.Text = "[" + chunk.ChunkName.ToUpper() + "] " + chunk.ChunkDescription + " [" + EditorHelpers.LayerIdToString(chunk.ChunkLayer)+ "]";
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
                            displayName = (string)field.GetValue(chunk);
                        }
                    }

                    TreeNode newNode = curParentNode.Nodes.Add("[" + i + "] " + displayName);
                    newNode.Tag = chunk;


                    if(chunk.ChunkLayer != EditorHelpers.EntityLayer.DefaultLayer)
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
            Console.WriteLine("Updating ETV took: " + timer.Elapsed);
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
                var entData = (WindWakerEntityData) e.Node.Tag;
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
                    if(validLayers.Contains(chunk.ChunkLayer))
                        continue;

                    validLayers.Add(chunk.ChunkLayer);
                }
            }

            for(int i = validLayers.Count-1; i >= 0; i--)
            {
                LayersListBox.Items.Add(EditorHelpers.LayerIdToString(validLayers[i]));
            }

            //Select the Default layer by uh... default.
            if(LayersListBox.Items.Count > 0)
                LayersListBox.SetSelected(LayersListBox.Items.Count-1, true);

            LayersListBox.ResumeLayout();
            LayersListBox.EndUpdate();
        }

        private void LayersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedEntityLayer = EditorHelpers.ConvertStringToLayerId((string)((ListBox) sender).SelectedItem);
            UpdateEntityTreeview();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save our MRU File List
            _mruMenu.SaveToRegistry(_mruRegKey + "\\MRU");

            //ToDo: Ask if the user wants to save.
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
                WindWakerEntityData.PlyrChunk plyr = chunk as WindWakerEntityData.PlyrChunk;;

                if (chunk.GetType().IsDefined(typeof(EntEditorType), true))
                {
                    Console.WriteLine("Yes!");
                    editorType = (EntEditorType) chunk.GetType().GetCustomAttributes(typeof (EntEditorType), false)[0];
                }

                Type editType = null;
                if (editorType == null)
                {
                    editType = typeof (UnsupportedEntity);
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

                WindWakerEntityData.BaseChunk chunk = (WindWakerEntityData.BaseChunk) contextEntityTreeRoot.Tag;
                chunk.WriteData(stream);

                fs.Flush();
                fs.Close();
            }
        }

        private void newFromArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] filePaths = EditorHelpers.ShowOpenFileDialog("Wind Waker Archives (*.arc; *.rarc)|*.arc; *.rarc|All Files (*.*)|*.*", true);

            //A canceled OFD returns an empty array.
            if(filePaths.Length == 0)
                return;

            //A canceled CWDRFA returns an empty string
            string workDir = CreateWorkingDirFromArchive(filePaths);
            if (workDir == string.Empty)
                return;

            OpenFileFromWorkingDir(workDir);
        }

        // <summary>
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
            //Clear our Renderers
            foreach (var renderer in _renderers)
            {
                renderer.ClearRenderableList();
            }

            //Then unload the worldspace project
            _loadedWorldspaceProject = null;
            _selectedEntityFile = null;
            _selectedEntityLayer = EditorHelpers.EntityLayer.DefaultLayer;
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
                    Console.WriteLine("Invoking external tool yaz0enc on {0}", filePath);
                    ProcessStartInfo yazCompress = new ProcessStartInfo(yazCompressFileName);
                    yazCompress.WindowStyle = ProcessWindowStyle.Hidden;
                    yazCompress.Arguments = "\"" + filePath + ".arc\"";
                    Process.Start(yazCompress);
                }
            }

            //HackHack: The process does funny things and if we move the file
            //immediately it doesn't extract + it never raises Exited events properly.
            System.Threading.Thread.Sleep(100);

            //We're going to delete the old Stage.arc and rename the Yaz0 compressed one for easier/less confusing inclusion.
            string stageFilePath = Path.Combine(_loadedWorldspaceProject.ProjectFilePath, "Stage.arc");
            string compressedStageFilePath = stageFilePath + ".yaz0";
            if (File.Exists(stageFilePath) && File.Exists(compressedStageFilePath))
            {
                File.Delete(stageFilePath);
                File.Move(compressedStageFilePath, stageFilePath);
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
