using System.IO;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OSK
{
    public class JsonSystem : IFile
    {
        // Todo: Call For Mobile Devices
        // onApplicationPause() => SaveJson

        public void Save<T>(string fileName, T data, bool ableEncrypt = false)
        {
            var filePath = IOUtility.FilePath(fileName + ".json");
            try
            {
                string saveJson = JsonUtility.ToJson(data);

                // Format numeric values to specified decimal places
                saveJson = FormatJsonDecimals(saveJson, 4);

                if (ableEncrypt)
                {
                    var saveBytes = Encoding.UTF8.GetBytes(saveJson);
                    File.WriteAllBytes(filePath, Obfuscator.Encrypt(saveBytes, Main.Configs.init.encryptKey));
                }
                else
                {
                    File.WriteAllText(filePath, saveJson);
                }
                RefreshEditor();

                OSK.Logg.Log($"[Save File Success]: {fileName + ".json"} \n {filePath}", Color.green);
            }
            catch (System.Exception ex)
            {
                OSK.Logg.LogError($"[Save File Exception]: {fileName + ".json"}  {ex.Message}");
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

        public T Load<T>(string fileName, bool ableEncrypt = false)
        {
            var filePath = IOUtility.FilePath(fileName + ".json");
            if (!File.Exists(filePath))
            {
                OSK.Logg.LogError($"[Load File Error]: {fileName + ".json"} NOT found");
                return default;
            } 
            try
            {
                string loadJson;
                if (ableEncrypt)
                {
                    var loadBytes = Obfuscator.Decrypt(File.ReadAllBytes(filePath), Main.Configs.init.encryptKey);
                    loadJson = Encoding.UTF8.GetString(loadBytes);
                }
                else
                {
                    loadJson = File.ReadAllText(filePath);
                }

                if (!IsValidJson(loadJson))
                {
                    OSK.Logg.LogError($"[Load File Error]: {fileName + ".json"} contains invalid JSON");
                    return default;
                }

                // Deserialize JSON to object
                T data = JsonUtility.FromJson<T>(loadJson);
                OSK.Logg.Log($"[Load File Success]: {fileName + ".json"} \n {filePath}", Color.green);
                return data;
            }
            catch (System.Exception ex)
            {
                OSK.Logg.LogError($"[Load File Exception]: {fileName + ".json"}  {ex.Message}");
                return default;
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

        public void Delete(string fileName)
        {
            IOUtility.DeleteFile(fileName + ".json");
            Logg.Log($"[Delete File Success]: {fileName + ".json"}");
        }
        
        public T Query<T>(string fileName, bool condition)
        {
            if (condition)
            {
                return Load<T>(fileName);
            }
            return default;
        }

        private void RefreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}