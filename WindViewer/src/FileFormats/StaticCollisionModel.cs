using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using WindViewer.Editor;

namespace WindViewer.FileFormats
{
    public class StaticCollisionModel : BaseArchiveFile
    {
        #region File Formats
        class FileHeader
        {
            public const int Size = 48;

            public int VertexCount;
            public int VertexOffset;
            public int TriangleCount;
            public int TriangleOffset;
            public int Unknown1Count;
            public int Unknown1Offset;
            public int Unknown2Count;
            public int Unknown2Offset;
            public int TypeCount;
            public int TypeOffset;
            public int Unknown3Count;
            public int Unknown3Offset;

            public void Load(byte[] data, ref int offset)
            {
                VertexCount = FSHelpers.Read32(data, 0);
                VertexOffset = FSHelpers.Read32(data, 4);
                TriangleCount = FSHelpers.Read32(data, 8);
                TriangleOffset = FSHelpers.Read32(data, 12);
                Unknown1Count = FSHelpers.Read32(data, 16);
                Unknown1Offset = FSHelpers.Read32(data, 20);
                Unknown2Count = FSHelpers.Read32(data, 24);
                Unknown2Offset = FSHelpers.Read32(data, 28);
                TypeCount = FSHelpers.Read32(data, 32);
                TypeOffset = FSHelpers.Read32(data, 36);
                Unknown3Count = FSHelpers.Read32(data, 40);
                Unknown3Offset = FSHelpers.Read32(data, 44);

                offset += Size;
            }

            public void Save(BinaryWriter stream)
            {
                FSHelpers.Write32(stream, (int) VertexCount);
                FSHelpers.Write32(stream, (int)VertexOffset);
                FSHelpers.Write32(stream, (int)TriangleCount);
                FSHelpers.Write32(stream, (int)TriangleOffset);
                FSHelpers.Write32(stream, (int)Unknown1Count);
                FSHelpers.Write32(stream, (int)Unknown1Offset);
                FSHelpers.Write32(stream, (int)Unknown2Count);
                FSHelpers.Write32(stream, (int)Unknown2Offset);
                FSHelpers.Write32(stream, (int)TypeCount);
                FSHelpers.Write32(stream, (int)TypeOffset);
                FSHelpers.Write32(stream, (int)Unknown3Count);
                FSHelpers.Write32(stream, (int)Unknown3Offset);
            }
        }

        class Vertex
        {
            public const int Size = 12;

            public Vector3 Position;

            public void Load(byte[] data, ref int offset)
            {
                Position = new Vector3(
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset)),
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 4)),
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 8)));
                offset += Size;
            }

            public void Save(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, Position.X);
                FSHelpers.WriteFloat(stream, Position.Y);
                FSHelpers.WriteFloat(stream, Position.Z);
            }
        }

        class Triangle
        {
            public const int Size = 10;

            public ushort[] Vertices = new ushort[3];
            public ushort Unknown1, Unknown2;

            public void Load(byte[] data, ref int offset)
            {
                Vertices[0] = (ushort) FSHelpers.Read16(data, offset);
                Vertices[1] = (ushort)FSHelpers.Read16(data, offset + 2);
                Vertices[2] = (ushort)FSHelpers.Read16(data, offset + 4);
                Unknown1 = (ushort)FSHelpers.Read16(data, offset + 6);
                Unknown2 = (ushort)FSHelpers.Read16(data, offset + 8);

                offset += Size;
            }

            public void Save(BinaryWriter stream)
            {
                for(int i =0; i < 3; i++)
                    FSHelpers.Write16(stream, Vertices[i]);

                FSHelpers.Write16(stream, Unknown1);
                FSHelpers.Write16(stream, Unknown2);
            }
        }

        class Type
        {
            public const int Size = 52;

            public int NameOffset;
            public Vector3 Unknown1;
            public int Unknown2, Unknown3;
            public Vector3 Unknown4;
            public int Unknown5, Unknown6, Unknown7, Unknown8;

            public string Name = string.Empty;

            public void Load(byte[] data, ref int offset)
            {
                NameOffset = FSHelpers.Read32(data, offset);
                Unknown1 = new Vector3(
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 4)),
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 8)),
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 12)));
                Unknown2 = FSHelpers.Read32(data, offset + 16);
                Unknown3 = FSHelpers.Read32(data, offset + 20);
                Unknown4 = new Vector3(
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 24)),
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 28)),
                    FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, offset + 32)));
                Unknown5 = FSHelpers.Read32(data, (int)offset + 36);
                Unknown6 = FSHelpers.Read32(data, (int)offset + 40);
                Unknown7 = FSHelpers.Read32(data, (int)offset + 44);
                Unknown8 = FSHelpers.Read32(data, (int)offset + 48);

                Name = FSHelpers.ReadString(data, (int)NameOffset);

                offset += Size;
            }

            public void Save(BinaryWriter stream)
            {
                FSHelpers.Write32(stream, NameOffset);
                FSHelpers.WriteVector(stream, Unknown1);
                FSHelpers.Write32(stream, Unknown2);
                FSHelpers.Write32(stream, Unknown3);
                FSHelpers.WriteVector(stream, Unknown4);
                FSHelpers.Write32(stream, Unknown5);
                FSHelpers.Write32(stream, Unknown6);
                FSHelpers.Write32(stream, Unknown7);
                FSHelpers.Write32(stream, Unknown8);
            }
        }

        /*public class RenderData : IRenderable
        {
            public RenderData(int vertexCount, int indexCount)
            {
                _vertexCount = vertexCount;
                _indexCount = indexCount;
            }

            public Vector3[] Vertexes;
            public int[] Indexes;
            public override int[] GetIndices()
            {
                return Indexes;
            }

            public override Vector3[] GetVerts()
            {
                return Vertexes;
            }

            public override float[] GetTexCoords()
            {
                return new[]
                {
                    0f, 0f,
                    1f, 0f,
                    1f, 1f,
                    0f, 1f,
                };
            }
        }*/
        #endregion

        //public RenderData Renderable;
        private byte[] _byteCopy;

        public override void Load(byte[] data)
        {
            FileHeader header = new FileHeader();
            int offset = 0;
            header.Load(data, ref offset);

            offset = header.VertexOffset;
            List<Vector3> verts = new List<Vector3>();
            for (int i = 0; i < header.VertexCount; i++)
            {
                Vertex vert = new Vertex();
                vert.Load(data, ref offset);
                verts.Add(vert.Position);
            }

            List<int> tris = new List<int>();
            offset = header.TriangleOffset;
            for (int i = 0; i < header.TriangleCount; i++)
            {
                Triangle tri = new Triangle();
                tri.Load(data, ref offset);
                for(int k = 0; k < 3; k++)
                    tris.Add(tri.Vertices[k]);

                /*Renderable = new RenderData(verts.Count, tris.Count);
                Renderable.Vertexes = verts.ToArray();
                Renderable.Indexes = tris.ToArray();*/
            }

            _byteCopy = data;
        }

        public override void Save(BinaryWriter stream)
        {
            FSHelpers.WriteArray(stream, _byteCopy);
        }
    }
}