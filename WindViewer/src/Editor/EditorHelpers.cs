using System;
using System.Drawing;

namespace WindViewer.Editor
{
    public static class EditorHelpers
    {
        public enum EntityLayer
        {
            DefaultLayer = -1,
            Zero = 0, One = 1, Two = 2, Three = 3, Four = 4,
            Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9,
            A = 10, B = 11, C = 12, D = 13, E = 14, F = 15,
            Invalid = 16,
        }

        public static string LayerIdToString(EntityLayer layerId)
        {
            switch (layerId)
            {
                case EntityLayer.DefaultLayer: return "Default Layer";
                case EntityLayer.Zero: return "Layer 0";
                case EntityLayer.One: return "Layer 1";
                case EntityLayer.Two: return "Layer 2";
                case EntityLayer.Three: return "Layer 3";
                case EntityLayer.Four: return "Layer 4";
                case EntityLayer.Five: return "Layer 5";
                case EntityLayer.Six: return "Layer 6";
                case EntityLayer.Seven: return "Layer 7";
                case EntityLayer.Eight: return "Layer 8";
                case EntityLayer.Nine: return "Layer 9";
                case EntityLayer.A: return "Layer A";
                case EntityLayer.B: return "Layer B";
                case EntityLayer.C: return "Layer C";
                case EntityLayer.D: return "Layer D";
                case EntityLayer.E: return "Layer E";
                case EntityLayer.F: return "Layer F";

                default:
                    Console.WriteLine("WARNING: Unknown EntityLayer LayerID when converting to string! {0}", layerId);
                    return "UNKN";
            }
        }

        public static Color LayerIdToColor(EntityLayer layerId)
        {
            switch (layerId)
            {
                case EntityLayer.DefaultLayer: return Color.LightGreen;
                case EntityLayer.Zero: return Color.LightBlue;
                case EntityLayer.One: return Color.LightGoldenrodYellow;
                case EntityLayer.Two: return Color.LightPink;
                case EntityLayer.Three: return Color.LightSalmon;
                case EntityLayer.Four: return Color.LightSeaGreen;
                case EntityLayer.Five: return Color.LightSteelBlue;
                case EntityLayer.Six: return Color.LightSlateGray;
                case EntityLayer.Seven: return Color.LightCoral;
                case EntityLayer.Eight: return Color.LightSkyBlue;
                case EntityLayer.Nine: return Color.LightCyan;
                case EntityLayer.A: return Color.MediumAquamarine;
                case EntityLayer.B: return Color.MediumSeaGreen;
                case EntityLayer.C: return Color.MediumSpringGreen;
                case EntityLayer.D: return Color.MediumTurquoise;
                case EntityLayer.E: return Color.SlateBlue;
                case EntityLayer.F: return Color.MediumOrchid;

                default:
                    Console.WriteLine("WARNING: Unknown EntityLayer LayerID when converting to string! {0}", layerId);
                    return Color.Red;
            }
        }

        public static EntityLayer ConvertStringToLayerId(string lastChar)
        {
            switch (lastChar)
            {
                case "Layer 0": case "0": return EntityLayer.Zero;
                case "Layer 1": case "1": return EntityLayer.One;
                case "Layer 2": case "2": return EntityLayer.Two;
                case "Layer 3": case "3": return EntityLayer.Three;
                case "Layer 4": case "4": return EntityLayer.Four;
                case "Layer 5": case "5": return EntityLayer.Five;
                case "Layer 6": case "6": return EntityLayer.Six;
                case "Layer 7": case "7": return EntityLayer.Seven;
                case "Layer 8": case "8": return EntityLayer.Eight;
                case "Layer 9": case "9": return EntityLayer.Nine;
                case "Layer A": case "A": return EntityLayer.A;
                case "Layer B": case "B": return EntityLayer.B;
                case "Layer C": case "C": return EntityLayer.C;
                case "Layer D": case "D": return EntityLayer.D;
                case "Layer E": case "E": return EntityLayer.E;
                case "Layer F": case "F": return EntityLayer.F;
                case "Default Layer": return EntityLayer.DefaultLayer;

                default:
                    Console.WriteLine("WARNING: Failed to convert ACT*/SCO* chunk to layer! Last Char: " + lastChar);
                    return EntityLayer.Invalid;
            }
        }
    }
}
