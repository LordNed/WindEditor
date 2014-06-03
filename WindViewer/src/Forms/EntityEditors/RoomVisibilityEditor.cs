using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class RoomVisibilityEditor : UserControl
    {
        private WindWakerEntityData.RTBLChunk _curChunk;

        public RoomVisibilityEditor()
        {
            InitializeComponent();
        }

        private void RoomVisibilityEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.RTBLChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {

        }
    }
}
