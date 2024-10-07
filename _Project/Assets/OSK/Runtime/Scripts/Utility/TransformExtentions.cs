using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public static class TransformExtentions
    {
        public static void SetPosRect(this Transform rectTransform, Vector3 pos)
        {
            var r = rectTransform as RectTransform;
            if (r != null) r.anchoredPosition = pos;
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
    }
}