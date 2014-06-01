using OpenTK;

namespace WindViewer.Editor
{
    public abstract class IRenderable
    {
        // TODO(mtwilliams): some sort of render context should be exposed, with
        // things like all the matricies and inverse matricies, etc.
        public abstract void Render(Matrix4 viewProjMatrix);
    }
}
