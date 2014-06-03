using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class EnvironmentSoundEditor : UserControl
    {
        private WindWakerEntityData.SondChunk _curChunk;

        public EnvironmentSoundEditor()
        {
            InitializeComponent();
        }

        private void EnvironmentSoundEditor_Load(object sender, System.EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.SondChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldName.Text = _curChunk.Name;
            fieldPosition.SetValue(_curChunk.Transform.Position);
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldPadding1.Value = _curChunk.Padding;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldSoundId.Value = _curChunk.SoundId;
            fieldSoundRadius.Value = _curChunk.SoundRadius;
            fieldPadding2.Value = _curChunk.Padding2;
            fieldPadding3.Value = _curChunk.Padding3;
            fieldPadding4.Value = _curChunk.Padding4;
        }

        private void fieldName_TextChanged(object sender, System.EventArgs e)
        {
            _curChunk.Name = fieldName.Text;
        }

        private void fieldPosition_XValueChanged(object sender, System.EventArgs e)
        {
            var num = (NumericUpDown) sender;
            _curChunk.Transform.Position.X = (float)num.Value;
        }

        private void fieldPosition_YValueChanged(object sender, System.EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Y = (float)num.Value;
        }

        private void fieldPosition_ZValueChanged(object sender, System.EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Z = (float)num.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Unknown1 = (byte) fieldUnknown1.Value;
        }

        private void fieldPadding1_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Padding = (byte)fieldPadding1.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Unknown2 = (byte)fieldUnknown2.Value;
        }

        private void fieldSoundId_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.SoundId = (byte)fieldSoundId.Value;
        }

        private void fieldSoundRadius_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.SoundRadius = (byte)fieldSoundRadius.Value;
        }

        private void fieldPadding2_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Padding2 = (byte)fieldPadding2.Value;
        }

        private void fieldPadding3_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Padding3 = (byte)fieldPadding3.Value;
        }

        private void fieldPadding4_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Padding4 = (byte)fieldPadding4.Value;
        }
    }
}
