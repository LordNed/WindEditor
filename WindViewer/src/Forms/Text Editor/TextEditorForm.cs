using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using WindViewer;
using WindViewer.Editor;

namespace WindViewer.Editor
{
    public partial class TextEditorForm : Form
    {
        public List<message> messageList;

        public List<message> exportList;

        public List<ByteColorAlpha> colorList;

        public int selectedMessage = 0;

        public int highestMessageID = 0;

        public bool isLoaded = false;

        public bool stringWasFound = false;

        public int cursorPos;

        public TextEditorForm()
        {
            InitializeComponent();
        }

        private void fillListBox()
        {
            listBox1.Enabled = true;

            listBox1.Items.Clear();

            foreach (message mess in messageList)
            {
                if (mess.stringOffset == 0)
                {
                    listBox1.Items.Add("Null Entry");
                }

                else
                {
                    listBox1.Items.Add(mess.messageID + ". " + new string(mess.stringData.ToArray()));
                }
            }
        }

        private void fillBoxes()
        {
            textBox.Enabled = true;

            typeCombo.Enabled = true;
            drawBox.Enabled = true;
            posCombo.Enabled = true;
            itemIdBox.Enabled = true;
            linesBox.Enabled = true;
            currentIndexBox.Enabled = true;

            clearBoxes();

            currentIndexBox.Text = selectedMessage.ToString();

            if (messageList[selectedMessage].stringOffset == 0)
            {
                textBox.Text = "Entry not used";

                textBox.Enabled = false;

                typeCombo.Enabled = false;
                drawBox.Enabled = false;
                posCombo.Enabled = false;
                itemIdBox.Enabled = false;
                linesBox.Enabled = false;
            }

            else
            {
                textBox.Text = new string(messageList[selectedMessage].stringData.ToArray());
                typeCombo.SelectedIndex = getMessageBoxType();
                drawBox.Text = messageList[selectedMessage].charDrawType.ToString();
                posCombo.SelectedIndex = getMessageBoxPosition();
                itemIdBox.Text = messageList[selectedMessage].itemImage.ToString();
                linesBox.Text = messageList[selectedMessage].linesPerTextBox.ToString();
            }
        }

        private int getMessageBoxType()
        {
            int msgType = 0;

            switch (messageList[selectedMessage].textBoxType)
            {
                case 0:
                case 3:
                case 4:
                case 8:
                case 10:
                    msgType = 0;
                    break;
                case 1:
                    msgType = 1;
                    break;
                case 2:
                    msgType = 2;
                    break;
                case 5:
                    msgType = 3;
                    break;
                case 6:
                    msgType = 4;
                    break;
                case 7:
                    msgType = 5;
                    break;
                case 9:
                    msgType = 6;
                    break;
            }

            return msgType;
        }

        private int getMessageBoxPosition()
        {
            int pos = 0;

            switch (messageList[selectedMessage].boxPosition)
            {
                case 0:
                case 1:
                    pos = 0;
                    break;
                case 2:
                    pos = 1;
                    break;
                case 3:
                case 4:
                    pos = 2;
                    break;
            }

            return pos;
        }

        private void clearBoxes()
        {
            textBox.Clear();

            drawBox.Clear();

            itemIdBox.Clear();
            linesBox.Clear();
            currentIndexBox.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMessage = 0;

            selectedMessage = listBox1.SelectedIndex;

            fillBoxes();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            List<char> tempCharList = new List<char>(textBox.Text.ToArray());

            messageList[selectedMessage].stringData = new List<char>(tempCharList);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isLoaded == true)
            {
                highestMessageID = getHighestID();

                message newMessage = new message();

                newMessage.stringOffset = -559038737;
                newMessage.messageID = (short)(highestMessageID + 1);
                newMessage.padding1 = 0;
                newMessage.messageType = 96;
                newMessage.textBoxType = 0;
                newMessage.charDrawType = 0;
                newMessage.boxPosition = 0;
                newMessage.itemImage = 255;
                newMessage.unknown1 = 0;
                newMessage.groupID1 = 0;
                newMessage.groupID2 = 0;
                newMessage.groupID3 = 0;
                newMessage.padding2 = 0;
                newMessage.linesPerTextBox = 4;
                newMessage.padding3 = 0;

                string temp = "";

                newMessage.stringData = new List<char>(temp.ToArray());

                messageList.Add(newMessage);

                fillListBox();
            }
        }

        private int getHighestID()
        {
            int curId;
            int highID = 0;

            for (int i = 0; i < messageList.Count; i++)
            {
                curId = messageList[i].messageID;

                if (highID < curId)
                {
                    highID = curId;
                }
            }

            return highID;
        }

        private void typeBox_Leave(object sender, EventArgs e)
        {
            //messageList[selectedMessage].textBoxType = Convert.ToByte(typeBox.Text);
        }

        private void drawBox_Leave(object sender, EventArgs e)
        {
            messageList[selectedMessage].charDrawType = Convert.ToByte(drawBox.Text);
        }

        private void posBox_Leave(object sender, EventArgs e)
        {
            //messageList[selectedMessage].boxPosition = Convert.ToByte(posBox.Text);
        }

        private void itemIdBox_Leave(object sender, EventArgs e)
        {
            messageList[selectedMessage].itemImage = Convert.ToByte(itemIdBox.Text);
        }

        private void linesBox_Leave(object sender, EventArgs e)
        {
            messageList[selectedMessage].linesPerTextBox = Convert.ToByte(linesBox.Text);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportBmgRes();
        }

        private void exportBmgRes()
        {
            FolderSelect.FolderSelectDialog folderDialog = new FolderSelect.FolderSelectDialog();

                if (folderDialog.ShowDialog())
                {
                    string workingDir = Path.Combine(folderDialog.FileName, "bmgres");

                    string bmgName = Path.Combine(workingDir, "zel_00.bmg");

                    Directory.CreateDirectory(workingDir);

                    BMGExporter export = new BMGExporter();
                    
                    exportList = new List<message>();

                    //We need to make sure that we're encoding the tags for a copy of the main message list.
                    //So, since there is no other easy fucking way to do this, we'll just create new message
                    //classes and fill the fields ourselves

                    foreach (message mes in messageList)
                    {
                        message newMessage = new message();

                        newMessage.stringOffset = mes.stringOffset;
                        newMessage.messageID = mes.messageID;
                        newMessage.padding1 = mes.padding1;
                        newMessage.messageType = mes.messageType;
                        newMessage.textBoxType = mes.textBoxType;
                        newMessage.charDrawType = mes.charDrawType;
                        newMessage.boxPosition = mes.boxPosition;
                        newMessage.itemImage = mes.itemImage;
                        newMessage.unknown1 = mes.unknown1;
                        newMessage.groupID1 = mes.groupID1;
                        newMessage.groupID2 = mes.groupID2;
                        newMessage.groupID3 = mes.groupID3;
                        newMessage.padding2 = mes.padding2;
                        newMessage.linesPerTextBox = mes.linesPerTextBox;
                        newMessage.padding3 = mes.padding3;

                        newMessage.stringData = mes.stringData;

                        newMessage.originalStringDataLength = mes.originalStringDataLength;

                        exportList.Add(newMessage);
                    }

                    export.export(exportList, bmgName);

                    BMCExporter bmcExport = new BMCExporter();

                    bmcExport.BmcExporter(colorList, workingDir);
                }
        }

        /*private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                    BMGExporter export = new BMGExporter();

                    exportList = new List<message>();

                    //We need to make sure that we're encoding the tags for a copy of the main message list.
                    //So, since there is no other easy fucking way to do this, we'll just create new message
                    //classes and fill the fields ourselves

                    foreach (message mes in messageList)
                    {
                        message newMessage = new message();

                        newMessage.stringOffset = mes.stringOffset;
                        newMessage.messageID = mes.messageID;
                        newMessage.padding1 = mes.padding1;
                        newMessage.messageType = mes.messageType;
                        newMessage.textBoxType = mes.textBoxType;
                        newMessage.charDrawType = mes.charDrawType;
                        newMessage.boxPosition = mes.boxPosition;
                        newMessage.itemImage = mes.itemImage;
                        newMessage.unknown1 = mes.unknown1;
                        newMessage.groupID1 = mes.groupID1;
                        newMessage.groupID2 = mes.groupID2;
                        newMessage.groupID3 = mes.groupID3;
                        newMessage.padding2 = mes.padding2;
                        newMessage.linesPerTextBox = mes.linesPerTextBox;
                        newMessage.padding3 = mes.padding3;

                        newMessage.stringData = mes.stringData;

                        newMessage.originalStringDataLength = mes.originalStringDataLength;

                        exportList.Add(newMessage);
                }

                    export.export(exportList, openFileDialog1.FileName);

                    MessageBox.Show("Save complete.");
            }
        }*/

        private void searchByIDBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (searchByIDBox.Text != "")
                {
                    foreach (char chara in searchByIDBox.Text)
                    {
                        if (char.IsLetter(chara) == true)
                        {
                            MessageBox.Show("Search ID must only include numbers.");
                            return;
                        }

                        if (char.IsSymbol(chara) == true)
                        {
                            MessageBox.Show("Search ID must only include numbers.");
                            return;
                        }

                        if (char.IsPunctuation(chara) == true)
                        {
                            MessageBox.Show("Search ID must only include numbers.");
                            return;
                        }

                        if (char.IsSeparator(chara) == true)
                        {
                            MessageBox.Show("Search ID must only include numbers.");
                            return;
                        }
                    }

                    int searchID = Convert.ToInt32(searchByIDBox.Text);
                    search(searchID);
                }
            }
        }

        private void search(int searchID)
        {
            int foundMessageID = 0;

            for (int i = 0; i < messageList.Count; i++)
            {
                if (messageList[i].messageID == searchID)
                {
                    foundMessageID = i;

                    break;
                }

                if (i == messageList.Count - 1)
                {
                    if (messageList[i].messageID != searchID)
                    {
                        MessageBox.Show("ID not found.");

                        return;
                    }

                    if (messageList[i].messageID == searchID)
                    {
                        foundMessageID = i;

                        break;
                    }
                }

                continue;
            }

            listBox1.SelectedIndex = foundMessageID;
        }

        private void label7_MouseHover(object sender, EventArgs e)
        {
            ToolTip tool = new ToolTip();

            tool.SetToolTip(searchByIDLabel, "Search by message ID");
        }

        private void searchByTextLabel_MouseHover(object sender, EventArgs e)
        {
            ToolTip tool = new ToolTip();

            tool.SetToolTip(searchByTextLabel, "Search by string text");
        }

        private void currentIndexBox_KeyDown(object sender, KeyEventArgs e)
        {
            int newIndex = 0;

            if (e.KeyCode == Keys.Return)
            {
                if (currentIndexBox.Text != "")
                {
                    foreach (char chara in currentIndexBox.Text)
                    {
                        if (char.IsLetter(chara) == true)
                        {
                            MessageBox.Show("Desired index must only include numbers.");
                            return;
                        }

                        if (char.IsSymbol(chara) == true)
                        {
                            MessageBox.Show("Desired index must only include numbers.");
                            return;
                        }

                        if (char.IsPunctuation(chara) == true)
                        {
                            MessageBox.Show("Desired index must only include numbers.");
                            return;
                        }

                        if (char.IsSeparator(chara) == true)
                        {
                            MessageBox.Show("Desired index must only include numbers.");
                            return;
                        }
                    }

                    newIndex = Convert.ToInt32(currentIndexBox.Text);

                    if (newIndex >= messageList.Count)
                    {
                        MessageBox.Show("Desired index was out of range.");
                        return;
                    }

                    listBox1.SelectedIndex = newIndex;

                    currentIndexBox.Select(currentIndexBox.Text.Length, 0);
                }
            }
        }

        private void searchByTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool cont = true;

            if (e.KeyCode == Keys.Return)
            {
                if (searchByTextBox.Text != "")
                {
                    searchByText(listBox1.SelectedIndex);

                    while (cont == true)
                    {
                        if (stringWasFound == true)
                        {
                            break;
                        }

                        else
                        {
                            DialogResult result = MessageBox.Show("String not found. Restart search from beginning?", "", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                searchByText(-1);
                            }

                            else
                            {
                                cont = false;
                            }
                        }
                    }
                }
            }
        }

        private void searchByText(int startIndex)
        {
            string searchString = searchByTextBox.Text;

            stringWasFound = false;

            for (int i = startIndex + 1; i < messageList.Count - 1; i++)
            {
                if (messageList[i].stringOffset == 0)
                {
                    continue;
                }

                string sourceData = new string(messageList[i].stringData.ToArray());

                bool contains = Regex.Match(sourceData, searchString, RegexOptions.IgnoreCase).Success;

                if (contains == true)
                {
                    listBox1.SelectedIndex = i;

                    stringWasFound = true;

                    break;
                }
            }
        }

        private void TextEditorForm_Load(object sender, EventArgs e)
        {
            isLoaded = true;
        }

        public void loadBmgRes(GameFormatReader.GCWii.Binaries.GC.RARC bmgres)
        {
            foreach (GameFormatReader.GCWii.Binaries.GC.RARC.FileEntry entry in bmgres.Nodes[0].Entries)
            {
                if (entry.Name.EndsWith(".bmg"))
                {
                    MessageParser msgParse = new MessageParser();
                    
                    messageList = msgParse.parser(entry.Data);

                    fillListBox();

                    listBox1.SelectedIndex = 0;
                }

                if (entry.Name.EndsWith(".bmc"))
                {
                    BMCParser colParse = new BMCParser();

                    colorList = colParse.BmcParser(entry.Data);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            messageList = null;
            listBox1.Items.Clear();

            isLoaded = false;

            textBox.Clear();

            drawBox.Clear();

            itemIdBox.Clear();
            linesBox.Clear();
            searchByIDBox.Clear();
            searchByTextBox.Clear();
            currentIndexBox.Clear();

            searchByIDBox.Clear();
            searchByTextBox.Clear();

        }

        private void textColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextColorEditorForm textColorEditor = new TextColorEditorForm();

            textColorsToolStripMenuItem.Enabled = false;

            textColorEditor.loadColorEditor(colorList);

            textColorEditor.Show();

            textColorEditor.Disposed += new EventHandler(textColorEditor_Disposed);
        }

        void textColorEditor_Disposed(object sender, EventArgs e)
        {
            textColorsToolStripMenuItem.Enabled = true;
        }

        private void typeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateMessageBoxType();
        }

        private void updateMessageBoxType()
        {
            switch (typeCombo.SelectedIndex)
            {
                case 0:
                    messageList[selectedMessage].textBoxType = 0;
                    break;
                case 1:
                    messageList[selectedMessage].textBoxType = 1;
                    break;
                case 2:
                    messageList[selectedMessage].textBoxType = 2;
                    break;
                case 3:
                    messageList[selectedMessage].textBoxType = 5;
                    break;
                case 4:
                    messageList[selectedMessage].textBoxType = 6;
                    break;
                case 5:
                    messageList[selectedMessage].textBoxType = 7;
                    break;
                case 6:
                    messageList[selectedMessage].textBoxType = 9;
                    break;

            }
        }

        private void posCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateBoxPosition();
        }

        private void updateBoxPosition()
        {
            switch (posCombo.SelectedIndex)
            {
                case 0:
                    messageList[selectedMessage].boxPosition = 0;
                    break;
                case 1:
                    messageList[selectedMessage].boxPosition = 2;
                    break;
                case 2:
                    messageList[selectedMessage].boxPosition = 3;
                    break;
            }
        }

        private void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                RichTextBox box = (RichTextBox)sender;
                box.SelectionStart = box.GetCharIndexFromPosition(e.Location);
                box.SelectionLength = 0;

                cursorPos = textBox.SelectionStart;
            }
        }

        private void insertControlTag(string type, bool variable, int value)
        {
            string tempControlTag = "";

            if (variable)
            {
                if (type == "draw")
                {
                    if (value == 0)
                        tempControlTag = "<" + type + ":" + "instant" + ">";
                    if (value == 1)
                        tempControlTag = "<" + type + ":" + "char" + ">";
                }

                if (type == "control stick")
                {
                    if (value == 0)
                        tempControlTag = "<" + type + ":" + "up" + ">";
                    if (value == 1)
                        tempControlTag = "<" + type + ":" + "down" + ">";
                    if (value == 2)
                        tempControlTag = "<" + type + ":" + "left" + ">";
                    if (value == 3)
                        tempControlTag = "<" + type + ":" + "right" + ">";
                    if (value == 4)
                        tempControlTag = "<" + type + ":" + "up + down" + ">";
                    if (value == 5)
                        tempControlTag = "<" + type + ":" + "left + right" + ">";
                }
                
                if (type == "arrow")
                {
                    if (value == 0)
                            tempControlTag = "<" + type + ":" + "up" + ">";
                    if (value == 1)
                            tempControlTag = "<" + type + ":" + "down" + ">";
                    if (value == 2)
                            tempControlTag = "<" + type + ":" + "left" + ">";
                    if (value == 3)
                            tempControlTag = "<" + type + ":" + "right" + ">";
                }

                else
                {
                    tempControlTag = "<" + type + ":" + Convert.ToString(value) + ">";
                }
            }

            else
            {
                tempControlTag = "<" + type + ">";
            }

            textBox.Text = textBox.Text.Insert(cursorPos, tempControlTag);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 0);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 1);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 2);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 3);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 4);
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 5);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 6);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 7);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            insertControlTag("color", true, 8);
        }

        private void animToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("anim", true, 0);
        }

        private void soundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("sound", true, 0);
        }

        private void waitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            insertControlTag("wait", true, 0);
        }

        private void waitAndDismissToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("wait + dismiss", true, 0);
        }

        private void instantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("draw", true, 0);
        }

        private void byCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("draw", true, 1);
        }

        private void heartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("heart", false, 0);
        }

        private void playerNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("player", false, 0);
        }

        private void aButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("A button", false, 0);
        }

        private void bButtonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            insertControlTag("B button", false, 0);
        }

        private void xButtonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            insertControlTag("X button", false, 0);
        }

        private void yButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("Y button", false, 0);
        }

        private void zButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("Z button", false, 0);
        }

        private void dPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("D-pad", false, 0);
        }

        private void leftTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("L trigger", false, 0);
        }

        private void rightTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("R trigger", false, 0);
        }

        private void cStickToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            insertControlTag("C-stick", false, 0);
        }

        private void staticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("control stick", false, 0);
        }

        private void pointingUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("control stick", true, 0);
        }

        private void pointingDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("control stick", true, 1);
        }

        private void pointingLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("control stick", true, 2);
        }

        private void pointingRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("control stick", true, 3);
        }

        private void pointingUpDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("control stick", true, 4);
        }

        private void pointingLeftRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("control stick", true, 5);
        }

        private void upArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("arrow", true, 0);
        }

        private void downArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("arrow", true, 1);
        }

        private void leftArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("arrow", true, 2);
        }

        private void rightArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("arrow", true, 3);
        }

        private void hookshotStarburstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("target starburst", false, 0);
        }

        private void starburstAButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertControlTag("starburst A button", false, 0);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (messageList != null)
            {
                List<char> tempCharList = new List<char>(textBox.Text.ToArray());

                messageList[selectedMessage].stringData = new List<char>(tempCharList);
            }
        }
    }
}
