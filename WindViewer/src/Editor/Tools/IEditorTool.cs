namespace WindViewer.Editor
{
    public interface IEditorTool
    {
        void PreUpdate();
        void Update();
        void LateUpdate();

        void Render();
    }
}