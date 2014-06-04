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
