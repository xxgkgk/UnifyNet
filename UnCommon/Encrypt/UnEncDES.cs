using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using UnCommon.Config;

namespace UnCommon.Encrypt
{
    /// <summary>
    /// DES
    /// </summary>
    public class UnEncDES
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="en">待加密数据</param>
        /// <param name="key">密钥,8的倍数</param>
        /// <returns>失败返回源数据</returns>
        public static string encrypt(string key, string en)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(en);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return en;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="de">待解密数据</param>
        /// <param name="key">密钥,8的倍数</param>
        /// <returns>失败返回源数据</returns>
        public static string decrypt(string key, string de)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Convert.FromBase64String(de);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return de;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="en">待加密的字符串</param>
        /// <returns></returns>
        public static string encrypt(string en)
        {
            return encrypt(UnInit.getDESKey(), en);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="de">待解密的字符串</param>
        /// <returns></returns>
        public static string decrypt(string de)
        {
            return decrypt(UnInit.getDESKey(), de);
        }
    }

}
