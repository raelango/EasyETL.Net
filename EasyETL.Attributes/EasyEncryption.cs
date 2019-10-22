using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Attributes
{
    public static class EasyEncryption
    {
        private const string AesIV256 = @"!QAZ2WSX#EDC4RFV";
        private const string AesKey256 = @"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";


        public static string Encrypt256(this string text)
        {
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Convert string to byte array
            byte[] src = Encoding.Unicode.GetBytes(text);

            // encryption
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                // Convert byte array to Base64 strings
                return Convert.ToBase64String(dest);
            }
        }

        /// <summary>
        /// AES decryption
        /// </summary>
        public static string Decrypt256(this string text)
        {
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Convert Base64 strings to byte array

            // decryption
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                try
                {
                    byte[] src = System.Convert.FromBase64String(text);
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    return Encoding.Unicode.GetString(dest);

                }
                catch
                {
                    return text;
                }
            }
        }
    }
}
