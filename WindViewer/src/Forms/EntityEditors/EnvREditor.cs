using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class EnvREditor : UserControl
    {
        private WindWakerEntityData.EnvrChunk _curChunk;

        public EnvREditor()
        {
            InitializeComponent();
        }

        private void EnvREditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.EnvrChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldClearA.Value = _curChunk.ClearColorIndexA;
            fieldRainingA.Value = _curChunk.RainingColorIndexA;
            fieldSnowingA.Value = _curChunk.SnowingColorIndexA;
            fieldUnknownA.Value = _curChunk.UnknownColorIndexA;
            fieldClearB.Value = _curChunk.ClearColorIndexB;
            fieldRainingB.Value = _curChunk.RainingColorIndexB;
            fieldSnowingB.Value = _curChunk.SnowingColorIndexB;
            fieldUnknownB.Value = _curChunk.UnknownColorIndexB;
        }

        private void fieldClearA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ClearColorIndexA = (byte) fieldClearA.Value;
        }

        private void fieldRainingA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RainingColorIndexA = (byte)fieldRainingA.Value;
        }

        private void fieldSnowingA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.SnowingColorIndexA = (byte)fieldSnowingA.Value;
        }

        private void fieldUnknownA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.UnknownColorIndexA = (byte)fieldUnknownA.Value;
        }

        private void fieldClearB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ClearColorIndexB = (byte)fieldClearB.Value;
        }

        private void fieldRainingB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RainingColorIndexB = (byte)fieldRainingB.Value;
        }

        private void fieldSnowingB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.SnowingColorIndexB = (byte)fieldSnowingB.Value;
        }

        private void fieldUnknownB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.UnknownColorIndexB = (byte)fieldUnknownB.Value;
        }
    }
}
