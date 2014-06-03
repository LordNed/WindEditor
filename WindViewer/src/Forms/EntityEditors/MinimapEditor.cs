using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class MinimapEditor : UserControl
    {
        private WindWakerEntityData.TwoDMAChunk _curChunk;

        public MinimapEditor()
        {
            InitializeComponent();
        }

        private void _2DMapEditor_Load(object sender, System.EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.TwoDMAChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldFullMapImageScaleX.Value = (decimal) _curChunk.FullMapImageScaleX;
            fieldFullMapImageScaleY.Value = (decimal) _curChunk.FullMapImageScaleY;

            fieldFullMapSpaceScaleX.Value = (decimal) _curChunk.FullMapSpaceScaleX;
            fieldFullMapSpaceScaleY.Value = (decimal)_curChunk.FullMapSpaceScaleY;

            fieldFullMapXCoord.Value = (decimal) _curChunk.FullMapXCoord;
            fieldFullMapYCoord.Value = (decimal)_curChunk.FullMapYCoord;

            fieldZoomedMapXScroll1.Value = (decimal) _curChunk.ZoomedMapXScrolling1;
            fieldZoomedMapYScroll1.Value = (decimal)_curChunk.ZoomedMapYScrolling1;

            fieldZoomedMapXScroll2.Value = (decimal)_curChunk.ZoomedMapXScrolling2;
            fieldZoomedMapYScroll2.Value = (decimal)_curChunk.ZoomedMapYScrolling2;

            fieldZoomedMapXCoord.Value = (decimal) _curChunk.ZoomedMapXCoord;
            fieldZoomedMapYCoord.Value = (decimal)_curChunk.ZoomedMapYCoord;

            fieldZoomedMapScale.Value = (decimal) _curChunk.ZoomedMapScale;

            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldMapIndex.Value = _curChunk.MapIndex;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldPadding.Value = _curChunk.Padding;
        }

        private void fieldFullMapImageScaleX_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.FullMapImageScaleX = (float) fieldFullMapImageScaleX.Value;
        }

        private void fieldFullMapImageScaleY_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.FullMapImageScaleY = (float)fieldFullMapImageScaleY.Value;
        }

        private void fieldFullMapSpaceScaleX_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.FullMapSpaceScaleX = (float) fieldFullMapSpaceScaleX.Value;
        }

        private void fieldFullMapSpaceScaleY_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.FullMapSpaceScaleY = (float) fieldFullMapSpaceScaleY.Value;
        }

        private void fieldFullMapXCoord_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.FullMapXCoord = (float) fieldFullMapXCoord.Value;
        }

        private void fieldFullMapYCoord_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.FullMapYCoord = (float) fieldFullMapYCoord.Value;
        }

        private void fieldZoomedMapXScroll1_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.ZoomedMapXScrolling1 = (float) fieldZoomedMapXScroll1.Value;
        }

        private void fieldZoomedMapYScroll1_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.ZoomedMapYScrolling1 = (float) fieldZoomedMapYScroll1.Value;
        }

        private void fieldZoomedMapXScroll2_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.ZoomedMapXScrolling2 = (float) fieldZoomedMapXScroll2.Value;
        }

        private void fieldZoomedMapYScroll2_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.ZoomedMapYScrolling2 = (float) fieldZoomedMapYScroll2.Value;
        }

        private void fieldZoomedMapXCoord_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.ZoomedMapXCoord = (float) fieldZoomedMapXCoord.Value;
        }

        private void fieldZoomedMapYCoord_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.ZoomedMapYCoord = (float) fieldZoomedMapYCoord.Value;
        }

        private void fieldZoomedMapScale_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.ZoomedMapScale = (float) fieldZoomedMapScale.Value;
        }

        private void fieldUnknown1_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Unknown1 = (byte) fieldUnknown1.Value;
        }

        private void fieldMapIndex_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.MapIndex = (byte)fieldMapIndex.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Unknown2 = (byte)fieldUnknown2.Value;
        }

        private void fieldPadding_ValueChanged(object sender, System.EventArgs e)
        {
            _curChunk.Padding = (byte)fieldPadding.Value;
        }
    }
}
