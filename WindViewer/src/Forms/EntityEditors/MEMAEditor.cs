using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class MEMAEditor : UserControl
    {
        private WindWakerEntityData.MemaChunk _curChunk;

        public MEMAEditor()
        {
            InitializeComponent();
        }

        private void MEMAEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.MemaChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldNumBytes.Value = _curChunk.MemSize;
        }

        private void fieldNumBytes_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.MemSize = (int) fieldNumBytes.Value;
        }
    }
}
