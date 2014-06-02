using System;

namespace WindViewer.Editor
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayName : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EntEditorType : Attribute
    {
        private Type _editorType;

        public EntEditorType(Type type)
        {
            _editorType = type;
        }

        public int MinEditorWidth { get; set; }

        public Type EditorType()
        {
            return _editorType;
        }
    }
}