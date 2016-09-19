using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace WebApp.Plumping { 
    public static class AesCryptor {
        public static byte[] KeyAndIvBytes {
            get { return Convert.FromBase64String(ConfigurationManager.AppSettings["payway_encryption_key"]); }
        } 
        public static string ByteArrayToHexString(byte[] ba) {
            return BitConverter.ToString(ba);
        } 
        public static byte[] StringToByteArray(string hex) {
            return Convert.FromBase64String(hex); 
        }
        public static string DecodeAndDecrypt(string cipherText)
        {
            return AesDecrypt(StringToByteArray(cipherText)); 
        } 
        public static string EncryptAndEncode(string plaintext) {
            return ByteArrayToHexString(AesEncrypt(plaintext));
        } 
        private static string AesDecrypt(Byte[] inputBytes) {
            string plaintext;
            var lst = new List<byte>();
            lst.AddRange(IV);
            lst.AddRange(inputBytes); 
            using (var memoryStream = new MemoryStream(lst.ToArray())) {
                using (var cryptoStream = new CryptoStream(memoryStream, GetCryptoAlgorithm().CreateDecryptor(KeyAndIvBytes, KeyAndIvBytes), CryptoStreamMode.Read)) {
                    using (var srDecrypt = new StreamReader(cryptoStream)) {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plaintext;//.Substring(IV.Length - 1)
        } 
        private static byte[] AesEncrypt(string inputText) {
            var inputBytes = Encoding.UTF8.GetBytes(inputText);//AbHLlc5uLone0D1q
            byte[] result;
            using (var memoryStream = new MemoryStream()) {
                using (var cryptoStream = new CryptoStream(memoryStream, GetCryptoAlgorithm().CreateEncryptor(KeyAndIvBytes, KeyAndIvBytes), CryptoStreamMode.Write)) {
                    cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    cryptoStream.FlushFinalBlock(); 
                    result = memoryStream.ToArray();
                }
            } 
            return result;
        }

        private static Byte[] IV = new Byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        private static RijndaelManaged GetCryptoAlgorithm() {
            return new RijndaelManaged {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 128, //16 bytes, result in a 24 characters base64String key
                BlockSize = 128 //16 bytes
            }; 
        }
    }
}