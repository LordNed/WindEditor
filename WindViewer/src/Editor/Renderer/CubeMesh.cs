using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public class CubeMesh : IRenderable
    {
        private readonly int _glVbo;
        private readonly int _glEbo;

        public CubeMesh()
        {
            Vector3[] meshVerts = 
            { 
                new Vector3(-50f, -50f,  -50f),
                new Vector3(50f, -50f,  -50f),
                new Vector3(50f, 50f,  -50f),
                new Vector3(-50f, 50f,  -50f),
                new Vector3(-50f, -50f,  50f),
                new Vector3(50f, -50f,  50f),
                new Vector3(50f, 50f,  50f),
                new Vector3(-50f, 50f,  50f),
            };


            int[] meshIndexes =
            {
                //front
                0, 7, 3,
                0, 4, 7,
                //back
                1, 2, 6,
                6, 5, 1,
                //left
                0, 2, 1,
                0, 3, 2,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };

            GL.GenBuffers(1, out _glVbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(meshVerts.Length * 4 * 3), meshVerts, BufferUsageHint.StaticDraw);
            GL.GenBuffers(1, out _glEbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _glEbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(meshIndexes.Length * 4), meshIndexes,
                BufferUsageHint.StaticDraw);

        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _glEbo);
        }

        public void Draw(BaseRenderer renderer)
        {
            GL.DrawElements(PrimitiveType.TriangleStrip, 36, DrawElementsType.UnsignedInt, 0);
        }
    }
}