using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindViewer
{
    public static class Helpers
    {
        public static void Swap(ref byte V1, ref byte V2)
        {
            byte Tmp = V1; V1 = V2; V2 = Tmp;
        }

        public static void Swap(ref int V1, ref int V2)
        {
            int Tmp = V1; V1 = V2; V2 = Tmp;
        }

        public static Byte Read8(byte[] Data, int Offset)
        {
            return (Buffer.GetByte(Data, Offset));
        }

        public static UInt16 Read16(byte[] Data, int Offset)
        {
            return (UInt16)((Buffer.GetByte(Data, Offset) << 8) | Buffer.GetByte(Data, Offset + 1));
        }

        public static UInt32 Read32(byte[] Data, int Offset)
        {
            return (UInt32)((Buffer.GetByte(Data, Offset) << 24) | (Buffer.GetByte(Data, Offset + 1) << 16) | (Buffer.GetByte(Data, Offset + 2) << 8) | Buffer.GetByte(Data, Offset + 3));
        }

        public static UInt16 Read16Swap(byte[] Data, int Offset)
        {
            return (UInt16)((Buffer.GetByte(Data, Offset + 1) << 8) | Buffer.GetByte(Data, Offset));
        }

        public static UInt32 Read32Swap(byte[] Data, int Offset)
        {
            return (UInt32)((Buffer.GetByte(Data, Offset + 3) << 24) | (Buffer.GetByte(Data, Offset + 2) << 16) | (Buffer.GetByte(Data, Offset + 1) << 8) | Buffer.GetByte(Data, Offset));
        }

        public static byte[] LoadBinary(string Path)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(Path));
            byte[] Data = br.ReadBytes((int)br.BaseStream.Length);
            br.Close();
            return Data;
        }

        public static string ReadString(byte[] Data, ref int Offset)
        {
            if (Offset >= Data.Length) return null;
            while (Data[Offset] == 0) Offset++;
            int Length = Array.IndexOf(Data, (byte)0, Offset) - Offset;
            string ReturnString = ReadString(Data, Offset, Length);
            Offset += Length;
            return ReturnString;
        }

        public static string ReadString(byte[] Data, int Offset)
        {
            if (Offset >= Data.Length) return null;
            while (Data[Offset] == 0) Offset++;
            int Length = Array.IndexOf(Data, (byte)0, Offset) - Offset;
            return ReadString(Data, Offset, Length);
        }

        public static string ReadString(byte[] Data, int Offset, int Length)
        {
            if (Offset >= Data.Length) return null;
            while (Data[Offset] == 0) Offset++;
            byte[] TempBuffer = new Byte[Length + 1];
            Buffer.BlockCopy(Data, Offset, TempBuffer, 0, Length);
            return Encoding.GetEncoding(1251).GetString(TempBuffer, 0, Array.IndexOf(TempBuffer, (byte)0));
        }

        public static float ConvertIEEE754Float(UInt32 Raw)
        {
            byte[] Data = new byte[4];
            for (int i = 0; i < 4; i++)
                Data[i] = (byte)((Raw >> (i * 8)) & 0xFF);
            return BitConverter.ToSingle(Data, 0);
        }

        public static SizeF MeasureString(string s, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(image))
                {
                    result = g.MeasureString(s, font);
                }
            }
            return result;
        }
    }

    public static class ArrayExtensions
    {
        public static void Init<T>(this T[] array, T defaultValue)
        {
            if (array == null)
                return;

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }
        }

        public static void Fill<T>(this T[] array, T[] data)
        {
            if (array == null)
                return;

            for (int i = 0; i < array.Length; i += data.Length)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    try
                    {
                        array[i + j] = data[j];
                    }
                    catch
                    {
                        return;
                    }
                }
            }
        }

        public static string GetContentString<T>(this T[] array)
        {
            return "(" + string.Join(", ", array.Select(x => x.ToString()).ToArray()) + ")";
        }
    }
}
