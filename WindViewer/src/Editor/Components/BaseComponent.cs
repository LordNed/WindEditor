namespace WindViewer.Editor
{
    public class BaseComponent
    {
        public EditorObject editorObject { get; set; }

        public Transform transform {
            get { return editorObject.transform; }
        }

        /*public BaseComponent()
        {
            EditorCore.Instance.RegisterComponent(this);
        }*/

        public virtual void Update() { }
    }
}