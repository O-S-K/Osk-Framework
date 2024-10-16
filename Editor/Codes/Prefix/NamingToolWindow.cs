using UnityEditor;
using UnityEngine;

public class NamingToolWindow : EditorWindow
{
    private NamingRules namingRules;
    private GameObject selectedObject;

    [MenuItem("OSK-Framework/Tools/Naming Tool")]
    public static void ShowWindow()
    {
        GetWindow<NamingToolWindow>("Naming Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Naming Tool", EditorStyles.boldLabel);
        namingRules = (NamingRules)EditorGUILayout.ObjectField("Naming Rules", namingRules, typeof(NamingRules), false);

        if (GUILayout.Button("Apply Naming"))
        {
            ApplyNamingRules();
        }
    }

    private void ApplyNamingRules()
    {
        if (namingRules == null)
        {
            Debug.LogError("NamingRules not assigned!");
            return;
        }

        var selectedObjects = Selection.gameObjects;
        foreach (var obj in selectedObjects)
        {
            foreach (var rule in namingRules.rules)
            {
                // Get the type from the string
                System.Type componentType = System.Type.GetType(rule.componentType.selectedComponent);
                if (componentType != null)
                {
                    var component = obj.GetComponent(componentType);
                    if (component != null)
                    {
                        // Generate new name based on the rule
                        string newName = rule.prefix.Replace("#NAME#", obj.name);
                        if (obj.name != newName)
                        {
                            obj.name = newName;
                        }
                    }
                }
            }
        }
    }
}