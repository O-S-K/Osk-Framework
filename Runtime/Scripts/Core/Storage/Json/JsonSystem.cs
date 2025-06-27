using System.IO;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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
                string saveJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                saveJson = FormatJsonDecimals(saveJson, 4);

                if (ableEncrypt)
                {
                    var saveBytes = Encoding.UTF8.GetBytes(saveJson);
                    File.WriteAllBytes(filePath, Obfuscator.Encrypt(saveBytes, IOUtility.encryptKey));
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
            var path = IOUtility.FilePath(fileName + ".json");
            if (!File.Exists(path))
            {
                OSK.Logg.LogError($"[Load File Error]: {fileName + ".json"} NOT found at {path}");
                return default;
            }
            try
            {
                string loadJson;
                if (ableEncrypt)
                {
                    var loadBytes = Obfuscator.Decrypt(File.ReadAllBytes(path), IOUtility.encryptKey);
                    loadJson = Encoding.UTF8.GetString(loadBytes);
                }
                else
                {
                    loadJson = File.ReadAllText(path);
                }

                if (!IsValidJson(loadJson))
                {
                    OSK.Logg.LogError($"[Load File Error]: {fileName + ".json"} contains invalid JSON");
                    return default;
                }

                // Deserialize JSON to object
                T data = JsonConvert.DeserializeObject<T>(loadJson);
                OSK.Logg.Log($"[Load File Success]: {fileName + ".json"} \n {path}", Color.green);
                return data;
            }
            catch (System.Exception ex)
            {
                OSK.Logg.LogError($"[Load File Exception]: {fileName + ".json"}  {ex.Message}");
                return default;
            }
        }

        private bool IsValidJson(string json)
        {
            try
            {
                JsonConvert.DeserializeObject<object>(json);
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
