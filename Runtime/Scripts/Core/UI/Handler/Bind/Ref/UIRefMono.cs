using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    public class UIRefMono : MonoBehaviour
    {
        protected IReferenceHolder _referenceHolder;

        public IReferenceHolder ReferenceHolder
        {
            get
            {
                if (_referenceHolder == null)
                    _referenceHolder = GetComponent<IReferenceHolder>();
                return _referenceHolder;
            }
        }

        [Button]
        public void AddBindRef()
        {
            if (GetComponent<IReferenceHolder>() != null)
            {
                Debug.LogWarning("ReferenceHolder already exists.");
                return;
            }

            gameObject.AddComponent<MonoReferenceHolder>();
            Debug.Log("ReferenceHolder added.");
        }

        protected virtual void Awake()
        {
            _referenceHolder = GetComponent<IReferenceHolder>();
            if (_referenceHolder == null)
            {
                Debug.LogError("IReferenceHolder component is missing on " + gameObject.name);
            }
        }

        public void IterateRefs(System.Action<string, object> func) => _referenceHolder.Foreach(func);

        public T GetRef<T>(string name, bool isTry = false) where T : Object
        {
            return _referenceHolder.GetRef<T>(name, isTry);
        }
    }
}