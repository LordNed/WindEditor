using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor.Components.DefaultComponents;
using WindViewer.Editor.Renderer;
using WindViewer.Editor.Tools;

namespace WindViewer.Editor
{
    /// <summary>
    /// The EditorCore handles most of the actual editor logic. The MainForm will just feed
    /// appropriate events back into this which should allow us to make the UI and editor less
    /// coupled in the future.
    /// </summary>
    public class EditorCore : Singleton<EditorCore>
    {
        
        private List<BaseRenderer> _renderList;
        private List<IEditorTool> _editorToolList;
        private List<Camera> _cameraList;
        
        /// <summary> Used to calculate the delta time of each processed frame. </summary>
        private Stopwatch _dtStopWatch;

        private List<BaseComponent> _componentList;
        private List<EditorObject> _editorObjectList; 

        public EditorCore()
        {
            _renderList = new List<BaseRenderer>();
            _editorToolList = new List<IEditorTool>();
            _cameraList = new List<Camera>();
            _dtStopWatch = new Stopwatch();
            _componentList = new List<BaseComponent>();
            _editorObjectList = new List<EditorObject>();

            var dbgRender = new DebugRenderer();

            // Add our default renderers
            _renderList.Add(new J3DRenderer());
            _renderList.Add(dbgRender);

            // Add the default tools
            _editorToolList.Add(dbgRender);

            // Initialize the default renderers
            foreach(var renderer in _renderList)
                renderer.Initialize();

            // Add a default camera
            Console.WriteLine("Starting camera.");
            EditorObject cameraObj = new EditorObject();
            var camera = cameraObj.AddComponent<Camera>();
            camera.Rect = new Rect(1, 1, 0, 0);
            camera.ClearColor = Color.DodgerBlue;
            RegisterCamera(camera);

            RegisterComponent(cameraObj.AddComponent<FPSCameraMovement>());
            Console.WriteLine("end Camera");
        }

        /// <summary>
        /// This is the main editor application loop. This should
        /// be called each frame and will handle input, rendering
        /// etc.
        /// </summary>
        public void ProcessFrame()
        {
            // Calculate a new DeltaTime for this frame (time it took the last frame to render).
            Time.Internal_UpdateTime(_dtStopWatch.ElapsedMilliseconds/1000f);
            _dtStopWatch.Restart();

            // Calculate the input for this frame (calculate if a button was clicked/released/held, etc.)
            Input.Internal_UpdateInputState();

            // Update all registered tools
            foreach (IEditorTool tool in _editorToolList)
                tool.Update();

            // Update all components
            foreach(BaseComponent component in _componentList)
                component.Update();

            // Now draw each camera
            GL.Enable(EnableCap.ScissorTest);
            foreach (var camera in _cameraList)
            {
                GL.Viewport((int) (camera.Rect.X*Display.Width), (int) (camera.Rect.Y*Display.Height), camera.PixelWidth,
                            camera.PixelHeight);
                GL.Scissor((int) (camera.Rect.X*Display.Width), (int) (camera.Rect.Y*Display.Height), camera.PixelWidth,
                           camera.PixelHeight);


                //Actual render stuff
                GL.ClearColor(camera.ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


                // Render each renderer with this camera.
                foreach (var renderer in _renderList)
                    renderer.Render(camera);
            }
            GL.Disable(EnableCap.ScissorTest);

            // Perform late update on all registered tools.
            foreach (IEditorTool tool in _editorToolList)
                tool.PostRenderUpdate();

            DebugRenderer.DrawWireCube(Vector3.Zero, Color.DarkRed, Quaternion.Identity, new Vector3(10, 10, 10));
        }

        public void RegisterComponent(BaseComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component", "Cannot register a null component!");

            if (_componentList.Contains(component))
                throw new Exception(string.Format("Component list already contains component {0}!", component));

            _componentList.Add(component);

            if (!_editorObjectList.Contains(component.editorObject))
                _editorObjectList.Add(component.editorObject);
        }

        public void RegisterCamera(Camera camera)
        {
            _cameraList.Add(camera);
        }

        /// <summary>
        /// Wrapper around the Input system.
        /// </summary>
        /// <param name="keyCode">Keycode of the key that was pressed.</param>
        /// <param name="bPressed">If the key was pressed down or released.</param>
        public void InputSetKeyState(Keys keyCode, bool bPressed)
        {
            Input.Internal_SetKeyState(keyCode, bPressed);
        }

        /// <summary>
        /// Wrapper around the Input system.
        /// </summary>
        /// <param name="button">MouseButton of the pressed button. Supports
        /// LMB, RMB, MMB.</param>
        /// <param name="bPressed">Whether or not the button was pressed.</param>
        public void InputSetMouseBtnState(MouseButtons button, bool bPressed)
        {
            Input.Internal_SetMouseBtnState(button, bPressed);
        }

        /// <summary>
        /// Wrapper around the Input system.
        /// </summary>
        /// <param name="mousePos">Position of mouse in pixels relative to render.</param>
        public void InputSetMousePos(Vector2 mousePos)
        {
            Input.Internal_SetMousePos(mousePos);
        }
    }
}