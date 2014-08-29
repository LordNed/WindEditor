using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindViewer;
using System.IO;

namespace WindViewer.Editor
{
    public partial class SongEditor : Form
    {
        public byte[] data;
        public int offset = 3769844;
        public song[] songArray = new song[8];

        public struct song
        {
            public byte numNotes;
            public byte note1;
            public byte note2;
            public byte note3;
            public byte note4;
            public byte note5;
            public byte note6;
  
        }

        public SongEditor()
        {
            InitializeComponent();
        }

        public void load(byte[] tempData)
        {
            data = tempData;

            parse();

            fillSongSelector();
        }

        private void parse()
        {
            int copyOffset = offset;

            for (int i = 0; i <= 7; i++)
            {
                song song = new song();

                song.numNotes = Helpers.Read8(data, copyOffset);
                song.note1 = Helpers.Read8(data, copyOffset + 1);
                song.note2 = Helpers.Read8(data, copyOffset + 2);
                song.note3 = Helpers.Read8(data, copyOffset + 3);
                song.note4 = Helpers.Read8(data, copyOffset + 4);
                song.note5 = Helpers.Read8(data, copyOffset + 5);
                song.note6 = Helpers.Read8(data, copyOffset + 6);


                if (song.note4 == 255)
                {
                    song.note4 = 5;
                }


                if (song.note5 == 255)
                {
                    song.note5 = 5;
                }


                if (song.note6 == 255)
                {
                    song.note6 = 5;
                }

                songArray[i] = song;

                copyOffset += 7;
            }

            return;
        }

        private void fillSongSelector()
        {
            songSelector.Enabled = true;

            int songIndex = 0;
            foreach (song song in songArray)
            {
                songSelector.Items.Add(songIndex);

                songIndex += 1;
            }

            songSelector.SelectedIndex = 0;

            fillFields();

        }

        private void songSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillFields();
        }

        private void fillFields()
        {
            noteCountSelector.Enabled = true;

            note1Direc.Enabled = true;
            note2Direc.Enabled = true;
            note3Direc.Enabled = true;
            note4Direc.Enabled = true;
            note5Direc.Enabled = true;
            note6Direc.Enabled = true;

            if (songArray[songSelector.SelectedIndex].numNotes == 3)
            {
                noteCountSelector.SelectedIndex = 0;

                note4Direc.Enabled = false;
                note5Direc.Enabled = false;
                note6Direc.Enabled = false;

            }
            else if (songArray[songSelector.SelectedIndex].numNotes == 4)
            {
                noteCountSelector.SelectedIndex = 1;

                note5Direc.Enabled = false;
                note6Direc.Enabled = false;
            }
            else if (songArray[songSelector.SelectedIndex].numNotes == 6)
            {
                noteCountSelector.SelectedIndex = 2;
            }

            note1Direc.SelectedIndex = songArray[songSelector.SelectedIndex].note1;
            note2Direc.SelectedIndex = songArray[songSelector.SelectedIndex].note2;
            note3Direc.SelectedIndex = songArray[songSelector.SelectedIndex].note3;
            note4Direc.SelectedIndex = songArray[songSelector.SelectedIndex].note4;
            note5Direc.SelectedIndex = songArray[songSelector.SelectedIndex].note5;
            note6Direc.SelectedIndex = songArray[songSelector.SelectedIndex].note6;

            fillImageBoxes();
        }

        private void fillImageBoxes()
        {
            #region Note 1
            if (songArray[songSelector.SelectedIndex].note1 == 0)
            {
                note1Box.Image = WindViewer.Properties.
                    Resources.WWMiddleNew;
            }

            if (songArray[songSelector.SelectedIndex].note1 == 1)
            {
                note1Box.Image = Properties.Resources.WWup;
            }

            if (songArray[songSelector.SelectedIndex].note1 == 2)
            {
                note1Box.Image = Properties.Resources.WWright;
            }

            if (songArray[songSelector.SelectedIndex].note1 == 3)
            {
                note1Box.Image = Properties.Resources.WWdown;
            }
            
            if (songArray[songSelector.SelectedIndex].note1 == 4)
            {
                note1Box.Image = Properties.Resources.WWleft;
            }

            #endregion

            #region Note 2
            if (songArray[songSelector.SelectedIndex].note2 == 0)
            {
                note2Box.Image = WindViewer.Properties.
                    Resources.WWMiddleNew;
            }

            if (songArray[songSelector.SelectedIndex].note2 == 1)
            {
                note2Box.Image = Properties.Resources.WWup;
            }

            if (songArray[songSelector.SelectedIndex].note2 == 2)
            {
                note2Box.Image = Properties.Resources.WWright;
            }

            if (songArray[songSelector.SelectedIndex].note2 == 3)
            {
                note2Box.Image = Properties.Resources.WWdown;
            }

            if (songArray[songSelector.SelectedIndex].note2 == 4)
            {
                note2Box.Image = Properties.Resources.WWleft;
            }

            #endregion

            #region Note 3
            if (songArray[songSelector.SelectedIndex].note3 == 0)
            {
                note3Box.Image = WindViewer.Properties.
                    Resources.WWMiddleNew;
            }

            if (songArray[songSelector.SelectedIndex].note3 == 1)
            {
                note3Box.Image = Properties.Resources.WWup;
            }

            if (songArray[songSelector.SelectedIndex].note3 == 2)
            {
                note3Box.Image = Properties.Resources.WWright;
            }

            if (songArray[songSelector.SelectedIndex].note3 == 3)
            {
                note3Box.Image = Properties.Resources.WWdown;
            }

            if (songArray[songSelector.SelectedIndex].note3 == 4)
            {
                note3Box.Image = Properties.Resources.WWleft;
            }

            #endregion

            #region Note 4
            if (songArray[songSelector.SelectedIndex].note4 == 0)
            {
                note4Box.Image = WindViewer.Properties.
                    Resources.WWMiddleNew;
            }

            if (songArray[songSelector.SelectedIndex].note4 == 1)
            {
                note4Box.Image = Properties.Resources.WWup;
            }

            if (songArray[songSelector.SelectedIndex].note4 == 2)
            {
                note4Box.Image = Properties.Resources.WWright;
            }

            if (songArray[songSelector.SelectedIndex].note4 == 3)
            {
                note4Box.Image = Properties.Resources.WWdown;
            }

            if (songArray[songSelector.SelectedIndex].note4 == 4)
            {
                note4Box.Image = Properties.Resources.WWleft;
            }

            #endregion

            #region Note 5
            if (songArray[songSelector.SelectedIndex].note5 == 0)
            {
                note5Box.Image = WindViewer.Properties.
                    Resources.WWMiddleNew;
            }

            if (songArray[songSelector.SelectedIndex].note5 == 1)
            {
                note5Box.Image = Properties.Resources.WWup;
            }

            if (songArray[songSelector.SelectedIndex].note5 == 2)
            {
                note5Box.Image = Properties.Resources.WWright;
            }

            if (songArray[songSelector.SelectedIndex].note5 == 3)
            {
                note5Box.Image = Properties.Resources.WWdown;
            }

            if (songArray[songSelector.SelectedIndex].note5 == 4)
            {
                note5Box.Image = Properties.Resources.WWleft;
            }

            #endregion

            #region Note 6
            if (songArray[songSelector.SelectedIndex].note6 == 0)
            {
                note6Box.Image = WindViewer.Properties.
                    Resources.WWMiddleNew;
            }

            if (songArray[songSelector.SelectedIndex].note6 == 1)
            {
                note6Box.Image = Properties.Resources.WWup;
            }

            if (songArray[songSelector.SelectedIndex].note6 == 2)
            {
                note6Box.Image = Properties.Resources.WWright;
            }

            if (songArray[songSelector.SelectedIndex].note6 == 3)
            {
                note6Box.Image = Properties.Resources.WWdown;
            }

            if (songArray[songSelector.SelectedIndex].note6 == 4)
            {
                note6Box.Image = Properties.Resources.WWleft;
            }

            #endregion
        }

        private void noteCountSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noteCountSelector.SelectedIndex == 0)
            {
                songArray[songSelector.SelectedIndex].numNotes = 3;

                songArray[songSelector.SelectedIndex].note4 = 5;
                songArray[songSelector.SelectedIndex].note5 = 5;
                songArray[songSelector.SelectedIndex].note6 = 5;
            }

            if (noteCountSelector.SelectedIndex == 1)
            {
                songArray[songSelector.SelectedIndex].numNotes = 4;

                songArray[songSelector.SelectedIndex].note5 = 5;
                songArray[songSelector.SelectedIndex].note6 = 5;
            }

            if (noteCountSelector.SelectedIndex == 2)
            {
                songArray[songSelector.SelectedIndex].numNotes = 6;
            }

            fillFields();
        }

        private void note1Direc_SelectedIndexChanged(object sender, EventArgs e)
        {
            songArray[songSelector.SelectedIndex].note1 = (byte)note1Direc.SelectedIndex;

            fillFields();
        }

        private void note2Direc_SelectedIndexChanged(object sender, EventArgs e)
        {
            songArray[songSelector.SelectedIndex].note2 = (byte)note2Direc.SelectedIndex;

            fillFields();
        }

        private void note3Direc_SelectedIndexChanged(object sender, EventArgs e)
        {
            songArray[songSelector.SelectedIndex].note3 = (byte)note3Direc.SelectedIndex;

            fillFields();
        }

        private void note4Direc_SelectedIndexChanged(object sender, EventArgs e)
        {
            songArray[songSelector.SelectedIndex].note4 = (byte)note4Direc.SelectedIndex;

            fillFields();
        }

        private void note5Direc_SelectedIndexChanged(object sender, EventArgs e)
        {
            songArray[songSelector.SelectedIndex].note5 = (byte)note5Direc.SelectedIndex;

            fillFields();

        }

        private void note6Direc_SelectedIndexChanged(object sender, EventArgs e)
        {
            songArray[songSelector.SelectedIndex].note6 = (byte)note6Direc.SelectedIndex;

            fillFields();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (data != null)
            {
                saveSongs();
            }
        }

        private void saveSongs()
        {
            byte[] songVals = new byte[7];

            int writeOffset = offset;

            BinaryWriter bw = new BinaryWriter(File.Open(Path.Combine(WindViewer.Properties.Settings.Default.rootDiskDir,
                "&&systemdata\\Start.dol"), FileMode.Open));

            foreach (song song in songArray)
            {
                songVals[0] = song.numNotes;
                songVals[1] = song.note1;
                songVals[2] = song.note2;
                songVals[3] = song.note3;
                songVals[4] = song.note4;
                songVals[5] = song.note5;
                songVals[6] = song.note6;

                if (songVals[4] == 5)
                {
                    songVals[4] = 255;
                }

                if (songVals[5] == 5)
                {
                    songVals[5] = 255;
                }

                if (songVals[6] == 5)
                {
                    songVals[6] = 255;
                }

                bw.Seek(writeOffset, 0);

                for (int m = 0; m <= 6; m++)
                {
                    bw.Write(songVals[m]);
                }

                writeOffset += 7;
            }

            bw.Close();
        }
    }
}
