using System.Windows.Forms;
using OpenTK;

namespace WindViewer.Editor.Components.DefaultComponents
{
    public class FPSCameraMovement : BaseComponent
    {
        public float MoveSpeed = 1000f;
        public float MouseSensitivity = 0.1f;

        public FPSCameraMovement()
        {
            
        }

        public override void Update()
        {
            // ToDo: Anything but this.
            if (Input.GetKey(Keys.W))
                Move(0f, 0f, 1);
            if (Input.GetKey(Keys.S))
                Move(0f, 0f, -1);
            if (Input.GetKey(Keys.A))
                Move(1, 0f, 0f);
            if (Input.GetKey(Keys.D))
                Move(-1, 0f, 0f);

            if (Input.GetMouseButton(1))
            {
                Rotate(Input.MouseDelta.X, Input.MouseDelta.Y);
            }

            /*if (Input.GetKeyDown(Keys.Q))
            {
                Ray mouseRay = camera.ViewportPointToRay(Input.MousePosition);
                float distance;
                Vector3 point;

                bool bIntersects = Physics.RayVsPlane(mouseRay, new Plane(Vector3.Zero, Vector3.UnitY),
                    out distance, out point);

                DebugRenderer.DrawLine(mouseRay.Origin, mouseRay.Origin + mouseRay.Direction * 250f);
                Console.WriteLine("Intersects: {0}, Distance: {1} At: {2}", bIntersects, distance, point);

            }*/
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = Vector3.Zero;
            offset += transform.Right * x;
            offset += transform.Forward * z;
            offset.Y += y;

            offset.NormalizeFast();

            float moveSpeed = MoveSpeed;
            if (Input.GetKey(Keys.ShiftKey))
                moveSpeed *= 2;
            transform.Position += Vector3.Multiply(offset, moveSpeed * Time.DeltaTime);
        }

        public void Rotate(float x, float y)
        {

            transform.Rotate(Vector3.UnitY, -x * MouseSensitivity);
            transform.Rotate(transform.Right, y * MouseSensitivity);
            Vector3 up = Vector3.Cross(transform.Forward, transform.Right);
            if (Vector3.Dot(up, Vector3.UnitY) <= 0.001)
            {
                //rotate back if we went too far...
                transform.Rotate(transform.Right, -y * MouseSensitivity);
            }

        }
    }
}