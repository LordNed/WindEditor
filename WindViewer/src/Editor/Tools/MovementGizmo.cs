using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using OpenTK;
using WindViewer.Editor.Renderer;

namespace WindViewer.Editor.Tools
{
    public class MovementGizmo : IEditorTool
    {
        private enum AxisDirections
        {
            X = 0,
            Y = 1,
            Z = 2,
            /*XZ = 3, //Floor
            YX = 4, //Side 1
            YZ = 5, //Side 2*/
            None = -1,
        }

        private float _gizmoScale = 150f;
        private float _gizmoAxisWidth = 8f;
        private float _gizmoRayOffset;
        private Vector3 _gizmoPosAtStart;
        private bool _gizmoIsTracking;

        private AxisDirections _selectedAxis = AxisDirections.None;
        private BoundingBox[] _axisBoundingBoxes;

        private Color _xAxisColor = Color.Red;
        private Color _xAxisColorSelected = Color.DarkRed;

        private Color _yAxisColor = Color.GreenYellow;
        private Color _yAxisColorSelected = Color.LimeGreen;

        private Color _zAxisColor = Color.DeepSkyBlue;
        private Color _zAxisColorSelected = Color.Blue;

        private Transform transform = new Transform();

        public MovementGizmo()
        {
            _axisBoundingBoxes = new BoundingBox[3];

            //X Axis
            _axisBoundingBoxes[0] = new BoundingBox(new Vector3(0, -_gizmoAxisWidth, -_gizmoAxisWidth),
                new Vector3(_gizmoScale, _gizmoAxisWidth, _gizmoAxisWidth));
            //Y Axis
            _axisBoundingBoxes[1] = new BoundingBox(new Vector3(-_gizmoAxisWidth, 0, -_gizmoAxisWidth),
                new Vector3(_gizmoAxisWidth, _gizmoScale, _gizmoAxisWidth));
            //Z Axis
            _axisBoundingBoxes[2] = new BoundingBox(new Vector3(-_gizmoAxisWidth, -_gizmoAxisWidth, 0),
                new Vector3(_gizmoAxisWidth, _gizmoAxisWidth, _gizmoScale));


            transform.Rotate(new Vector3(0, 1, 0).Normalized(), 45f);
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _selectedAxis = CheckSelectedAxis();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selectedAxis = AxisDirections.None;
                _gizmoIsTracking = false;
                _gizmoRayOffset = 0f;
                _gizmoPosAtStart = Vector3.Zero;
            }

            if (_selectedAxis != AxisDirections.None)
            {
                switch (_selectedAxis)
                {
                    case AxisDirections.X:
                        HandleXAxisMovement();
                        break;
                }
            }

            //DebugRenderer.DrawLine(Vector3.Zero, new Vector3(0, 0, 500), Color.White);

            DrawGizmoDebugLines();
        }

        private void HandleXAxisMovement()
        {
            Ray mouseRay = Camera.Current.ViewportPointToRay(Input.MousePosition);

            //Transform the ray into AABB space based on the rotation.
            Quaternion invertRot = transform.Rotation;
            invertRot.Conjugate();
            mouseRay.Origin = invertRot.Mult(mouseRay.Origin);
            mouseRay.Direction = invertRot.Mult(mouseRay.Direction);
            mouseRay.Direction.Normalize();

            if (!_gizmoIsTracking)
            {
                
                Vector3 rayIntersect;
                float distance;
                
                _gizmoIsTracking = Physics.RayVsPlane(mouseRay, transform.Position, transform.Up, out distance, out rayIntersect);
                _gizmoPosAtStart = transform.Position;
                _gizmoRayOffset = (_gizmoPosAtStart - rayIntersect).X;
            }

            //Get their current mouse position
            Vector3 curRayPos;

            Physics.RayVsPlane(mouseRay, transform.Position, transform.Up, out curRayPos);

            //Get the difference on the x axis
            float deltaX = curRayPos.X - _gizmoPosAtStart.X;

            transform.Position = _gizmoPosAtStart + (transform.Right*deltaX);
            Console.WriteLine("delta: " + deltaX);
        }

        private AxisDirections CheckSelectedAxis()
        {
            Ray mouseRay = Camera.Current.ViewportPointToRay(Input.MousePosition);
            DebugRenderer.DrawLine(mouseRay.Origin, mouseRay.Origin + mouseRay.Direction * 25000, Color.Green, 5f);


            //Transform the ray into AABB space based on the rotation.
            Quaternion invertRot = transform.Rotation;
            invertRot.Conjugate();

            Vector3 prevOrig = mouseRay.Origin;
            mouseRay.Origin = invertRot.Mult(mouseRay.Origin);
            mouseRay.Direction = invertRot.Mult(mouseRay.Direction);
            mouseRay.Direction.Normalize();

            Console.WriteLine("Prev: {0} Post: {1}", prevOrig, mouseRay.Origin);

            DebugRenderer.DrawLine(mouseRay.Origin, mouseRay.Origin + mouseRay.Direction * 25000, Color.Yellow, 5f);

            float closestAxis = float.MaxValue;
            int selected = -1;
            for (int i = 0; i < _axisBoundingBoxes.Length; i++)
            {
                float distance;
                bool bIntersects = Physics.RayVsAABB(mouseRay, _axisBoundingBoxes[i].Min + transform.Position, _axisBoundingBoxes[i].Max + transform.Position, out distance);
                
                if(!bIntersects)
                    continue;
            
                if (distance < closestAxis)
                {
                    closestAxis = distance;
                    selected = i;
                }
            }

            return (AxisDirections) selected;
        }

        private void DrawGizmoDebugLines()
        {
            Color xColor = _selectedAxis == AxisDirections.X ? _xAxisColorSelected : _xAxisColor;
            Color yColor = _selectedAxis == AxisDirections.Y ? _yAxisColorSelected : _yAxisColor;
            Color zColor = _selectedAxis == AxisDirections.Z ? _zAxisColorSelected : _zAxisColor;

            //X
            DebugRenderer.DrawLine(transform.Position, transform.Position + transform.Right * _gizmoScale, xColor);
            //Y
            DebugRenderer.DrawLine(transform.Position, transform.Position + transform.Up * _gizmoScale, yColor);
            //Z
            DebugRenderer.DrawLine(transform.Position, transform.Position + transform.Forward * _gizmoScale, zColor);

            for (int i = 0; i < _axisBoundingBoxes.Length; i++)
            {
                BoundingBox bbox = _axisBoundingBoxes[i];
                DebugRenderer.DrawWireCube(transform.Position + (bbox.Max - bbox.Min)/2 - (new Vector3(_gizmoAxisWidth, _gizmoAxisWidth, _gizmoAxisWidth) ), Color.White, Quaternion.Identity,
                    (bbox.Max - bbox.Min)/2);
            }
        }

        public void PostRenderUpdate() { }
    }
}