#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(DIContextInject),true)]
    public class InjectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.color = Color.yellow;
            GUILayout.Space(30); 
            GUILayout.Label("Registered Dependencies", EditorStyles.boldLabel);
            GUILayout.Label("-------------------------------------------------------------------------------------");
            GUILayout.Space(10);

            var dependencies = DIContainer.GetRegisteredDependencies();
            foreach (var dependency in dependencies)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Type: {dependency.Key.Name}", GUILayout.Width(150));
                GUILayout.Label($"Instance: {dependency.Value?.ToString() ?? "null"}", GUILayout.Width(500));
                GUILayout.EndHorizontal();
            }

            GUI.color = Color.cyan;
            GUILayout.Space(20);
            GUILayout.Label("DIContainer", EditorStyles.boldLabel);
            GUILayout.Label("-------------------------------------------------------------------------------------");
            GUILayout.Space(10);

            if (GUILayout.Button("Validate Dependencies In Hierarchy"))
            {
                DIContainer.ValidateDependencies();
            }
            if (GUILayout.Button("Clear All Injectable Fields"))
            {
                DIContainer.ClearDependencies();
            }
            GUI.color = Color.white;
        }
    }
}
#endif