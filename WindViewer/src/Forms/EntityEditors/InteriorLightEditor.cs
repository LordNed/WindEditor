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
    public partial class InteriorLightEditor : UserControl
    {
        private WindWakerEntityData.LghtChunk _curChunk;

        public InteriorLightEditor()
        {
            InitializeComponent();
        }

        private void InteriorLightEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.LghtChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldPosition.SetValue(_curChunk.Transform.Position);
            fieldRadius.SetValue(_curChunk.Scale);
            fieldColorR.Value = _curChunk.Color.R;
            fieldColorG.Value = _curChunk.Color.G;
            fieldColorB.Value = _curChunk.Color.B;
            fieldColorA.Value = _curChunk.Color.A;
        }

        private void fieldPosition_XValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown) sender;
            _curChunk.Transform.Position.X = (float) num.Value;
        }

        private void fieldPosition_YValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Y = (float)num.Value;
        }

        private void fieldPosition_ZValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Transform.Position.Z = (float)num.Value;
        }

        private void fieldRadius_XValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Scale.X = (float)num.Value;
        }

        private void fieldRadius_YValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Scale.Y = (float)num.Value;
        }

        private void fieldRadius_ZValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            _curChunk.Scale.Z = (float)num.Value;
        }

        private void fieldColorR_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Color.R = (byte)fieldColorR.Value;
        }

        private void fieldColorG_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Color.G = (byte)fieldColorG.Value;
        }

        private void fieldColorB_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Color.B = (byte)fieldColorB.Value;
        }

        private void fieldColorA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Color.A = (byte)fieldColorA.Value;
        }
    }
}
