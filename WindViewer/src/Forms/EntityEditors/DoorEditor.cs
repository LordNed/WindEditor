using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class DoorEditor : UserControl
    {
        private WindWakerEntityData.TgdrChunk _curChunk;

        public DoorEditor()
        {
            InitializeComponent();
        }

        private void DoorEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.TgdrChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldName.Text = _curChunk.Name;
            fieldParam1.Value = _curChunk.Param1;
            fieldParam2.Value = _curChunk.Param2;
            fieldParam3.Value = _curChunk.Param3;
            fieldParam4.Value = _curChunk.Param4;
            fieldPosition.SetValue(_curChunk.Transform.Position);
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldYRotation.Value = _curChunk.YRotation;
            fieldDoorModel.Value = _curChunk.DoorModel;
            field3F.Value = _curChunk.Const3F;
            fieldZero.Value = _curChunk.ConstZero;
            fieldPadding.Value = _curChunk.Padding1;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldUnknown3.Value = _curChunk.Unknown3;
            fieldUnknown4.Value = _curChunk.Unknown4;
            fieldPadding2.Value = _curChunk.Padding2;
        }

        private void fieldName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.Name = fieldName.Text;
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
            var num = (NumericUpDown) sender;
            _curChunk.Transform.Position.X = (float) num.Value;
        }

        private void fieldPosition_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Y = (float)num.Value;
        }

        private void fieldPosition_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Z = (float)num.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (ushort) fieldUnknown1.Value;
        }

        private void fieldYRotation_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.YRotation = (ushort)fieldYRotation.Value;
        }

        private void fieldDoorModel_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.DoorModel = (byte)fieldDoorModel.Value;
        }

        private void field3F_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Const3F = (byte) field3F.Value;
        }

        private void fieldZero_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ConstZero = (byte) fieldZero.Value;
        }

        private void fieldPadding_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding1 = (byte) fieldPadding.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (byte)fieldUnknown2.Value;
        }

        private void fieldUnknown3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown3 = (byte)fieldUnknown3.Value;
        }

        private void fieldUnknown4_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown4 = (byte)fieldUnknown4.Value;
        }

        private void fieldPadding2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding2 = (byte) fieldPadding2.Value;
        }
    }
}
