using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSK
{
    public class FileSystem : IFile
    {
        public void Save<T>(string fileName, T data, bool isEncrypt = false)
        {
            try
            {
                var path = IOUtility.GetPath(fileName + ".txt");

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    if (isEncrypt)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            binaryFormatter.Serialize(memoryStream, data);
                            var encryptedData = FileSecurity.Encrypt(memoryStream.ToArray(), Main.ConfigsManager.init.EncryptKey);
                            file.Write(encryptedData, 0, encryptedData.Length);
                        }
                    }
                    else
                    {
                        binaryFormatter.Serialize(file, data);
                    }
                }

                RefreshEditor();
                OSK.Logg.Log($"[Save File Success]: {fileName + ".txt"} {DateTime.Now}\n{path}", Color.green);
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError($"[Save File Exception]: {fileName + ".txt"} {ex.Message}");
            }
        }

        public T Load<T>(string fileName, bool isDecrypt = false)
        {
            try
            {
                var path = IOUtility.GetPath(fileName + ".txt");
                if (!File.Exists(path))
                {
                    OSK.Logg.LogError("[Load File Error]: " + fileName + ".txt" + " NOT found");
                    return default;
                }

                Logg.Log($"[Load File Success]: {fileName + ".txt"} \n {path}", Color.green);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.Open))
                {
                    if (isDecrypt)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(FileSecurity.Decrypt(file, Main.ConfigsManager.init.EncryptKey)))
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
                OSK.Logg.LogError("[Load File Exception]: " + fileName + ".txt" + " " + ex.Message);
                return default;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void Delete(string fileName)
        {
            IOUtility.DeleteFile(fileName + ".txt");
            OSK.Logg.Log($"[Delete File Success]: {fileName}.txt");
        }
        
        public void Write(string fileName, string json)
        {
            var path = IOUtility.GetPath(fileName + ".txt");
            OSK.Logg.Log("Path Save: " + path);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }

        public string Read(string fileName)
        {
            var path = IOUtility.GetPath(fileName + ".txt");
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