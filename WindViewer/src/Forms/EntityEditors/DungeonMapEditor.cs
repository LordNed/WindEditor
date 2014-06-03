using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class DungeonMapEditor : UserControl
    {
        private WindWakerEntityData.DMAPChunk _curChunk;

        public DungeonMapEditor()
        {
            InitializeComponent();
        }

        private void DungeonMapEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.DMAPChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldMapSpaceX.Value = (decimal) _curChunk.MapSpaceX;
            fieldMapSpaceY.Value = (decimal) _curChunk.MapSpaceY;
            fieldMapSpaceScale.Value = (decimal) _curChunk.MapSpaceScale;
            fieldUnknown1.Value = (decimal) _curChunk.Unknown1;
        }

        private void fieldMapSpaceX_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MapSpaceX = (float) fieldMapSpaceX.Value;
        }

        private void fieldMapSpaceY_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MapSpaceY = (float) fieldMapSpaceY.Value;
        }

        private void fieldMapSpaceScale_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MapSpaceScale = (float) fieldMapSpaceScale.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (float) fieldUnknown1.Value;
        }
    }
}
