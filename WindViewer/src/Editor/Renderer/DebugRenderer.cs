using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor.Tools;
using WindViewer.Forms;

namespace WindViewer.Editor.Renderer
{
    public class DebugRenderer : BaseRenderer, IEditorTool
    {
        private class Instance
        {
            public Instance(Vector3 pos, Color color, Quaternion rot, Vector3 scale, float lifetime)
            {
                Position = pos;
                Rotation = rot;
                Scale = scale;
                Color = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                Lifetime = lifetime;
            }

            public Vector3 Position { get; private set; }
            public Quaternion Rotation { get; private set; }
            public Vector3 Scale { get; private set; }
            public Vector4 Color { get; private set; }
            public float Lifetime;
        }

        private class LineInstance : IDisposable, IRenderable
        {
            private int _vboId;

            public Vector4 Color;
            public float Lifetime;

            public LineInstance(Vector3 startPos, Vector3 endPos, Color color, float lifetime)
            {
                Vector3[] verts = new[] { startPos, endPos };
                Color = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255F);
                Lifetime = lifetime;

                GL.GenBuffers(1, out _vboId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * 4 * 3), verts, BufferUsageHint.StaticDraw);
            }

            public void Dispose()
            {
                GL.DeleteBuffer(_vboId);
            }

            public void Bind()
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
            }

            public void Draw(BaseRenderer renderer)
            {

                GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            }
        }


        private Dictionary<IRenderable, List<Instance>> _renderList;
        private List<LineInstance> _lineRenderList;

        private static DebugRenderer _instance;

        //Instances of Geometry
        private IRenderable _cubeMeshWire;
        private IRenderable _cubeMeshSolid;

        public override void Initialize()
        {
            _renderList = new Dictionary<IRenderable, List<Instance>>();
            _lineRenderList = new List<LineInstance>();
            _instance = this;
            _cubeMeshWire = new CubeMeshWire();
            _cubeMeshSolid = new CubeMeshSolid();

            CreateShaderFromFile("debug", "shaders/debug_vs.glsl", "shaders/debug_fs.glsl");
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

            //Link shaders 
            GL.LinkProgram(shader.ProgramId);

            if (GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetProgramInfoLog(shader.ProgramId));

            shader.UniformMVP = GL.GetUniformLocation(shader.ProgramId, "modelview");
            shader.UniformColor = GL.GetUniformLocation(shader.ProgramId, "inColor");

            if (GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetProgramInfoLog(shader.ProgramId));

            _shaders.Add(name, shader);
        }

        public override void OnSceneUnload()
        {
            _renderList.Clear();
        }

        public override void Render(Camera camera)
        {
            UseShader("debug");

            //State Muckin'
            GL.Enable(EnableCap.DepthTest);

            //Clear any previously bound buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Matrix4 viewProjMatrix = camera.GetViewProjMatrix();

            foreach (var meshType in _renderList)
            {
                //Bind the objects buffer, enable attribs and set layout.
                meshType.Key.Bind();
                GL.VertexAttribPointer((int)ShaderAttributeIds.Position, 3, VertexAttribPointerType.Float, false, 3 * 4, 0);
                GL.EnableVertexAttribArray((int)ShaderAttributeIds.Position);

                foreach (var instance in meshType.Value)
                {
                    Matrix4 worldMatrix = Matrix4.CreateScale(instance.Scale) * Matrix4.CreateFromQuaternion(instance.Rotation) * Matrix4.CreateTranslation(instance.Position);
                    Matrix4 finalMatrix = worldMatrix * viewProjMatrix;

                    //Upload the WVP to the GPU
                    GL.UniformMatrix4(_currentShader.UniformMVP, false, ref finalMatrix);

                    //Upload this primitives color.
                    GL.Uniform3(_currentShader.UniformColor, new Vector3(instance.Color.X, instance.Color.Y, instance.Color.Z));

                    meshType.Key.Draw(this);
                }

                GL.DisableVertexAttribArray((int)ShaderAttributeIds.Position);
            }

            //Now draw all of our debug lines
            //Upload the WVP to the GPU
            GL.UniformMatrix4(_currentShader.UniformMVP, false, ref viewProjMatrix);
            foreach (LineInstance instance in _lineRenderList)
            {
                //Bind the object buffer, enable attribs and set layout.
                instance.Bind();
                GL.VertexAttribPointer((int)ShaderAttributeIds.Position, 3, VertexAttribPointerType.Float, false, 3 * 4, 0);
                GL.EnableVertexAttribArray((int)ShaderAttributeIds.Position);

                //Upload the primitives color.
                GL.Uniform3(_currentShader.UniformColor, new Vector3(instance.Color.X, instance.Color.Y, instance.Color.Z));

                instance.Draw(this);


                GL.DisableVertexAttribArray((int)ShaderAttributeIds.Position);
            }
        }

        public override void SetModelMatrix(Matrix4 matrix) { }

        public static void DrawLine(Vector3 startPos, Vector3 endPos, float duration = 0f)
        {
            DrawLine(startPos, endPos, Color.White, duration);
        }
        public static void DrawLine(Vector3 startPos, Vector3 endPos, Color color, float duration = 0f)
        {
            _instance._lineRenderList.Add(new LineInstance(startPos, endPos, color, duration));
        }

        public static void DrawWireCube(Vector3 position, Color color, Quaternion rotation, Vector3 scale)
        {
            if (!_instance._renderList.ContainsKey(_instance._cubeMeshWire))
                _instance._renderList.Add(_instance._cubeMeshWire, new List<Instance>());

            _instance._renderList[_instance._cubeMeshWire].Add(new Instance(position, color, rotation, scale, 0f));
        }

        public static void DrawCube(Vector3 position, Color color, Quaternion rotation, Vector3 scale)
        {
            if (!_instance._renderList.ContainsKey(_instance._cubeMeshSolid))
                _instance._renderList.Add(_instance._cubeMeshSolid, new List<Instance>());

            _instance._renderList[_instance._cubeMeshSolid].Add(new Instance(position, color, rotation, scale, 0f));
        }

        public void Update()
        {

        }

        public void PostRenderUpdate()
        {
            foreach (var type in _renderList)
            {
                List<Instance> pendRemoval = new List<Instance>();
                foreach (var instance in type.Value)
                {
                    instance.Lifetime -= MainEditor.DeltaTime;
                    if (instance.Lifetime <= 0f)
                        pendRemoval.Add(instance);
                }

                foreach (Instance instance in pendRemoval)
                {
                    type.Value.Remove(instance);
                }
            }

            List<LineInstance> linePendRemoval = new List<LineInstance>();
            foreach (var lineInstance in _lineRenderList)
            {
                lineInstance.Lifetime -= MainEditor.DeltaTime;

                if (lineInstance.Lifetime <= 0f)
                    linePendRemoval.Add(lineInstance);
            }
            foreach (LineInstance instance in linePendRemoval)
            {
                _lineRenderList.Remove(instance);
                instance.Dispose();
            }
        }
    }
}