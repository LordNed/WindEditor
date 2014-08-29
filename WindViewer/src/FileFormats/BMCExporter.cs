using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindViewer.Editor
{
    class BMCExporter
    {
        BinaryWriter bwriter;

        string filePath;

        public void BmcExporter(List<ByteColorAlpha> colorList, string folderPath)
        {
            filePath = Path.Combine(folderPath, "color.bmc");

            bwriter = new BinaryWriter(File.Open(filePath, FileMode.Create));

            FSHelpers.WriteString(bwriter, "MGCLbmc1", 8);
            FSHelpers.Write32(bwriter, 34);
            FSHelpers.Write32(bwriter, 1);

            for (int i = 0; i < 4; i++)
            {
                FSHelpers.Write32(bwriter, 0);
            }

            FSHelpers.WriteString(bwriter, "CLT1", 4);
            FSHelpers.Write32(bwriter, 1056);
            FSHelpers.Write16(bwriter, 256);
            FSHelpers.Write16(bwriter, 0);

            foreach (ByteColorAlpha color in colorList)
            {
                FSHelpers.Write8(bwriter, color.R);
                FSHelpers.Write8(bwriter, color.G);
                FSHelpers.Write8(bwriter, color.B);
                FSHelpers.Write8(bwriter, color.A);
            }

            for (int i = 0; i < 5; i++)
            {
                FSHelpers.Write32(bwriter, 0);
            }

            bwriter.Close();
        }
    }
}
