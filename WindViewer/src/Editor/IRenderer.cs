using System;
using System.Collections;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindViewer.Editor
{
    class IRenderer
    {
        // TODO(mtwilliams): Instrusive linked-list.
        protected List<IRenderable> _renderable = new List<IRenderable>();

        public void AddRenderable(IRenderable renderable)
        {
            _renderable.Add(renderable);
        }

        public void RemoveRenderable(IRenderable renderable)
        {
            _renderable.Remove(renderable);
        }

        public void Render(Camera camera, float aspectRatio)
        {
            Matrix4 projMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 0.01f, 1000f);
            Matrix4 viewProjMatrix = camera.GetViewMatrix() * projMatrix;

            foreach (IRenderable renderable in _renderable)
            {
                renderable.Render(viewProjMatrix);
            }
        }
    }
}
