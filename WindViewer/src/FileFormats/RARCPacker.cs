using System;
using System.IO;
using System.Text;

// This is a port of the JayTheHam <jaytheham@gmail.com> RARCPacker by pho <pho@gatchan.org>
// It dumps the RARC structure from WindViewer into an .arc file
// The original packer only worked for folders with depth <= 2, this should work for any depth level

namespace WindViewer.FileFormats
{
	class RARCPacker
	{

		struct RarcHeader
		{
			public string Magic; //'RARC'
			public uint Size; //size of the .arc file
			public uint Unknown1; //0x00000020 Either this or Unknown6 is the offset to the NodeTable, and the other is how much should be added to all the offsets?
			public uint DataStartOffset; //Where the actual data starts. Add 0x20 to this.

			public uint SizeOfDataTable1;
			public uint SizeOfDataTable2;
			public uint Unknown4;
			public uint Unknown5;

			public uint NumNodes;
			public uint Unknown6; //0x00000020
			public uint NumFiles1; //Total number of file entries
			public uint FileEntriesOffset; //Add 0x20

			public uint SizeOfStringTable; //Size of StringTable after it has been padded out so that the data table begins at an address ending in 0
			public uint StringTableOffset; //Where is the string table stored? You have to add 0x20 to this value.
			public ushort NumFiles2; //Number of file entries
			public ushort Unknown8;
			public uint Unknown9;
		}

		struct Node
		{
			public string Name; //First four letters of the folder name, in CAPS
			public uint FilenameOffset; //directory name, offset into string table
			public ushort FoldernameHash; //A hash(?) of the foldername
			public ushort NumFileEntries; //how many files belong to this node
			public uint FirstFileEntryOffset; //Number of the first file entry that belongs to this node, Zero-based [from?]
		}

		struct FileEntry
		{
			public ushort Id; //file id. If this is 0xFFFF, then this entry is a subdirectory link
			public ushort FilenameHash; //A hash(?) of the file/foldername
			public ushort Unknown2; //If folder 0x0200, if file 0x1100.    This should be a byte and filenameOffset 3 bytes? 
			public ushort FilenameOffset; //file/subdir name, offset into string table
			public uint DataOffset; //offset to file data (for subdirs: index of Node representing the subdir)
			public uint DataSize; //size of data
			public uint Zero; //seems to be always '0'

			//unknown2
			//0x0200 = Folder
			//0x9500 = .szs layer data
		};

		private static string _stringTable;
        private static uint _numNodesDone;    //How many nodes have been filled out
        private static uint _numFilesWithData;//How many files with data have been added
        private static uint _lengthOfDataTable;//How much data has been added

		private static FileEntry[] _fileEntries;
		private static Node[] _nodes;
		private static byte[][] _filesData;

		private static int _totalNumFilesAdded;


		public static int countRARCDirs(RARC.FileNode root)
        {
			int count = 0;
			foreach (RARC.FileNode n in root.ChildNodes)
            {
                Console.WriteLine("Node: " + n.NodeName);
                count++;
                count += countRARCDirs (n);
			}

			return count;
		}


		public static int CountRarcFiles(RARC.FileNode root)
        {
			int count = 0;
            foreach (RARC.FileEntry f in root.Files)
            {
                Console.WriteLine("File: " + f.FileName);
				count++;
			}

            foreach (RARC.FileNode n in root.ChildNodes)
            {
				count += CountRarcFiles (n);
			}

			return count;
		}


        public static void CreateEntries(RARC.FileNode root)
        {

            CreateFileEntries(root);
            CreateNodeEntries(root);

            CreateDummyFiles();


            foreach (RARC.FileNode n in root.ChildNodes) {
                CreateNodes(n);
                CreateEntries(n);

            }

        }

        public static void CreateFileEntries(RARC.FileNode root)
        {
            foreach (RARC.FileEntry f in root.Files)
            {
                Console.WriteLine("Creating fileEntry for " + f.FileName);
                _fileEntries[_totalNumFilesAdded].Id = (ushort)_totalNumFilesAdded;

                if (f.FileName.EndsWith(".szs"))//Check if szs file and use right.. marker?
                    _fileEntries[_totalNumFilesAdded].Unknown2 = 0x9500;
                else
                    _fileEntries[_totalNumFilesAdded].Unknown2 = 0x1100;

                _fileEntries[_totalNumFilesAdded].FilenameOffset = (ushort)_stringTable.Length;
                string fileName = f.FileName;
                _stringTable = _stringTable + fileName + (char)0x00;
                _fileEntries[_totalNumFilesAdded].FilenameHash = Hash(fileName);

                _fileEntries[_totalNumFilesAdded].DataOffset = _lengthOfDataTable;
                _lengthOfDataTable += (uint)f.fileData.Length;
                //Pad the data table out so new files start at a 0-based address
                while ((_lengthOfDataTable % 16) != 0)
                    _lengthOfDataTable++;
                if ((_lengthOfDataTable % 32) != 0)//Check the new address is a multiple of 32 and add 16 if not
                    _lengthOfDataTable += 16;
                _filesData[_numFilesWithData] = f.fileData;

                _fileEntries[_totalNumFilesAdded].DataSize = (uint)f.fileData.Length;

                _numFilesWithData++;
                _totalNumFilesAdded++;
            }

        }

        public static void CreateNodeEntries(RARC.FileNode root)
        {

            foreach (RARC.FileNode n in root.ChildNodes)
            {
                Console.WriteLine("Creating fileEntry for node " + n.NodeName);

                _fileEntries[_totalNumFilesAdded].Id = 0xFFFF;
                _fileEntries[_totalNumFilesAdded].Unknown2 = 0x0200;
                _fileEntries[_totalNumFilesAdded].FilenameOffset = (ushort)0xFFFE;
                string fileName = n.NodeName;
                _fileEntries[_totalNumFilesAdded].FilenameHash = Hash(fileName);
                _fileEntries[_totalNumFilesAdded].DataOffset = (uint)0xFFFE;
                _fileEntries[_totalNumFilesAdded].DataSize = 0x10;

                _totalNumFilesAdded++;

            }

        }


        public static void CreateNodes(RARC.FileNode n)
        {
            Console.WriteLine("Creating Node for " + n.NodeName);
            string dirName = n.NodeName;
            dirName = dirName.ToUpper();
            for (int c = 0; c < dirName.Length; c++)
            {
                if (c == 4)
                    break;
                _nodes[_numNodesDone].Name = _nodes[_numNodesDone].Name + dirName[c];

            }
            _nodes[_numNodesDone].FilenameOffset = (uint)_stringTable.Length;
            _stringTable = _stringTable + dirName.ToLower() + (char)0x00;
            int numFiles = n.Files.Count;
            _nodes[_numNodesDone].NumFileEntries = (ushort)(numFiles + 2);
            _nodes[_numNodesDone].FirstFileEntryOffset = (uint)_totalNumFilesAdded;

            dirName = n.NodeName;
            _nodes[_numNodesDone].FoldernameHash = Hash(dirName);

            _numNodesDone++;
        }


		public static void CompressRARC(string fullPath, RARC.FileNode root)
		{

            Console.WriteLine("\n>> Compressing " + root.NodeName + " to " + fullPath);
            string newFile = root.NodeName;

			_stringTable = CreateStringTable();//Setup the string table

			int directoriesCount = countRARCDirs(root);
			_nodes = new Node[directoriesCount + 1]; //Add 1 for the ROOT node

			_numNodesDone = 0;
			_numFilesWithData = 0;
			_lengthOfDataTable = 0;

			//Fill out the ROOT node
			_nodes[0].Name = "ROOT";

			_nodes[0].FilenameOffset = (uint)_stringTable.Length;
			String rootDirName = newFile;
			_stringTable = _stringTable + rootDirName + (char)0x00;

			_nodes[0].FoldernameHash = Hash(rootDirName);

			_nodes[0].NumFileEntries = (ushort) (root.Files.Count + root.ChildNodes.Count + 2);

			_nodes[0].FirstFileEntryOffset = 0;

			_numNodesDone++; //One node is complete


			//Get the total number of subdirectories and files
			//string[] allFiles = Directory.GetFiles(args[0], "*", SearchOption.AllDirectories);
			//int numOfFilesAndDirs = allFiles.Length + directoriesCount;
			int filesCount = CountRarcFiles(root);
			int numOfFilesAndDirs = filesCount + directoriesCount;

			//Now set up an array of FileEntrys(Taking into account the "." and ".." file entries for each folder
			_fileEntries = new FileEntry[numOfFilesAndDirs + (directoriesCount * 2) + 2];
            Console.WriteLine("# fileEntries " + (numOfFilesAndDirs + (directoriesCount * 2) + 2));

            _filesData = new byte[filesCount][]; //Setup an array to store all the file data paths in
			_totalNumFilesAdded = 0; //How many file entries have been done


            CreateEntries(root);


			//Fill out the filename & data offsets for the folder entries with the offset from the appropriate Node
			for (int n = 0; n < _totalNumFilesAdded; n++)
			{
				if (_fileEntries[n].FilenameOffset == 0xFFFE)
				{
					uint nodeNum = 0;
					foreach (Node node in _nodes)
					{
						if (node.FoldernameHash == _fileEntries[n].FilenameHash)
						{
							_fileEntries[n].FilenameOffset = (ushort)node.FilenameOffset;
							_fileEntries[n].DataOffset = nodeNum;
						}
						nodeNum++;
					}
				}
			}

			//Make the data table a mutiple of 16
			int numOfPaddingBytes = 0;
			while ((_lengthOfDataTable % 16) != 0)
			{
				_lengthOfDataTable++;
				numOfPaddingBytes++;
			}

			//Fill out Header information
			RarcHeader header = new RarcHeader();
			header.Magic = "RARC";
			header.NumFiles1 = (uint)_totalNumFilesAdded;
			header.NumFiles2 = (ushort)_totalNumFilesAdded;
			header.SizeOfDataTable1 = _lengthOfDataTable;
			header.SizeOfDataTable2 = _lengthOfDataTable;
			header.Unknown1 = 0x20;
			header.Unknown6 = 0x20;
			header.Unknown8 = 0x100;

			header.FileEntriesOffset = (_numNodesDone * 16) + 0x20;
			if ((header.FileEntriesOffset % 32) != 0)//Check if it's a multiple of 32 and make it one if it's not
				header.FileEntriesOffset += 16;

            Console.WriteLine("fileEntriesOffset is: " + header.FileEntriesOffset);
            Console.WriteLine("totalNumFilesAdded: " + (_totalNumFilesAdded+1));
			header.NumNodes = _numNodesDone;

            int numFileEntries = (numOfFilesAndDirs + (directoriesCount * 2) + 2);

			int x = 0;
            while (0 != (((numFileEntries) * 20) + x) % 16)
				x++;

            header.StringTableOffset = header.FileEntriesOffset + (uint)((numFileEntries * 20) + x);
			if ((header.StringTableOffset % 32) != 0)//Check if it's a multiple of 32 and make it one if it's not
				header.StringTableOffset += 16;

            Console.WriteLine("stringTableOffset is: " + header.StringTableOffset);

			while (0 != (_stringTable.Length) % 16)//Pad out the string table so the data table starts at a 0based address
				_stringTable = _stringTable + (char)0x00;

			header.DataStartOffset = (uint)(header.StringTableOffset + _stringTable.Length);
			if ((header.DataStartOffset % 32) != 0)//Check if it's a multiple of 32 and make it one if it's not
				header.DataStartOffset += 16;

            Console.WriteLine("dataStartOffset is: " + header.DataStartOffset);
			

            header.SizeOfStringTable = (uint)_stringTable.Length;
			header.Size = _lengthOfDataTable + header.DataStartOffset + 0x20;

			//Let's write it out

            // Uncomment while testing
            //FullPath += ".new.arc";

			FileStream filestreamWriter = new FileStream(fullPath, FileMode.Create);
			BinaryWriter binWriter = new BinaryWriter(filestreamWriter);

            Console.WriteLine("Writing to file: " + fullPath);
            Console.WriteLine("Writing header...");
			//First the Header is written
			binWriter.Write(header.Magic[0]);
			binWriter.Write(header.Magic[1]);
			binWriter.Write(header.Magic[2]);
			binWriter.Write(header.Magic[3]);

			byte[] buffer = BitConverter.GetBytes(header.Size);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.Unknown1);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.DataStartOffset);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.SizeOfDataTable1);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.SizeOfDataTable2);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.Unknown4);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.Unknown5);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.NumNodes);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.Unknown6);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.NumFiles1);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.FileEntriesOffset);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);


			buffer = BitConverter.GetBytes(header.SizeOfStringTable);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.StringTableOffset);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.NumFiles2);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.Unknown8);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(header.Unknown9);
			Array.Reverse(buffer);
			filestreamWriter.Write(buffer, 0, buffer.Length);

            Console.WriteLine("Writing nodes...");
			//Write each of the nodes
			foreach (Node currentNode in _nodes)
			{
                Console.WriteLine("Writing " + currentNode.Name);
				binWriter.Write(currentNode.Name[0]);
				if (currentNode.Name.Length > 1)       //Incase the dirname is only 1 char long
					binWriter.Write(currentNode.Name[1]);
				else
					filestreamWriter.WriteByte(0x20);
				if (currentNode.Name.Length > 2)       //Incase the dirname is only 2 char long
					binWriter.Write(currentNode.Name[2]);
				else
					filestreamWriter.WriteByte(0x20);
				if (currentNode.Name.Length == 4)       //Incase the dirname is only 3 char long
					binWriter.Write(currentNode.Name[3]);
				else
					filestreamWriter.WriteByte(0x20);

				buffer = BitConverter.GetBytes(currentNode.FilenameOffset);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);
				buffer = BitConverter.GetBytes(currentNode.FoldernameHash);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);
				buffer = BitConverter.GetBytes(currentNode.NumFileEntries);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);
				buffer = BitConverter.GetBytes(currentNode.FirstFileEntryOffset);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);
			}

			//Pad out the file to get the file entries at their correct offset
			while (filestreamWriter.Position < (header.FileEntriesOffset + 32))
			{
				filestreamWriter.WriteByte(0x00);
			}

			//Write all the file entries
			foreach (FileEntry entry in _fileEntries)
			{
                Console.WriteLine(String.Format("Writing fileEntry {0:X6}",entry.FilenameOffset));

				buffer = BitConverter.GetBytes(entry.Id);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(entry.FilenameHash);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(entry.Unknown2);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(entry.FilenameOffset);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(entry.DataOffset);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(entry.DataSize);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(entry.Zero);
				Array.Reverse(buffer);
				filestreamWriter.Write(buffer, 0, buffer.Length);
			}

			//Pad out the file to get the string table at its correct offset

			while (filestreamWriter.Position < (header.StringTableOffset + 32))
			{
               filestreamWriter.WriteByte(0x00);
			}

			//Write string table
			Encoding enc = Encoding.UTF8;
			binWriter.Write(enc.GetBytes(_stringTable));

			//Pad out the file to get the data table at its correct offset
			while (filestreamWriter.Position < (header.DataStartOffset + 32))
			{
				filestreamWriter.WriteByte(0x00);
			}

			//Write files data
			foreach (byte[] file in _filesData)
			{
                Console.WriteLine("Dumping file data...");
                buffer = file;
				filestreamWriter.Write(buffer, 0, buffer.Length);
				while ((filestreamWriter.Position % 32) != 0)//Pad out the data so the next file begins on a 0-based multiple of 32
					filestreamWriter.WriteByte(0x00);
			}
			for (int pad = 0; pad < numOfPaddingBytes; pad++)//Write the bytes neccessary to make the entire file divisble by 32
				filestreamWriter.WriteByte(0x00);

			binWriter.Close();
			filestreamWriter.Close();

			Console.WriteLine("Packed and good to go!");
		}

		static string CreateStringTable()
		{
			char spacer = (char)0x00;
			char fullStop = '.';
			char[] stringTableHeader = new char[5];
			stringTableHeader[0] = fullStop;
			stringTableHeader[1] = spacer;
			stringTableHeader[2] = fullStop;
			stringTableHeader[3] = fullStop;
			stringTableHeader[4] = spacer;
			//Load the string table with the "Header"
			String stringTable = new string(stringTableHeader);

			return stringTable;
		}

		static void CreateDummyFiles()
		{

            Console.WriteLine("Creating fileEntry: .");
            Console.WriteLine("Creating fileEntry: ..");
			//Add in the "Dummy" folder entries
			for (int i = 0; i < 2; i++)
			{
				_fileEntries[_totalNumFilesAdded].Id = 0xFFFF;
				_fileEntries[_totalNumFilesAdded].Unknown2 = 0x0200;
				if (i == 0)
				{
					_fileEntries[_totalNumFilesAdded].FilenameOffset = 0;
					_fileEntries[_totalNumFilesAdded].FilenameHash = Hash(Convert.ToString(_stringTable[0]));
					_fileEntries[_totalNumFilesAdded].DataOffset = (uint)(_numNodesDone - 1);
				}
				else
				{
					_fileEntries[_totalNumFilesAdded].FilenameOffset = 0x0002;
					string name = "..";
					_fileEntries[_totalNumFilesAdded].FilenameHash = Hash(name);
					if (_numNodesDone == 1)
						_fileEntries[_totalNumFilesAdded].DataOffset = 0xFFFFFFFF;
					else
						_fileEntries[_totalNumFilesAdded].DataOffset = 0;
				}
				_fileEntries[_totalNumFilesAdded].DataSize = 0x10;

				_totalNumFilesAdded++;
			}
		}

		static void CreateNode(string[] folder)
		{
			string dirName = new FileInfo(folder[0]).Name;
			dirName = dirName.ToUpper();
			for (int c = 0; c < dirName.Length; c++)
			{
				if (c == 4)
					break;
				_nodes[_numNodesDone].Name = _nodes[_numNodesDone].Name + dirName[c];

			}
			_nodes[_numNodesDone].FilenameOffset = (uint)_stringTable.Length;
			_stringTable = _stringTable + dirName.ToLower() + (char)0x00;
			string[] numFiles = Directory.GetFileSystemEntries(folder[0]);
			_nodes[_numNodesDone].NumFileEntries = (ushort)(numFiles.Length + 2);
			_nodes[_numNodesDone].FirstFileEntryOffset = (uint)_totalNumFilesAdded;

			dirName = new FileInfo(folder[0]).Name;
			_nodes[_numNodesDone].FoldernameHash = Hash(dirName);

			_numNodesDone++;
		}

		static ushort Hash(string filename)
		{
			ushort hash = 0;
			int multiplier = 1;
			byte currentChar;

			for (int i = (filename.Length - 1); i >= 0; i--)
			{
				currentChar = Convert.ToByte(filename[i]);
				hash += (ushort)(currentChar * multiplier);
				multiplier = (multiplier * 3);
			}
			return hash;
			//The hash of filename "xyz" = (z*M)+(y*M)+(x*M) etc...
			//M = 1 to start with, after each multipication M = (M*3)
		}

	}
}
