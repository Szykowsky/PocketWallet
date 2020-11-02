using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PocketWallet.Helpers
{
    public static class HashHelper
    {
        public static string SHA512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                return CalculateHash(hashedInputBytes);
            }
        }

        public static string HMACSHA512(string input, string secretKey)
        {
            var secretkeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var inputBytes = Encoding.UTF8.GetBytes(input);

            using (var hmac = new HMACSHA512(secretkeyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                return CalculateHash(hashValue);
            }
        }

        private static string CalculateHash(byte[] hashedInputBytes)
        {
            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
            }

            return hashedInputStringBuilder.ToString();
        }
    }
}
