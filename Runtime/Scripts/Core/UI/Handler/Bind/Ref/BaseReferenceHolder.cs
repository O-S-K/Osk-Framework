using UnityEngine;
using System.Collections.Generic;
using System;

namespace OSK
{
    public class BaseReferenceHolder : MonoBehaviour, IReferenceHolder
    {
        private Dictionary<string, UnityEngine.Object> objectRefs = new Dictionary<string, UnityEngine.Object>();
        private Dictionary<string, object> valueRefs = new Dictionary<string, object>();
        public List<RefData> DataRefs = new List<RefData>();
        protected bool hasInit;

        public void Foreach(Action<string, object> deal)
        {
            foreach (KeyValuePair<string, UnityEngine.Object> obj in objectRefs)
                deal(obj.Key, obj.Value);
        }

        public T GetRef<T>(string name, bool isLog = false) where T : UnityEngine.Object
        {
            CheckInit();
            UnityEngine.Object obj;
            if (objectRefs != null && objectRefs.TryGetValue(name, out obj))
                return obj as T;
            if (!isLog)
                Debug.Log(("Miss Ref " + name));
            return default;
        }

        public float GetValue(string name)
        {
            return valueRefs.TryGetValue(name, out var obj) ? (float)obj : 0.0f;
        }

        public Color GetColor(string name)
        {
            return valueRefs.TryGetValue(name, out var obj) ? (Color)obj : Color.white;
        }

        protected virtual void Awake() => this.CheckInit();

        protected void CheckInit()
        {
            if (hasInit)
                return;
            hasInit = true;
            foreach (var data in DataRefs)
            {
                if (data.bindObj != null)
                {
                    objectRefs ??= new Dictionary<string, UnityEngine.Object>();
                    objectRefs.Add(data.name, data.bindObj);
                }
                else
                {
                    valueRefs ??= new Dictionary<string, object>();
                    valueRefs.Add(data.name, data.bindVal);
                }
            }
        }

        public virtual void OnDestroy()
        {
            if (Application.isPlaying)
                DataRefs = null;
            objectRefs = null;
            valueRefs = null;
        }
    }
}