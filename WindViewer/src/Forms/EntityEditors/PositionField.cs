using System;
using System.ComponentModel;
using System.Windows.Forms;
using OpenTK;

namespace WindViewer.Forms.EntityEditors
{
    public partial class PositionField : UserControl
    {
        [EditorBrowsable(EditorBrowsableState.Always)] [Browsable(true)] [Category("Custom")]
        public event EventHandler XValueChanged;
        [EditorBrowsable(EditorBrowsableState.Always)] [Browsable(true)] [Category("Custom")]
        public event EventHandler YValueChanged;
        [EditorBrowsable(EditorBrowsableState.Always)] [Browsable(true)] [Category("Custom")]
        public event EventHandler ZValueChanged;

        private float _minimum, _maximum;

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Custom")]
        [Browsable(true)]
        public float Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                fieldX.Minimum = (decimal)_minimum;
                fieldY.Minimum = (decimal)_minimum;
                fieldZ.Minimum = (decimal)_minimum;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Custom")]
        [Browsable(true)]
        public float Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                fieldX.Maximum = (decimal)_maximum;
                fieldY.Maximum = (decimal)_maximum;
                fieldZ.Maximum = (decimal)_maximum;
            }
        }

        public PositionField()
        {
            InitializeComponent();
        }

        public void SetValue(Vector3 position)
        {
            fieldX.Value = (decimal)position.X;
            fieldY.Value = (decimal)position.Y;
            fieldZ.Value = (decimal)position.Z;
        }

        public void SetXValue(float xValue)
        {
            fieldX.Value = (decimal)xValue;
        }

        public void SetYValue(float yValue)
        {
            fieldY.Value = (decimal)yValue;
        }

        public void SetZValue(float zValue)
        {
            fieldZ.Value = (decimal)zValue;
        }

        private void fieldX_ValueChanged(object sender, EventArgs e)
        {
            if (XValueChanged != null)
                XValueChanged(sender, e);
        }

        private void fieldY_ValueChanged(object sender, EventArgs e)
        {
            if (YValueChanged != null)
                YValueChanged(sender, e);
        }

        private void fieldZ_ValueChanged(object sender, EventArgs e)
        {
            if (ZValueChanged != null)
                ZValueChanged(sender, e);
        }
    }
}
