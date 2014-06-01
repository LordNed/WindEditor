using System;
using System.Collections;
using System.Collections.Generic;

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

        public void Render(Camera camera)
        {
            foreach (IRenderable R in _renderable)
            {
                R.Render();
            }
        }
    }
}
