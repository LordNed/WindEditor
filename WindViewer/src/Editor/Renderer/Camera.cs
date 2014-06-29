using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;
using WindViewer.Forms;

namespace WindViewer.Editor.Renderer
{
    public class Camera
    {
        /// <summary> The near clipping plane distance. </summary>
        public float NearClipPlane = 250f;
        /// <summary> The far clipping plane distance. </summary>
        public float FarClipPlane = 15000f;
        /// <summary> Vertical field of view in degrees. </summary>
        public float FieldOfView = 45f;
        /// <summary> Viewport width/height. Read only. </summary>
        public float AspectRatio { get { return _rect.Width / _rect.Height; } }
        /// <summary> Width of the camera viewport in pixels. Read only. </summary>
        public int PixelWidth { get { return (int)_rect.Width; }}
        /// <summary> Height of the camera viewport in pixels. Read only. </summary>
        public int PixelHeight { get { return (int)_rect.Height; } }

        //ToDo: Camera movement should really go onto a component for the Camera.
        public Transform transform { get; private set; }
        public float MoveSpeed = 1000f;
        public float MouseSensitivity = 0.1f;


        private Rect _rect;

        private Matrix4 _projMatrix;
        private Matrix4 _viewMatrix;

        public Camera()
        {
            transform = new Transform();
        }

        public Camera(Rect viewport)
        {
            _rect = viewport;
            transform = new Transform();
        }

        public Ray ViewportPointToRay(Vector2 position)
        {
            _projMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FieldOfView), AspectRatio, NearClipPlane, FarClipPlane);
            _viewMatrix = GetViewMatrix();
            Vector4 unProject = UnProject(ref _projMatrix, _viewMatrix, new Size(PixelWidth, PixelHeight), position);
            return new Ray(transform.Position, new Vector3(-unProject.Xyz.Normalized()));
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = Vector3.Zero;
            offset += transform.Right*x;
            offset += transform.Forward*z;
            offset.Y += y;

            offset.NormalizeFast();

            float moveSpeed = MoveSpeed;
            if (EditorHelpers.GetKey(Keys.ShiftKey))
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



        public static Vector4 UnProject(ref Matrix4 projection, Matrix4 view, Size viewport, Vector2 mouse)
        {
            Vector4 vec;

            vec.X = 2.0f * mouse.X / viewport.Width - 1;
            vec.Y = -(2.0f * mouse.Y / viewport.Height - 1);
            vec.Z = 0;
            vec.W = 1.0f;

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > float.Epsilon || vec.W < float.Epsilon)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            return vec;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(transform.Position, transform.Position + transform.Forward, Vector3.UnitY);
        }

        
    }
}
