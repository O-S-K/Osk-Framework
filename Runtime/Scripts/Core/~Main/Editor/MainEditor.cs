using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(Main))]
    public class MainEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Static References", EditorStyles.boldLabel);

            DrawStaticFieldCheck("Observer", Main.Observer);
            DrawStaticFieldCheck("EventBus", Main.EventBus);
            DrawStaticFieldCheck("State", Main.State);
            DrawStaticFieldCheck("Pool", Main.Pool);
            DrawStaticFieldCheck("Command", Main.Command);
            DrawStaticFieldCheck("Scene", Main.Scene);
            DrawStaticFieldCheck("Res", Main.Res);
            DrawStaticFieldCheck("Save", Main.Save);
            DrawStaticFieldCheck("Data", Main.Data);
            DrawStaticFieldCheck("Network", Main.Network);
            DrawStaticFieldCheck("WebRequest", Main.WebRequest);
            DrawStaticFieldCheck("Configs", Main.Configs);
            DrawStaticFieldCheck("UI", Main.UI);
            DrawStaticFieldCheck("Sound", Main.Sound);
            DrawStaticFieldCheck("Localization", Main.Localization);
            DrawStaticFieldCheck("Entity", Main.Entity);
            DrawStaticFieldCheck("Time", Main.Time);
            DrawStaticFieldCheck("Ability", Main.Ability);
            DrawStaticFieldCheck("Native", Main.Native);
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
}