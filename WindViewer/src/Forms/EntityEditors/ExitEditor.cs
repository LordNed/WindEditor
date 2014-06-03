using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class ExitEditor : UserControl
    {
        private WindWakerEntityData.SclsChunk _curChunk;

        public ExitEditor()
        {
            InitializeComponent();
        }

        private void ExitEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.SclsChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldDestName.Text = _curChunk.DestinationName;
            fieldSpawnIndex.Value = _curChunk.SpawnNumber;
            fieldDestRoomIndex.Value = _curChunk.DestinationRoomNumber;
            fieldUnknown1.Value = _curChunk.ExitType;
            fieldPadding.Value = _curChunk.UnknownPadding;
        }

        private void fieldDestName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.DestinationName = fieldDestName.Text;
        }

        private void fieldSpawnIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.SpawnNumber = (byte)fieldSpawnIndex.Value;
        }

        private void fieldDestRoomIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.DestinationRoomNumber = (byte) fieldDestRoomIndex.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ExitType = (byte) fieldUnknown1.Value;
        }

        private void fieldPadding_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.UnknownPadding = (byte) fieldPadding.Value;
        }
    }


}
