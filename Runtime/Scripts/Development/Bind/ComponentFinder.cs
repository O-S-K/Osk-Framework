using System.Reflection;
using UnityEngine;

public static class ComponentFinder
{
    
    public static void AutoAssignComponents(MonoBehaviour script)
    {
        var fields = script.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            if (field.GetValue(script) == null)
            {
                if (field.GetCustomAttribute<FindAttribute>() is FindAttribute findAttribute)
                {
                    AssignComponent(script, field, findAttribute.name, findAttribute.componentType);
                }
                else if (field.GetCustomAttribute<FindChildAttribute>() is FindChildAttribute findChildAttribute)
                {
                    AssignComponent(script, field, findChildAttribute.name, findChildAttribute.componentType, true);
                }
                else if (field.GetCustomAttribute<FindParentAttribute>() is FindParentAttribute findParentAttribute)
                {
                    AssignComponent(script, field, findParentAttribute.name, findParentAttribute.componentType, false, true);
                }
            }
        }
    }

    private static void AssignComponent(MonoBehaviour script, FieldInfo field, string name, ComponentType type, bool isChild = false, bool isParent = false)
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
            var component = targetObject.GetComponent(GetUnityTypeFromComponentType(type));
            if (component != null)
            {
                field.SetValue(script, component);
                Debug.Log($"Assigned {type} to field {field.Name} in {script.name}.");
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
            if (parent.name == name) return parent.gameObject;
            parent = parent.parent;
        }
        return null;
    }

    private static System.Type GetUnityTypeFromComponentType(ComponentType componentType)
    {
        return componentType switch
        {
            ComponentType.Button => typeof(UnityEngine.UI.Button),
            ComponentType.Text => typeof(UnityEngine.UI.Text),
            ComponentType.TextTMPro => typeof(TMPro.TextMeshProUGUI),
            ComponentType.Image => typeof(UnityEngine.UI.Image),
            ComponentType.Slider => typeof(UnityEngine.UI.Slider),
            ComponentType.InputField => typeof(UnityEngine.UI.InputField),
            ComponentType.Script => typeof(MonoBehaviour),
            ComponentType.Rigidbody => typeof(Rigidbody),
            ComponentType.Collider => typeof(Collider),
            ComponentType.GameObject => typeof(GameObject),
            _ => null,
        };
    }
}
