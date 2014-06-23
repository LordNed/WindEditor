using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor;
using WindViewer.Editor.Renderer;
using WindViewer.Forms;

namespace WindViewer.FileFormats
{
    /// <summary>
    /// The behemoth of file formats, the JStudioModel. Well I think it's a JStudioModel, it's a J3D and their tools are called JStudio, 
    /// so good enough. Anyways. It renders things.
    /// 
    /// Many thanks to @Drakonite, shuffle2, JMC47, and phire for helping me understand the various
    /// bits and pieces of the format, and some debugging ideas!
    /// 
    /// Test extra commit. 
    /// </summary>
    public class JStudioModel : BaseArchiveFile
    {
        public enum ArrayTypes
        {
            PositionMatrixIndex, Tex0MatrixIndex, Tex1MatrixIndex, Tex2MatrixIndex, Tex3MatrixIndex,
            Tex4MatrixIndex, Tex5MatrixIndex, Tex6MatrixIndex, Tex7MatrixIndex,
            Position, Normal, Color0, Color1, Tex0, Tex1, Tex2, Tex3, Tex4, Tex5, Tex6, Tex7,
            PositionMatrixArray, NormalMatrixArray, TextureMatrixArray, LitMatrixArray, NormalBinormalTangent,
            MaxAttr, NullAttr = 0xFF,
        }

        public enum DataTypes
        {
            Unsigned8 = 0x0,
            Signed8 = 0x1,
            Unsigned16 = 0x2,
            Signed16 = 0x3,
            Float32 = 0x4,
            RGBA8 = 0x5,
        }

        public enum TextureFormats
        {
            RGB565 = 0x0,
            RGB888 = 0x1,
            RGBX8 = 0x2,
            RGBA4 = 0x3,
            RGBA6 = 0x4,
            RGBA8 = 0x5,
        }

        public enum PrimitiveTypes
        {
            Points = 0xB8,
            Lines = 0xA8,
            LineStrip = 0xB0,
            Triangles = 0x80,
            TriangleStrip = 0x98,
            TriangleFan = 0xA0,
            Quads = 0x80,
        }

        //Temp in case I fuck up
        private byte[] _origDataCache;

        private List<BaseChunk> _chunkList;

        public override void Load(byte[] data)
        {
            _origDataCache = data;

            _chunkList = new List<BaseChunk>();

            int dataOffset = 0;

            Header header = new Header();
            header.Load(data, ref dataOffset);

            //STEP 1: We're going to load all of the data out of memory straight into the chunks that
            //hold them. These should be relatively close to accurate copies of the file format, but
            //the Wiki is probably a better place to draw that information from.
            for (int i = 0; i < header.ChunkCount; i++)
            {
                BaseChunk baseChunk = null;

                //Read the first four bytes to get the tag.
                string tagName = FSHelpers.ReadString(data, dataOffset, 4);

                switch (tagName)
                {
                    case "INF1":
                        baseChunk = new Inf1Chunk();
                        break;
                    case "VTX1":
                        baseChunk = new Vtx1Chunk();
                        break;
                    case "EVP1":
                        baseChunk = new Evp1Chunk();
                        break;
                    case "DRW1":
                        baseChunk = new Drw1Chunk();
                        break;
                    case "JNT1":
                        baseChunk = new Jnt1Chunk();
                        break;
                    case "SHP1":
                        baseChunk = new Shp1Chunk();
                        break;
                    case "TEX1":
                        baseChunk = new Tex1Chunk();
                        break;
                    case "MAT3":
                    case "ANK1":
                    default:
                        Console.WriteLine("Found unknown chunk {0}!", tagName);
                        baseChunk = new DefaultChunk();
                        break;
                }

                baseChunk.Load(data, ref dataOffset);
                _chunkList.Add(baseChunk);
            }

            //STEP 2: Once all of the data is loaded, we're going to pull different data from
            //different chunks to transform the data into something
            vertData = BuildVertexArraysFromFile(data);


            //Haaaaaaaack there goes my lung. Generate a vbo, bind and upload data.
            GL.GenBuffers(1, out _glVbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Count * 36), vertData.ToArray(), BufferUsageHint.StaticDraw);

            J3DRenderer.Draw += J3DRendererOnDraw;
            J3DRenderer.Bind += J3DRendererOnBind;
        }

        private List<VertexFormatLayout> vertData; 

        public override void Save(BinaryWriter stream)
        {
            stream.Write(_origDataCache);
        }

        private struct PrimitiveList
        {
            public int VertexStart;
            public int VertexCount;
        }

        private List<PrimitiveList> _renderList = new List<PrimitiveList>();

        private void J3DRendererOnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);

        }

        private int _numPrimRender;
        private float _lastChange;

        private void J3DRendererOnDraw()
        {
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            //GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 10);

            if (EditorHelpers.GetKey(Keys.O) && Math.Abs(MainEditor.Time - _lastChange) > 0.02f)
            {
                _numPrimRender++;
                Console.WriteLine("Rendering: " + _numPrimRender);
                _lastChange = MainEditor.Time;
            }
            if (EditorHelpers.GetKey(Keys.P) && Math.Abs(MainEditor.Time - _lastChange) > 0.02f)
            {
                _numPrimRender--;
                if (_numPrimRender < 0)
                    _numPrimRender = 0;
                Console.WriteLine("Rendering: " + _numPrimRender);
                _lastChange = MainEditor.Time;
            }
            if (EditorHelpers.GetKey(Keys.I))
                _numPrimRender = _renderList.Count - 1;


            for(int i = 0; i < Math.Min(_numPrimRender, _renderList.Count); i++)
            {
                PrimitiveList primitive = _renderList[i];
                GL.DrawArrays(PrimitiveType.TriangleStrip, primitive.VertexStart, primitive.VertexCount);
            }
        }

        private int _glVbo;

        public T GetChunkByType<T>() where T : class
        {
            foreach (BaseChunk file in _chunkList)
            {
                if (file is T)
                    return file as T;
            }

            return default(T);
        }

        public struct VertexFormatLayout
        {
            public Vector3 Position;
            public Vector4 Color;
            public Vector2 TexCoord;
        }


        private List<VertexFormatLayout> BuildVertexArraysFromFile(byte[] data)
        {
            Vtx1Chunk vtxChunk = GetChunkByType<Vtx1Chunk>();
            List<VertexFormatLayout> finalData = new List<VertexFormatLayout>();

            Shp1Chunk shp1Chunk = GetChunkByType<Shp1Chunk>();


            //Now, let's try to get our data.
            for (uint i = 0; i < shp1Chunk.GetBatchCount(); i++)
            {
                Shp1Chunk.Batch batch = shp1Chunk.GetBatch(i);
                Shp1Chunk.BatchPacketLocation packetLoc = shp1Chunk.GetBatchPacketLocation(i);

                for (int p = 0; p < batch.PacketCount; p++)
                {
                    uint numPrimitiveBytesRead = packetLoc.Offset;

                    while (numPrimitiveBytesRead < packetLoc.Offset + packetLoc.PacketSize)
                    {
                        //The data is going to be stored as:
                        //[Primitive][Primitive.VertexCount * (AttributeType.ElementCount * sizeof(AttributeType.DataType))]

                        Shp1Chunk.BatchPrimitive primitive = new Shp1Chunk.BatchPrimitive();
                        primitive.Load(shp1Chunk._dataCopy, shp1Chunk._primitiveDataOffset + numPrimitiveBytesRead);
                        numPrimitiveBytesRead += Shp1Chunk.BatchPrimitive.Size;

                        if (primitive.Type == 0)
                        {
                            Console.WriteLine("Delta: " +
                                              ((int)(packetLoc.Offset + packetLoc.PacketSize)-numPrimitiveBytesRead ).ToString());
                            //I think we've hit the end and we should break here.
                            break;
                        }

                        var primList = new PrimitiveList();
                        primList.VertexCount = primitive.VertexCount;
                        primList.VertexStart = finalData.Count;

                        _renderList.Add(primList);

                        

                        if (primitive.Type != PrimitiveTypes.TriangleStrip)
                            Console.WriteLine("Well there you. go.");


                        //Now, for each Vertex we're going to read the right number of bytes... we're hacking it in this case
                        //to fixed amount of 8...

                        for (int vert = 0; vert < primitive.VertexCount; vert++)
                        {
                            VertexFormatLayout newVertex = new VertexFormatLayout();
                            for (uint vertIndex = 0; vertIndex < 3; vertIndex++)
                            {
                                ushort curIndex =
                                    (ushort)FSHelpers.Read16(shp1Chunk._dataCopy, (int)(shp1Chunk._primitiveDataOffset + numPrimitiveBytesRead));

                                switch (vertIndex)
                                {
                                    case 0:
                                        newVertex.Position = vtxChunk.GetPosition(curIndex);
                                        break;
                                    case 1:
                                        newVertex.Color = vtxChunk.GetColor0(curIndex);
                                        break;
                                    case 2:
                                        newVertex.TexCoord = vtxChunk.GetTex0(curIndex, 8);
                                        break;
                                }

                                numPrimitiveBytesRead += 2; //Should be element size, but w/e.
                            }

                            //Add our vertex to our list of Vertexes
                            finalData.Add(newVertex);
                        }
                    }
                }

                Console.WriteLine("Finished batch {0}, triangleStrip count: {1}", i, _renderList.Count);
            }

            return finalData;
        }

        #region File Formats

        private class Header
        {
            public string Magic;
            public string Type;
            public int FileSize;
            public int ChunkCount;

            public void Load(byte[] data, ref int offset)
            {
                Magic = FSHelpers.ReadString(data, 0, 4);
                Type = FSHelpers.ReadString(data, 4, 4);
                FileSize = FSHelpers.Read32(data, 8);
                ChunkCount = FSHelpers.Read32(data, 12);

                //Four variables are followed by an unused tag and some padding.
                offset += 32;
            }

            public void Save(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Magic, 4);
                FSHelpers.WriteString(stream, Type, 4);
                FSHelpers.Write32(stream, FileSize);
                FSHelpers.Write32(stream, ChunkCount);

                //Write in the unused tag and padding
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0x53565233, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFFFFFF, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFFFFFF, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFFFFFF, 4));
            }
        }

        private class BaseChunk
        {
            public string Type;
            public int ChunkSize;

            public byte[] _dataCopy;

            public virtual void Load(byte[] data, ref int offset)
            {
                Type = FSHelpers.ReadString(data, offset, 4);
                ChunkSize = FSHelpers.Read32(data, offset + 4);

                _dataCopy = FSHelpers.ReadN(data, offset, ChunkSize);
            }

            public virtual void Save(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Type, 4);
                FSHelpers.Write32(stream, ChunkSize);
            }
        }

        private class DefaultChunk : BaseChunk
        {
            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                offset += ChunkSize;
            }
        }

        private class Inf1Chunk : BaseChunk
        {
            private ushort _unknown1; //? Anim related
            private uint _batchCount;
            private uint _vertexCount;
            private uint _hierarchyDataOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _unknown1 = (ushort)FSHelpers.Read16(data, offset + 8);
                _batchCount = (uint) FSHelpers.Read32(data, offset + 12); //2 bytes padding after Unknown1
                _vertexCount = (uint)FSHelpers.Read32(data, offset + 16);
                _hierarchyDataOffset = (uint)FSHelpers.Read32(data, offset + 20);

                offset += ChunkSize;
            }

            //"SceneGraphRaw"
            public class HierarchyData
            {
                public enum HierarchyDataTypes : ushort
                {
                    Finish = 0x0, NewNode = 0x01, EndNode = 0x02,
                    Joint = 0x10, Material = 0x11, Shape = 0x12,
                }

                public HierarchyDataTypes Type { get; private set; }
                public ushort Index { get; private set; }

                public void Load(byte[] data, uint offset)
                {
                    Type = (HierarchyDataTypes)FSHelpers.Read16(data, (int)offset);
                    Index = (ushort)FSHelpers.Read16(data, (int)offset + 0x2);
                }

                public const uint Size = 4;
            }

            public uint GetVertexCount()
            {
                return _vertexCount;
            }

            public uint GetBatchCount()
            {
                return _batchCount;
            }

            public ushort GetUnknown1()
            {
                return _unknown1;
            }

            public List<HierarchyData> GetHierarchyData()
            {
                var data = new List<HierarchyData>();
                HierarchyData curNode;
                uint readOffset = 0;

                do
                {
                    curNode = new HierarchyData();
                    curNode.Load(_dataCopy, _hierarchyDataOffset + readOffset);
                    data.Add(curNode);

                    readOffset += HierarchyData.Size;
                } while (curNode.Type != HierarchyData.HierarchyDataTypes.Finish);

                return data;
            }
        }

        private class Vtx1Chunk : BaseChunk
        {
            public class VertexFormat
            {
                public ArrayTypes ArrayType;
                public uint ArrayCount;
                public DataTypes DataType;
                public byte DecimalPoint;

                public void Load(byte[] data, int offset)
                {
                    ArrayType = (ArrayTypes)FSHelpers.Read32(data, offset);
                    ArrayCount = (uint)FSHelpers.Read32(data, offset + 4);
                    DataType = (DataTypes)FSHelpers.Read32(data, offset + 8);
                    DecimalPoint = FSHelpers.Read8(data, offset + 12);
                }

                public const int Size = 16;
            }

            public enum VertexDataTypes
            {
                Position = 0,
                Normal = 1,
                Color0 = 3,
                Tex0 = 5,
            }

            private int _vertexFormatsOffset;
            private int[] VertexDataOffsets = new int[13];

            //Not part of the header
            public List<VertexFormat> VertexFormats = new List<VertexFormat>();

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _vertexFormatsOffset = FSHelpers.Read32(data, offset + 0x8);
                for (int i = 0; i < 13; i++)
                    VertexDataOffsets[i] = FSHelpers.Read32(data, (offset + 0xC) + (i * 0x4));

                //Load the VertexFormats 
                int dataOffsetCpy = offset + _vertexFormatsOffset;
                VertexFormat vFormat;
                do
                {
                    vFormat = new VertexFormat();
                    vFormat.Load(data, dataOffsetCpy);
                    VertexFormats.Add(vFormat);

                    dataOffsetCpy += VertexFormat.Size;
                } while (vFormat.ArrayType != ArrayTypes.NullAttr);

                offset += ChunkSize;
            }
            
            public VertexFormat GetVertexFormat(int index)
            {
                VertexFormat vtxFmt = new VertexFormat();
                vtxFmt.Load(_dataCopy, _vertexFormatsOffset + (index * VertexFormat.Size));
                return vtxFmt;
            }

            public Vector3 GetPosition(int index)
            {
                Vector3 newPos = new Vector3();
                for(int i = 0; i < 3; i++)
                    newPos[i] = FSHelpers.ReadFloat(_dataCopy, VertexDataOffsets[(int)VertexDataTypes.Position] + (index * Vector3.SizeInBytes) + (i*4));

                return newPos;
            }

            public Vector4 GetColor0(int index)
            {
                Vector4 newColor = new Vector4();
                for(int i = 0; i < 4; i++)
                    newColor[i] = FSHelpers.Read8(_dataCopy, VertexDataOffsets[(int)VertexDataTypes.Color0] + (index * 4) + i)/255f;

                return newColor;
            }

            public Vector2 GetTex0(int index, int decimalPlace)
            {
                Vector2 newTexCoord = new Vector2();
                float scaleFactor = (float) Math.Pow(0.5, decimalPlace);

                for (int i = 0; i < 2; i++)
                    newTexCoord[i] = FSHelpers.Read16(_dataCopy,
                        VertexDataOffsets[(int) VertexDataTypes.Tex0] + (index*4) + (i*0x2))*scaleFactor;

                return newTexCoord;
            }

        }

        private class Evp1Chunk : BaseChunk
        {
            public ushort SectionCount;
            public uint CountsArrayOffset;
            public uint IndicesOffset;
            public uint WeightsOffset;
            public uint MatrixDataOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                SectionCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                CountsArrayOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                IndicesOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                WeightsOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                MatrixDataOffset = (uint)FSHelpers.Read32(data, offset + 0x18);


                offset += ChunkSize;
            }
        }

        private class Drw1Chunk : BaseChunk
        {
            public ushort SectionCount;
            public uint IsWeightedOffset;
            public uint DataOffset;

            //Not part of header
            public bool[] IsWeighted;
            public ushort[] Data; //Related to that thing in collision perhaps?

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                SectionCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                IsWeightedOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                DataOffset = (uint)FSHelpers.Read32(data, offset + 0x10);

                IsWeighted = new bool[SectionCount];
                Data = new ushort[SectionCount];

                for (int i = 0; i < SectionCount; i++)
                {
                    IsWeighted[i] = Convert.ToBoolean(FSHelpers.Read8(data, (int)(offset + IsWeightedOffset + i)));
                    Data[i] = (ushort)FSHelpers.Read16(data, (int)(offset + DataOffset + (i * 2)));
                }

                offset += ChunkSize;
            }
        }

        private class Jnt1Chunk : BaseChunk
        {
            public ushort SectionCount;
            public uint EntryOffset;
            public uint UnknownOffset;
            public uint StringTableOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                SectionCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                EntryOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                UnknownOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                StringTableOffset = (uint)FSHelpers.Read32(data, offset + 0x14);

                offset += ChunkSize;
            }

        }

        /// <summary>
        /// Since primitives have different attributes (ie: some have position, some have normals, some have texcoords)
        /// each different primitive is placed into a "batch", and each batch ahs a fixed set of vertex attributes. Each 
        /// batch can then have several "packets", which are used for animations. A packet is a collection of primitives.
        /// </summary>
        private class Shp1Chunk : BaseChunk
        {
            public class Batch
            {
                public byte MatrixType;
                public ushort PacketCount;
                public ushort AttribOffset;
                public ushort FirstMatrixIndex;
                public ushort PacketIndex;

                public float Unknown;
                public Vector3 BoundingBoxMin;
                public Vector3 BoundingBoxMax;

                //Bleh
                public List<BatchAttribute> BatchAttributes = new List<BatchAttribute>();

                public void Load(byte[] data, uint offset)
                {
                    MatrixType = FSHelpers.Read8(data, (int)offset);
                    PacketCount = (ushort)FSHelpers.Read16(data, (int)offset + 0x2);
                    AttribOffset = (ushort)FSHelpers.Read16(data, (int)offset + 0x4);
                    FirstMatrixIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x6);
                    PacketIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x8);

                    Unknown = FSHelpers.ReadFloat(data, (int)offset + 0xC);
                    BoundingBoxMin = FSHelpers.ReadVector3(data, (int)offset + 0x10);
                    BoundingBoxMax = FSHelpers.ReadVector3(data, (int)offset + 0x1C);
                }

                public const uint Size = 40;
            }

            public class BatchAttribute
            {
                public ArrayTypes AttribType;
                public DataTypes DataType;

                public void Load(byte[] data, ref int offset)
                {
                    AttribType = (ArrayTypes)FSHelpers.Read32(data, offset);
                    DataType = (DataTypes)FSHelpers.Read32(data, offset + 0x4);

                    offset += 0x8;
                }
            }

            public struct BatchPacketLocation
            {
                public uint PacketSize;
                public uint Offset;

                //Not part of the BPLocation header
                public const uint Size = 8;
            }

            public class BatchPrimitive
            {
                public PrimitiveTypes Type;
                public ushort VertexCount;

                public void Load(byte[] data, uint offset)
                {
                    Type = (PrimitiveTypes) FSHelpers.Read8(data, (int)offset);
                    VertexCount = (ushort)FSHelpers.Read16(data, (int)offset + 0x1);
                }

                public const uint Size = 3;
            }

            private ushort _batchCount;
            private uint _batchDataOffset;
            private uint _unknownTableOffset;
            private uint _zero;
            private uint _attributeOffset;
            private uint _matrixTableOffset;
            public uint _primitiveDataOffset;
            private uint _matrixDataOffset;
            private uint _packetLocationOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                //Load information from header
                _batchCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _batchDataOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _unknownTableOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _zero = (uint)FSHelpers.Read32(data, offset + 0x14);
                _attributeOffset = (uint)FSHelpers.Read32(data, offset + 0x18);
                _matrixTableOffset = (uint)FSHelpers.Read32(data, offset + 0x1C);
                _primitiveDataOffset = (uint)FSHelpers.Read32(data, offset + 0x20);
                _matrixDataOffset = (uint)FSHelpers.Read32(data, offset + 0x24);
                _packetLocationOffset = (uint)FSHelpers.Read32(data, offset + 0x28);

                offset += ChunkSize;
            }

            public Batch GetBatch(uint index)
            {
                Batch newBatch = new Batch();
                newBatch.Load(_dataCopy, _batchDataOffset + (index * Batch.Size));

                return newBatch;
            }

            public ushort GetBatchCount()
            {
                return _batchCount;
            }

            public BatchPacketLocation GetBatchPacketLocation(uint index)
            {
                BatchPacketLocation newBp = new BatchPacketLocation();
                newBp.PacketSize = (uint)FSHelpers.Read32(_dataCopy, (int)(_packetLocationOffset + (index*BatchPacketLocation.Size) + 0x0));
                newBp.Offset = (uint)FSHelpers.Read32(_dataCopy, (int)(_packetLocationOffset + (index * BatchPacketLocation.Size) + 0x4));

                return newBp;
            }
        }

        /// <summary>
        /// The Tex1 chunk stores a series of BTI images within the BMD/BDL format.
        /// The BMD/BDL also stores a string table which contains the file name for
        /// each image. The TEX1 chunk stores _textureCount headers immediately after
        /// the chunk, and then the data for those textures comes.
        /// </summary>
        private class Tex1Chunk : BaseChunk
        {
            private ushort _textureCount;
            private uint _textureHeaderOffset;
            private uint _stringTableOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);
                _textureCount = (ushort) FSHelpers.Read16(data, offset + 0x08);
                _textureHeaderOffset = (uint) FSHelpers.Read32(data, offset + 0xC);
                _stringTableOffset = (uint) FSHelpers.Read32(data, offset + 0x10);

                offset += ChunkSize;

                for (uint i = 0; i < _textureCount; i++)
                {
                    BTI tex = GetTexture(i);
                }
                
            }

            public BTI GetTexture(uint index)
            {
                if (index > _textureCount)
                    new Exception("Invalid index provided to GetTexture!");

                uint dataOffset = _textureHeaderOffset + (index*BTI.FileHeader.Size);
                BTI tex = new BTI();

                //Before load the texture we need to modify the source byte array, because reasons.
                int headerOffset = (int) (((index+1)*32) );

                tex.Load(_dataCopy, dataOffset, headerOffset);

                return tex;
            }

            public ushort GetTextureCount()
            {
                return _textureCount;
            }
        }

        #endregion
    }
}
