using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public sealed class J3DRenderer : BaseRenderer
    {
        public struct VertexFormatLayout
        {
            public Vector3 Position;
            public Vector4 Color;
            public Vector2 TexCoord;
        }

        public static J3DRenderer Instance;

        private List<IRenderable> _renderList;
        private Matrix4 _viewProjMatrix; //ToDo: Not a good solution.

        public override void Initialize()
        {
            Instance = this;
            _renderList = new List<IRenderable>();
            CreateShaderFromFile("debug", "shaders/debug_vs.glsl", "shaders/debug_fs.glsl");
            CreateShaderFromFile("j3d", "shaders/j3d_vs.glsl", "shaders/j3d_fs.glsl");
        }

        public override void OnSceneUnload()
        {
            _renderList.Clear();
        }

        public void AddRenderable(IRenderable renderable)
        {
            _renderList.Add(renderable);
        }

        public void RemoveRenderable(IRenderable renderable)
        {
            _renderList.Remove(renderable);
        }
        

        protected override void CreateShaderFromFile(string name, string vertShader, string fragShader)
        {
            Shader shader = new Shader();
            shader.Name = name;

            //Initialize the OpenGL Program
            shader.ProgramId = GL.CreateProgram();

            int vertShaderId, fragShaderId;
            LoadShader(vertShader, ShaderType.VertexShader, shader.ProgramId, out vertShaderId);
            LoadShader(fragShader, ShaderType.FragmentShader, shader.ProgramId, out fragShaderId);

            //Deincriment the reference count on the shaders so that they 
            //don't exist until the context is destroyed.
            GL.DeleteShader(vertShaderId);
            GL.DeleteShader(fragShaderId);

            GL.BindAttribLocation(shader.ProgramId, (int)ShaderAttributeIds.Position, "vertexPos");
            GL.BindAttribLocation(shader.ProgramId, (int)ShaderAttributeIds.Color, "inColor");
            GL.BindAttribLocation(shader.ProgramId, (int)ShaderAttributeIds.TexCoord, "vertexUV");

            //Link shaders 
            GL.LinkProgram(shader.ProgramId);

            shader.UniformMVP = GL.GetUniformLocation(shader.ProgramId, "modelview");
            shader.UniformColor = GL.GetUniformLocation(shader.ProgramId, "inColor");

            if (GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetProgramInfoLog(shader.ProgramId));

            _shaders.Add(name, shader);
        }

        public override void Render(Camera camera)
        {
            UseShader("j3d");

            //State Muckin'
            GL.Enable(EnableCap.DepthTest);

            //Clear any previously bound buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); 

            //Build a View Projection Matrix out of camera settings.
            _viewProjMatrix = camera.GetViewProjMatrix();

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

        public override void UseShader(string name)
        {
            base.UseShader(name);
            Matrix4 finalMatrix = _cachedModelMatrix * _viewProjMatrix;
            GL.UniformMatrix4(_currentShader.UniformMVP, false, ref finalMatrix);
        }

        private Matrix4 _cachedModelMatrix = Matrix4.Identity;
        public override void SetModelMatrix(Matrix4 modelMatrix)
        {
            //Upload matrix to the GPU
            _cachedModelMatrix = modelMatrix;
            Matrix4 finalMatrix = modelMatrix * _viewProjMatrix;
            GL.UniformMatrix4(_currentShader.UniformMVP, false, ref finalMatrix);
        }
    }
}