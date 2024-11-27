using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OSK
{
    public static class PathUtility
    { 
        public static string GetPath(string fileName)
        {
            string directory = Application.persistentDataPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return Path.Combine(directory, $"{fileName}");
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