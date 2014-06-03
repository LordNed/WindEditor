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
    public partial class CameraBehaviorEditor : UserControl
    {
        private WindWakerEntityData.RcamChunk _curChunk;

        public CameraBehaviorEditor()
        {
            InitializeComponent();
        }

        private void CameraBehaviorEditor_Load(object sender, EventArgs e)
        {

            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.RcamChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldCameraType.Text = _curChunk.CameraType;
            fieldRaroIndex.Value = _curChunk.RaroIndex;
            fieldPadding1.Value = _curChunk.Padding1;
            fieldPadding2.Value = _curChunk.Padding2;
            fieldPadding3.Value = _curChunk.Padding3;
        }

        private void fieldCameraType_TextChanged(object sender, EventArgs e)
        {
            _curChunk.CameraType = fieldCameraType.Text;
        }

        private void fieldRaroIndex_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.RaroIndex = (byte)fieldRaroIndex.Value;
        }

        private void fieldPadding1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding1 = (byte) fieldPadding1.Value;
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
