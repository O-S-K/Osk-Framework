using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 

[CustomPropertyDrawer(typeof(ComponentTypeSelector))]
public class ComponentTypeSelectorDrawer : PropertyDrawer
{
    private string[] componentTypes;

    public ComponentTypeSelectorDrawer()
    {  
        // Get all component types dynamically
        var types = typeof(Component).Assembly.GetTypes();
        var filteredTypes = new List<string>();

        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(Component)) && !type.IsAbstract)
            {
                filteredTypes.Add(type.Name);
            }
        }

        componentTypes = filteredTypes.ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var selectedComponentProperty = property.FindPropertyRelative("selectedComponent");

        // Find current selected index
        int selectedIndex = Array.IndexOf(componentTypes, selectedComponentProperty.stringValue);
        if (selectedIndex == -1) selectedIndex = 0;

        // Show dropdown
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, componentTypes);

        // Update the selected value
        selectedComponentProperty.stringValue = componentTypes[selectedIndex];
    }
}