namespace WindViewer.Editor.Tools
{
    interface IEditorTool
    {
        void PreUpdate();
        void Update();
        void LateUpdate();

        void Render();
    }
}