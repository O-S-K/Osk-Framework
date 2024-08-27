using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using OSK.Utils;

namespace OSK.Save
{
    public static class FileUtils
    { 
        private static string getPath(string fileName, bool isSaveToDocument = true)
        {
           return PathFile.Path(fileName + ".txt", isSaveToDocument);
        }

        public static bool ExistsFile(string fileName, bool isSaveToDocument = true)
        {
            return File.Exists(getPath(fileName, isSaveToDocument));
        }
         
        public static void SaveData<T>(string fileName, object data, bool isSaveToDocument = true)
        {
            try
            {
                if (data == null) return;
                var path = getPath(fileName, isSaveToDocument);
                CustomDebug.Log("Path File", path);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(file, (T)data);
                    // Utils.Utilities.CalculateMD5Hash(file.ToString());
                    file.Close();
                    refreshEditor();
                    CustomDebug.Log("[Save File Susscess]: " + fileName + " " + DateTime.Now + "\n" + path);
                }
            }
            catch (Exception ex)
            {
                CustomDebug.LogError("[Save File Exception]: " + fileName + " " + ex.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        public static object LoadData<T>(string fileName, bool isSaveToDocument = true)
        {
            try
            {
                var data = default(T);
                var path = getPath(fileName, isSaveToDocument);
                if (File.Exists(path))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (FileStream file = File.Open(path, FileMode.Open))
                    {
                        data = (T)binaryFormatter.Deserialize(file);
                        file.Close();
                        CustomDebug.Log("[Load File Susscess]: " + fileName + ".txt");
                        CustomDebug.Log("Path File", path);
                    }

                    return data;
                }
                else
                {
                    CustomDebug.LogError("[Load File Error]: " + fileName + " " + "NOT found");
                    return null;
                }
            }
            catch (Exception ex)
            {
                CustomDebug.LogError("[Load File Exception]: " + fileName + " " + ex.Message);
                return null;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        public static void WriteToFile(string fileName, string json, bool isSaveToDocument = true)
        {
            var path = getPath(fileName, isSaveToDocument);
            
            CustomDebug.Log("Path Save: " + path);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        } 
        public static string ReadFromFile(string fileName, bool isSaveToDocument = true)
        {
            var path = getPath(fileName, isSaveToDocument);
            if (File.Exists(path))
            {
                var reader = new StreamReader(path);
                return reader.ReadToEnd();
            }
            else
            {
                CustomDebug.LogError("File Not Found !");
            }
            return null;
        }

        public static void DeleteFile(string fileName, bool isSaveToDocument = true)
        {
            try
            {
                var path = getPath(fileName, isSaveToDocument);
                if (File.Exists(path))
                {
                    CustomDebug.Log("[Delete File Susscess]: " + fileName);
                    File.Delete(path);
                    refreshEditor();
                }
                else
                {
                    CustomDebug.LogError("[Delete File Error]: " + fileName + " " + "NOT found");
                }
            }
            catch (Exception ex)
            {
                CustomDebug.LogError("[Delete File Exception]: " + fileName + " " + ex.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private static void refreshEditor()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

    }
}
