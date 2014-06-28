using System;
using System.IO;
using OpenTK;
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
        public abstract void OnSceneUnload();
       
        public abstract void Render(Camera camera, float aspectRatio);
        public abstract void SetModelMatrix(Matrix4 matrix);
        protected abstract void CreateShaderFromFile(string vertShader, string fragShader);

        protected void LoadShader(string fileName, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (var streamReader = new StreamReader(fileName))
            {
                GL.ShaderSource(address, streamReader.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);

            int compileSuccess;
            GL.GetShader(address, ShaderParameter.CompileStatus, out compileSuccess);

            if (compileSuccess == 0)
                Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public void Dispose()
        {
            GL.DeleteProgram(_programId);
        }
    }
}
