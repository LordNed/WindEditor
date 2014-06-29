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
            Direction = direction.Normalized();
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