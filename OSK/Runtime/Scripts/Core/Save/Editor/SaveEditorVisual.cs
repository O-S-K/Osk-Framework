using UnityEditor;
using UnityEngine;
using System.IO;
using CustomPlayerPref.PlayerPrefsEditor;

namespace OSK
{
    [CustomEditor(typeof(SaveManager))]
    public class SaveEditorVisual : Editor
    {
        private SaveManager manager;

        private void OnEnable()
        {
            manager = (SaveManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("--- Save System ---", EditorStyles.boldLabel);
            DisplaySavedFiles();
        }

        private void DisplaySavedFiles()
        {
            EditorGUILayout.Space();
            DrawBackground(Color.white);
            DisplayFileSystemFiles();

            
            EditorGUILayout.Space();
            DrawBackground(Color.white);
            DisplayPlayerPrefsKeys();
        }

        private void DisplayFileSystemFiles()
        {
            EditorGUILayout.LabelField("FileSystem", EditorStyles.boldLabel, GUILayout.Width(400));

            var files = manager.File.GetAllFiles(Application.persistentDataPath);
            foreach (var file in files)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(file); // Display the filename

                if (GUILayout.Button("Delete"))
                {
                    DeleteFile(Path.Combine(Application.persistentDataPath, file));
                }

                if (GUILayout.Button("Open Path"))
                {
                    OpenFileLocation(Path.Combine(Application.persistentDataPath, file));
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DisplayPlayerPrefsKeys()
        {
            EditorGUILayout.LabelField("PlayerPrefs", GUILayout.Width(400));

            if (GUILayout.Button("Open"))
            {
                PlayerPrefsEditor.Init();
            }
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"Deleted file: {filePath}");
            }
            else
            {
                Debug.LogError($"File not found: {filePath}");
            }
        }

        private void OpenFileLocation(string filePath)
        {
            string fileDirectory = Path.GetDirectoryName(filePath);
            if (Directory.Exists(fileDirectory))
            {
                EditorUtility.RevealInFinder(fileDirectory); // Opens the directory containing the file
            }
            else
            {
                Debug.LogError($"Directory not found: {fileDirectory}");
            }
        }
        
        private void DrawBackground(Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1); 
            EditorGUI.DrawRect(rect, color);
        }
    }
}