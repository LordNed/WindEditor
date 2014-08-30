using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindViewer.Editor
{
    /// <summary>
    /// The BMC file holds color data for the text. A control code specifies the index of a color,
    /// and the engine changes the text's color to that one.
    /// </summary>
    class BMCParser
    {
        public List<WindViewer.Editor.ByteColorAlpha> colorList;

        int offset = 0;

        public List<WindViewer.Editor.ByteColorAlpha> BmcParser(byte[] data)
        {
            colorList = new List<Editor.ByteColorAlpha>();

            int numColors = Helpers.Read16(data, 40);

            offset = 44;

            for (int i = 0; i < numColors; i++)
            {
                Editor.ByteColorAlpha tempCol = new Editor.ByteColorAlpha();

                tempCol.R = Helpers.Read8(data, offset);
                tempCol.G = Helpers.Read8(data, offset + 1);
                tempCol.B = Helpers.Read8(data, offset + 2);
                tempCol.A = Helpers.Read8(data, offset + 3);

                colorList.Add(tempCol);

                offset += 4;
            }

            return colorList;
        }
    }
}
