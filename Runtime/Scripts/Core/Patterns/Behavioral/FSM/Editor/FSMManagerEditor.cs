using UnityEditor;
using UnityEngine;
using OSK;
using System.Linq;

[CustomEditor(typeof(FSMManager))]
public class FSMManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FSMManager fsmManager = (FSMManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("State Machines", EditorStyles.boldLabel);

        foreach (var group in fsmManager.k_GroupStateMachines.ToList())
        {
            DrawGroup(fsmManager, group.Key, group.Value);
        }

        EditorGUILayout.Space();
    }

    private void DrawGroup(FSMManager fsmManager, string groupName, StateMachine stateMachine)
    {
        EditorGUILayout.LabelField($"Group: {groupName}", EditorStyles.boldLabel);
        if (GUILayout.Button($"Delete Group '{groupName}'"))
        {
            fsmManager.Remove(groupName);
        }

        EditorGUILayout.LabelField("States:");
        var currentState = stateMachine.GetCurrentState();
        foreach (var state in stateMachine.GetStates().ToList())
        {
            EditorGUILayout.BeginHorizontal();

            // Tên của State
            GUIStyle stateStyle = new GUIStyle(GUI.skin.label)
            {
                padding = new RectOffset(10, 0, 0, 0),
                fontStyle = FontStyle.Normal,
                normal = { textColor = state == currentState ? Color.green : Color.white }
            };
            EditorGUILayout.LabelField($"- {state.GetType().Name}", stateStyle);

            // Nút chuyển trạng thái
            EditorGUI.BeginDisabledGroup(state == currentState);
            if (GUILayout.Button("Switch", GUILayout.Width(60)))
            {
                stateMachine.Init(state);
            }
            EditorGUI.EndDisabledGroup();
            
            // Nút xóa State
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                stateMachine.Remove(state);
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();
    }
}