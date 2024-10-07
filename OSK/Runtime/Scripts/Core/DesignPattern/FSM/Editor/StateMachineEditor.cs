using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace OSK
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : Editor
    {
        private StateMachine stateMachine;

        private void OnEnable()
        {
            stateMachine = (StateMachine)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("State Machine Groups", EditorStyles.boldLabel);
            DisplayStateGroups();
        }

        private void DisplayStateGroups()
        {
            if (stateMachine == null) return;

            foreach (var group in stateMachine.GetGroups())
            {
                EditorGUILayout.LabelField(group.Name, EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                foreach (var state in group.States)
                {
                    GUIStyle stateStyle = new GUIStyle(GUI.skin.label)
                    {
                        padding = new RectOffset(10, 0, 0, 0),
                        fontStyle = FontStyle.Normal,
                        normal =
                        {
                            textColor = state.StateName == group.CurrentState.StateName ? Color.green : Color.white
                        }
                    };

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"- {state.StateName}", stateStyle);

                    // Add button to switch state
                    if (GUILayout.Button("Switch", GUILayout.Width(60)))
                    {
                        group.SetCurrentState(state);
                        // Optionally, mark the object as dirty if needed
                        EditorUtility.SetDirty(stateMachine);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}