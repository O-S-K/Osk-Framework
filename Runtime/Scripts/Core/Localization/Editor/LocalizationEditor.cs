using System.IO;
using System.Text;
using CustomInspector;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationEditor : Editor
    {
        private LocalizationManager localization;

        private void OnEnable()
        {
            localization = (LocalizationManager)target;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(30);
            EditorGUILayout.LabelField("Localization Editor", EditorStyles.boldLabel);

            // if (GUILayout.Button("Convert Excel to CSV"))
            // {
            //     ConvertExcelToCsv();
            // }
            if (GUILayout.Button("Open Excel File"))
            {
                OpenExcelFile();
            }
            if (GUILayout.Button("Open CSV File"))
            {
                OpenCsvFile();
            }
            
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Set Random Language"))
            {
                SetDefaultLanguage();
            }
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        
        private void SetDefaultLanguage()
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
            {
                localization.SwitchLanguage(SystemLanguage.Vietnamese);
            }
            else
            {
                localization.SwitchLanguage(SystemLanguage.English);
            }
        }

        private void OpenExcelFile()
        {
            UnityEditor.EditorUtility.RevealInFinder(localization.excelFilePath);
        }

        private void OpenCsvFile()
        {
            UnityEditor.EditorUtility.RevealInFinder(localization.outputCsvPath + "Localization.csv");
        }
    }
}