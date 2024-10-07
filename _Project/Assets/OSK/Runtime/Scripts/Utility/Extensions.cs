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

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            float v = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            return v;
        }

       
        public static void DestroyAllChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(parent.GetChild(i).gameObject);
            }
        }

        public static void DeActiveAllGameObject(this GameObject[] gameObjects)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }
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