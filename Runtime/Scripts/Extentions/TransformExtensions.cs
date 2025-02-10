using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public static class TransformExtensions
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

        public static void SetX(this Transform transform, float x)
        {
            var position = transform.position;
            transform.position = new Vector3(x, position.y, position.z);
        }

        public static void SetY(this Transform transform, float y)
        {
            var position = transform.position;
            transform.position = new Vector3(position.x, y, position.z);
        }

        public static void SetZ(this Transform transform, float z)
        {
            var position = transform.position;
            transform.position = new Vector3(position.x, position.y, z);
        }

        public static void Reset(this Transform transform, Space space = Space.Self)
        {
            switch (space)
            {
                case Space.Self:
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                    break;

                case Space.World:
                    transform.position = Vector3.zero;
                    transform.rotation = Quaternion.identity;
                    break;
            }

            transform.localScale = Vector3.one;
        }

        public static Transform GetClosest(this Transform position, IEnumerable<Transform> otherPositions)
        {
            Transform closest = null;
            float closestDistance = Mathf.Infinity;

            foreach (var otherPosition in otherPositions)
            {
                float distance = Vector3.Distance(position.position, otherPosition.position);

                if (distance < closestDistance)
                {
                    closest = otherPosition;
                    closestDistance = distance;
                }
            }
            return closest;
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