using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnCommon.UDP;
using UnCommon.Encrypt;
using UnCommon.Config;

namespace UnCommon.Extend
{
    /// <summary>
    /// string扩展类
    /// </summary>
    public static class UnExtString
    {
        /// <summary>
        /// 将字符串转换为Int
        /// </summary>
        /// <param name="t">字符串</param>
        /// <returns>当转换失败时返回null</returns>
        public static int? toInt(this string t)
        {
            int r;
            if (int.TryParse(t, out r))
            {
                return r;
            }
            return null;
        }

        /// <summary>
        /// 将字符串转为Long
        /// </summary>
        /// <param name="t">字符串</param>
        /// <returns>当转换失败时返回null</returns>
        public static long? toLong(this string t)
        {
            long r;
            if (long.TryParse(t, out r))
            {
                return r;
            }
            return null;
        }

        /// <summary>
        /// 将字符串转为double
        /// </summary>
        /// <param name="t">字符串</param>
        /// <returns>当转换失败时返回null</returns>
        public static double? toDouble(this string t)
        {
            double r;
            if(double.TryParse(t, out r))
            {
                return r;
            }
            return null;
        }

        /// <summary>
        /// 将字符串转为decimal
        /// </summary>
        /// <param name="t">字符串</param>
        /// <returns>当转换失败时返回null</returns>
        public static decimal? toDecimal(this string t)
        {
            decimal r;
            if (decimal.TryParse(t, out r))
            {
                return r;
            }
            return null;
        }

        /// <summary>
        /// 将字符串转为Guid
        /// </summary>
        /// <param name="t">字符串</param>
        /// <returns>当转换失败时返回null</returns>
        public static Guid? toGuid(this string t)
        {
            Guid r;
            if (Guid.TryParse(t, out r))
            {
                return r;
            }
            return null;
        }

        /// <summary>
        /// 将字符串转为bool
        /// </summary>
        /// <param name="t">字符串</param>
        /// <returns>当转换失败时返回null</returns>
        public static bool? toBool(this string t)
        {
            bool r;
            if (bool.TryParse(t, out r))
            {
                return r;
            }
            return null;
        }

        /// <summary>
        /// 16位加密
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>返回16位md5</returns>
        public static string md5Hash16(this string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UnInit.getEncoding().GetBytes(input)), 4, 8);
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }

        /// <summary>
        /// 32位加密
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>返回32位md5</returns>
        public static string md5Hash(this string input)
        {
            return UnEncMD5.getMd5Hash(input);
        }

        /// <summary>
        /// 强加密
        /// </summary>
        /// <returns>返回强加密md5</returns>
        public static string md5Hashs(this string input)
        {
            return UnEncMD5.getMd5Hashs(input);
        }

        /// <summary>
        /// 校验是否md5加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool isMd5Hash(this string input, string hash)
        {
            // Hash the input.
            string hashOfInput = md5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="t"></param>
        /// <param name="beginString">开始字符串</param>
        /// <param name="endString">结束字符串</param>
        /// <returns></returns>
        public static string subStringBetween(this string t, string beginString, string endString)
        {
            byte[] beginBytes = UnInit.getEncoding().GetBytes(beginString);
            byte[] endBytes = UnInit.getEncoding().GetBytes(endString);
            byte[] b = UnInit.getEncoding().GetBytes(t);
            byte[] data = b.subByteBetween(beginBytes, endBytes);
            if (data != null)
            {
                return UnInit.getEncoding().GetString(data);
            }
            return null;
        }

        /// <summary>
        /// 获取UDP事件枚举
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static UnUdpEveEnum getUnUdpEveEnum(this string t)
        {
            switch (t)
            {
                case "up":
                    return UnUdpEveEnum.upFilePackage;
                case "upb":
                    return UnUdpEveEnum.upFilePackageBack;
                case "uq":
                    return UnUdpEveEnum.upFileQuery;
                case "uqb":
                    return UnUdpEveEnum.upFileQueryBack;
                case "mp":
                    return UnUdpEveEnum.msgPackage;
                case "mpb":
                    return UnUdpEveEnum.msgPackageBack;
                case "df":
                    return UnUdpEveEnum.downFile;
                case "dfb":
                    return UnUdpEveEnum.downFileBack;
                case "dq":
                    return UnUdpEveEnum.downFileQuery;
                case "dqb":
                    return UnUdpEveEnum.downFileQueryBack;
                default:
                    return UnUdpEveEnum.none;
            }
        }

        /// <summary>
        /// sha1
        /// </summary>
        /// <param name="t"></param>
        /// <param name="isRemove"></param>
        /// <returns></returns>
        public static string sha1Hash(this string t, bool isRemove)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(t);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            if (isRemove)
            {
                str_sha1_out = str_sha1_out.Replace("-", "");
            }
            return str_sha1_out;
        }

        /// <summary>
        /// 字节数
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int byteLength(this string t)
        {
            byte[] sarr = System.Text.Encoding.Default.GetBytes(t);
            int len = sarr.Length;
            return len;
        }

        /// <summary>
        /// 字节区间判断
        /// </summary>
        /// <param name="t"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool isBetweenLength(this string t, int? min, int? max)
        {
            int length = (t + "").byteLength();
            if (min != null && length < min)
            {
                return false;
            }
            if (max != null && length > max)
            {
                return false;
            }
            return true;
        } 

    }
}
