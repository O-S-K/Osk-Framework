using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
            
            
            GUIStyle stateStyle1 = new GUIStyle(GUI.skin.label)
            {
                padding = new RectOffset(10, 0, 0, 0),
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.green }
            };
            EditorGUILayout.LabelField($"-> {group.CurrentState} ", stateStyle1);

            foreach (var state in group.States)
            {
                GUIStyle stateStyle = new GUIStyle(GUI.skin.label)
                {
                    padding = new RectOffset(10, 0, 0, 0),
                    fontStyle = FontStyle.Normal,
                    normal = { textColor = state == group.CurrentState ? Color.red : Color.black }
                };

                EditorGUILayout.LabelField($"- {state}", stateStyle);
            }

            EditorGUI.indentLevel--;
        }
    }
}