using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OSK
{
    public static class PathFile
    { 
        public static string GetPath(string fileName)
        {
            // Use persistentDataPath for saving game data
            string directory = Application.persistentDataPath;

            // Ensure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return Path.Combine(directory, $"{fileName}");
        }
    }
}