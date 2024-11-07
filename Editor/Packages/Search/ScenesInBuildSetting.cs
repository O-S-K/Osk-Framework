#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;

namespace OSK
{
    public class ScenesInBuildSetting : EditorWindow
    {
         private string searchString = "";
        private List<string> filteredScenes;
        private Vector2 scrollPosition;

        private GUIContent SceneLoadedButtonAddContent = new GUIContent("+", "Load scene additively");
        private GUIContent SceneLoadedButtonActiveContent = new GUIContent("*", "Active scene (cannot unload)");
        private GUIContent SceneLoadedButtonRemoveContent = new GUIContent("-", "Unload scene");

        private static Texture2D toolbarIcon;

        [MenuItem("Window/Scenes In Build Setting")]
        public static void ShowWindow()
        {
            GetWindow<ScenesInBuildSetting>("Scenes In Build Setting");
        }

        [InitializeOnLoadMethod]
        private static void AddToolbarIcon()
        {
            toolbarIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Icons/ScenesInBuildSettingIcon.png");

            if (toolbarIcon != null)
            {
                EditorApplication.update += OnUpdate;
            }
            else
            {
                Debug.LogWarning("Scenes In Build Setting icon not found at 'Assets/Editor/Icons/ScenesInBuildSettingIcon.png'.");
            }
        }

        private static void OnUpdate()
        {
            if (toolbarIcon != null)
            {
                EditorToolbarUtility.AddToolbarIcon(toolbarIcon, "Scenes In Build Setting", ShowWindow);
                EditorApplication.update -= OnUpdate; // Remove to ensure it runs only once
            }
        }

        private void OnEnable()
        {
            RefreshSceneList();
        }

        private void OnGUI()
        {
            DrawSearchBar();
            DrawSceneList();
        }

        private void DrawSearchBar()
        {
            GUILayout.Label("Search:", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            searchString = EditorGUILayout.TextField(searchString, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Refesh", GUILayout.Width(50)))
            {
                RefreshSceneList();
            }
            
            if (GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                searchString = "";
                RefreshSceneList();
            }

            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                FilterScenes();
            }
        }

        private void DrawSceneList()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var scenePath in filteredScenes)
            {
                EditorGUILayout.BeginHorizontal();

                // Button to open the scene
                if (GUILayout.Button(System.IO.Path.GetFileNameWithoutExtension(scenePath), GUILayout.ExpandWidth(true)))
                {
                    OpenScene(scenePath);
                }

                // Check if the scene is loaded
                var scene = EditorSceneManager.GetSceneByPath(scenePath);
                bool isLoaded = scene.isLoaded;
                bool isActiveScene = scene == EditorSceneManager.GetActiveScene();

                // Button to load scene additively
                if (!isLoaded && GUILayout.Button(SceneLoadedButtonAddContent, GUILayout.Width(30)))
                {
                    LoadSceneAdditively(scenePath);
                }

                // Button to mark scene as active
                if (isLoaded && !isActiveScene && GUILayout.Button(SceneLoadedButtonActiveContent, GUILayout.Width(30)))
                {
                    SetActiveScene(scene);
                }

                // Button to unload scene
                if (isLoaded && !isActiveScene && GUILayout.Button(SceneLoadedButtonRemoveContent, GUILayout.Width(30)))
                {
                    UnloadScene(scene);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void RefreshSceneList()
        {
            filteredScenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    filteredScenes.Add(scene.path);
                }
            }

            FilterScenes();
        }

        private void FilterScenes()
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return;
            }

            filteredScenes = filteredScenes.FindAll(scenePath =>
                scenePath.ToLower().Contains(searchString.ToLower()));
        }

        private void OpenScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }
        }

        private void LoadSceneAdditively(string scenePath)
        {
            if (!EditorSceneManager.GetSceneByPath(scenePath).isLoaded)
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
        }

        private void SetActiveScene(Scene scene)
        {
            if (scene.isLoaded)
            {
                EditorSceneManager.SetActiveScene(scene);
            }
        }

        private void UnloadScene(Scene scene)
        {
            if (scene.isLoaded && scene != EditorSceneManager.GetActiveScene())
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }

    public static class EditorToolbarUtility
    {
        public static void AddToolbarIcon(Texture2D icon, string tooltip, Action onClick)
        {
            GUIStyle toolbarButtonStyle = new GUIStyle("AppCommand");
            toolbarButtonStyle.imagePosition = ImagePosition.ImageAbove;

            if (GUILayout.Button(new GUIContent(icon, tooltip), toolbarButtonStyle))
            {
                onClick.Invoke();
            }
        }
    }
}
#endif