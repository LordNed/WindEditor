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
            _vertexCount = 8;
            _indexCount = 36;
        }

        public override Vector3[] GetVerts()
        {
            return new Vector3[] { new Vector3(-50f, -50f,  -50f),
                new Vector3(50f, -50f,  -50f),
                new Vector3(50f, 50f,  -50f),
                new Vector3(-50f, 50f,  -50f),
                new Vector3(-50f, -50f,  50f),
                new Vector3(50f, -50f,  50f),
                new Vector3(50f, 50f,  50f),
                new Vector3(-50f, 50f,  50f),
            };
        }

        public override int[] GetIndices()
        {
            return new int[]{
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
        }
    }
}
