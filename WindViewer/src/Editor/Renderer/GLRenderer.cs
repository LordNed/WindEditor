using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public class GLRenderer
    {
        //OpenTK stuff.
        private int _programId;
        private int _vertexShaderId;
        private int _fragmentShaderId;

        //OpenTK::Shader
        private int _attributeVpos;
        private int _uniformMVP;

        private List<RenderableObject> _renderableObjects;
        private GLControl _glControl;
        private Camera _camera;

        public GLRenderer(GLControl control, Camera camera)
        {
            _renderableObjects = new List<RenderableObject>();

            //Initialize our Shader
            _programId = GL.CreateProgram();
            LoadShader("src/shaders/vs.glsl", ShaderType.VertexShader, _programId, out _vertexShaderId);
            LoadShader("src/shaders/fs.glsl", ShaderType.FragmentShader, _programId, out _fragmentShaderId);

            GL.LinkProgram(_programId);
            Console.WriteLine(GL.GetProgramInfoLog(_programId));

            //Bind to the shader attributes
            _attributeVpos = GL.GetAttribLocation(_programId, "vPosition");
            _uniformMVP = GL.GetUniformLocation(_programId, "modelview");

            if (_attributeVpos == -1 || _uniformMVP == -1)
            {
                Console.WriteLine("Error binding attributes!");
            }

            _glControl = control;
            _camera = camera;
        }

        public void AddToRenderList(RenderableObject obj)
        {
            _renderableObjects.Add(obj);
            obj.UpdateBuffers(_attributeVpos); //BLEH!
        }

        public void RenderFrame()
        {
            //Set global things here
            //GL.Viewport(0, 0, _glControl.Width, _glControl.Height);
            GL.ClearColor(Color.GreenYellow);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_programId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //What does this do?
            GL.EnableVertexAttribArray(_attributeVpos);

            foreach (RenderableObject o in _renderableObjects)
            {
                Matrix4 projMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4f, _glControl.Width / (float)_glControl.Height, 0.01f, 1000f);

                //Create Model matrix based on Objects Translation/Rotation
                o.CalculateModelMatrix();
                o.ViewProjectionMatrix = _camera.GetViewMatrix()*projMatrix;
                o.ModelViewProjectionMatrix = o.ModelMatrix*o.ViewProjectionMatrix;

                //Upload the Model Matrix to the GPU
                GL.UniformMatrix4(_uniformMVP, false, ref o.ModelViewProjectionMatrix);
                o.Render();
            }

            GL.DisableVertexAttribArray(_attributeVpos);
            GL.Flush();

            _glControl.SwapBuffers();
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
    }
}