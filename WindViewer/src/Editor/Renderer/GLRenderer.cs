using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public class GLRenderer : IRenderer
    {
        public GLRenderer()
        {
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
        }

        public override void AddRenderable(IRenderable renderable)
        {
            base.AddRenderable(renderable);
            renderable.UpdateBuffers();
            GL.VertexAttribPointer(_attributeVpos, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        override public void Render(Camera camera, float aspectRatio)
        {
            GL.UseProgram(_programId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //What does this do?
            GL.EnableVertexAttribArray(_attributeVpos);
            //GL.VertexAttribPointer(_attributeVpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            foreach (IRenderable o in _renderableObjects)
            {
                Matrix4 projMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 0.01f, 1000f);

                //Create Model matrix based on Objects Translation/Rotation
                o.CalculateModelMatrix();
                o.ViewProjectionMatrix = camera.GetViewMatrix()*projMatrix;
                o.ModelViewProjectionMatrix = o.ModelMatrix*o.ViewProjectionMatrix;

                //Upload the Model Matrix to the GPU
                GL.UniformMatrix4(_uniformMVP, false, ref o.ModelViewProjectionMatrix);

                //Render the object.
                o.Render();
            }

            GL.DisableVertexAttribArray(_attributeVpos);
            GL.Flush();
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