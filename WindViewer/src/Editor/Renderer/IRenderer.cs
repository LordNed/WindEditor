using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor.Tools;

namespace WindViewer.Editor.Renderer
{
    public abstract class IRenderer : IEditorTool, IDisposable
    {
        protected enum ShaderAttributeIds
        {
            Position, Color,
        }

        //Shader Identifier
        protected int _programId;

        //OpenTK::Shader Attributes
        protected int _uniformMVP;

        protected abstract void InitializeShader(string vertShader, string fragShader);
        public abstract void Render(Camera camera, float aspectRatio);

        protected void LoadShader(string fileName, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(fileName))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            Console.WriteLine(GL.GetError());
            GL.CompileShader(address);
            Console.WriteLine(GL.GetError());
            GL.AttachShader(program, address);
            
            //if(GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public void Dispose()
        {
            GL.DeleteProgram(_programId);
        }
    }
}
