using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Math;
using WindViewer.Editor.Renderer;
using WindViewer.Editor.Tools;

namespace WindViewer.Editor
{
    /// <summary>
    /// The EditorCore handles most of the actual editor logic. The MainForm will just feed
    /// appropriate events back into this which should allow us to make the UI and editor less
    /// coupled in the future.
    /// </summary>
    public class EditorCore
    {
        private List<BaseRenderer> _renderList;
        private List<IEditorTool> _editorToolList;
        private List<Camera> _cameraList;


        /// <summary> Used to calculate the delta time of each processed frame. </summary>
        private Stopwatch _dtStopWatch;

        public EditorCore()
        {
            _renderList = new List<BaseRenderer>();
            _editorToolList = new List<IEditorTool>();
            _cameraList = new List<Camera>();
            _dtStopWatch = new Stopwatch();

            var dbgRender = new DebugRenderer();

            // Add our default renderers
            _renderList.Add(new J3DRenderer());
            _renderList.Add(dbgRender);

            // Add the default tools
            _editorToolList.Add(dbgRender);

            // Add a default camera
            _cameraList.Add(new Camera());

            // Initialize the default renderers
            foreach(var renderer in _renderList)
                renderer.Initialize();
        }

        /// <summary>
        /// This is the main editor application loop. This should
        /// be called each frame and will handle input, rendering
        /// etc.
        /// </summary>
        public void ProcessFrame()
        {
            // Calculate a new DeltaTime for this frame (time it took the last frame to render).
            Time.Internal_SetDeltaTime(_dtStopWatch.ElapsedMilliseconds/1000f);
            Time.Internal_SetTimeSinceStart(Time.TimeSinceStart + Time.DeltaTime);
            _dtStopWatch.Restart();

            // Calculate the input for this frame (calculate if a button was clicked/released/held, etc.)
            Input.Internal_UpdateInputState();

            // Update all registered tools
            foreach(IEditorTool tool in _editorToolList)
                tool.Update();

            // Now draw each camera
            GL.Enable(EnableCap.ScissorTest);
            foreach (var camera in _cameraList)
            {
                GL.Viewport((int)(camera.Rect.X * Display.Width), (int)(camera.Rect.Y * Display.Height), camera.PixelWidth, camera.PixelHeight);
                GL.Scissor((int)(camera.Rect.X * Display.Width), (int)(camera.Rect.Y * Display.Height), camera.PixelWidth, camera.PixelHeight);


                //Actual render stuff
                GL.ClearColor(camera.ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


                // Render each renderer with this camera.
                foreach (var renderer in _renderList)
                    renderer.Render(camera);


                // temp...
                if (Input.GetKey(Keys.W))
                    camera.Move(0f, 0f, 1);
                if (Input.GetKey(Keys.S))
                    camera.Move(0f, 0f, -1);
                if (Input.GetKey(Keys.A))
                    camera.Move(1, 0f, 0f);
                if (Input.GetKey(Keys.D))
                    camera.Move(-1, 0f, 0f);

                if (Input.GetMouseButton(1))
                {
                    camera.Rotate(Input.MouseDelta.X, Input.MouseDelta.Y);
                }

                if (Input.GetKeyDown(Keys.Q))
                {
                    Ray mouseRay = camera.ViewportPointToRay(Input.MousePosition);
                    float distance;
                    Vector3 point;

                    bool bIntersects = Physics.RayVsPlane(mouseRay, new Plane(Vector3.Zero, Vector3.UnitY),
                        out distance, out point);

                    DebugRenderer.DrawLine(mouseRay.Origin, mouseRay.Origin + mouseRay.Direction * 250f);
                    Console.WriteLine("Intersects: {0}, Distance: {1} At: {2}", bIntersects, distance, point);

                }
            }
            GL.Disable(EnableCap.ScissorTest);

            // Perform late update on all registered tools.
            foreach(IEditorTool tool in _editorToolList)
                tool.PostRenderUpdate();

            DebugRenderer.DrawWireCube(Vector3.Zero, Color.DarkRed, Quaternion.Identity, new Vector3(10, 10, 10));
        }
    }
}