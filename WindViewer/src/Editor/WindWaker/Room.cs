using System.Linq;
using GameFormatReader.Common;
using OpenTK;
using WindViewer.FileFormats;

namespace WindViewer.Editor.WindWaker
{
    public class Room : ZArchive
    {
        public Vector2 Translation;
        public HalfRotationSingle Rotation;

        public void PostLoad(WwEntData roomEntData, WwEntData stageEntData)
        {
            //Search the stage data for a MULT chunk.
            var mults = stageEntData.EntityData.OfType<WwEntData.RoomTransform>();
            foreach (WwEntData.RoomTransform roomTransform in mults)
            {
                if (roomTransform.RoomNumber == RoomNumber)
                {
                    Translation.X = roomTransform.TranslationX;
                    Translation.Y = roomTransform.TranslationY;
                    Rotation = roomTransform.Rotation;
                }
            }

        }
    }
}