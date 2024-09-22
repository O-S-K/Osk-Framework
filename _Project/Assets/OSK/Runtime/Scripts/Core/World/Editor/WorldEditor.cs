using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Static References", EditorStyles.boldLabel);

        DrawStaticFieldCheck("Observer", World.Observer);
        DrawStaticFieldCheck("EventBus", World.EventBus);
        DrawStaticFieldCheck("State", World.State);
        DrawStaticFieldCheck("Pool", World.Pool);
        DrawStaticFieldCheck("Command", World.Command);
        DrawStaticFieldCheck("Scene", World.Scene);
        DrawStaticFieldCheck("Res", World.Res);
        DrawStaticFieldCheck("Data", World.Data);
        DrawStaticFieldCheck("Log", World.Log);
        DrawStaticFieldCheck("Network", World.Network);
        DrawStaticFieldCheck("WebRequest", World.WebRequest);
        DrawStaticFieldCheck("Configs", World.Configs);
        DrawStaticFieldCheck("UI", World.UI);
        DrawStaticFieldCheck("Sound", World.Sound);
        DrawStaticFieldCheck("Localization", World.Localization);
        DrawStaticFieldCheck("Entity", World.Entity);
        DrawStaticFieldCheck("Time", World.Time);
        DrawStaticFieldCheck("Ability", World.Ability);
        DrawStaticFieldCheck("Performance", World.Performance);
    }

    private void DrawStaticFieldCheck(string label, object value)
    {
        bool isAssigned = value != null;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(150));
        EditorGUILayout.LabelField(isAssigned ? "Ok" : "Null",
            isAssigned ? EditorStyles.whiteLabel : EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
    }
}