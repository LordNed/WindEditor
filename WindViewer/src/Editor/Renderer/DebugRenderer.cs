using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public class DebugRenderer : BaseRenderer
    {
        private class Instance
        {
            public Instance(Vector3 pos, Color color, Quaternion rot, Vector3 scale)
            {
                Position = pos;
                Rotation = rot;
                Scale = scale;
                Color = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            }

            public Vector3 Position { get; private set; }
            public Quaternion Rotation { get; private set; }
            public Vector3 Scale { get; private set; }
            public Vector4 Color { get; private set; }
        }

        private class LineInstance : IDisposable, IRenderable
        {
            private int _vboId;
            public Vector4 Color;

            public LineInstance(Vector3 startPos, Vector3 endPos, Color color)
            {
                Vector3[] verts = new[] {startPos, endPos};
                Color = new Vector4(color.R/255f, color.G/255f, color.B/255f, color.A/255F);

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
        private int _uniformColor;

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

            CreateShaderFromFile("shaders/debug_vs.glsl", "shaders/debug_fs.glsl");
        }

        protected override void CreateShaderFromFile(string vertShader, string fragShader)
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

            GL.BindAttribLocation(_programId, (int)ShaderAttributeIds.Position, "vertexPos");

            //Link shaders 
            GL.LinkProgram(_programId);

            if (GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetProgramInfoLog(_programId));

            _uniformMVP = GL.GetUniformLocation(_programId, "modelview");
            _uniformColor = GL.GetUniformLocation(_programId, "inColor");
     
            if (GL.GetError() != ErrorCode.NoError)
                Console.WriteLine(GL.GetProgramInfoLog(_programId));
        }

        public override void OnSceneUnload()
        {
            _renderList.Clear();
        }

        public override void Render(Camera camera)
        {
            GL.UseProgram(_programId);

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
                    GL.UniformMatrix4(_uniformMVP, false, ref finalMatrix);

                    //Upload this primitives color.
                    GL.Uniform3(_uniformColor, new Vector3(instance.Color.X, instance.Color.Y, instance.Color.Z));

                    meshType.Key.Draw(this);
                }

                GL.DisableVertexAttribArray((int)ShaderAttributeIds.Position);
            }

            //Now draw all of our debug lines
            //Upload the WVP to the GPU
            GL.UniformMatrix4(_uniformMVP, false, ref viewProjMatrix);
            foreach (LineInstance instance in _lineRenderList)
            {
                //Bind the object buffer, enable attribs and set layout.
                instance.Bind();
                GL.VertexAttribPointer((int)ShaderAttributeIds.Position, 3, VertexAttribPointerType.Float, false, 3 * 4, 0);
                GL.EnableVertexAttribArray((int)ShaderAttributeIds.Position);

                //Upload the primitives color.
                GL.Uniform3(_uniformColor, new Vector3(instance.Color.X, instance.Color.Y, instance.Color.Z));

                instance.Draw(this);


                GL.DisableVertexAttribArray((int)ShaderAttributeIds.Position);
            }
        }

        public override void SetModelMatrix(Matrix4 matrix) { }

        public static void DrawLine(Vector3 startPos, Vector3 endPos, Color color)
        {
            _instance._lineRenderList.Add(new LineInstance(startPos, endPos, color));
        }

        public static void DrawWireCube(Vector3 position, Color color, Quaternion rotation, Vector3 scale)
        {
            if (!_instance._renderList.ContainsKey(_instance._cubeMeshWire))
                _instance._renderList.Add(_instance._cubeMeshWire, new List<Instance>());

            _instance._renderList[_instance._cubeMeshWire].Add(new Instance(position, color, rotation, scale));
        }

        public static void DrawCube(Vector3 position, Color color, Quaternion rotation, Vector3 scale)
        {
            if (!_instance._renderList.ContainsKey(_instance._cubeMeshSolid))
                _instance._renderList.Add(_instance._cubeMeshSolid, new List<Instance>());

            _instance._renderList[_instance._cubeMeshSolid].Add(new Instance(position, color, rotation, scale));
        }
    }
}