using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(FSMManager))]
    public class FSMEditor : Editor
    {
        private FSMManager _fsmManager;

        private void OnEnable()
        {
            _fsmManager = (FSMManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Final State Machine Groups", EditorStyles.boldLabel);
            DisplayStateGroups();
        }

        private void DisplayStateGroups()
        {
            if (_fsmManager == null) return;
            if (_fsmManager.CurrentGroup != null)
            {
                GUIStyle CurrentGroup = new GUIStyle(GUI.skin.label)
                {
                    padding = new RectOffset(10, 0, 0, 0),
                    fontStyle = FontStyle.Normal,
                    normal =
                    {
                        textColor = Color.green
                    }
                };

                GUILayout.Space(10);
                EditorGUILayout.LabelField("Current FSM: " + _fsmManager.CurrentGroup.Name, CurrentGroup);
                GUILayout.Space(10);
            }
            
            var groups = new List<GroupState>(_fsmManager.GetGroups());
            foreach (var group in groups)
            {
                EditorGUILayout.LabelField(group.Name, EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                
                var keys =  new List<IStateMachine>(group.States);
                foreach (var state in keys)
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

                    if (GUILayout.Button("Switch", GUILayout.Width(60)))
                    {
                        group.Switch(state, new[] { true });
                        EditorUtility.SetDirty(_fsmManager);
                    }
                    // remove 
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        group.RemoveState(state);
                        EditorUtility.SetDirty(_fsmManager);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}