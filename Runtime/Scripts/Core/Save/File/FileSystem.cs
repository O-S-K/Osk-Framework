using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;

namespace OSK
{
    public class FileSystem : IFile
    {
        public void Save<T>(string fileName, T data, bool isEncrypt = false)
        {
            try
            {
                var path = IOUtility.GetPath(fileName + ".dat");
                using (FileStream fileStream = File.Open(path, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    string json = JsonConvert.SerializeObject(data, Formatting.None);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                    // Ghi độ dài của dữ liệu trước (để kiểm tra khi đọc)
                    writer.Write(jsonBytes.Length);
                    writer.Write(jsonBytes);
                }

                if (isEncrypt)
                {
                    byte[] encryptedData = FileSecurity.Encrypt(File.ReadAllBytes(path), Main.Configs.init.encryptKey);
                    File.WriteAllBytes(path, encryptedData);
                }

                RefreshEditor();
                OSK.Logg.Log($"[Save File Success]: {fileName + ".dat"} {DateTime.Now}\n{path}", Color.green);
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError($"[Save File Exception]: {fileName + ".dat"} {ex.Message}");
            }
        }

        public T Load<T>(string fileName, bool isDecrypt = false)
        {
            try
            {
                var path = IOUtility.GetPath(fileName + ".dat");
                if (!File.Exists(path))
                {
                    OSK.Logg.LogError($"[Load File Error]: {fileName + ".dat"} NOT found");
                    return default;
                }

                byte[] fileBytes = File.ReadAllBytes(path);
                if (isDecrypt)
                {
                    fileBytes = FileSecurity.Decrypt(fileBytes, Main.Configs.init.encryptKey);
                }

                using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                using (BinaryReader reader = new BinaryReader(memoryStream))
                {
                    int dataLength = reader.ReadInt32();
                    byte[] jsonBytes = reader.ReadBytes(dataLength);
                    string json = Encoding.UTF8.GetString(jsonBytes);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError($"[Load File Exception]: {fileName + ".dat"} {ex.Message}");
                return default;
            }
        }

        public void Delete(string fileName)
        {
            IOUtility.DeleteFile(fileName + ".dat");
            OSK.Logg.Log($"[Delete File Success]: {fileName}.dat");
        }
        
        public T Query<T>(string fileName, bool condition)
        {
            if (condition)
            {
                return Load<T>(fileName);
            }
            return default;
        }
        
              
        public void WriteAllLines(string fileName, string[] json)
        {
            var path = IOUtility.GetPath(fileName + ".txt");
            OSK.Logg.Log("Path Save: " + path);
            File.WriteAllLines(path, json);
        }

        private void RefreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
