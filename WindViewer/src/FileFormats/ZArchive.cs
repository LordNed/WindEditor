using System;
using System.Collections.Generic;
using System.IO;

namespace WindViewer.FileFormats
{
    /// <summary>
    /// This is metadata to go with the WorldspaceProject. Each ZArchive describes one (eventual) .arc file.
    /// It holds a list of BaseArchiveFiles, which describe a file (room.dzb, model1.btk, etc.) that the archive
    /// contains. 
    /// </summary>
    public sealed class ZArchive
    {
        //This is a list of all loaded files from the Archive.
        private readonly List<BaseArchiveFile> _archiveFiles;

        //This is the name of the Archive, ie: "Room0" "Stage", etc.
        public string Name;

        //If this is a Room, the Room number, -1 if stage.
        public int RoomNumber;

        public ZArchive()
        {
            _archiveFiles = new List<BaseArchiveFile>();
            Name = "Unnamed";
        }

        public void AddFileToArchive(BaseArchiveFile file)
        {
            _archiveFiles.Add(file);
        }

        public void RemoveFileFromArchive(BaseArchiveFile file)
        {
            _archiveFiles.Remove(file);
        }

        public List<BaseArchiveFile> GetAllFiles()
        {
            return _archiveFiles;
        }

        /// <summary>
        /// Invokes the Save() interface on each ArchiveFile. Generates the required
        /// folders and saves out each individual file.
        /// </summary>
        /// <param name="archiveRootFolder">Folder inside the WrkDir to save to, ie: "C:\...\MiniHyo\Room0\"</param>
        public void Save(string archiveRootFolder)
        {
            foreach (BaseArchiveFile archiveFile in _archiveFiles)
            {
                //Create the sub-folder
                string subFolder = Path.Combine(archiveRootFolder, archiveFile.FolderName);
                Directory.CreateDirectory(subFolder);

                //Open the file for Read/Write Access
                using (BinaryWriter bw = new BinaryWriter(File.Open(Path.Combine(subFolder, archiveFile.FileName), FileMode.Create)))
                {
                    try
                    {
                        archiveFile.Save(bw);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving file " + archiveFile.FileName + " to " + subFolder + "! Error: " + ex);
                    }
                }
            }

        }

        /// <summary>
        /// Pass this a Room&lt;x&gt; folder or a Stage folder directory! This will look for specific subfolders 
        /// (bdl, btk, dzb, dzr, dzs, dat, etc.) and load each file within them as appropriate.
        /// </summary>
        /// <param name="directory">Absolute file path to a folder containing a bdl/btk/etc. files(s)</param>
        public void LoadFromDirectory(string directory)
        {
            //Get all of the sub folders (bdl, btk, etc.)
            string[] subFolders = Directory.GetDirectories(directory);

            foreach (string folder in subFolders)
            {
                //Then grab all of the files that are inside this folder and we'll load each one.
                string[] subFiles = Directory.GetFiles(folder);

                foreach (string filePath in subFiles)
                {
                    BinaryReader br = new BinaryReader(File.OpenRead(filePath));
                    try
                    {
                        BaseArchiveFile file;

                        byte[] fileData = br.ReadBytes((int)br.BaseStream.Length);
                        switch ((new DirectoryInfo(folder).Name).ToLower())
                        {
                            /* Map Collision Format */
                            case "dzb":
                                file = new StaticCollisionModel();
                                break;

                            /* Room and Stage Entity Data */
                            case "dzr":
                            case "dzs":
                                //Apparently Nintendo likes to mis-categorize files sometimes and put the wrong
                                //file format inside the wrong folder! We'll name-check dzr and dzs before loading
                                //them as they have fixed names (Room.*)
                                if (filePath.EndsWith(".dzr") || filePath.EndsWith(".dzs"))
                                    file = new WindWakerEntityData();
                                else
                                    file = new GenericArchiveData();
                                break;

                            /* 3D Model Formats */
                            case "bmd":
                            case "bdl":
                            case "bck":
                            case "brk":
                            case "btk":
                                file = new JStudioModel();
                                break;

                            default:
                                Console.WriteLine("Unknown folder " + folder +
                                                  " found. Creating GenericData holder for it!");
                                file = new GenericArchiveData();
                                break;
                        }

                        file.Load(fileData);
                        file.FileName = Path.GetFileName(filePath);
                        file.FolderName = new DirectoryInfo(folder).Name;
                        file.ParentArchive = this;

                        //Now that we've created the appropriate file (and hopefully mapped them all out!) we'll just stick
                        //it in our list of loaded files. They can later be gotten with the templated getter!
                        _archiveFiles.Add(file);
                        br.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error opening file " + filePath + " for reading. Error Message: " + ex);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the first file IArchiveFile derived class or null if no files of that exists.
        /// Use GetAllFilesByType&lt;T&gt; if there may be multiple of a file (as is the case with models)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFileByType<T>() where T : class
        {
            foreach (BaseArchiveFile file in _archiveFiles)
            {
                if (file is T)
                    return file as T;
            }

            return default(T);
        }


        public List<T> GetAllFilesByType<T>() where T : class
        {
            List<T> returnList = new List<T>();
            foreach (BaseArchiveFile file in _archiveFiles)
            {
                if (file is T)
                    returnList.Add(file as T);
            }

            return returnList;
        }
    }
}
