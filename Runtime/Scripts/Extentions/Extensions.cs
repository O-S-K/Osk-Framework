using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public static class Extensions
    { 
        public static GameObject CreateGO(this GameObject gameObject, Transform parent)
        {
            if(parent == null)
            {
                Debug.LogError("Parent is null");
                return null;
            }
            
            if(parent.Find(gameObject.name) != null)
            {
                Debug.LogError("GameObject with name " + gameObject.name + " already exists in parent " + parent.name);
                return parent.Find(gameObject.name).gameObject;
            }
            
            GameObject go = new GameObject(gameObject.name);
            go.transform.SetParent(parent);
            return go;
        }
 
    }
}