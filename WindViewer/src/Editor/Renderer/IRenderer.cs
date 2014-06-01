using System.Collections.Generic;
using WindViewer.Editor.Tools;

namespace WindViewer.Editor.Renderer
{
    public abstract class IRenderer : IEditorTool
    {
        //Shader Identifier
        protected int _programId;

        //OpenTK::Shader Attributes
        protected int _attributeVpos;
        protected int _uniformMVP;

        public virtual void AddRenderable(IRenderable renderable)
        {
            
        }

        public virtual void RemoveRenderable(IRenderable renderable)
        {

        }

        public abstract void Render(Camera camera, float aspectRatio);
    }
}
