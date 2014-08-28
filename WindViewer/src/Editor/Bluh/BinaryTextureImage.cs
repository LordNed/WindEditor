using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using WindViewer.Editor;

namespace WindViewer.FileFormats
{
    /// <summary>
    /// The BinaryTextureImage (or BTI) format is used by Wind Waker (and several other Nintendo
    /// games) to store texture images. There are a variety of encoding methods, some of which
    /// are supported right now for decoding. This does not currently support encoding BTI files
    /// but will at some point in time. It does not load mipmaps from the file currently.
    /// 
    /// Image data can be retrieved by calling GetData() which will return an ARGB array of bytes
    /// containing the information. For files without alpha data their values will be set to 0xFF.
    /// 
    /// BTI files are stored both individually on disk and embedded within other file formats. 
    /// Because of this, there are two ways to load data. Their usage is denoted in the appropriate
    /// loading function.
    /// </summary>
    public class BinaryTextureImage : BaseArchiveFile
    {
        //This data is mostly a duplicate from the FileHeader but should be accessible
        //without going through the header every time, as they're part of the BTI file
        //and not part of the header specifically.
        public TextureFormat Format { get; private set; }
        public ushort Width { get; private set; }
        public ushort Height { get; private set; }
        public WrapMode WrapS { get; private set; }
        public WrapMode WrapT { get; private set; }
        public FilterMode MinFilter { get; private set; }
        public FilterMode MagFilter { get; private set; }

        //These are internal copies of the data for now. Any settings required from them
        //should be stored above.
        private FileHeader _header;
        private Palette _imagePalette;
        private byte[] _argbImageData;

        //Temp for saving
        private byte[] _dataCache;

        #region File Formats
        /// <summary>
        /// ImageFormat specifies how the data within the image is encoded.
        /// Included is a chart of how many bits per pixel there are, 
        /// the width/height of each block, how many bytes long the
        /// actual block is, and a description of the type of data stored.
        /// </summary>
        public enum TextureFormat : byte
        {
            //Bits per Pixel | Block Width | Block Height | Block Size | Type / Description
            I4 = 0x00,      // 4 | 8 | 8 | 32 | grey
            I8 = 0x01,      // 8 | 8 | 8 | 32 | grey
            IA4 = 0x02,     // 8 | 8 | 4 | 32 | grey + alpha
            IA8 = 0x03,     //16 | 4 | 4 | 32 | grey + alpha
            RGB565 = 0x04,  //16 | 4 | 4 | 32 | color
            RGB5A3 = 0x05,  //16 | 4 | 4 | 32 | color + alpha
            RGBA32 = 0x06,  //32 | 4 | 4 | 64 | color + alpha
            C4 = 0x08,      //4 | 8 | 8 | 32 | palette choices (IA8, RGB565, RGB5A3)
            C8 = 0x09,      //8, 8, 4, 32 | palette choices (IA8, RGB565, RGB5A3)
            C14X2 = 0x0a,   //16 (14 used) | 4 | 4 | 32 | palette (IA8, RGB565, RGB5A3)
            CMPR = 0x0e,    //4 | 8 | 8 | 32 | mini palettes in each block, RGB565 or transparent.
        }

        /// <summary>
        /// Defines how textures handle going out of [0..1] range for texcoords.
        /// 
        /// Standard WrapModes derived from OpenGL docs, only ClampToEdge and Repeat
        /// seem to be used, but MirroredRepeat is included in the rare case that is
        /// actually used.
        /// </summary>
        public enum WrapMode : byte
        {
            ClampToEdge = 0,
            Repeat = 1,
            MirroredRepeat = 2,
        }

        /// <summary>
        /// PaletteFormat specifies how the data within the palette is stored. An
        /// image uses a single palette (except CMPR which defines its own
        /// mini-palettes within the Image data). Only C4, C8, and C14X2 use
        /// palettes. For all other formats the type and count is zero.
        /// </summary>
        public enum PaletteFormat : byte
        {
            IA8 = 0x00,
            RGB565 = 0x01,
            RGB5A3 = 0x02,
        }

        /// <summary>
        /// FilterMode specifies what type of filtering the file should use for
        /// min/mag. The current values are presumed from the OpenGL docs, hopefully
        /// Nintendo derived theirs from the OpenGL specification since other stuff
        /// seems to be.
        /// </summary>
        public enum FilterMode : byte
        {
            /* Valid in both Min and Mag Filter */
            Nearest = 0x1,
            Linear = 0x2,

            /* Valid in only Min Filter */
            NearestMipmapNearest = 0x3,
            NearestMipmapLinear = 0x4,
            LinearMipmapNearest = 0x5,
            LinearMipmapLinear = 0x6,
        }

        /// <summary>
        /// The Palette simply stores the color data as loaded from the file.
        /// It does not convert the files based on the Palette type to RGBA8.
        /// </summary>
        public class Palette
        {
            private byte[] _paletteData;

            public void Load(byte[] data, uint paletteEntryCount, uint offset)
            {
                //Files that don't have palettes have an entry count of zero.
                if (paletteEntryCount == 0)
                {
                    _paletteData = new byte[0];
                    return;
                }

                //All palette formats are 2 bytes per entry.
                _paletteData = new byte[paletteEntryCount * 2];
                Array.Copy(data, offset, _paletteData, 0, paletteEntryCount * 2);
            }

            public byte[] GetBytes()
            {
                return _paletteData;
            }
        }

        /// <summary>
        /// FileHeader mostly useful for reading and writing BTI files, as well as standing
        /// as a fairly complete/documented part of the file format. Padding is 
        /// included/documented in the loading code and noted as appropriate.
        /// </summary>
        public class FileHeader
        {
            //Not part of the structure, overall size of FileHeader.
            public const int Size = 32;

            public TextureFormat Format { get; private set; }
            private byte _alphaUnknownSetting; //0 for no alpha, 0x02 (and other values) 
            public ushort Width { get; private set; }
            public ushort Height { get; private set; }
            public WrapMode WrapS { get; private set; } //0 or 1 (Clamp or Repeat?)
            public WrapMode WrapT { get; private set; }
            public PaletteFormat PaletteFormat { get; private set; }
            private byte _unknown1;
            public ushort PaletteEntryCount { get; private set; } //Numer of entries in palette
            public uint PaletteDataOffset { get; private set; }//Relative to file header
            private uint _unknown2; //0 in MKWii (RGBA for border?)
            public FilterMode FilterSettingMin { get; private set; } //1 or 0? (Assumed filters are in min/mag order)
            public FilterMode FilterSettingMag { get; private set; } //
            private byte _minLod; //FixedPoint number, 1/8 = conversion
            private byte _maxLod; //FixedPoint number, 1/8 = conversion
            private byte _imageCount; //(= numMipmaps + 1)
            private byte _unknown3; //0 in MKWii //Padding
            private ushort _lodBias; //FixedPoint number, 1/100 = conversion
            public uint ImageDataOffset { get; private set; } //Relative to file header


            public void Load(byte[] data, uint offset)
            {
                Format = (TextureFormat)FSHelpers.Read8(data, (int)offset + 0x00);
                _alphaUnknownSetting = FSHelpers.Read8(data, (int)offset + 0x01);
                Width = (ushort)FSHelpers.Read16(data, (int)offset + 0x02);
                Height = (ushort)FSHelpers.Read16(data, (int)offset + 0x04);
                WrapS = (WrapMode)FSHelpers.Read8(data, (int)offset + 0x06);
                WrapT = (WrapMode)FSHelpers.Read8(data, (int)offset + 0x07);
                PaletteFormat = (PaletteFormat)FSHelpers.Read8(data, (int)offset + 0x8);
                _unknown1 = FSHelpers.Read8(data, (int)offset + 0x09);
                PaletteEntryCount = (ushort)FSHelpers.Read16(data, (int)offset + 0xA);
                PaletteDataOffset = (uint)FSHelpers.Read32(data, (int)offset + 0xC);
                _unknown2 = (uint)FSHelpers.Read32(data, (int)offset + 0x10);
                FilterSettingMin = (FilterMode)FSHelpers.Read8(data, (int)offset + 0x14);
                FilterSettingMag = (FilterMode)FSHelpers.Read8(data, (int)offset + 0x15);
                _minLod = FSHelpers.Read8(data, (int)offset + 0x16);
                _maxLod = FSHelpers.Read8(data, (int)offset + 0x17);
                _imageCount = FSHelpers.Read8(data, (int)offset + 0x18);
                _unknown3 = FSHelpers.Read8(data, (int)offset + 0x19);
                _lodBias = (ushort)FSHelpers.Read16(data, (int)offset + 0x1A);
                ImageDataOffset = (uint)FSHelpers.Read32(data, (int)offset + 0x1C);
            }
        }
        #endregion


        /// <summary>
        /// Loads a BTI file from a byte array. BTI files have two ways of being stored,
        /// both externally in their own file (simple), and embedded within another 
        /// file (complex). If you're loading from an isolated file, simply pass the
        /// entire file to data and zero for the other two parameters.
        /// 
        /// However, embedded files are more complicated. An embedded file stores the
        /// data in the following format: [bti header][bti header][bti header][bti data]
        /// [bti data][bti data]. The embedded bti files store their offsets relative not
        /// to the start of the first bti header, but to themselves.
        /// 
        /// To handle this, pass the offset into the byte[] that the header is at for 
        /// <paramref name="mainOffset"/> and then pass (btiHeaderIndex * FileHeader.Size)
        /// for <paramref name="dataOffset"/>. This load function will resolve the offsets
        /// for loading.
        /// </summary>
        /// <param name="data">Array holding entire BTI file.</param>
        /// <param name="mainOffset">Offset from start of array to BTI file.</param>
        /// <param name="dataOffset">Additional offset required by embedded BTI files.</param>
        public void Load(byte[] data, uint mainOffset, uint dataOffset)
        {
            _header = new FileHeader();
            _header.Load(data, mainOffset);

            //Copy our public settings out of the header and into the BinaryTextureImage instance.
            Format = _header.Format;
            Width = _header.Width;
            Height = _header.Height;
            WrapS = _header.WrapS;
            WrapT = _header.WrapT;
            MinFilter = _header.FilterSettingMin;
            MagFilter = _header.FilterSettingMag;

            //Grab the palette data
            _imagePalette = new Palette();
            _imagePalette.Load(data, _header.PaletteEntryCount, dataOffset + _header.PaletteDataOffset);

            //Now lets load a copy of the image data out of the file.
            _argbImageData = DecodeData(data, Width, Height, dataOffset + _header.ImageDataOffset, Format, _imagePalette, _header.PaletteFormat);
        }

        /// <summary>
        /// Call this if you want to initialize a BTI file from a filestream on disk.
        /// </summary>
        /// <param name="data">Entire contents of file.</param>
        public override void Load(byte[] data)
        {
            _dataCache = data;

            Load(data, 0, 0);
        }

        public override void Save(BinaryWriter stream)
        {
            FSHelpers.WriteArray(stream, _dataCache);
        }

        public void WriteImageToFile(string outputFile)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //Lock the bitmap for writing, copy the bits and then unlock for saving.
            IntPtr ptr = bmpData.Scan0;
            byte[] imageData = GetData();
            Marshal.Copy(imageData, 0, ptr, imageData.Length);
            bmp.UnlockBits(bmpData);

            bmp.Save(outputFile);
        }

        /// <summary>
        /// Returns a byte array of ARGB encoded data, regardless of original input
        /// data. For formats that do not support Alpha, Alpha channel is set to a
        /// value of 0xFF (zero transparency).
        /// </summary>
        /// <returns>Argb format array.</returns>
        public byte[] GetData()
        {
            return _argbImageData;
        }

        /// <summary>
        /// Nintendo doesn't provide the filesize anywhere in the file, so
        /// we're going to provide our own.
        /// 
        /// ToDo: This doesn't support Mipmaps!
        /// </summary>
        /// <returns></returns>
        public uint GetFileSize()
        {
            double bpp;
            switch (Format)
            {
                case TextureFormat.CMPR:
                case TextureFormat.C4:
                case TextureFormat.I4: bpp = 4; break;
                case TextureFormat.I8:
                case TextureFormat.C8:
                case TextureFormat.IA4: bpp = 8; break;
                case TextureFormat.C14X2:
                case TextureFormat.IA8:
                case TextureFormat.RGB565:
                case TextureFormat.RGB5A3: bpp = 16; break;
                case TextureFormat.RGBA32: bpp = 32; break;
                default:
                    Console.WriteLine("Unknown Header Format for GetFileSize. Assuming 16bpp!");
                    bpp = 16;
                    break;
            }

            return (uint)(FileHeader.Size + (Width * Height * (bpp / 8)) + (_header.PaletteEntryCount * 2));
        }

        #region Decoding
        private static byte[] DecodeData(byte[] data, uint width, uint height, uint dataOffset, TextureFormat format, Palette imagePalette, PaletteFormat paletteFormat)
        {
            switch (format)
            {
                case TextureFormat.RGBA32:
                    return DecodeRgba32(data, dataOffset, width, height);

                case TextureFormat.C4:
                    return DecodeC4(data, dataOffset, width, height, imagePalette, paletteFormat);

                case TextureFormat.RGB565:
                    return DecodeRgb565(data, dataOffset, width, height);

                case TextureFormat.CMPR:
                    return DecodeCmpr(data, dataOffset, width, height);

                case TextureFormat.IA8:
                    return DecodeIA8(data, dataOffset, width, height);

                case TextureFormat.I4:
                    return DecodeI4(data, dataOffset, width, height);

                case TextureFormat.RGB5A3:
                    return DecodeRgb5A3(data, dataOffset, width, height);

                default:
                    Console.WriteLine("Unknown BTI Format {0}, unable to decode!", format);
                    return new byte[0];
            }
        }

        private static byte[] DecodeRgba32(byte[] fileData, uint dataOffset, uint width, uint height)
        {
            uint numBlocksW = width / 4; //4 byte block width
            uint numBlocksH = height / 4; //4 byte block height 

            byte[] decodedData = new byte[width * height * 4];

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
                            if ((xBlock * 4 + pX >= width) || (yBlock * 4 + pY >= height))
                                continue;

                            //Now we're looping through each pixel in a block, but a pixel is four bytes long. 
                            uint destIndex = (uint)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 4) + pX));
                            decodedData[destIndex + 3] = fileData[dataOffset + 0]; //Alpha
                            decodedData[destIndex + 2] = fileData[dataOffset + 1]; //Red
                            dataOffset += 2;
                        }
                    }

                    //...but we have to do it twice, because RGBA32 stores two sub-blocks per block. (AR, and GB)
                    for (int pY = 0; pY < 4; pY++)
                    {
                        for (int pX = 0; pX < 4; pX++)
                        {
                            //Ensure the pixel we're checking is within bounds of the image.
                            if ((xBlock * 4 + pX >= width) || (yBlock * 4 + pY >= height))
                                continue;

                            //Now we're looping through each pixel in a block, but a pixel is four bytes long. 
                            uint destIndex = (uint)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 4) + pX));
                            decodedData[destIndex + 1] = fileData[dataOffset + 0]; //Green
                            decodedData[destIndex + 0] = fileData[dataOffset + 1]; //Blue
                            dataOffset += 2;
                        }
                    }

                }
            }

            return decodedData;
        }

        private static byte[] DecodeC4(byte[] fileData, uint dataOffset, uint width, uint height, Palette imagePalette, PaletteFormat paletteFormat)
        {
            //4 bpp, 8 block width/height, block size 32 bytes, possible palettes (IA8, RGB565, RGB5A3)
            uint numBlocksW = width / 8;
            uint numBlocksH = height / 8;

            byte[] decodedData = new byte[width * height * 8];

            //Read the indexes from the file
            for (int yBlock = 0; yBlock < numBlocksH; yBlock++)
            {
                for (int xBlock = 0; xBlock < numBlocksW; xBlock++)
                {
                    //Inner Loop for pixels
                    for (int pY = 0; pY < 8; pY++)
                    {
                        for (int pX = 0; pX < 8; pX += 2)
                        {
                            //Ensure we're not reading past the end of the image.
                            if ((xBlock * 8 + pX >= width) || (yBlock * 8 + pY >= height))
                                continue;

                            byte t = (byte)(fileData[dataOffset] & 0xF0);
                            byte t2 = (byte)(fileData[dataOffset] & 0x0F);

                            decodedData[width * ((yBlock * 8) + pY) + (xBlock * 8) + pX + 0] = (byte)(t >> 4);
                            decodedData[width * ((yBlock * 8) + pY) + (xBlock * 8) + pX + 1] = t2;

                            dataOffset += 1;
                        }
                    }
                }
            }

            //Now look them up in the palette and turn them into actual colors.
            byte[] finalDest = new byte[decodedData.Length / 2];

            int pixelSize = paletteFormat == PaletteFormat.IA8 ? 2 : 4;
            int destOffset = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    UnpackPixelFromPalette(decodedData[y * width + x], ref finalDest, destOffset, imagePalette.GetBytes(), paletteFormat);
                    destOffset += pixelSize;
                }
            }

            return finalDest;
        }

        private static byte[] DecodeRgb565(byte[] fileData, uint dataOffset, uint width, uint height)
        {
            //16 bpp, 4 block width/height, block size 32 bytes, color.
            uint numBlocksW = width / 4;
            uint numBlocksH = height / 4;

            byte[] decodedData = new byte[width * height * 4];

            //Read the indexes from the file
            for (int yBlock = 0; yBlock < numBlocksH; yBlock++)
            {
                for (int xBlock = 0; xBlock < numBlocksW; xBlock++)
                {
                    //Inner Loop for pixels
                    for (int pY = 0; pY < 4; pY++)
                    {
                        for (int pX = 0; pX < 4; pX++)
                        {
                            //Ensure we're not reading past the end of the image.
                            if ((xBlock * 4 + pX >= width) || (yBlock * 4 + pY >= height))
                                continue;

                            ushort sourcePixel = (ushort)FSHelpers.Read16(fileData, (int)dataOffset);
                            RGB565ToRGBA8(sourcePixel, ref decodedData,
                                (int)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 4) + pX)));

                            dataOffset += 2;
                        }
                    }
                }
            }

            return decodedData;
        }

        private static byte[] DecodeCmpr(byte[] fileData, uint dataOffset, uint width, uint height)
        {
            //Decode S3TC1
            byte[] buffer = new byte[width * height * 4];

            for (int y = 0; y < height / 4; y += 2)
            {
                for (int x = 0; x < width / 4; x += 2)
                {
                    for (int dy = 0; dy < 2; ++dy)
                    {
                        for (int dx = 0; dx < 2; ++dx, dataOffset += 8)
                        {
                            if (4 * (x + dx) < width && 4 * (y + dy) < height)
                                Buffer.BlockCopy(fileData, (int)dataOffset, buffer, (int)(8 * ((y + dy) * width / 4 + x + dx)), 8);
                        }
                    }
                }
            }

            for (int i = 0; i < width * height / 2; i += 8)
            {
                FSHelpers.Swap(ref buffer[i], ref buffer[i + 1]);
                FSHelpers.Swap(ref buffer[i + 2], ref buffer[i + 3]);

                buffer[i + 4] = S3TC1ReverseByte(buffer[i + 4]);
                buffer[i + 5] = S3TC1ReverseByte(buffer[i + 5]);
                buffer[i + 6] = S3TC1ReverseByte(buffer[i + 6]);
                buffer[i + 7] = S3TC1ReverseByte(buffer[i + 7]);
            }

            //Now decompress the DXT1 data within it.
            return DecompressDxt1(buffer, width, height);
        }

        private static byte S3TC1ReverseByte(byte b)
        {
            byte b1 = (byte)(b & 0x3);
            byte b2 = (byte)(b & 0xC);
            byte b3 = (byte)(b & 0x30);
            byte b4 = (byte)(b & 0xC0);

            return (byte)((b1 << 6) | (b2 << 2) | (b3 >> 2) | (b4 >> 6));
        }

        private static byte[] DecompressDxt1(byte[] src, uint width, uint height)
        {
            uint dataOffset = 0;
            byte[] finalData = new byte[width * height * 4];

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    UInt16 color1 = FSHelpers.Read16Swap(src, dataOffset);
                    UInt16 color2 = FSHelpers.Read16Swap(src, dataOffset + 2);
                    UInt32 bits = FSHelpers.Read32Swap(src, dataOffset + 4);
                    dataOffset += 8;

                    byte[][] ColorTable = new byte[4][];
                    for (int i = 0; i < 4; i++)
                        ColorTable[i] = new byte[4];

                    RGB565ToRGBA8(color1, ref ColorTable[0], 0);
                    RGB565ToRGBA8(color2, ref ColorTable[1], 0);

                    if (color1 > color2)
                    {
                        ColorTable[2][0] = (byte)((2 * ColorTable[0][0] + ColorTable[1][0] + 1) / 3);
                        ColorTable[2][1] = (byte)((2 * ColorTable[0][1] + ColorTable[1][1] + 1) / 3);
                        ColorTable[2][2] = (byte)((2 * ColorTable[0][2] + ColorTable[1][2] + 1) / 3);
                        ColorTable[2][3] = 0xFF;

                        ColorTable[3][0] = (byte)((ColorTable[0][0] + 2 * ColorTable[1][0] + 1) / 3);
                        ColorTable[3][1] = (byte)((ColorTable[0][1] + 2 * ColorTable[1][1] + 1) / 3);
                        ColorTable[3][2] = (byte)((ColorTable[0][2] + 2 * ColorTable[1][2] + 1) / 3);
                        ColorTable[3][3] = 0xFF;
                    }
                    else
                    {
                        ColorTable[2][0] = (byte)((ColorTable[0][0] + ColorTable[1][0] + 1) / 2);
                        ColorTable[2][1] = (byte)((ColorTable[0][1] + ColorTable[1][1] + 1) / 2);
                        ColorTable[2][2] = (byte)((ColorTable[0][2] + ColorTable[1][2] + 1) / 2);
                        ColorTable[2][3] = 0xFF;

                        ColorTable[3][0] = (byte)((ColorTable[0][0] + 2 * ColorTable[1][0] + 1) / 3);
                        ColorTable[3][1] = (byte)((ColorTable[0][1] + 2 * ColorTable[1][1] + 1) / 3);
                        ColorTable[3][2] = (byte)((ColorTable[0][2] + 2 * ColorTable[1][2] + 1) / 3);
                        ColorTable[3][3] = 0x00;
                    }

                    for (int iy = 0; iy < 4; ++iy)
                    {
                        for (int ix = 0; ix < 4; ++ix)
                        {
                            if (((x + ix) < width) && ((y + iy) < height))
                            {
                                int di = (int)(4 * ((y + iy) * width + x + ix));
                                int si = (int)(bits & 0x3);
                                finalData[di + 0] = ColorTable[si][0];
                                finalData[di + 1] = ColorTable[si][1];
                                finalData[di + 2] = ColorTable[si][2];
                                finalData[di + 3] = ColorTable[si][3];
                            }
                            bits >>= 2;
                        }
                    }
                }
            }

            return finalData;
        }

        private static byte[] DecodeIA8(byte[] fileData, uint dataOffset, uint width, uint height)
        {
            uint numBlocksW = width / 4; //4 byte block width
            uint numBlocksH = height / 4; //4 byte block height 

            byte[] decodedData = new byte[width * height * 4];

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
                            if ((xBlock * 4 + pX >= width) || (yBlock * 4 + pY >= height))
                                continue;

                            //Now we're looping through each pixel in a block, but a pixel is four bytes long. 
                            uint destIndex = (uint)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 4) + pX));
                            decodedData[destIndex + 3] = fileData[dataOffset + 0];
                            decodedData[destIndex + 2] = fileData[dataOffset + 1];
                            decodedData[destIndex + 1] = fileData[dataOffset + 1];
                            decodedData[destIndex + 0] = fileData[dataOffset + 1];
                            dataOffset += 2;
                        }
                    }
                }
            }

            return decodedData;
        }

        private static byte[] DecodeI4(byte[] fileData, uint dataOffset, uint width, uint height)
        {
            uint numBlocksW = width / 8; //8 byte block width
            uint numBlocksH = height / 8; //8 byte block height 

            byte[] decodedData = new byte[width * height * 4];

            for (int yBlock = 0; yBlock < numBlocksH; yBlock++)
            {
                for (int xBlock = 0; xBlock < numBlocksW; xBlock++)
                {
                    //For each block, we're going to examine block width / block height number of 'pixels'
                    for (int pY = 0; pY < 8; pY++)
                    {
                        for (int pX = 0; pX < 8; pX += 2)
                        {
                            //Ensure the pixel we're checking is within bounds of the image.
                            if ((xBlock * 8 + pX >= width) || (yBlock * 8 + pY >= height))
                                continue;

                            byte t = (byte)(fileData[dataOffset] & 0xF0);
                            byte t2 = (byte)(fileData[dataOffset] & 0x0F);
                            uint destIndex = (uint)(4 * (width * ((yBlock * 8) + pY) + (xBlock * 8) + pX));

                            decodedData[destIndex + 3] = (byte)(t * 0x11);
                            decodedData[destIndex + 2] = (byte)(t * 0x11);
                            decodedData[destIndex + 1] = (byte)(t * 0x11);
                            decodedData[destIndex + 0] = 0xFF;

                            decodedData[destIndex + 7] = (byte)(t2 * 0x11);
                            decodedData[destIndex + 6] = (byte)(t2 * 0x11);
                            decodedData[destIndex + 5] = (byte)(t2 * 0x11);
                            decodedData[destIndex + 4] = 0xFF;

                            dataOffset++;
                        }
                    }
                }
            }

            return decodedData;
        }

        private static byte[] DecodeRgb5A3(byte[] fileData, uint dataOffset, uint width, uint height)
        {
            uint numBlocksW = width / 4; //4 byte block width
            uint numBlocksH = height / 4; //4 byte block height 

            byte[] decodedData = new byte[width * height * 4];

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
                            if ((xBlock * 4 + pX >= width) || (yBlock * 4 + pY >= height))
                                continue;

                            ushort sourcePixel = (ushort)FSHelpers.Read16(fileData, (int)dataOffset);
                            RGB5A3ToRGBA8(sourcePixel, ref decodedData,
                                (int)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 4) + pX)));

                            dataOffset += 2;
                        }
                    }
                }
            }

            return decodedData;
        }

        private static void UnpackPixelFromPalette(int paletteIndex, ref byte[] dest, int offset, byte[] paletteData, PaletteFormat format)
        {
            switch (format)
            {
                case PaletteFormat.IA8:
                    dest[0] = paletteData[2 * paletteIndex + 1];
                    dest[1] = paletteData[2 * paletteIndex + 0];
                    break;
                case PaletteFormat.RGB565:
                    RGB565ToRGBA8((ushort)FSHelpers.Read16(paletteData, 2 * paletteIndex), ref dest, offset);
                    break;
                case PaletteFormat.RGB5A3:
                    RGB5A3ToRGBA8((ushort)FSHelpers.Read16(paletteData, 2 * paletteIndex), ref dest, offset);
                    break;
            }
        }

        /// <summary>
        /// Convert a RGB565 encoded pixel (two bytes in length) to a RGBA (4 byte in length)
        /// pixel.
        /// </summary>
        /// <param name="sourcePixel">RGB565 encoded pixel.</param>
        /// <param name="dest">Destination array for RGBA pixel.</param>
        /// <param name="destOffset">Offset into destination array to write RGBA pixel.</param>
        private static void RGB565ToRGBA8(ushort sourcePixel, ref byte[] dest, int destOffset)
        {
            byte r, g, b;
            r = (byte)((sourcePixel & 0xF100) >> 11);
            g = (byte)((sourcePixel & 0x7E0) >> 5);
            b = (byte)((sourcePixel & 0x1F));

            r = (byte)((r << (8 - 5)) | (r >> (10 - 8)));
            g = (byte)((g << (8 - 6)) | (g >> (12 - 8)));
            b = (byte)((b << (8 - 5)) | (b >> (10 - 8)));

            dest[destOffset] = b;
            dest[destOffset + 1] = g;
            dest[destOffset + 2] = r;
            dest[destOffset + 3] = 0xFF; //Set alpha to 1
        }

        /// <summary>
        /// Convert a RGB5A3 encoded pixel (two bytes in length) to an RGBA (4 byte in length)
        /// pixel.
        /// </summary>
        /// <param name="sourcePixel">RGB5A3 encoded pixel.</param>
        /// <param name="dest">Destination array for RGBA pixel.</param>
        /// <param name="destOffset">Offset into destination array to write RGBA pixel.</param>
        private static void RGB5A3ToRGBA8(ushort sourcePixel, ref byte[] dest, int destOffset)
        {
            byte r, g, b, a;

            //No alpha bits
            if ((sourcePixel & 0x8000) == 0x8000)
            {
                a = 0xFF;
                r = (byte)((sourcePixel & 0x7C00) >> 10);
                g = (byte)((sourcePixel & 0x3E0) >> 5);
                b = (byte)(sourcePixel & 0x1F);

                r = (byte)((r << (8 - 5)) | (r >> (10 - 8)));
                g = (byte)((g << (8 - 5)) | (g >> (10 - 8)));
                b = (byte)((b << (8 - 5)) | (b >> (10 - 8)));
            }
            //Alpha bits
            else
            {
                a = (byte)((sourcePixel & 0x7000) >> 12);
                r = (byte)((sourcePixel & 0xF00) >> 8);
                g = (byte)((sourcePixel & 0xF0) >> 4);
                b = (byte)(sourcePixel & 0xF);

                a = (byte)((a << (8 - 3)) | (a << (8 - 6)) | (a >> (9 - 8)));
                r = (byte)((r << (8 - 4)) | r);
                g = (byte)((g << (8 - 4)) | g);
                b = (byte)((b << (8 - 4)) | b);
            }

            dest[destOffset + 0] = a;
            dest[destOffset + 1] = b;
            dest[destOffset + 2] = g;
            dest[destOffset + 3] = r;
        }
        #endregion
    }
}