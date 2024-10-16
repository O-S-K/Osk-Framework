using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace OSK
{
    public class SecurityData  
    {
        private static readonly string key = "b14ca5898a4e4133bbce2ea2315a1916";

        public static string Encrypt(string plainText)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            using (Aes aes = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(key, new byte[]
                {
                    0x49, 0x76, 0x61, 0x6e, 0x20,
                    0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                    }

                    plainText = System.Convert.ToBase64String(ms.ToArray());
                }
            }

            return plainText;
        }

        public static string Decrypt(string cipherText)
        {
            byte[] data = System.Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(key, new byte[]
                {
                    0x49, 0x76, 0x61, 0x6e, 0x20,
                    0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.UTF8.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }
        
        
        public static string CalculateMD5Hash(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            Logg.Log("CalculateMD5Hash" +  sb.ToString());
            return sb.ToString();
        }
    }
}