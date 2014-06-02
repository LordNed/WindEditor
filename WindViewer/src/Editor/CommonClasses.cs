using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace WindViewer.Editor
{
    public class HalfRotation
    {
        public short X, Y, Z;

        public HalfRotation()
        {
            X = Y = Z = 0;
        }

        public HalfRotation(byte[] data, ref int srcOffset)
        {
            X = (short)FSHelpers.Read16(data, srcOffset);
            Y = (short)FSHelpers.Read16(data, srcOffset + 2);
            Z = (short)FSHelpers.Read16(data, srcOffset + 4);

            srcOffset += 6;
        }

        public Vector3 ToDegrees()
        {
            Vector3 rot = new Vector3();
            rot.X = X / 182.04444444444f;
            rot.Y = Y / 182.04444444444f;
            rot.Z = Z / 182.04444444444f;

            return rot;
        }

        public void SetDegrees(Vector3 rot)
        {
            X = (short)(rot.X * 182.04444444444f);
            Y = (short)(rot.Y * 182.04444444444f);
            Z = (short)(rot.Z * 182.04444444444f);
        }

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
        public ushort Value;

        public HalfRotationSingle()
        {
            Value = 0;
        }

        public HalfRotationSingle(byte[] data, int srcOffset)
        {
            Value = (ushort)FSHelpers.Read16(data, srcOffset);
        }

        public float ToDegrees()
        {
            return Value / 182.04444444444f;
        }

        public void SetDegrees(float rot)
        {
            Value = (ushort)(rot * 182.04444444444f);
        }
    }

        public class ByteColor
        {
            public byte R, G, B;

            public ByteColor()
            {
                R = B = G = 0;
            }

            public ByteColor(byte[] data, ref int srcOffset)
            {
                R = FSHelpers.Read8(data, srcOffset + 0);
                G = FSHelpers.Read8(data, srcOffset + 1);
                B = FSHelpers.Read8(data, srcOffset + 2);

                srcOffset += 3;
            }

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
