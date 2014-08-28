using OpenTK;

namespace WindViewer.Editor
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }

    public struct BoundingBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }
    }

    public struct Rect
    {
        public float Width;
        public float Height;
        public float X;
        public float Y;

        public Rect(float width, float height, float x, float y)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }
    }
}