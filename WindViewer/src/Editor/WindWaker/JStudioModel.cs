using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private List<RenderBatch> _renderList;
        private Dictionary<int, int> _textureCache;
        private List<SkeletonJoint> _skeleton; 
        public bool Selected;

        private List<J3DRenderer.VertexFormatLayout> _vertDataBind;

        public override void Load(byte[] data)
        {
            _file = new J3DFormat();
            _renderList = new List<RenderBatch>();
            _textureCache = new Dictionary<int, int>();
            _file.Load(data);
            _dataCache = data;

            Selected = false;


            /* Dump info for debugging */
            Console.WriteLine("Model: {0}, Vertex Count: {1} Packet Count: {2} Joint Count: {3}", FileName, _file.Info.GetVertexCount(), _file.Info.GetPacketCount(), _file.Joints.GetJointCount());
            Console.WriteLine("Envelope Count: {0} Draw Count: {1}", _file.Envelopes.GetEnvelopeCount(), _file.Draw.GetDrawCount());

            //Extract the data from the file format into something we can use.
            _vertDataBind = BuildVertexArraysFromFile();

            //Build our scene-graph so we can iterate through it.
            _root = BuildSceneGraphFromInfo();
            _skeleton = BuildSkeletonFromHierarchy();

            //Generate our VBO, and upload the data
            GL.GenBuffers(1, out _glVbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_vertDataBind.Count * 9 * 4), _vertDataBind.ToArray(),
                BufferUsageHint.StaticDraw);

            //Make sure we get drawn
            J3DRenderer.Instance.AddRenderable(this);
        }

        public override void Save(BinaryWriter stream)
        {
            stream.Write(_dataCache);
        }

        private List<SkeletonJoint> BuildSkeletonFromHierarchy()
        {
            List<SkeletonJoint> joints = new List<SkeletonJoint>();
            IterateHierarchyForSkeletonRecursive(_root, joints, -1);

            return joints;
        }


        private void IterateHierarchyForSkeletonRecursive(SceneGraph curNode, List<SkeletonJoint> jointList, int parentId) 
        {
            switch (curNode.NodeType)
            {
                case J3DFormat.HierarchyDataTypes.NewNode:
                    parentId = jointList.Count-1;
                    break;

                case J3DFormat.HierarchyDataTypes.Joint:
                    J3DFormat.Joint j3dJoint = _file.Joints.GetJoint(curNode.DataIndex);
                    SkeletonJoint joint = new SkeletonJoint();
                    joint.Name = _file.Joints.GetString(_file.Joints.GetStringTableEntry(_file.Joints.GetStringIndex(curNode.DataIndex))); //Todo: You have got to be kidding me.
                    joint.Rotation = new Quaternion(j3dJoint.GetRotation().ToDegrees(), 0f);
                    joint.Transform = j3dJoint.GetTranslation();
                    joint.ParentId = parentId;

                    jointList.Add(joint);
                    break;
            }

            foreach (SceneGraph child in curNode.Children)
            {
                IterateHierarchyForSkeletonRecursive(child, jointList, parentId);
            }
        }

        private class SkeletonJoint
        {
            public string Name;
            public Quaternion Rotation;
            public Vector3 Transform;

            public int ParentId;
        }

        private class RenderBatch
        {
            public List<RenderPacket> Packets;

            public RenderBatch()
            {
                Packets = new List<RenderPacket>();
            }
        }

        private class RenderPacket
        {
            public ushort[] DrawIndexes;
            public List<PrimitiveList> PrimList;

            public RenderPacket()
            {
                PrimList = new List<PrimitiveList>();
            }
        }

        private class PrimitiveList
        {
            public int VertexStart;
            public int VertexCount;
            public PrimitiveType DrawType;
            public List<ushort> PosMatrixIndex;

            public PrimitiveList()
            {
                PosMatrixIndex = new List<ushort>(); 
            }
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
        }

        public void Draw(BaseRenderer renderer)
        {
            /* Recursively iterate through the J3D scene graph to bind and draw all
             * of the batches within the J3D model. */
            Matrix4[] root = new Matrix4[_file.Joints.GetJointCount()];
            for (int i = 0; i < root.Length; i++)
                root[i] = Matrix4.Identity;

            Matrix4 ident = Matrix4.Identity;
            WalkModelJointRecursive(ref root, ref ident, _root, 0);
            //DrawModelRecursive(ref root, _root, renderer, false);

            if (Selected)
            {
                Matrix4 root2 = Matrix4.Identity;
                //DrawModelRecursive(ref root2, _root, renderer, true);
            }
        }

        private void WalkModelJointRecursive(ref Matrix4[] jointMatrix, ref Matrix4 jointParent, SceneGraph curNode, int sceneDepth)
        {
            switch (curNode.NodeType)
            {
                case J3DFormat.HierarchyDataTypes.Joint:
                    var joint = _file.Joints.GetJoint(curNode.DataIndex);
                    string jointName = _file.Joints.GetString(
                        _file.Joints.GetStringTableEntry(_file.Joints.GetStringIndex(curNode.DataIndex)));

                    Vector3 jointRot = joint.GetRotation().ToDegrees();
                    Vector3 translation = joint.GetTranslation();
                    Matrix4 tranMatrix = Matrix4.CreateTranslation(translation);
                    Matrix4 rotMatrix = Matrix4.CreateRotationX(jointRot.X) * Matrix4.CreateRotationY(jointRot.Y) * Matrix4.CreateRotationZ(jointRot.Z);
                    Matrix4 scaleMatrix = Matrix4.CreateScale(joint.GetScale());
                    Matrix4 modelMatrix = tranMatrix * rotMatrix * scaleMatrix;


                    jointMatrix[curNode.DataIndex] = modelMatrix*jointParent;
                    Vector3 finalJointTranslation = jointMatrix[curNode.DataIndex].ExtractTranslation();
                    Vector3 parentJointTrans = (Vector3.TransformPosition(Vector3.Zero, jointParent));
                    DebugRenderer.DrawLine(finalJointTranslation, parentJointTrans, Color.White);

                    jointParent = jointMatrix[curNode.DataIndex];
                    Console.WriteLine("Joint {0} at depth: {1}", jointName, sceneDepth);
                    break;

                case J3DFormat.HierarchyDataTypes.NewNode:
                    sceneDepth++;
                    break;
            }

            foreach (SceneGraph subNode in curNode.Children)
            {
                WalkModelJointRecursive(ref jointMatrix, ref jointParent, subNode, sceneDepth);
            }
        }

        private void DrawModelRecursive(ref Matrix4[] jointMatrix, SceneGraph curNode, BaseRenderer renderer, bool bSelectedPass)
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
                        #region Selected
                        float[] front_face_wireframe_color = { 1.0f, 1.0f, 1.0f, 1.0f };
                        float[] back_face_wireframe_color = { 0.7f, 0.7f, 0.7f, 0.7f };

                        GL.LineWidth(1);
                        GL.Enable(EnableCap.CullFace);
                        //GL.Disable(EnableCap.DepthTest);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        GL.EnableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position);

                        // 1. Draw the back-faces with a darker color:
                        GL.CullFace(CullFaceMode.Back);
                        GL.VertexAttrib4((int)BaseRenderer.ShaderAttributeIds.Color, back_face_wireframe_color);
                        foreach (var packet in _renderList[curNode.DataIndex].Packets)
                        {
                            int vertexIndex = 0;
                            foreach (var primitive in packet.PrimList)
                            {
                                //Uhh... 
                                ushort drawIndex = packet.DrawIndexes[primitive.PosMatrixIndex[vertexIndex]/3];
                                bool isWeighted = _file.Draw.IsWeighted(drawIndex);

                                if (isWeighted)
                                {
                                    
                                }
                                else
                                {
                                    var jnt = _file.Joints.GetJoint(curNode.DataIndex);
                                    Vector3 jntRot = jnt.GetRotation().ToDegrees();
                                    Vector3 trans = jnt.GetTranslation();
                                    Matrix4 trnMatrix = Matrix4.CreateTranslation(trans);
                                    Matrix4 rtMatrix = Matrix4.CreateRotationX(jntRot.X) * Matrix4.CreateRotationY(jntRot.Y) *
                                                        Matrix4.CreateRotationZ(jntRot.Z);
                                    Matrix4 sclMatrix = Matrix4.CreateScale(jnt.GetScale());

                                    Matrix4 final = trnMatrix * rtMatrix * sclMatrix;

                                    //renderer.SetModelMatrix(Matrix4.Identity);
                                    renderer.SetModelMatrix(final);
                                }

                                GL.DrawArrays(primitive.DrawType, primitive.VertexStart, primitive.VertexCount);

                                vertexIndex++;
                            }
                            
                        }
      

                        // 2. Draw the front-faces with a lighter color:
                        GL.CullFace(CullFaceMode.Front);
                        GL.VertexAttrib4((int)BaseRenderer.ShaderAttributeIds.Color, front_face_wireframe_color);
                        /*foreach (var primitive in _renderList[curNode.DataIndex])
                        {
                            GL.DrawArrays(primitive.DrawType, primitive.VertexStart, primitive.VertexCount);
                        }*/

                        GL.DisableVertexAttribArray((int)BaseRenderer.ShaderAttributeIds.Position);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                        //GL.Enable(EnableCap.DepthTest);
                        GL.LineWidth(1);
                        #endregion
                    }
                    else
                    {
                        SetVertexAttribArraysForBatch(true, curNode.DataIndex);
                        //GL.CullFace(CullFaceMode.Front);
                        for (int packetIndex = 0; packetIndex < _renderList[curNode.DataIndex].Packets.Count; packetIndex++)
                        {
                            RenderPacket packet = _renderList[curNode.DataIndex].Packets[packetIndex];
                            foreach (var primitive in packet.PrimList)
                            {
                                renderer.SetModelMatrix(Matrix4.Identity);
                                var transformedData =
                                    new J3DRenderer.VertexFormatLayout[primitive.VertexCount];
                                if (primitive.PosMatrixIndex.Count > 0)
                                {
                                    //For each vertex within this primitive, we're going to get its id.
                                    for (int vertexIndex = 0; vertexIndex < primitive.VertexCount; vertexIndex++)
                                    {
                                        ushort vertIndex = primitive.PosMatrixIndex[vertexIndex];
                                        ushort drawIndex = packet.DrawIndexes[vertIndex / 3];

                                        //ehh
                                        int seriously = 0;
                                        while (drawIndex == 0xFFFF)
                                        {
                                            RenderPacket prevPacket =
                                                _renderList[curNode.DataIndex].Packets[packetIndex - seriously];
                                            drawIndex = prevPacket.DrawIndexes[vertIndex/3];
                                            seriously++;
                                        }

                                        bool isWeighted = _file.Draw.IsWeighted(drawIndex);
                                        if (isWeighted)
                                        {
                                            
                                        }
                                        else
                                        {
                                            //If the vertex is not weighted, we're just going to use the position
                                            //from the bone matrix. Something like this.
                                            Vector3 vertPosition =
                                                _vertDataBind[primitive.VertexStart + vertexIndex].Position;

                                            transformedData[vertexIndex] =
                                                _vertDataBind[primitive.VertexStart + vertexIndex];
                                            transformedData[vertexIndex].Position = Vector3.TransformPosition(vertPosition, jointMatrix[_file.Draw.GetIndex(drawIndex)]);
                                            /*ushort jointIndex = ;

                                            var jnt = _file.Joints.GetJoint(jointIndex);
                                            Vector3 jntRot = jnt.GetRotation().ToDegrees();
                                            Vector3 trans = jnt.GetTranslation();
                                            Matrix4 trnMatrix = Matrix4.CreateTranslation(trans);
                                            Matrix4 rtMatrix = Matrix4.CreateRotationX(jntRot.X) * Matrix4.CreateRotationY(jntRot.Y) *
                                                                Matrix4.CreateRotationZ(jntRot.Z);
                                            Matrix4 sclMatrix = Matrix4.CreateScale(jnt.GetScale());

                                            Matrix4 final = trnMatrix * rtMatrix * sclMatrix;*/
                                            //renderer.SetModelMatrix(Matrix4.Identity);
                                        }
                                    }
                                }

                                //Re-upload the subsection to the buffer.
                                GL.BindBuffer(BufferTarget.ArrayBuffer, _glVbo);
                                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(primitive.VertexStart*(9*4)),
                                    (IntPtr)(primitive.VertexCount*(9*4)), transformedData);

                                float[] front_face_wireframe_color = { 1.0f, 1.0f, 1.0f, 1.0f };
                                GL.VertexAttrib4((int)BaseRenderer.ShaderAttributeIds.Color, front_face_wireframe_color);
                                GL.DrawArrays(primitive.DrawType, primitive.VertexStart, primitive.VertexCount);                                
                            }

                        }
 
                        SetVertexAttribArraysForBatch(false, curNode.DataIndex);
                    }

                    break;
            }

            foreach (SceneGraph subNode in curNode.Children)
            {
                DrawModelRecursive(ref jointMatrix, subNode, renderer, bSelectedPass);
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

        private SceneGraph BuildSceneGraphFromInfo()
        {
            SceneGraph root = new SceneGraph();
            var hierarchyData = _file.Info.GetHierarchyData();

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

                RenderBatch renderBatch = new RenderBatch();
                _renderList.Add(renderBatch);

                //Console.WriteLine("[{0}] Unk0: {5}, Attb: {6} Mtx Type: {1} #Packets {2}[{3}] Matrix Index: {4}", i, batch.MatrixType, batch.PacketCount, batch.PacketIndex, batch.FirstMatrixIndex, batch.Unknown0, batch.AttribOffset);

                uint attributeCount = 0;
                for (uint attribIndex = 0; attribIndex < 13; attribIndex++)
                {
                    J3DFormat.BatchAttribute attrib = _file.Shapes.GetAttribute(attribIndex, batch.AttribOffset);
                    if (attrib.AttribType == J3DFormat.ArrayTypes.NullAttr)
                        break;

                    attributeCount++;
                }

                for (ushort p = 0; p < batch.PacketCount; p++)
                {
                    RenderPacket renderPacket = new RenderPacket();
                    renderBatch.Packets.Add(renderPacket);

                    //Matrix Data
                    J3DFormat.PacketMatrixData pmd = _file.Shapes.GetPacketMatrixData((ushort) (batch.FirstMatrixIndex + p));
                    renderPacket.DrawIndexes = new ushort[pmd.Count];
                    for (ushort mtx = 0; mtx < pmd.Count; mtx++)
                    {
                        //Console.WriteLine("{4} {0} Packet: {1} Index: {2} PMD Unknown: {3}", i, p, _file.Shapes.GetMatrixTableIndex((ushort) (pmd.FirstIndex + mtx)), pmd.Unknown, mtx);
                        renderPacket.DrawIndexes[mtx] =_file.Shapes.GetMatrixTableIndex((ushort) (pmd.FirstIndex + mtx));
                    }

                    J3DFormat.BatchPacketLocation packetLoc = _file.Shapes.GetBatchPacketLocation((ushort) (batch.PacketIndex + p));
                    uint numPrimitiveBytesRead = packetLoc.Offset;
                    while (numPrimitiveBytesRead < packetLoc.Offset + packetLoc.PacketSize)
                    {
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
                        primList.DrawType = primitive.Type == J3DFormat.PrimitiveTypes.TriangleStrip ? PrimitiveType.TriangleStrip : PrimitiveType.TriangleFan; //Todo: More support
                        renderPacket.PrimList.Add(primList);

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

                                    case J3DFormat.ArrayTypes.PositionMatrixIndex:
                                        //Console.WriteLine("B: {0} P: {1} Prim: {2} Vert{3} Index: {4}", i, p, renderPacket.PrimList.Count, vert, curIndex);
                                        primList.PosMatrixIndex.Add(curIndex);
                                        break;
                                    default:
                                        Console.WriteLine("Unknown AttribType {0}, Index: {1}", batchAttrib.AttribType, curIndex);
                                        break;
                                }

                                numPrimitiveBytesRead += GetAttribElementSize(batchAttrib.DataType);
                            }

                            //Add our vertex to our list of Vertexes
                            finalData.Add(newVertex);
                        }

                        //Console.WriteLine("Batch {0} Prim {1} #Vertices with PosMtxIndex: {2}", i, p, primList.PosMatrixIndex.Count);
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