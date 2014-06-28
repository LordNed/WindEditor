using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using WindViewer.FileFormats;
using WindViewer.Forms;

namespace WindViewer.Editor
{
    /// <summary>
    /// A Worldspace Project refers to a collection of Stages and Rooms. This class acts as meta-data about
    /// a project the user is working on as nothing in here will get compiled into the actual archive. However
    /// it allows us to easily ekep track of which stages/rooms the user has open and their associated file
    /// structure.
    /// </summary>
    public class WorldspaceProject
    {
        //This refers to the Stage these files belong too. NULL if no stage exists.
        public ZArchive Stage { get; private set; }

        //This is a list of currently loaded Rooms. Returns a list of length zero if no rooms are loaded.
        //Does not include the Stage archive. If you wish to include the Stage archive, call GetAllArchives()
        public List<ZArchive> Rooms { get; private set; }

        //This is the name of Worldspace Project, sans .wrkDir extension. Max 8 chars.
        //Used by game to hold Stage/Room .arcs
        public string Name;

        //Absolute file path (ie: C:\..MiniHyo.wrkDir) of the project director
        public string ProjectFilePath;

        public WorldspaceProject()
        {
            Stage = null;
            Rooms = new List<ZArchive>();
        }

        public void SaveAllArchives()
        {
            foreach (ZArchive archive in GetAllArchives())
            {
                archive.Save(Path.Combine(ProjectFilePath, archive.Name));
            }
        }

        public List<ZArchive> GetAllArchives()
        {
            List<ZArchive> archive = new List<ZArchive>(Rooms);
            if (Stage != null)
                archive.Add(Stage);

            return archive;
        }

        /// <summary>
        /// This will create a new WorldspaceProject from an existing working directory.
        /// </summary>
        /// <param name="dirFilePath">A filepath that ends in ".wrkDir" that is the root folder of the project.</param>
        public void LoadFromDirectory(string dirFilePath)
        {
            //Name (sans .wrkDir)
            string wrkDirName = new DirectoryInfo(dirFilePath).Name;
            Name = wrkDirName.Substring(0, wrkDirName.LastIndexOf(".wrkDir"));

            ProjectFilePath = dirFilePath;

            //We're going to scan for folders in this directory and construct ZArchives out of their contents.
            string[] subFolders = Directory.GetDirectories(dirFilePath);

            //We'll generate a ZArchive for each subfolder and load the ZArchive with their contents
            foreach (string folder in subFolders)
            {
                ZArchive arc = new ZArchive();
                arc.LoadFromDirectory(folder);
               
                //Check to see if this is a stage (name starts with "Stage") or a Room ("Room")
                string folderName = new DirectoryInfo(folder).Name;
                arc.Name = folderName;

                if (folderName.ToLower().StartsWith("stage"))
                {
                    Console.WriteLine("Loaded Stage for " + Name);
                    Stage = arc;
                    arc.RoomNumber = -1;
                }
                else if(folderName.ToLower().StartsWith("room"))
                {
                    Console.WriteLine("Loading \"" + folderName + "\" as Room for " + Name);
                    Rooms.Add(arc);

                    // In a 'Stage', there is data that is indexed by Room number. The actual rooms don't store
                    // this data internally, it is only by file name. So we're going to strip apart the filename
                    // to get the room number. If we can't get the room from the filename (ie: user has renamed
                    // archive) then we'll just ask them.
                    int roomNumber = 0;
                    
                    //If it starts with "Room" then it's (probably) a Windwaker Archive.
                    if (folderName.Substring(0, 4).ToLower() == "room")
                    {
                        //Use Regex here to grab what is between "Room" and ".arc", since it goes up to "Room23.arc"
                        string[] numbers = Regex.Split(folderName, @"\D+");
                        string trimmedNumbers = String.Join("", numbers);
                        trimmedNumbers = trimmedNumbers.Trim();

                        roomNumber = int.Parse(trimmedNumbers);
                    }
                    //If it starts with R ("Rxx_00, xx being Room Number"), it's Twilight Princess
                    else if (folderName.Substring(0, 1).ToLower() == "r")
                    {
                        //I *think* these follow the Rxx_00 pattern, where xx is the room number. _00 can change, xx might be 1 or 3, who knows!

                        //We're going to use RegEx here to make sure we only grab what is between R and _00 which could be multipl.e
                        string[] numbers = Regex.Split(folderName.Substring(0, folderName.Length - 6), @"\D+");
                        string trimmedNumbers = String.Join("", numbers);
                        trimmedNumbers = trimmedNumbers.Trim();

                        roomNumber = int.Parse(trimmedNumbers);
                    }
                    else
                    {
                        InvalidRoomNumberPopup popup = new InvalidRoomNumberPopup();
                        popup.SetFailedRoomDescription(folderName);
                        popup.ShowDialog(MainEditor.ActiveForm);

                        roomNumber = (int)popup.roomNumberSelector.Value;
                    }

                    arc.RoomNumber = roomNumber;
                }
                else
                {
                    Console.WriteLine("Found non-stage and non-room archive \"{0}\" in WorldspaceProject {1}!", folderName, Name);
                }
            }

        }
    }

    /// <summary>
    /// This is metadata to go with the WorldspaceProject. Each ZArchive describes one (eventual) .arc file.
    /// It holds a list of BaseArchiveFiles, which describe a file (room.dzb, model1.btk, etc.) that the archive
    /// contains. 
    /// </summary>
    public class ZArchive
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
                        Console.WriteLine("Error saving file {0} to {1}! Error: {3}", archiveFile.FileName, subFolder, ex);
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
            if (!Directory.Exists(directory))
                new Exception("Invalid directory specified for WorldspaceProject.");

            //Get all of the sub folders (bdl, btk, etc.)
            string[] subFolders = Directory.GetDirectories(directory);

            foreach (string folder in subFolders)
            {
                //Then grab all of the files that are inside this folder and we'll load each one.
                string[] subFiles = Directory.GetFiles(folder);

                foreach (string filePath in subFiles)
                {
                    BinaryReader br = new BinaryReader(File.OpenRead(filePath));
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
                            file = new JStudioModel();
                            break;
                
                        case "tex":
                            file = new BinaryTextureImage();
                            break;

                        case "bck":
                        case "brk":
                        case "btk":
                        default:
                            Console.WriteLine("Unknown file extension {0} found ({1}). Creating GenericData holder for it!", Path.GetExtension(filePath), Path.GetFileName(filePath));
                            file = new GenericArchiveData();
                            break;
                    }

                    file.FileName = Path.GetFileName(filePath);
                    file.FolderName = new DirectoryInfo(folder).Name;
                    file.ParentArchive = this;
                    file.Load(fileData);

                    //Now that we've created the appropriate file (and hopefully mapped them all out!) we'll just stick
                    //it in our list of loaded files. They can later be gotten with the templated getter!
                    _archiveFiles.Add(file);
                    br.Close();
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

    public abstract class BaseArchiveFile
    {
        //What folder does this get saved into (dzb, dzr, etc.)
        public string FolderName;

        //What the file name was (room.dzb, model1.btk, etc.)
        public string FileName;

        //Reference to the parent ZArchive that this file belongs to
        public ZArchive ParentArchive;

        public abstract void Load(byte[] data);
        public abstract void Save(BinaryWriter stream);
    }
}
