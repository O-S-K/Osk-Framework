using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OSK
{
    public static class IOUtility
    {
        public static string StreamingAssetsPath
        {
            get
            {
#if UNITY_ANDROID
                return "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IOS
                return "file://" + Application.streamingAssetsPath;
#else
                return Application.streamingAssetsPath;
#endif
            }
        }

        public static bool IsFileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

        public static bool IsDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }

            return false;
        }

        public static bool DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
                return true;
            }

            return false;
        }


        public static string GetPath(string fileName)
        {
            string directory = Application.persistentDataPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return Path.Combine(directory, $"{fileName}");
        }

        public static string FilePath(string name)
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, $"{name}");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, $"{name}");
            }
            return filePath;
        }
        
        
        public static List<string> GetAll(string fileName)
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

#if UNITY_EDITOR
        public static string GetPathAfterResources(Object asset)
        {
            string fullPath = UnityEditor.AssetDatabase.GetAssetPath(asset);
            int resourcesIndex = fullPath.IndexOf("Resources/");
            if (resourcesIndex >= 0)
            {
                return fullPath.Substring(resourcesIndex + "Resources/".Length);
            }

            Logg.LogWarning("Asset not found in resources");
            return fullPath;
        }
#endif
    }
}