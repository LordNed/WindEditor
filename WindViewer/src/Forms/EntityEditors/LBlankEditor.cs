using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class LBlankEditor : UserControl
    {
        private WindWakerEntityData.LbnkChunk _curChunk;

        public LBlankEditor()
        {
            InitializeComponent();
        }

        private void LBlankEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.LbnkChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldBlank.Value = _curChunk.Data;
        }

        private void fieldBlank_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Data = (byte) fieldBlank.Value;
        }
    }
}
