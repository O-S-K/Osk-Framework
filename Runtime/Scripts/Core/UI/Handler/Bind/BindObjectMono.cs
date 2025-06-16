using UnityEngine;

namespace OSK
{
    /// Inherit this script to get auto refs in MonoReferenceHolder
    public class BindObjectMono : MonoBehaviour
    {
        [SerializeReference]
        protected IReferenceHolder referenceHolder;
        public IReferenceHolder ReferenceHolder
        {
            get
            {
                if (referenceHolder == null)
                    referenceHolder = GetComponent<IReferenceHolder>();
                return referenceHolder;
            }
        }
        public void IterateRefs(System.Action<string, object> func) => ReferenceHolder.Foreach(func);
        public T GetRef<T>(string name, bool isTry = false) where T : Object
        {
            return ReferenceHolder.GetRef<T>(name, isTry);
        }
    }
}
