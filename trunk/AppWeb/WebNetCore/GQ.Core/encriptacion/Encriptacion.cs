using System;
using System.Security.Cryptography;
using System.Text;

namespace GQ.Core.encriptacion
{
    /// <summary>
    /// Proveee servicios para encripatacion de strings usando MD5
    /// </summary>
    public static class Encriptacion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Encriptar(string value, string strKey)
        {
            return encrypt(value, strKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Desencriptar(string value, string strKey)
        {
            return decrypt(value, strKey);
        }

        /// <summary>
        /// Encrypt the given string using the specified key.
        /// </summary>
        /// <param name="strToEncrypt">The string to be encrypted.</param>
        /// <param name="strKey">The encryption key.</param>
        /// <returns>The encrypted string.</returns>
        private static string encrypt(string strToEncrypt, string strKey)
        {
            if (string.IsNullOrWhiteSpace(strToEncrypt))
                strToEncrypt = "";
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto =
                    new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey;
                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.UTF8.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = ASCIIEncoding.UTF8.GetBytes(strToEncrypt);
                return Convert.ToBase64String(objDESCrypto.CreateEncryptor().
                    TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        /// <summary>
        /// Decrypt the given string using the specified key.
        /// </summary>
        /// <param name="strEncrypted">The string to be decrypted.</param>
        /// <param name="strKey">The decryption key.</param>
        /// <returns>The decrypted string.</returns>
        private static string decrypt(string strEncrypted, string strKey)
        {
            if (!string.IsNullOrWhiteSpace(strEncrypted))
            {
                try
                {
                    TripleDESCryptoServiceProvider objDESCrypto =
                        new TripleDESCryptoServiceProvider();
                    MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                    byte[] byteHash, byteBuff;
                    string strTempKey = strKey;
                    byteHash = objHashMD5.ComputeHash(ASCIIEncoding.UTF8.GetBytes(strTempKey));
                    objHashMD5 = null;
                    objDESCrypto.Key = byteHash;
                    objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                    byteBuff = Convert.FromBase64String(strEncrypted);
                    string strDecrypted = ASCIIEncoding.UTF8.GetString
                    (objDESCrypto.CreateDecryptor().TransformFinalBlock
                    (byteBuff, 0, byteBuff.Length));
                    objDESCrypto = null;
                    return strDecrypted;
                }
                catch (Exception ex)
                {
                    return "Wrong Input. " + ex.Message;
                }
            }

            return strEncrypted;
        }
    }
}