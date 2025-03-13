using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OSK
{
    public static class ComponentExtensions
    {
        private static List<Component> componentCache = new List<Component>();

        
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
 
        public static bool HasComponentOrInterface<T>(this GameObject obj) where T : class
        {
            obj.GetComponents<Component>(componentCache);
            return componentCache.OfType<T>().Any();
        }

        public static T GetComponentOrInterface<T>(this GameObject obj) where T : class
        {
            obj.GetComponents<Component>(componentCache);
            return componentCache.OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetAllComponentsOrInterfaces<T>(this GameObject obj) where T : class
        {
            return obj.GetComponents<Component>().OfType<T>();
        }

        public static IEnumerable<T> GetAllComponentsOrInterfacesInChildren<T>(this GameObject obj) where T : class
        {
            return obj.GetComponentsInChildren<Component>().OfType<T>();
        }

        public static bool HasComponentOrInterface<T>(this Component component) where T : class
        {
            return HasComponentOrInterface<T>(component.gameObject);
        }

        public static T GetComponentOrInterface<T>(this Component component) where T : class
        {
            return GetComponentOrInterface<T>(component.gameObject);
        }

        public static IEnumerable<T> GetAllComponentsOrInterfaces<T>(this Component component) where T : class
        {
            return GetAllComponentsOrInterfaces<T>(component.gameObject);
        }

        public static IEnumerable<T> GetAllComponentsOrInterfacesInChildren<T>(this Component component) where T : class
        {
            return GetAllComponentsOrInterfacesInChildren<T>(component.gameObject);
        }

        public static T AddChild<T>(this GameObject parent) where T : Component
        {
            return AddChild<T>(parent, typeof(T).Name);
        }

        public static GameObject AddChild(this GameObject parent, GameObject child, bool worldPositionStays = false)
        {
            child.transform.SetParent(parent.transform, worldPositionStays);
            return parent;
        }

        public static T AddChild<T>(this GameObject parent, string name) where T : Component
        {
            var obj = AddChild(parent, name, typeof(T));
            return obj.GetComponent<T>();
        }

        public static GameObject AddChild(this GameObject parent, params Type[] components)
        {
            return AddChild(parent, "Game Object", components);
        }

        public static GameObject AddChild(this GameObject parent, string name, params Type[] components)
        {
            var obj = new GameObject(name, components);
            if (parent != null)
            {
                if (obj.transform is RectTransform) obj.transform.SetParent(parent.transform, true);
                else obj.transform.parent = parent.transform;
            }

            return obj;
        }

        public static GameObject LoadChild(this GameObject parent, string resourcePath)
        {
            var obj = (GameObject)Object.Instantiate(Resources.Load(resourcePath));
            if (obj != null && parent != null)
            {
                if (obj.transform is RectTransform) obj.transform.SetParent(parent.transform, true);
                else obj.transform.parent = parent.transform;
            }

            return obj;
        }

        public static GameObject LoadChild(this Transform parent, string resourcePath)
        {
            var obj = (GameObject)Object.Instantiate(Resources.Load(resourcePath));
            if (obj != null && parent != null)
            {
                if (obj.transform is RectTransform) obj.transform.SetParent(parent, true);
                else obj.transform.parent = parent;
            }
            return obj;
        }

        public static void DestroyAllChildrenImmediately(this GameObject obj)
        {
            DestroyAllChildrenImmediately(obj.transform);
        }

        public static void DestroyAllChildrenImmediately(this Transform trans)
        {
            while (trans.childCount != 0)
                Object.DestroyImmediate(trans.GetChild(0).gameObject);
        }

        public static void Deactivate(this Component component)
        {
            component.gameObject.SetActive(false);
        }

        public static void Activate(this Component component)
        {
            component.gameObject.SetActive(true);
        }

        public static bool IsParentedBy(this GameObject obj, GameObject parent)
        {
            if (obj.transform.parent == null)
                return false;
            return obj.transform.parent.gameObject == parent || obj.transform.parent.gameObject.IsParentedBy(parent);
        }
    }
}