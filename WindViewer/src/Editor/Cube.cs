using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor
{
    public class Cube : IRenderable
    {
        public Transform Transform;

        private int VertexCount;
        private int IndexCount;

        private int _indexBuffer;
        private int _vertexBuffer;

        // TODO(mtwilliams): Move this crap to IShaderManager or something.
        private int _programId;
        private int _vertexShaderId;
        private int _pixelShaderId;
        private int _attributeVertexPos;
        private int _uniformModelViewProjMatrix;

        public Cube()
        {
            VertexCount = 8;
            IndexCount = 36;
            Transform = new Transform();

            // TODO(mtwilliams): cache this shit.
            GenerateIndexBuffer();
            GenerateVertexBuffer();
            LoadShaders();
        }

        public Vector3[] GetVerts()
        {
            return new Vector3[] {
                new Vector3(-0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, 0.5f,  -0.5f),
                new Vector3(-0.5f, 0.5f,  -0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, 0.5f,  0.5f),
                new Vector3(-0.5f, 0.5f,  0.5f),
            };
        }

        public int[] GetIndices(int offset = 0)
        {
            int[] inds = new int[] {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };



            if (offset != 0)
            {
                for (int i = 0; i < inds.Length; i++)
                {
                    inds[i] += offset;
                }
            }

            return inds;
        }

        public Matrix4 CalculateModelMatrix()
        {
            return Matrix4.CreateScale(Transform.Scale) * Matrix4.CreateFromQuaternion(Transform.Rotation) * Matrix4.CreateTranslation(Transform.Position);
        }

        public Vector3[] GetColorData()
        {
            return new Vector3[] {
                new Vector3( 1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 1f, 0f),
                new Vector3( 1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 1f, 0f),
                new Vector3( 1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f)
            };
        }

        private void GenerateIndexBuffer()
        {
            GL.GenBuffers(1, out _indexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
            int[] indicies = GetIndices();
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicies.Length * sizeof(int)), indicies, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void GenerateVertexBuffer()
        {
            GL.GenBuffers(1, out _vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            Vector3[] verts = GetVerts();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * Vector3.SizeInBytes), verts, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void LoadShaderFromFile(String fileName, ShaderType type, int program, out int address)
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

        private void LoadShaders()
        {
            _programId = GL.CreateProgram();
            LoadShaderFromFile("src/shaders/vs.glsl", ShaderType.VertexShader, _programId, out _vertexShaderId);
            LoadShaderFromFile("src/shaders/fs.glsl", ShaderType.FragmentShader, _programId, out _pixelShaderId);
            GL.LinkProgram(_programId);
            Console.WriteLine(GL.GetProgramInfoLog(_programId));

            _attributeVertexPos = GL.GetAttribLocation(_programId, "vPosition");
            _uniformModelViewProjMatrix = GL.GetUniformLocation(_programId, "modelview");

            if (_attributeVertexPos == -1 || _uniformModelViewProjMatrix == -1)
            {
                Console.WriteLine("Error binding attributes!");
                throw new Exception("Error binding attributes!");
            }
        }

        public override void Render(Matrix4 viewProjMatrix)
        {
            Matrix4 modelViewProjMatrix = Matrix4.Identity;// CalculateModelMatrix() * viewProjMatrix;

            GL.UseProgram(_programId);
            GL.UniformMatrix4(_uniformModelViewProjMatrix, false, ref modelViewProjMatrix);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            GL.EnableVertexAttribArray(_attributeVertexPos);
            GL.VertexAttribPointer(_attributeVertexPos, 3, VertexAttribPointerType.Float, false, 0, 0);
            
            GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            
            GL.DisableVertexAttribArray(_attributeVertexPos);
        }
    }
}
