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
using WindViewer.FileFormats;

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
        private IRenderer _renderer;

        //Events
        public static event Action<WindWakerEntityData> SelectedEntityFileChanged;

        //Misc
        private MruStripMenu _mruMenu;
        private string _mruRegKey = "SOFTWARE\\Wind Viewer";
        private bool _glControlInitalized;

        public MainEditor()
        {
            //Initialize the WinForm
            InitializeComponent();

            _mruMenu = new MruStripMenu(mruList, OnMruClickedHandler, _mruRegKey + "\\MRU", 6);
        }

        private void TestLayout_Load(object sender, EventArgs e)
        {
            //Things we're keeping.
            _loadedWorldspaceProject = null;

            _camera = new Camera();
            _renderer = new GLRenderer();

            Cube cube1 = new Cube();
            Cube cube2 = new Cube();
            Cube cube3 = new Cube();
            Cube cube4 = new Cube();
            cube1.transform.Position = new Vector3(0, 0, -5);
            cube1.transform.Scale = new Vector3(0.25f, 0.5f, 0.25f);

            cube2.transform.Position = new Vector3(0, 0, -5);
            cube2.transform.Scale = new Vector3(0.25f, 0.5f, 0.25f);

            _renderer.AddRenderable(cube1);
            //_renderer.AddRenderable(cube2);

            //Test
            TestUserControl tcu = new TestUserControl();
            tcu.Dock = DockStyle.Fill;

            PropertiesBox.SuspendLayout();
            PropertiesBox.Controls.Add(tcu);
            PropertiesBox.ResumeLayout();

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
            switch (e.KeyCode)
            {
                case Keys.W:
                    _camera.Move(0f,  0f, 0.1f);
                    break;
                case Keys.S:
                    _camera.Move(0f, 0f, -0.1f);
                    break;
                case Keys.A:
                    _camera.Move(-0.1f, 0f, 0f);
                    break;
                case Keys.D:
                    _camera.Move(.1f, 0f, 0f);
                    break;
            }

           Console.WriteLine("cam pos: " + _camera.transform.Position);
        }

        void glControl_KeyUp(object sender, KeyEventArgs e)
        {
        }

        void glControl_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private Vector2 _lastMousePos;
        void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 newMousePos = new Vector2(e.X, e.Y);
            Vector2 delta = newMousePos - _lastMousePos;

            //_camera.Rotate(delta.X, delta.Y);
            _lastMousePos = newMousePos;
            //Console.WriteLine("x: " + e.X + " y: " + e.Y);
        }

        void glControl_MouseUp(object sender, MouseEventArgs e)
        {
        }

        void RenderFrame()
        {
            if (!_glControlInitalized)
                return;

            GL.ClearColor(Color.GreenYellow);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            _renderer.Render(_camera, (float)glControl.Width / (float)glControl.Height);

            glControl.SwapBuffers();
        }



        private void LoadShader(String fileName, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(fileName))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }



        #endregion

        #region Toolstrip Callbacks
        private void OpenFileFromWorkingDir(string workDir)
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
            toolStripStatusLabel1.Text = "Loading Worlspace Project...";
            saveAllToolStripMenuItem.Enabled = true;

            _loadedWorldspaceProject = new WorldspaceProject();
            _loadedWorldspaceProject.LoadFromDirectory(workDir);
            
            UpdateProjectFolderTreeview();

            _mruMenu.AddFile(_loadedWorldspaceProject.ProjectFilePath);
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
        #endregion


        private void UpdateEntityTreeview()
        {
            Stopwatch timer = Stopwatch.StartNew();
            toolStripStatusLabel1.Text = "Updating Entity Treeview...";

            EntityTreeview.SuspendLayout();
            EntityTreeview.BeginUpdate();
            EntityTreeview.Nodes.Clear();

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
                    if(chunk.ChunkLayer != EditorHelpers.EntityLayer.DefaultLayer)
                        newNode.BackColor = EditorHelpers.LayerIdToColor(chunk.ChunkLayer);
                    i++;
                }
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
    }
}
