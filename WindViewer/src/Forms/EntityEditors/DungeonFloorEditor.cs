using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class DungeonFloorEditor : UserControl
    {
        private WindWakerEntityData.FlorChunk _curChunk;

        public DungeonFloorEditor()
        {
            InitializeComponent();
        }

        private void DungeonFloorEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.FlorChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldLowerY.Value = (decimal)_curChunk.LowerBoundaryYCoord;
            fieldFloorNum.Value = _curChunk.FloorId;
            fieldIncluded0.Value = _curChunk.IncludedRooms[0];
            fieldIncluded1.Value = _curChunk.IncludedRooms[1];
            fieldIncluded2.Value = _curChunk.IncludedRooms[2];
            fieldIncluded3.Value = _curChunk.IncludedRooms[3];
            fieldIncluded4.Value = _curChunk.IncludedRooms[4];
            fieldIncluded5.Value = _curChunk.IncludedRooms[5];
            fieldIncluded6.Value = _curChunk.IncludedRooms[6];
            fieldIncluded7.Value = _curChunk.IncludedRooms[7];
            fieldIncluded8.Value = _curChunk.IncludedRooms[8];
            fieldIncluded9.Value = _curChunk.IncludedRooms[9];
            fieldIncluded10.Value = _curChunk.IncludedRooms[10];
            fieldIncluded11.Value = _curChunk.IncludedRooms[11];
            fieldIncluded12.Value = _curChunk.IncludedRooms[12];
            fieldIncluded13.Value = _curChunk.IncludedRooms[13];
            fieldIncluded14.Value = _curChunk.IncludedRooms[14];
        }

        private void fieldLowerY_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.LowerBoundaryYCoord = (float) fieldLowerY.Value;
        }

        private void fieldFloorNum_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.FloorId = (byte) fieldFloorNum.Value;
        }

        private void fieldIncluded0_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[0] = (byte) fieldIncluded0.Value;
        }

        private void fieldIncluded1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[1] = (byte)fieldIncluded1.Value;
        }

        private void fieldIncluded2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[2] = (byte)fieldIncluded2.Value;
        }

        private void fieldIncluded3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[3] = (byte)fieldIncluded3.Value;
        }

        private void fieldIncluded4_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[4] = (byte)fieldIncluded4.Value;
        }

        private void fieldIncluded5_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[5] = (byte)fieldIncluded5.Value;
        }

        private void fieldIncluded6_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[6] = (byte)fieldIncluded6.Value;
        }

        private void fieldIncluded7_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[7] = (byte)fieldIncluded7.Value;
        }

        private void fieldIncluded8_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[8] = (byte)fieldIncluded8.Value;
        }

        private void fieldIncluded9_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[9] = (byte)fieldIncluded9.Value;
        }

        private void fieldIncluded10_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[10] = (byte)fieldIncluded10.Value;
        }

        private void fieldIncluded11_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[11] = (byte)fieldIncluded11.Value;
        }

        private void fieldIncluded12_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[12] = (byte)fieldIncluded12.Value;
        }

        private void fieldIncluded13_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[13] = (byte)fieldIncluded13.Value;
        }

        private void fieldIncluded14_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.IncludedRooms[14] = (byte)fieldIncluded14.Value;
        }
    }
}
