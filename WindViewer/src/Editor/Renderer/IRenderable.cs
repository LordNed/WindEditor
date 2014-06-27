namespace WindViewer.Editor.Renderer
{
    public interface IRenderable
    {
        void Bind();
        void Draw(BaseRenderer renderer);
    }
}