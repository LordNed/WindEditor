using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WindViewer.Editor.Renderer;
using WindViewer.FileFormats;

namespace WindViewer.Editor.WindWaker
{
    public class JStudioModel : BaseArchiveFile, IRenderable
    {
        private J3DFormat _file;
        private byte[] _dataCache;
        private int _glVbo;
        private SceneGraph _root;
        private Dictionary<int, List<PrimitiveList>> _renderList;
        private Dictionary<int, int> _textureCache;

        public bool Selected;

        public override void Load(byte[] data)
        {
            _file = new J3DFormat();
            _renderList = new Dictionary<int, List<PrimitiveList>>();
            _textureCache = new Dictionary<int, int>();
            _file.Load(data);
            _dataCache = data;

            Selected = false;

            //Extract the data from the file format into something we can use.
            var vertData = BuildVertexArraysFromFile();

            //Build our scene-graph so we can iterate through it.
            _root = BuildSceneGraphFromInfo(_file.Info);

            //Generate our VBO, and upload the data
            GL.GenBuffers(1, out _glVbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Count * 9 * 4), vertData.ToArray(),
                BufferUsageHint.StaticDraw);

            //Make sure we get drawn
            J3DRenderer.Instance.AddRenderable(this);
        }

        public override void Save(BinaryWriter stream)
        {
            stream.Write(_dataCache);
        }


        private struct PrimitiveList
        {
            public int VertexStart;
            public int VertexCount;
            public PrimitiveType DrawType;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
        }

        public void Draw(BaseRenderer renderer)
        {
            /* Recursively iterate through the J3D scene graph to bind and draw all
             * of the batches within the J3D model. */
            DrawModelRecursive(_root, renderer, false);

            if (Selected)
            {
                GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position);
                GL.LineWidth(2);
                GL.Disable(EnableCap.DepthTest);
                DrawModelRecursive(_root, renderer, true);
                GL.Enable(EnableCap.DepthTest);
                GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position);
                GL.LineWidth(1);
            }
        }

        private void DrawModelRecursive(SceneGraph curNode, BaseRenderer renderer, bool bSelectedPass)
        {
            switch (curNode.NodeType)
            {
                case J3DFormat.HierarchyDataTypes.Material:
                    if (!bSelectedPass)
                        GL.BindTexture(TextureTarget.Texture2D, GetGLTexIdFromCache(curNode.DataIndex));
                    break;

                case J3DFormat.HierarchyDataTypes.Batch:
                    /* For each batch, we're going to enable the
                         * appropriate Vertex Attributes for that batch
                         * and set default values for vertex attribs that
                         * the batch doesn't use, then draw all primitives
                         * within it.*/
                    if (bSelectedPass)
                    {
                        foreach (var primitive in _renderList[curNode.DataIndex])
                        {
                            GL.DrawArrays(PrimitiveType.LineStrip, primitive.VertexStart, primitive.VertexCount);
                        }
                    }
                    else
                    {
                        SetVertexAttribArraysForBatch(true, curNode.DataIndex);
                        foreach (var primitive in _renderList[curNode.DataIndex])
                        {
                            GL.DrawArrays(primitive.DrawType, primitive.VertexStart, primitive.VertexCount);
                        }
                        SetVertexAttribArraysForBatch(false, curNode.DataIndex);
                    }

                    break;

                case J3DFormat.HierarchyDataTypes.Joint:
                    var joint = _file.Joints.GetJoint(curNode.DataIndex);
                    Vector3 jointRot = joint.GetRotation().ToDegrees();
                    Vector3 translation = joint.GetTranslation();

                    if (ParentArchive != null)
                    {
                        if (FileName == "model.bdl")
                        {
                            Room room = ParentArchive as Room;
                            if (room != null)
                            {
                                translation += new Vector3(room.Translation.X, 0, room.Translation.Y);
                                jointRot.Y += room.Rotation.ToDegrees();
                            }
                        }
                    }

                    Matrix4 tranMatrix = Matrix4.CreateTranslation(translation);
                    Matrix4 rotMatrix = Matrix4.CreateRotationX(jointRot.X) * Matrix4.CreateRotationY(jointRot.Y) *
                                        Matrix4.CreateRotationZ(jointRot.Z);
                    Matrix4 scaleMatrix = Matrix4.CreateScale(joint.GetScale());

                    Matrix4 modelMatrix = tranMatrix * rotMatrix * scaleMatrix;

                    renderer.SetModelMatrix(modelMatrix);
                    break;
            }

            foreach (SceneGraph subNode in curNode.Children)
            {
                DrawModelRecursive(subNode, renderer, bSelectedPass);
            }
        }

        private void SetVertexAttribArraysForBatch(bool bEnabled, ushort batchIndex)
        {
            List<J3DFormat.BatchAttribute> attributes = _file.Shapes.GetAttributesForBatch(batchIndex);

            foreach (var attribute in attributes)
            {
                if (bEnabled)
                {
                    switch (attribute.AttribType)
                    {
                        case J3DFormat.ArrayTypes.Position: GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position); break;
                        case J3DFormat.ArrayTypes.Color0: GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Color); break;
                        case J3DFormat.ArrayTypes.Tex0: GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.TexCoord); break;
                    }
                }
                else
                {
                    switch (attribute.AttribType)
                    {
                        case J3DFormat.ArrayTypes.Position: GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position); break;
                        case J3DFormat.ArrayTypes.Color0: GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Color); break;
                        case J3DFormat.ArrayTypes.Tex0: GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.TexCoord); break;
                    }
                }
            }
        }

        private SceneGraph BuildSceneGraphFromInfo(J3DFormat.InfoChunk info)
        {
            if (info == null)
                return null;

            SceneGraph root = new SceneGraph();
            var hierarchyData = info.GetHierarchyData();

            BuildNodeRecursive(ref root, hierarchyData, 0);

            return root;
        }

        private int BuildNodeRecursive(ref SceneGraph parent, List<J3DFormat.HierarchyData> nodeList, int listIndex)
        {
            for (int i = listIndex; i < nodeList.Count; ++i)
            {
                J3DFormat.HierarchyData node = nodeList[i];
                SceneGraph newNode;

                switch (node.Type)
                {
                    //If it's a new node, push down in the stack one.
                    case J3DFormat.HierarchyDataTypes.NewNode:
                        newNode = new SceneGraph(node.Type, node.Index);
                        i += BuildNodeRecursive(ref newNode, nodeList, i + 1);
                        parent.Children.Add(newNode);
                        break;

                    //If it's the end node, we need to go up.
                    case J3DFormat.HierarchyDataTypes.EndNode:
                        return i - listIndex + 1;

                    //If it's a material, joint, or shape, just produce them.
                    case J3DFormat.HierarchyDataTypes.Material:
                    case J3DFormat.HierarchyDataTypes.Joint:
                    case J3DFormat.HierarchyDataTypes.Batch:
                    case J3DFormat.HierarchyDataTypes.Finish:
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

        private List<J3DRenderer.VertexFormatLayout> BuildVertexArraysFromFile()
        {
            List<J3DRenderer.VertexFormatLayout> finalData = new List<J3DRenderer.VertexFormatLayout>();


            //Now, let's try to get our data.
            for (uint i = 0; i < _file.Shapes.GetBatchCount(); i++)
            {
                J3DFormat.Batch batch = _file.Shapes.GetBatch(i);

                //Console.WriteLine("[{0}] Unk0: {5}, Attb: {6} Mtx Type: {1} #Packets {2}[{3}] Matrix Index: {4}", i, batch.MatrixType, batch.PacketCount, batch.PacketIndex, batch.FirstMatrixIndex, batch.Unknown0, batch.AttribOffset);


                uint attributeCount = 0;
                for (uint attribIndex = 0; attribIndex < 13; attribIndex++)
                {
                    J3DFormat.BatchAttribute attrib = _file.Shapes.GetAttribute(attribIndex, batch.AttribOffset);
                    if (attrib.AttribType == J3DFormat.ArrayTypes.NullAttr)
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
                    J3DFormat.BatchPacketLocation packetLoc = _file.Shapes.GetBatchPacketLocation(batch.PacketIndex + p);

                    uint numPrimitiveBytesRead = packetLoc.Offset;

                    while (numPrimitiveBytesRead < packetLoc.Offset + packetLoc.PacketSize)
                    {
                        //The data is going to be stored as:
                        //[Primitive][Primitive.VertexCount * (AttributeType.ElementCount * sizeof(AttributeType.DataType))]

                        J3DFormat.BatchPrimitive primitive = _file.Shapes.GetPrimitive(numPrimitiveBytesRead);
                        numPrimitiveBytesRead += J3DFormat.BatchPrimitive.Size;

                        //Game pads the chunks out with zeros, so this is signal for early break.
                        if (primitive.Type == 0)
                        {
                            break;
                        }

                        var primList = new PrimitiveList();
                        primList.VertexCount = primitive.VertexCount;
                        primList.VertexStart = finalData.Count;
                        primList.DrawType = primitive.Type == J3DFormat.PrimitiveTypes.TriangleStrip ? PrimitiveType.TriangleStrip : PrimitiveType.TriangleFan;

                        _renderList[(int)i].Add(primList);


                        //Todo: that's pretty shitty too.
                        for (int vert = 0; vert < primitive.VertexCount; vert++)
                        {
                            J3DRenderer.VertexFormatLayout newVertex = new J3DRenderer.VertexFormatLayout();
                            for (uint vertIndex = 0; vertIndex < attributeCount; vertIndex++)
                            {
                                var batchAttrib = _file.Shapes.GetAttribute(vertIndex, batch.AttribOffset);
                                ushort curIndex = _file.Shapes.GetPrimitiveIndex(numPrimitiveBytesRead, batchAttrib);

                                switch (batchAttrib.AttribType)
                                {
                                    case J3DFormat.ArrayTypes.Position:
                                        newVertex.Position = _file.Vertexes.GetPosition(curIndex);
                                        break;
                                    case J3DFormat.ArrayTypes.Normal:
                                        newVertex.Color = new Vector4(_file.Vertexes.GetNormal(curIndex, 14), 1); //temp
                                        break;
                                    case J3DFormat.ArrayTypes.Color0:
                                        newVertex.Color = _file.Vertexes.GetColor0(curIndex);
                                        break;
                                    case J3DFormat.ArrayTypes.Tex0:
                                        newVertex.TexCoord = _file.Vertexes.GetTex0(curIndex, 8);
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

        private int GetGLTexIdFromCache(int j3dTextureId)
        {
            if (_textureCache.ContainsKey(j3dTextureId))
                return _textureCache[j3dTextureId];

            //Console.WriteLine("Generating GL texture for id: " + j3dTextureId);

            //Look up the material first.
            //_file.Materials.GetMaterialRemapTable((ushort)j3dTextureId);
            J3DFormat.MaterialInitData matData = _file.Materials.GetMaterialInitData(_file.Materials.GetMaterialRemapTable((ushort)j3dTextureId));

            //If the texture cache doesn't contain the ID, we're going to load it here.

            ushort textureIndex = matData.GetTextureIndex(0);
            if (textureIndex == 0xFF || textureIndex == 0xFFFF)
            {
                _textureCache[j3dTextureId] = 0;
                return 0;
            }
            BinaryTextureImage image = _file.Textures.GetTexture(_file.Materials.GetMaterialIndex(textureIndex));
            //image.WriteImageToFile("image_" + matChunk.GetMaterialIndex(matData.GetTextureIndex(0)) + image.Format + ".png");

            byte[] imageData = image.GetData();
            ushort imageWidth = image.Width;
            ushort imageHeight = image.Height;

            //Generate a black and white textureboard if the texture format is not supported.
            if (imageData.Length == 0)
            {
                imageData = new byte[]
                {
                    0, 0, 0, 255, 255, 255, 255, 255,
                    255, 255, 255, 255, 0, 0, 0, 255
                };
                imageWidth = 2;
                imageHeight = 2;
            }


            int glTextureId;
            GL.GenTextures(1, out glTextureId);
            GL.BindTexture(TextureTarget.Texture2D, glTextureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, imageWidth, imageHeight, 0, PixelFormat.Bgra, PixelType.UnsignedInt8888Reversed, imageData);


            _textureCache[j3dTextureId] = glTextureId;
            return glTextureId;
        }

        private class SceneGraph
        {
            public List<SceneGraph> Children;
            public J3DFormat.HierarchyDataTypes NodeType { get; private set; }
            public ushort DataIndex { get; private set; }

            public SceneGraph(J3DFormat.HierarchyDataTypes type, ushort index)
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

        private uint GetAttribElementSize(J3DFormat.DataTypes dataType)
        {
            switch (dataType)
            {
                case J3DFormat.DataTypes.Unsigned8:
                case J3DFormat.DataTypes.Signed8:
                    return 1;
                case J3DFormat.DataTypes.Unsigned16:
                case J3DFormat.DataTypes.Signed16:
                    return 2;
                case J3DFormat.DataTypes.Float32:
                case J3DFormat.DataTypes.Rgba8:
                    return 4;
            }

            Console.WriteLine("Unknown attrib datatype {0}, guessing at 2!", dataType);
            return 2;
        }


    }
}