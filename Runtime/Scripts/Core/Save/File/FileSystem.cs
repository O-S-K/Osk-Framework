using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;

namespace OSK
{
    public class FileSystem
    {
        public string GetPath(string fileName, bool isSaveToDocument = true)
        {
            return PathUtility.GetPath($"{fileName}");
        }

        public void SaveData<T>(string fileName, object data, bool isSaveToDocument = true)
        {
            try
            {
                if (data == null) return;
                var path = GetPath(fileName, isSaveToDocument);
                OSK.Logg.Log("Path File" + path);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(file, (T)data);
                    //Utils.Utilities.CalculateMD5Hash(file.ToString());
                    file.Close();
                    RefreshEditor();
                    OSK.Logg.Log("[Save File Success]: " + fileName + " " + DateTime.Now + "\n" + path);
                }
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError("[Save File Exception]: " + fileName + " " + ex.Message);
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
                        OSK.Logg.Log("[Load File Success]: " + fileName + ".txt");
                        OSK.Logg.Log("Path File" + path);
                    }

                    return data;
                }
                else
                {
                    OSK.Logg.LogError("[Load File Error]: " + fileName + " " + "NOT found");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError("[Load File Exception]: " + fileName + " " + ex.Message);
                return default(T);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        
        public List<string> GetAllFiles(string fileName, bool isSaveToDocument = true)
        {
            List<string> allFiles = new List<string>();

            if (Directory.Exists(GetPath(fileName, isSaveToDocument)))
            {
                var files = Directory.GetFiles(GetPath(fileName, isSaveToDocument));
                foreach (var file in files)
                {
                    allFiles.Add(Path.GetFileName(file));
                }
            }

            return allFiles;
        }

        public void WriteToFile(string fileName, string json, bool isSaveToDocument = true)
        {
            var path = GetPath(fileName, isSaveToDocument);

            OSK.Logg.Log("Path Save: " + path);
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
                OSK.Logg.LogError("File Not Found !");
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
                    OSK.Logg.Log("[Delete File Success]: " + fileName);
                    File.Delete(path);
                    RefreshEditor();
                }
                else
                {
                    OSK.Logg.LogError("[Delete File Error]: " + fileName + " " + "NOT found");
                }
            }
            catch (Exception ex)
            {
                OSK.Logg.LogError("[Delete File Exception]: " + fileName + " " + ex.Message);
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