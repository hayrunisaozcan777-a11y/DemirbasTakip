using System.Security.Cryptography;
using System.Text;

namespace DemirbasTakip.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string plainText)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var hashBytes = sha256.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var b in hashBytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static bool Verify(string plainText, string hash) => Hash(plainText) == hash;
    }
}