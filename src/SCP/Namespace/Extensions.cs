using System;
using System.Security.Cryptography;
using System.Text;

namespace SCP
{
    public static class Extensions
    {
        public static string ToMd5(this string text)
        {
            if (string.IsNullOrEmpty(text)) text = "";
            var byteDataToHash = Encoding.ASCII.GetBytes(text);
            var byteHashValue = new MD5CryptoServiceProvider().ComputeHash(byteDataToHash);
            return BitConverter.ToString(byteHashValue).Replace("-", "").ToLower();
        }

        public static string SanitizeEmail(this string email)
        {
            if (string.IsNullOrEmpty(email)) return "";
            if (!email.Contains("@"))
            {
                return email;
            }
            var ix = email.IndexOf("@", StringComparison.Ordinal);
            var subEmail = email.Substring(0, ix) + "*****";
            return subEmail;
        }
    }
}