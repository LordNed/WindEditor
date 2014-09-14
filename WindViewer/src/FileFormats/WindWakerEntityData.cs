﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
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

            var chnkHeaders = new List<ChunkHeader>();
            for (int i = 0; i < header.ChunkCount; i++)
            {
                ChunkHeader chunkHeader = new ChunkHeader();
                chunkHeader.Load(data, ref offset);
                chnkHeaders.Add(chunkHeader);
            }

            var orderedList = chnkHeaders.OrderBy(kvp => kvp.ChunkOffset);

            foreach (ChunkHeader chunkHeader in orderedList)
            {
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
                    chunk.ChunkName = chunkHeader.Tag;
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
            //Write the file header
            FileHeader header = new FileHeader();
            header.ChunkCount = _chunkList.Count;
            header.Save(stream);

            //Save the current position of the stream, then allocate numChunkHeaders * chunkHeaderSize
            //bytes in the stream. We'll then create the chunk headers as we write the chunk data,
            //and then come back to this position and write the headers in afterwards.
            int chunkHeaderOffset = (int) stream.BaseStream.Position;
            stream.BaseStream.Position += _chunkList.Count*ChunkHeader.Size;
            List<ChunkHeader> chunkHeaders = new List<ChunkHeader>();

            int rtblHeaderOffset;

            foreach (KeyValuePair<Type, List<BaseChunk>> pair in _chunkList)
            {
                ChunkHeader chunkHeader = new ChunkHeader();
                chunkHeader.ChunkOffset = (int) stream.BaseStream.Position;
                chunkHeader.Tag = pair.Value[0].ChunkName; //ToDo: We're in trouble if the chunk has no children.
                chunkHeader.ElementCount = pair.Value.Count;

                chunkHeaders.Add(chunkHeader);

                if (chunkHeader.Tag == "RTBL")
                {
                    rtblHeaderOffset = (int) stream.BaseStream.Position;
                    stream.BaseStream.Position += pair.Value.Count*RTBLChunk.Header.Size;

                    //Then write all of the Entry and Table pairs.
                    foreach (BaseChunk chunk in pair.Value)
                    {
                        RTBLChunk rtblHeader = (RTBLChunk) chunk;
                        rtblHeader.EntryHeader.EntryOffset = (int) stream.BaseStream.Position;

                        //Write the EntryData to disk which writes the Table offset as being
                        //immediately after itself.
                        rtblHeader.EntryHeader.Entry.WriteData(stream);
                        rtblHeader.EntryHeader.Entry.Table.WriteData(stream);
                    }
                    
                    //Then go back and write all of the rtblHeaders to disk now that we've set their offsets.
                    stream.BaseStream.Position = rtblHeaderOffset;
                    foreach (BaseChunk baseChunk in pair.Value)
                    {
                        baseChunk.WriteData(stream);
                    }

                    //Finally skip us to the next clear spot in the damn file.
                    stream.Seek(0, SeekOrigin.End);
                }
                else
                {
                    //Write all of the chunk data into the stream
                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        BaseChunk chunk = pair.Value[i];
                        chunk.WriteData(stream);
                    }
                }
            }

            //Now that we've created teh chunk headers and they have correct offsets set, lets go back
            //and write them to the actual file.
            stream.BaseStream.Position = chunkHeaderOffset;
            foreach (ChunkHeader chunkHeader in chunkHeaders)
            {
                chunkHeader.WriteData(stream);
            }
        }

        #region File Formats
        class FileHeader
        {
            public const int Size = 4;
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
            public const int Size = 12;

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

            public void WriteData(BinaryWriter stream)
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
            public string ChunkName;

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
        [EntEditorType(typeof(EnvREditor))]
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
        [EntEditorType(typeof(ColoEditor))]
        public class ColoChunk : BaseChunk
        {
            public byte DawnIndexA; //Index of the Pale entry to use for Dawn
            public byte MorningIndexA;
            public byte NoonIndexA;
            public byte AfternoonIndexA;
            public byte DuskIndexA;
            public byte NightIndexA;

            public byte DawnIndexB; //Index of the Pale entry to use for Dawn
            public byte MorningIndexB;
            public byte NoonIndexB;
            public byte AfternoonIndexB;
            public byte DuskIndexB;
            public byte NightIndexB;

            public ColoChunk()
                : base("COLO", "Color")
            {
                DawnIndexA = MorningIndexA = NoonIndexA = AfternoonIndexA = DuskIndexA = NightIndexA = 0;
            }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                DawnIndexA = FSHelpers.Read8(data, srcOffset + 0);
                MorningIndexA = FSHelpers.Read8(data, srcOffset + 1);
                NoonIndexA = FSHelpers.Read8(data, srcOffset + 2);
                AfternoonIndexA = FSHelpers.Read8(data, srcOffset + 3);
                DuskIndexA = FSHelpers.Read8(data, srcOffset + 4);
                NightIndexA = FSHelpers.Read8(data, srcOffset + 5);

                DawnIndexB = FSHelpers.Read8(data, srcOffset + 6);
                MorningIndexB = FSHelpers.Read8(data, srcOffset + 7);
                NoonIndexB = FSHelpers.Read8(data, srcOffset + 8);
                AfternoonIndexB = FSHelpers.Read8(data, srcOffset + 9);
                DuskIndexB = FSHelpers.Read8(data, srcOffset + 10);
                NightIndexB = FSHelpers.Read8(data, srcOffset + 11);

                srcOffset += 12;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.Write8(stream, DawnIndexA);
                FSHelpers.Write8(stream, MorningIndexA);
                FSHelpers.Write8(stream, NoonIndexA);
                FSHelpers.Write8(stream, AfternoonIndexA);
                FSHelpers.Write8(stream, DuskIndexA);
                FSHelpers.Write8(stream, NightIndexA);

                FSHelpers.Write8(stream, DawnIndexB);
                FSHelpers.Write8(stream, MorningIndexB);
                FSHelpers.Write8(stream, NoonIndexB);
                FSHelpers.Write8(stream, AfternoonIndexB);
                FSHelpers.Write8(stream, DuskIndexB);
                FSHelpers.Write8(stream, NightIndexB);
            }
        }

        /// <summary>
        /// The Pale (short for Palette) chunk contains the actual RGB colors for different
        /// types of lighting. 
        /// </summary>
        [EntEditorType(typeof(PaleEditor))]
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
            [UnitTestValue((byte)0xFF)]public byte Padding1;
            [UnitTestValue((byte)0xFF)]public byte Padding2;

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
                Padding1 = FSHelpers.Read8(data, srcOffset + 1);
                Padding2 = FSHelpers.Read8(data, srcOffset + 2);
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
                FSHelpers.Write8(stream, Padding1);
                FSHelpers.Write8(stream, Padding2);

                FSHelpers.WriteArray(stream, OceanFadeInto.GetBytes());
                FSHelpers.WriteArray(stream, ShoreFadeInto.GetBytes());
            }
        }

        /// <summary>
        /// The Virt (short for uh.. Virtual? I dunno) chunk contains color data for the skybox. Indexed by a Pale
        /// chunk.
        /// </summary>
        [EntEditorType(typeof(VirtEditor))]
        public class VirtChunk : BaseChunk
        {
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public ByteColorAlpha HorizonCloudColor; //The Horizon
            public ByteColorAlpha CenterCloudColor;  //Directly above you
            public ByteColor CenterSkyColor;
            public ByteColor HorizonColor;
            public ByteColor SkyFadeTo; //Color to fade to from CenterSky. 

            [UnitTestValue((byte)0xFF)] public byte Padding1;
            [UnitTestValue((byte)0xFF)]public byte Padding2;
            [UnitTestValue((byte)0xFF)]public byte Padding3;

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
                Unknown1 = (uint)FSHelpers.Read32(data, srcOffset);
                Unknown2 = (uint)FSHelpers.Read32(data, srcOffset + 4);
                Unknown3 = (uint)FSHelpers.Read32(data, srcOffset + 8);
                Unknown4 = (uint)FSHelpers.Read32(data, srcOffset + 12);
                srcOffset += 16;

                HorizonCloudColor = new ByteColorAlpha(data, ref srcOffset);
                CenterCloudColor = new ByteColorAlpha(data, ref srcOffset);

                CenterSkyColor = new ByteColor(data, ref srcOffset);
                HorizonColor = new ByteColor(data, ref srcOffset);
                SkyFadeTo = new ByteColor(data, ref srcOffset);

                Padding1 = FSHelpers.Read8(data, srcOffset);
                Padding2 = FSHelpers.Read8(data, srcOffset + 1);
                Padding3 = FSHelpers.Read8(data, srcOffset + 2);
                srcOffset += 3;
            }

            public override void WriteData(BinaryWriter stream)
            {
                //Fixed values that doesn't seem to change.
                FSHelpers.Write32(stream, (int)Unknown1);
                FSHelpers.Write32(stream, (int)Unknown2);
                FSHelpers.Write32(stream, (int)Unknown3);
                FSHelpers.Write32(stream, (int)Unknown4);

                FSHelpers.WriteArray(stream, HorizonCloudColor.GetBytes());
                FSHelpers.WriteArray(stream, CenterCloudColor.GetBytes());
                FSHelpers.WriteArray(stream, CenterSkyColor.GetBytes());
                FSHelpers.WriteArray(stream, HorizonColor.GetBytes());
                FSHelpers.WriteArray(stream, SkyFadeTo.GetBytes());

                FSHelpers.Write8(stream, Padding1);
                FSHelpers.Write8(stream, Padding2);
                FSHelpers.Write8(stream, Padding3);
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
            public string StageName; //Name of the stage to spawn in
            public byte SpawnID; //ID of the spawn point to spawn at
            public byte RoomID; //ID of the room to spawn in
            public byte FadeoutID; //ID of the fadeout

            public SclsChunk()
                : base("SCLS", "Exits")
            {
                StageName = "INVALID";
            }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                StageName = FSHelpers.ReadString(data, srcOffset, 8);
                SpawnID = FSHelpers.Read8(data, srcOffset + 8);
                RoomID = FSHelpers.Read8(data, srcOffset + 9);
                FadeoutID = FSHelpers.Read8(data, srcOffset + 10);

                srcOffset += 12;
            }


            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, StageName, 8);
                FSHelpers.Write8(stream, SpawnID);
                FSHelpers.Write8(stream, RoomID);
                FSHelpers.Write8(stream, FadeoutID);
                FSHelpers.Write8(stream, 0xFF);
            }
        }

        /// <summary>
        /// The Plyr (Player) chunk defines spawn points for Link.
        /// </summary>
        [EntEditorType(typeof(PlayerEditor))]
        public class PlyrChunk : BaseChunkSpatial
        {
            public byte ID; //ID of the spawn
            [DisplayName]
            public string Name; //"Link"
            public byte EventIndex; //Specifies an event from the DZS file to play upon spawn. FF = no event.
            [UnitTestValue((byte)0xFF)] public byte Unknown1; //Padding?
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
                Rotation = new HalfRotation(data, ref srcOffset); //There's no Z rotation

                srcOffset -= 1;

                ID = FSHelpers.Read8(data, srcOffset++);
   
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
                FSHelpers.Write8(stream, 0xFF);
                FSHelpers.Write8(stream, ID);

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
            [UnitTestValue((ushort)0xFFFF)] public ushort Unknown1; //Probably padding
            [UnitTestValue((byte)0xFF)] public byte Unknown2; //More padding?
            [UnitTestValue((byte)0xFF)] public byte Unknown3; //Possibly not padding
            [UnitTestValue((ushort)0xFFFF)] public ushort Padding; //ACTUAL PADDING!?
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
            [UnitTestValue((byte)0xFF)] public byte Padding;
            public byte Unknown2; //Typically FF, but Outset's entries have the room number (0x2C) here
            public byte SoundId;
            public byte SoundRadius;
            [UnitTestValue((byte)0xFF)] public byte Padding2;
            [UnitTestValue((byte)0xFF)] public byte Padding3;
            [UnitTestValue((byte)0xFF)] public byte Padding4;

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
            public byte Param1;
            public byte Param2;
            public byte Param3;
            public byte Param4;
            //public Vector3 Position;
            public ushort RoomId;
            public ushort YRotation;
            public byte ChestItem;
            public byte Unknown1;
            [UnitTestValue((ushort)0xFFFF)]public ushort Padding; //Rupees, Hookshot, etc.


            public TresChunk():base("TRES", "Treasure Chests (Non-Ocean)"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Name = FSHelpers.ReadString(data, srcOffset, 8);
                Param1 = FSHelpers.Read8(data, srcOffset + 8);
                Param2 = FSHelpers.Read8(data, srcOffset + 9);
                Param3 = FSHelpers.Read8(data, srcOffset + 10);
                Param4 = FSHelpers.Read8(data, srcOffset + 11);
                
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));

                RoomId = (ushort)FSHelpers.Read16(data, srcOffset + 24);
                YRotation = (ushort)FSHelpers.Read16(data, srcOffset + 26);
                ChestItem = FSHelpers.Read8(data, srcOffset + 28);
                Unknown1 = FSHelpers.Read8(data, srcOffset + 29);
                Padding = (ushort)FSHelpers.Read16(data, srcOffset + 30);
                srcOffset += 32;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Name, 8);
                FSHelpers.Write8(stream, Param1);
                FSHelpers.Write8(stream, Param2);
                FSHelpers.Write8(stream, Param3);
                FSHelpers.Write8(stream, Param4);
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);
                FSHelpers.Write16(stream, RoomId);
                FSHelpers.Write16(stream, YRotation);
                FSHelpers.Write8(stream, ChestItem);
                FSHelpers.Write8(stream, Unknown1);
                FSHelpers.Write16(stream, Padding);
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
            [UnitTestValue((byte)0xFF)] public byte Unknown;

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
            [UnitTestValue((ushort)0xFFFF)] public ushort Padding;

            public RaroChunk():base("RARO", "Camera Ref Data"){}
            public RaroChunk(string chunkName, string chunkDescription):base(chunkName, chunkDescription){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 0));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 4));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 8));
                srcOffset += 12;
                
                Rotation = new HalfRotation(data, ref srcOffset);
                Padding = (ushort)FSHelpers.Read16(data, srcOffset);
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
            [UnitTestValue((byte)0xFF)]public byte Padding1;
            [UnitTestValue((byte)0xFF)]public byte Padding2;
            [UnitTestValue((byte)0xFF)]public byte Padding3;


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

        [EntEditorType(typeof(ActorEditor))]
        public class ActrChunk : BaseChunkSpatial
        {
            [DisplayName] public string Name;
            public byte Unknown1;
            public byte RpatIndex;
            public byte Unknown2;
            public byte BehaviorType;
            //public Vector3 Position;
            public HalfRotation Rotation;

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
                Rotation = new HalfRotation(data, ref srcOffset);
                //srcOffset += 6; //ToDo: Fix me

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

                FSHelpers.Write16(stream, (ushort)Rotation.X);
                FSHelpers.Write16(stream, (ushort)Rotation.Y);
                FSHelpers.Write16(stream, (ushort)Rotation.Z); //ToDo: Fix me

                FSHelpers.Write16(stream, EnemyNumber);
            }
        }

        public class TgobChunk : ActrChunk
        {
            public TgobChunk():base("TGOB", "Actor"){}
        }

        [EntEditorType(typeof(StagePropertyEditor))]
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
        [EntEditorType(typeof(MinimapEditor))]
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
            [UnitTestValue((byte)0x80)]public byte Unknown1; //Always 0x80?
            public byte MapIndex; //number of the map image to use. For instance, using the first image would be 80, the second 81, and so on.
            public byte Unknown2; //variable, but changing it has no immediate result
            [UnitTestValue((byte)0xFF)]public byte Padding;

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

        [EntEditorType(typeof(DungeonMapEditor))]
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
            public class Header
            {
                public const int Size = 4;

                public int EntryOffset;

                public Entry Entry;
                public void Load(byte[] data, int srcOffset)
                {
                    EntryOffset = FSHelpers.Read32(data, srcOffset);

                    Entry = new Entry();
                    Entry.Load(data, EntryOffset);
                }

                public void WriteData(BinaryWriter stream)
                {
                    FSHelpers.Write32(stream, EntryOffset);
                }
            }

            public class Entry
            {
                public const int Size = 8;

                public byte NumRooms;
                public byte Unknown1;
                public ushort Unknown2;
                public int TableOffset;

                public Table Table;

                public void Load(byte[] data, int srcOffset)
                {
                    NumRooms = FSHelpers.Read8(data, srcOffset);
                    Unknown1 = FSHelpers.Read8(data, srcOffset + 1);
                    Unknown2 = (ushort) FSHelpers.Read16(data, srcOffset + 2);
                    TableOffset = FSHelpers.Read32(data, srcOffset + 4);

                    Table = new Table();
                    Table.NumRooms = NumRooms;
                    Table.Load(data, TableOffset);
                }

                public void WriteData(BinaryWriter stream)
                {
                    FSHelpers.Write8(stream, NumRooms);
                    FSHelpers.Write8(stream, Unknown1);
                    FSHelpers.Write16(stream, Unknown2);
                    FSHelpers.Write32(stream, (int)stream.BaseStream.Position + 4);
                }
            }

            public class Table
            {
                public byte CurrentRoom;
                public byte[] ConcurrentRooms;

                //Not part of the Table file format, for loading/saving purposes only!!
                public int NumRooms;

                public void Load(byte[] data, int tableOffset)
                {
                    CurrentRoom = FSHelpers.Read8(data, tableOffset);

                    ConcurrentRooms = new byte[NumRooms];
                    for (int i = 0; i < NumRooms; i++)
                        ConcurrentRooms[i] = FSHelpers.Read8(data, tableOffset + 1 + i);
                }

                public void WriteData(BinaryWriter stream)
                {
                    FSHelpers.Write8(stream, CurrentRoom);
                    for (int i = 0; i < NumRooms; i++)
                        FSHelpers.Write8(stream, ConcurrentRooms[i]);
                }
            }

            public Header EntryHeader;

            public RTBLChunk() : base("RTBL", "Room Visibility Table") { }

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                EntryHeader = new Header();
                EntryHeader.Load(data, srcOffset);

                srcOffset += 4;
            }

            public override void WriteData(BinaryWriter stream)
            {
                EntryHeader.WriteData(stream);
            }
        }

        /// <summary>
        /// Presumed to stand for "Left BlaNK" it is typically all null values, except every
        /// now and then when there's an odd byte mixed in.
        /// </summary>
        [EntEditorType(typeof(LBlankEditor))]
        public class LbnkChunk : BaseChunk
        {
            [UnitTestValue((byte)0xFF)]public byte Data;

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
            [UnitTestValue((byte)0xFF)]public byte Padding;

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

        [EntEditorType(typeof(DoorEditor))]
        public class TgdrChunk : BaseChunkSpatial
        {
            [DisplayName] public string Name;
            public byte Param1;
            public byte Param2;
            public byte Param3;
            public byte Param4;
            //public Vector3 Position;
            public ushort Unknown1;
            public ushort YRotation;
            public byte DoorModel;
            [UnitTestValue((byte)0x3F)]public byte Const3F;
            [UnitTestValue((byte)0x00)]public byte ConstZero;
            [UnitTestValue((byte)0xFF)]public byte Padding1;
            public byte Unknown2;
            public byte Unknown3;
            public byte Unknown4;
            [UnitTestValue((byte)0xFF)]public byte Padding2;

            public TgdrChunk():base("TGDR", "Doors"){}

            public override void LoadData(byte[] data, ref int srcOffset)
            {
                Name = FSHelpers.ReadString(data, srcOffset, 8);
                Param1 = FSHelpers.Read8(data, srcOffset + 8);
                Param2 = FSHelpers.Read8(data, srcOffset + 9);
                Param3 = FSHelpers.Read8(data, srcOffset + 10);
                Param4 = FSHelpers.Read8(data, srcOffset + 11);
                Transform.Position.X = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 12));
                Transform.Position.Y = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 16));
                Transform.Position.Z = FSHelpers.ConvertIEEE754Float((uint)FSHelpers.Read32(data, srcOffset + 20));
                Unknown1 = (ushort)FSHelpers.Read16(data, srcOffset + 24);
                YRotation = (ushort)FSHelpers.Read16(data, srcOffset + 26);
                DoorModel = FSHelpers.Read8(data, srcOffset + 28);
                Const3F = FSHelpers.Read8(data, srcOffset + 29);
                ConstZero = FSHelpers.Read8(data, srcOffset + 30);
                Padding1 = FSHelpers.Read8(data, srcOffset + 31);
                Unknown2 = FSHelpers.Read8(data, srcOffset + 32);
                Unknown3 = FSHelpers.Read8(data, srcOffset + 33);
                Unknown4 = FSHelpers.Read8(data, srcOffset + 34);
                Padding2 = FSHelpers.Read8(data, srcOffset + 35);

                srcOffset += 36;
            }

            public override void WriteData(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Name, 8);
                FSHelpers.Write8(stream, Param1);
                FSHelpers.Write8(stream, Param2);
                FSHelpers.Write8(stream, Param3);
                FSHelpers.Write8(stream, Param4);
                FSHelpers.WriteFloat(stream, Transform.Position.X);
                FSHelpers.WriteFloat(stream, Transform.Position.Y);
                FSHelpers.WriteFloat(stream, Transform.Position.Z);
                FSHelpers.Write16(stream, Unknown1);
                FSHelpers.Write16(stream, YRotation);
                FSHelpers.Write8(stream, DoorModel);
                FSHelpers.Write8(stream, Const3F);
                FSHelpers.Write8(stream, ConstZero);
                FSHelpers.Write8(stream, Padding1);
                FSHelpers.Write8(stream, Unknown2);
                FSHelpers.Write8(stream, Unknown3);
                FSHelpers.Write8(stream, Unknown4);
                FSHelpers.Write8(stream, Padding2);
            }


        }
        #endregion
    }
}