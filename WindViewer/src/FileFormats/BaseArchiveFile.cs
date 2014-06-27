using System.IO;

namespace WindViewer.FileFormats
{
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
