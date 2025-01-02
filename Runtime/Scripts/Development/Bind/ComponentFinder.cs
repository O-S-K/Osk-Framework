using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class ComponentFinder
{

    public static void AutoAssignComponents(Component script)
    {
        var fields = script.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<FindAttribute>() is FindAttribute findAttribute)
            {
                if (field.GetValue(script) == null || findAttribute.overrideExisting)
                {
                    AssignComponent(script, field, findAttribute.name, findAttribute.componentType);
                }
            }
            else if (field.GetCustomAttribute<FindChildAttribute>() is FindChildAttribute findChildAttribute)
            {
                if (field.GetValue(script) == null || findChildAttribute.overrideExisting)
                {
                    AssignComponent(script, field, findChildAttribute.name, findChildAttribute.componentType, true);
                }
            }
            else if (field.GetCustomAttribute<FindParentAttribute>() is FindParentAttribute findParentAttribute)
            {
                if (field.GetValue(script) == null || findParentAttribute.overrideExisting)
                {
                    AssignComponent(script, field, findParentAttribute.name, findParentAttribute.componentType, false, true);
                }
            }
        }
    }


    private static void AssignComponent(Component script, FieldInfo field, string name, ComponentType type, bool isChild = false, bool isParent = false)
    {
        GameObject targetObject = null;

        if (isChild)
        {
            targetObject = FindInChildren(script.gameObject, name);
        }
        else if (isParent)
        {
            targetObject = FindInParents(script.gameObject, name);
        }
        else
        {
            targetObject = GameObject.Find(name);
        }

        if (targetObject != null)
        {
            Debug.Log($"Found target object: {targetObject.name} for field {field.Name}");
            CheckComponetType(script, field, type, targetObject);
        }
        else
        {
            Debug.LogWarning($"GameObject {name} not found for field {field.Name} in {script.name}.");
        }
    }

    private static void CheckComponetType(Component script, FieldInfo field, ComponentType type, GameObject targetObject)
    {
        Debug.Log($"Processing field: {field.Name}, FieldType: {field.FieldType.Name}, TargetObject: {targetObject.name}, Type: {type}");

        if (type == ComponentType.GameObject)
        {
            Debug.Log($"Assigned GameObject {targetObject.name} to field {field.Name} in {script.name}.");
            field.SetValue(script, targetObject);
        }
        else if (type == ComponentType.Component)
        {
            var component = targetObject.GetComponent(field.FieldType);
            if (component != null)
            {
                Debug.Log($"Assigned {field.FieldType.Name} to field {field.Name} in {script.name}.");
                field.SetValue(script, component);
            }
            else
            {
                Debug.LogWarning($"Field {field.Name} of type {field.FieldType.Name} could not be assigned in {script.name}. Target GameObject: {targetObject.name}");
            }
        }
        else
        {
            var component = targetObject.GetComponent(GetUnityTypeFromComponentType(type));
            if (component != null)
            {
                Debug.Log($"Assigned {type} to field {field.Name} in {script.name}.");
                field.SetValue(script, component);
                EditorUtility.SetDirty(script);
            }
        }
    }

    private static GameObject FindInChildren(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == name) return child.gameObject;
            var found = FindInChildren(child.gameObject, name);
            if (found != null) return found;
        }
        return null;
    }



    private static GameObject FindInParents(GameObject child, string name)
    {
        var parent = child.transform.parent;
        while (parent != null)
        {
            if (parent.name == name)
                return parent.gameObject;
            parent = parent.parent;
        }
        return null;
    }

    private static System.Type GetUnityTypeFromComponentType(ComponentType componentType)
    {
        Debug.Log("Component Type: " + componentType.ToString());
        return componentType switch
        {
            ComponentType.Graphic => typeof(Graphic),
            ComponentType.Selectable => typeof(Selectable),
            ComponentType.Component => typeof(Component),
            ComponentType.GameObject => typeof(GameObject),
            _ => null,
        };
    }
}
