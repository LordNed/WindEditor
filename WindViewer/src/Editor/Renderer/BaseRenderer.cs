using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace WindViewer.Editor.Renderer
{
    public abstract class BaseRenderer : IDisposable
    {
        public enum ShaderAttributeIds
        {
            Position, Color,
            TexCoord
        }

        public class Shader : IDisposable
        {
            public void Use()
            {
                GL.UseProgram(ProgramId);
            }

            public void Dispose()
            {
                GL.DeleteProgram(ProgramId);
            }

            public string Name;
            public int ProgramId;
            public int UniformMVP;
            public int UniformColor;
        }

        //Shaders
        protected Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();
        public Shader _currentShader = null;

        public abstract void Initialize();
        public abstract void OnSceneUnload();

        public abstract void Render(Camera camera);
        public abstract void SetModelMatrix(Matrix4 matrix);
        protected abstract void CreateShaderFromFile(string name, string vertShader, string fragShader);

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

        public virtual void UseShader(string name)
        {
            _currentShader = _shaders[name];
            _currentShader.Use();
        }

        public void Dispose()
        {
            //should automatically dispose _shaders
        }
    }
}
