using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindViewer.Editor
{
    public partial class TextColorEditorForm : Form
    {
        List<Editor.ByteColorAlpha> colorList = new List<Editor.ByteColorAlpha>();

        int selectedColor = 0;

        Color textColor;

        public TextColorEditorForm()
        {
            InitializeComponent();
        }

        private void colorBox_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                colorList[selectedColor].R = colorDialog1.Color.R;
                colorList[selectedColor].G = colorDialog1.Color.G;
                colorList[selectedColor].B = colorDialog1.Color.B;

                updateColors();
            }
        }

        public void loadColorEditor(List<Editor.ByteColorAlpha> colList)
        {
            colorList = colList;

            for (int i = 0; i < colorList.Count; i++)
            {
                colorIndexPicker.Items.Add(i);
            }

            colorIndexPicker.SelectedIndex = 0;
        }

        private void changeTextColor()
        {
            if (selectedColor >= 256)
            {
                MessageBox.Show("There are only 256 color slots in the file.");

                colorIndexPicker.SelectedIndex = 0;
            }

            if (selectedColor == -1)
            {
                selectedColor = Convert.ToInt32(colorIndexPicker.Text);
            }

            textColor = Color.FromArgb(colorList[selectedColor].A, colorList[selectedColor].R,
                colorList[selectedColor].G, colorList[selectedColor].B);
        }

        private void changeColorBoxColor()
        {
            colorBox.BackColor = textColor;
        }

        private void changeTextBoxTextColor()
        {
            textTestBox.ForeColor = textColor;
        }

        private void colorIndexPicker_SelectedItemChanged(object sender, EventArgs e)
        {
            selectedColor = colorIndexPicker.SelectedIndex;

            updateColors();
        }

        private void updateColors()
        {
            changeTextColor();

            changeColorBoxColor();

            changeTextBoxTextColor();
        }
    }
}
