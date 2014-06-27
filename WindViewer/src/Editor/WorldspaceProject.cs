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
}
