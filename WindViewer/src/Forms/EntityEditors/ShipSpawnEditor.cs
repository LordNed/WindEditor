using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class ShipSpawnEditor : UserControl
    {
        private WindWakerEntityData.ShipChunk _curChunk;

        public ShipSpawnEditor()
        {
            InitializeComponent();
        }

        private void ShipSpawnEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.ShipChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldPosition.SetValue(_curChunk.Transform.Position);
            fieldYRot.Value = _curChunk.YRotation;
            fieldUnknown1.Value = _curChunk.Unknown;
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

        private void fieldYRot_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.YRotation = (ushort) fieldYRot.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown = (ushort)fieldUnknown1.Value;
        }
    }
}
