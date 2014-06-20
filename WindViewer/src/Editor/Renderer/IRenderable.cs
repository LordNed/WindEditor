using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor
{
    public abstract class IRenderable : IDisposable
    {
        private int _vboPosition;
        private int _vboTexCoord;
        private int _iboIndexes;
        private int _diffuseTextureId;

        protected int _vertexCount;
        protected int _indexCount;


        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public Transform transform;

        public abstract int[] GetIndices();
        public abstract Vector3[] GetVerts();
        public abstract float[] GetTexCoords();

        protected IRenderable()
        {
            GL.GenBuffers(1, out _vboPosition);
            GL.GenBuffers(1, out _iboIndexes);
            GL.GenBuffers(1, out _vboTexCoord);

            transform = new Transform();

            GenerateTexture();
        }

        public virtual void GenerateTexture()
        {
            _diffuseTextureId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, _diffuseTextureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);


            float[] checkerBoard = { 0f, 0f, 0f, 1f, 1f, 1f, 1f, 1f, 1f, 0f, 0f, 0f };
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 2, 2, 0, PixelFormat.Rgb, PixelType.Float,
                checkerBoard);

        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vboPosition);
            GL.DeleteBuffer(_iboIndexes);
            GL.DeleteBuffer(_vboTexCoord);
            GL.DeleteTexture(_diffuseTextureId);
        }

        public virtual void UpdateBuffers()
        {
            //Bind Index Buffer & Upload Data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboIndexes);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(_indexCount * sizeof(int)), GetIndices(), BufferUsageHint.StaticDraw);

            //Bind Vertex Buffer & Upload Data
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboPosition);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_vertexCount * Vector3.SizeInBytes), GetVerts(), BufferUsageHint.StaticDraw);

            //Bind UV Buffer & Upload Data (teeemmemep)
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboTexCoord);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(GetTexCoords().Length * 4), GetTexCoords(), BufferUsageHint.StaticDraw);
        }

        public virtual void BindBuffers()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboIndexes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboPosition);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboTexCoord);
            GL.BindTexture(TextureTarget.Texture2D, _diffuseTextureId);
        }

        public virtual void Render()
        {
            GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0);
        }

        public virtual void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(transform.Scale) * Matrix4.CreateFromQuaternion(transform.Rotation) *
                          Matrix4.CreateTranslation(transform.Position);
        }


    }
}
