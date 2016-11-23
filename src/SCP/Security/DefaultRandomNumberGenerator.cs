using System;
using System.Security.Cryptography;

namespace SCP.Security
{
    public class DefaultRandomNumberGenerator : IRandomNumberGenerator
    {
        public int Generate()
        {
            var rc = 0;
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                rc = BitConverter.ToInt32(tokenData, 0);
            }
            return rc;
        }

        public long GenerateLong()
        {
            long rc = 0;
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                rc = BitConverter.ToInt64(tokenData, 0);
            }
            return rc;
        }
    }
}