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
                    case AxisDirections.Y:
                        HandleYAxisMovement();
                        break;
                    case AxisDirections.Z:
                        HandleZAxisMovement();
                        break;
                }
            }

            //DebugRenderer.DrawLine(Vector3.Zero, new Vector3(0, 0, 500), Color.White);

            DrawGizmoDebugLines();
        }

        private void HandleXAxisMovement()
        {
            Ray mouseRay = Camera.Current.ViewportPointToRay(Input.MousePosition);

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

            transform.Position = _gizmoPosAtStart + (transform.Right * deltaX) + new Vector3(_gizmoRayOffset, 0, 0);
        }

        private void HandleYAxisMovement()
        {
            Ray mouseRay = Camera.Current.ViewportPointToRay(Input.MousePosition);

            if (!_gizmoIsTracking)
            {

                Vector3 rayIntersect;
                float distance;

                _gizmoIsTracking = Physics.RayVsPlane(mouseRay, transform.Position, Camera.Current.Transform.Forward, out distance, out rayIntersect);
                _gizmoPosAtStart = transform.Position;
                _gizmoRayOffset = (_gizmoPosAtStart - rayIntersect).Y;
            }

            //Get their current mouse position
            Vector3 curRayPos;

            Physics.RayVsPlane(mouseRay, transform.Position, Camera.Current.Transform.Forward, out curRayPos);

            //Get the difference on the x axis
            float deltaY = curRayPos.Y - _gizmoPosAtStart.Y;

            transform.Position = _gizmoPosAtStart + (transform.Up*deltaY) + new Vector3(0, _gizmoRayOffset, 0);
        }

        private void HandleZAxisMovement()
        {
            Ray mouseRay = Camera.Current.ViewportPointToRay(Input.MousePosition);

            if (!_gizmoIsTracking)
            {

                Vector3 rayIntersect;
                float distance;

                _gizmoIsTracking = Physics.RayVsPlane(mouseRay, transform.Position, transform.Up, out distance, out rayIntersect);
                _gizmoPosAtStart = transform.Position;
                _gizmoRayOffset = (_gizmoPosAtStart - rayIntersect).Z;
            }

            //Get their current mouse position
            Vector3 curRayPos;

            Physics.RayVsPlane(mouseRay, transform.Position, transform.Up, out curRayPos);

            //Get the difference on the x axis
            float deltaZ = curRayPos.Z - _gizmoPosAtStart.Z;

            transform.Position = _gizmoPosAtStart + (transform.Forward * deltaZ) + new Vector3(0, 0, _gizmoRayOffset);
        }

        private AxisDirections CheckSelectedAxis()
        {
            Ray mouseRay = Camera.Current.ViewportPointToRay(Input.MousePosition);
            //DebugRenderer.DrawLine(mouseRay.Origin, mouseRay.Origin + mouseRay.Direction * 25000, Color.Green, 5f);

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