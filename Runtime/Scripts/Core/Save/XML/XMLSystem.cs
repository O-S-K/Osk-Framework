using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace OSK
{
    public class XMLSystem : IFile
    {
        public void Save<T>(string fileName, T data, bool isEncrypt = false)
        {
            string path = IOUtility.GetPath(fileName + ".xml");

            try
            {
                using (FileStream stream = File.Open(path, FileMode.OpenOrCreate))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    if (isEncrypt)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            serializer.Serialize(memoryStream, data);
                            byte[] encryptedData =
                                FileSecurity.Encrypt(memoryStream.ToArray(), Main.Configs.init.encryptKey);
                            stream.Write(encryptedData, 0, encryptedData.Length);
                        }
                    }
                    else
                    {
                        serializer.Serialize(stream, data);
                    }

                    Logg.Log($"[Save File Success]: {fileName + ".xml"} \n {path} ", Color.green);
                }
            }
            catch (System.Exception ex)
            {
                OSK.Logg.LogError($"[Save File Exception]: {fileName + ".xml"}  {ex.Message}");
            }
        }

        public T Load<T>(string fileName, bool isEncrypt = false)
        {
            string path = IOUtility.GetPath(fileName + ".xml");
            if (!File.Exists(path))
            {
                Logg.LogError($"[Load File Error]: {fileName + ".xml"}  {path}");
                return default(T);
            }

            try
            {
                using (FileStream stream = File.Open(path, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    if (isEncrypt)
                    {
                        byte[] decryptedData = FileSecurity.Decrypt(stream, Main.Configs.init.encryptKey);
                        using (MemoryStream memoryStream = new MemoryStream(decryptedData))
                        {
                            Logg.Log($"[Load File Success]: {fileName + ".xml"} \n {path}", Color.green);
                            return (T)serializer.Deserialize(memoryStream);
                        }
                    }
                    else
                    {
                        Logg.Log($"[Load File Success]: {fileName + ".xml"} \n {path}", Color.green);
                        return (T)serializer.Deserialize(stream);
                    }
                }
            }
            catch (System.Exception ex)
            {
                OSK.Logg.LogError($"[Load File Exception]: {fileName + ".xml"}  {ex.Message}");
                return default;
            }
        }

        public void Delete(string fileName)
        {
            IOUtility.DeleteFile(fileName + ".xml");
        }

        public T Query<T>(string fileName, bool condition)
        {
            if (condition)
            {
                return Load<T>(fileName);
            }
            return default;
        }
    }
}