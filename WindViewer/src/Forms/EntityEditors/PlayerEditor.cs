using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class PlayerEditor : UserControl
    {
        private WindWakerEntityData.PlyrChunk _curChunk;

        public PlayerEditor()
        {
            InitializeComponent();
        }

        private void PlayerEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += delegate(WindWakerEntityData.BaseChunk chunk)
            {
                _curChunk = chunk as WindWakerEntityData.PlyrChunk;
                UpdateEditorUiFromFile();
            };
        }

        private void UpdateEditorUiFromFile()
        {
            fieldName.Text = _curChunk.Name;
            fieldEventIndex.Value = _curChunk.EventIndex;
            fieldUnkn1.Value = _curChunk.Unknown1;
            fieldSpawnType.Value = _curChunk.SpawnType;
            fieldRoomNumber.Value = _curChunk.RoomNumber;
            fieldPosX.Value = (decimal) _curChunk.Transform.Position.X;
            fieldPosY.Value = (decimal) _curChunk.Transform.Position.Y;
            fieldPosZ.Value = (decimal) _curChunk.Transform.Position.Z;

            fieldRotX.Value = 255;
            fieldRotY.Value = 255;
            fieldRotZ.Value = 255;
        }

        private void fieldName_TextChanged(object sender, EventArgs e)
        {
            _curChunk.Name = fieldName.Text;
        }

        private void fieldEventIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.EventIndex = (byte) fieldEventIndex.Value;
        }

        private void fieldUnkn1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (byte) fieldUnkn1.Value;
        }

        private void fieldSpawnType_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.SpawnType = (byte) fieldSpawnType.Value;
        }

        private void fieldRoomNumber_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RoomNumber = (byte) fieldRoomNumber.Value;
        }

        private void fieldPos_ValueChanged(object sender, EventArgs e)
        {
            if(sender == fieldPosX)
                _curChunk.Transform.Position.X = (float) fieldPosX.Value;
            if(sender == fieldPosY)
                _curChunk.Transform.Position.Y = (float) fieldPosY.Value;
            if(sender == fieldPosZ)
                _curChunk.Transform.Position.Z = (float) fieldPosZ.Value;
        }

        private void fieldRot_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("NOT SUPPORTED YET STOP STOP STOP");
        }

    }
}
