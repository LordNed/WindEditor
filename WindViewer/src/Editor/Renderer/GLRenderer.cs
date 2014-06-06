using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor.Renderer
{
    public sealed class GLRenderer : IRenderer
    {
        private readonly List<IRenderable> _renderableObjects = new List<IRenderable>();

        public GLRenderer()
        {
            InitializeShader("shaders/vs.glsl", "shaders/fs.glsl");

            //We kind of need these.
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.Front);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
        }

        public override void AddRenderable(IRenderable renderable)
        {
            base.AddRenderable(renderable);

            _renderableObjects.Add(renderable);
            renderable.UpdateBuffers();
        }

        public override void RemoveRenderable(IRenderable renderable)
        {
            base.RemoveRenderable(renderable);

            _renderableObjects.Remove(renderable);
        }

        public override void ClearRenderableList()
        {
            _renderableObjects.Clear();
        }

        override public void Render(Camera camera, float aspectRatio)
        {
            GL.UseProgram(_programId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //Clear any previously bound buffer

            //Enable Attributes for this shader
            GL.EnableVertexAttribArray((int)ShaderAttributeIds.Position);

            foreach (IRenderable o in _renderableObjects)
            {
                Matrix4 projMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 100f, 8000f);

                //Create Model matrix based on Objects Translation/Rotation
                o.CalculateModelMatrix();
                o.ViewProjectionMatrix = camera.GetViewMatrix()*projMatrix;
                o.ModelViewProjectionMatrix = o.ModelMatrix*o.ViewProjectionMatrix;

                //Upload the Model Matrix to the GPU
                GL.UniformMatrix4(_uniformMVP, false, ref o.ModelViewProjectionMatrix);

                //Bind the buffers for the model we want to draw.
                o.BindBuffers();

                //Now update the vertex attribute to point to the newly bound buffer.
                GL.VertexAttribPointer((int)ShaderAttributeIds.Position, 3, VertexAttribPointerType.Float, false, 0, 0);

                //Render the object.
                o.Render();
            }

            GL.DisableVertexAttribArray((int)ShaderAttributeIds.Position);
            GL.Flush();
        }

        
    }
}