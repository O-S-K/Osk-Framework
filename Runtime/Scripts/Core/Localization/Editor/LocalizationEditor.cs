using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CustomInspector;
using ExcelDataReader;
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
        
        void ConvertExcelToCsv()
        {
            StringBuilder sb = new StringBuilder();

            // Check folder path
            if (!Directory.Exists(localization.outputCsvPath))
            {
                Directory.CreateDirectory(localization.outputCsvPath);
            }
            
            // Check if file exists
            var file = localization.outputCsvPath + "Localization.csv";
            if (!File.Exists(file))
            {
                Logg.Log("File not found, creating new file at: " + file);
                File.WriteAllText(file, string.Empty); // Tạo file rỗng
            } 
         

            // Load file Excel
            using (var stream = File.Open(localization.excelFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var sheet = result.Tables[0];   // Lấy bảng tính đầu tiên

                    // copy data from Excel to CSV
                    for (int i = 0; i < sheet.Rows.Count; i++)
                    {
                        for (int j = 0; j < sheet.Columns.Count; j++)
                        {
                            sb.Append(sheet.Rows[i][j].ToString());

                            // Add comma if this is not the last column
                            if (j < sheet.Columns.Count - 1)
                            {
                                sb.Append(",");
                            }
                        }
                        sb.AppendLine(); 
                    }
                }
            }

            // Write to file
            File.WriteAllText(file, sb.ToString());
            Logg.Log("File CSV created at: " + file);
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