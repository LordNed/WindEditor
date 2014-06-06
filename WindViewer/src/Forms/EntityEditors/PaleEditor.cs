using System;
using System.Windows.Forms;
using WindViewer.FileFormats;

namespace WindViewer.Forms.EntityEditors
{
    public partial class PaleEditor : UserControl
    {
        private WindWakerEntityData.PaleChunk _curChunk;

        public PaleEditor()
        {
            InitializeComponent();
        }

        private void PaleEditor_Load(object sender, EventArgs e)
        {
            MainEditor.SelectedEntityChanged += MainEditorOnSelectedEntityChanged;

            HandleDestroyed += delegate
            {
                MainEditor.SelectedEntityChanged -= MainEditorOnSelectedEntityChanged;
            };
        }

        private void MainEditorOnSelectedEntityChanged(WindWakerEntityData.BaseChunk baseChunk)
        {
            _curChunk = baseChunk as WindWakerEntityData.PaleChunk;
            UpdateEditorUiFromFile();
        }

        private void UpdateEditorUiFromFile()
        {
            fieldActorAmbient.SetXValue(_curChunk.ActorAmbient.R);
            fieldActorAmbient.SetYValue(_curChunk.ActorAmbient.G);
            fieldActorAmbient.SetZValue(_curChunk.ActorAmbient.B);

            fieldShadowColor.SetXValue(_curChunk.ShadowColor.R);
            fieldShadowColor.SetYValue(_curChunk.ShadowColor.G);
            fieldShadowColor.SetZValue(_curChunk.ShadowColor.B);

            fieldFillLight.SetXValue(_curChunk.RoomFillColor.R);
            fieldFillLight.SetYValue(_curChunk.RoomFillColor.G);
            fieldFillLight.SetZValue(_curChunk.RoomFillColor.B);

            fieldRoomAmbient.SetXValue(_curChunk.RoomAmbient.R);
            fieldRoomAmbient.SetYValue(_curChunk.RoomAmbient.G);
            fieldRoomAmbient.SetZValue(_curChunk.RoomAmbient.B);

            fieldWave.SetXValue(_curChunk.WaveColor.R);
            fieldWave.SetYValue(_curChunk.WaveColor.G);
            fieldWave.SetZValue(_curChunk.WaveColor.B);

            fieldOcean.SetXValue(_curChunk.OceanColor.R);
            fieldOcean.SetYValue(_curChunk.OceanColor.G);
            fieldOcean.SetZValue(_curChunk.OceanColor.B);

            fieldWhite1.SetXValue(_curChunk.UnknownColor1.R);
            fieldWhite1.SetYValue(_curChunk.UnknownColor1.G);
            fieldWhite1.SetZValue(_curChunk.UnknownColor1.B);

            fieldWhite2.SetXValue(_curChunk.UnknownColor2.R);
            fieldWhite2.SetYValue(_curChunk.UnknownColor2.G);
            fieldWhite2.SetZValue(_curChunk.UnknownColor2.B);

            fieldDoor.SetXValue(_curChunk.DoorwayColor.R);
            fieldDoor.SetYValue(_curChunk.DoorwayColor.G);
            fieldDoor.SetZValue(_curChunk.DoorwayColor.B);

            fieldUnknown3.SetXValue(_curChunk.UnknownColor3.R);
            fieldUnknown3.SetYValue(_curChunk.UnknownColor3.G);
            fieldUnknown3.SetZValue(_curChunk.UnknownColor3.B);

            fieldFog.SetXValue(_curChunk.FogColor.R);
            fieldFog.SetYValue(_curChunk.FogColor.G);
            fieldFog.SetZValue(_curChunk.FogColor.B);

            fieldVirt.Value = _curChunk.VirtIndex;
            fieldPadding1.Value = _curChunk.Padding1;
            fieldPadding2.Value = _curChunk.Padding2;

            fieldOceanFade.SetXValue(_curChunk.OceanFadeInto.R);
            fieldOceanFade.SetYValue(_curChunk.OceanFadeInto.G);
            fieldOceanFade.SetZValue(_curChunk.OceanFadeInto.B);
            fieldOceanFadeA.Value = _curChunk.OceanFadeInto.A;

            fieldShoreFade.SetXValue(_curChunk.ShoreFadeInto.R);
            fieldShoreFade.SetYValue(_curChunk.ShoreFadeInto.G);
            fieldShoreFade.SetZValue(_curChunk.ShoreFadeInto.B);
            fieldShoreFadeA.Value = _curChunk.ShoreFadeInto.A;
        }

        private void fieldActorAmbient_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown) sender;
            _curChunk.ActorAmbient.R = (byte)num.Value;
        }

        private void fieldActorAmbient_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ActorAmbient.G = (byte)num.Value;
        }

        private void fieldActorAmbient_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ActorAmbient.B = (byte)num.Value;
        }

        private void fieldShadowColor_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ShadowColor.R = (byte)num.Value;
        }

        private void fieldShadowColor_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ShadowColor.G = (byte)num.Value;
        }

        private void fieldShadowColor_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ShadowColor.B = (byte)num.Value;
        }

        private void fieldFillLight_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.RoomFillColor.R = (byte)num.Value;
        }

        private void fieldFillLight_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.RoomFillColor.G = (byte)num.Value;
        }

        private void fieldFillLight_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.RoomFillColor.B = (byte)num.Value;
        }

        private void fieldRoomAmbient_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.RoomAmbient.R = (byte)num.Value;
        }

        private void fieldRoomAmbient_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.RoomAmbient.G = (byte)num.Value;
        }

        private void fieldRoomAmbient_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.RoomAmbient.B = (byte)num.Value;
        }

        private void fieldWave_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.WaveColor.R = (byte)num.Value;
        }

        private void fieldWave_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.WaveColor.G= (byte)num.Value;
        }

        private void fieldWave_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.WaveColor.B = (byte)num.Value;
        }

        private void fieldOcean_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.OceanColor.R = (byte)num.Value;
        }

        private void fieldOcean_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.OceanColor.G = (byte)num.Value;
        }

        private void fieldOcean_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.OceanColor.B = (byte)num.Value;
        }

        private void fieldWhite1_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor1.R = (byte)num.Value;
        }

        private void fieldWhite1_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor1.G = (byte)num.Value;
        }

        private void fieldWhite1_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor1.B = (byte)num.Value;
        }

        private void fieldWhite2_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor2.R = (byte)num.Value;
        }

        private void fieldWhite2_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor2.G = (byte)num.Value;
        }

        private void fieldWhite2_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor2.B = (byte)num.Value;
        }

        private void fieldDoor_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.DoorwayColor.R = (byte)num.Value;
        }

        private void fieldDoor_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.DoorwayColor.G = (byte)num.Value;
        }

        private void fieldDoor_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.DoorwayColor.B = (byte)num.Value;
        }

        private void fieldUnknown3_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor3.R = (byte)num.Value;

        }

        private void fieldUnknown3_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor3.G = (byte)num.Value;
        }

        private void fieldUnknown3_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.UnknownColor3.B = (byte)num.Value;
        }

        private void fieldFog_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.FogColor.R = (byte)num.Value;
        }

        private void fieldFog_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.FogColor.G = (byte)num.Value;
        }

        private void fieldFog_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.FogColor.B = (byte)num.Value;
        }

        private void fieldVirt_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.VirtIndex = (byte) fieldVirt.Value;
        }

        private void fieldPadding1_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding1 = (byte) fieldPadding1.Value;
        }

        private void fieldPadding2_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.Padding2 = (byte) fieldPadding2.Value;
        }

        private void fieldOceanFade_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.OceanFadeInto.R = (byte)num.Value;
        }

        private void fieldOceanFade_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.OceanFadeInto.G = (byte)num.Value;
        }

        private void fieldOceanFade_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.OceanFadeInto.B = (byte)num.Value;
        }

        private void fieldOceanFadeA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.OceanFadeInto.A = (byte) fieldOceanFadeA.Value;
        }

        private void fieldShoreFade_XValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ShoreFadeInto.R = (byte)num.Value;
        }

        private void fieldShoreFade_YValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ShoreFadeInto.G = (byte)num.Value;
        }

        private void fieldShoreFade_ZValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            _curChunk.ShoreFadeInto.B = (byte)num.Value;
        }

        private void fieldShoreFadeA_ValueChanged(object sender, EventArgs e)
        {
            _curChunk.ShoreFadeInto.A = (byte) fieldShoreFadeA.Value;
        }
    }
}
