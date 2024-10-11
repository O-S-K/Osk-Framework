using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace OSK
{
    public class XMLSystem
    {
        public void Save<T>(T data, string namePath)
        {
            string fullPath = GetDataPath(namePath);
            using (FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, data);
            }
        }

        public T Load<T>(string namePath)
        {
            string fullPath = GetDataPath(namePath);
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"File not found at path: {fullPath}");
                return default(T);
            }

            using (FileStream stream = File.Open(fullPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }

        public static string GetDataPath(string namePath)
        {
            return $"{Application.persistentDataPath}/{namePath}";
        }
    }
}