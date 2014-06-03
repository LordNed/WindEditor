using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class MECOEditor : UserControl
    {
        private WindWakerEntityData.MecoChunk _curChunk;

        public MECOEditor()
        {
            InitializeComponent();
        }

        private void MECOEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.MecoChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldRoomId.Value = _curChunk.RoomNumber;
            fieldMEMAIndex.Value = _curChunk.MemaIndex;
        }

        private void fieldRoomId_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RoomNumber = (byte) fieldRoomId.Value;
        }

        private void fieldMEMAIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MemaIndex = (byte)fieldMEMAIndex.Value;
        }
    }
}
