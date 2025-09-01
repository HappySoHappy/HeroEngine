using System.Security.Cryptography;
using System.Text;

namespace HeroEngine.Util
{
    public static class Encryption
    {

        public static string AESEncrypt(string text)
        {
            text = XOR(text);
            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey(32); // 32 bytes for AES-256
                aes.IV = GenerateKey(16);  // 16 bytes for AES IV
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] textBytes = Encoding.UTF8.GetBytes(text);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public static string AESDecrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey(32); // 32 bytes for AES-256
                aes.IV = GenerateKey(16);  // 16 bytes for AES IV
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return XOR(Encoding.UTF8.GetString(decryptedBytes));
                }
            }
        }

        public static string XOR(string text)
        {
            var result = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                result.Append((char)(text[i] ^ 0x88));
            }

            return result.ToString();
        }

        private static byte[] GenerateKey(int size)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes("A2579"));
                byte[] key = new byte[size];
                Array.Copy(hash, key, Math.Min(hash.Length, key.Length));
                return key;
            }
        }

        public static string BcryptEncrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
        }

        public static bool BcryptCompare(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
