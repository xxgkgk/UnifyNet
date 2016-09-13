using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using UnCommon.Config;
using UnCommon.Tool;

namespace UnCommon.Encrypt
{
    /// <summary>
    /// md5加密类
    /// </summary>
    public class UnEncMD5
    {
        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static string getMd5Hash(FileInfo f)
        {
            return getMd5Hash(f.FullName, 0);
        }

        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getMd5Hash(string filePath, int type)
        {
            string smd5 = filePath;
            try
            {
                using (FileStream t = new FileStream(filePath, FileMode.Open))
                {
                    int l = 1024;
                    long l1 = t.Length / 3;
                    long l2 = t.Length / 2;
                    if (l1 < l)
                    {
                        l = (int)l1;
                    }
                    byte[] begin = new byte[l];
                    byte[] middle = new byte[l];
                    byte[] end = new byte[l];
                    t.Seek(0, SeekOrigin.Begin);
                    t.Read(begin, 0, begin.Length);

                    t.Seek(l2, SeekOrigin.Begin);
                    t.Read(middle, 0, middle.Length);

                    t.Seek(t.Length - end.Length, SeekOrigin.Begin);
                    t.Read(end, 0, end.Length);

                    string body = UnTo.byteToIntStr(begin) + UnTo.byteToIntStr(middle) + UnTo.byteToIntStr(end);
                    return getMd5Hash(body);
                }
            }
            catch 
            {
                return getMd5Hash(filePath);
            }
        }

        /// <summary>
        /// 字符串MD5
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string getMd5Hash(string s)
        {
            //实例化一个md5对像
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] data = md5Hasher.ComputeHash(UnInit.getEncoding().GetBytes(s));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            StringBuilder sBuilder = new StringBuilder();
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // 返回字符串
            return sBuilder.ToString();
        }

        /// <summary>
        /// 变异加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string getMd5Hashs(string s)
        {
            string s1 = getMd5Hash(s) + UnInit.getMD5Key();
            s1 = getMd5Hash(s1.ToLower());
            return s1;
        }

        /// <summary>
        /// 双重加密 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string getMd5Hash2(string s)
        {
            string s1 = getMd5Hash(s);
            s1 = getMd5Hash(s1.ToLower());
            return s1;
        }


        /// <summary>
        /// 字节组md5
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string getMd5Hash(byte[] t)
        {
            return getMd5Hash(UnTo.byteToIntStr(t));
        }

    }
}
