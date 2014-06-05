using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class ColoEditor : UserControl
    {
        private WindWakerEntityData.ColoChunk _curChunk;

        public ColoEditor()
        {
            InitializeComponent();
        }

        private void ColoEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.ColoChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldDawnA.Value = _curChunk.DawnIndexA;
            fieldMorningA.Value = _curChunk.MorningIndexA;
            fieldNoonA.Value = _curChunk.NoonIndexA;
            fieldAfternoonA.Value = _curChunk.AfternoonIndexA;
            fieldDuskA.Value = _curChunk.DuskIndexA;
            fieldNightA.Value = _curChunk.NightIndexA;

            fieldDawnB.Value = _curChunk.DawnIndexB;
            fieldMorningB.Value = _curChunk.MorningIndexB;
            fieldNoonB.Value = _curChunk.NoonIndexB;
            fieldAfternoonB.Value = _curChunk.AfternoonIndexB;
            fieldDuskB.Value = _curChunk.DuskIndexB;
            fieldNightB.Value = _curChunk.NightIndexB;
        }

        private void fieldDawnA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.DawnIndexA = (byte) fieldDawnA.Value;
        }

        private void fieldMorningA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MorningIndexA = (byte)fieldMorningA.Value;
        }

        private void fieldNoonA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.NoonIndexA = (byte) fieldNoonA.Value;
        }

        private void fieldAfternoonA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.AfternoonIndexA = (byte) fieldAfternoonA.Value;
        }

        private void fieldDuskA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.DuskIndexA = (byte) fieldDuskA.Value;
        }

        private void fieldNightA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.NightIndexA = (byte) fieldNightA.Value;
        }

        private void fieldDawnB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.DawnIndexB = (byte)fieldDawnB.Value;
        }

        private void fieldMorningB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MorningIndexB = (byte)fieldMorningB.Value;
        }

        private void fieldNoonB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.NoonIndexB = (byte)fieldNoonB.Value;
        }

        private void fieldAfternoonB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.AfternoonIndexB = (byte)fieldAfternoonB.Value;
        }

        private void fieldDuskB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.DuskIndexB = (byte)fieldDuskB.Value;
        }

        private void fieldNightB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.NightIndexB = (byte)fieldNightB.Value;
        }
    }
}
