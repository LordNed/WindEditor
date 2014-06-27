using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public sealed class J3DRenderer : BaseRenderer
    {
        public static event Action Bind;
        public static event Action Draw;

        private readonly List<IRenderable> _renderList; 
        public static J3DRenderer Instance;
        public J3DRenderer()
        {
            Instance = this;
            _renderList = new List<IRenderable>();
        }

        public override void Initialize()
        {
            CreateShader("shaders/j3d_vs.glsl", "shaders/j3d_fs.glsl");
        }

        public struct VertexFormatLayout
        {
            public Vector3 Position;
            public Vector4 Color;
            public Vector2 TexCoord;

            public VertexFormatLayout(Vector3 pos, Vector4 color, Vector2 tex)
            {
                Position = pos;
                Color = color;
                TexCoord = tex;
            }
        }

        public void AddRenderable(IRenderable renderable)
        {
            _renderList.Add(renderable);
        }

        public void RemoveRenderable(IRenderable renderable)
        {
            _renderList.Remove(renderable);
        }

        protected override void CreateShader(string vertShader, string fragShader)
        {
            //Initialize the OpenGL Program
            _programId = GL.CreateProgram();

            int vertShaderId, fragShaderId;
            LoadShader(vertShader, ShaderType.VertexShader, _programId, out vertShaderId);
            LoadShader(fragShader, ShaderType.FragmentShader, _programId, out fragShaderId);

            //Deincriment the reference count on the shaders so that they 
            //don't exist until the context is destroyed.
            GL.DeleteShader(vertShaderId);
            GL.DeleteShader(fragShaderId);
            
            GL.BindAttribLocation(_programId, (int) ShaderAttributeIds.Position, "vertexPos");
            GL.BindAttribLocation(_programId, (int) ShaderAttributeIds.Color, "inColor");
            GL.BindAttribLocation(_programId, (int) ShaderAttributeIds.TexCoord, "vertexUV");

            //Link shaders 
            GL.LinkProgram(_programId);

            _uniformMVP = GL.GetUniformLocation(_programId, "modelview");

            if (GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetProgramInfoLog(_programId));
        }

        public override void Render(Camera camera, float aspectRatio)
        {
            GL.UseProgram(_programId);

            //State Muckin'
            GL.Enable(EnableCap.DepthTest);

            //Clear any previously bound buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); 


            //Todo: Temp
            Matrix4 projMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 100f, 10000f);
            Matrix4 modelMatrix = Matrix4.Identity;
            Matrix4 viewMatrix = camera.GetViewMatrix();

            Matrix4 finalMatrix = modelMatrix*viewMatrix*projMatrix;

            //Upload matrix to the GPU
            GL.UniformMatrix4(_uniformMVP, false, ref finalMatrix);

            /* Because the J3D models are very complex, we're going to 
             * allow them to completely render themselves, including
             * binding buffers, enabling vertex attribs, etc. */
            foreach (var renderable in _renderList)
            {
                renderable.Bind();

                GL.VertexAttribPointer((int)ShaderAttributeIds.Position, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);
                GL.VertexAttribPointer((int)ShaderAttributeIds.Color, 4, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);
                GL.VertexAttribPointer((int)ShaderAttributeIds.TexCoord, 2, VertexAttribPointerType.Float, false, 9 * 4, 7 * 4);

                renderable.Draw(this);
            }

            GL.Flush();
        }

        private Matrix4 _projMatrix, _viewMatrix;
        public void GetCamMatrix(out int uniform, out Matrix4 viewProj)
        {
            uniform = _uniformMVP;
            viewProj = _viewMatrix*_projMatrix;
        }
    }
}