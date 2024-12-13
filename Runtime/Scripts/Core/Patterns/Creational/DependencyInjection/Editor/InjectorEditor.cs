#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace OSK
{
    [CustomEditor(typeof(DIContextInject), true)]
    public class InjectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Add some space for clarity
            GUILayout.Space(30);

            // Displaying registered dependencies
            GUI.color = Color.yellow;
            GUILayout.Label("Registered Dependencies", EditorStyles.boldLabel);
            GUILayout.Label("-------------------------------------------------------------------------------------");
            GUILayout.Space(10);

            var dependencies = DIContainer.GetRegisteredDependencies();

            if (dependencies.Count == 0)
            {
                GUILayout.Label("No registered dependencies found.", EditorStyles.boldLabel);
            }
            else
            {
                if (Application.isPlaying)
                {
                    // Display each dependency with its key and interface
                    foreach (var dependency in dependencies)
                    {
                        GUILayout.BeginHorizontal();
                        GUI.color = Color.yellow;
                        GUILayout.Label($"Provide: {dependency.Key.Name}", GUILayout.Width(150));
                        string instances = string.Join(", ", dependency.Value.Select(obj => obj?.GetType().Name ?? "null"));
                        GUI.color = Color.green;
                        GUILayout.Label($"In: {instances}", GUILayout.Width(500));
                        GUI.color = Color.white;
                    
                        GUILayout.EndHorizontal();
                    }
                }
            }

            // Validate and Clear buttons section
            GUILayout.Space(20);
            GUI.color = Color.cyan;
            GUILayout.Label("DIContainer Actions", EditorStyles.boldLabel);
            GUILayout.Label("-------------------------------------------------------------------------------------");
            GUILayout.Space(10);

            // Validate Dependencies Button
            if (GUILayout.Button("Validate Dependencies In Hierarchy"))
            {
                DIContainer.ValidateDependencies();
            }

            // Clear All Injectable Fields Button
            if (GUILayout.Button("Clear All Injectable Fields"))
            {
                DIContainer.ClearDependencies();
            }

            // Reset GUI color to default
            GUI.color = Color.white;
        }
    }
}
#endif