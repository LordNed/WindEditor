using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindViewer.FileFormats;
using WindViewer.Forms;

namespace WindViewer.src.Forms.EntityEditors
{
    public partial class EventEditor : UserControl
    {
        private WindWakerEntityData.EvntChunk _curChunk;

        public EventEditor()
        {
            InitializeComponent();
        }

        private void EventEditor_Load(object sender, EventArgs e)
        {

            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.EvntChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldEventName.Text = _curChunk.EventName;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldUnknown3.Value = _curChunk.Unknown3;
            fieldUnknown4.Value = _curChunk.Unknown4;
            fieldUnknown5.Value = _curChunk.Unknown5;
            fieldRoomId.Value = _curChunk.RoomNumber;
            fieldPadding1.Value = _curChunk.Padding1;
            fieldPadding2.Value = _curChunk.Padding2;
            fieldPadding3.Value = _curChunk.Padding3;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (byte) fieldUnknown1.Value;
        }

        private void fieldEventName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.EventName = fieldEventName.Text;
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

        private void fieldUnknown5_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown5 = (byte)fieldUnknown5.Value;
        }

        private void fieldRoomId_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RoomNumber = (byte)fieldRoomId.Value;
        }

        private void fieldPadding1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding1 = (byte)fieldPadding1.Value;
        }

        private void fieldPadding2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding2 = (byte)fieldPadding2.Value;
        }

        private void fieldPadding3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding3 = (byte)fieldPadding3.Value;
        }
    }
}
