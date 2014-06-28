using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameFormatReader.Common;
using OpenTK;
using WindViewer.Editor;

namespace WindViewer.FileFormats
{
    public class HalfRotation
    {
        /// <summary> Used to convert from -32768/32767 to -180/180 </summary>
        private const float RotationConversion = 182.04444444444f;

        public short X, Y, Z;

        public HalfRotation()
        {
            X = Y = Z = 0;
        }

        public HalfRotation(EndianBinaryReader reader)
        {
            X = reader.ReadInt16();
            Y = reader.ReadInt16();
            Z = reader.ReadInt16();
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public Vector3 ToDegrees()
        {
            Vector3 rot = new Vector3();
            rot.X = X / RotationConversion;
            rot.Y = Y / RotationConversion;
            rot.Z = Z / RotationConversion;

            return rot;
        }

        public void SetDegrees(Vector3 rot)
        {
            X = (short)(rot.X * RotationConversion);
            Y = (short)(rot.Y * RotationConversion);
            Z = (short)(rot.Z * RotationConversion);
        }

        [Obsolete("Use Write instead.")]
        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(X).Reverse());
            bytes.AddRange(BitConverter.GetBytes(Y).Reverse());
            bytes.AddRange(BitConverter.GetBytes(Z).Reverse());

            return bytes.ToArray();
        }
    }

    public class HalfRotationSingle
    {
        /// <summary> Used to convert from -32768/32767 to -180/180 </summary>
        private const float RotationConversion = 182.04444444444f;

        public short Value;

        public HalfRotationSingle()
        {
            Value = 0;
        }

        public HalfRotationSingle(EndianBinaryReader reader)
        {
            Value = reader.ReadInt16();
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Value);
        }

        public float ToDegrees()
        {
            return Value / RotationConversion;
        }

        public void SetDegrees(float rot)
        {
            Value = (short)(rot * RotationConversion);
        }
    }

        public class ByteColor
        {
            public byte R, G, B;

            public ByteColor()
            {
                R = B = G = 0;
            }

            public ByteColor(byte r, byte g, byte b)
            {
                R = r;
                G = g;
                B = b;
            }

            public ByteColor(EndianBinaryReader reader)
            {
                R = reader.ReadByte();
                G = reader.ReadByte();
                B = reader.ReadByte();
            }

            public void Write(EndianBinaryWriter writer)
            {
                writer.Write(R);
                writer.Write(G);
                writer.Write(B);
            }

            [Obsolete("Use Write instead")]
            public byte[] GetBytes()
            {
                byte[] bytes = new byte[3];
                bytes[0] = R;
                bytes[1] = G;
                bytes[2] = B;

                return bytes;
            }
        }

    public class ByteColorAlpha
    {
        public byte R, G, B, A;

        public ByteColorAlpha(byte[] data, ref int srcOffset)
        {
            R = FSHelpers.Read8(data, srcOffset + 0);
            G = FSHelpers.Read8(data, srcOffset + 1);
            B = FSHelpers.Read8(data, srcOffset + 2);
            A = FSHelpers.Read8(data, srcOffset + 3);

            srcOffset += 4;
        }

        public ByteColorAlpha()
        {
            R = G = B = A = 0;
        }

        public ByteColorAlpha(ByteColor color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = 0;
        }

        public ByteColorAlpha(EndianBinaryReader reader)
        {
            R = reader.ReadByte();
            G = reader.ReadByte();
            B = reader.ReadByte();
            A = reader.ReadByte();
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
            writer.Write(A);
        }

        [Obsolete("Use Write instead.")]
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[4];
            bytes[0] = R;
            bytes[1] = G;
            bytes[2] = B;
            bytes[3] = A;

            return bytes;
        }
    }
}
