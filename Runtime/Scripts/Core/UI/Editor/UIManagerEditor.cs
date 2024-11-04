#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace OSK
{
    [CustomEditor(typeof(RootUI))]
    public class UIManagerEditor : Editor
    {
        private RootUI uiManager;

        private void OnEnable()
        {
            uiManager = (RootUI)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Select Data SO"))
            {
                FindViewDataSOAssets();
            }

            // Draw background for the views section
            DrawBackground(Color.green);
            EditorGUILayout.LabelField("--- List Views  ---", EditorStyles.boldLabel);
            DisplayViews();
            
            GUILayout.Space(10);
            
            EditorGUILayout.LabelField("--- List Views History ---", EditorStyles.boldLabel);
            ShowListViewHistory();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        
        private void ShowListViewHistory()
        {
            List<View> views = uiManager.ListViews.ListViewHistory.ToList();
            foreach (var view in views)
            {
                EditorGUILayout.LabelField(view.name);
            }
        }

        private void FindViewDataSOAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:ListViewSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No ViewData found in the project.");
                return;
            }

            List<ListViewSO> viewDatas = new List<ListViewSO>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ListViewSO v = AssetDatabase.LoadAssetAtPath<ListViewSO>(path);
                viewDatas.Add(v);
            }

            if (viewDatas.Count == 0)
            {
                Debug.LogError("No ViewData found in the project.");
                return;
            }
            else
            {
                foreach (ListViewSO v in viewDatas)
                {
                    Debug.Log("ViewData found: " + v.name);
                    Selection.activeObject = v;
                    EditorGUIUtility.PingObject(v);
                }
            }
        }

        private void DrawBackground(Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, color);
        }

        private void DisplayViews()
        {
            if (!Application.isPlaying)
                return;

            List<View> views = uiManager.ListViews.GetAll(true);
            foreach (var _view in views)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(_view.name, GUILayout.Width(400));

                bool isVisible = uiManager.ListViews.Get(_view).IsShowing;

                GUI.enabled = !isVisible;
                if (GUILayout.Button("Open"))
                {
                    uiManager.ListViews.Open(_view, true);
                }

                GUI.enabled = isVisible;
                if (GUILayout.Button("Hide"))
                {
                    uiManager.ListViews.Hide(_view);
                }

                if (GUILayout.Button("Delete"))
                {
                    uiManager.ListViews.Delete(_view);
                }

                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif