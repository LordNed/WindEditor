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
        public Type EditorType;
        public EntEditorType(Type type)
        {
            EditorType = type;
        }
    }
}