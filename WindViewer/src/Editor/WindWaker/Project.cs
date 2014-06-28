using System.IO;
using WindViewer.FileFormats;

namespace WindViewer.Editor.WindWaker
{
    public class Project : WorldspaceProject
    {
        public override void LoadFromDirectory(string dirFilePath)
        {
            base.LoadFromDirectory(dirFilePath);

            /* Once we have loaded all of the sub-folders/files into their various
             * formats, we're going to pull that data out and store it in an 
             * editor-centric way instead of a file format centric way. */

            //ToDo: This is temp until we finish porting all over.

            //Stage: Snag its ent data
            string folderPath = Path.Combine(ProjectFilePath, "Stage/dzs/");
            if (!File.Exists(folderPath + "stage.dzs"))
                return;

            WwEntData stageEntData = new WwEntData(folderPath + "stage.dzs");

            foreach (ZArchive zArchive in Rooms)
            {
                Room room = (Room) zArchive;

                //Room: Snag our alternative WWEntData for now.
                folderPath = Path.Combine(ProjectFilePath, room.Name);
                folderPath = Path.Combine(folderPath, "dzr/");
                if(!File.Exists(folderPath + "room.dzr"))
                    continue;

                WwEntData roomEntData = new WwEntData(folderPath + "room.dzr");

                room.PostLoad(roomEntData, stageEntData);
            }
            
        }
    }
}