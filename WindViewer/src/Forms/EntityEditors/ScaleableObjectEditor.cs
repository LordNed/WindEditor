using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class ScaleableObjectEditor : UserControl
    {
        private WindWakerEntityData.ScobChunk _curChunk;

        public ScaleableObjectEditor()
        {
            InitializeComponent();
        }

        private void PlayerEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.ScobChunk;
            UpdateEditorUiFromFile();
        }


        private void UpdateEditorUiFromFile()
        {
            fieldObjName.Text = _curChunk.ObjectName;
            fieldParam1.Value = _curChunk.Param0;
            fieldParam2.Value = _curChunk.Param1;
            fieldParam3.Value = _curChunk.Param2;
            fieldParam4.Value = _curChunk.Param3;
            fieldPosition.SetValue(_curChunk.Position);
            fieldTextId.Value = _curChunk.TextId;
            fieldYRotation.Value = (decimal)_curChunk.YRotation.ToDegrees();
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldScale.SetXValue(_curChunk.ScaleX);
            fieldScale.SetYValue(_curChunk.ScaleY);
            fieldScale.SetZValue(_curChunk.ScaleZ);
            fieldPadding.Value = _curChunk.Padding;
        }

        private void fieldObjName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.ObjectName = fieldObjName.Text;
        }

        private void fieldParam1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param0 = (byte) fieldParam1.Value;
        }

        private void fieldParam2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param1 = (byte)fieldParam2.Value;
        }

        private void fieldParam3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param2 = (byte)fieldParam3.Value;
        }

        private void fieldParam4_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Param3 = (byte)fieldParam4.Value;
        }

        private void fieldPosition_XValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Position.X = (float) num.Value;
        }

        private void fieldPosition_YValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Position.Y = (float)num.Value;
        }

        private void fieldPosition_ZValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Position.Z = (float)num.Value;
        }

        private void fieldTextId_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.TextId = (ushort) fieldTextId.Value;
        }

        private void fieldYRotation_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.YRotation.Value = (ushort)fieldYRotation.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (ushort)fieldUnknown1.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (ushort)fieldUnknown2.Value;
        }

        private void fieldScale_XValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.ScaleX = (byte) num.Value;
        }

        private void fieldScale_YValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.ScaleY = (byte)num.Value;
        }

        private void fieldScale_ZValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.ScaleZ = (byte)num.Value;
        }

        private void fieldPadding_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding = (byte) fieldPadding.Value;
        }
    }
}
