using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WindViewer;
using WindViewer.Editor;

namespace WindViewer.Editor
{    
    /// <summary>
    /// The Wind Waker uses a version of the BMG file format to store message data. This class file parses
    /// the data, converts the binary-based control codes into text-based control tags, and returns a 
    /// list of messages for use elsewhere in the program.
    /// </summary>

    public class message
    {
        public int stringOffset;
        public short messageID;
        public short padding1;
        public int messageType;
        public byte textBoxType;
        public byte charDrawType;
        public byte boxPosition;
        public byte itemImage; //Only set when textBoxType == 9
        public byte unknown1; //Only set when itemImage is set, and always appears to be 0 of 1
        public byte groupID1;
        public byte groupID2;
        public byte groupID3;
        public short padding2;
        public byte linesPerTextBox;
        public byte padding3;

        public List<char> stringData;

        public int originalStringDataLength;

        public void save(BinaryWriter bw)
        {
            FSHelpers.Write32(bw, stringOffset);
            FSHelpers.Write16(bw, (ushort)messageID);
            FSHelpers.Write16(bw, (ushort)padding1);
            FSHelpers.Write32(bw, messageType);
            FSHelpers.Write8(bw, textBoxType);
            FSHelpers.Write8(bw, charDrawType);
            FSHelpers.Write8(bw, boxPosition);
            FSHelpers.Write8(bw, itemImage);
            FSHelpers.Write8(bw, unknown1);
            FSHelpers.Write8(bw, groupID1);
            FSHelpers.Write8(bw, groupID2);
            FSHelpers.Write8(bw, groupID3);
            FSHelpers.Write16(bw, (ushort)padding2);
            FSHelpers.Write8(bw, linesPerTextBox);
            FSHelpers.Write8(bw, padding3);
        }
    }

    class bmgHeader
    {
        public string MESGbmg1;
        public int fileSize;
        public int numChunks;
        public int unknown1;
        public int padding1;
        public int padding2;
        public int padding3;
    }

    class inf1Header
    {
        public string inf1; //"INF1"
        public int chunkSize;
        public short messageCount;
        public short messageEntrySize; //Always 0x18/24
        public int padding1; //Always 0
    }

    class dat1Header
    {
        public string dat1;
        public int chunkSize;
    }

    class MessageParser
    {
        public List<message> messages = new List<message>();

        public int offset = 0;

        public int dat1Offset = 0;

        public List<message> parser(byte[] data)
        {
            bmgHeader bmgHeader = new bmgHeader();

            bmgHeader.MESGbmg1 = Helpers.ReadString(data, offset);
            bmgHeader.fileSize = (int)Helpers.Read32(data, offset + 8);
            bmgHeader.numChunks = (int)Helpers.Read32(data, offset + 12);
            bmgHeader.unknown1 = (int)Helpers.Read32(data, offset + 16);
            bmgHeader.padding1 = (int)Helpers.Read32(data, offset + 20);
            bmgHeader.padding2 = (int)Helpers.Read32(data, offset + 24);
            bmgHeader.padding3 = (int)Helpers.Read32(data, offset + 28);

            offset += 32;

            inf1Header inf1Header = new inf1Header();

            inf1Header.inf1 = Helpers.ReadString(data, offset);
            inf1Header.chunkSize = (int)Helpers.Read32(data, offset + 4);
            inf1Header.messageCount = (short)Helpers.Read16(data, offset + 8);
            inf1Header.messageEntrySize = (short)Helpers.Read16(data, offset + 10);
            inf1Header.padding1 = (int)Helpers.Read32(data, offset + 12);

            dat1Offset += inf1Header.chunkSize + 32;

            dat1Header dat1Header = new dat1Header();

            dat1Header.dat1 = Helpers.ReadString(data, dat1Offset);
            dat1Header.chunkSize = (int)Helpers.Read32(data, dat1Offset + 4);

            dat1Offset += 8;

            offset += 16;

            for (int i = 0; i < inf1Header.messageCount; i++)
            {
                message message = new message();

                message.stringOffset = (int)Helpers.Read32(data, offset);
                message.messageID = (short)Helpers.Read16(data, offset + 4);
                message.padding1 = (short)Helpers.Read16(data, offset + 6);
                message.messageType = (int)Helpers.Read32(data, offset + 8);
                message.textBoxType = Helpers.Read8(data, offset + 12);
                message.charDrawType = Helpers.Read8(data, offset + 13);
                message.boxPosition = Helpers.Read8(data, offset + 14);
                message.itemImage = Helpers.Read8(data, offset + 15);
                message.unknown1 = Helpers.Read8(data, offset + 16);
                message.groupID1 = Helpers.Read8(data, offset + 17);
                message.groupID2 = Helpers.Read8(data, offset + 18);
                message.groupID3 = Helpers.Read8(data, offset + 19);
                message.padding2 = (short)Helpers.Read16(data, offset + 20);
                message.linesPerTextBox = Helpers.Read8(data, offset + 22);
                message.padding3 = Helpers.Read8(data, offset + 23);

                if (message.stringOffset == 0)
                {
                    offset += 24;
                }

                else
                {
                    //message.stringData = getStringData(data, message.stringOffset, dat1Offset);

                    //message.originalStringDataLength = message.stringData.Count;

                    offset += 24;
                }

                messages.Add(message);
            }

            for (int i = 0; i < messages.Count; i++)
            {
                /*
                 * This grabs the string data for each entry, wherever applicable. We need
                 * to check for a couple of things before we can actually get data.
                */

                if (messages[i].stringOffset == 0) //Check if this is a nulled entry
                {
                    continue;
                }

                messages[i].stringData = new List<char>();

                int stringOffset = dat1Offset + messages[i].stringOffset;

                if (messages[i] == messages[messages.Count - 1]) //Check if this is the last entry
                {
                    messages[i].originalStringDataLength = (dat1Header.chunkSize - 8) - messages[i].stringOffset;

                    messages[i].stringData = getStringData(data, stringOffset, messages[i].originalStringDataLength);

                    continue;
                }

                if (messages[i + 1].stringOffset == 0) //Check if we can use the next entry to calculate the string length
                {
                    int k = 1;

                    while (messages[i + k].stringOffset == 0)
                    {
                        k += 1;
                    }

                    messages[i].originalStringDataLength = messages[i + k].stringOffset - messages[i].stringOffset;

                    messages[i].stringData = getStringData(data, stringOffset, messages[i].originalStringDataLength);

                    continue;
                }

                messages[i].originalStringDataLength = messages[i + 1].stringOffset - messages[i].stringOffset;

                messages[i].stringData = getStringData(data, stringOffset, messages[i].originalStringDataLength);
            }

            foreach (message message in messages)
            {
                if (message.stringOffset == 0)
                {
                    continue;
                }

                message.stringData = convertControlCodes(message.stringData);
            }

            return messages;
        }

        public List<char> getStringData(byte[] data, int stringOffset, int stringLength)
        {
            List<char> letterArray = new List<char>();

            for (int i = 0; i < stringLength; i++)
            {
                letterArray.Add((char)Helpers.Read8(data, stringOffset + i));
            }

            return letterArray;
        }

        public List<char> convertControlCodes(List<char> data)
        {
            List<char> editData = new List<char>(data);
            int offset = 0;

            byte codeLength;
            byte codeType;
            byte codeData;
            byte[] indexAppendArray = new byte[2];
            short index;

            string codeString = "";
            char[] codeStringArray;

            foreach (byte character in data)
            {
                if (character == 26)
                {
                    codeLength = (byte)editData[offset + 1];

                    if (codeLength == 5)
                    {
                        codeType = (byte)editData[offset + 2];
                        codeData = (byte)editData[offset + 4];

                        if (codeType == 1)
                        {
                            //We need to grab a short out of the char/byte data. So,
                            //We take the two bytes we need and put them into an array
                            //so that BitConverter can splice them together.

                            indexAppendArray[0] = (byte)editData[offset + 4];
                            indexAppendArray[1] = (byte)editData[offset + 3];

                            index = BitConverter.ToInt16(indexAppendArray, 0);

                            codeString = "<sound:" + index + ">";

                            codeStringArray = codeString.ToCharArray();

                            editData.InsertRange(offset + 5, codeStringArray);

                            for (int i = 0; i < 5; i++)
                            {
                                editData.RemoveAt(offset);
                            }

                            offset += codeString.Length - 5;
                        }

                        if (codeType == 2)
                        {
                            //We need to grab a short out of the char/byte data. So,
                            //We take the two bytes we need and put them into an array
                            //so that BitConverter can splice them together.

                            indexAppendArray[0] = (byte)editData[offset + 4];
                            indexAppendArray[1] = (byte)editData[offset + 3];

                            index = BitConverter.ToInt16(indexAppendArray, 0);

                            codeString = "<unkwn:" + index + ">";

                            codeStringArray = codeString.ToCharArray();

                            editData.InsertRange(offset + 5, codeStringArray);

                            for (int i = 0; i < 5; i++)
                            {
                                editData.RemoveAt(offset);
                            }

                            offset += codeString.Length - 5;
                        }

                        if (codeType == 3)
                        {
                            //We need to grab a short out of the char/byte data. So,
                            //We take the two bytes we need and put them into an array
                            //so that BitConverter can splice them together.

                            indexAppendArray[0] = (byte)editData[offset + 4];
                            indexAppendArray[1] = (byte)editData[offset + 3];

                            index = BitConverter.ToInt16(indexAppendArray, 0);

                            codeString = "<anim:" + index + ">";

                            codeStringArray = codeString.ToCharArray();

                            editData.InsertRange(offset + 5, codeStringArray);

                            for (int i = 0; i < 5; i++)
                            {
                                editData.RemoveAt(offset);
                            }

                            offset += codeString.Length - 5;
                        }

                        if (codeType == 0)
                        {
                            switch (codeData)
                            {
                                case 0:
                                    codeString = "<player>";
                                    break;
                                case 1:
                                    codeString = "<draw:instant>";
                                    break;
                                case 2:
                                    codeString = "<draw:char>";
                                    break;
                                case 8:
                                    codeString = "<2 choices>";
                                    break;
                                case 9:
                                    codeString = "<3 choices>";
                                    break;
                                case 10:
                                    codeString = "<A button>";
                                    break;
                                case 11:
                                    codeString = "<B button>";
                                    break;
                                case 12:
                                    codeString = "<C-stick>";
                                    break;
                                case 13:
                                    codeString = "<L trigger>";
                                    break;
                                case 14:
                                    codeString = "<R trigger>";
                                    break;
                                case 15:
                                    codeString = "<X button>";
                                    break;
                                case 16:
                                    codeString = "<Y button>";
                                    break;
                                case 17:
                                    codeString = "<Z button>";
                                    break;
                                case 18:
                                    codeString = "<D-pad>";
                                    break;
                                case 19:
                                    codeString = "<control stick>";
                                    break;
                                case 20:
                                    codeString = "<arrow:left>";
                                    break;
                                case 21:
                                    codeString = "<arrow:right>";
                                    break;
                                case 22:
                                    codeString = "<arrow:up>";
                                    break;
                                case 23:
                                    codeString = "<arrow:down>";
                                    break;
                                case 24:
                                    codeString = "<control stick:up>";
                                    break;
                                case 25:
                                    codeString = "<control stick:down>";
                                    break;
                                case 26:
                                    codeString = "<control stick:left>";
                                    break;
                                case 27:
                                    codeString = "<control stick:right>";
                                    break;
                                case 28:
                                    codeString = "<control stick:up + down>";
                                    break;
                                case 29:
                                    codeString = "<control stick:left + right>";
                                    break;
                                case 30:
                                    codeString = "<choice:1>";
                                    break;
                                case 31:
                                    codeString = "<choice:2>";
                                    break;

                                case 33:
                                    codeString = "<rupee cost:red>";
                                    break;
                                case 34:
                                    codeString = "<rupee cost:white>";
                                    break;
                                case 35:
                                    codeString = "<item name>";
                                    break;

                                case 39:
                                    codeString = "<starburst A button>";
                                    break;
                                case 40:
                                    codeString = "<blow count>";
                                    break;
                                case 41:
                                    codeString = "<pirate ship password>";
                                    break;
                                case 42:
                                    codeString = "<target starburst>";
                                    break;
                                case 43:
                                    codeString = "<value>";
                                    break;
                                case 44:
                                    codeString = "<rupee total>";
                                    break;

                                case 48:
                                    codeString = "<yard count>";
                                    break;
                                case 57:
                                    codeString = "<heart>";
                                    break;
                                
                                default:
                                    codeString = "<unknowncode:" + codeData + ">";
                                    break;
                            }

                            codeStringArray = codeString.ToCharArray();

                            editData.InsertRange(offset + 5, codeStringArray);

                            for (int i = 0; i < 5; i++)
                            {
                                editData.RemoveAt(offset);
                            }

                            offset += codeString.Length - 5;
                        }
                    }

                    if (codeLength == 6)
                    {
                        codeData = (byte)editData[offset + 5];

                        codeString = "<color:" + codeData + ">";

                        codeStringArray = codeString.ToCharArray();

                        editData.InsertRange(offset + 6, codeStringArray);

                        for (int i = 0; i < 6; i++)
                        {
                            editData.RemoveAt(offset);
                        }

                        offset += codeString.Length - 6;
                    }

                    if (codeLength == 7)
                    {
                        codeType = (byte)editData[offset + 4];

                        if (codeType == 1)
                        {
                            byte codeData2 = (byte)editData[offset + 5];
                            codeData = (byte)editData[offset + 6];

                            byte[] temp = new byte[2];

                            temp[0] = codeData2;
                            temp[1] = codeData;

                            Array.Reverse(temp);

                            short codeData3 = BitConverter.ToInt16(temp, 0); 

                            codeString = "<scale:" + codeData3 + ">";

                            codeStringArray = codeString.ToCharArray();

                            editData.InsertRange(offset + 7, codeStringArray);

                            for (int i = 0; i < 7; i++)
                            {
                                editData.RemoveAt(offset);
                            }

                            offset += codeString.Length - 7;
                        }

                        if (codeType == 4)
                        {
                            byte codeData2 = (byte)editData[offset + 5];
                            codeData = (byte)editData[offset + 6];

                            byte[] temp = new byte[2];

                            temp[0] = codeData2;
                            temp[1] = codeData;

                            Array.Reverse(temp);

                            short codeData3 = BitConverter.ToInt16(temp, 0); 

                            codeString = "<wait + dismiss:" + codeData3 + ">";

                            codeStringArray = codeString.ToCharArray();

                            editData.InsertRange(offset + 7, codeStringArray);

                            for (int i = 0; i < 7; i++)
                            {
                                editData.RemoveAt(offset);
                            }

                            offset += codeString.Length - 7;
                        }

                        if (codeType == 7)
                        {
                            byte codeData2 = (byte)editData[offset + 5];
                            codeData = (byte)editData[offset + 6];

                            byte[] temp = new byte[2];

                            temp[0] = codeData2;
                            temp[1] = codeData;

                            Array.Reverse(temp);

                            short codeData3 = BitConverter.ToInt16(temp, 0); 

                            codeString = "<wait:" + codeData3 + ">";

                            codeStringArray = codeString.ToCharArray();

                            editData.InsertRange(offset + 7, codeStringArray);

                            for (int i = 0; i < 7; i++)
                            {
                                editData.RemoveAt(offset);
                            }

                            offset += codeString.Length - 7;
                        }
                    
                    }
                }

                offset += 1;
            }

            return editData;
        }
    }
}
