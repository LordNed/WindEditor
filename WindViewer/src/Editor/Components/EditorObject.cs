using System;
using System.Collections.Generic;

namespace WindViewer.Editor
{
    public sealed class EditorObject
    {
        private readonly List<BaseComponent> _objectComponentList;
        
        public Transform transform
        {
            get { return _transformCache ?? (_transformCache = AddComponent<Transform>()); }
        }

        private Transform _transformCache;

        public EditorObject()
        {
            _objectComponentList = new List<BaseComponent>();
        }

         public T AddComponent<T>() where T : BaseComponent
         {
             var newComp = (T) Activator.CreateInstance(typeof (T));
             _objectComponentList.Add(newComp);
             newComp.editorObject = this;

             return newComp;
         }
    }
}