using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor.Tools;

namespace WindViewer.Editor.Renderer
{
    public abstract class IRenderer : IEditorTool
    {
        protected enum ShaderAttributeIds
        {
            Position = 1,
            ModelViewPerspectiveMatrix = 2,
        }
        //Shader Identifier
        protected int _programId;

        //OpenTK::Shader Attributes
        protected int _attributeVpos;
        protected int _uniformMVP;

        protected virtual void InitializeShader(string vertShader, string fragShader)
        {
            //Initialize program
            _programId = GL.CreateProgram();

            int vertShaderId, fragShaderId;
            LoadShader(vertShader, ShaderType.VertexShader, _programId, out vertShaderId);
            LoadShader(fragShader, ShaderType.FragmentShader, _programId, out fragShaderId);

            //GL.BindAttribLocation(_programId, (int)ShaderAttributeIds.Position, "vPosition");
            //GL.BindAttribLocation(_programId, (int) ShaderAttributeIds.ModelViewPerspectiveMatrix, "modelview");

            //Link & Debug Spew!
            GL.LinkProgram(_programId);

            _attributeVpos = GL.GetAttribLocation(_programId, "vPosition");
            _uniformMVP = GL.GetUniformLocation(_programId, "modelview");


            Console.WriteLine(GL.GetProgramInfoLog(_programId));
        }

        private void LoadShader(string fileName, ShaderType type, int program, out int address)
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

        public virtual void AddRenderable(IRenderable renderable)
        {
            
        }

        public virtual void RemoveRenderable(IRenderable renderable)
        {

        }

        public abstract void Render(Camera camera, float aspectRatio);
    }
}
