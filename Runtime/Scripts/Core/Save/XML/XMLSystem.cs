using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace OSK
{
    public class XMLSystem
    {
        public void Save<T>(string nameFile, T data, bool isEncrypt = false)
        {
            string path = IOUtility.GetPath(nameFile + ".xml");

            using (FileStream stream = File.Open(path, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                if (isEncrypt)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        serializer.Serialize(memoryStream, data);
                        byte[] encryptedData = FileSecurity.Encrypt(memoryStream.ToArray());
                        stream.Write(encryptedData, 0, encryptedData.Length);
                    }
                }
                else
                {
                    serializer.Serialize(stream, data);
                }
                Logg.Log($"[Save File Success]: {nameFile}  {path} ");
            }
        }
        
        public T Load<T>(string nameFile, bool isEncrypt = false)
        {
            string path = IOUtility.GetPath(nameFile + ".xml");
            if (!File.Exists(path))
            {
                Debug.LogError($"File not found at path: {path}");
                return default(T);
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                if (isEncrypt)
                {
                    byte[] decryptedData = FileSecurity.Decrypt(stream);
                    using (MemoryStream memoryStream = new MemoryStream(decryptedData))
                    {
                        Logg.Log($"[Load File Success]: {nameFile}  {path} ");
                        return (T)serializer.Deserialize(memoryStream);
                    }
                }
                else
                {
                    Logg.Log($"[Load File Success]: {nameFile}  {path} ");
                    return (T)serializer.Deserialize(stream);
                }
            }
        }
    }
}