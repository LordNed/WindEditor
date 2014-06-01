using System;
using System.Collections.Generic;
using OpenTK;

namespace WindViewer.Editor.Renderer
{
    public abstract class IRenderer
    {
        //Shader Identifier
        protected int _programId;
        //protected int _vertexShaderId;
        //protected int _fragmentShaderId;

        //OpenTK::Shader Attributes
        protected int _attributeVpos;
        protected int _uniformMVP;

        // TODO(mtwilliams): Instrusive linked-list.
        protected List<IRenderable> _renderableObjects = new List<IRenderable>();

        public virtual void AddRenderable(IRenderable renderable)
        {
            _renderableObjects.Add(renderable);
        }

        public virtual void RemoveRenderable(IRenderable renderable)
        {
            _renderableObjects.Remove(renderable);
        }

        public abstract void Render(Camera camera, float aspectRatio);
    }
}
