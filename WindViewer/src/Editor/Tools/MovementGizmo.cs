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
            }

            if (_selectedAxis != AxisDirections.None)
            {
                
            }

            //DebugRenderer.DrawLine(Vector3.Zero, new Vector3(0, 0, 500), Color.White);

            DrawGizmoDebugLines();
        }

        private AxisDirections CheckSelectedAxis()
        {
            Ray mouseRay = Camera.Current.ViewportPointToRay(Input.MousePosition);

            float closestAxis = float.MaxValue;
            int selected = -1;
            for (int i = 0; i < _axisBoundingBoxes.Length; i++)
            {
                float distance;
                bool bIntersects = Physics.RayVsAABB(mouseRay, _axisBoundingBoxes[i].Min, _axisBoundingBoxes[i].Max, out distance);
                
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
            DebugRenderer.DrawLine(transform.Position, transform.Right * _gizmoScale, xColor);
            //Y
            DebugRenderer.DrawLine(transform.Position, transform.Up * _gizmoScale, yColor);
            //Z
            DebugRenderer.DrawLine(transform.Position, transform.Forward * _gizmoScale, zColor);

            for (int i = 0; i < _axisBoundingBoxes.Length; i++)
            {
                BoundingBox bbox = _axisBoundingBoxes[i];
                DebugRenderer.DrawWireCube((bbox.Max - bbox.Min)/2 - (new Vector3(_gizmoAxisWidth, _gizmoAxisWidth, _gizmoAxisWidth) ), Color.White, Quaternion.Identity,
                    (bbox.Max - bbox.Min)/2);
            }
        }

        public void PostRenderUpdate() { }
    }
}