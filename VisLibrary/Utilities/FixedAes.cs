using System.Security.Cryptography;
using System.Text;


namespace VisLibrary.Utilities
{
    public class FixedAes
    {
        private static readonly Aes _aes = Aes.Create();

        static FixedAes()
        {
            _aes.KeySize = 256; // 192、256
            _aes.Key = Encoding.ASCII.GetBytes("R#7kDfP9@Tq2&eXu"); // 16字元
            _aes.IV = Encoding.ASCII.GetBytes("G@5sMvW3^Zr1*pLq"); // 16字元
        }

        /// <summary>加密</summary>
        public static string Encrypt(object value)
        {
            return _aes.Encrypt(value);
        }

        /// <summary>解密</summary>
        public static string Decrypt(string value)
        {
            return _aes.Decrypt(value);
        }
    }
}

