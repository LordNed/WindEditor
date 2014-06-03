using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class StagePropertyEditor : UserControl
    {
        private WindWakerEntityData.StagChunk _curChunk;

        public StagePropertyEditor()
        {
            InitializeComponent();
        }

        private void StagePropertyEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.StagChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldZMin.Value = (decimal)_curChunk.MinDepth;
            fieldZMax.Value = (decimal) _curChunk.MaxDepth;
            fieldKeyCounter.Value = _curChunk.KeyCounterDisplay;
            fieldParticleId.Value = _curChunk.LoadedParticleBank;
            fieldItemUsage.Value = _curChunk.ItemUsageAndMinimap;
            fieldPadding.Value = _curChunk.Padding;
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldUnknown3.Value = _curChunk.Unknown3;
            fieldDrawDistance.Value = _curChunk.DrawDistance;
        }

        private void fieldZMin_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MinDepth = (float) fieldZMin.Value;
        }

        private void fieldZMax_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MaxDepth = (float) fieldZMax.Value;
        }

        private void fieldKeyCounter_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.KeyCounterDisplay = (ushort) fieldKeyCounter.Value;
        }

        private void fieldParticleId_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.LoadedParticleBank = (ushort) fieldParticleId.Value;
        }

        private void fieldItemUsage_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ItemUsageAndMinimap = (ushort) fieldItemUsage.Value;
        }

        private void fieldPadding_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding = (byte) fieldPadding.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (byte) fieldUnknown1.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (byte)fieldUnknown2.Value;
        }

        private void fieldUnknown3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown3 = (byte)fieldUnknown3.Value;
        }

        private void fieldDrawDistance_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.DrawDistance = (ushort) fieldDrawDistance.Value;
        }
    }
}
