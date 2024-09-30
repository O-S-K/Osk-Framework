using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using OSK.Utils;

namespace OSK
{
    public class FileSystem
    {
        public string GetPath(string fileName, bool isSaveToDocument = true)
        {
            return PathFile.GetPath($"{fileName}");
        }

        public void SaveData<T>(string fileName, object data, bool isSaveToDocument = true)
        {
            try
            {
                if (data == null) return;
                var path = GetPath(fileName, isSaveToDocument);
                Debug.Log("Path File" + path);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(file, (T)data);
                    //Utils.Utilities.CalculateMD5Hash(file.ToString());
                    file.Close();
                    RefreshEditor();
                    Debug.Log("[Save File Success]: " + fileName + " " + DateTime.Now + "\n" + path);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[Save File Exception]: " + fileName + " " + ex.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        public T Load<T>(string fileName, bool isSaveToDocument = true)
        {
            try
            {
                var data = default(T);
                var path = GetPath(fileName, isSaveToDocument);
                if (File.Exists(path))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (FileStream file = File.Open(path, FileMode.Open))
                    {
                        data = (T)binaryFormatter.Deserialize(file);
                        file.Close();
                        Debug.Log("[Load File Susscess]: " + fileName + ".txt");
                        Debug.Log("Path File" + path);
                    }

                    return data;
                }
                else
                {
                    Debug.LogError("[Load File Error]: " + fileName + " " + "NOT found");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[Load File Exception]: " + fileName + " " + ex.Message);
                return default(T);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        public void WriteToFile(string fileName, string json, bool isSaveToDocument = true)
        {
            var path = GetPath(fileName, isSaveToDocument);

            Debug.Log("Path Save: " + path);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }

        public string ReadFromFile(string fileName, bool isSaveToDocument = true)
        {
            var path = GetPath(fileName, isSaveToDocument);
            if (File.Exists(path))
            {
                var reader = new StreamReader(path);
                return reader.ReadToEnd();
            }
            else
            {
                Debug.LogError("File Not Found !");
            }

            return null;
        }

        public void DeleteFile(string fileName, bool isSaveToDocument = true)
        {
            try
            {
                var path = GetPath(fileName, isSaveToDocument);
                if (File.Exists(path))
                {
                    Debug.Log("[Delete File Success]: " + fileName);
                    File.Delete(path);
                    RefreshEditor();
                }
                else
                {
                    Debug.LogError("[Delete File Error]: " + fileName + " " + "NOT found");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[Delete File Exception]: " + fileName + " " + ex.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void RefreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}