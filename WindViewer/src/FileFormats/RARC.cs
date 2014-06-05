#region Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using WindViewer.Editor;

#endregion

namespace WindViewer.FileFormats
{
    public class RARC
    {
        #region Variables

        public string Filename;
        string FullPath;
        public byte[] Data { get; private set; }
        RARCHeader Header;
        public FileNode Root;
        public bool IsCompressed;

        #endregion

        #region Constructors

        public RARC() { }

        public RARC(string Path)
        {
            Load(Path);
            //printAllFileEntries(Root);

        }

        public void Load(string Path)
        {
            FullPath = Path;

            BinaryReader BR = new BinaryReader(File.OpenRead(Path));
            Data = BR.ReadBytes((int)BR.BaseStream.Length);
            BR.Close();

            Filename = System.IO.Path.GetFileName(FullPath);

            if (FSHelpers.ReadString(Data, 0, 4) == "Yaz0")
            {
                byte[] DecData = null;
                DecompressYaz0(Data, 0, ref DecData);
                Data = DecData;

                IsCompressed = true;
            }

            Header = new RARCHeader(this, Data, ref Root);

        }

        public static void printAllFileEntries(FileNode Root){
            Console.WriteLine("Node: " + Root.NodeName);
            foreach (RARC.FileEntry f in Root.Files) {
                Console.WriteLine("  Entry:" + f.FileName);
            }

            foreach (RARC.FileNode n in Root.ChildNodes)
                printAllFileEntries(n);

        }


        public void Save()
        {
            //Helpers.SaveBinary(FullPath, Header.FileData);
            RARCPacker.CompressRARC(FullPath, Root);

        }

        #endregion

        #region Yaz0 Decompression

        private static void DecompressYaz0(byte[] Input, uint Offset, ref byte[] Output)
        {
            UInt32 Size = (uint)FSHelpers.Read32(Input, (int)(Offset + 4));
            Output = new byte[Size];

            int SrcPlace = (int)Offset + 0x10, DstPlace = 0;

            uint ValidBitCount = 0;
            byte CodeByte = 0;
            while (DstPlace < Size)
            {
                if (ValidBitCount == 0)
                {
                    CodeByte = Input[SrcPlace];
                    ++SrcPlace;
                    ValidBitCount = 8;
                }

                if ((CodeByte & 0x80) != 0)
                {
                    Output[DstPlace] = Input[SrcPlace];
                    DstPlace++;
                    SrcPlace++;
                }
                else
                {
                    byte Byte1 = Input[SrcPlace];
                    byte Byte2 = Input[SrcPlace + 1];
                    SrcPlace += 2;

                    uint Dist = (uint)(((Byte1 & 0xF) << 8) | Byte2);
                    uint CopySource = (uint)(DstPlace - (Dist + 1));
                    uint NumBytes = (uint)(Byte1 >> 4);
                    if (NumBytes == 0)
                    {
                        NumBytes = (uint)(Input[SrcPlace] + 0x12);
                        SrcPlace++;
                    }
                    else
                        NumBytes += 2;

                    int i;
                    for (i = 0; i < NumBytes; ++i)
                    {
                        Output[DstPlace] = Output[CopySource];
                        CopySource++;
                        DstPlace++;
                    }
                }

                CodeByte <<= 1;
                ValidBitCount -= 1;
            }
        }

        #endregion

        #region Classes

        public class RARCHeader
        {
            public const int Size = 64;

            public RARC BaseRARC;

            public string Tag;
            public UInt32 FileSize;
            public UInt32 DataStartOffset;
            public UInt32 NumNodes;
            public UInt32 FileEntriesOffset;
            public UInt32 StringTableOffset;

            public byte[] FileData;

            public RARCHeader(RARC BR, byte[] Data, ref FileNode Root)
            {
                BaseRARC = BR;

                Tag = FSHelpers.ReadString(Data, 0, 4);
                FileSize = (uint)FSHelpers.Read32(Data, 4);
                DataStartOffset = (uint)FSHelpers.Read32(Data, 12);
                NumNodes = (uint)FSHelpers.Read32(Data, 32);
                FileEntriesOffset = (uint)FSHelpers.Read32(Data, 44);
                StringTableOffset = (uint)FSHelpers.Read32(Data, 52);

                FileData = Data;

                Root = new FileNode(BaseRARC, Data, 0, this);
            }

            public override string ToString()
            {
                return String.Format("Tag: {0}, FileSize: {1:X8}, DataStartOffset: {2:X8}, NumNodes: {3}, FileEntriesOffset: {4:X8}, StringTableOffset: {5:X8}",
                    Tag, FileSize, DataStartOffset, NumNodes, FileEntriesOffset, StringTableOffset);
            }
        }

        public class FileNode
        {
            public const int Size = 16;

            public RARC BaseRARC;

            public string Tag;
            public UInt32 FilenameOffset;
            public UInt16 NumFileEntries;
            public UInt32 FirstFileEntryOffset;

            public string NodeName;
            public List<FileNode> ChildNodes = new List<FileNode>();
            public List<FileEntry> Files = new List<FileEntry>();

            public FileNode(RARC BR, byte[] Data, UInt32 Offset, RARCHeader Header)
            {
                BaseRARC = BR;

                Offset = RARCHeader.Size + (Offset * Size);

                Tag = FSHelpers.ReadString(Data, (int)Offset, 4);
                FilenameOffset = (uint)FSHelpers.Read32(Data, (int)Offset + 4);
                NumFileEntries = (ushort)FSHelpers.Read16(Data, (int)Offset + 10);
                FirstFileEntryOffset = (uint)FSHelpers.Read32(Data, (int)Offset + 12);

                NodeName = FSHelpers.ReadString(Data, (int)(FilenameOffset + Header.StringTableOffset + 32));
                Console.WriteLine("Reading node: " + NodeName);
                for (int i = 0; i < NumFileEntries; ++i)
                {
                    UInt32 ReadOffset = (UInt32)(Header.FileEntriesOffset + (FirstFileEntryOffset * FileEntry.Size) + (i * FileEntry.Size) + 32);
                    FileEntry CurrentFile = new FileEntry(BaseRARC, Data, ReadOffset, Header);
                    Console.WriteLine("Found fileEntry: " + CurrentFile.FileName);
                    if (CurrentFile.ID == 0xFFFF || CurrentFile.Unknown2 == 0x0200)         // 0x2000 correct???
                    {
                        if (CurrentFile.FilenameOffset != 0 && CurrentFile.FilenameOffset != 2)
                            ChildNodes.Add(new FileNode(BaseRARC, Data, CurrentFile.DataOffset, Header));
                    }
                    else
                        Files.Add(CurrentFile);
                }
            }

            public override string ToString()
            {
                return String.Format("Tag: {0}, FilenameOffset: {1:X8}, NumFileEntries: {2}, FirstFileEntryOffset: {3:X8}, NodeName: {4}", Tag, FilenameOffset, NumFileEntries, FirstFileEntryOffset, NodeName);
            }
        }

        public class FileEntry
        {
            public const int Size = 20;

            public RARC BaseRARC;

            public UInt16 ID;
            public UInt16 Unknown1;
            public UInt16 Unknown2;
            public UInt16 FilenameOffset;
            public UInt32 DataOffset;
            public UInt32 DataSize;

            public string FileName;
            public bool IsCompressed;

            public byte[] fileData;

            RARCHeader Header;

            public FileEntry(RARC BR, byte[] Data, UInt32 Offset, RARCHeader Head)
            {
                BaseRARC = BR;

                ID = (ushort)FSHelpers.Read16(Data, (int)Offset);
                Unknown1 = (ushort)FSHelpers.Read16(Data, (int)Offset + 2);
                Unknown2 = (ushort)FSHelpers.Read16(Data, (int)Offset + 4);
                FilenameOffset = (ushort)FSHelpers.Read16(Data, (int)Offset + 6);
                DataOffset = (uint)FSHelpers.Read32(Data, (int)Offset + 8);
                DataSize = (uint)FSHelpers.Read32(Data, (int)Offset + 12);

                FileName = FSHelpers.ReadString(Data, (int)(FilenameOffset + Head.StringTableOffset + 32));

                IsCompressed = FSHelpers.ReadString(Head.FileData, (int)(Head.DataStartOffset + DataOffset + 32), 4) == "Yaz0";

                Header = Head;
                fileData = GetFileData_OLD();
            }

            public byte[] GetFileData_OLD()
            {
                byte[] Data = null;
                if (IsCompressed == true)
                    DecompressYaz0(Header.FileData, (Header.DataStartOffset + DataOffset + 32), ref Data);
                else
                {
                    Data = new byte[DataSize];
                    Buffer.BlockCopy(Header.FileData, (int)(Header.DataStartOffset + DataOffset + 32), Data, 0, (int)DataSize);
                }
                return Data;
            }

            public byte[] GetFileData(){    
                return fileData;
            }

            public void SetFileData_OLD(byte[] NewData)
            {
                if (IsCompressed == true)
                {
                    throw new Exception("Cannot set data in compressed file");
                }
                else
                {
                    if (NewData.Length == DataSize)
                        Buffer.BlockCopy(NewData, 0, Header.FileData, (int)(Header.DataStartOffset + DataOffset + 32), (int)DataSize);
                    else
                        throw new Exception("Data size mismatch");
                }
            }

            public void SetFileData(byte[] NewData)
            {
                if (IsCompressed == true)
                {
                    throw new Exception("Cannot set data in compressed file");
                }
                else
                {
                    fileData = NewData;
                    DataSize = (uint) fileData.Length;
                }
            }

            public override string ToString()
            {
                return String.Format("ID: {0}, FilenameOffset: {1:X8}, DataOffset: {2:X8}, DataSize: {3:X8}, FileName: {4}", ID, FilenameOffset, DataOffset, DataSize, FileName);
            }
        }

        #endregion
    }
}
