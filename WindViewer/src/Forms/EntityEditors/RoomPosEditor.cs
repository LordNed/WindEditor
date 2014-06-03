using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class RoomPosEditor : UserControl
    {
        private WindWakerEntityData.MultChunk _curChunk;

        public RoomPosEditor()
        {
            InitializeComponent();
        }

        private void RoomPosEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.MultChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldXPos.Value = (decimal)_curChunk.TranslationX;
            fieldYPos.Value = (decimal) _curChunk.TranslationY;
            fieldYRot.Value = _curChunk.YRotation;
            fieldRoomId.Value = _curChunk.RoomNumber;
            fieldUnknown1.Value = _curChunk.Unknown;
        }

        private void fieldXPos_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.TranslationX = (float) fieldXPos.Value;
        }

        private void fieldYPos_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.TranslationY = (float)fieldYPos.Value;
        }

        private void fieldYRot_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.YRotation = (short)fieldXPos.Value;
        }

        private void fieldRoomId_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RoomNumber = (byte) fieldRoomId.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown = (byte) fieldUnknown1.Value;
        }
    }
}
