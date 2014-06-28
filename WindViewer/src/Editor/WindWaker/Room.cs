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

        public Room()
        {
            Translation = Vector2.Zero;
            Rotation = new HalfRotationSingle();
        }

        public void PostLoad(WwEntData roomEntData, WwEntData stageEntData)
        {
            //Search the stage data for a MULT chunk.
            foreach (var chunk in stageEntData.EntityData)
            {
                WwEntData.RoomTransform roomTransform = chunk as WwEntData.RoomTransform;
                if(roomTransform == null)
                    continue;

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