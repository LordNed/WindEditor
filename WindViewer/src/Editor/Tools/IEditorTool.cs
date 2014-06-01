namespace WindViewer.Editor.Tools
{
    public abstract class IEditorTool
    {
        public virtual void PreUpdate() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }

        public virtual void Render() { }
    }
}