using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Text.RegularExpressions;

namespace OSK
{
    public class JsonSystem
    {
        // Todo: Call For Mobile Devices
        // onApplicationPause() => SaveJson

        public void Save<T>(string fileName, T obj, bool ableEncrypt = false, int decimalPlaces = 4)
        {
            var filePath = IOUtility.FilePath(fileName);
            try
            {
                // Serialize object to JSON
                string saveJson = JsonUtility.ToJson(obj);

                // Format numeric values to specified decimal places
                saveJson = FormatJsonDecimals(saveJson, decimalPlaces);

                if (ableEncrypt)
                {
                    var saveBytes = Encoding.UTF8.GetBytes(saveJson);
                    File.WriteAllBytes(filePath, Obfuscator.Encrypt(saveBytes));
                }
                else
                {
                    File.WriteAllText(filePath, saveJson);
                }

                OSK.Logg.Log($"Successfully saved JSON to: {filePath}", Color.green);
                OSK.Logg.Log("SaveJson: " + saveJson, Color.green);
            }
            catch (System.Exception ex)
            {
                OSK.Logg.LogError($"Error saving JSON to {filePath}: {ex.Message}");
            }
        }

        // Helper method to format JSON numbers to specified decimal places
        private string FormatJsonDecimals(string json, int decimalPlaces)
        {
            return Regex.Replace(json, @"\d+\.\d+", match =>
            {
                if (double.TryParse(match.Value, out double number))
                {
                    return System.Math.Round(number, decimalPlaces).ToString("F" + decimalPlaces);
                }
                return match.Value;
            });
        }


        public void Load<T>(string fileName, T obj, bool ableEncrypt = false)
        {
            var filePath = IOUtility.FilePath(fileName);

            if (!File.Exists(filePath))
            {
                OSK.Logg.LogError($"File not found at path: {filePath}");
                return;
            }
            try
            {
                string loadJson;
                if (ableEncrypt)
                {
                    var loadBytes = Obfuscator.Decrypt(File.ReadAllBytes(filePath));
                    loadJson = Encoding.UTF8.GetString(loadBytes);
                }
                else
                {
                    loadJson = File.ReadAllText(filePath);
                }

                // Validate JSON format
                if (string.IsNullOrEmpty(loadJson) || !IsValidJson(loadJson))
                {
                    OSK.Logg.LogError("Invalid JSON format in file: " + filePath);
                    return;
                }

                JsonUtility.FromJsonOverwrite(loadJson, obj);
                OSK.Logg.Log("LoadJson: " + loadJson, Color.green);
            }
            catch (System.Exception ex)
            {
                OSK.Logg.LogError($"Error loading file: {ex.Message}");
            }
        }

        // Helper method to validate JSON format
        private bool IsValidJson(string json)
        {
            try
            {
                var temp = JsonUtility.FromJson<object>(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<string> Get(string name)
        {
            List<string> savedFiles = new List<string>();
            var filePath = IOUtility.FilePath(name);

            if (Directory.Exists(filePath))
            {
                var files = Directory.GetFiles(filePath, "*.json");
                foreach (var file in files)
                {
                    savedFiles.Add(Path.GetFileName(file));
                }
            }

            return savedFiles;
        }

        private void RefreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}