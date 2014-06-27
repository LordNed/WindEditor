using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public abstract class BaseRenderer : IDisposable
    {
        public enum ShaderAttributeIds
        {
            Position, Color,
            TexCoord
        }

        //Shader Identifier
        protected int _programId;

        //OpenTK::Shader Attributes
        protected int _uniformMVP;

        public abstract void Initialize();
        protected abstract void CreateShader(string vertShader, string fragShader);
        public abstract void Render(Camera camera, float aspectRatio);

        protected void LoadShader(string fileName, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (var streamReader = new StreamReader(fileName))
            {
                GL.ShaderSource(address, streamReader.ReadToEnd());
            }
            Console.WriteLine(GL.GetError());
            GL.CompileShader(address);
            Console.WriteLine(GL.GetError());
            GL.AttachShader(program, address);
            
            if(GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public void Dispose()
        {
            GL.DeleteProgram(_programId);
        }
    }
}
