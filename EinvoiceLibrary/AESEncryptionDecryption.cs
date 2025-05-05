using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class ClsEncryptionDecryption
    {
        public string Encryption(string PlainText, string Key)
        {
            string EncryptedData = "";
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                //objrij.Mode = CipherMode.CBC;
                objrij.Mode = CipherMode.ECB;
                objrij.Padding = PaddingMode.PKCS7;
                byte[] textData = Encoding.UTF8.GetBytes(PlainText);
                objrij.BlockSize = 128;
                objrij.KeySize = 128;
                objrij.Key = StringToByteArray(Key);
                //objrij.IV =byte[128];




                ICryptoTransform encryptTransform = objrij.CreateEncryptor();
                string Data = ByteArrayToString(encryptTransform.TransformFinalBlock(textData, 0, textData.Length));
                encryptTransform.Dispose();
                EncryptedData = Data.ToUpper();
            }
            catch (Exception EX)
            {
                throw EX;
            }
            return EncryptedData;
        }
        public string Encryption(string PlainText, string Key, string IV)
        {
            string EncryptedData = "";
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                objrij.Mode = CipherMode.CBC;
                objrij.Padding = PaddingMode.PKCS7;
                byte[] textData = Encoding.UTF8.GetBytes(PlainText);
                objrij.BlockSize = 128;
                objrij.KeySize = 128;
                objrij.Key = StringToByteArray(Key);
                objrij.IV = StringToByteArray(IV);
                
                ICryptoTransform encryptTransform = objrij.CreateEncryptor();
                string Data = ByteArrayToString(encryptTransform.TransformFinalBlock(textData, 0, textData.Length));
                encryptTransform.Dispose();
                EncryptedData = Data.ToUpper();
            }
            catch (Exception EX)
            {
                throw EX;
            }
            return EncryptedData;
        }
        public string Decryption(string encryptedText, string Key, string IV)
        {
            string DecryptedData = "";
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                objrij.Mode = CipherMode.CBC;
                objrij.Padding = PaddingMode.PKCS7;
                objrij.BlockSize = 128;
                objrij.KeySize = 128;
                objrij.Key = StringToByteArray(Key);
                objrij.IV = StringToByteArray(IV);
                byte[] inputByteArray = StringToByteArray(encryptedText);
                ICryptoTransform encryptTransform = objrij.CreateDecryptor();
                byte[] TextByte = encryptTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                DecryptedData = Encoding.UTF8.GetString(TextByte);
                return DecryptedData;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public string Decryption(string encryptedText, byte[] Key, string IV)
        {
            string DecryptedData = "";
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                objrij.Mode = CipherMode.CBC;
                objrij.Padding = PaddingMode.PKCS7;
                objrij.BlockSize = 128;
                objrij.KeySize = 128;
                objrij.Key = Key;
                objrij.IV = StringToByteArray(IV);
                byte[] inputByteArray = StringToByteArray(encryptedText);
                ICryptoTransform encryptTransform = objrij.CreateDecryptor();
                byte[] TextByte = encryptTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                DecryptedData = Encoding.UTF8.GetString(TextByte);
                return DecryptedData;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public string Decryption(string encryptedText, string Key)
        {
            string DecryptedData = "";
            try
            {
                RijndaelManaged objrij = new RijndaelManaged();
                objrij.Mode = CipherMode.ECB;
                objrij.Padding = PaddingMode.PKCS7;
                objrij.BlockSize = 128;
                objrij.KeySize = 128;
                objrij.Key = StringToByteArray(Key);
                //objrij.IV = StringToByteArray(IV);
                byte[] inputByteArray = StringToByteArray(encryptedText);
                ICryptoTransform encryptTransform = objrij.CreateDecryptor();
                byte[] TextByte = encryptTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                DecryptedData = Encoding.UTF8.GetString(TextByte);
                return DecryptedData;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }
    }
}
