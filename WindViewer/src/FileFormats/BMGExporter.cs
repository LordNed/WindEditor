using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WindViewer.Editor;

namespace WindViewer.Editor
{
    class BMGExporter
    {
        public int fileSize = 0;
        public int dat1Size = 0;
        public short numMessages = 0;

        public void export(List<message> messageList, string fileName)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create));

            List<message> exportMessageList = new List<message>(messageList);

            //Write BMG header
            FSHelpers.WriteString(bw, "MESGbmg1", 8);
            FSHelpers.Write32(bw, fileSize);
            FSHelpers.Write32(bw, 2);
            FSHelpers.Write32(bw, 16777216);
            FSHelpers.Write32(bw, 0);
            FSHelpers.Write32(bw, 0);
            FSHelpers.Write32(bw, 0);

            //Write INF1 header
            FSHelpers.WriteString(bw, "INF1", 4);
            FSHelpers.Write32(bw, (exportMessageList.Count * 24) + 8 + 16);
            FSHelpers.Write16(bw, (ushort)exportMessageList.Count);
            FSHelpers.Write16(bw, 24);
            FSHelpers.Write16(bw, 0); //This is a field that's 0 for the English text bank
            FSHelpers.Write16(bw, 0);

            foreach (message mess in exportMessageList)
            {
                if (mess.stringOffset == 0)
                {
                    if (mess.messageID == 0)
                    {
                        continue;
                    }
                }

                else
                {
                    if (mess.messageID == 3036)
                    {
                    }
                    mess.stringData = convertTagsToControlCodes(mess.stringData);
                }
            }

            for (int i = 0; i < exportMessageList.Count; i++)
            {
                if (exportMessageList[i].stringOffset == 0)
                {
                        continue;
                }

                if (exportMessageList[i].stringOffset == -559038737)
                {
                    exportMessageList[i].stringOffset = exportMessageList[i - 1].stringOffset + exportMessageList[i - 1].stringData.Count;

                    continue;
                }

                if (exportMessageList[i].originalStringDataLength != exportMessageList[i].stringData.Count)
                {
                    int lengthDifference = exportMessageList[i].stringData.Count - exportMessageList[i].originalStringDataLength;

                    for (int k = 1; k < exportMessageList.Count - (i + 1); k++)
                    {
                        if (exportMessageList[i + k].stringOffset == 0)
                        {
                            continue;
                        }

                        exportMessageList[i + k].stringOffset += lengthDifference;
                    }
                }
            }

            foreach (message mes in exportMessageList)
            {
                mes.save(bw);
            }

            //Original file has 8 bytes of padding at the end, so what the hell, let's write it in too
            FSHelpers.Write32(bw, 0);
            FSHelpers.Write32(bw, 0);

            //Build DAT1 header
            FSHelpers.WriteString(bw, "DAT1", 4);

            foreach (message mes in exportMessageList)
            {
                if (mes.stringOffset == 0)
                {
                    continue;
                }

                dat1Size += mes.stringData.Count;
            }

            dat1Size += 9;

            FSHelpers.Write32(bw, dat1Size);
            FSHelpers.Write8(bw, 0);

            foreach (message mes in exportMessageList)
            {
                if (mes.stringOffset == 0)
                {
                    continue;
                }

                for (int b = 0; b < mes.stringData.Count; b++)
                {
                    FSHelpers.Write8(bw, (byte)mes.stringData[b]);
                }
            }

            fileSize = (int)bw.BaseStream.Position;

            bw.BaseStream.Position = 8;

            FSHelpers.Write32(bw, fileSize);

            bw.Flush();
            bw.Close();
        }

        public List<char> convertTagsToControlCodes(List<char> data)
        {
            List<char> tagBuffer = new List<char>();
            List<char> codeBuffer = new List<char>();
            List<char> editData = new List<char>(data);

            int offset = 0;

            for (int i = 0; i < data.Count; i++)
            {
                tagBuffer.Clear();
                codeBuffer.Clear();

                if (data[i] == 60) //We've hit a "<" character, which means it's a control tag
                {
                    int k = 0;

                    while (data[i + k] != 62) //Grab the control tag
                    {
                        tagBuffer.Add(data[i + k]);
                        k += 1;
                    }

                    tagBuffer.Add((char)62); //Above loop doesn't add ">" char, so here's a simple way to correct that

                    string tagString = new string(tagBuffer.ToArray()); //Now we have a string to compare things to!

                    string tagPartial = tagString.Substring(1, 5);

                    #region Non-Variable Tags
                    if (tagString == "<player>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                                                                                     //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<draw:instant>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)1);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<draw:char>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)2);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<heart>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)57);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<2 choices>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)8);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<3 choices>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)9);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<A button>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)10);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<B button>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)11);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<C-stick>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)12);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<L trigger>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)13);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<R trigger>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)14);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<X button>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)15);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<Y button>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)16);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<Z button>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)17);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<D-pad>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)18);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<control stick>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)19);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<arrow:left>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)20);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<arrow:right>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)21);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<arrow:up>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)22);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<arrow:down>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)23);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<control stick:up>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)24);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<control stick:down>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)25);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<control stick:left>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)26);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<control stick:right>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)27);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<control stick:up + down>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)28);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<control stick:left + right>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)29);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<choice:1>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)30);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<choice:2>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)31);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<rupee cost:red>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)33);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<rupee cost:white>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)34);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<item name>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)35);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<starburst A button>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)39);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<blow count>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)40);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<pirate ship password>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)41);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<target starburst>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)42);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<value>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)43);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<rupee total>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)44);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }

                    if (tagString == "<yard count>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)48);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }
#endregion

                    #region Color tags
                    if (tagString == "<color:0>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:1>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)1);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:2>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)2);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:3>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)3);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:4>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)4);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:5>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)5);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:6>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)6);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:7>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)7);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:8>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)8);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }

                    if (tagString == "<color:9>")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)6);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)9);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 6;
                    }
                    #endregion

                    #region Scale tag
                    if (tagPartial == "scale")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)7);
                        codeBuffer.Add((char)255);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)1);

                        string temp = tagString.Substring(7, tagString.Length - 7);

                        List<char> tempchar = new List<char>(temp);

                        tempchar.Remove((char)62);

                        string temp2 = new string(tempchar.ToArray());

                        short thingy = Convert.ToInt16(temp2);

                        byte[] thingyII = BitConverter.GetBytes(thingy);

                        Array.Reverse(thingyII);

                        codeBuffer.Add((char)thingyII[0]);
                        codeBuffer.Add((char)thingyII[1]);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 7;
                    }
                    #endregion

                    #region Wait tags
                    if (tagPartial == "wait:")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)7);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)7);

                        string temp = tagString.Substring(6, tagString.Length - 6);

                        List<char> tempchar = new List<char>(temp);

                        tempchar.Remove((char)62);

                        string temp2 = new string(tempchar.ToArray());

                        short thingy = Convert.ToInt16(temp2);

                        byte[] thingyII = BitConverter.GetBytes(thingy);

                        Array.Reverse(thingyII);

                        codeBuffer.Add((char)thingyII[0]);
                        codeBuffer.Add((char)thingyII[1]);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 7;
                    }

                    if (tagPartial == "wait ")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)7);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)0);
                        codeBuffer.Add((char)4);

                        string temp = tagString.Substring(16, tagString.Length - 16);

                        List<char> tempchar = new List<char>(temp);

                        tempchar.Remove((char)62);

                        string temp2 = new string(tempchar.ToArray());

                        short thingy = Convert.ToInt16(temp2);

                        byte[] thingyII = BitConverter.GetBytes(thingy);

                        Array.Reverse(thingyII);

                        codeBuffer.Add((char)thingyII[0]);
                        codeBuffer.Add((char)thingyII[1]);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 7;
                    }
                    #endregion

                    #region Sound tag
                    if (tagPartial == "sound")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)1);

                        string temp = tagString.Substring(7, tagString.Length - 7);

                        List<char> tempchar = new List<char>(temp);

                        tempchar.Remove((char)62);

                        string temp2 = new string(tempchar.ToArray());

                        short thingy = Convert.ToInt16(temp2);

                        byte[] thingyII = BitConverter.GetBytes(thingy);

                        Array.Reverse(thingyII);

                        codeBuffer.Add((char)thingyII[0]);
                        codeBuffer.Add((char)thingyII[1]);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }
                    #endregion

                    #region Unknown tag (length 5, type 2)
                    if (tagPartial == "unkwn")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)2);

                        string temp = tagString.Substring(7, tagString.Length - 7);

                        List<char> tempchar = new List<char>(temp);

                        tempchar.Remove((char)62);

                        string temp2 = new string(tempchar.ToArray());

                        short thingy = Convert.ToInt16(temp2);

                        byte[] thingyII = BitConverter.GetBytes(thingy);

                        Array.Reverse(thingyII);

                        codeBuffer.Add((char)thingyII[0]);
                        codeBuffer.Add((char)thingyII[1]);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }
                    #endregion

                    #region Anim tag
                    if (tagPartial == "anim:")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)3);

                        string temp = tagString.Substring(6, tagString.Length - 6);

                        List<char> tempchar = new List<char>(temp);

                        tempchar.Remove((char)62);

                        string temp2 = new string(tempchar.ToArray());

                        short thingy = Convert.ToInt16(temp2);

                        byte[] thingyII = BitConverter.GetBytes(thingy);

                        Array.Reverse(thingyII);

                        codeBuffer.Add((char)thingyII[0]);
                        codeBuffer.Add((char)thingyII[1]);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }
                    #endregion

                    #region Unknown Tag (length 5, type 0)
                    if (tagPartial == "unkno")
                    {
                        codeBuffer.Add((char)26); //Build control code
                        codeBuffer.Add((char)5);
                        codeBuffer.Add((char)0);

                        string temp = tagString.Substring(13, tagString.Length - 13);

                        List<char> tempchar = new List<char>(temp);

                        tempchar.Remove((char)62);

                        string temp2 = new string(tempchar.ToArray());

                        short thingy = Convert.ToInt16(temp2);

                        byte[] thingyII = BitConverter.GetBytes(thingy);

                        Array.Reverse(thingyII);

                        codeBuffer.Add((char)thingyII[0]);
                        codeBuffer.Add((char)thingyII[1]);

                        editData.InsertRange(offset + tagString.Length, codeBuffer); //Insert control code at the end of the
                        //control tag

                        for (int b = 0; b < tagString.Length; b++) //Remove the control tag
                        {
                            editData.RemoveAt(offset);
                        }

                        offset -= tagString.Length - 5;
                    }
                    #endregion
                }

                offset += 1;
            }

            //Editing the end of a string deletes the null terminator, so this
            //re-adds it if necessary.
            if (editData[editData.Count - 1] != 0) 
            {
                editData.Add((char)0);
            }

            return editData;
        }
    }
}
