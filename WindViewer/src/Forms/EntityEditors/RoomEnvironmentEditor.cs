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
    public partial class RoomEnvironmentEditor : UserControl
    {
        private WindWakerEntityData.FiliChunk _curChunk;

        public RoomEnvironmentEditor()
        {
            InitializeComponent();
        }

        private void RoomEnvironmentEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.FiliChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldTimePassage.Value = _curChunk.TimePassage;
            fieldWind.Value = _curChunk.WindSettings;
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldLighting.Value = _curChunk.LightingType;
            fieldUnknown2.Value = (decimal)_curChunk.Unknown2;
        }

        private void fieldTimePassage_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.TimePassage = (byte) fieldTimePassage.Value;
        }

        private void fieldWind_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.WindSettings = (byte)fieldWind.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (byte)fieldUnknown1.Value;
        }

        private void fieldLighting_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.LightingType = (byte)fieldLighting.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (byte)fieldUnknown2.Value;
        }
    }
}
