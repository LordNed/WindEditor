using System;
using System.Linq;
using System.Windows.Forms;

namespace WindViewer.Forms
{
    public partial class FloatConverter : Form
    {
        public FloatConverter()
        {
            InitializeComponent();
        }

        private bool _supressEvents;

        private void floatValue_Changed(object sender, EventArgs e)
        {
            if(_supressEvents)
                return;

            float tryFloat;
            if (float.TryParse(floatValue.Text, out tryFloat))
            {
                //Convert it to Hexadecimal and Byteswap it.
                byte[] floatBytes = BitConverter.GetBytes(tryFloat);
                Array.Reverse(floatBytes);

                //Build our Hex string
                string hexString = BitConverter.ToString(floatBytes).Replace("-", " ");

                //Now update the Hex output
                _supressEvents = true;
                hexValue.Text = hexString;
                _supressEvents = false;

            }
            else
            {
                _supressEvents = true;
                hexValue.Text = string.Empty;
                _supressEvents = false;
            }
        }

        private void hexValue_Changed(object sender, EventArgs e)
        {
            if (_supressEvents)
                return;

            //Figure out if it's a hex string without spaces
            if (hexValue.Text.Length == 8)
            {
                //See if it contains spaces, if so it's /NOT/ a whole hex string.
                int spaceIndex = hexValue.Text.IndexOf(' ');
                if (spaceIndex == -1)
                {
                    //Hey they didn't have any spaces, guess it's a whole hex string (sans spaces)
                    HexToFloat(hexValue.Text);
                }
            }
            else if (hexValue.Text.Length == 11)
            {
                //Ensure there's actually spaces in this one (otherwise 11 digits of 0 works...)
                int spaceIndex = hexValue.Text.IndexOf(' ');
                if (spaceIndex != -1)
                {
                    string hexString = hexValue.Text.Replace(" ", "");
                    HexToFloat(hexString);
                }
            }
            else
            {
                _supressEvents = true;
                floatValue.Text = string.Empty;
                _supressEvents = false;
            }
        }

        /// <summary>
        /// Usage: Pass this a hex string without spaces (ie: "40000000") and it will
        /// update the float for you because it is nice.
        /// </summary>
        /// <param name="hexString"></param>
        private void HexToFloat(string hexString)
        {
            byte[] bytes = StringToByteArray(hexString);

            //Reverse it into little-endian
            Array.Reverse(bytes);

            //Then try and convert it to floats.
            float value = BitConverter.ToSingle(bytes, 0);

            _supressEvents = true;
            floatValue.Text = value.ToString();
            _supressEvents = false;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
