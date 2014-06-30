using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using OpenTK.Math;

namespace OpenTK.Math
{
    /// <summary>
    /// Represents a basic three-dimensional plane where ax + by + cz + d = 0.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Plane : IEquatable<Plane>
    {
        #region Fields

        /// <summary>
        /// The A component of the Plane.
        /// </summary>
        public float A;

        /// <summary>
        /// The B component of the Plane.
        /// </summary>
        public float B;

        /// <summary>
        /// The C component of the Plane.
        /// </summary>
        public float C;

        /// <summary>
        /// The D component of the Plane.
        /// </summary>
        public float D;

        /// <summary>
        /// Zero Plane with all components set to 0.
        /// </summary>
        public static readonly Plane ZeroPlane = new Plane(Vector3.Zero, 0);

        /// <summary>
        /// The Marshaled size of a plane struct.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(new Plane());

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Plane from another plane.
        /// </summary>
        /// <param name="plane">The plane components</param>
        public Plane(Plane plane)
        {
            A = plane.A;
            B = plane.B;
            C = plane.C;
            D = plane.D;
        }

        /// <summary>
        /// Constructs a Plane from the supplied plane components.
        /// </summary>
        /// <param name="a">The A component of the Plane.</param>
        /// <param name="b">The B component of the Plane.</param>
        /// <param name="c">The C component of the Plane.</param>
        /// <param name="d">The D component of the Plane.</param>
        public Plane(float a, float b, float c, float d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        /// <summary>
        /// Constructs a Plane from a Normal vector and the D component.
        /// </summary>
        /// <param name="normal">The normal Vector of the Plane.</param>
        /// /// <param name="d">The D component of the Plane.</param>
        public Plane(Vector3 normal, float d)
        {
            A = normal.X;
            B = normal.Y;
            C = normal.Z;
            D = d;
        }

        /// <summary>
        /// Constructs a Plane from a Point and a Normal vector.
        /// </summary>
        /// <param name="point">A point on the Plane.</param>
        /// <param name="normal">The normal Vector of the Plane.</param>
        public Plane(Vector3 point, Vector3 normal)
        {
            A = normal.X;
            B = normal.Y;
            C = normal.Z;
            D = -Vector3.Dot(normal, point);
        }

        /// <summary>
        /// Constructs a Plane from three Points.
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <param name="p3">Point 3</param>
        public Plane(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 edge1 = p2 - p1;
            Vector3 edge2 = p3 - p1;

            Vector3 normal = Vector3.Cross(edge1, edge2);
            normal.Normalize();

            A = normal.X;
            B = normal.Y;
            C = normal.Z;
            D = -Vector3.Dot(normal, p1);
        }

        #endregion

        #region Opperator overloads

        /// <summary>
        /// Equality Opperator.
        /// </summary>
        /// <param name="left">Left Plane to compare.</param>
        /// <param name="right">Right Plane to compare.</param>
        /// <returns>true if the planes are equal, false otherwise.</returns>
        public static bool operator ==(Plane left, Plane right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality Opperator.
        /// </summary>
        /// <param name="left">Left Plane to compare.</param>
        /// <param name="right">Right Plane to compare.</param>
        /// <returns>true if the planes are not equal, false otherwise.</returns>
        public static bool operator !=(Plane left, Plane right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Functions

        #region This property

        /// <summary>
        /// Gets the individual Plane components in array notation.
        /// </summary>
        /// <param name="index">The index of the Plane component. (0=A, 1=B, 2=C, 3=D)</param>
        /// <returns>The indexed Plane component.</returns>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return A;
                    case 1: return B;
                    case 2: return C;
                    case 3: return D;
                    default: throw new ArgumentOutOfRangeException("index", index, "Should be 0, 1, 2 or 3.");
                }
            }
        }

        #endregion

        #region NormalLength

        /// <summary>
        /// Gets the length (magnitude) of the normal vector.
        /// </summary>
        public float NormalLength
        {
            get
            {
                return (float)System.Math.Sqrt(A * A + B * B + C * C);
            }
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Normalizes the plane.
        /// This method normalizes a plane so that |a, b, c| == 1.
        /// </summary>
        public void Normalize()
        {
            float scale = 1.0f / this.NormalLength;
            A *= scale;
            B *= scale;
            C *= scale;
            D *= scale;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Computes the dot product of a plane and a Vector3.
        /// The w parameter of the vector is assumed to be 1.
        /// </summary>
        /// <param name="v">The Vector3.</param>
        /// <returns>The dot product of this plane and the vector.</returns>
        public float Dot(Vector3 v)
        {
            return A * v.X +
                   B * v.Y +
                   C * v.Z +
                   D * 1;
        }

        /// <summary>
        /// Computes the dot product of a plane and a Vector4.
        /// </summary>
        /// <param name="v">The Vector4.</param>
        /// <returns>The dot product of this plane and the vector.</returns>
        public float Dot(Vector4 v)
        {
            return A * v.X +
                   B * v.Y +
                   C * v.Z +
                   D * v.W;
        }

        #endregion

        #region Scale

        /// <summary>
        /// Scales the current Plane by the given scaling factor.
        /// </summary>
        /// <param name="s">The scaling factor.</param>
        public void Scale(float s)
        {
            this.A = A * s;
            this.B = B * s;
            this.C = C * s;
            this.D = D * s;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms the Plane by a given Matrix.
        /// The input matrix is the inverse transpose of the actual transformation.
        /// The inverse transpose of the transformation matrix.
        /// The inverse transpose is required by this method so that the normal
        /// vector of the transformed plane can be correctly transformed as well.
        /// </summary>
        /// <param name="m">The Tranformation Matrix.</param>
        public void Transform(Matrix4 m)
        {
            throw new NotImplementedException("Not implemented The Transform Function Yet");
        }

        #endregion

        #region GetDistance

        /// <summary>
        /// Gets the Distance from the plane to a specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The distance from the plane to the point.</returns>
        public float GetDistance(Vector3 point)
        {
            Vector3 normal = new Vector3(A, B, C);

            return Vector3.Dot(normal, point) + D;
        }

        #endregion

        #endregion

        #region Static functions

        #region DotNormal

        /// <summary>
        /// Computes the dot product of a plane and a 3-D vector.
        /// The w parameter of the vector is assumed to be 0.
        /// </summary>
        /// <param name="p">The Plane.</param>
        /// <param name="v">The Vector3.</param>
        /// <returns>The dot product of this plane and the vector.</returns>
        public static float DotNormal(Plane p, Vector3 v)
        {
            return p.A * v.X +
                   p.B * v.Y +
                   p.C * v.Z +
                   p.D * 0;
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Returns the normal of a plane.
        /// </summary>
        /// <param name="p">The plane to normalize.</param>
        /// <returns>The normalized plane.</returns>
        public static Plane Normalize(Plane p)
        {
            float scale = 1.0f / p.NormalLength;
            p.A *= scale;
            p.B *= scale;
            p.C *= scale;
            p.D *= scale;

            return p;
        }

        #endregion

        #region IntersectLine

        /// <summary>
        /// Finds the intersection between a plane and a line.
        /// If the line is parallel to the plane, a Vector3 structure set to (0, 0, 0) is returned.
        /// </summary>
        /// <param name="p">Source Plane.</param>
        /// <param name="v1">Line starting point.</param>
        /// <param name="v2">Line ending point.</param>
        /// <returns></returns>
        public static Vector3 IntersectLine(Plane p, Vector3 v1, Vector3 v2)
        {
            throw new NotImplementedException("Not implemented The IntersectLine Function Yet");
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a Plane by a given Matrix.
        /// The input matrix is the inverse transpose of the actual transformation.
        /// The inverse transpose of the transformation matrix.
        /// The inverse transpose is required by this method so that the normal
        /// vector of the transformed plane can be correctly transformed as well.
        /// </summary>
        /// <param name="p">The Plane to transform.</param>
        /// <param name="m">The tranformation matrix.</param>
        /// <returns>The tranformed Plane</returns>
        public static Plane Transform(Plane p, Matrix4 m)
        {
            throw new NotImplementedException("Not implemented The Transform Function Yet");
        }

        #endregion

        #region GetDistance

        /// <summary>
        /// Gets the Distance from a plane to a specified point.
        /// </summary>
        /// <param name="plane">The Plane.</param>
        /// <param name="point">The Point.</param>
        /// <returns>The distance from the plane to the point.</returns>
        public static float GetDistance(Plane plane, Vector3 point)
        {
            Vector3 normal = new Vector3(plane.A, plane.B, plane.C);

            return Vector3.Dot(normal, point) + plane.D;
        }

        #endregion

        #endregion

        #region ToString

        /// <summary>
        /// Returns a System.String that represents the current Plane.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("({0}, {1}, {2}, {3})", A, B, C, D);
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode() ^ D.GetHashCode();
        }

        #endregion

        #region Equals

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Plane))
                return false;

            return this.Equals((Plane)obj);
        }

        #endregion

        #region IEquatable<Plane> Members

        /// <summary>Indicates whether the current plane is equal to another plane.</summary>
        /// <param name="p">A plane to compare with this plane.</param>
        /// <returns>true if the current plane is equal to the plane parameter; otherwise, false.</returns>
        public bool Equals(Plane p)
        {
            return
                A == p.A &&
                B == p.B &&
                C == p.C &&
                D == p.D;
        }

        #endregion
    }
}
