using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
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

                    uint dataOffset = header.ImageDataOffset;
                    byte[] destData = new byte[header.GetWidth()*header.GetHeight()*4];

                    for (int yBlock = 0; yBlock < numBlocksH; yBlock++)
                    {
                        for (int xBlock = 0; xBlock < numBlocksW; xBlock++)
                        {
                            //For each block, we're going to examine block width / block height number of 'pixels'
                            for (int pY = 0; pY < 4; pY++)
                            {
                                for (int pX = 0; pX < 4; pX++)
                                {
                                    //Ensure the pixel we're checking is within bounds of the image.
                                    if ((xBlock*4 + pX >= header.GetWidth()) || (yBlock*4 + pY >= header.GetHeight()))
                                    {
                                        continue;
                                    }

                                    //Now we're looping through each pixel in a block, but a pixel is four bytes long. 
                                    uint destIndex = (uint)(4*(header.GetWidth()*((yBlock *4)+ pY) + (xBlock *4)+ pX));
                                    destData[destIndex + 3] = data[dataOffset + 0]; //Alpha
                                    destData[destIndex + 2] = data[dataOffset + 1]; //Red
                                    dataOffset += 2;
                                }
                            }

                            //...but we have to do it twice, because RGBA32 stores two sub-blocks per block. (AR, and GB)
                            for (int pY = 0; pY < 4; pY++)
                            {
                                for (int pX = 0; pX < 4; pX++)
                                {
                                    //Ensure the pixel we're checking is within bounds of the image.
                                    if ((xBlock * 4 + pX >= header.GetWidth()) || (yBlock * 4 + pY >= header.GetHeight()))
                                    {
                                        continue;
                                    }

                                    //Now we're looping through each pixel in a block, but a pixel is four bytes long. 
                                    uint destIndex = (uint)(4 * (header.GetWidth() * ((yBlock * 4) + pY) + (xBlock * 4) + pX));
                                    destData[destIndex + 1] = data[dataOffset + 0]; //Green
                                    destData[destIndex + 0] = data[dataOffset + 1]; //Blue
                                    dataOffset += 2;
                                }
                            }
                            
                        }
                    }

                    //Test? 
                    Bitmap bmp = new Bitmap((int)header.GetWidth(), (int)header.GetHeight());
                    Rectangle rect = new Rectangle(0, 0, (int)header.GetWidth(), (int)header.GetHeight());
                    BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

                    //Unsafe mass copy, yay
                    IntPtr ptr = bmpData.Scan0;
                    Marshal.Copy(destData, 0, ptr, destData.Length);
                    bmp.UnlockBits(bmpData);




                    bmp.Save("poked_with_stick.png");
                 
                    break;

                default:
                    Console.WriteLine("Unsupported image format {0}!", header.GetFormat());
                    break;
            }

            Console.WriteLine("After");
        }

        public override void Save(BinaryWriter stream)
        {
            FSHelpers.WriteArray(stream, _dataCache);
        }
    }
}