using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using WindViewer.Editor;

namespace WindViewer.FileFormats
{
    /// <summary>
    /// The behemoth of file formats, the JStudioModel. Well I think it's a JStudioModel, it's a J3D and their tools are called JStudio, 
    /// so good enough. Anyways. It renders things.
    /// 
    /// Many thanks to @Drakonite, shuffle2, JMC47, Sage of Mirrors, Jasper, and phire for helping me understand the various
    /// bits and pieces of the format, and some debugging ideas! Also thanks to @slime73 & others for rendering help.
    /// </summary>
    public class J3DFormat
    {
        #region Public Enums
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
            Rgba8 = 0x5,
        }

        public enum TextureFormats
        {
            Rgb565 = 0x0,
            Rgb888 = 0x1,
            Rgbx8 = 0x2,
            Rgba4 = 0x3,
            Rgba6 = 0x4,
            Rgba8 = 0x5,
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

        public enum HierarchyDataTypes : ushort
        {
            Finish = 0x0, NewNode = 0x01, EndNode = 0x02,
            Joint = 0x10, Material = 0x11, Batch = 0x12,
        }
        #endregion

        public InfoChunk Info;
        public VertexChunk Vertexes;
        public EnvelopeChunk Envelopes;
        public DrawChunk Draw;
        public JointChunk Joints;
        public ShapeChunk Shapes;
        public Material3Chunk Materials;
        public TextureChunk Textures;

        public void Load(byte[] data)
        {
            int dataOffset = 0;

            var header = new Header();
            header.Load(data, ref dataOffset);

            for (int i = 0; i < header.ChunkCount; i++)
            {
                BaseChunk baseChunk;

                //Read the first four bytes to get the tag.
                string tagName = FSHelpers.ReadString(data, dataOffset, 4);

                switch (tagName)
                {
                    case "INF1":
                        baseChunk = new InfoChunk();
                        Info = baseChunk as InfoChunk;
                        break;
                    case "VTX1":
                        baseChunk = new VertexChunk();
                        Vertexes = baseChunk as VertexChunk;
                        break;
                    case "EVP1":
                        baseChunk = new EnvelopeChunk();
                        Envelopes = baseChunk as EnvelopeChunk;
                        break;
                    case "DRW1":
                        baseChunk = new DrawChunk();
                        Draw = baseChunk as DrawChunk;
                        break;
                    case "JNT1":
                        baseChunk = new JointChunk();
                        Joints = baseChunk as JointChunk;
                        break;
                    case "SHP1":
                        baseChunk = new ShapeChunk();
                        Shapes = baseChunk as ShapeChunk;
                        break;
                    case "TEX1":
                        baseChunk = new TextureChunk();
                        Textures = baseChunk as TextureChunk;
                        break;
                    case "MAT3":
                        baseChunk = new Material3Chunk();
                        Materials = baseChunk as Material3Chunk;
                        break;
                    case "ANK1":
                    default:
                        Console.WriteLine("Found unknown chunk {0}!", tagName);
                        baseChunk = new DefaultChunk();
                        break;
                }

                baseChunk.Load(data, ref dataOffset);
            }
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

        public class BaseChunk
        {
            public string Type;
            public int ChunkSize;

            protected byte[] _dataCopy;

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

        /// <summary>
        /// Used for undefined chunks to properly incriment the data stream
        /// as we read in.
        /// </summary>
        private class DefaultChunk : BaseChunk
        {
            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                offset += ChunkSize;
            }
        }

        public class InfoChunk : BaseChunk
        {
            /* All other chunks have a 4 byte tag, a 4 byte chunk size, and then
             * a 2 byte "entry" count (+ 2 more bytes padding) at the start. This
             * chunk doesn't seem to ever initialize the entry count and the only 
             * varying data in this chunk seems uses a different method of finding
             * the last entry. */
            private ushort _unusedEntryCount; //Not unused, a lot of Links models have it.
            private uint _batchCount;
            private uint _vertexCount;
            private uint _hierarchyDataOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _unusedEntryCount = (ushort)FSHelpers.Read16(data, offset + 8);
                _batchCount = (uint)FSHelpers.Read32(data, offset + 12); //2 bytes padding after _unusedEntryCount
                _vertexCount = (uint)FSHelpers.Read32(data, offset + 16);
                _hierarchyDataOffset = (uint)FSHelpers.Read32(data, offset + 20);

                offset += ChunkSize;
            }

            public uint GetVertexCount()
            {
                return _vertexCount;
            }

            public uint GetBatchCount()
            {
                return _batchCount;
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
                } while (curNode.Type != HierarchyDataTypes.Finish);

                return data;
            }
        }

        public class HierarchyData
        {
            public HierarchyDataTypes Type { get; private set; }
            public ushort Index { get; private set; }

            public void Load(byte[] data, uint offset)
            {
                Type = (HierarchyDataTypes)FSHelpers.Read16(data, (int)offset);
                Index = (ushort)FSHelpers.Read16(data, (int)offset + 0x2);
            }

            public const uint Size = 4;
        }

        public class VertexChunk : BaseChunk
        {
            private uint _vertexFormatsOffset;
            private uint _positionDataOffset;
            private uint _normalDataOffset;
            private uint _normalBinormalTangentDataOffset; //Presumed
            private uint _color0DataOffset;
            private uint _color1DataOffset; //Presumed
            private uint _tex0DataOffset;
            private uint _tex1DataOffset; //Presumed
            private uint _tex2DataOffset; //Presumed
            private uint _tex3DataOffset; //Presumed
            private uint _tex4DataOffset; //Presumed
            private uint _tex5DataOffset; //Presumed
            private uint _tex6DataOffset; //Presumed
            private uint _tex7DataOffset; //Presumed

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _vertexFormatsOffset = (uint)FSHelpers.Read32(data, offset + 0x8);
                _positionDataOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _normalDataOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _normalBinormalTangentDataOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                _color0DataOffset = (uint)FSHelpers.Read32(data, offset + 0x18);
                _color1DataOffset = (uint)FSHelpers.Read32(data, offset + 0x1C);
                _tex0DataOffset = (uint)FSHelpers.Read32(data, offset + 0x20);
                _tex1DataOffset = (uint)FSHelpers.Read32(data, offset + 0x24);
                _tex2DataOffset = (uint)FSHelpers.Read32(data, offset + 0x28);
                _tex3DataOffset = (uint)FSHelpers.Read32(data, offset + 0x2C);
                _tex4DataOffset = (uint)FSHelpers.Read32(data, offset + 0x30);
                _tex5DataOffset = (uint)FSHelpers.Read32(data, offset + 0x34);
                _tex6DataOffset = (uint)FSHelpers.Read32(data, offset + 0x38);
                _tex7DataOffset = (uint)FSHelpers.Read32(data, offset + 0x3C);

                offset += ChunkSize;
            }

            public VertexFormat GetVertexFormat(uint index)
            {
                VertexFormat vtxFmt = new VertexFormat();
                vtxFmt.Load(_dataCopy, _vertexFormatsOffset + (index * VertexFormat.Size));
                return vtxFmt;
            }


            public List<VertexFormat> GetAllVertexFormats()
            {
                var allFormats = new List<VertexFormat>();
                VertexFormat vFormat;
                uint dataOffset = _vertexFormatsOffset;
                do
                {
                    vFormat = new VertexFormat();
                    vFormat.Load(_dataCopy, dataOffset);
                    allFormats.Add(vFormat);

                    dataOffset += VertexFormat.Size;
                } while (vFormat.ArrayType != ArrayTypes.NullAttr);

                return allFormats;
            }

            public Vector3 GetPosition(uint index)
            {
                Vector3 newPos = new Vector3();
                for (int i = 0; i < 3; i++)
                    newPos[i] = FSHelpers.ReadFloat(_dataCopy, (int)(_positionDataOffset + (index * Vector3.SizeInBytes) + (i * 4)));

                return newPos;
            }

            public Vector3 GetNormal(uint index, int decimalPlace)
            {
                Vector3 newNormal = new Vector3();
                float scaleFactor = (float)Math.Pow(0.5, decimalPlace);

                for (int i = 0; i < 3; i++)
                    newNormal[i] = FSHelpers.Read16(_dataCopy,
                        (int)(_normalDataOffset + (index * 6) + (i * 2))) * scaleFactor;

                return newNormal;
            }

            public Vector4 GetColor0(int index)
            {
                Vector4 newColor = new Vector4();
                for (int i = 0; i < 4; i++)
                    newColor[i] = FSHelpers.Read8(_dataCopy, (int)_color0DataOffset + (index * 4) + i) / 255f;

                return newColor;
            }

            public Vector2 GetTex0(int index, int decimalPlace)
            {
                Vector2 newTexCoord = new Vector2();
                float scaleFactor = (float)Math.Pow(0.5, decimalPlace);

                for (int i = 0; i < 2; i++)
                    newTexCoord[i] = FSHelpers.Read16(_dataCopy, (int)_tex0DataOffset + (index * 4) + (i * 0x2)) * scaleFactor;

                return newTexCoord;
            }

        }
        
        public class VertexFormat
        {
            public ArrayTypes ArrayType { get; private set; }
            public uint ArrayCount { get; private set; }
            public DataTypes DataType { get; private set; }
            public byte DecimalPoint { get; private set; }

            public void Load(byte[] data, uint offset)
            {
                ArrayType = (ArrayTypes)FSHelpers.Read32(data, (int)offset);
                ArrayCount = (uint)FSHelpers.Read32(data, (int)offset + 4);
                DataType = (DataTypes)FSHelpers.Read32(data, (int)offset + 8);
                DecimalPoint = FSHelpers.Read8(data, (int)offset + 12);
            }

            public const int Size = 16;
        }

        public class EnvelopeChunk : BaseChunk
        {
            private ushort _sectionCount; //BST has 3 
            private uint _countsArrayOffset; //BST values: 2, 2, 2
            private uint _indicesOffset; //BST's is 12 bytes in length. (six shorts) 06 07 00 06 00 07 (pairs?)
            private uint _weightsOffset; //24 bytes length, 6 floats. (one per indice) (pairs + weights to each side of the pair)
            private uint _matrixDataOffset; //3x4 float array. Indeded into by something else. 15 in BST

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _sectionCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _countsArrayOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _indicesOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _weightsOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                _matrixDataOffset = (uint)FSHelpers.Read32(data, offset + 0x18);


                offset += ChunkSize;
            }

            public byte GetCount(uint index)
            {
                return FSHelpers.Read8(_dataCopy, (int)(_countsArrayOffset + index));
            }

            public ushort GetIndex(uint index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)(_indicesOffset + (index * 0x2)));
            }

            public float GetWeight(uint index)
            {
                return FSHelpers.ReadFloat(_dataCopy, (int)(_weightsOffset + (index * 0x4)));
            }

            public Matrix3x4 GetMatrix(ushort index)
            {
                Matrix3x4 matrix = new Matrix3x4();
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        float rawFloat = FSHelpers.ReadFloat(_dataCopy, (int)_matrixDataOffset + (index * (3 * 4 * 4)) + ((row * 4 * 4) + (col * 4)));
                        matrix[row, col] = (float)Math.Round(rawFloat, 4);
                    }
                }

                return matrix;
            }
        }

        public class DrawChunk : BaseChunk
        {
            private ushort _sectionCount;
            private uint _isWeightedOffset;
            private uint _dataOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _sectionCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _isWeightedOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _dataOffset = (uint)FSHelpers.Read32(data, offset + 0x10);

                offset += ChunkSize;
            }

            public bool IsWeighted(ushort index)
            {
                return Convert.ToBoolean(FSHelpers.Read8(_dataCopy, (int)_isWeightedOffset + index));
            }

            public ushort GetIndex(ushort index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_dataOffset + (index * 0x2));
            }
        }

        public class JointChunk : BaseChunk
        {
            private ushort _jointCount;
            private uint _entryOffset;
            private uint _stringIdTableOffset;
            private uint _stringTableOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _jointCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _entryOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _stringIdTableOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _stringTableOffset = (uint)FSHelpers.Read32(data, offset + 0x14);

                offset += ChunkSize;
            }

            public ushort GetUnknown(ushort index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_stringIdTableOffset + (index * 0x2));
            }

            public Joint GetJoint(ushort index)
            {
                Joint joint = new Joint();
                joint.Load(_dataCopy, _entryOffset + (index * Joint.Size));

                return joint;
            }

            public ushort GetStringIndex(ushort index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_stringIdTableOffset + (index * 0x2));
            }

            public ushort GetStringTableSize()
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_stringTableOffset);
            }

            public StringTableEntry GetStringTableEntry(ushort index)
            {
                var ste = new StringTableEntry();
                /* String Table Header */
                ste.UnknownIndex = (ushort)FSHelpers.Read16(_dataCopy, (int)_stringTableOffset + 0x4 + (index * 0x4));
                ste.StringOffset = (ushort)FSHelpers.Read16(_dataCopy, (int)_stringTableOffset + 0x6 + (index * 0x4));

                return ste;
            }

            public string GetString(StringTableEntry entry)
            {
                return FSHelpers.ReadString(_dataCopy, (int)_stringTableOffset + entry.StringOffset);
            }
        }

        public class Joint
        {
            private ushort _unknown1; //0 = has unknown2, bboxmin, bboxmax. 
            private byte _unknown2; //Unknown
            private Vector3 _scale;
            private HalfRotation _rotation;
            private Vector3 _translation;
            private float _unknown3;
            private Vector3 _boundingBoxMin;
            private Vector3 _boundingBoxMax;

            public void Load(byte[] data, uint offset)
            {
                _unknown1 = (ushort)FSHelpers.Read16(data, (int)offset + 0x0);
                //One byte padding.
                _unknown2 = (byte)FSHelpers.Read16(data, (int)offset + 0x3);
                _scale = FSHelpers.ReadVector3(data, (int)offset + 0x4);
                _rotation = FSHelpers.ReadHalfRot(data, offset + 0x10);
                //2 bytes padding
                _translation = FSHelpers.ReadVector3(data, (int)offset + 0x18);
                _unknown3 = FSHelpers.ReadFloat(data, (int)offset + 0x24);
                _boundingBoxMin = FSHelpers.ReadVector3(data, (int)offset + 0x28);
                _boundingBoxMax = FSHelpers.ReadVector3(data, (int)offset + 0x34);
            }

            public ushort GetUnknown1()
            {
                return _unknown1;
            }

            public byte GetUnknown2()
            {
                return _unknown2;
            }

            public Vector3 GetScale()
            {
                return _scale;
            }

            public HalfRotation GetRotation()
            {
                return _rotation;
            }

            public Vector3 GetTranslation()
            {
                return _translation;
            }

            public float GetUnknownFloat()
            {
                return _unknown3;
            }

            public Vector3 GetBoundingBoxMin()
            {
                return _boundingBoxMin;
            }

            public Vector3 GetBoundingBoxMax()
            {
                return _boundingBoxMax;
            }

            public const uint Size = 64;
        }

        public class ShapeChunk : BaseChunk
        {
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
                newBp.PacketSize = (uint)FSHelpers.Read32(_dataCopy, (int)(_packetLocationOffset + (index * BatchPacketLocation.Size) + 0x0));
                newBp.Offset = (uint)FSHelpers.Read32(_dataCopy, (int)(_packetLocationOffset + (index * BatchPacketLocation.Size) + 0x4));

                return newBp;
            }

            public BatchAttribute GetAttribute(uint attribIndex, uint attribOffset)
            {
                BatchAttribute newAttrib = new BatchAttribute();
                newAttrib.Load(_dataCopy, _attributeOffset + attribOffset + (attribIndex * BatchAttribute.Size));

                return newAttrib;
            }

            public ushort GetUnknown(uint index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)(_unknownTableOffset + (index * 0x2)));
            }

            public List<BatchAttribute> GetAttributesForBatch(ushort batchIndex)
            {
                Batch batch = GetBatch(batchIndex);
                List<BatchAttribute> attribs = new List<BatchAttribute>();

                BatchAttribute curAttrib;
                uint i = 0;
                do
                {
                    curAttrib = GetAttribute(i, batch.AttribOffset);
                    attribs.Add(curAttrib);
                    i++;

                } while (curAttrib.AttribType != ArrayTypes.NullAttr);

                return attribs;
            }

            public BatchPrimitive GetPrimitive(uint offset)
            {
                var primitive = new BatchPrimitive();
                primitive.Load(_dataCopy, _primitiveDataOffset + offset);

                return primitive;
            }

            public ushort GetPrimitiveIndex(uint offset, BatchAttribute primitiveAttrib)
            {
                switch (primitiveAttrib.DataType)
                {
                    case DataTypes.Signed16:
                        return (ushort) FSHelpers.Read16(_dataCopy, (int)(_primitiveDataOffset + offset));
                    case DataTypes.Signed8:
                        return FSHelpers.Read8(_dataCopy, (int) (_primitiveDataOffset + offset));
                    default:
                        throw new Exception("Unknown datatype index.");
                }
            }
        }

        /// <summary>
        /// Since primitives have different attributes (ie: some have position, some have normals, some have texcoords)
        /// each different primitive is placed into a "batch", and each batch ahs a fixed set of vertex attributes. Each 
        /// batch can then have several "packets", which are used for animations. A packet is a collection of primitives.
        /// </summary>
        public class Batch
        {
            public byte MatrixType;
            public byte Unknown0;
            public ushort PacketCount;
            public ushort AttribOffset;
            public ushort FirstMatrixIndex;
            public ushort PacketIndex;

            public float Unknown;
            public Vector3 BoundingBoxMin;
            public Vector3 BoundingBoxMax;

            public void Load(byte[] data, uint offset)
            {
                MatrixType = FSHelpers.Read8(data, (int)offset);
                Unknown0 = FSHelpers.Read8(data, (int)offset + 1);
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

            public void Load(byte[] data, uint offset)
            {
                AttribType = (ArrayTypes)FSHelpers.Read32(data, (int)offset);
                DataType = (DataTypes)FSHelpers.Read32(data, (int)offset + 0x4);
            }

            public const uint Size = 8;
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
                Type = (PrimitiveTypes)FSHelpers.Read8(data, (int)offset);
                VertexCount = (ushort)FSHelpers.Read16(data, (int)offset + 0x1);
            }

            public const uint Size = 3;
        }

        /// <summary>
        /// The Tex1 chunk stores a series of BTI images within the BMD/BDL format.
        /// The BMD/BDL also stores a string table which contains the file name for
        /// each image. The TEX1 chunk stores _textureCount headers immediately after
        /// the chunk, and then the data for those textures comes.
        /// </summary>
        public class TextureChunk : BaseChunk
        {
            private ushort _textureCount;
            private uint _textureHeaderOffset;
            private uint _stringTableOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);
                _textureCount = (ushort)FSHelpers.Read16(data, offset + 0x08);
                _textureHeaderOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _stringTableOffset = (uint)FSHelpers.Read32(data, offset + 0x10);

                offset += ChunkSize;
            }

            public BinaryTextureImage GetTexture(uint index)
            {
                if (index > _textureCount)
                    throw new Exception("Invalid index provided to GetTexture!");

                var texture = new BinaryTextureImage();

                //Before load the texture we need to modify the source byte array, because reasons.
                uint headerOffset = ((index) * 32);

                texture.Load(_dataCopy, _textureHeaderOffset + headerOffset, headerOffset + 32);

                return texture;
            }

            public ushort GetTextureCount()
            {
                return _textureCount;
            }
        }

        public class Material3Chunk : BaseChunk
        {
            private ushort _materialCount;
            private uint _materialInitDataOffset;
            private uint _materialIndexOffset;
            private uint _stringTableOffset;
            private uint _indirectTextureOffset; //Indirect textures is using the output of of one TEV stage as tex coords for another.
            private uint _gxCullModeOffset;
            private uint _gxColorOffset;
            private uint _colorChannelNumOffset; 
            private uint _colorChannelInfoOffset;
            private uint _gxColor2Offset;
            private uint _lightInfoOffset;
            private uint _texGenNumberOffset; 
            private uint _texCoordInfoOffset;
            private uint _texCoordInfo2Offset;
            private uint _texMatrixInfoOffset;
            private uint _texMatrix2InfoOffset;
            private uint _textureIndex;
            private uint _tevOrderInfoOffset;
            private uint _gxColorS10Offset; //(Signed 10-bit value)
            private uint _gxColor3Offset;
            private uint _tevStageNumInfoOffset; 
            private uint _tevStageInfoOffset;
            private uint _tevSwapModeInfoOffset;
            private uint _tevSwapModeTableInfoOffset;
            private uint _fogInfoOffset;
            private uint _alphaCompareInfoOffset;
            private uint _blendInfoOffset;
            private uint _zModeInfoOffset; //Depth mode
            private uint _zCompareInfoOffset; 
            private uint _ditherINfoOffset;
            private uint _nbtScaleInfoOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _materialCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                //Two bytes padding
                _materialInitDataOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _materialIndexOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _stringTableOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                _indirectTextureOffset = (uint)FSHelpers.Read32(data, offset + 0x18);
                _gxCullModeOffset = (uint)FSHelpers.Read32(data, offset + 0x1C);
                _gxColorOffset = (uint)FSHelpers.Read32(data, offset + 0x20);
                _colorChannelNumOffset = (uint)FSHelpers.Read32(data, offset + 0x24);
                _colorChannelInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x28);
                _gxColor2Offset = (uint)FSHelpers.Read32(data, offset + 0x2C);
                _lightInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x30);
                _texGenNumberOffset = (uint)FSHelpers.Read32(data, offset + 0x34);
                _texCoordInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x38);
                _texCoordInfo2Offset = (uint)FSHelpers.Read32(data, offset + 0x3C);
                _texMatrixInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x40);
                _texMatrix2InfoOffset = (uint)FSHelpers.Read32(data, offset + 0x44);
                _textureIndex = (uint)FSHelpers.Read32(data, offset + 0x48);
                _tevOrderInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x4C);
                _gxColorS10Offset = (uint)FSHelpers.Read32(data, offset + 0x50);
                _gxColor3Offset = (uint)FSHelpers.Read32(data, offset + 0x54);
                _tevStageNumInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x58);
                _tevStageInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x5C);
                _tevSwapModeInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x60);
                _tevSwapModeTableInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x64);
                _fogInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x68);
                _alphaCompareInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x6C);
                _blendInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x70);
                _zModeInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x74);
                _zCompareInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x78);
                _ditherINfoOffset = (uint)FSHelpers.Read32(data, offset + 0x7C);
                _nbtScaleInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x80);

                offset += ChunkSize;


                GetMaterialInitData(0);
            }

            public MaterialInitData GetMaterialInitData(uint index)
            {
                if (index > _materialCount)
                    throw new Exception("Invalid MaterialInit data requested.");

                MaterialInitData initData = new MaterialInitData();
                initData.Load(_dataCopy, _materialInitDataOffset + (index * MaterialInitData.Size));

                return initData;
            }

            public ushort GetMaterialIndex(uint index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)(_textureIndex + (index * 0x2)));
            }


            public const int Size = 132;

            public ushort GetMaterialRemapTable(ushort index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)(_materialIndexOffset + (index * 0x2)));
            }
        }

        //ToDo: These offsets are a little bit messed up.
        public class MaterialInitData
        {
            private byte _unknown1; //Read by PatchedMaterial, always 1?
            private byte _unknown2; //Mostly 0, sometimes 2.
            private ushort _padding1; //Always 0
            private ushort _indirectTexturingIndex;
            private ushort _cullModeIndex;
            private ushort[] _ambientColorIndex = new ushort[2]; //2 ushorts
            private ushort[] _colorChannelIndex = new ushort[4]; //4 ushorts
            private ushort[] _materialColorIndex = new ushort[2]; //2 ushorts
            private ushort[] _lightingIndex = new ushort[8]; //8 ushorts
            private ushort[] _texCoordIndex = new ushort[8]; //8 ushorts
            private ushort[] _texCoord2Index = new ushort[8]; //8 ushorts
            private ushort[] _texMatrixIndex = new ushort[8]; //8 ushorts
            private ushort[] _texMatrix2Index = new ushort[8]; //8 ushorts
            private ushort[] _textureIndex = new ushort[8]; //8 ushorts (diffuse textures)
            private ushort[] _tevConstantColorIndex = new ushort[4]; //4 ushorts
            private byte[] _constColorSel = new byte[16]; //16 (4 * RGBA?)
            private byte[] _constAlphaSel = new byte[16]; //16 (4 * RGBA?)
            private ushort[] _tevOrderIndex = new ushort[16]; //16 ushorts
            private ushort[] _tevColorIndex = new ushort[4]; //4 ushorts
            private ushort[] _tevStageInfoIndex = new ushort[16]; //16 ushorts
            private ushort[] _tevSwapModeInfoindex = new ushort[16]; //16 ushorts
            private ushort[] _tevSwapModeTableInfoindex = new ushort[4]; //4 ushorts
            private ushort[] _unconfirmedIndexes = new ushort[16]; //16 of them!

            public void Load(byte[] data, uint offset)
            {
                _unknown1 = FSHelpers.Read8(data, (int)offset + 0x0);
                _unknown2 = FSHelpers.Read8(data, (int)offset + 0x1);
                _padding1 = (ushort)FSHelpers.Read16(data, (int)offset + 0x2);
                _indirectTexturingIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x4);
                _cullModeIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x6);
                for (int i = 0; i < 2; i++)
                    _ambientColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x8 + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _colorChannelIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xC + (i * 0x2));
                for (int i = 0; i < 2; i++)
                    _materialColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x14 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _lightingIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x18 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texCoordIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x28 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texCoord2Index[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x38 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texMatrixIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x48 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texMatrix2Index[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x58 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _textureIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x84 + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _tevConstantColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x94 + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _constColorSel[i] = FSHelpers.Read8(data, (int)offset + 0x9C + (i * 0x1));
                for (int i = 0; i < 16; i++)
                    _constAlphaSel[i] = FSHelpers.Read8(data, (int)offset + 0xAC + (i * 0x1));
                for (int i = 0; i < 16; i++)
                    _tevOrderIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xBC + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _tevColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xDC + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _tevStageInfoIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xE4 + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _tevSwapModeInfoindex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x104 + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _tevSwapModeTableInfoindex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x124 + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _unconfirmedIndexes[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x12C + (i * 0x2));
            }

            public ushort GetTextureIndex(ushort index)
            {
                return _textureIndex[index];
            }

            public const int Size = 332;
        }


        public struct StringTableEntry
        {
            public ushort UnknownIndex;
            public ushort StringOffset;

            public override string ToString()
            {
                return string.Format("[{0}] - {1}", UnknownIndex, StringOffset);
            }
        }

        #endregion
    }
}
