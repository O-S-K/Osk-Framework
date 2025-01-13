using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public static class TransformExtension
    {
        public static void SetParent(this Transform transform, Transform parent)
        {
            if(transform.parent != parent)
                transform.SetParent(parent);
        } 
        public static void SetPosRect(this Transform rectTransform, Vector3 pos)
        {
            var r = rectTransform as RectTransform;
            if (r != null) r.anchoredPosition = pos;
        }
        
        public static RectTransform GetRectTransform(this Transform transform)
        {
            return transform as RectTransform;
        }

        public static void SetTopSibling(this Transform transform)
        {
            transform.SetSiblingIndex(transform.parent.childCount - 1);
        }

        public static void SetSiblingIndex(this Transform transform, int index)
        {
            transform.SetSiblingIndex(index);
        }

        public static void SetBottomSibling(this Transform transform)
        {
            transform.SetSiblingIndex(0);
        }

        // example : transform.position =  transform.position.With(y: 5);
        public static Vector3 With(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector3.x, y ?? vector3.y, z ?? vector3.z);
        }

        public static Vector3 WithX(this Vector3 vector3, float x)
        {
            return new Vector3(x, vector3.y, vector3.z);
        }

        public static Vector3 WithY(this Vector3 vector3, float y)
        {
            return new Vector3(vector3.x, y, vector3.z);
        }

        public static Vector3 WithZ(this Vector3 vector3, float z)
        {
            return new Vector3(vector3.x, vector3.y, z);
        }

        // example : transform.position =  transform.position.Add(x: 1, y: 5);
        public static Vector3 Add(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector3.x + (x ?? 0), vector3.y + (y ?? 0), vector3.z + (z ?? 0));
        }
        
        public static void DestroyAllChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
        
        public static void DestroyAllChildren(this GameObject gameObject)
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        
        public static void DeActiveAllGameObject(this GameObject[] gameObjects)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }
        }
    }
}