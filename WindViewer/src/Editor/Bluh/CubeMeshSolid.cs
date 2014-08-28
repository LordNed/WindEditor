using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public class CubeMeshSolid : IRenderable
    {
        private readonly int _glVbo;
        private readonly int _glEbo;

        public CubeMeshSolid()
        {
            Vector3[] meshVerts = 
            { 
                //Top Verts
                new Vector3(-1f, 1f,  -1f),
                new Vector3(1f, 1f,  -1f),
                new Vector3(1f, 1f,  1f),
                new Vector3(-1f, 1f,  1f),
                //Bottom Verts
                new Vector3(-1f, -1f,  1f),
                new Vector3(-1f, -1f,  -1f),
                new Vector3(1f, -1f,  -1f),
                new Vector3(1f, -1f,  1f),
            };

            //ToDo: This is a really round about way, I'm sure there's a way with less edges.
            int[] meshIndexes =
            {
                4, 3, 0, 5, 6, 1, 0, 3, 2, 1, 6, 7, 2, 3, 4, 7, 6, 5
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
            GL.DrawElements(PrimitiveType.Triangles, 18, DrawElementsType.UnsignedInt, 0);
        }
    }
}