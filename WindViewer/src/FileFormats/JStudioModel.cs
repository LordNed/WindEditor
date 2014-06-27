using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor;
using WindViewer.Editor.Renderer;
using WindViewer.Forms;

namespace WindViewer.FileFormats
{
    /// <summary>
    /// The behemoth of file formats, the JStudioModel. Well I think it's a JStudioModel, it's a J3D and their tools are called JStudio, 
    /// so good enough. Anyways. It renders things.
    /// 
    /// Many thanks to @Drakonite, shuffle2, JMC47, Sage of Mirrors, Jasper, and phire for helping me understand the various
    /// bits and pieces of the format, and some debugging ideas!
    /// </summary>
    public class JStudioModel : BaseArchiveFile, IRenderable
    {
        public enum ArrayTypes
        {
            PositionMatrixIndex, Tex0MatrixIndex, Tex1MatrixIndex, Tex2MatrixIndex, Tex3MatrixIndex,
            Tex4MatrixIndex, Tex5MatrixIndex, Tex6MatrixIndex, Tex7MatrixIndex,
            Position, Normal, Color0, Color1, Tex0, Tex1, Tex2, Tex3, Tex4, Tex5, Tex6, Tex7,
            PositionMatrixArray, NormalMatrixArray, TextureMatrixArray, LitMatrixArray, NormalBinormalTangent,
            MaxAttr, NullAttr = 0xFF,
        }

        public enum DataTypes
        {
            Unsigned8 = 0x0,
            Signed8 = 0x1,
            Unsigned16 = 0x2,
            Signed16 = 0x3,
            Float32 = 0x4,
            Rgba8 = 0x5,
        }

        public enum TextureFormats
        {
            Rgb565 = 0x0,
            Rgb888 = 0x1,
            Rgbx8 = 0x2,
            Rgba4 = 0x3,
            Rgba6 = 0x4,
            Rgba8 = 0x5,
        }

        public enum PrimitiveTypes
        {
            Points = 0xB8,
            Lines = 0xA8,
            LineStrip = 0xB0,
            Triangles = 0x80,
            TriangleStrip = 0x98,
            TriangleFan = 0xA0,
            Quads = 0x80,
        }

        public enum HierarchyDataTypes : ushort
        {
            Finish = 0x0, NewNode = 0x01, EndNode = 0x02,
            Joint = 0x10, Material = 0x11, Batch = 0x12,
        }

        public class HierarchyData
        {
            public HierarchyDataTypes Type { get; private set; }
            public ushort Index { get; private set; }

            public void Load(byte[] data, uint offset)
            {
                Type = (HierarchyDataTypes)FSHelpers.Read16(data, (int)offset);
                Index = (ushort)FSHelpers.Read16(data, (int)offset + 0x2);
            }

            public override string ToString()
            {
                return string.Format("{0} [{1}]", Type, Index);
            }

            public const uint Size = 4;
        }

        public class VertexFormat
        {
            public ArrayTypes ArrayType { get; private set; }
            public uint ArrayCount { get; private set; }
            public DataTypes DataType { get; private set; }
            public byte DecimalPoint { get; private set; }

            public void Load(byte[] data, uint offset)
            {
                ArrayType = (ArrayTypes)FSHelpers.Read32(data, (int)offset);
                ArrayCount = (uint)FSHelpers.Read32(data, (int)offset + 4);
                DataType = (DataTypes)FSHelpers.Read32(data, (int)offset + 8);
                DecimalPoint = FSHelpers.Read8(data, (int)offset + 12);
            }

            public const int Size = 16;
        }

        public enum VertexDataTypes
        {
            Position = 0,
            Normal = 1,
            Color0 = 3,
            Tex0 = 5,
        }


        //Temp in case I fuck up
        private byte[] _origDataCache;

        private List<BaseChunk> _chunkList;

        public override void Load(byte[] data)
        {
            _origDataCache = data;

            _chunkList = new List<BaseChunk>();

            int dataOffset = 0;

            var header = new Header();
            header.Load(data, ref dataOffset);

            //STEP 1: We're going to load all of the data out of memory straight into the chunks that
            //hold them. These should be relatively close to accurate copies of the file format, but
            //the Wiki is probably a better place to draw that information from.
            for (int i = 0; i < header.ChunkCount; i++)
            {
                BaseChunk baseChunk;

                //Read the first four bytes to get the tag.
                string tagName = FSHelpers.ReadString(data, dataOffset, 4);

                switch (tagName)
                {
                    case "INF1":
                        baseChunk = new Inf1Chunk();
                        break;
                    case "VTX1":
                        baseChunk = new Vtx1Chunk();
                        break;
                    case "EVP1":
                        baseChunk = new Evp1Chunk();
                        break;
                    case "DRW1":
                        baseChunk = new Drw1Chunk();
                        break;
                    case "JNT1":
                        baseChunk = new Jnt1Chunk();
                        break;
                    case "SHP1":
                        baseChunk = new Shp1Chunk();
                        break;
                    case "TEX1":
                        baseChunk = new Tex1Chunk();
                        break;
                    case "MAT3":
                        baseChunk = new Mat3Chunk();
                        break;
                    case "ANK1":
                    default:
                        Console.WriteLine("Found unknown chunk {0}!", tagName);
                        baseChunk = new DefaultChunk();
                        break;
                }

                baseChunk.Load(data, ref dataOffset);
                _chunkList.Add(baseChunk);
            }

            _enabledVertexAttribs = GetEnabledVertexAttribs();

            //STEP 2: Once all of the data is loaded, we're going to pull different data from
            //different chunks to transform the data into something
            vertData = BuildVertexArraysFromFile(data);


            //Haaaaaaaack there goes my lung. Generate a vbo, bind and upload data.
            GL.GenBuffers(1, out _glVbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Count * 36), vertData.ToArray(), BufferUsageHint.StaticDraw);

            J3DRenderer.Draw += J3DRendererOnDraw;
            J3DRenderer.Bind += J3DRendererOnBind;

            _sceneGraph = BuildSceneGraphFromInfo(GetChunkByType<Inf1Chunk>());

            //IterateSceneGraphRecursive(_sceneGraph);
            J3DRenderer.Instance.AddRenderable(this);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
        }

        public void Draw(BaseRenderer renderer)
        {
            /* Since our VertexFormat doesn't change, we're going to bind our global
             * buffer for the object, and then set the VertexAttribPointers for the 
             * VertexFormat. Each batch can then enable/disable the VertexAttribs
             * they use. */

            //ToDo: Move this to the J3DREnderer




            /*GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position);
            GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Color);
            GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.TexCoord);*/

            /* Recursively iterate through the J3D scene graph to bind and draw all
             * of the batches within the J3D model. */
            DrawModelRecursive(_sceneGraph, renderer);

            /*GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position);
            GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Color);
            GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.TexCoord);*/
        }

        private void DrawModelRecursive(SceneGraph curNode, BaseRenderer renderer)
        {
            switch (curNode.NodeType)
            {
                case HierarchyDataTypes.Material:
                    GL.BindTexture(TextureTarget.Texture2D, GetGLTexIdFromCache(curNode.DataIndex));
                    break;

                case HierarchyDataTypes.Batch:
                    /* For each batch, we're going to enable the
                         * appropriate Vertex Attributes for that batch
                         * and set default values for vertex attribs that
                         * the batch doesn't use, then draw all primitives
                         * within it.*/
                    SetVertexAttribArraysForBatch(true, curNode.DataIndex);

                    foreach (var primitive in _renderList[curNode.DataIndex])
                    {
                        GL.DrawArrays(primitive.DrawType, primitive.VertexStart, primitive.VertexCount);
                    }

                    SetVertexAttribArraysForBatch(false, curNode.DataIndex);
                    break;
            }

            foreach (SceneGraph subNode in curNode.Children)
            {
                DrawModelRecursive(subNode, renderer);
            }
        }

        private void SetVertexAttribArraysForBatch(bool bEnabled, ushort batchIndex)
        {
            Shp1Chunk shp1 = GetChunkByType<Shp1Chunk>();
            List<Shp1Chunk.BatchAttribute> attributes = shp1.GetAttributesForBatch(batchIndex);

            foreach (var attribute in attributes)
            {
                if (bEnabled)
                {
                    switch (attribute.AttribType)
                    {
                        case ArrayTypes.Position: GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position); break;
                        case ArrayTypes.Color0: GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Color); break;
                        case ArrayTypes.Tex0: GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.TexCoord); break;
                    }
                }
                else
                {
                    switch (attribute.AttribType)
                    {
                        case ArrayTypes.Position: GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position); break;
                        case ArrayTypes.Color0: GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Color); break;
                        case ArrayTypes.Tex0: GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.TexCoord); break;
                    }
                }
            }
        }

        private void IterateSceneGraphRecursive(SceneGraph curNode)
        {
            switch (curNode.NodeType)
            {
                case HierarchyDataTypes.Material:
                    //Console.WriteLine("Bind material index {0}", curNode.DataIndex);
                    //lol this is so wrong, but maybe something will not explode.
                    GL.BindTexture(TextureTarget.Texture2D, GetGLTexIdFromCache(curNode.DataIndex));
                    break;
                case HierarchyDataTypes.Batch:
                    //Console.WriteLine("Draw shape index {0}", curNode.DataIndex);

                    foreach (PrimitiveList prim in _renderList[curNode.DataIndex])
                    {
                        //if (_numPrimsDrawn < _numPrimToRender)
                        GL.DrawArrays(prim.DrawType, prim.VertexStart, prim.VertexCount);
                        _numPrimsDrawn++;
                    }
                    break;
                case HierarchyDataTypes.Joint:
                    Jnt1Chunk jnt1Chunk = GetChunkByType<Jnt1Chunk>();
                    var joint = jnt1Chunk.GetJoint(curNode.DataIndex);
                    Vector3 jointRot = joint.GetRotation().ToDegrees();

                    Matrix4 tranMatrix = Matrix4.CreateTranslation(joint.GetTranslation());
                    Matrix4 rotMatrix = Matrix4.CreateRotationX(jointRot.X) * Matrix4.CreateRotationY(jointRot.Y) *
                                        Matrix4.CreateRotationZ(jointRot.Z);
                    Matrix4 scaleMatrix = Matrix4.CreateScale(joint.GetScale());

                    Matrix4 modelMatrix = tranMatrix * rotMatrix * scaleMatrix;

                    int uniformId;
                    Matrix4 viewProj;
                    J3DRenderer.Instance.GetCamMatrix(out uniformId, out viewProj);
                    Matrix4 finalMatrix = modelMatrix * viewProj;

                    GL.UniformMatrix4(uniformId, false, ref finalMatrix);

                    break;
            }

            foreach (SceneGraph subNode in curNode.Children)
            {
                IterateSceneGraphRecursive(subNode);
            }
        }

        private List<VertexDataTypes> GetEnabledVertexAttribs()
        {
            var vtxChunk = GetChunkByType<Vtx1Chunk>();

            var enabledVertexAttribs = new List<VertexDataTypes>();
            /*for (int i = 0; i < vtxChunk.GetAllVertexFormats().Count; i++)
            {
                //if (vtxChunk.VertexDataOffsets[i] == 0)
                    //continue;
                enabledVertexAttribs.Add((VertexDataTypes)i);
            }*/

            return enabledVertexAttribs;
        }


        private void BindEnabledVertexAttribs()
        {
            //Pre-build the stride size.
            int stride = 0;

            for (int i = 0; i < _enabledVertexAttribs.Count; i++)
            {
                stride += GetElementSizeFromAttrib(_enabledVertexAttribs[i]);
            }

            int ongoingOffset = 0;

            foreach (var attrib in _enabledVertexAttribs)
            {
                switch (attrib)
                {
                    case VertexDataTypes.Position:
                        GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position);
                        GL.VertexAttribPointer((int)BaseRenderer.ShaderAttributeIds.Position, 3,
                            VertexAttribPointerType.Float, false, stride, ongoingOffset);
                        ongoingOffset += GetElementSizeFromAttrib(attrib);
                        break;

                    case VertexDataTypes.Color0:
                        GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Color);
                        GL.VertexAttribPointer((int)BaseRenderer.ShaderAttributeIds.Color, 4,
                            VertexAttribPointerType.Float, false, stride, ongoingOffset);
                        ongoingOffset += GetElementSizeFromAttrib(attrib);
                        break;

                    case VertexDataTypes.Tex0:
                        GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.TexCoord);
                        GL.VertexAttribPointer((int)BaseRenderer.ShaderAttributeIds.TexCoord, 2,
                            VertexAttribPointerType.Float, false, stride, ongoingOffset);
                        ongoingOffset += GetElementSizeFromAttrib(attrib);
                        break;
                }
            }
        }

        private int GetElementSizeFromAttrib(VertexDataTypes attrib)
        {
            switch (attrib)
            {
                case VertexDataTypes.Position: return 3 * 4;
                case VertexDataTypes.Normal: return 3 * 4;
                case VertexDataTypes.Color0: return 4 * 4;
                case VertexDataTypes.Tex0: return 2 * 4;
                default:
                    Console.WriteLine("Unknown vertex attrib type {0}!", attrib);
                    return 0;
            }
        }


        private SceneGraph _sceneGraph;
        private List<VertexDataTypes> _enabledVertexAttribs;
        private int _numPrimsDrawn;


        private SceneGraph BuildSceneGraphFromInfo(Inf1Chunk info)
        {
            if (info == null)
                return null;

            SceneGraph root = new SceneGraph();
            var hierarchyData = info.GetHierarchyData();

            BuildNodeRecursive(ref root, hierarchyData, 0);

            return root;
        }

        private int BuildNodeRecursive(ref SceneGraph parent, List<HierarchyData> nodeList, int listIndex)
        {
            for (int i = listIndex; i < nodeList.Count; ++i)
            {
                HierarchyData node = nodeList[i];
                SceneGraph newNode;

                switch (node.Type)
                {
                    //If it's a new node, push down in the stack one.
                    case HierarchyDataTypes.NewNode:
                        newNode = new SceneGraph(node.Type, node.Index);
                        i += BuildNodeRecursive(ref newNode, nodeList, i + 1);
                        parent.Children.Add(newNode);
                        break;

                    //If it's the end node, we need to go up.
                    case HierarchyDataTypes.EndNode:
                        return i - listIndex + 1;

                    //If it's a material, joint, or shape, just produce them.
                    case HierarchyDataTypes.Material:
                    case HierarchyDataTypes.Joint:
                    case HierarchyDataTypes.Batch:
                    case HierarchyDataTypes.Finish:
                        break;
                    default:
                        Console.WriteLine("You broke something.");
                        break;
                }

                //Update what we're about to add, because NewNodes can change it.
                node = nodeList[i];
                parent.Children.Add(new SceneGraph(node.Type, node.Index));
            }

            return 0;
        }

        private List<VertexFormatLayout> vertData;

        public override void Save(BinaryWriter stream)
        {
            stream.Write(_origDataCache);
        }

        private struct PrimitiveList
        {
            public int VertexStart;
            public int VertexCount;
            public PrimitiveType DrawType;
        }

        private Dictionary<int, List<PrimitiveList>> _renderList = new Dictionary<int, List<PrimitiveList>>();

        private void J3DRendererOnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            BindEnabledVertexAttribs();
        }

        private int _numPrimToRender;
        private float _lastChange;

        private void J3DRendererOnDraw()
        {
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            //GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 10);

            if (EditorHelpers.GetKey(Keys.O) && Math.Abs(MainEditor.Time - _lastChange) > 0.02f)
            {
                _numPrimToRender++;
                Console.WriteLine("Rendering: " + _numPrimToRender);
                _lastChange = MainEditor.Time;
            }
            if (EditorHelpers.GetKey(Keys.P) && Math.Abs(MainEditor.Time - _lastChange) > 0.02f)
            {
                _numPrimToRender--;
                if (_numPrimToRender < 0)
                    _numPrimToRender = 0;
                Console.WriteLine("Rendering: " + _numPrimToRender);
                _lastChange = MainEditor.Time;
            }
            if (EditorHelpers.GetKey(Keys.I))
                _numPrimToRender = _renderList.Count - 1;

            _numPrimsDrawn = 0;
            IterateSceneGraphRecursive(_sceneGraph);
            /*for (int i = 0; i < Math.Min(_numPrimRender, _renderList.Count); i++)
            {
                PrimitiveList primitive = _renderList[i];
                GL.DrawArrays(PrimitiveType.TriangleStrip, primitive.VertexStart, primitive.VertexCount);
            }*/
        }

        private int _glVbo;

        public T GetChunkByType<T>() where T : class
        {
            return _chunkList.OfType<T>().Select(file => file).FirstOrDefault();
        }

        public struct VertexFormatLayout
        {
            public Vector3 Position;
            // Vector3 Normal;
            public Vector4 Color;
            public Vector2 TexCoord;
        }


        private List<VertexFormatLayout> BuildVertexArraysFromFile(byte[] data)
        {
            Vtx1Chunk vtxChunk = GetChunkByType<Vtx1Chunk>();
            List<VertexFormatLayout> finalData = new List<VertexFormatLayout>();

            Shp1Chunk shp1Chunk = GetChunkByType<Shp1Chunk>();
            Drw1Chunk drw1Chunk = GetChunkByType<Drw1Chunk>();
            Jnt1Chunk jnt1Chunk = GetChunkByType<Jnt1Chunk>();

            //Now, let's try to get our data.
            for (uint i = 0; i < shp1Chunk.GetBatchCount(); i++)
            {
                Shp1Chunk.Batch batch = shp1Chunk.GetBatch(i);

                Console.WriteLine("[{0}] Unk0: {5}, Attb: {6} Mtx Type: {1} #Packets {2}[{3}] Matrix Index: {4}", i, batch.MatrixType, batch.PacketCount, batch.PacketIndex, batch.FirstMatrixIndex, batch.Unknown0, batch.AttribOffset);


                uint attributeCount = 0;
                for (uint attribIndex = 0; attribIndex < 13; attribIndex++)
                {
                    Shp1Chunk.BatchAttribute attrib = shp1Chunk.GetAttribute(attribIndex, batch.AttribOffset);
                    if (attrib.AttribType == ArrayTypes.NullAttr)
                        break;

                    attributeCount++;
                }

                /*bool isWeighted = drw1Chunk.IsWeighted(batch.FirstMatrixIndex);
                if (!isWeighted)
                {
                    ushort jointIndex = drw1Chunk.GetIndex(batch.FirstMatrixIndex);
                    var joint = jnt1Chunk.GetJoint(jointIndex);
                    Console.WriteLine(joint);
                }*/

                _renderList[(int)i] = new List<PrimitiveList>();
                for (uint p = 0; p < batch.PacketCount; p++)
                {
                    Shp1Chunk.BatchPacketLocation packetLoc = shp1Chunk.GetBatchPacketLocation(batch.PacketIndex + p);

                    uint numPrimitiveBytesRead = packetLoc.Offset;

                    while (numPrimitiveBytesRead < packetLoc.Offset + packetLoc.PacketSize)
                    {
                        //The data is going to be stored as:
                        //[Primitive][Primitive.VertexCount * (AttributeType.ElementCount * sizeof(AttributeType.DataType))]

                        Shp1Chunk.BatchPrimitive primitive = new Shp1Chunk.BatchPrimitive();
                        primitive.Load(shp1Chunk._dataCopy, shp1Chunk._primitiveDataOffset + numPrimitiveBytesRead);
                        numPrimitiveBytesRead += Shp1Chunk.BatchPrimitive.Size;

                        //Game pads the chunks out with zeros, so this is signal for early break.
                        if (primitive.Type == 0)
                        {
                            break;
                        }

                        var primList = new PrimitiveList();
                        primList.VertexCount = primitive.VertexCount;
                        primList.VertexStart = finalData.Count;
                        primList.DrawType = primitive.Type == PrimitiveTypes.TriangleStrip ? PrimitiveType.TriangleStrip : PrimitiveType.TriangleFan;

                        if (primitive.Type != PrimitiveTypes.TriangleStrip)
                            Console.WriteLine("not: " + primitive.Type);

                        _renderList[(int)i].Add(primList);


                        //Todo: that's pretty shitty too.

                        //Now, for each Vertex we're going to read the right number of bytes... we're hacking it in this case
                        //to fixed amount of 8...

                        for (int vert = 0; vert < primitive.VertexCount; vert++)
                        {
                            VertexFormatLayout newVertex = new VertexFormatLayout();
                            for (uint vertIndex = 0; vertIndex < attributeCount; vertIndex++)
                            {
                                ushort curIndex =
                                    (ushort)FSHelpers.Read16(shp1Chunk._dataCopy, (int)(shp1Chunk._primitiveDataOffset + numPrimitiveBytesRead));

                                var batchAttrib = shp1Chunk.GetAttribute(vertIndex, batch.AttribOffset);

                                if (GetAttribElementSize(batchAttrib.DataType) == 1)
                                {
                                    curIndex =
                                        (ushort)
                                            FSHelpers.Read8(shp1Chunk._dataCopy,
                                                (int)(shp1Chunk._primitiveDataOffset + numPrimitiveBytesRead));
                                }


                                ArrayTypes attribType = batchAttrib.AttribType;

                                switch (attribType)
                                {
                                    case ArrayTypes.Position:
                                        newVertex.Position = vtxChunk.GetPosition(curIndex);
                                        break;
                                    case ArrayTypes.Normal:
                                        newVertex.Color = new Vector4(vtxChunk.GetNormal(curIndex, 14), 1); //temp
                                        break;
                                    case ArrayTypes.Color0:
                                        newVertex.Color = vtxChunk.GetColor0(curIndex);
                                        break;
                                    case ArrayTypes.Tex0:
                                        newVertex.TexCoord = vtxChunk.GetTex0(curIndex, 8);
                                        break;


                                }

                                numPrimitiveBytesRead += GetAttribElementSize(batchAttrib.DataType);
                            }

                            //Add our vertex to our list of Vertexes
                            finalData.Add(newVertex);
                        }
                    }
                }

                //Console.WriteLine("Finished batch {0}, triangleStrip count: {1}", i, _renderList.Count);
            }

            return finalData;
        }

        #region File Formats

        private class Header
        {
            public string Magic;
            public string Type;
            public int FileSize;
            public int ChunkCount;

            public void Load(byte[] data, ref int offset)
            {
                Magic = FSHelpers.ReadString(data, 0, 4);
                Type = FSHelpers.ReadString(data, 4, 4);
                FileSize = FSHelpers.Read32(data, 8);
                ChunkCount = FSHelpers.Read32(data, 12);

                //Four variables are followed by an unused tag and some padding.
                offset += 32;
            }

            public void Save(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Magic, 4);
                FSHelpers.WriteString(stream, Type, 4);
                FSHelpers.Write32(stream, FileSize);
                FSHelpers.Write32(stream, ChunkCount);

                //Write in the unused tag and padding
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0x53565233, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFFFFFF, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFFFFFF, 4));
                FSHelpers.WriteArray(stream, FSHelpers.ToBytes(0xFFFFFFFF, 4));
            }
        }

        private class BaseChunk
        {
            public string Type;
            public int ChunkSize;

            public byte[] _dataCopy;

            public virtual void Load(byte[] data, ref int offset)
            {
                Type = FSHelpers.ReadString(data, offset, 4);
                ChunkSize = FSHelpers.Read32(data, offset + 4);

                _dataCopy = FSHelpers.ReadN(data, offset, ChunkSize);
            }

            public virtual void Save(BinaryWriter stream)
            {
                FSHelpers.WriteString(stream, Type, 4);
                FSHelpers.Write32(stream, ChunkSize);
            }
        }

        /// <summary>
        /// Used for undefined chunks to properly incriment the data stream
        /// as we read in.
        /// </summary>
        private class DefaultChunk : BaseChunk
        {
            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                offset += ChunkSize;
            }
        }

        private class Inf1Chunk : BaseChunk
        {
            /* All other chunks have a 4 byte tag, a 4 byte chunk size, and then
             * a 2 byte "entry" count (+ 2 more bytes padding) at the start. This
             * chunk doesn't seem to ever initialize the entry count and the only 
             * varying data in this chunk seems uses a different method of finding
             * the last entry. */
            private ushort _unusedEntryCount; //Not unused, a lot of Links models have it.
            private uint _batchCount;
            private uint _vertexCount;
            private uint _hierarchyDataOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _unusedEntryCount = (ushort)FSHelpers.Read16(data, offset + 8);
                _batchCount = (uint)FSHelpers.Read32(data, offset + 12); //2 bytes padding after _unusedEntryCount
                _vertexCount = (uint)FSHelpers.Read32(data, offset + 16);
                _hierarchyDataOffset = (uint)FSHelpers.Read32(data, offset + 20);

                offset += ChunkSize;
            }

            public uint GetVertexCount()
            {
                return _vertexCount;
            }

            public uint GetBatchCount()
            {
                return _batchCount;
            }

            public List<HierarchyData> GetHierarchyData()
            {
                var data = new List<HierarchyData>();
                HierarchyData curNode;
                uint readOffset = 0;

                do
                {
                    curNode = new HierarchyData();
                    curNode.Load(_dataCopy, _hierarchyDataOffset + readOffset);
                    data.Add(curNode);

                    readOffset += HierarchyData.Size;
                } while (curNode.Type != HierarchyDataTypes.Finish);

                return data;
            }
        }

        private class Vtx1Chunk : BaseChunk
        {
            private uint _vertexFormatsOffset;
            private uint _positionDataOffset;
            private uint _normalDataOffset;
            private uint _normalBinormalTangentDataOffset; //Presumed
            private uint _color0DataOffset;
            private uint _color1DataOffset; //Presumed
            private uint _tex0DataOffset;
            private uint _tex1DataOffset; //Presumed
            private uint _tex2DataOffset; //Presumed
            private uint _tex3DataOffset; //Presumed
            private uint _tex4DataOffset; //Presumed
            private uint _tex5DataOffset; //Presumed
            private uint _tex6DataOffset; //Presumed
            private uint _tex7DataOffset; //Presumed

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _vertexFormatsOffset = (uint)FSHelpers.Read32(data, offset + 0x8);
                _positionDataOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _normalDataOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _normalBinormalTangentDataOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                _color0DataOffset = (uint)FSHelpers.Read32(data, offset + 0x18);
                _color1DataOffset = (uint)FSHelpers.Read32(data, offset + 0x1C);
                _tex0DataOffset = (uint)FSHelpers.Read32(data, offset + 0x20);
                _tex1DataOffset = (uint)FSHelpers.Read32(data, offset + 0x24);
                _tex2DataOffset = (uint)FSHelpers.Read32(data, offset + 0x28);
                _tex3DataOffset = (uint)FSHelpers.Read32(data, offset + 0x2C);
                _tex4DataOffset = (uint)FSHelpers.Read32(data, offset + 0x30);
                _tex5DataOffset = (uint)FSHelpers.Read32(data, offset + 0x34);
                _tex6DataOffset = (uint)FSHelpers.Read32(data, offset + 0x38);
                _tex7DataOffset = (uint)FSHelpers.Read32(data, offset + 0x3C);

                offset += ChunkSize;
            }

            public VertexFormat GetVertexFormat(uint index)
            {
                VertexFormat vtxFmt = new VertexFormat();
                vtxFmt.Load(_dataCopy, _vertexFormatsOffset + (index * VertexFormat.Size));
                return vtxFmt;
            }


            public List<VertexFormat> GetAllVertexFormats()
            {
                var allFormats = new List<VertexFormat>();
                VertexFormat vFormat;
                uint dataOffset = _vertexFormatsOffset;
                do
                {
                    vFormat = new VertexFormat();
                    vFormat.Load(_dataCopy, dataOffset);
                    allFormats.Add(vFormat);

                    dataOffset += VertexFormat.Size;
                } while (vFormat.ArrayType != ArrayTypes.NullAttr);

                return allFormats;
            }

            public Vector3 GetPosition(uint index)
            {
                Vector3 newPos = new Vector3();
                for (int i = 0; i < 3; i++)
                    newPos[i] = FSHelpers.ReadFloat(_dataCopy, (int)(_positionDataOffset + (index * Vector3.SizeInBytes) + (i * 4)));

                return newPos;
            }

            public Vector3 GetNormal(uint index, int decimalPlace)
            {
                Vector3 newNormal = new Vector3();
                float scaleFactor = (float)Math.Pow(0.5, decimalPlace);

                for (int i = 0; i < 3; i++)
                    newNormal[i] = FSHelpers.Read16(_dataCopy,
                        (int)(_normalDataOffset + (index * 6) + (i * 2))) * scaleFactor;

                return newNormal;
            }

            public Vector4 GetColor0(int index)
            {
                Vector4 newColor = new Vector4();
                for (int i = 0; i < 4; i++)
                    newColor[i] = FSHelpers.Read8(_dataCopy, (int)_color0DataOffset + (index * 4) + i) / 255f;

                return newColor;
            }

            public Vector2 GetTex0(int index, int decimalPlace)
            {
                Vector2 newTexCoord = new Vector2();
                float scaleFactor = (float)Math.Pow(0.5, decimalPlace);

                for (int i = 0; i < 2; i++)
                    newTexCoord[i] = FSHelpers.Read16(_dataCopy, (int)_tex0DataOffset + (index * 4) + (i * 0x2)) * scaleFactor;

                return newTexCoord;
            }

        }

        private class Evp1Chunk : BaseChunk
        {
            private ushort _sectionCount; //BST has 3 
            private uint _countsArrayOffset; //BST values: 2, 2, 2
            private uint _indicesOffset; //BST's is 12 bytes in length. (six shorts) 06 07 00 06 00 07 (pairs?)
            private uint _weightsOffset; //24 bytes length, 6 floats. (one per indice) (pairs + weights to each side of the pair)
            private uint _matrixDataOffset; //3x4 float array. Indeded into by something else. 15 in BST

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _sectionCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _countsArrayOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _indicesOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _weightsOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                _matrixDataOffset = (uint)FSHelpers.Read32(data, offset + 0x18);


                offset += ChunkSize;
            }

            public byte GetCount(uint index)
            {
                return FSHelpers.Read8(_dataCopy, (int)(_countsArrayOffset + index));
            }

            public ushort GetIndex(uint index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)(_indicesOffset + (index * 0x2)));
            }

            public float GetWeight(uint index)
            {
                return FSHelpers.ReadFloat(_dataCopy, (int)(_weightsOffset + (index * 0x4)));
            }

            public Matrix3x4 GetMatrix(ushort index)
            {
                Matrix3x4 matrix = new Matrix3x4();
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        float rawFloat = FSHelpers.ReadFloat(_dataCopy, (int)_matrixDataOffset + (index * (3 * 4 * 4)) + ((row * 4 * 4) + (col * 4)));
                        matrix[row, col] = (float)Math.Round(rawFloat, 4);
                    }
                }

                return matrix;
            }
        }

        private class Drw1Chunk : BaseChunk
        {
            private ushort _sectionCount;
            private uint _isWeightedOffset;
            private uint _dataOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _sectionCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _isWeightedOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _dataOffset = (uint)FSHelpers.Read32(data, offset + 0x10);

                offset += ChunkSize;

                for (ushort i = 0; i < _sectionCount; i++)
                {
                    Console.WriteLine("[{0}] - {1} / {2}", i, IsWeighted(i), GetIndex(i));
                }
            }

            public bool IsWeighted(ushort index)
            {
                return Convert.ToBoolean(FSHelpers.Read8(_dataCopy, (int)_isWeightedOffset + index));
            }

            public ushort GetIndex(ushort index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_dataOffset + (index * 0x2));
            }
        }

        private class Jnt1Chunk : BaseChunk
        {
            private ushort _jointCount;
            private uint _entryOffset;
            private uint _stringIdTableOffset;
            private uint _stringTableOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _jointCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _entryOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _stringIdTableOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _stringTableOffset = (uint)FSHelpers.Read32(data, offset + 0x14);

                offset += ChunkSize;

                for (ushort i = 0; i < _jointCount; i++)
                {
                    JntEntry jnt = GetJoint(i);
                    Console.WriteLine("[{0}] - {1} / {2} ({3})", i, jnt.GetUnknown1(), jnt.GetUnknown2(),
                        GetString(GetStringTableEntry(i)));
                }
            }

            public ushort GetUnknown(ushort index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_stringIdTableOffset + (index * 0x2));
            }

            public JntEntry GetJoint(ushort index)
            {
                JntEntry joint = new JntEntry();
                joint.Load(_dataCopy, _entryOffset + (index * JntEntry.Size));

                return joint;
            }

            public ushort GetStringIndex(ushort index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_stringIdTableOffset + (index * 0x2));
            }

            public ushort GetStringTableSize()
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)_stringTableOffset);
            }

            public StringTableEntry GetStringTableEntry(ushort index)
            {
                var ste = new StringTableEntry();
                /* String Table Header */
                ste.UnknownIndex = (ushort)FSHelpers.Read16(_dataCopy, (int)_stringTableOffset + 0x4 + (index * 0x4));
                ste.StringOffset = (ushort)FSHelpers.Read16(_dataCopy, (int)_stringTableOffset + 0x6 + (index * 0x4));

                return ste;
            }

            public string GetString(StringTableEntry entry)
            {
                return FSHelpers.ReadString(_dataCopy, (int)_stringTableOffset + entry.StringOffset);
            }

            public struct StringTableEntry
            {
                public ushort UnknownIndex;
                public ushort StringOffset;

                public override string ToString()
                {
                    return string.Format("[{0}] - {1}", UnknownIndex, StringOffset);
                }
            }

            public class JntEntry
            {
                private ushort _unknown1; //0 = has unknown2, bboxmin, bboxmax. 
                private byte _unknown2; //Unknown
                private Vector3 _scale;
                private HalfRotation _rotation;
                private Vector3 _translation;
                private float _unknown3;
                private Vector3 _boundingBoxMin;
                private Vector3 _boundingBoxMax;

                public void Load(byte[] data, uint offset)
                {
                    _unknown1 = (ushort)FSHelpers.Read16(data, (int)offset + 0x0);
                    //One byte padding.
                    _unknown2 = (byte)FSHelpers.Read16(data, (int)offset + 0x3);
                    _scale = FSHelpers.ReadVector3(data, (int)offset + 0x4);
                    _rotation = FSHelpers.ReadHalfRot(data, offset + 0x10);
                    //2 bytes padding
                    _translation = FSHelpers.ReadVector3(data, (int)offset + 0x18);
                    _unknown3 = FSHelpers.ReadFloat(data, (int)offset + 0x24);
                    _boundingBoxMin = FSHelpers.ReadVector3(data, (int)offset + 0x28);
                    _boundingBoxMax = FSHelpers.ReadVector3(data, (int)offset + 0x34);
                }

                public ushort GetUnknown1()
                {
                    return _unknown1;
                }

                public byte GetUnknown2()
                {
                    return _unknown2;
                }

                public Vector3 GetScale()
                {
                    return _scale;
                }

                public HalfRotation GetRotation()
                {
                    return _rotation;
                }

                public Vector3 GetTranslation()
                {
                    return _translation;
                }

                public float GetUnknownFloat()
                {
                    return _unknown3;
                }

                public Vector3 GetBoundingBoxMin()
                {
                    return _boundingBoxMin;
                }

                public Vector3 GetBoundingBoxMax()
                {
                    return _boundingBoxMax;
                }

                public const uint Size = 64;
            }

        }

        /// <summary>
        /// Since primitives have different attributes (ie: some have position, some have normals, some have texcoords)
        /// each different primitive is placed into a "batch", and each batch ahs a fixed set of vertex attributes. Each 
        /// batch can then have several "packets", which are used for animations. A packet is a collection of primitives.
        /// </summary>
        private class Shp1Chunk : BaseChunk
        {
            public class Batch
            {
                public byte MatrixType;
                public byte Unknown0;
                public ushort PacketCount;
                public ushort AttribOffset;
                public ushort FirstMatrixIndex;
                public ushort PacketIndex;

                public float Unknown;
                public Vector3 BoundingBoxMin;
                public Vector3 BoundingBoxMax;

                //Bleh
                public List<BatchAttribute> BatchAttributes = new List<BatchAttribute>();

                public void Load(byte[] data, uint offset)
                {
                    MatrixType = FSHelpers.Read8(data, (int)offset);
                    Unknown0 = FSHelpers.Read8(data, (int)offset + 1);
                    PacketCount = (ushort)FSHelpers.Read16(data, (int)offset + 0x2);
                    AttribOffset = (ushort)FSHelpers.Read16(data, (int)offset + 0x4);
                    FirstMatrixIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x6);
                    PacketIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x8);

                    Unknown = FSHelpers.ReadFloat(data, (int)offset + 0xC);
                    BoundingBoxMin = FSHelpers.ReadVector3(data, (int)offset + 0x10);
                    BoundingBoxMax = FSHelpers.ReadVector3(data, (int)offset + 0x1C);
                }

                public const uint Size = 40;
            }

            public class BatchAttribute
            {
                public ArrayTypes AttribType;
                public DataTypes DataType;

                public void Load(byte[] data, uint offset)
                {
                    AttribType = (ArrayTypes)FSHelpers.Read32(data, (int)offset);
                    DataType = (DataTypes)FSHelpers.Read32(data, (int)offset + 0x4);
                }

                public const uint Size = 8;
            }

            public struct BatchPacketLocation
            {
                public uint PacketSize;
                public uint Offset;

                //Not part of the BPLocation header
                public const uint Size = 8;
            }

            public class BatchPrimitive
            {
                public PrimitiveTypes Type;
                public ushort VertexCount;

                public void Load(byte[] data, uint offset)
                {
                    Type = (PrimitiveTypes)FSHelpers.Read8(data, (int)offset);
                    VertexCount = (ushort)FSHelpers.Read16(data, (int)offset + 0x1);
                }

                public const uint Size = 3;
            }

            private ushort _batchCount;
            private uint _batchDataOffset;
            private uint _unknownTableOffset;
            private uint _zero;
            private uint _attributeOffset;
            private uint _matrixTableOffset;
            public uint _primitiveDataOffset;
            private uint _matrixDataOffset;
            private uint _packetLocationOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                //Load information from header
                _batchCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                _batchDataOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _unknownTableOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _zero = (uint)FSHelpers.Read32(data, offset + 0x14);
                _attributeOffset = (uint)FSHelpers.Read32(data, offset + 0x18);
                _matrixTableOffset = (uint)FSHelpers.Read32(data, offset + 0x1C);
                _primitiveDataOffset = (uint)FSHelpers.Read32(data, offset + 0x20);
                _matrixDataOffset = (uint)FSHelpers.Read32(data, offset + 0x24);
                _packetLocationOffset = (uint)FSHelpers.Read32(data, offset + 0x28);

                offset += ChunkSize;
            }

            public Batch GetBatch(uint index)
            {
                Batch newBatch = new Batch();
                newBatch.Load(_dataCopy, _batchDataOffset + (index * Batch.Size));

                return newBatch;
            }

            public ushort GetBatchCount()
            {
                return _batchCount;
            }

            public BatchPacketLocation GetBatchPacketLocation(uint index)
            {
                BatchPacketLocation newBp = new BatchPacketLocation();
                newBp.PacketSize = (uint)FSHelpers.Read32(_dataCopy, (int)(_packetLocationOffset + (index * BatchPacketLocation.Size) + 0x0));
                newBp.Offset = (uint)FSHelpers.Read32(_dataCopy, (int)(_packetLocationOffset + (index * BatchPacketLocation.Size) + 0x4));

                return newBp;
            }

            public BatchAttribute GetAttribute(uint attribIndex, uint attribOffset)
            {
                BatchAttribute newAttrib = new BatchAttribute();
                newAttrib.Load(_dataCopy, _attributeOffset + attribOffset + (attribIndex * BatchAttribute.Size));

                return newAttrib;
            }

            public ushort GetUnknown(uint index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)(_unknownTableOffset + (index * 0x2)));
            }

            public List<BatchAttribute> GetAttributesForBatch(ushort batchIndex)
            {
                Batch batch = GetBatch(batchIndex);
                List<BatchAttribute> attribs = new List<BatchAttribute>();

                BatchAttribute curAttrib;
                uint i = 0;
                do
                {
                    curAttrib = GetAttribute(i, batch.AttribOffset);
                    attribs.Add(curAttrib);
                    i++;

                } while (curAttrib.AttribType != ArrayTypes.NullAttr);

                return attribs;
            }
        }

        /// <summary>
        /// The Tex1 chunk stores a series of BTI images within the BMD/BDL format.
        /// The BMD/BDL also stores a string table which contains the file name for
        /// each image. The TEX1 chunk stores _textureCount headers immediately after
        /// the chunk, and then the data for those textures comes.
        /// </summary>
        private class Tex1Chunk : BaseChunk
        {
            private ushort _textureCount;
            private uint _textureHeaderOffset;
            private uint _stringTableOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);
                _textureCount = (ushort)FSHelpers.Read16(data, offset + 0x08);
                _textureHeaderOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _stringTableOffset = (uint)FSHelpers.Read32(data, offset + 0x10);

                offset += ChunkSize;

                for (uint i = 0; i < _textureCount; i++)
                {
                    BinaryTextureImage tex = GetTexture(i);
                }

            }

            public BinaryTextureImage GetTexture(uint index)
            {
                if (index > _textureCount)
                {
                    new Exception("Invalid index provided to GetTexture!");
                }

                BinaryTextureImage tex = new BinaryTextureImage();

                //Before load the texture we need to modify the source byte array, because reasons.
                uint headerOffset = ((index) * 32);

                tex.Load(_dataCopy, _textureHeaderOffset + headerOffset, headerOffset + 32);

                return tex;
            }

            public ushort GetTextureCount()
            {
                return _textureCount;
            }
        }

        private class Mat3Chunk : BaseChunk
        {
            private ushort _materialCount;
            private uint _materialInitDataOffset;
            private uint _materialIndexOffset; //"Us" ? Seems to index tex coords.
            private uint _stringTableOffset;
            private uint _indirectTextureOffset; //Indirect textures is using the output of of one TEV stage as tex coords for another.
            private uint _gxCullModeOffset;
            private uint _gxColorOffset;
            private uint _colorChannelNumOffset; //UC?
            private uint _colorChannelInfoOffset;
            private uint _gxColor2Offset;
            private uint _lightInfoOffset;
            private uint _texGenNumberOffset; //UC?
            private uint _texCoordInfoOffset;
            private uint _texCoordInfo2Offset;
            private uint _texMatrixInfoOffset;
            private uint _texMatrix2InfoOffset;
            private uint _texCoordOrMatrixOffset; //US? Related to _materialIndexOffsetS? Unknown.
            private uint _tevOrderInfoOffset;
            private uint _gxColorS10Offset; //(Signed 10-bit value)
            private uint _gxColor3Offset;
            private uint _tevStageNumInfoOffset; //UC?
            private uint _tevStageInfoOffset;
            private uint _tevSwapModeInfoOffset;
            private uint _tevSwapModeTableInfoOffset;
            private uint _fogInfoOffset;
            private uint _alphaCompareInfoOffset;
            private uint _blendInfoOffset;
            private uint _zModeInfoOffset; //Depth mode
            private uint _zCompareInfoOffset; //UC?
            private uint _ditherINfoOffset;
            private uint _nbtScaleInfoOffset;

            public override void Load(byte[] data, ref int offset)
            {
                base.Load(data, ref offset);

                _materialCount = (ushort)FSHelpers.Read16(data, offset + 0x8);
                //Two bytes padding
                _materialInitDataOffset = (uint)FSHelpers.Read32(data, offset + 0xC);
                _materialIndexOffset = (uint)FSHelpers.Read32(data, offset + 0x10);
                _stringTableOffset = (uint)FSHelpers.Read32(data, offset + 0x14);
                _indirectTextureOffset = (uint)FSHelpers.Read32(data, offset + 0x18);
                _gxCullModeOffset = (uint)FSHelpers.Read32(data, offset + 0x1C);
                _gxColorOffset = (uint)FSHelpers.Read32(data, offset + 0x20);
                _colorChannelNumOffset = (uint)FSHelpers.Read32(data, offset + 0x24);
                _colorChannelInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x28);
                _gxColor2Offset = (uint)FSHelpers.Read32(data, offset + 0x2C);
                _lightInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x30);
                _texGenNumberOffset = (uint)FSHelpers.Read32(data, offset + 0x34);
                _texCoordInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x38);
                _texCoordInfo2Offset = (uint)FSHelpers.Read32(data, offset + 0x3C);
                _texMatrixInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x40);
                _texMatrix2InfoOffset = (uint)FSHelpers.Read32(data, offset + 0x44);
                _texCoordOrMatrixOffset = (uint)FSHelpers.Read32(data, offset + 0x48);
                _tevOrderInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x4C);
                _gxColorS10Offset = (uint)FSHelpers.Read32(data, offset + 0x50);
                _gxColor3Offset = (uint)FSHelpers.Read32(data, offset + 0x54);
                _tevStageNumInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x58);
                _tevStageInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x5C);
                _tevSwapModeInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x60);
                _tevSwapModeTableInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x64);
                _fogInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x68);
                _alphaCompareInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x6C);
                _blendInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x70);
                _zModeInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x74);
                _zCompareInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x78);
                _ditherINfoOffset = (uint)FSHelpers.Read32(data, offset + 0x7C);
                _nbtScaleInfoOffset = (uint)FSHelpers.Read32(data, offset + 0x80);

                offset += ChunkSize;


                GetMaterialInitData(0);
            }

            public MaterialInitData GetMaterialInitData(uint index)
            {
                if (index > _materialCount)
                    throw new Exception("Invalid MaterialInit data requested.");

                MaterialInitData initData = new MaterialInitData();
                initData.Load(_dataCopy, _materialInitDataOffset + (index * MaterialInitData.Size));

                return initData;
            }

            public ushort GetMaterialIndex(uint index)
            {
                return (ushort)FSHelpers.Read16(_dataCopy, (int)(_texCoordOrMatrixOffset + (index * 0x2)));
            }


            public const int Size = 132;
        }

        //ToDo: These offsets are a little bit messed up.
        public class MaterialInitData
        {
            private byte _unknown1; //Read by PatchedMaterial, always 1?
            private byte _unknown2; //Mostly 0, sometimes 2.
            private ushort _padding1; //Always 0
            private ushort _indirectTexturingIndex;
            private ushort _cullModeIndex;
            private ushort[] _ambientColorIndex = new ushort[2]; //2 ushorts
            private ushort[] _colorChannelIndex = new ushort[4]; //4 ushorts
            private ushort[] _materialColorIndex = new ushort[2]; //2 ushorts
            private ushort[] _lightingIndex = new ushort[8]; //8 ushorts
            private ushort[] _texCoordIndex = new ushort[8]; //8 ushorts
            private ushort[] _texCoord2Index = new ushort[8]; //8 ushorts
            private ushort[] _texMatrixIndex = new ushort[8]; //8 ushorts
            private ushort[] _texMatrix2Index = new ushort[8]; //8 ushorts
            private ushort[] _textureIndex = new ushort[8]; //8 ushorts (diffuse textures)
            private ushort[] _tevConstantColorIndex = new ushort[4]; //4 ushorts
            private byte[] _constColorSel = new byte[16]; //16 (4 * RGBA?)
            private byte[] _constAlphaSel = new byte[16]; //16 (4 * RGBA?)
            private ushort[] _tevOrderIndex = new ushort[16]; //16 ushorts
            private ushort[] _tevColorIndex = new ushort[4]; //4 ushorts
            private ushort[] _tevStageInfoIndex = new ushort[16]; //16 ushorts
            private ushort[] _tevSwapModeInfoindex = new ushort[16]; //16 ushorts
            private ushort[] _tevSwapModeTableInfoindex = new ushort[4]; //4 ushorts
            private ushort[] _unconfirmedIndexes = new ushort[16]; //16 of them!

            public void Load(byte[] data, uint offset)
            {
                _unknown1 = FSHelpers.Read8(data, (int)offset + 0x0);
                _unknown2 = FSHelpers.Read8(data, (int)offset + 0x1);
                _padding1 = (ushort)FSHelpers.Read16(data, (int)offset + 0x2);
                _indirectTexturingIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x4);
                _cullModeIndex = (ushort)FSHelpers.Read16(data, (int)offset + 0x6);
                for (int i = 0; i < 2; i++)
                    _ambientColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x8 + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _colorChannelIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xC + (i * 0x2));
                for (int i = 0; i < 2; i++)
                    _materialColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x14 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _lightingIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x18 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texCoordIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x28 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texCoord2Index[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x38 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texMatrixIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x48 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _texMatrix2Index[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x58 + (i * 0x2));
                for (int i = 0; i < 8; i++)
                    _textureIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x84 + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _tevConstantColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x94 + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _constColorSel[i] = FSHelpers.Read8(data, (int)offset + 0x9C + (i * 0x1));
                for (int i = 0; i < 16; i++)
                    _constAlphaSel[i] = FSHelpers.Read8(data, (int)offset + 0xAC + (i * 0x1));
                for (int i = 0; i < 16; i++)
                    _tevOrderIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xBC + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _tevColorIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xDC + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _tevStageInfoIndex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0xE4 + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _tevSwapModeInfoindex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x104 + (i * 0x2));
                for (int i = 0; i < 4; i++)
                    _tevSwapModeTableInfoindex[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x124 + (i * 0x2));
                for (int i = 0; i < 16; i++)
                    _unconfirmedIndexes[i] = (ushort)FSHelpers.Read16(data, (int)offset + 0x12C + (i * 0x2));
            }

            public ushort GetTextureIndex(ushort index)
            {
                return _textureIndex[index];
            }

            public const int Size = 332;
        }

        #endregion

        #region Editor stuff to figure out later

        private Dictionary<int, int> _textureCache = new Dictionary<int, int>();
        private int GetGLTexIdFromCache(int j3dTextureId)
        {
            if (_textureCache.ContainsKey(j3dTextureId))
                return _textureCache[j3dTextureId];

            Console.WriteLine("Generating GL texture for id: " + j3dTextureId);

            //Look up the material first.
            Mat3Chunk matChunk = GetChunkByType<Mat3Chunk>();
            MaterialInitData matData = matChunk.GetMaterialInitData((uint)j3dTextureId);

            //If the texture cache doesn't contain the ID, we're going to load it here.
            Tex1Chunk texChunk = GetChunkByType<Tex1Chunk>();
            ushort textureIndex = matData.GetTextureIndex(0);
            if (textureIndex == 0xFF || textureIndex == 0xFFFF)
            {
                _textureCache[j3dTextureId] = 0;
                return 0;
            }
            BinaryTextureImage image = texChunk.GetTexture(matChunk.GetMaterialIndex(textureIndex));
            //image.WriteImageToFile("image_" + matChunk.GetMaterialIndex(matData.GetTextureIndex(0)) + ".png");

            int glTextureId;
            GL.GenTextures(1, out glTextureId);
            GL.BindTexture(TextureTarget.Texture2D, glTextureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, image.Width, image.Height, 0, PixelFormat.Bgra, PixelType.UnsignedInt8888Reversed, image.GetData());


            _textureCache[j3dTextureId] = glTextureId;
            return glTextureId;
        }

        private class SceneGraph
        {
            public List<SceneGraph> Children;
            public HierarchyDataTypes NodeType { get; private set; }
            public ushort DataIndex { get; private set; }

            public SceneGraph(HierarchyDataTypes type, ushort index)
            {
                Children = new List<SceneGraph>();
                NodeType = type;
                DataIndex = index;
            }

            public SceneGraph()
            {
                Children = new List<SceneGraph>();
                NodeType = 0;
                DataIndex = 0;
            }

            public override string ToString()
            {
                return string.Format("{0} [{1}]", NodeType, Children.Count);
            }
        }

        private uint GetAttribElementSize(DataTypes dataType)
        {
            switch (dataType)
            {
                case DataTypes.Unsigned8:
                case DataTypes.Signed8:
                    return 1;
                case DataTypes.Unsigned16:
                case DataTypes.Signed16:
                    return 2;
                case DataTypes.Float32:
                case DataTypes.Rgba8:
                    return 4;
            }

            Console.WriteLine("Unknown attrib datatype {0}, guessing at 2!", dataType);
            return 2;
        }

        #endregion
    }
}
