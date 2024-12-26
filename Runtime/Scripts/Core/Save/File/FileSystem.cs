using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSK
{
    public class FileSystem
    { 
        
        public void Save<T>(string fileName, T data, bool isEncrypt = false)
        {
            try
            {
                var path = IOUtility.GetPath(fileName);
                OSK.Logg.Log("Path File: " + path);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    if (isEncrypt)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            binaryFormatter.Serialize(memoryStream, data);
                            var encryptedData = FileSecurity.Encrypt(memoryStream.ToArray());
                            file.Write(encryptedData, 0, encryptedData.Length);
                        }
                    }
                    else
                    {
                        binaryFormatter.Serialize(file, data);
                    }
                }

                RefreshEditor();
                OSK.Logg.Log($"[Save File Success]: {fileName} {DateTime.Now}\n{path}");
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError($"[Save File Exception]: {fileName} {ex.Message}");
            }
        }
        
        public T Load<T>(string fileName, bool isDecrypt = false)
        {
            try
            {
                var path = IOUtility.GetPath(fileName);
                if (!File.Exists(path))
                {
                    OSK.Logg.LogError("[Load File Error]: " + fileName + " NOT found");
                    return default;
                }

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.Open))
                {
                    if (isDecrypt)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(FileSecurity.Decrypt(file)))
                        {
                            return (T)binaryFormatter.Deserialize(memoryStream);
                        }
                    }
                    else
                    {
                        return (T)binaryFormatter.Deserialize(file);
                    }
                }
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError("[Load File Exception]: " + fileName + " " + ex.Message);
                return default;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        
        public List<string> GetAll(string fileName)
        {
            List<string> allFiles = new List<string>();
            var path = IOUtility.GetPath(fileName);

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    allFiles.Add(Path.GetFileName(file));
                }
            }

            return allFiles;
        }

        public void Write(string fileName, string json)
        {
            var path = IOUtility.GetPath(fileName);
            OSK.Logg.Log("Path Save: " + path);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }

        public string Read(string fileName)
        {
            var path =  IOUtility.GetPath(fileName);
            if (File.Exists(path))
            {
                var reader = new StreamReader(path);
                return reader.ReadToEnd();
            }
            else
            {
                OSK.Logg.LogError("File Not Found !");
            }

            return null;
        }

        private void RefreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}