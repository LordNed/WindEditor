using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class TreasureChestEditor : UserControl
    {
        private WindWakerEntityData.TresChunk _curChunk;

        public TreasureChestEditor()
        {
            InitializeComponent();
        }

        private void TreasureChestEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.TresChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldChestName.Text = _curChunk.Name;
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldChestType.Value = _curChunk.ChestType;
            fieldPosition.SetValue(_curChunk.Transform.Position);
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldYRotation.Value = _curChunk.YRotation;
            fieldChestItem.Value = _curChunk.ChestContents;
            fieldPadding.Value = _curChunk.Padding;
        }

        private void fieldChestName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.Name = fieldChestName.Text;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (byte) fieldUnknown1.Value;
        }

        private void fieldChestType_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ChestType = (ushort) fieldChestType.Value;
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

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (ushort) fieldUnknown2.Value;
        }

        private void fieldYRotation_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.YRotation = (ushort) fieldYRotation.Value;
        }

        private void fieldChestItem_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ChestContents = (ushort) fieldChestItem.Value;
        }

        private void fieldPadding_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding = (ushort) fieldPadding.Value;
        }


    }
}
