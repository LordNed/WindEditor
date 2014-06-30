using OpenTK;

namespace WindViewer.Editor
{
    public static class MathExtensions
    {
        public static Vector3 Mult(this Quaternion value, Vector3 vec)
        {
            Quaternion vectorQuat, inverseQuat, resultQuat;
            Vector3 resultVector;

            vectorQuat = new Quaternion(vec.X, vec.Y, vec.Z, 0f);
            inverseQuat = value.Invert_Custom();
            resultQuat = vectorQuat * inverseQuat;
            resultQuat = value*resultQuat;

            resultVector = new Vector3(resultQuat.X, resultQuat.Y, resultQuat.Z);
            return resultVector;
        }

        public static Quaternion Invert_Custom(this Quaternion value)
        {
            Quaternion newQuat = new Quaternion(value.X, value.Y, value.Z, value.W);
            float length = 1.0f / ((newQuat.X * newQuat.X) + (newQuat.Y * newQuat.Y) + (newQuat.Z * newQuat.Z) + (newQuat.W * newQuat.W));
            newQuat.X *= -length;
            newQuat.Y *= -length;
            newQuat.Z *= -length;
            newQuat.W *= length;

            return newQuat;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                value = max;
            if (value < min)
                value = min;

            return value;
        }
    }
}
