using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils.Encryption
{
    /// <summary>
    /// A class that attempts to mimic the MCrypt functionality within PhP
    /// </summary>
    public class MCrypt
    {
        /// <summary>
        /// Encrypts a string with the given key
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, byte[] key)
        {
            //This mimics the MCrypt function of PhP where:

            //$details = trim(mcrypt_encrypt(MCRYPT_DES, $key, $enquiry_id, MCRYPT_MODE_CBC, $key));
            //$details = base64_encode($details);

            byte[] toEncryptBytes = Encoding.ASCII.GetBytes(toEncrypt);

            DES des = DES.Create();
            des.IV = key;
            des.Key = key;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform encryptor = des.CreateEncryptor();

            byte[] encryptedBytes = encryptor.TransformFinalBlock(toEncryptBytes, 0, toEncryptBytes.Length);

            string base64EncryptedText = Convert.ToBase64String(encryptedBytes);
            
            return base64EncryptedText;
        }

        /// <summary>
        /// Decrypts a given string using the given key
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt, byte[] key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(toDecrypt);

            DES des = DES.Create();
            des.IV = key;
            des.Key = key;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform decryptor = des.CreateDecryptor();

            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            string decryptedText = Encoding.ASCII.GetString(decryptedBytes);

            return decryptedText;
        }
    }
}
