using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.IO;

namespace OSK
{
    public class FileSecurity  
    {

        public static byte[] Encrypt(byte[] data, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];  

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] Decrypt(FileStream fileStream, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16]; 

                using (ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, decrypt, CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(memoryStream);
                    }
                    return memoryStream.ToArray();
                }
            }
        }
        
        
        public static string CalculateMD5Hash(string input)
        {
            var md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            Logg.Log("CalculateMD5Hash" +  sb);
            return sb.ToString();
        }
    }
}