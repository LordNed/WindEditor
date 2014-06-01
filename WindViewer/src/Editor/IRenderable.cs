using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor
{
    public abstract class IRenderable
    {
        private int _vboPosition;
        private int _iboIndexes;

        protected int _vertexCount;
        protected int _indexCount;

        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public Transform transform;

        public abstract int[] GetIndices();
        public abstract Vector3[] GetVerts();

        protected IRenderable()
        {
            GL.GenBuffers(1, out _vboPosition);
            GL.GenBuffers(1, out _iboIndexes);

            transform = new Transform();
        }

        public virtual void UpdateBuffers()
        {
            //Bind Index Buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboIndexes);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) (_indexCount*sizeof (int)), GetIndices(), BufferUsageHint.StaticDraw);

            //Bind Vertex Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboPosition);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_vertexCount * Vector3.SizeInBytes), GetVerts(), BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        }

        public virtual void Render()
        {
            GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, _iboIndexes);            
        }

        public virtual void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(transform.Scale)*Matrix4.CreateFromQuaternion(transform.Rotation)*
                          Matrix4.CreateTranslation(transform.Position);
        }
    }
}
