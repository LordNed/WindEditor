using System.Windows.Forms.VisualStyles;
using WindViewer.Editor;

namespace WindViewer.FileFormats
{
    public class BTI
    {
        public enum ImageFormat : byte
        {  
                    //Bits per Pixel | Block Width | Block Height | Block Size | Type / Description
            I4 = 0x00,  // 4 | 8 | 8 | 32 | grey
            I8 = 0x01,  // 8 | 8 | 8 | 32 | grey
            IA4 = 0x02, // 8 | 8 | 4 | 32 | grey + alpha
            IA8 = 0x03, //16 | 4 | 4 | 32 | grey + alpha
            RGB565 = 0x04, //16 | 4 | 4 | 32 | color
            RGB5A3 = 0x05, //16 | 4 | 4 | 32 | color + alpha
            RGBA32 = 0x06, //32 | 4 | 4 | 64 | color + alpha
            C4 = 0x08, //4 | 8 | 8 | 32 | palette (IA8, RGB565, RGB5A3)
            C8 = 0x09, //8, 8, 4, 32 | palette (IA8, RGB565, RGB5A3)
            C14X2 = 0x0a, //16 (14 used) | 4 | 4 | 32 | palette (IA8, RGB565, RGB5A3)
            CMPR = 0x0e, //4 | 8 | 8 | 32 | mini palettes in each block, RGB565 or transparent.
        }

        public class FileHeader
        {
            private ImageFormat _format;
            private byte _alphaEnabled; //0 for no alpha, 0x02 (and anything else) for alpha enabled
            private ushort _width;
            private ushort _height;
            private byte _wrapS; //0 or 1 (Clamp or Repeat?)
            private byte _wrapT;
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
            private uint _imageDataOffset; //Relative to file header

            public void Load(byte[] data, uint offset)
            {
                _format = (ImageFormat) FSHelpers.Read8(data, (int)offset + 0x00);
                _alphaEnabled = FSHelpers.Read8(data, (int) offset + 0x01);
                _width = (ushort)FSHelpers.Read16(data, (int) offset + 0x02);
                _height =(ushort)FSHelpers.Read16(data, (int) offset + 0x04);
                _wrapS = FSHelpers.Read8(data, (int) offset + 0x06);
                _wrapT = FSHelpers.Read8(data, (int) offset + 0x07);
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
                _imageDataOffset = (uint) FSHelpers.Read32(data, (int) offset + 0x1C);
            }
        }
    }
}