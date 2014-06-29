using System;
using OpenTK;

namespace WindViewer.Editor
{
    public class Physics
    {
        public static bool RayVsAABB(Ray ray, Vector3 bboxMin, Vector3 bboxMax, out float distance)
        {
            /*Vector3 dirFrac = new Vector3();
            dirFrac.X = 1f/ray.Direction.X;
            dirFrac.Y = 1f/ray.Direction.Y;
            dirFrac.Z = 1f/ray.Direction.Z;

            float t1 = (bboxMin.X - ray.Origin.X)*dirFrac.X;
            float t2 = (bboxMax.X - ray.Origin.X)*dirFrac.X;
            float t3 = (bboxMin.Y - ray.Origin.Y)*dirFrac.Y;
            float t4 = (bboxMax.Y - ray.Origin.Y)*dirFrac.Y;
            float t5 = (bboxMin.Z - ray.Origin.Z)*dirFrac.Z;
            float t6 = (bboxMax.Z - ray.Origin.Z)*dirFrac.Z;

            float tMin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
            float tMax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

            //if tMax < 0, ray is intersecting AABB, but whole AABB is behind us.
            if (tMax < 0)
            {
                distance = tMax;
                return false;
            }

            //If tmin > tmax, ray doesn't intersect AABB
            if (tMin > tMax)
            {
                distance = tMax;
                return false;
            }

            distance = tMin;
            return true;*/

            Vector3 t1 = Vector3.Zero;
            Vector3 t2 = Vector3.Zero;

            float tNear = -float.MaxValue;
            float tFar = float.MaxValue;

            //For each axis
            for (int i = 0; i < 3; i++)
            {
                //If ray is parallel to plane in this direction
                if (Math.Abs(ray.Direction[0]) < float.Epsilon)
                {
                    if ((ray.Origin[i] < bboxMin[i]) || (ray.Origin[i] > bboxMax[i]))
                    {
                        //Parallel and outside box, no intersection possible.
                        distance = -1;
                        return false;
                    }
                }
                else
                {
                    //Ray is not parallel to a plane in that direction.
                    t1[i] = (bboxMin[i] - ray.Origin[i])/ray.Direction[i];
                    t2[i] = (bboxMax[i] - ray.Origin[i])/ray.Direction[i];

                    //Ensure T1 holds intersection with near plane.
                    if (t1[i] > t2[i])
                    {
                        float temp = t1[i];
                        t1[i] = t2[i];
                        t2[i] = temp;
                    }

                    if (t1[i] > tNear)
                        tNear = t1[i];
                    if (t2[i] < tFar)
                        tFar = t2[i];

                    if ((tNear > tFar) || (tFar < 0))
                    {
                        distance = -1;
                        return false;
                    }
                }
            }

            distance = tFar - tNear;
            return true;
        }
    }
}