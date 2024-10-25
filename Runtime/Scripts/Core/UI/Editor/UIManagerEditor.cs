#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace OSK
{
    [CustomEditor(typeof(HUD))]
    public class UIManagerEditor : Editor
    {
        private HUD uiManager;

        private void OnEnable()
        {
            uiManager = (HUD)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            // spam all the screens and popups
            if (GUILayout.Button("Show All Views"))
            {
                SpawnAllViews();
            }
            EditorGUILayout.Space();
            if(GUILayout.Button("Clear All Views"))
            {
                CleanAllViews();
            }
            EditorGUILayout.Space();


            // Draw background for the screens section
            DrawBackground(Color.red);
            EditorGUILayout.LabelField("--- List Screens ---", EditorStyles.boldLabel);
            DisplayScreens();
            EditorGUILayout.Space();

            // Draw background for the popups section
            DrawBackground(Color.green);
            EditorGUILayout.LabelField("--- List Popups  ---", EditorStyles.boldLabel);
            DisplayPopups();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void CleanAllViews()
        {
            
        }

        private void SpawnAllViews()
        {
            List<UIScreen> screens = uiManager.GetScreenManager.ListUIPopupSo.UIScreens;
            foreach (var screen in screens)
            {
                GameObject s = PrefabUtility.InstantiatePrefab(screen.gameObject) as GameObject;
                s.transform.SetParent( uiManager.GetScreenManager.transform);
                s.transform.localPosition = Vector3.zero;
            }
                
            List<Popup> popups = uiManager.GetPopupManager.ListUIPopupSo.Popups;
            foreach (var popup in popups)
            {
                GameObject p = PrefabUtility.InstantiatePrefab(popup.gameObject) as GameObject;
                p.transform.SetParent(uiManager.GetPopupManager.transform);
                p.transform.localPosition = Vector3.zero;
            }
        }

        private void DrawBackground(Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, color);
        }

        private void DisplayScreens()
        {
            if (!Application.isPlaying)
                return;

            List<UIScreen> screens = uiManager.GetScreenManager.GetAllScreens();
            foreach (var screen in screens)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(screen.name, GUILayout.Width(400));

                bool isVisible = uiManager.GetScreenManager.GetScreen(screen).IsShowing;
                GUI.enabled = !isVisible;
                if (GUILayout.Button("Show"))
                {
                    uiManager.GetScreenManager.Show(screen);
                }

                GUI.enabled = isVisible;
                if (GUILayout.Button("Hide"))
                {
                    uiManager.GetScreenManager.Hide(screen);
                }

                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DisplayPopups()
        {
            if (!Application.isPlaying)
                return;

            List<Popup> popups = uiManager.GetPopupManager.GetAllPopups();
            foreach (var popup in popups)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(popup.name, GUILayout.Width(400));

                bool isVisible = uiManager.GetPopupManager.Get(popup).IsShowing;

                GUI.enabled = !isVisible;
                if (GUILayout.Button("Show"))
                {
                    uiManager.GetPopupManager.Show(popup, true);
                }

                GUI.enabled = isVisible;
                if (GUILayout.Button("Hide"))
                {
                    uiManager.GetPopupManager.Hide(popup);
                }

                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif