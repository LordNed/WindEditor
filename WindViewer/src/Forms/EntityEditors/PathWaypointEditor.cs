using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class PathWaypointEditor : UserControl
    {
        private WindWakerEntityData.RppnChunk _curChunk;

        public PathWaypointEditor()
        {
            InitializeComponent();
        }

        private void PathWaypointEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.RppnChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldPosition.SetValue(_curChunk.Transform.Position);
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (uint) fieldUnknown1.Value;
        }

        private void fieldPosition_XValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown) sender;
            _curChunk.Transform.Position.X = (float) num.Value;
        }

        private void fieldPosition_YValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Y = (float)num.Value;
        }

        private void fieldPosition_ZValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Z = (float)num.Value;
        }
    }
}
