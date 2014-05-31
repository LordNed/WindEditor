using OpenTK;

namespace WindViewer.Editor
{
    public class Camera
    {
        public Transform transform { get; private set; }
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.1f;

        public Camera()
        {
            transform = new Transform();
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = Vector3.Zero;
            offset += transform.Right*-x;
            offset += transform.Forward*z;
            offset.Y += y;

            offset.NormalizeFast();
            transform.Position += Vector3.Multiply(offset, MoveSpeed);
        }

        public void Rotate(float x, float y)
        {
            transform.Rotate(transform.Up, x * MouseSensitivity);
            transform.Rotate(transform.Right, y * MouseSensitivity);
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(transform.Position, transform.Position + transform.Forward, Vector3.UnitY);
        }
    }
}
