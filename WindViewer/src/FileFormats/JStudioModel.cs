using System;
using System.Collections.Generic;
using System.IO;
using WindViewer.Editor;

namespace WindViewer.FileFormats
{
    public class JStudioModel : BaseArchiveFile
    {
        //Temp in case I fuck up
        private byte[] _origDataCache;


        public override void Load(byte[] data)
        {
            _origDataCache = data;

            int dataOffset = 0;

            Header header = new Header();
            header.Load(data, ref dataOffset);

            //Load all of the chunks
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
                    case "SHP1":
                    case "TEX1":
                    case "MAT3":
                    case "ANK1":
                        break;
                    default:
                        Console.WriteLine("Found unknown chunk {0}!", tagName);
                        baseChunk = new BaseChunk();
                        break;
                }

                baseChunk.Load(data, ref dataOffset);
            }

        }

        public override void Save(BinaryWriter stream)
        {
            stream.Write(_origDataCache);
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

            public virtual void Load(byte[] data, ref int offset)
            {
                Type = FSHelpers.ReadString(data, offset, 4);
                ChunkSize = FSHelpers.Read32(data, offset + 4);
            }

            public virtual void Save(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Type, 4);
                FSHelpers.Write32(stream, ChunkSize);
            }
        }

        private class Inf1Chunk : BaseChunk
        {
            public short Unknown1;
            public int Unknown2;
            public int VertexCount;
            public int DataOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                Unknown1 = FSHelpers.Read16(data, offset+8);
                Unknown2 = FSHelpers.Read32(data, offset+12); //2 bytes padding after Unknown1
                VertexCount = FSHelpers.Read32(data, offset+16);
                DataOffset = FSHelpers.Read32(data, offset+20);

                var hierarchyDataList = new List<HierarchyData>();
                int dataOffsetCpy = DataOffset;

                //Nintendo doesn't seem to define how many of these there are anywhere,
                //so we're going to have to sort of guess at the maximum (since the struct
                //is padded to 32 byte alignment) and then break early. Bleh.
                int maxHierarchyData = (ChunkSize - 24)/4; //24 bytes for the header, 4 bytes per HierarchyData
                for (int i = 0; i < maxHierarchyData; i++)
                {
                    HierarchyData hData = new HierarchyData();
                    hData.Load(data, ref dataOffsetCpy);

                    hierarchyDataList.Add(hData);

                    //Anything after the Finish command is just "This is padding data to " padding.
                    if (hData.Type == HierarchyData.HierarchyDataTypes.Finish)
                        break;
                }

                offset += ChunkSize;
            }

            //"SceneGraphRaw"
            private class HierarchyData
            {
                public enum HierarchyDataTypes : ushort
                {
                    Finish = 0x0, NewNode = 0x01, EndNode = 0x02,
                    Joint = 0x10, Material = 0x11, Shape = 0x12,
                }

                public HierarchyDataTypes Type;
                public ushort Index;

                public void Load(byte[] data, ref int offset)
                {
                    Type = (HierarchyDataTypes)FSHelpers.Read16(data, offset);
                    Index = (ushort) FSHelpers.Read16(data, offset + 2);

                    offset += 4;
                }
            }
        }

        private class Vtx1Chunk : BaseChunk
        {
            public int DataOffset;
            public int[] VertexDataOffsets; //13 of 'em!

            //Not part of the header
            public List<VertexFormat> VertexFormats = new List<VertexFormat>();

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                DataOffset = FSHelpers.Read32(data, offset + 0x8);

                int dataOffsetCpy = DataOffset;
                for (int i = 0; i < ChunkSize; i++)
                {
                    VertexFormat vFormat = new VertexFormat();
                    vFormat.Load(data, ref dataOffsetCpy);
                    VertexFormats.Add(vFormat);

                    if (vFormat.ArrayType == VertexFormat.ArrayTypes.NullAttr)
                        break;
                }

                //Jump back to the vertex data offsets in the header.
                dataOffsetCpy = 0xC;
                
                VertexDataOffsets = new int[13];
                int curVertexFormat = 0;

                for (int i = 0; i < VertexDataOffsets.Length; i++)
                {
                    VertexDataOffsets[i] = FSHelpers.Read32(data, dataOffsetCpy);
                    dataOffsetCpy += 0x4;

                    if(VertexDataOffsets[i] == 0)
                        continue;

                    VertexFormats[curVertexFormat].VTX1OffsetToVertexData = VertexDataOffsets[i];
                    curVertexFormat++;
                }

                offset += ChunkSize;
            }


            public class VertexFormat
            {
                public enum ArrayTypes
                {
                    PositionMatrixIndex, Tex0MatrixIndex, Tex1MatrixIndex, Tex2MatrixIndex, Tex3MatrixIndex,
                    Tex4MatrixIndex, Tex5MatrixIndex, Tex6MatrixIndex, Tex7MatrixIndex,
                    Position, Normal, Color0, Color1, Tex0, Tex1, Tex2, Tex3, Tex4, Tex5, Tex6,Tex7,
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
                    RGB888  = 0x1,
                    RGBX8 = 0x2,
                    RGBA4 = 0x3,
                    RGBA6 = 0x4,
                    RGBA8 = 0x5,
                }

                public ArrayTypes ArrayType;
                public uint ArrayCount;
                public DataTypes DataType;
                public byte DecimalPoint;

                //Not part of data
                public int VTX1OffsetToVertexData;

                public void Load(byte[] data, ref int offset)
                {
                    ArrayType = (ArrayTypes)FSHelpers.Read32(data, offset);
                    ArrayCount = (uint)FSHelpers.Read32(data, offset+4);
                    DataType = (DataTypes)FSHelpers.Read32(data, offset+8);
                    DecimalPoint = FSHelpers.Read8(data, offset+12);

                    offset += 16; //3 bytes padding after DecimalPoint
                }
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

                SectionCount = (ushort) FSHelpers.Read16(data, offset + 0x8);
                CountsArrayOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                IndicesOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                WeightsOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                MatrixDataOffset = (uint) FSHelpers.Read32(data, offset + 0x18);


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

                SectionCount = (ushort) FSHelpers.Read16(data, 0x8);
                IsWeightedOffset = (uint) FSHelpers.Read32(data, 0xC);
                DataOffset = (uint) FSHelpers.Read32(data, 0x10);

                IsWeighted = new bool[SectionCount];
                Data = new ushort[SectionCount];

                for (int i = 0; i < SectionCount; i++)
                {
                    IsWeighted[i] = Convert.ToBoolean(FSHelpers.Read8(data, (int)(offset + IsWeightedOffset + i)));
                    Data[i] = (ushort) FSHelpers.Read16(data, (int) (offset + DataOffset + (i * 2)));
                }

                offset += ChunkSize;
            }
        }
        #endregion
    }
}
