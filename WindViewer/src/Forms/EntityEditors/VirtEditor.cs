using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class VirtEditor : UserControl
    {
        private WindWakerEntityData.VirtChunk _curChunk;

        public VirtEditor()
        {
            InitializeComponent();
        }

        private void VirtEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.VirtChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldUnknown1.Value = _curChunk.Unknown1;
            fieldUnknown2.Value = _curChunk.Unknown2;
            fieldUnknown3.Value = _curChunk.Unknown3;
            fieldUnknown4.Value = _curChunk.Unknown4;

            fieldHorizonColor.SetXValue(_curChunk.HorizonCloudColor.R);
            fieldHorizonColor.SetYValue(_curChunk.HorizonCloudColor.G);
            fieldHorizonColor.SetZValue(_curChunk.HorizonCloudColor.B);
            fieldHorzCloudA.Value = _curChunk.HorizonCloudColor.A;

            fieldCenterCloudRGB.SetXValue(_curChunk.CenterCloudColor.R);
            fieldCenterCloudRGB.SetYValue(_curChunk.CenterCloudColor.G);
            fieldCenterCloudRGB.SetZValue(_curChunk.CenterCloudColor.B);
            fieldHorzCloudA.Value = _curChunk.CenterCloudColor.A;

            fieldSkyColor.SetXValue(_curChunk.CenterSkyColor.R);
            fieldSkyColor.SetYValue(_curChunk.CenterSkyColor.G);
            fieldSkyColor.SetZValue(_curChunk.CenterSkyColor.B);

            fieldHorizonColor.SetXValue(_curChunk.HorizonColor.R);
            fieldHorizonColor.SetYValue(_curChunk.HorizonColor.G);
            fieldHorizonColor.SetZValue(_curChunk.HorizonColor.B);

            fieldSkyFadeTo.SetXValue(_curChunk.SkyFadeTo.R);
            fieldSkyFadeTo.SetYValue(_curChunk.SkyFadeTo.G);
            fieldSkyFadeTo.SetZValue(_curChunk.SkyFadeTo.B);

            fieldPadding.SetXValue(_curChunk.Padding1);
            fieldPadding.SetYValue(_curChunk.Padding2);
            fieldPadding.SetZValue(_curChunk.Padding3);
        }

        private void fieldUnknown1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown1 = (uint) fieldUnknown1.Value;
        }

        private void fieldUnknown2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown2 = (uint)fieldUnknown2.Value;
        }

        private void fieldUnknown3_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown3 = (uint)fieldUnknown3.Value;
        }

        private void fieldUnknown4_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Unknown4 = (uint)fieldUnknown4.Value;
        }

        private void fieldHorzCloudRGB_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown) sender;
            _curChunk.HorizonCloudColor.R = (byte) num.Value;
        }

        private void fieldHorzCloudRGB_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.HorizonCloudColor.G = (byte)num.Value;
        }

        private void fieldHorzCloudRGB_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.HorizonCloudColor.B = (byte)num.Value;
        }

        private void fieldHorzCloudA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.HorizonCloudColor.A = (byte) fieldHorzCloudA.Value;
        }

        private void fieldCenterCloudRGB_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.CenterCloudColor.R = (byte)num.Value;
        }

        private void fieldCenterCloudRGB_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.CenterCloudColor.G = (byte)num.Value;
        }

        private void fieldCenterCloudRGB_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.CenterCloudColor.B = (byte)num.Value;
        }

        private void fieldCenterCloudA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.CenterCloudColor.A = (byte)fieldCenterCloudA.Value;
        }

        private void fieldSkyColor_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.CenterSkyColor.R = (byte)num.Value;
        }

        private void fieldSkyColor_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.CenterSkyColor.G = (byte)num.Value;
        }

        private void fieldSkyColor_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.CenterSkyColor.B = (byte)num.Value;
        }

        private void fieldHorizonColor_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.HorizonColor.R = (byte)num.Value;
        }

        private void fieldHorizonColor_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.HorizonColor.G = (byte)num.Value;
        }

        private void fieldHorizonColor_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.HorizonColor.B = (byte)num.Value;
        }

        private void fieldSkyFadeTo_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.SkyFadeTo.R = (byte)num.Value;
        }

        private void fieldSkyFadeTo_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.SkyFadeTo.G = (byte)num.Value;
        }

        private void fieldSkyFadeTo_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.SkyFadeTo.B = (byte)num.Value;
        }

        private void fieldPadding_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Padding1 = (byte)num.Value;
        }

        private void fieldPadding_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Padding2 = (byte)num.Value;
        }

        private void fieldPadding_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.Padding3 = (byte)num.Value;
        }
    }
}
