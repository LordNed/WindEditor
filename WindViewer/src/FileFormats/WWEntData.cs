using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using System.Xml.Linq;
using GameFormatReader.Common;
using OpenTK;

namespace WindViewer.FileFormats
{
    public sealed class WwEntData
    {
        public WwEntData(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath", "filepath cannot be null");
            if (!File.Exists(filePath))
                throw new IOException(string.Format("File {0} does not exist", filePath));

            using (var reader = new EndianBinaryReader(File.OpenRead(filePath), Endian.Big))
            {
                uint chunkCount = reader.ReadUInt32();
                ReadChunks(reader, chunkCount);
            }
        }

        #region Classes / Structs
        public abstract class BaseChunk
        {
            public abstract void LoadData(EndianBinaryReader reader);
            public abstract void WriteData(EndianBinaryWriter writer);
        }

        private struct ChunkHeader
        {
            /// <summary> 4 byte ASCII name for chunk.</summary>
            public string Tag;
            /// <summary> Number of entries of this chunk type.</summary>
            public uint ChunkEntries;
            /// <summary> Offset from start of file to element.</summary>
            public uint ChunkOffset;
        }

        public class EnvironmentColor : BaseChunk
        {
            /// <summary> Index into the Palette array to use during "Dawn" time in game. </summary>
            //ToDo: Find out the difference between the A and B indexes.
            public byte DawnIndexA;
            public byte MorningIndexA;
            public byte NoonIndexA;
            public byte AfternoonIndexA;
            public byte DuskIndexA;
            public byte NightIndexA;

            /// <summary> Index into the Palette array to use during "Dawn" time in game. </summary>
            public byte DawnIndexB;
            public byte MorningIndexB;
            public byte NoonIndexB;
            public byte AfternoonIndexB;
            public byte DuskIndexB;
            public byte NightIndexB;

            public override void LoadData(EndianBinaryReader reader)
            {
                DawnIndexA = reader.ReadByte();
                MorningIndexA = reader.ReadByte();
                NoonIndexA = reader.ReadByte();
                AfternoonIndexA = reader.ReadByte();
                DuskIndexA = reader.ReadByte();
                NightIndexA = reader.ReadByte();

                DawnIndexB = reader.ReadByte();
                MorningIndexB = reader.ReadByte();
                NoonIndexB = reader.ReadByte();
                AfternoonIndexB = reader.ReadByte();
                DuskIndexB = reader.ReadByte();
                NightIndexB = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(DawnIndexA);
                writer.Write(MorningIndexA);
                writer.Write(NoonIndexA);
                writer.Write(AfternoonIndexA);
                writer.Write(DuskIndexA);
                writer.Write(NightIndexA);

                writer.Write(DawnIndexB);
                writer.Write(MorningIndexB);
                writer.Write(NoonIndexB);
                writer.Write(AfternoonIndexB);
                writer.Write(DuskIndexB);
                writer.Write(NightIndexB);
            }
        }

        public class EnvironmentPalette : BaseChunk
        {
            /// <summary> RGB <see cref="ByteColor"/> for various objects in scene. </summary>
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

            /// <summary> Index into the <see cref="EnvironmentSky"/> array. </summary>
            public byte EnvironmentSkyIndex;
            public ushort Padding;

            /// <summary> RGBA <see cref="ByteColorAlpha"/> for various objects in scene. </summary>
            public ByteColorAlpha OceanFadeInto;
            public ByteColorAlpha ShoreFadeInto;

            public override void LoadData(EndianBinaryReader reader)
            {
                ActorAmbient = new ByteColor(reader);
                ShadowColor = new ByteColor(reader);
                RoomFillColor = new ByteColor(reader);
                RoomAmbient = new ByteColor(reader);
                WaveColor = new ByteColor(reader);
                OceanColor = new ByteColor(reader);
                UnknownColor1 = new ByteColor(reader);
                UnknownColor2 = new ByteColor(reader);
                DoorwayColor = new ByteColor(reader);
                UnknownColor3 = new ByteColor(reader);
                FogColor = new ByteColor(reader);

                EnvironmentSkyIndex = reader.ReadByte();
                Padding = reader.ReadUInt16();

                OceanFadeInto = new ByteColorAlpha(reader);
                ShoreFadeInto = new ByteColorAlpha(reader);
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                ActorAmbient.Write(writer);
                ShadowColor.Write(writer);
                RoomFillColor.Write(writer);
                RoomAmbient.Write(writer);
                WaveColor.Write(writer);
                OceanColor.Write(writer);
                UnknownColor1.Write(writer);
                UnknownColor2.Write(writer);
                DoorwayColor.Write(writer);
                FogColor.Write(writer);
                writer.Write(EnvironmentSkyIndex);
                writer.Seek(0x2, SeekOrigin.Current); //Padding
                OceanFadeInto.Write(writer);
                ShoreFadeInto.Write(writer);
            }
        }

        public class EnvironmentSky : BaseChunk
        {
            /// <summary> Unknown purpose. All four unknowns have a constant value. </summary>
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            /// <summary> Color of the clouds on the horizon. </summary>
            public ByteColorAlpha HorizonCloudColor;
            /// <summary> Color of the clouds above the player. </summary>
            public ByteColorAlpha CenterCloudColor;
            /// <summary> Color of the background sky above the player. </summary>
            public ByteColor CenterSkyColor;
            /// <summary> Color of the horizon glow. </summary>
            public ByteColor HorizonColor;
            /// <summary> Color of the background sky at the horizon. </summary>
            public ByteColor SkyFadeTo;

            private ushort _padding1;
            private byte _padding2;

            public override void LoadData(EndianBinaryReader reader)
            {
                //First 16 bytes are 80 00 00 00 (repeated 4 times). Unknown why.
                Unknown1 = reader.ReadUInt32();
                Unknown2 = reader.ReadUInt32();
                Unknown3 = reader.ReadUInt32();
                Unknown4 = reader.ReadUInt32();

                HorizonCloudColor = new ByteColorAlpha(reader);
                CenterCloudColor = new ByteColorAlpha(reader);

                CenterSkyColor = new ByteColor(reader);
                HorizonColor = new ByteColor(reader);
                SkyFadeTo = new ByteColor(reader);

                _padding1 = reader.ReadUInt16();
                _padding2 = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                HorizonCloudColor.Write(writer);
                CenterCloudColor.Write(writer);
                CenterSkyColor.Write(writer);
                HorizonColor.Write(writer);
                SkyFadeTo.Write(writer);

                writer.Write(_padding1);
                writer.Write(_padding2);
            }
        }

        public class EnvironmentLighting : BaseChunk
        {
            public byte ClearColorIndexA; //Index of the Color entry to use for clear weather.
            public byte RainingColorIndexA; //There's two sets, A and B. B's usage is unknown but identical.
            public byte SnowingColorIndexA;
            public byte UnknownColorIndexA; //We don't know what weather this color is used for! Might be lava or forest

            public byte ClearColorIndexB;
            public byte RainingColorIndexB;
            public byte SnowingColorIndexB;
            public byte UnknownColorIndexB;

            public override void LoadData(EndianBinaryReader reader)
            {
                ClearColorIndexA = reader.ReadByte();
                RainingColorIndexA = reader.ReadByte();
                SnowingColorIndexA = reader.ReadByte();
                UnknownColorIndexA = reader.ReadByte();

                ClearColorIndexB = reader.ReadByte();
                RainingColorIndexB = reader.ReadByte();
                SnowingColorIndexB = reader.ReadByte();
                UnknownColorIndexB = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(ClearColorIndexA);
                writer.Write(RainingColorIndexA);
                writer.Write(SnowingColorIndexA);
                writer.Write(UnknownColorIndexA);

                writer.Write(ClearColorIndexB);
                writer.Write(RainingColorIndexB);
                writer.Write(SnowingColorIndexB);
                writer.Write(UnknownColorIndexB);
            }
        }

        public class RoomExit : BaseChunk
        {
            public string DestinationName;
            public byte SpawnNumber;
            public byte DestinationRoomNumber;
            public byte ExitType;
            private byte _padding;

            public override void LoadData(EndianBinaryReader reader)
            {
                DestinationName = reader.ReadString(8);
                SpawnNumber = reader.ReadByte();
                DestinationRoomNumber = reader.ReadByte();
                ExitType = reader.ReadByte();
                _padding = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(DestinationName, 8);
                writer.Write(SpawnNumber);
                writer.Write(DestinationRoomNumber);
                writer.Write(ExitType);
                writer.Write(_padding);
            }
        }

        public class PlayerSpawn : BaseChunk
        {
            /// <summary> Always seems to be "Link" </summary>
            public string Name;
            /// <summary> Specifies an event from the DZS file to play upon spawn. FF = no event. </summary>
            public byte EventIndex;
            public byte Unknown1; //Padding?
            /// <summary> How Link enters the room. </summary>
            public byte SpawnType;
            /// <summary> Room number the spawn is in. </summary>
            public byte RoomNumber;
            /// <summary> Position of the player spawn. </summary>
            public Vector3 Position;
            /// <summary> Rotation of the player spawn as a fixed precision number. See <see cref="HalfRotation"/></summary>
            public HalfRotation Rotation;

            private ushort _padding;

            public override void LoadData(EndianBinaryReader reader)
            {
                Name = reader.ReadString(8);
                EventIndex = reader.ReadByte();
                Unknown1 = reader.ReadByte();
                SpawnType = reader.ReadByte();
                RoomNumber = reader.ReadByte();
                Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                Rotation = new HalfRotation(reader);
                _padding = reader.ReadUInt16();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Name, 8);
                writer.Write(EventIndex);
                writer.Write(Unknown1);
                writer.Write(SpawnType);
                writer.Write(RoomNumber);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Position.Z);
                Rotation.Write(writer);
                writer.Write(_padding);
            }
        }

        public class EnvironmentSound : BaseChunk
        {
            /// <summary> Always seems to be "sndpath" </summary>
            public string Name;
            /// <summary> Position teh sound plays from. </summary>
            public Vector3 Position;
            public byte Unknown1; //Typically 00, one example had 08.
            public byte Unknown2;
            public byte Unknown3; //Typically FF, but Outset's entries have the room number (0x2C) here
            public byte SoundId;
            public byte SoundRadius;
            private ushort _padding1;
            private byte _padding2;

            public override void LoadData(EndianBinaryReader reader)
            {
                Name = reader.ReadString(8);
                Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                Unknown1 = reader.ReadByte();
                Unknown2 = reader.ReadByte();
                Unknown3 = reader.ReadByte();
                SoundId = reader.ReadByte();
                SoundRadius = reader.ReadByte();
                _padding1 = reader.ReadUInt16();
                _padding2 = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Name, 8);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Position.Z);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(SoundId);
                writer.Write(SoundRadius);
                writer.Write(_padding1);
                writer.Write(_padding2);
            }
        }

        public class EnvironmentMisc : BaseChunk
        {
            public byte TimePassage;
            public byte WindSettings;
            public byte Unknown1;
            public byte LightingType; //04 is normal, 05 is shadowed.
            public float Unknown2;

            public override void LoadData(EndianBinaryReader reader)
            {
                TimePassage = reader.ReadByte();
                WindSettings = reader.ReadByte();
                Unknown1 = reader.ReadByte();
                LightingType = reader.ReadByte();
                Unknown2 = reader.ReadSingle();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(TimePassage);
                writer.Write(WindSettings);
                writer.Write(Unknown1);
                writer.Write(LightingType);
                writer.Write(Unknown2);
            }
        }

        public class MemoryAllocationIndex : BaseChunk
        {
            /// <summary> Which room number this applies to  </summary>
            public byte RoomNumber;
            /// <summary> Which index in the Mema array to use. </summary>
            public byte MemAllocIndex;

            public override void LoadData(EndianBinaryReader reader)
            {
                RoomNumber = reader.ReadByte();
                MemAllocIndex = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(RoomNumber);
                writer.Write(MemAllocIndex);
            }
        }

        public class MemoryAllocation : BaseChunk
        {
            /// <summary> Amount of memory to allocate for a room. </summary>
            public uint MemSize;

            public override void LoadData(EndianBinaryReader reader)
            {
                MemSize = reader.ReadUInt32();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(MemSize);
            }
        }

        public class TreasureChest : BaseChunk
        {
            public string Name;
            public byte Param1;
            public byte Param2;
            public byte Param3;
            public byte Param4;
            public Vector3 Position;
            public ushort RoomId;
            public HalfRotationSingle Rotation;
            public byte ChestItem;
            public byte Unknown1;
            private ushort _padding;

            public override void LoadData(EndianBinaryReader reader)
            {
                Name = reader.ReadString(8);
                Param1 = reader.ReadByte();
                Param2 = reader.ReadByte();
                Param3 = reader.ReadByte();
                Param4 = reader.ReadByte();
                Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                RoomId = reader.ReadUInt16();
                Rotation = new HalfRotationSingle(reader);
                ChestItem = reader.ReadByte();
                Unknown1 = reader.ReadByte();
                _padding = reader.ReadUInt16();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Name, 8);
                writer.Write(Param1);
                writer.Write(Param2);
                writer.Write(Param3);
                writer.Write(Param4);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Position.Z);
                writer.Write(RoomId);
                Rotation.Write(writer);
                writer.Write(ChestItem);
                writer.Write(Unknown1);
                writer.Write(_padding);
            }
        }

        public class ShipSpawnPoint : BaseChunk
        {
            public Vector3 Position;
            public HalfRotationSingle Rotation;
            public ushort Unknown;
            public override void LoadData(EndianBinaryReader reader)
            {
                Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                Rotation = new HalfRotationSingle(reader);
                Unknown = reader.ReadUInt16();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Position.Z);
                Rotation.Write(writer);
                writer.Write(Unknown);
            }
        }

        public class RoomTransform : BaseChunk
        {
            public float TranslationX;
            public float TranslationY;
            public HalfRotationSingle Rotation;
            public byte RoomNumber;
            private byte _padding;

            public override void LoadData(EndianBinaryReader reader)
            {
                TranslationX = reader.ReadSingle();
                TranslationY = reader.ReadSingle();
                Rotation = new HalfRotationSingle(reader);
                RoomNumber = reader.ReadByte();
                _padding = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(TranslationX);
                writer.Write(TranslationY);
                Rotation.Write(writer);
                writer.Write(RoomNumber);
                writer.Write(_padding);
            }
        }

        public class StageSettings : BaseChunk
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

            public override void LoadData(EndianBinaryReader reader)
            {
                MinDepth = reader.ReadSingle();
                MaxDepth = reader.ReadSingle();
                KeyCounterDisplay = reader.ReadUInt16();
                LoadedParticleBank = reader.ReadUInt16();
                ItemUsageAndMinimap = reader.ReadUInt16();
                Padding = reader.ReadByte();
                Unknown1 = reader.ReadByte();
                Unknown2 = reader.ReadByte();
                Unknown3 = reader.ReadByte();
                DrawDistance = reader.ReadUInt16();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(MinDepth);
                writer.Write(MaxDepth);
                writer.Write(KeyCounterDisplay);
                writer.Write(LoadedParticleBank);
                writer.Write(ItemUsageAndMinimap);
                writer.Write(Padding);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(DrawDistance);
            }
        }

        public class DungeonFloor : BaseChunk
        {
            /// <summary> Y value of the lower boundary of a floor. When link crosses the coord, the map switches him to being on that floor. </summary>
            public float LowerYCoordBoundary;
            public byte FloorId; //????
            public byte[] IncludedRooms = new byte[15];

            public override void LoadData(EndianBinaryReader reader)
            {
                LowerYCoordBoundary = reader.ReadSingle();
                FloorId = reader.ReadByte();
                for (int i = 0; i < 15; i++)
                    IncludedRooms[i] = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(LowerYCoordBoundary);
                writer.Write(FloorId);
                for (int i = 0; i < 15; i++)
                    writer.Write(IncludedRooms[i]);
            }
        }

        public class LBlankChunk : BaseChunk
        {
            public byte Data;

            public override void LoadData(EndianBinaryReader reader)
            {
                Data = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Data);
            }
        }

        public class RoomDoor : BaseChunk
        {
            public string Name;
            public byte Param1;
            public byte Param2;
            public byte Param3;
            public byte Param4;
            public Vector3 Position;
            public ushort Unknown1;
            public HalfRotationSingle Rotation;
            public byte DoorModel;
            public byte Const3F;
            public byte ConstZero;
            public byte Padding1;
            public byte Unknown2;
            public byte Unknown3;
            public byte Unknown4;
            public byte Padding2;

            public override void LoadData(EndianBinaryReader reader)
            {
                Name = reader.ReadString(8);
                Param1 = reader.ReadByte();
                Param2 = reader.ReadByte();
                Param3 = reader.ReadByte();
                Param4 = reader.ReadByte();
                Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                Unknown1 = reader.ReadUInt16();
                Rotation = new HalfRotationSingle(reader);
                DoorModel = reader.ReadByte();
                Const3F = reader.ReadByte();
                ConstZero = reader.ReadByte();
                Padding1 = reader.ReadByte();
                Unknown2 = reader.ReadByte();
                Unknown3 = reader.ReadByte();
                Unknown4 = reader.ReadByte();
                Padding2 = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Name, 8);
                writer.Write(Param1);
                writer.Write(Param2);
                writer.Write(Param3);
                writer.Write(Param4);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Position.Z);
                writer.Write(Unknown1);
                Rotation.Write(writer);
                writer.Write(DoorModel);
                writer.Write(Const3F);
                writer.Write(ConstZero);
                writer.Write(Padding1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Padding2);
            }
        }

        public class Actor : BaseChunk
        {
            public string Name;
            public byte Unknown1;
            public byte RpatIndex;
            public byte Unknown2;
            public byte BehaviorType;
            public Vector3 Position;
            public HalfRotation Rotation;

            public ushort EnemyNumber; //Unknown purpose. Enemies are given a number here based on their position in the actor list.

            public override void LoadData(EndianBinaryReader reader)
            {
                Name = reader.ReadString(8);
                Unknown1 = reader.ReadByte();
                RpatIndex = reader.ReadByte();
                Unknown2 = reader.ReadByte();
                BehaviorType = reader.ReadByte();
                Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                Rotation = new HalfRotation(reader);
                EnemyNumber = reader.ReadUInt16();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Name, 8);
                writer.Write(Unknown1);
                writer.Write(RpatIndex);
                writer.Write(Unknown2);
                writer.Write(BehaviorType);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Position.Z);
                Rotation.Write(writer);
                writer.Write(EnemyNumber);
            }
        }

        public class EnvironmentEvent : BaseChunk
        {
            public byte Unknown1;
            public string EventName;
            public byte Unknown2;
            public byte Unknown3;
            public byte Unknown4;
            public byte Unknown5;
            public byte RoomNumber;
            private byte _padding1;
            private byte _padding2;
            private byte _padding3;

            public override void LoadData(EndianBinaryReader reader)
            {
                Unknown1 = reader.ReadByte();
                EventName = reader.ReadString(15);
                Unknown2 = reader.ReadByte();
                Unknown3 = reader.ReadByte();
                Unknown4 = reader.ReadByte();
                Unknown5 = reader.ReadByte();
                RoomNumber = reader.ReadByte();
                _padding1 = reader.ReadByte();
                _padding2 = reader.ReadByte();
                _padding3 = reader.ReadByte();

            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(EventName, 15);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(RoomNumber);
                writer.Write(_padding1);
                writer.Write(_padding2);
                writer.Write(_padding3);
            }
        }

        public class ScaleableActor : BaseChunk
        {
            public string ObjectName; //Always 8 bytes
            public byte Param0;
            public byte Param1;
            public byte Param2;
            public byte Param3; //Params are context-sensitive. They differ between objects.
            public Vector3 Position;
            public ushort TextId; //Only objects that call up text use this, contains TextID
            public HalfRotationSingle Rotation;
            public ushort Unknown1;
            public ushort Unknown2; //May be padding? Always seems to be FF FF
            public byte ScaleX;
            public byte ScaleY;
            public byte ScaleZ;
            private byte _padding;

            public override void LoadData(EndianBinaryReader reader)
            {
                ObjectName = reader.ReadString(8);
                Param0 = reader.ReadByte();
                Param1 = reader.ReadByte();
                Param2 = reader.ReadByte();
                Param3 = reader.ReadByte();
                Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                TextId = reader.ReadUInt16();
                Rotation = new HalfRotationSingle(reader);
                Unknown1 = reader.ReadUInt16();
                Unknown2 = reader.ReadUInt16();
                ScaleX = reader.ReadByte();
                ScaleY = reader.ReadByte();
                ScaleZ = reader.ReadByte();
                _padding = reader.ReadByte();
            }

            public override void WriteData(EndianBinaryWriter writer)
            {
                writer.Write(ObjectName, 8);
                writer.Write(Param0);
                writer.Write(Param1);
                writer.Write(Param2);
                writer.Write(Param3);
                writer.Write(Position.X);
                writer.Write(Position.Y);
                writer.Write(Position.Z);
                writer.Write(TextId);
                Rotation.Write(writer);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(ScaleX);
                writer.Write(ScaleY);
                writer.Write(ScaleZ);
                writer.Write(_padding);
            }
        }
        #endregion

        #region Private Methods

        private void ReadChunks(EndianBinaryReader reader, uint chunkCount)
        {
            var chunkHeaders = new List<ChunkHeader>();
            for (uint i = 0; i < chunkCount; i++)
            {
                var cHeader = new ChunkHeader();
                cHeader.Tag = reader.ReadString(4);
                cHeader.ChunkEntries = reader.ReadUInt32();
                cHeader.ChunkOffset = reader.ReadUInt32();
                chunkHeaders.Add(cHeader);
            }

            foreach (ChunkHeader cHeader in chunkHeaders)
            {
                for (uint elementIndex = 0; elementIndex < cHeader.ChunkEntries; elementIndex++)
                {
                    BaseChunk chunk = null;
                    switch (cHeader.Tag.Substring(0, 3).ToUpper())
                    {
                        case "COL": chunk = new EnvironmentColor(); break;
                        case "PAL": chunk = new EnvironmentPalette(); break;
                        case "VIR": chunk = new EnvironmentSky(); break;
                        case "ENV": chunk = new EnvironmentLighting(); break;
                        case "SCL": chunk = new RoomExit(); break;
                        case "PLY": chunk = new PlayerSpawn(); break;
                        case "SON": chunk = new EnvironmentSound(); break;
                        case "FIL": chunk = new EnvironmentMisc(); break;
                        case "MEC": chunk = new MemoryAllocationIndex(); break;
                        case "MEM": chunk = new MemoryAllocation(); break;
                        case "TRE": chunk = new TreasureChest(); break;
                        case "SHI": chunk = new ShipSpawnPoint(); break;
                        case "MUL": chunk = new RoomTransform(); break;
                        case "STA": chunk = new StageSettings(); break;
                        case "FLO": chunk = new DungeonFloor(); break;
                        case "LBN": chunk = new LBlankChunk(); break;
                        case "TGD": chunk = new RoomDoor(); break;
                        case "EVN": chunk = new EnvironmentEvent(); break;
                        //ACTR
                        //SCOB

                        //ToDo: RPA, PAT, RPP, PPN, LGH, LGT, RAR, ARO, TGO, RCA, CAM, TWO, 2DM, DMA
                    }

                    if (chunk == null)
                    {
                        //throw new InvalidDataException(string.Format("Unknown chunk {0} in DZS/DZR file.", cHeader.Tag));
                        Console.WriteLine("Unknown chunk {0} in DZS/DZR file.", cHeader.Tag);
                        continue;
                    }

                    reader.BaseStream.Seek(cHeader.ChunkOffset, SeekOrigin.Begin);
                    chunk.LoadData(reader);
                }
            }
        }
        #endregion
    }
}