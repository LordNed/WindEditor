using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class ActorEditor : UserControl
    {
        private WindWakerEntityData.ActrChunk _curChunk;

        public ActorEditor()
        {
            InitializeComponent();
        }

        private void ActorEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.ActrChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldName.Text = _curChunk.Name;
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldRpatIndex.Value = _curChunk.RpatIndex;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldBehaviorType.Value = _curChunk.BehaviorType;
            fieldPosition.SetValue(_curChunk.Transform.Position);
            fieldRotation.SetXValue(_curChunk.Rotation.X);
            fieldRotation.SetYValue(_curChunk.Rotation.Y);
            fieldRotation.SetZValue(_curChunk.Rotation.Z);
            fieldEnemyNumber.Value = _curChunk.EnemyNumber;
        }

        private void fieldName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.Name = fieldName.Text;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (byte)fieldUnknown1.Value;
        }

        private void fieldRpatIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RpatIndex = (byte)fieldRpatIndex.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (byte)fieldUnknown2.Value;
        }

        private void fieldBehaviorType_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.BehaviorType = (byte)fieldBehaviorType.Value;
        }

        private void fieldPosition_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Transform.Position.X = (float)num.Value;
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

        private void fieldRotation_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Rotation.X = (short)num.Value;
        }

        private void fieldRotation_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Rotation.Y = (short)num.Value;
        }

        private void fieldRotation_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Rotation.Z = (short)num.Value;
        }

        private void fieldEnemyNumber_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.EnemyNumber = (ushort)fieldEnemyNumber.Value;
        }
    }
}
