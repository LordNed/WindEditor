using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using GameFormatReader.Common;
using WindViewer.Properties;

namespace WindViewer.Editor
{
    public class FileSystem
    {
        /// <summary>
        /// Usage: using(var myFile = FileSystem.LoadFile("relativePath")) { // Do stuff with myFile }
        /// 
        /// Returns a EndianBinaryReader (using big-endian) which contains the specified file. This 
        /// function will eventually be Folder vs. ISO agnostic.
        /// </summary>
        /// <param name="relativeFilePath">Relative to the root dir of the WindWaker ISO extract.</param>
        public static EndianBinaryReader LoadFileRelative(string relativeFilePath)
        {
            return LoadFileRelative(relativeFilePath, Endian.Big);
        }

        /// <summary>
        /// Usage: using(var myFile = FileSystem.LoadFile("relativePath")) { // Do stuff with myFile }
        /// 
        /// Returns a EndianBinaryReader (of specified endian-ness) which contains the specified file. This 
        /// function will eventually be Folder vs. ISO agnostic.
        /// </summary>
        /// <param name="relativeFilePath">Relative to the root dir of the WindWaker ISO extract.</param>
        public static EndianBinaryReader LoadFileRelative(string relativeFilePath, Endian endian)
        {
            if (string.IsNullOrEmpty(Settings.Default.rootDiskDir))
                throw new SettingsPropertyNotFoundException("Root Disk Dir not configured, cannot retrieve local file.");

            string combinedFilePath = Path.Combine(Settings.Default.rootDiskDir, relativeFilePath);
            if (!File.Exists(combinedFilePath))
                throw new FileNotFoundException("Requested local file not found.", combinedFilePath);

            return new EndianBinaryReader(File.Open(combinedFilePath, FileMode.Open, FileAccess.Read), endian);
        }

        /*public static void dumpArcToFile(GameFormatReader.GCWii.Binaries.GC.RARC archive, string destDir)
        {
            //So you wanna dump a RARC file's contents to disk, eh?

            //Combine working dir path with the name of the root folder
            string workingDir = Path.Combine(destDir, archive.Nodes[0].Name);

            //Declaring binary writer for later use
            BinaryWriter bwriter;

            //Create root dir
            Directory.CreateDirectory(workingDir);

            //This'll iterate through the entries belonging to the root node
            //and do two things - create subdirs and write files to the root
            //if necessary
            for (int i = 0; i < archive.Nodes[0].Entries.Length; i++)
            {
                //Create a path for the folder or file
                string tempDirString = Path.Combine(workingDir, archive.Nodes[0].Entries[i].Name);

                //Create subdirs
                if (archive.Nodes[0].Entries[i].IsDirectory)
                {
                    //Create directory from tempDirString
                    Directory.CreateDirectory(tempDirString);
                }

                //Create files on root dir
                else
                {
                    //Instantiate bwriter with the file path
                    bwriter = new BinaryWriter(File.Open(tempDirString, FileMode.Create));

                    //Write the entry's data
                    FSHelpers.WriteArray(bwriter, archive.Nodes[0].Entries[i].Data);

                    //Close bwriter
                    bwriter.Close();
                }
            }

            if (archive.Nodes.Length > 1)
            {
                //There are folders on the root? Things just got real

                for (int i = 1; i < archive.Nodes.Length; i++)
                {
                    //Our new working directory
                    string subDirWorkingPath = Path.Combine(workingDir, archive.Nodes[i].Name);

                    foreach (GameFormatReader.GCWii.Binaries.GC.RARC.FileEntry entry in archive.Nodes[i].Entries)
                    {
                        //Create a name for the file
                        string subDirFileOrFolderPath = Path.Combine(subDirWorkingPath, entry.Name);

                        //This way we can only output actual files
                        if (entry.Data != null)
                        {
                            bwriter = new BinaryWriter(File.Open(subDirFileOrFolderPath, FileMode.Create));

                            FSHelpers.WriteArray(bwriter, entry.Data);

                            bwriter.Close();
                        }
                    }
                }
            }
        }*/

        public static void arcContentsToFile(GameFormatReader.GCWii.Binaries.GC.RARC archive, string destDir)
        {
            //This function doesn't work for arcs with multiple levels of directories,
            //But it doesn't seem like Nintendo uses those commonly, anyway.

            string workingDir = Path.Combine(destDir, archive.Nodes[0].Name);

            string rootDir = workingDir;

            Directory.CreateDirectory(workingDir);

            for (int i = 0; i < archive.Nodes.Length; i++)
            {
                extractContents(archive.Nodes[i], archive, workingDir);

                if (i < archive.Nodes.Length - 1)
                {
                    workingDir = Path.Combine(rootDir, archive.Nodes[i + 1].Name);
                }
            }
        }

        public static void extractContents(GameFormatReader.GCWii.Binaries.GC.RARC.Node node, GameFormatReader.GCWii.Binaries.GC.RARC archive, string workingDir)
        {
            BinaryWriter bwriter;

            string targetElement;

            foreach (GameFormatReader.GCWii.Binaries.GC.RARC.FileEntry entry in node.Entries)
            {
                targetElement = Path.Combine(workingDir, entry.Name);

                if (!entry.IsDirectory)
                {
                    bwriter = new BinaryWriter(File.Open(targetElement, FileMode.Create));

                    FSHelpers.WriteArray(bwriter, entry.Data);

                    bwriter.Close();
                }

                else
                {
                        Directory.CreateDirectory(targetElement);
                }
            }
        }
    }
}