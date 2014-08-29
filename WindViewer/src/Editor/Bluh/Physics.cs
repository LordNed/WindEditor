using System;
using OpenTK;
using OpenTK.Math;

namespace WindViewer.Editor
{
    public class Physics
    {
        /*public static bool RayVsAABB(Vector3 p, Vector3 d, Vector3 bboxMin, Vector3 bboxMax, out float tmin, out Vector3 q)
        {
            tmin = 0f; //Set to -float.MaxValue to get first hit on line.
            float tmax = float.MaxValue; //Set to max distance ray can travel (for segment)

            //For all three slaps
            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(d[i]) < float.Epsilon)
                {
                    //Ray is parallel to slab. No hit if origin not within slab.
                    if (p[i] < bboxMin[i] || p[i] > bboxMax[i])
                    {
                        q = Vector3.Zero;
                        return false;
                    }
                }
                else
                {
                    //Compute intersection t value of ray with near and far plane of slab.
                    float ood = 1.0f/d[i];
                    float t1 = (bboxMin[i] - p[i])*ood;
                    float t2 = (bboxMax[i] - p[i])*ood;

                    //Make t1 be intersection with near plane, t2 with far plane.
                    if (t1 > t2)
                    {
                        float temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }

                    //Comput intersection of slab intersection intervals
                    tmin = Math.Max(tmin, t1);
                    tmax = Math.Min(tmax, t2);

                    //Exit with no collision as soon as slab intersection becomes empty
                    if (tmin > tmax)
                    {
                        q = Vector3.Zero;
                        return false;
                    }
                }
            }

            //Ray intersects all three slabs. Return point q and intersection t value.
            q = p + d*tmin;
            return true;
        }*/

        public static bool RayVsPlane(Ray ray, Vector3 planeOrigin, Vector3 planeNormal)
        {
            float dummy1;
            Vector3 dummy2;
            return RayVsPlane(ray, planeOrigin, planeNormal, out dummy1, out dummy2);
        }

        public static bool RayVsPlane(Ray ray, Vector3 planeOrigin, Vector3 planeNormal, out Vector3 intersectPoint)
        {
            float dummy;
            return RayVsPlane(ray, planeOrigin, planeNormal, out dummy, out intersectPoint);
        }

        public static bool RayVsPlane(Ray ray, Vector3 planeOrigin, Vector3 planeNormal, out float distance, out Vector3 intersectPoint)
        {
            return RayVsPlane(ray, new Plane(planeOrigin, planeNormal), out distance, out intersectPoint);
        }

        public static bool RayVsPlane(Ray ray, Plane plane, out float outDistance, out Vector3 intersectPoint)
        {
            bool bIntersect = SegmentVsPlane(ray.Origin, ray.Origin + ray.Direction * float.MaxValue, plane, out outDistance, out intersectPoint);
            outDistance *= float.MaxValue;

            return bIntersect;
        }

        public static bool SegmentVsPlane(Vector3 pointA, Vector3 pointB, Plane plane, out float outFraction, out Vector3 intersectPoint)
        {
            //Compute the t value for the directed line ab intersecting the plane.
            Vector3 ab = pointB - pointA;

            //Plane ABC is the plane's normal.
            outFraction = (plane.D - Vector3.Dot(new Vector3(plane.A, plane.B, plane.C), pointA)) / Vector3.Dot(new Vector3(plane.A, plane.B, plane.C), ab);

            //If t in [0..1] compute and return intersection point
            if (outFraction >= 0f && outFraction <= 1f)
            {
                intersectPoint = pointA + outFraction * ab;
                return true;
            }

            //Else, no intersection.
            intersectPoint = Vector3.Zero;
            return false;
        }

        public static bool RayVsAABB(Ray ray, Vector3 bMin, Vector3 bMax, out float distance)
        {
            float dirFracX = 1.0f / ray.Direction.X;
            float dirFracY = 1.0f / ray.Direction.Y;
            float dirFracZ = 1.0f / ray.Direction.Z;

            float t1 = (bMin.X - ray.Origin.X) * dirFracX;
            float t2 = (bMax.X - ray.Origin.X) * dirFracX;
            float t3 = (bMin.Y - ray.Origin.Y) * dirFracY;
            float t4 = (bMax.Y - ray.Origin.Y) * dirFracY;
            float t5 = (bMin.Z - ray.Origin.Z) * dirFracZ;
            float t6 = (bMax.Z - ray.Origin.Z) * dirFracZ;

            float tmin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
            float tmax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

            if (tmax < 0)
            {
                distance = tmax;
                return false;
            }

            if (tmin > tmax)
            {
                distance = tmax;
                return false;
            }

            distance = tmin;
            return true;
        }
    }
}