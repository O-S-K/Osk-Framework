using UnityEngine;

namespace OSK
{
    public class UIBaseItem : MonoBehaviour
    {
        protected BaseReferenceHolder Holder;
           public int Index;
       
           public GameObject GameObject => this.Holder.gameObject;
       
           public void Bind(Transform item) => this.Holder = item.GetComponent<BaseReferenceHolder>();
       
           public void Bind(Component item) => this.Holder = item.GetComponent<BaseReferenceHolder>();
       
           public void Bind(GameObject item) => this.Holder = item.GetComponent<BaseReferenceHolder>();
       
           public T GetRef<T>(string name, bool isTry = false) where T : Object
           {
             return this.Holder.GetRef<T>(name, isTry);
           }
    }
}
