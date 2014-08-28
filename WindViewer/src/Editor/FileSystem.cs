using System.Configuration;
using System.IO;
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
    }
}