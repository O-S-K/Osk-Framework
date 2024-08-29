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
           return PathFile.Path(fileName + ".txt", isSaveToDocument);
        }

        public bool ExistsFile(string fileName, bool isSaveToDocument = true)
        {
            return File.Exists(GetPath(fileName, isSaveToDocument));
        }
         
        public void SaveData<T>(string fileName, object data, bool isSaveToDocument = true)
        {
            try
            {
                if (data == null) return;
                var path = GetPath(fileName, isSaveToDocument);
                Logger.Log("Path File" + path);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(file, (T)data);
                    //Utils.Utilities.CalculateMD5Hash(file.ToString());
                    file.Close();
                    RefreshEditor();
                    Logger.Log("[Save File Susscess]: " + fileName + " " + DateTime.Now + "\n" + path);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("[Save File Exception]: " + fileName + " " + ex.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        public T LoadData<T>(string fileName, bool isSaveToDocument = true)
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
                        Logger.Log("[Load File Susscess]: " + fileName + ".txt");
                        Logger.Log("Path File" + path);
                    }

                    return data;
                }
                else
                {
                    Logger.LogError("[Load File Error]: " + fileName + " " + "NOT found");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("[Load File Exception]: " + fileName + " " + ex.Message);
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
            
            Logger.Log("Path Save: " + path);
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
                Logger.LogError("File Not Found !");
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
                    Logger.Log("[Delete File Susscess]: " + fileName);
                    File.Delete(path);
                    RefreshEditor();
                }
                else
                {
                    Logger.LogError("[Delete File Error]: " + fileName + " " + "NOT found");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("[Delete File Exception]: " + fileName + " " + ex.Message);
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
