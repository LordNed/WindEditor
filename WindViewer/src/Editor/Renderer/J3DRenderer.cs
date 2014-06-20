using System;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public sealed class J3DRenderer : IRenderer
    {
        public J3DRenderer()
        {
            InitializeShader("shaders/j3d_vs.glsl", "shaders/j3d_fs.glsl");
        }

        protected override void InitializeShader(string vertShader, string fragShader)
        {
            //Initialize the OpenGL Program
            _programId = GL.CreateProgram();

            int vertShaderId, fragShaderId;
            LoadShader(vertShader, ShaderType.VertexShader, _programId, out vertShaderId);
            LoadShader(fragShader, ShaderType.FragmentShader, _programId, out fragShaderId);

            //Link shaders 
            GL.LinkProgram(_programId);

            //Deincriment the reference count on the shaders so that they 
            //don't exist until the context is destroyed.
            GL.DeleteShader(vertShaderId);
            GL.DeleteShader(fragShaderId);

            _uniformMVP = GL.GetUniformLocation(_programId, "MVP");
            GL.BindAttribLocation(_programId, (int) ShaderAttributeIds.Position, "vertexPos");
            GL.BindAttribLocation(_programId, (int) ShaderAttributeIds.TexCoord, "vertexTexCoord");


            if (GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetProgramInfoLog(_programId));
        }

        public override void ClearRenderableList()
        {
            
        }

        public override void Render(Camera camera, float aspectRatio)
        {
            
        }
    }
}