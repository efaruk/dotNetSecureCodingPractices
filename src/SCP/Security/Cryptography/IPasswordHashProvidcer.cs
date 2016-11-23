namespace SCP.Security.Cryptography
{
    public interface IPasswordHashProvidcer
    {
        string Hash(string clearText, string salt);
    }
}