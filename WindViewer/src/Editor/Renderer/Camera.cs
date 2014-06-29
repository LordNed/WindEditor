using System;
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
        public float FarClipPlane = 45000f;
        /// <summary> Vertical field of view in degrees. </summary>
        public float FieldOfView = 45f;
        /// <summary> Viewport width/height. Read only. </summary>
        public float AspectRatio { get { return PixelWidth / (float)PixelHeight; } }
        /// <summary> Width of the camera viewport in pixels. Read only. </summary>
        public int PixelWidth { get { return (int)(Rect.Width * Display.Width); }}
        /// <summary> Height of the camera viewport in pixels. Read only. </summary>
        public int PixelHeight { get { return (int)(Rect.Height * Display.Height); } }
        /// <summary> Color to clear the backbuffer with. </summary>
        public Color ClearColor;
        /// <summary> Where on screen the camera is rendered (in normalized coordinates) </summary>
        public Rect Rect
        {
            get
            {
                return _rect;
            }
            set
            {
                _rect.Width = MathExtensions.Clamp(value.Width, 0, 1);
                _rect.Height = MathExtensions.Clamp(value.Height, 0, 1);
                _rect.X = MathExtensions.Clamp(value.X, 0, 1);
                _rect.Y = MathExtensions.Clamp(value.Y, 0, 1);

                //Update the Proj matrix.
                GetViewProjMatrix();
            }
        }

        //ToDo: Camera movement should really go onto a component for the Camera.
        public Transform Transform { get; private set; }
        public float MoveSpeed = 1000f;
        public float MouseSensitivity = 0.1f;

        private Rect _rect;
        private Matrix4 _projMatrix;
        private Matrix4 _viewMatrix;

        public Camera()
        {
            Transform = new Transform();
            Rect = new Rect(1, 1, 0, 0);
            ClearColor = Color.SeaGreen;
        }

        public Ray ViewportPointToRay(Vector3 mousePos)
        {
            Vector4 unProject = UnProject(ref _projMatrix, _viewMatrix, new Size(PixelWidth, PixelHeight), mousePos);
            Vector3 projPos = unProject.Xyz;
            Vector3 startPos = Transform.Position;

            return new Ray(Transform.Position, (projPos-startPos).Normalized());
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = Vector3.Zero;
            offset += Transform.Right*x;
            offset += Transform.Forward*z;
            offset.Y += y;

            offset.NormalizeFast();

            float moveSpeed = MoveSpeed;
            if (Input.GetKey(Keys.ShiftKey))
                moveSpeed *= 2;
            Transform.Position += Vector3.Multiply(offset, moveSpeed * MainEditor.DeltaTime);
        }

        public void Rotate(float x, float y)
        {
            
            Transform.Rotate(Vector3.UnitY, -x * MouseSensitivity);
            Transform.Rotate(Transform.Right, y * MouseSensitivity);
            Vector3 up = Vector3.Cross(Transform.Forward, Transform.Right);
            if (Vector3.Dot(up, Vector3.UnitY) <= 0.001)
            {
                //rotate back if we went too far...
                Transform.Rotate(Transform.Right, -y * MouseSensitivity);
            }

        }

        public static Vector4 UnProject(ref Matrix4 projection, Matrix4 view, Size viewport, Vector3 mouse)
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

        public Matrix4 GetViewProjMatrix()
        {
            _viewMatrix = Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Forward, Vector3.UnitY);
            _projMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FieldOfView), AspectRatio, NearClipPlane, FarClipPlane);

            return _viewMatrix*_projMatrix;
        }

        
    }
}
