using System.Security.Cryptography;
using System.Text;

namespace SCP.Security.Cryptography
{
    public class DefaultPasswordHashProvider : IPasswordHashProvidcer
    {
        public string Hash(string clearText, string salt)
        {
            if (string.IsNullOrEmpty(clearText)) clearText = "";
            if (string.IsNullOrEmpty(salt)) salt = "";
            var salted = clearText + salt;
            var bytes = Encoding.Unicode.GetBytes(salted);
            var sha256Managed = new SHA256Managed();
            var hashBytes = sha256Managed.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.AppendFormat("{0:x2}", b);
            var hash = sb.ToString();
            return hash;
        }
    }
}