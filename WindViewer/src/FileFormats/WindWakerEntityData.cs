using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using OpenTK;
using WindViewer.Editor;
using WindViewer.Forms.EntityEditors;
using WindViewer.src.Forms.EntityEditors;

namespace WindViewer.FileFormats
{
    /// <summary>
    /// This applies to two types of files in WindWaker archives. Both the "DZR" and "DZS" files use the
    /// same format, so we create one file to work with both. "DZR" = "Zelda Room Data" while "DZS" = "Zelda
    /// Stage Data".
    /// </summary>
    public class WindWakerEntityData : BaseArchiveFile
    {
        private Dictionary<Type, List<BaseChunk>> _chunkList;
 
        public override void Load(byte[] data)
        {
            int offset = 0;
            FileHeader header = new FileHeader();
            header.Load(data, ref offset);

            _chunkList = new Dictionary<Type, List<BaseChunk>>();

            for (int i = 0; i < header.ChunkCount; i++)
            {
                ChunkHeader chunkHeader = new ChunkHeader();
                chunkHeader.Load(data, ref offset);

                for (int k = 0; k < chunkHeader.ElementCount; k++)
                {
                    BaseChunk chunk; 

                    switch (chunkHeader.Tag.Substring(0, 3).ToUpper())
                    {
                        case "ENV": chunk = new EnvrChunk(); break; 
                        case "COL": chunk = new ColoChunk(); break;
                        case "PAL": chunk = new PaleChunk(); break;
                        case "VIR": chunk = new VirtChunk(); break;
                        case "SCL": chunk = new SclsChunk(); break;
                        case "PLY": chunk = new PlyrChunk(); break;
                        case "RPA": chunk = new RPATChunk(); break;
                        case "PAT": chunk = new PathChunk(); break;
                        case "RPP": chunk = new RppnChunk(); break;
                        case "PPN": chunk = new PpntChunk(); break;
                        case "SON": chunk = new SondChunk(); break;
                        case "FIL": chunk = new FiliChunk(); break;
                        case "MEC": chunk = new MecoChunk(); break;
                        case "MEM": chunk = new MemaChunk(); break;
                        case "TRE": chunk = new TresChunk(); break;
                        case "SHI": chunk = new ShipChunk(); break;
                        case "MUL": chunk = new MultChunk(); break;
                        case "LGH": chunk = new LghtChunk(); break;
                        case "LGT": chunk = new LgtvChunk(); break;
                        case "RAR": chunk = new RaroChunk(); break;
                        case "ARO": chunk = new ArobChunk(); break;
                        case "EVN": chunk = new EvntChunk(); break;
                        case "TGO": chunk = new TgobChunk(); break;
                        case "ACT": 
                            chunk = new ActrChunk();
                            if (!chunkHeader.Tag.ToUpper().EndsWith("R"))
                            {
                                chunk.ChunkLayer = EditorHelpers.ConvertStringToLayerId(chunkHeader.Tag.ToUpper().Substring(3, 1));
                            }
                            break;
                        case "SCO": 
                            chunk = new ScobChunk();
                            if (!chunkHeader.Tag.EndsWith("B"))
                            {
                                chunk.ChunkLayer = EditorHelpers.ConvertStringToLayerId(chunkHeader.Tag.ToUpper().Substring(3, 1));
                            }
                            break;
                        case "STA": chunk = new StagChunk(); break;
                        case "RCA": chunk = new RcamChunk(); break;
                        case "CAM": chunk = new CamrChunk(); break;
                        case "FLO": chunk = new FlorChunk(); break;
                        case "TWO": chunk = new TwoDChunk(); break;
                        case "2DM": chunk = new TwoDMAChunk(); break;
                        case "DMA": chunk = new DMAPChunk(); break;
                        case "LBN": chunk = new LbnkChunk(); break;
                        case "TGD": chunk = new TgdrChunk(); break;
                        case "RTB": chunk = new RTBLChunk(); break;
                        
                        default:
                            Console.WriteLine("Unsupported Chunk Tag: " + chunkHeader.Tag + " Chunk will not be saved!");
                            chunk = null;
                            break;
                    }

                    if(chunk == null)
                        continue;

                    //Console.WriteLine(chunkHeader.Tag + " offset: " + chunkHeader.ChunkOffset);
                    chunk.LoadData(data, ref chunkHeader.ChunkOffset);
                    AddChunk(chunk);
                }
            }
        }

        public void AddChunk(BaseChunk chunk)
        {
            if (!_chunkList.ContainsKey(chunk.GetType()))
            {
                _chunkList.Add(chunk.GetType(), new List<BaseChunk>());
            }

            _chunkList[chunk.GetType()].Add(chunk);
        }

        public Dictionary<Type, List<BaseChunk>> GetAllChunks()
        {
            return _chunkList;
        }

        

        public override void Save(BinaryWriter stream)
        {
            foreach (KeyValuePair<Type, List<BaseChunk>> keyValuePair in GetAllChunks())
            {
                if (keyValuePair.Key == typeof (LgtvChunk))
                {
                    foreach (BaseChunk chunk in keyValuePair.Value)
                    {
                        chunk.WriteData(stream);
                    }
                }
            }
        }

        #region File Formats
        class FileHeader
        {
            public int ChunkCount = 0;

            public void Load(byte[] data, ref int srcOffset)
            {
                ChunkCount = FSHelpers.Read32(data, srcOffset);
                srcOffset += 4;
            }

            public void Save(BinaryWriter stream)
            {
                FSHelpers.Write32(stream, ChunkCount);
            }
        }

        class ChunkHeader
        {
            //ASCII Name for Chunk
            public string Tag;
            //How many of this Chunk type
            public int ElementCount;
            //Offset from beginning of file to first element
            public int ChunkOffset;

            public ChunkHeader()
            {
                Tag = "OOPS"; //For chunks someone forgot to name
                ElementCount = 0;
                ChunkOffset = 0;
            }

            public void Load(byte[] data, ref int srcOffset)
            {
                Tag = FSHelpers.ReadString(data, srcOffset, 4); //Tag is 4 bytes in length.
                ElementCount = FSHelpers.Read32(data, srcOffset + 4);
                ChunkOffset = FSHelpers.Read32(data, srcOffset + 8);

                srcOffset += 12; //Header is 0xC/12 bytes in length
            }

            public void Save(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Tag, 4);
                FSHelpers.Write32(stream, ElementCount);
                FSHelpers.Write32(stream, ChunkOffset);
            }
        }

        /// <summary>
        /// All chunks derive from this baseclass which carries
        /// metadata about the chunk, plus a shared load/save 
        /// interface.
        /// </summary>
        public abstract class BaseChunk
        {
            protected BaseChunk(string chunkName, string chunkDescription)
            {
                ChunkName = chunkName;
                ChunkDescription = chunkDescription;
            }

            //Name of the Chunk (SCOB, VIRT, etc, etc.)
            public string ChunkName { get; private set; }

            //Long-form description of the chunk.
            public string ChunkDescription { get; private set; }

            //Layer this chunk belongs to (or -1 for default/no layer)
            public EditorHelpers.EntityLayer ChunkLayer = EditorHelpers.EntityLayer.DefaultLayer;

            protected BaseChunk()
            {
                ChunkName = "????";
                ChunkDescription = "Unknown Chunk";
            }

            public abstract void LoadData(byte[] data, ref int srcOffset);
            public abstract void WriteData(BinaryWriter stream);
        }

        /// <summary>
        /// This is an extended version of the BaseChunk that
        /// holds positional/rotational data for chunks who have
        /// physical prescence in the world (player spawns, etc.)
        /// </summary>
        public abstract class BaseChunkSpatial : BaseChunk
        {
            public Transform Transform;

            protected BaseChunkSpatial(string chunkName, string chunkDescription) : base(chunkName, chunkDescription)
            {
                Transform = new Transform();
            }
        }
        #endregion

        #region Chunk Types
        /// <summary>
        /// The Envr (short for Environment) chunk contains indexes of different color pallets
        ///  to use in different weather situations. 
        /// </summary>
        public class EnvrChunk : BaseChunk
        {
            public byte ClearColorIndexA; //Index of the Color entry to use for clear weather.
            public byte RainingColorIndexA; //There's two sets, A and B. B's usage is unknown but identical.
            public byte SnowingColorIndexA;
            public byte UnknownColorIndexA; //We don't know what weather this color is used for!

            public byte ClearColorIndexB;
            public byte RainingColorIndexB;
            public byte SnowingColorIndexB;
            public byte UnknownColorIndexB;

            public EnvrChunk() : base("ENVR", "Environment") { }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                ClearColorIndexA = FSHelpers.Read8(data, srcOffset + 0);
                RainingColorIndexA = FSHelpers.Read8(data, srcOffset + 1);
                SnowingColorIndexA = FSHelpers.Read8(data, srcOffset + 2);
                UnknownColorIndexA = FSHelpers.Read8(data, srcOffset + 3);

                ClearColorIndexB = FSHelpers.Read8(data, srcOffset + 4);
                RainingColorIndexB = FSHelpers.Read8(data, srcOffset + 5);
                SnowingColorIndexB = FSHelpers.Read8(data, srcOffset + 6);
                UnknownColorIndexB = FSHelpers.Read8(data, srcOffset + 7);

                srcOffset += 8;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write8(stream, ClearColorIndexA);
                FSHelpers.Write8(stream, RainingColorIndexA);
                FSHelpers.Write8(stream, SnowingColorIndexA);
                FSHelpers.Write8(stream, UnknownColorIndexA);

                FSHelpers.Write8(stream, ClearColorIndexB);
                FSHelpers.Write8(stream, RainingColorIndexB);
                FSHelpers.Write8(stream, SnowingColorIndexB);
                FSHelpers.Write8(stream, UnknownColorIndexB);
            }
        }

        /// <summary>
        /// Colo (short for Color) contains indexes into the Pale section. Color specifies
        /// which color to use for the different times of day.
        /// </summary>
        public class ColoChunk : BaseChunk
        {
            public byte DawnIndex; //Index of the Pale entry to use for Dawn
            public byte MorningIndex;
            public byte NoonIndex;
            public byte AfternoonIndex;
            public byte DuskIndex;
            public byte NightIndex;

            public ColoChunk()
                : base("COLO", "Color")
            {
                DawnIndex = MorningIndex = NoonIndex = AfternoonIndex = DuskIndex = NightIndex = 0;
            }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                DawnIndex = FSHelpers.Read8(data, srcOffset + 0);
                MorningIndex = FSHelpers.Read8(data, srcOffset + 1);
                NoonIndex = FSHelpers.Read8(data, srcOffset + 2);
                AfternoonIndex = FSHelpers.Read8(data, srcOffset + 3);
                DuskIndex = FSHelpers.Read8(data, srcOffset + 4);
                NightIndex = FSHelpers.Read8(data, srcOffset + 5);

                srcOffset += 6;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write8(stream, DawnIndex);
                FSHelpers.Write8(stream, MorningIndex);
                FSHelpers.Write8(stream, NoonIndex);
                FSHelpers.Write8(stream, AfternoonIndex);
                FSHelpers.Write8(stream, DuskIndex);
                FSHelpers.Write8(stream, NightIndex);
            }
        }

        /// <summary>
        /// The Pale (short for Palette) chunk contains the actual RGB colors for different
        /// types of lighting. 
        /// </summary>
        public class PaleChunk : BaseChunk
        {
            public ByteColor ActorAmbient;
            public ByteColor ShadowColor;
            public ByteColor RoomFillColor;
            public ByteColor RoomAmbient;
            public ByteColor WaveColor;
            public ByteColor OceanColor;
            public ByteColor UnknownColor1; //Unknown
            public ByteColor UnknownColor2; //Unknown
            public ByteColor DoorwayColor; //Tints the 'Light' mesh behind doors for entering/exiting to the exterior
            public ByteColor UnknownColor3; //Unknown
            public ByteColor FogColor;

            public byte VirtIndex; //Index of the Virt entry to use for Skybox Colors

            public ByteColorAlpha OceanFadeInto;
            public ByteColorAlpha ShoreFadeInto;

            public PaleChunk()
                : base("PALE", "Palette")
            {
                ActorAmbient = new ByteColor();
                ShadowColor = new ByteColor();
                RoomFillColor = new ByteColor();
                RoomAmbient = new ByteColor();
                WaveColor = new ByteColor();
                OceanColor = new ByteColor();
                UnknownColor1 = new ByteColor();
                UnknownColor2 = new ByteColor();
                DoorwayColor = new ByteColor();
                UnknownColor3 = new ByteColor();
                FogColor = new ByteColor();

                VirtIndex = 0;

                OceanFadeInto = new ByteColorAlpha();
                ShoreFadeInto = new ByteColorAlpha();
            }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                ActorAmbient = new ByteColor(data, ref srcOffset);
                ShadowColor = new ByteColor(data, ref srcOffset);
                RoomFillColor = new ByteColor(data, ref srcOffset);
                RoomAmbient = new ByteColor(data, ref srcOffset);
                WaveColor = new ByteColor(data, ref srcOffset);
                OceanColor = new ByteColor(data, ref srcOffset);
                UnknownColor1 = new ByteColor(data, ref srcOffset); //Unknown
                UnknownColor2 = new ByteColor(data, ref srcOffset); //Unknown
                DoorwayColor = new ByteColor(data, ref srcOffset);
                UnknownColor3 = new ByteColor(data, ref srcOffset); //Unknown
                FogColor = new ByteColor(data, ref srcOffset);

                VirtIndex = FSHelpers.Read8(data, srcOffset);
                srcOffset += 3; //Read8 + 2 Padding

                OceanFadeInto = new ByteColorAlpha(data, ref srcOffset);
                ShoreFadeInto = new ByteColorAlpha(data, ref srcOffset);
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteArray(stream, ActorAmbient.GetBytes());
                FSHelpers.WriteArray(stream, ShadowColor.GetBytes());
                FSHelpers.WriteArray(stream, RoomFillColor.GetBytes());
                FSHelpers.WriteArray(stream, RoomAmbient.GetBytes());
                FSHelpers.WriteArray(stream, WaveColor.GetBytes());
                FSHelpers.WriteArray(stream, OceanColor.GetBytes());
                FSHelpers.WriteArray(stream, UnknownColor1.GetBytes()); //Unknown
                FSHelpers.WriteArray(stream, UnknownColor2.GetBytes()); //Unknown
                FSHelpers.WriteArray(stream, DoorwayColor.GetBytes());
                FSHelpers.WriteArray(stream, UnknownColor3.GetBytes()); //Unknown

                FSHelpers.WriteArray(stream, FogColor.GetBytes());
                FSHelpers.Write8(stream, VirtIndex);
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0x0000, 2));//Two bytes padding on Virt Index

                FSHelpers.WriteArray(stream, OceanFadeInto.GetBytes());
                FSHelpers.WriteArray(stream, ShoreFadeInto.GetBytes());
            }
        }

        /// <summary>
        /// The Virt (short for uh.. Virtual? I dunno) chunk contains color data for the skybox. Indexed by a Pale
        /// chunk.
        /// </summary>
        public class VirtChunk : BaseChunk
        {
            public ByteColorAlpha HorizonCloudColor; //The Horizon
            public ByteColorAlpha CenterCloudColor;  //Directly above you
            public ByteColor CenterSkyColor;
            public ByteColor HorizonColor;
            public ByteColor SkyFadeTo; //Color to fade to from CenterSky. 

            public VirtChunk()
                : base("VIRT", "Skybox Lighting")
            {
                HorizonCloudColor = new ByteColorAlpha();
                CenterCloudColor = new ByteColorAlpha();
                CenterSkyColor = new ByteColor();
                HorizonColor = new ByteColor();
                SkyFadeTo = new ByteColor();
            }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                //First 16 bytes are 80 00 00 00 (repeated 4 times). Unknown why.
                srcOffset += 16;

                HorizonCloudColor = new ByteColorAlpha(data, ref srcOffset);
                CenterCloudColor = new ByteColorAlpha(data, ref srcOffset);

                CenterSkyColor = new ByteColor(data, ref srcOffset);
                HorizonColor = new ByteColor(data, ref srcOffset);
                SkyFadeTo = new ByteColor(data, ref srcOffset);

                //More apparently unused bytes.
                srcOffset += 3;
            }

            public override void WriteData(BinaryWriter stream)
            {
                //Fixed values that doesn't seem to change.
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0x80000000, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0x80000000, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0x80000000, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0x80000000, 4));

                FSHelpers.WriteArray(stream, HorizonCloudColor.GetBytes());
                FSHelpers.WriteArray(stream, CenterCloudColor.GetBytes());
                FSHelpers.WriteArray(stream, CenterSkyColor.GetBytes());
                FSHelpers.WriteArray(stream, HorizonColor.GetBytes());
                FSHelpers.WriteArray(stream, SkyFadeTo.GetBytes());

                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFFFF, 3)); //3 Bytes Padding
            }
        }

        /// <summary>
        /// The SCLS Chunk defines information about exits on a map. It is pointed to by
        /// the maps collision data (which supplies the actual positions)
        /// </summary>
        [EntEditorType(typeof(ExitEditor))]
        public class SclsChunk : BaseChunk
        {
            [DisplayName]
            public string DestinationName;
            public byte SpawnNumber;
            public byte DestinationRoomNumber;
            public byte ExitType;
            public byte UnknownPadding;

            public SclsChunk()
                : base("SCLS", "Exits")
            {
                DestinationName = "INVALID";
            }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                DestinationName = FSHelpers.ReadString(data, srcOffset, 8);
                SpawnNumber = FSHelpers.Read8(data, srcOffset + 8);
                DestinationRoomNumber = FSHelpers.Read8(data, srcOffset + 9);
                ExitType = FSHelpers.Read8(data, srcOffset + 10);
                UnknownPadding = FSHelpers.Read8(data, srcOffset + 11);

                srcOffset += 12;
            }


            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, DestinationName, 8);
                FSHelpers.Write8(stream, SpawnNumber);
                FSHelpers.Write8(stream, DestinationRoomNumber);
                FSHelpers.Write8(stream, ExitType);
                FSHelpers.Write8(stream, UnknownPadding);
            }
        }

        /// <summary>
        /// The Plyr (Player) chunk defines spawn points for Link.
        /// </summary>
        [EntEditorType(typeof(PlayerEditor))]
        public class PlyrChunk : BaseChunkSpatial
        {
            [DisplayName]
            public string Name; //"Link"
            public byte EventIndex; //Spcifies an event from the DZS file to play upon spawn. FF = no event.
            public byte Unknown1; //Padding?
            public byte SpawnType; //How Link enters the room.
            public byte RoomNumber; //Room number the spawn is in.
            //public Vector3 Position;
            public HalfRotation Rotation; //Temp

            public PlyrChunk(): base("PLYR", "Player Spawns")
            {
                Name = "Link";
            }


            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Name = FSHelpers.ReadString(data, srcOffset, 8);
                EventIndex = FSHelpers.Read8(data, srcOffset + 8);
                Unknown1 = FSHelpers.Read8(data, srcOffset + 9);
                SpawnType = FSHelpers.Read8(data, srcOffset + 10);
                RoomNumber = FSHelpers.Read8(data, srcOffset + 11);

                Vector3 position = new Vector3();
                position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));
                Transform.Position = position;

                srcOffset += 24;
                Rotation = new HalfRotation(data, ref srcOffset);
   
                srcOffset += 2; //Two bytes Padding
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Name, 8);
                FSHelpers.Write8(stream, EventIndex);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write8(stream, SpawnType);
                FSHelpers.Write8(stream, RoomNumber);
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);
                FSHelpers.Write16(stream, (ushort)Rotation.X);
                FSHelpers.Write16(stream, (ushort)Rotation.Y);
                FSHelpers.Write16(stream, (ushort)Rotation.Z);

                //Padding.
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFF, 2));
            }
        }

        ///<summary>
        ///RPAT and Path are two chunks that put RPPN and PPNT chunk entries into groups.
        ///RPAT and RPPN are found in DZR files, while Path and PPNT are found in DZS files.
        ///</summary>
        [EntEditorType(typeof(PathEditor))]
        public class RPATChunk : BaseChunk
        {
            public ushort NumPoints;
            public ushort Unknown1; //Probably padding
            public byte Unknown2; //More padding?
            public byte Unknown3; //Possibly not padding
            public ushort Padding; //ACTUAL PADDING!?
            public int FirstPointOffset; //Offset in the DZx file of the first waypoint in the group

            public RPATChunk():base("RPAT", "RPAT Paths")
            {
                InitDefaults();
            }

            public RPATChunk(string chunkName, string chunkDescription)
                : base(chunkName, chunkDescription)
            {
                InitDefaults();
            }

            private void InitDefaults()
            {
                NumPoints = 0;
                Unknown1 = BitConverter.ToUInt16(FSHelpers.ToBytes(0xFFFF, 2), 0);
                Unknown2 = 0xFF;
                Unknown3 = 0;
                Padding = 0;
                FirstPointOffset = 0;
            }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                NumPoints = (ushort)FSHelpers.Read16(data, srcOffset);
                Unknown1 = (ushort)FSHelpers.Read16(data, srcOffset + 2);
                Unknown2 = FSHelpers.Read8(data, srcOffset + 4);
                Unknown3 = FSHelpers.Read8(data, srcOffset + 5);
                Padding = (ushort)FSHelpers.Read16(data, srcOffset + 6);
                FirstPointOffset = FSHelpers.Read32(data, srcOffset + 8);

                srcOffset += 12;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write16(stream, NumPoints);
                FSHelpers.Write16(stream, Unknown1);
                FSHelpers.Write8(stream, Unknown2);
                FSHelpers.Write8(stream, Unknown3);
                FSHelpers.Write16(stream, Padding);
                FSHelpers.Write32(stream, FirstPointOffset);
            }
        }

        [EntEditorType(typeof(PathEditor))]
        public class PathChunk : RPATChunk
        {
            public PathChunk():base("PATH", "Path Paths"){}
        }

        [EntEditorType(typeof(EnvironmentSoundEditor))]
        public class SondChunk : BaseChunkSpatial
        {
            [DisplayName]
            public string Name; //Seems to always be "sndpath"
            //public Vector3 SourcePos; //Position the sound plays from
            public byte Unknown1; //Typically 00, one example had 08.
            public byte Padding;
            public byte Unknown2; //Typically FF, but Outset's entries have the room number (0x2C) here
            public byte SoundId;
            public byte SoundRadius;
            public byte Padding2;
            public byte Padding3;
            public byte Padding4;

            public SondChunk():base("SOND", "Sound") {}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Name = FSHelpers.ReadString(data, srcOffset, 8);

                Vector3 sourcePos = new Vector3();
                sourcePos.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 8));
                sourcePos.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                sourcePos.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Transform.Position = sourcePos;

                Unknown1 = FSHelpers.Read8(data, srcOffset + 20);
                Padding = FSHelpers.Read8(data, srcOffset + 21);
                Unknown2 = FSHelpers.Read8(data, srcOffset + 22);
                SoundId = FSHelpers.Read8(data, srcOffset + 23);
                SoundRadius = FSHelpers.Read8(data, srcOffset + 24);
                Padding2 = FSHelpers.Read8(data, srcOffset + 25);
                Padding3 = FSHelpers.Read8(data, srcOffset + 26);
                Padding4 = FSHelpers.Read8(data, srcOffset + 27);

                srcOffset += 28;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Name, 8);
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write8(stream, Padding);
                FSHelpers.Write8(stream, Unknown2);
                FSHelpers.Write8(stream, SoundId);
                FSHelpers.Write8(stream, SoundRadius);
                FSHelpers.Write8(stream, Padding2);
                FSHelpers.Write8(stream, Padding3);
                FSHelpers.Write8(stream, Padding4);
            }
        }

        [EntEditorType(typeof(DungeonFloorEditor))]
        public class FlorChunk : BaseChunk
        {
            public float LowerBoundaryYCoord; //Y value of the lower boundary of a floor. When link crosses the coord, the map switches him to being on that floor.
            public byte FloorId; //????
            public byte[] IncludedRooms;

            public FlorChunk():base("FLOR", "Dungeon Floors"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                LowerBoundaryYCoord = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset));
                FloorId = FSHelpers.Read8(data, srcOffset + 4);
                IncludedRooms = new byte[15];
                for (int i = 0; i < 15; i++)
                    IncludedRooms[i] = FSHelpers.Read8(data, srcOffset + 5 + i);

                srcOffset += 20;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, LowerBoundaryYCoord);
                FSHelpers.Write8(stream, FloorId);

                for (int i = 0; i < 15; i++)
                    FSHelpers.Write8(stream, IncludedRooms[i]);
            }
        }

        [EntEditorType(typeof(RoomEnvironmentEditor))]
        public class FiliChunk : BaseChunk
        {
            public byte TimePassage;
            public byte WindSettings;
            public byte Unknown1;
            public byte LightingType; //04 is normal, 05 is shadowed.
            public float Unknown2;

            public FiliChunk():base("FILI", "Misc."){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                TimePassage = FSHelpers.Read8(data, srcOffset + 0);
                WindSettings = FSHelpers.Read8(data, srcOffset + 1);
                Unknown1 = FSHelpers.Read8(data, srcOffset + 2);
                LightingType = FSHelpers.Read8(data, srcOffset + 3);

                Unknown2 = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                srcOffset += 8;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write8(stream, TimePassage);
                FSHelpers.Write8(stream, WindSettings);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write8(stream, LightingType);
                FSHelpers.WriteFloat(stream, Unknown2);
            }
        }

        [EntEditorType(typeof(CameraBehaviorEditor))]
        public class RcamChunk : BaseChunk
        {
            [DisplayName]
            public string CameraType;
            public byte RaroIndex;
            public byte Padding1;
            public byte Padding2;
            public byte Padding3;
            

            public RcamChunk(string chunkName, string chunkUsage):base(chunkName, chunkUsage){}
            public RcamChunk():base("RCAM", "Camera Usage"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                CameraType = FSHelpers.ReadString(data, srcOffset, 16);
                RaroIndex = FSHelpers.Read8(data, srcOffset + 16);
                Padding1 = FSHelpers.Read8(data, srcOffset + 17);
                Padding2 = FSHelpers.Read8(data, srcOffset + 18);
                Padding3 = FSHelpers.Read8(data, srcOffset + 19);
                srcOffset += 20;
            }


            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, CameraType, 16);
                FSHelpers.Write8(stream, RaroIndex);
                FSHelpers.Write8(stream, Padding1);
                FSHelpers.Write8(stream, Padding2);
                FSHelpers.Write8(stream, Padding3);
            }
        }

        [EntEditorType(typeof(CameraBehaviorEditor))]
        public class CamrChunk : RcamChunk
        {
            public CamrChunk():base("CAMR", "Camera Usage"){}
        }

        [EntEditorType(typeof(MECOEditor))]
        public class MecoChunk : BaseChunk
        {
            public byte RoomNumber; //Which room number this applies to
            public byte MemaIndex;  //Which index in the Mema array to use.

            public MecoChunk():base("MECO", "Memory Offset"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                RoomNumber = FSHelpers.Read8(data, srcOffset);
                MemaIndex = FSHelpers.Read8(data, srcOffset + 1);

                srcOffset += 2;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write8(stream, RoomNumber);
                FSHelpers.Write8(stream, MemaIndex);
            }

        }

        [EntEditorType(typeof(MEMAEditor))]
        public class MemaChunk : BaseChunk
        {
            public int MemSize; //Amount of memory to allocate for a room.

            public MemaChunk():base("MEMA", "Memory Allocation"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                MemSize = FSHelpers.Read32(data, srcOffset);
                srcOffset += 4;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write32(stream, (int)MemSize);
            }
        }

        [EntEditorType(typeof(TreasureChestEditor))]
        public class TresChunk : BaseChunkSpatial
        {
            [DisplayName]
            public string Name; //Usually Takara, 8 bytes + null terminator.
            public byte Unknown1;
            public ushort ChestType; //Big Key, Common Wooden, etc.
            //public Vector3 Position;
            public ushort Unknown2;
            public ushort YRotation; //Rotation on the Y axis
            public ushort ChestContents; //Rupees, Hookshot, etc.
            public ushort Padding;

            public TresChunk():base("TRES", "Treasure Chests (Non-Ocean)"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Name = FSHelpers.ReadString(data, srcOffset, 8);
                Unknown1 = FSHelpers.Read8(data, srcOffset + 9);
                ChestType = (ushort)FSHelpers.Read16(data, srcOffset + 10);
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));
                Unknown2 = (ushort)FSHelpers.Read16(data, srcOffset + 24);
                YRotation = (ushort)FSHelpers.Read16(data, srcOffset + 26);
                ChestContents = FSHelpers.Read8(data, srcOffset + 28);
                Unknown2 = (ushort)FSHelpers.Read32(data, srcOffset + 30);
                srcOffset += 32;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Name, 8);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write16(stream, ChestType);
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);
                FSHelpers.Write16(stream, Unknown2);
                FSHelpers.Write16(stream, YRotation);
                FSHelpers.Write16(stream, ChestContents);
                FSHelpers.Write32(stream, Padding);
            }
        }

        [EntEditorType(typeof(ShipSpawnEditor))]
        public class ShipChunk : BaseChunkSpatial
        {
            //public Vector3 Position;
            public ushort YRotation;
            public ushort Unknown;

            public ShipChunk():base("SHIP", "Ship Spawn Point"){}
            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 8));

                YRotation = (ushort)FSHelpers.Read16(data, srcOffset + 12);
                Unknown = (ushort) FSHelpers.Read16(data, srcOffset + 14);

                srcOffset += 16;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);

                FSHelpers.Write16(stream, YRotation);
                FSHelpers.Write16(stream, Unknown);
            }
        }

        [EntEditorType(typeof(PathWaypointEditor))]
        public class RppnChunk : BaseChunkSpatial
        {
            public uint Unknown1;
            //public Vector3 Position;

            public RppnChunk():base("RPPN", "Path Waypoint"){}
            public RppnChunk(string chunkName, string chunkDescription) : base(chunkName, chunkDescription){}
            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Unknown1 = (uint)FSHelpers.Read32(data, srcOffset);
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 8));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));

                srcOffset += 16;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write32(stream, (int)Unknown1);

                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);
            }
        }

        [EntEditorType(typeof(PathWaypointEditor))]
        public class PpntChunk : RppnChunk
        {
            public PpntChunk():base("PPNT", "Path Waypoint"){}
        }

        [EntEditorType(typeof(RoomPosEditor))]
        public class MultChunk : BaseChunk
        {
            public float TranslationX;
            public float TranslationY;
            public short YRotation;
            public byte RoomNumber;
            public byte Unknown;

            public MultChunk():base("MULT", "Room Position"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                TranslationX = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0));
                TranslationY = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                YRotation = FSHelpers.Read16(data, srcOffset + 8);
                RoomNumber = FSHelpers.Read8(data, srcOffset + 10);
                Unknown = FSHelpers.Read8(data, srcOffset + 11);


                srcOffset += 12;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, TranslationX);
                FSHelpers.WriteFloat(stream, TranslationY);
                FSHelpers.Write16(stream, (ushort)YRotation);
                FSHelpers.Write8(stream, RoomNumber);
                FSHelpers.Write8(stream, Unknown);
            }
        }

        [EntEditorType(typeof(InteriorLightEditor))]
        public class LghtChunk : BaseChunkSpatial
        {
            //public Vector3 Position;
            public Vector3 Scale; //Or Intensity
            public ByteColorAlpha Color;

            public LghtChunk():base("LGHT", "Interior Light Source"){}
            public LghtChunk(string chunkName, string chunkDescription):base(chunkName, chunkDescription){}
            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 8));

                Scale.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                Scale.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Scale.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));

                srcOffset += 24;
                Color = new ByteColorAlpha(data, ref srcOffset);
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);

                FSHelpers.WriteFloat(stream, Scale.X);
                FSHelpers.WriteFloat(stream, Scale.Y);
                FSHelpers.WriteFloat(stream, Scale.Z);

                FSHelpers.WriteArray(stream, Color.GetBytes());
            }
        }

        [EntEditorType(typeof(InteriorLightEditor))]
        public class LgtvChunk : LghtChunk
        {
            public LgtvChunk():base("LGTV", "Interior Light Source"){}
        }

        [EntEditorType(typeof(CameraWaypointEditor))]
        public class RaroChunk : BaseChunkSpatial
        {
            //public Vector3 Position;
            public HalfRotation Rotation;
            public short Padding;

            public RaroChunk():base("RARO", "Camera Ref Data"){}
            public RaroChunk(string chunkName, string chunkDescription):base(chunkName, chunkDescription){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 8));
                srcOffset += 12;
                
                Rotation = new HalfRotation(data, ref srcOffset);
                Padding = FSHelpers.Read16(data, srcOffset);
                srcOffset += 2;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);

                FSHelpers.Write16(stream, (ushort)Rotation.X);
                FSHelpers.Write16(stream, (ushort)Rotation.Y);
                FSHelpers.Write16(stream, (ushort)Rotation.Z);

                FSHelpers.Write16(stream, (ushort)Padding);
            }
        }

        [EntEditorType(typeof(CameraWaypointEditor))]
        public class ArobChunk : RaroChunk
        {
            public ArobChunk():base("AROB", "Camera Ref Data"){}
        }

        [EntEditorType(typeof(EventEditor))]
        public class EvntChunk : BaseChunk
        {
            public byte Unknown1;
            [DisplayName] public string EventName;
            public byte Unknown2;
            public byte Unknown3;
            public byte Unknown4;
            public byte Unknown5;
            public byte RoomNumber;
            public byte Padding1;
            public byte Padding2;
            public byte Padding3;


            public EvntChunk():base("EVNT", "Event"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Unknown1 = FSHelpers.Read8(data, srcOffset);
                EventName = FSHelpers.ReadString(data, srcOffset + 1, 15);
                Unknown2 = FSHelpers.Read8(data, srcOffset + 16);
                Unknown3 = FSHelpers.Read8(data, srcOffset + 17);
                Unknown4 = FSHelpers.Read8(data, srcOffset + 18);
                Unknown5 = FSHelpers.Read8(data, srcOffset + 19);
                RoomNumber = FSHelpers.Read8(data, srcOffset + 20);
                Padding1 = FSHelpers.Read8(data, srcOffset + 21);
                Padding2 = FSHelpers.Read8(data, srcOffset + 22);
                Padding3 = FSHelpers.Read8(data, srcOffset + 23);

                srcOffset += 24;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.WriteString(stream, EventName, 15);
                FSHelpers.Write8(stream, Unknown2);
                FSHelpers.Write8(stream, Unknown3);
                FSHelpers.Write8(stream, Unknown4);
                FSHelpers.Write8(stream, Unknown5);

                FSHelpers.Write8(stream, RoomNumber);

                FSHelpers.Write8(stream, Padding1);
                FSHelpers.Write8(stream, Padding2);
                FSHelpers.Write8(stream, Padding3);
            }
        }

        public class ActrChunk : BaseChunkSpatial
        {
            [DisplayName] public string Name;
            public byte Unknown1;
            public byte RpatIndex;
            public byte Unknown2;
            public byte BehaviorType;
            //public Vector3 Position;
            //public HalfRotation Rotation;

            public ushort EnemyNumber; //Unknown purpose. Enemies are given a number here based on their position in the actor list.
            
            public ActrChunk():base("ACTR", "Actor"){}
            public ActrChunk(string chunkName, string chunkDescription) : base(chunkName, chunkDescription) { }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Name = FSHelpers.ReadString(data, srcOffset, 8);
                Unknown1 = FSHelpers.Read8(data, srcOffset + 8);
                RpatIndex = FSHelpers.Read8(data, srcOffset + 9);
                Unknown2 = FSHelpers.Read8(data, srcOffset + 10);
                BehaviorType = FSHelpers.Read8(data, srcOffset + 11);

                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));

                srcOffset += 24;
                //Rotation = new HalfRotation(data, ref srcOffset);
                srcOffset += 6; //ToDo: Fix me

                EnemyNumber = (ushort)FSHelpers.Read16(data, srcOffset);

                srcOffset += 2; //Already got +24 from earlier, then +6 from HalfRotation.
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Name, 8);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write8(stream, RpatIndex);
                FSHelpers.Write8(stream, Unknown2);
                FSHelpers.Write8(stream, BehaviorType);
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);

                FSHelpers.Write16(stream, (ushort)255);
                FSHelpers.Write16(stream, (ushort)255);
                FSHelpers.Write16(stream, (ushort)255); //ToDo: Fix me

                FSHelpers.Write16(stream, EnemyNumber);
            }
        }

        public class TgobChunk : ActrChunk
        {
            public TgobChunk():base("TGOB", "Actor"){}
        }

        public class StagChunk : BaseChunk
        {
            public float MinDepth;
            public float MaxDepth;
            public ushort KeyCounterDisplay; //Seems to be a multi-use field?
            public ushort LoadedParticleBank; //Particle Bank to load for the worldspace. Unclear how this works exactly.
            public ushort ItemUsageAndMinimap; //Items link can use and what color the minimap backgorund is
            public byte Padding;
            public byte Unknown1;
            public byte Unknown2;
            public byte Unknown3;
            public ushort DrawDistance;

            public StagChunk():base("STAG", "Stage"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                MinDepth = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset));
                MaxDepth = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                KeyCounterDisplay = (ushort)FSHelpers.Read16(data, srcOffset + 8);
                LoadedParticleBank = (ushort)FSHelpers.Read16(data, srcOffset + 10);
                ItemUsageAndMinimap = (ushort) FSHelpers.Read16(data, srcOffset + 12);
                Padding = FSHelpers.Read8(data, srcOffset + 14);
                Unknown1 = FSHelpers.Read8(data, srcOffset + 15);
                Unknown2 = FSHelpers.Read8(data, srcOffset + 16);
                Unknown3 = FSHelpers.Read8(data, srcOffset + 17);
                DrawDistance = (ushort)FSHelpers.Read16(data, srcOffset + 18);

                srcOffset += 20;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, MinDepth);
                FSHelpers.WriteFloat(stream, MaxDepth);
                FSHelpers.Write16(stream, KeyCounterDisplay);
                FSHelpers.Write16(stream, LoadedParticleBank);
                FSHelpers.Write16(stream, ItemUsageAndMinimap);
                FSHelpers.Write8(stream, Padding);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write8(stream, Unknown2);
                FSHelpers.Write8(stream, Unknown3);
                FSHelpers.Write16(stream, DrawDistance);
            }
        }

        /// <summary>
        /// 2DMA holds the settings for the map display in the bottom left-hand corner of the screen.
        /// </summary>
        public class TwoDMAChunk : BaseChunk
        {
            public float FullMapImageScaleX;
            public float FullMapImageScaleY;
            public float FullMapSpaceScaleX;
            public float FullMapSpaceScaleY;
            public float FullMapXCoord;
            public float FullMapYCoord;
            public float ZoomedMapXScrolling1; //Something with scrolling, but that's also defined below?
            public float ZoomedMapYScrolling1; //Does something like scrolling on y-axis
            public float ZoomedMapXScrolling2;
            public float ZoomedMapYScrolling2;
            public float ZoomedMapXCoord;
            public float ZoomedMapYCoord;
            public float ZoomedMapScale; //That's what it appeared to affect, anyway
            public byte Unknown1; //Always 0x80?
            public byte MapIndex; //number of the map image to use. For instance, using the first image would be 80, the second 81, and so on.
            public byte Unknown2; //variable, but changing it has no immediate result
            public byte Padding;

            public TwoDMAChunk():base("2DMA", "Minimap"){}
            public TwoDMAChunk(string chunkName, string chunkDescription) : base(chunkName, chunkDescription) { }
            public override void LoadData(byte[] data, ref int srcOffset)
            {
                FullMapImageScaleX = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset));
                FullMapImageScaleY = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x4));
                FullMapSpaceScaleX = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x8));
                FullMapSpaceScaleY = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0xC));
                FullMapXCoord = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x10));
                FullMapYCoord = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x14));
                ZoomedMapXScrolling1 = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x18));
                ZoomedMapYScrolling1 = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x1C));
                ZoomedMapXScrolling2 = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x20));
                ZoomedMapYScrolling2 = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x24));
                ZoomedMapXCoord = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x28));
                ZoomedMapYCoord = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x2C));
                ZoomedMapScale = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x30));
                Unknown1 = FSHelpers.Read8(data, srcOffset + 0x34);
                MapIndex = FSHelpers.Read8(data, srcOffset + 0x35);
                Unknown2 = FSHelpers.Read8(data, srcOffset + 0x36);
                Padding = FSHelpers.Read8(data, srcOffset + 0x37);

                srcOffset += 0x38;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, FullMapImageScaleX);
                FSHelpers.WriteFloat(stream, FullMapImageScaleY);
                FSHelpers.WriteFloat(stream, FullMapSpaceScaleX);
                FSHelpers.WriteFloat(stream, FullMapSpaceScaleY);
                FSHelpers.WriteFloat(stream, FullMapXCoord);
                FSHelpers.WriteFloat(stream, FullMapYCoord);
                FSHelpers.WriteFloat(stream, ZoomedMapXScrolling1);
                FSHelpers.WriteFloat(stream, ZoomedMapYScrolling1);
                FSHelpers.WriteFloat(stream, ZoomedMapXScrolling2);
                FSHelpers.WriteFloat(stream, ZoomedMapYScrolling2);
                FSHelpers.WriteFloat(stream, ZoomedMapXCoord);
                FSHelpers.WriteFloat(stream, ZoomedMapYCoord);
                FSHelpers.WriteFloat(stream, ZoomedMapScale);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write8(stream, MapIndex);
                FSHelpers.Write8(stream, Unknown2);
                FSHelpers.Write8(stream, Padding);
            }
        }

        public class TwoDChunk : TwoDMAChunk
        {
            public TwoDChunk():base("TWOD", "Minimap"){}
        }

        public class DMAPChunk : BaseChunk
        {
            public float MapSpaceX;
            public float MapSpaceY;
            public float MapSpaceScale;
            public float Unknown1;

            public DMAPChunk():base("DMAP", "Dungon Map"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                MapSpaceX = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset));
                MapSpaceY = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x4));
                MapSpaceScale = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0x8));
                Unknown1 = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0xC));

                srcOffset += 0x10;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteFloat(stream, MapSpaceX);
                FSHelpers.WriteFloat(stream, MapSpaceY);
                FSHelpers.WriteFloat(stream, MapSpaceScale);
                FSHelpers.WriteFloat(stream, Unknown1);
            }
        }

        public class RTBLChunk : BaseChunk
        {
            public byte Unknown1;
            public ushort Unknown2;
            public byte[] Data;
            public int Index1;
            public int Index2;
            public bool LastRtblChunk;

            public RTBLChunk():base("RTBL", "Room Table")
            {
                LastRtblChunk = false;
            }


            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Index1 = (int)FSHelpers.Read32(data, srcOffset);
                byte dataSize = FSHelpers.Read8(data, Index1);
                Unknown1 = FSHelpers.Read8(data, Index1 + 0x1);
                Unknown2 = (ushort)FSHelpers.Read16(data, Index1 + 0x2); // 0x2 and 0x3 bytes seems to be always 0
                Index2 = (int)FSHelpers.Read32(data, Index1 + 0x4);
                Data = FSHelpers.ReadN(data, Index2, dataSize);

                srcOffset += 0x4;
            }

            public override void WriteData(BinaryWriter stream)
            {
                //throw new Exception("Hey we haven't tested this, you should test it now!");
                FSHelpers.WriteFloat(stream, Index1);
                int nextChunkOffset = (int)stream.BaseStream.Position;
                stream.Seek(Index1, SeekOrigin.Begin);

                FSHelpers.Write8(stream, (byte)Data.Length);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write16(stream, Unknown2);
                FSHelpers.WriteFloat(stream, Index2);
                stream.Seek(Index2, SeekOrigin.Begin);

                FSHelpers.WriteArray(stream, Data);
                if (!LastRtblChunk)
                {
                    // Do we need padding here?
                    stream.Seek(nextChunkOffset, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// Presumed to stand for "Left BlaNK" it is typically all null values, except every
        /// now and then when there's an odd byte mixed in.
        /// </summary>
        [EntEditorType(typeof(LBlankEditor))]
        public class LbnkChunk : BaseChunk
        {
            public byte Data;

            public LbnkChunk():base("LBNK", "Left Blank"){}
            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Data = FSHelpers.Read8(data, srcOffset);

                srcOffset += 1;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write8(stream, Data);
            }
        }

        [EntEditorType(typeof(ScaleableObjectEditor), MinEditorWidth = 210)]
        public class ScobChunk : BaseChunk
        {
            [DisplayName] public string ObjectName; //Always 8 bytes
            public byte Param0;
            public byte Param1;
            public byte Param2;
            public byte Param3; //Params are context-sensitive. They differ between objects.
            public Vector3 Position;
            public ushort TextId; //Only objects that call up text use this, contains TextID
            public HalfRotationSingle YRotation;
            public ushort Unknown1;
            public ushort Unknown2; //May be padding? Always seems to be FF FF
            public byte ScaleX;
            public byte ScaleY;
            public byte ScaleZ;
            public byte Padding;

            public ScobChunk():base("SCOB", "Scaleable Objects"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                ObjectName = FSHelpers.ReadString(data, srcOffset, 8);
                Param0 = FSHelpers.Read8(data, srcOffset + 8);
                Param1 = FSHelpers.Read8(data, srcOffset + 9);
                Param2 = FSHelpers.Read8(data, srcOffset + 10);
                Param3 = FSHelpers.Read8(data, srcOffset + 11);
                Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));
                TextId = (ushort)FSHelpers.Read16(data, srcOffset + 24);
                YRotation = new HalfRotationSingle(data, srcOffset + 26);
                Unknown1 = (ushort)FSHelpers.Read16(data, srcOffset + 28);
                Unknown2 = (ushort)FSHelpers.Read16(data, srcOffset + 30);
                ScaleX = FSHelpers.Read8(data, srcOffset + 32);
                ScaleY = FSHelpers.Read8(data, srcOffset + 33);
                ScaleZ = FSHelpers.Read8(data, srcOffset + 34);
                Padding = FSHelpers.Read8(data, srcOffset + 35);

                srcOffset += 36;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, ObjectName, 8);
                FSHelpers.Write8(stream, Param0);
                FSHelpers.Write8(stream, Param1);
                FSHelpers.Write8(stream, Param2);
                FSHelpers.Write8(stream, Param3);
                FSHelpers.WriteFloat(stream, Position.X);
                FSHelpers.WriteFloat(stream, Position.Y);
                FSHelpers.WriteFloat(stream, Position.Z);
                FSHelpers.Write16(stream, TextId);
                FSHelpers.Write16(stream, YRotation.Value);
                FSHelpers.Write16(stream, Unknown1);
                FSHelpers.Write16(stream, Unknown2);
                FSHelpers.Write8(stream, ScaleX);
                FSHelpers.Write8(stream, ScaleY);
                FSHelpers.Write8(stream, ScaleZ);
                FSHelpers.Write8(stream, Padding);
            }
        }

        public class TgdrChunk : BaseChunkSpatial
        {
            [DisplayName] public string Name;

            public ushort Unknown0; //Usually 0F FF?
            public ushort DoorType; //Unknown how it works.
            //public Vector3 Position;
            public ushort Unknown1;
            public ushort yRot;
            public ushort Unknown2;
            public ushort Padding;
            public int Unknown3;

            public TgdrChunk():base("TGDR", "Doors"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Name = FSHelpers.ReadString(data, srcOffset, 8);
                Unknown0 = (ushort)FSHelpers.Read16(data, srcOffset + 8);
                DoorType = (ushort)FSHelpers.Read16(data, srcOffset + 10);
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));
                Unknown1 = (ushort)FSHelpers.Read16(data, srcOffset + 24);
                yRot = (ushort)FSHelpers.Read16(data, srcOffset + 26);
                Unknown2 = (ushort)FSHelpers.Read16(data, srcOffset + 28);
                Padding = (ushort)FSHelpers.Read16(data, srcOffset + 30);
                Unknown3 = (int)FSHelpers.Read32(data, srcOffset + 32);

                srcOffset += 36;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Name, 8);
                FSHelpers.Write16(stream, Unknown0);
                FSHelpers.Write16(stream, DoorType);
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);
                FSHelpers.Write16(stream, Unknown1);
                FSHelpers.Write16(stream, yRot);
                FSHelpers.Write16(stream, Unknown2);
                FSHelpers.Write16(stream, Padding);
                FSHelpers.Write32(stream, Unknown3);
            }


        }
        #endregion
    }
}