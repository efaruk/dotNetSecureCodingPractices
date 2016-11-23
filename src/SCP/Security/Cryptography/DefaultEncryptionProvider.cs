using System;
using System.IO;
using System.Security.Cryptography;

namespace SCP.Security.Cryptography
{
    public class DefaultEncryptionProvider : IEncryptionProvider
    {
        public string Decrypt(string ciphertext, string key, string vector)
        {
            string plainText;
            var keyz = Convert.FromBase64String(key);
            var iv = Convert.FromBase64String(vector);
            var bytes = Convert.FromBase64String(ciphertext);
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = keyz;
                provider.IV = iv;
                using (var decryptor = provider.CreateDecryptor(keyz, iv))
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                plainText = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            return plainText;
        }

        public string Encrypt(string plainText, string key, string vector)
        {
            string ciphertext;
            var keyz = Convert.FromBase64String(key);
            var iv = Convert.FromBase64String(vector);
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = keyz;
                provider.IV = iv;
                using (var encryptor = provider.CreateEncryptor(keyz, iv))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                            var encrypted = ms.ToArray();
                            ciphertext = Convert.ToBase64String(encrypted);
                        }
                    }
                }
            }
            return ciphertext;
        }

        public string GenerateKey()
        {
            string key;
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.GenerateKey();
                var bytes = provider.Key;
                key = Convert.ToBase64String(bytes);
            }
            return key;
        }

        public string GenerateVector()
        {
            string vector;
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.GenerateIV();
                var bytes = provider.IV;
                vector = Convert.ToBase64String(bytes);
            }
            return vector;
        }
    }
}