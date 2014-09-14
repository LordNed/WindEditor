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
            fieldStageName.Text = _curChunk.StageName;
            fieldSpawnID.Value = _curChunk.SpawnID;
            fieldRoomID.Value = _curChunk.RoomID;
            fieldFadeoutID.Value = _curChunk.FadeoutID;
        }

        private void fieldDestName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.StageName = fieldStageName.Text;
        }

        private void fieldSpawnIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.SpawnID = (byte)fieldSpawnID.Value;
        }

        private void fieldDestRoomIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RoomID = (byte) fieldRoomID.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.FadeoutID = (byte) fieldFadeoutID.Value;
        }
    }


}
