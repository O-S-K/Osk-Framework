using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public static class Extensions
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }
        public static T GetOrAdd<T>(this Transform transform) where T : Component
        {
            T component = transform.GetComponent<T>();
            return component != null ? component : transform.gameObject.AddComponent<T>();
        }
        
        public static T GetOrAdd<T>(this MonoBehaviour mono) where T : Component
        {
            T component = mono.GetComponent<T>();
            return component != null ? component : mono.gameObject.AddComponent<T>();
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            float v = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            return v;
        }

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
 

        public static void RefreshList<T>(this List<T> list) where T : class
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                    list.RemoveAt(i);
            }
        }
    }
}