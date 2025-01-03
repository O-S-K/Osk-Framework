#if UNITY_EDITOR


using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    public static class ComponentFinder
    {
        public static void AutoAssignComponents(Component script)
        {
            var fields = script.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<FindAttribute>() is FindAttribute findAttribute)
                {
                    AssignComponent(script, field, findAttribute);
                }
            }
        }

        private static void AssignComponent(Component script, FieldInfo field, FindAttribute findAttribute)
        {
            GameObject targetObject = null;

            switch (findAttribute.findType)
            {
                case EFindType.Self:
                    targetObject = script.gameObject;
                    break;

                case EFindType.Child:
                    targetObject = string.IsNullOrEmpty(findAttribute.name)
                        ? FindInChildren(script.transform, findAttribute.type)
                        : script.transform.Find(findAttribute.name)?.gameObject;
                    break;

                case EFindType.Parent:
                    targetObject = string.IsNullOrEmpty(findAttribute.name)
                        ? FindInParents(script.transform, findAttribute.type)
                        : script.transform.parent?.Find(findAttribute.name)?.gameObject;
                    break;

                case EFindType.Scene:
                    targetObject = FindInScene(findAttribute.name, findAttribute.type);
                    break;

                case EFindType.Slibling:
                    targetObject = string.IsNullOrEmpty(findAttribute.name)
                        ? FindInSiblings(script.transform, findAttribute.type)
                        : script.transform.parent?.Find(findAttribute.name)?.gameObject;
                    break;
            }

            if (targetObject != null)
            {
                AssignField(script, field, targetObject);
            }
            else
            {
                Logg.LogError(
                    $"Could not find target for field {field.Name} in {script.name}." +
                    $" FindType: {findAttribute.findType}, Name: {findAttribute.name}, Type: {findAttribute.type?.Name}");
            }
        }

        private static GameObject FindInChildren(Transform parent, Type type)
        {
            foreach (Transform child in parent)
            {
                if (type == null || child.GetComponent(type) != null)
                {
                    return child.gameObject;
                }

                var result = FindInChildren(child, type);
                if (result != null)
                    return result;
            }

            return null;
        }

        private static GameObject FindInParents(Transform child, Type type)
        {
            var parent = child.parent;
            while (parent != null)
            {
                if (type == null || parent.GetComponent(type) != null)
                {
                    return parent.gameObject;
                }

                parent = parent.parent;
            }

            return null;
        }

        private static GameObject FindInScene(string name, Type type)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var obj = GameObject.Find(name);
                if (obj != null)
                    return obj;
            }

            if (type != null)
            {
                var obj = GameObject.FindObjectsOfType(type).FirstOrDefault();
                if (obj != null)
                    return (obj as Component)?.gameObject;
            }

            return null;
        }

        private static GameObject FindInSiblings(Transform child, Type type)
        {
            var parent = child.parent;
            if (parent == null)
                return null;

            foreach (Transform sibling in parent)
            {
                if (sibling == child)
                    continue;

                if (type == null || sibling.GetComponent(type) != null)
                {
                    return sibling.gameObject;
                }
            }

            return null;
        }

        private static void AssignField(Component script, FieldInfo field, GameObject targetObject)
        {
            var fieldType = field.FieldType;

            if (fieldType == typeof(GameObject))
            {
                field.SetValue(script, targetObject);
            }
            else if (typeof(Component).IsAssignableFrom(fieldType))
            {
                var component = targetObject.GetComponent(fieldType);
                if (component != null)
                {
                    field.SetValue(script, component);
                }
                else
                {
                    Debug.LogWarning(
                        $"Component of type {fieldType.Name} not found in GameObject {targetObject.name} for field {field.Name} in {script.name}.");
                }
            }
            else
            {
                Debug.LogWarning(
                    $"Field {field.Name} in {script.name} is of unsupported type {fieldType.Name}. Skipping assignment.");
            }

            EditorUtility.SetDirty(script.gameObject);
        }
    }
}
#endif