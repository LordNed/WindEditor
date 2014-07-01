using System;
using System.Reflection.Emit;
using OpenTK;

namespace WindViewer.Editor
{
    public class LQuaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public LQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public LQuaternion()
        {
            //Identity
            X = Y = Z = 0f;
            W = 1f;
        }

        public LQuaternion(Vector3 axis, float angle)
        {
            X = axis.X;
            Y = axis.Y;
            Z = axis.Z;
            W = angle;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public LQuaternion SetEulerAngles(float yaw, float pitch, float roll)
        {
            return SetEulerAnglesRad(MathHelper.DegreesToRadians(yaw), MathHelper.DegreesToRadians(pitch),
                MathHelper.DegreesToRadians(roll));
        }

        public LQuaternion SetEulerAnglesRad(float yaw, float pitch, float roll)
        {
            float hr = roll * 0.5f;
            float shr = (float)Math.Sin(hr);
            float chr = (float)Math.Cos(hr);
            float hp = pitch * 0.5f;
            float shp = (float)Math.Sin(hp);
            float chp = (float)Math.Cos(hp);
            float hy = yaw * 0.5f;
            float shy = (float)Math.Sin(hy);
            float chy = (float)Math.Cos(hy);

            float chy_shp = chy * shp;
            float shy_chp = shy * chp;
            float chy_chp = chy * chp;
            float shy_shp = shy * shp;

            X = (chy_shp * chr) + (shy_chp * shr); // cos(yaw/2) * sin(pitch/2) * cos(roll/2) + sin(yaw/2) * cos(pitch/2) * sin(roll/2)
            Y = (shy_chp * chr) - (chy_shp * shr); // sin(yaw/2) * cos(pitch/2) * cos(roll/2) - cos(yaw/2) * sin(pitch/2) * sin(roll/2)
            Z = (chy_chp * shr) - (shy_shp * chr); // cos(yaw/2) * cos(pitch/2) * sin(roll/2) - sin(yaw/2) * sin(pitch/2) * cos(roll/2)
            W = (chy_chp * chr) + (shy_shp * shr); // cos(yaw/2) * cos(pitch/2) * cos(roll/2) + sin(yaw/2) * sin(pitch/2) * sin(roll/2)
            return this;
        }

        public LQuaternion Normalize()
        {
            float len = Length() * Length();
            if (len != 0f && (Math.Abs(len - 1.0) > 0.00001f))
            {
                len = (float)Math.Sqrt(len);
                W /= len;
                X /= len;
                Y /= len;
                Z /= len;
            }

            return this;
        }

        public LQuaternion Conjugate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;

            return this;
        }

        public Vector3 Transform(Vector3 v)
        {
            LQuaternion cpy = new LQuaternion(X, Y, Z, W);
            cpy.Conjugate();
            cpy.MultiplyLeft(new LQuaternion(v.X, v.Y, v.Z, 0)).MultiplyLeft(this);

            v.X = cpy.X;
            v.Y = cpy.Y;
            v.Z = cpy.Z;

            return v;
        }

        public LQuaternion Multiply(LQuaternion other)
        {
            float newX = this.W * other.X + this.X * other.W + this.Y * other.Z - this.Z * other.Y;
            float newY = this.W * other.Y + this.Y * other.W + this.Z * other.X - this.X * other.Z;
            float newZ = this.W * other.Z + this.Z * other.W + this.X * other.Y - this.Y * other.X;
            float newW = this.W * other.W - this.X * other.X - this.Y * other.Y - this.Z * other.Z;
            this.X = newX;
            this.Y = newY;
            this.Z = newZ;
            this.W = newW;
            return this;
        }

        public LQuaternion MultiplyLeft(LQuaternion other)
        {
            float newX = other.W * this.X + other.X * this.W + other.Y * this.Z - other.Z * Y;
            float newY = other.W * this.Y + other.Y * this.W + other.Z * this.X - other.X * Z;
            float newZ = other.W * this.Z + other.Z * this.W + other.X * this.Y - other.Y * X;
            float newW = other.W * this.W - other.X * this.X - other.Y * this.Y - other.Z * Z;
            this.X = newX;
            this.Y = newY;
            this.Z = newZ;
            this.W = newW;
            return this;
        }
    }
}