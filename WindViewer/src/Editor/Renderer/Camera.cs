using OpenTK;
using OpenTK.Input;
using WindViewer.Forms;

namespace WindViewer.Editor
{
    public class Camera
    {
        public Transform transform { get; private set; }
        public float MoveSpeed = 800f;
        public float MouseSensitivity = 0.1f;

        public Camera()
        {
            transform = new Transform();
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = Vector3.Zero;
            offset += transform.Right*x;
            offset += transform.Forward*z;
            offset.Y += y;

            offset.NormalizeFast();

            float moveSpeed = MoveSpeed;
            if (EditorHelpers.KeysDown[(int) Key.ShiftLeft])
                moveSpeed *= 2;
            transform.Position += Vector3.Multiply(offset, moveSpeed * MainEditor.DeltaTime);
        }

        public void Rotate(float x, float y)
        {
            
            transform.Rotate(Vector3.UnitY, x * MouseSensitivity);
            transform.Rotate(transform.Right, y * MouseSensitivity);
            Vector3 up = Vector3.Cross(transform.Forward, transform.Right);
            if (Vector3.Dot(up, Vector3.UnitY) <= 0.001)
            {
                //rotate back if we went too far...
                transform.Rotate(transform.Right, -y * MouseSensitivity);
            }

        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(transform.Position, transform.Position + transform.Forward, Vector3.UnitY);
        }
    }
}
