namespace SCP.Security.Cryptography
{
    public interface IEncryptionProvider
    {
        string Decrypt(string ciphertext, string key, string vector);

        string Encrypt(string plainText, string key, string vector);

        string GenerateKey();

        string GenerateVector();
    }
}