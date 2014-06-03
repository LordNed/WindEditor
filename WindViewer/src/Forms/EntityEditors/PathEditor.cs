using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class PathEditor : UserControl
    {
        private WindWakerEntityData.RPATChunk _curChunk;

        public PathEditor()
        {
            InitializeComponent();
        }

        private void PathEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.RPATChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldNumPoints.Value = _curChunk.NumPoints;
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldUnknown3.Value = _curChunk.Unknown3;
            fieldPadding.Value = _curChunk.Padding;
            fieldEntryOffset.Value = _curChunk.FirstPointOffset;
        }

        private void fieldNumPoints_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.NumPoints = (ushort)fieldNumPoints.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (ushort)fieldUnknown1.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (byte)fieldUnknown2.Value;
        }

        private void fieldUnknown3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown3 = (byte)fieldUnknown3.Value;
        }

        private void fieldPadding_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding = (ushort)fieldPadding.Value;
        }

        private void fieldEntryOffset_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.FirstPointOffset = (int)fieldEntryOffset.Value;
        }

    }
}
