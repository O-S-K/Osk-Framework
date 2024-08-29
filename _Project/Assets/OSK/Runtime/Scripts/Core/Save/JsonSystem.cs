using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OSK
{
    public class JsonSystem
    {
        // Todo: Call For Mobile Devices
        // onApplicationPause() => SaveJson

        private const string AES_KEY = "0123456789ABCDEF";

        public void Save<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            Debug.Log("path -> " + World.Data.Json.GetPath(name));

            object jsonData = obj is Object ? obj : new global::Json.JsonMapper<T>(obj);
            var saveJson = JsonUtility.ToJson(jsonData);
            File.WriteAllText(filePath, saveJson);
            
            Debug.Log("SaveJson -> " + saveJson);
        }

        public void Load<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new global::Json.JsonMapper<T>(obj);
            if (!File.Exists(filePath))
            {
                var saveJson = JsonUtility.ToJson(jsonData);
                File.WriteAllText(filePath, saveJson);
            }

            JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), jsonData);
        }
        
        public string GetPath(string name)
        {
            return FilePath(name);
        }
        
        public void Delete(string name)
        {
            var filePath = FilePath(name);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void Encrypt<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new global::Json.JsonMapper<T>(obj);
            var saveBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(jsonData));
            File.WriteAllBytes(filePath, Obfuscator.Encrypt(saveBytes, AES_KEY));
        }

        public void Decrypt<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new global::Json.JsonMapper<T>(obj);
            if (!File.Exists(filePath))
            {
                var saveBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(jsonData));
                File.WriteAllBytes(filePath, Obfuscator.Encrypt(saveBytes, AES_KEY));
            }

            var loadBytes = Obfuscator.Decrypt(File.ReadAllBytes(filePath), AES_KEY);
            JsonUtility.FromJsonOverwrite(Encoding.UTF8.GetString(loadBytes), jsonData);
        }

        private string FilePath(string name)
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, $"{name}.json");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, $"{name}.json");
            }

            return filePath;
        }


        private void RefreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}