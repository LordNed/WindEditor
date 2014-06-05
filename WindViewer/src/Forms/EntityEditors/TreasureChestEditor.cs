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

        }

        private void fieldChestName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.Name = fieldChestName.Text;
        }

        private void fieldParam1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param1 = (byte) fieldParam1.Value;
        }

        private void fieldParam2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param2 = (byte)fieldParam2.Value;
        }

        private void fieldParam3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param3 = (byte)fieldParam3.Value;
        }

        private void fieldParam4_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param4 = (byte)fieldParam4.Value;
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

        private void fieldRoomId_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RoomId = (ushort) fieldRoomId.Value;
        }

        private void fieldYRotation_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.YRotation = (ushort)fieldYRotation.Value;
        }

        private void fieldChestItem_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ChestItem = (byte)fieldChestItem.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (byte)fieldUnknown1.Value;
        }

        private void fieldPadding_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding = (ushort)fieldPadding.Value;
        }

        
    }
}
