using System;
using System.Security.Cryptography;
using System.Text;

namespace SCP
{
    public static class Extensions
    {
        public static string ToMd5(this string text)
        {
            var byteDataToHash = Encoding.Unicode.GetBytes(text);
            var byteHashValue = new MD5CryptoServiceProvider().ComputeHash(byteDataToHash);
            return BitConverter.ToString(byteHashValue).Replace("-", "");
        }
    }
}