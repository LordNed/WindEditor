using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms.VisualStyles;
using WindViewer.Editor;

namespace WindViewer.FileFormats
{
    public class BTI : BaseArchiveFile
    {



        public enum ImageFormat : byte
        {  
                            //Bits per Pixel | Block Width | Block Height | Block Size | Type / Description
            I4 = 0x00,      // 4 | 8 | 8 | 32 | grey
            I8 = 0x01,      // 8 | 8 | 8 | 32 | grey
            IA4 = 0x02,     // 8 | 8 | 4 | 32 | grey + alpha
            IA8 = 0x03,     //16 | 4 | 4 | 32 | grey + alpha
            RGB565 = 0x04,  //16 | 4 | 4 | 32 | color
            RGB5A3 = 0x05,  //16 | 4 | 4 | 32 | color + alpha
            RGBA32 = 0x06,  //32 | 4 | 4 | 64 | color + alpha
            C4 = 0x08,      //4 | 8 | 8 | 32 | palette (IA8, RGB565, RGB5A3)
            C8 = 0x09,      //8, 8, 4, 32 | palette (IA8, RGB565, RGB5A3)
            C14X2 = 0x0a,   //16 (14 used) | 4 | 4 | 32 | palette (IA8, RGB565, RGB5A3)
            CMPR = 0x0e,    //4 | 8 | 8 | 32 | mini palettes in each block, RGB565 or transparent.
        }

        public enum WrapMode : byte
        {
            ClampToEdge = 0,
            Repeat = 1,
            MirroredRepeat = 2,
        }

        public class FileHeader
        {
            private ImageFormat _format;
            private byte _alphaEnabled; //0 for no alpha, 0x02 (and anything else) for alpha enabled
            private ushort _width;
            private ushort _height;
            private WrapMode _wrapS; //0 or 1 (Clamp or Repeat?)
            private WrapMode _wrapT;
            private ushort _paletteFormat;
            private ushort _paletteCount;
            private uint _paletteDataOffset; //Relative to file header
            private uint _unknown2; //0 in MKWii (RGBA for border?)
            private byte _filterSettingMin; //1 or 0? (Assumed filters are in min/mag order)
            private byte _filterSettingMag; //
            private ushort _padding1; //0 in MKWii. //Padding
            private byte _imageCount; //(= numMipmaps + 1)
            private byte _padding2; //0 in MKWii //Padding
            private ushort _padding3; //0 in MKWii
            public uint ImageDataOffset { get; private set; } //Relative to file header

            public void Load(byte[] data, uint offset)
            {
                _format = (ImageFormat) FSHelpers.Read8(data, (int)offset + 0x00);
                _alphaEnabled = FSHelpers.Read8(data, (int) offset + 0x01);
                _width = (ushort)FSHelpers.Read16(data, (int) offset + 0x02);
                _height =(ushort)FSHelpers.Read16(data, (int) offset + 0x04);
                _wrapS = (WrapMode) FSHelpers.Read8(data, (int) offset + 0x06);
                _wrapT = (WrapMode) FSHelpers.Read8(data, (int) offset + 0x07);
                _paletteFormat = (ushort) FSHelpers.Read16(data, (int) offset + 0x8);
                _paletteCount = (ushort) FSHelpers.Read16(data, (int) offset + 0xA);
                _paletteDataOffset = (uint) FSHelpers.Read32(data, (int) offset + 0xC);
                _unknown2 = (uint) FSHelpers.Read32(data, (int) offset + 0x10);
                _filterSettingMin = FSHelpers.Read8(data, (int) offset + 0x14);
                _filterSettingMag = FSHelpers.Read8(data, (int) offset + 0x15);
                _padding1 = (ushort) FSHelpers.Read16(data, (int) offset + 0x16);
                _imageCount = FSHelpers.Read8(data, (int) offset + 0x18);
                _padding2 = FSHelpers.Read8(data, (int) offset + 0x19);
                _padding3 = (ushort)FSHelpers.Read16(data, (int) offset + 0x1A);
                ImageDataOffset = (uint) FSHelpers.Read32(data, (int) offset + 0x1C);
            }

            public ImageFormat GetFormat()
            {
                return _format;
            }

            public uint GetWidth()
            {
                return _width;
            }

            public uint GetHeight()
            {
                return _height;
            }
        }

        //Temp for saving
        private byte[] _dataCache;

        //temp
        private class Block
        {
            public byte[] Data;
        }


        public override void Load(byte[] data)
        {
            _dataCache = data;

            FileHeader header = new FileHeader();
            header.Load(data, 0);

            switch (header.GetFormat())
            {
                case ImageFormat.RGBA32:
                    uint numBlocksW = header.GetWidth()/4; //4 byte block width
                    uint numBlocksH = header.GetHeight()/4; //4 byte block height 

                    Block[] blockList = new Block[numBlocksW*numBlocksH];
                    for (uint i = 0; i < numBlocksW*numBlocksH; i++)
                    {
                        Block curBlock = new Block();
                        curBlock.Data = FSHelpers.ReadN(data, (int)header.ImageDataOffset, 4 * 4 * 4); //Width * Height * Bpp

                        blockList[i] = curBlock;
                    }

                    //Test? 
                    Bitmap bmp = new Bitmap((int)header.GetWidth(), (int)header.GetHeight());
                    for (int i = 0; i < 4; i++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            byte[] argb = new byte[4];
                            for(int p = 0; p < 4; p++)
                                argb[p] = blockList[0].Data[(i*4) + k + p];

                            Color pixelColor = Color.FromArgb(argb[0], argb[1], argb[2], argb[3]);
                            bmp.SetPixel(i, k, pixelColor);
                        }
                    }

                    bmp.Save("poked_with_stick.bmp");

                    break;

                default:
                    Console.WriteLine("Unsupported image format {0}!", header.GetFormat());
                    break;
            }
        }

        public override void Save(BinaryWriter stream)
        {
            FSHelpers.WriteArray(stream, _dataCache);
        }
    }
}