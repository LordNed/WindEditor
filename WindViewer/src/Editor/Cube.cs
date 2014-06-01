using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor
{
    public class Cube : IRenderable
    {
        public Cube()
        {
            _vertexCount = 3;
            _indexCount = 3;
        }

        public override Vector3[] GetVerts()
        {
            return new Vector3[] {
                new Vector3(0.0f, 0.0f, 0.5f),
                new Vector3(1.0f, 0.0f, 0.5f),
                new Vector3(1.0f, 1.0f, 0.5f)
            };
        }

        public override int[] GetIndices()
        {
            return new int[] {
                0, 1, 2
            };
        }
    }
}
