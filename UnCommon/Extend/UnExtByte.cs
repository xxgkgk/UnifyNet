using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Config;
using UnCommon.Encrypt;

namespace UnCommon.Extend
{
    /// <summary>
    /// byte扩展类
    /// </summary>
    public static class UnExtByte
    {
        /// <summary>
        /// 第一个匹配项的索引
        /// </summary>
        /// <param name="t">被执行查找的Byte[]</param>
        /// <param name="searchBytes">要查找的Byte[]</param>
        /// <returns>searchBytes 的索引位置；如果未找到该字节数组，则为 -1</returns>
        public static int indexOf(this byte[] t, byte[] searchBytes)
        {
            if (t == null) { return -1; }
            if (searchBytes == null) { return -1; }
            if (t.Length == 0) { return -1; }
            if (searchBytes.Length == 0) { return -1; }
            if (t.Length < searchBytes.Length) { return -1; }
            for (int i = 0; i < t.Length - searchBytes.Length + 1; i++)
            {
                if (t[i] == searchBytes[0])
                {

                    if (searchBytes.Length == 1) { return i; }
                    bool flag = true;
                    for (int j = 1; j < searchBytes.Length; j++)
                    {
                        if (t[i + j] != searchBytes[j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) { return i; }
                }
            }
            return -1;
        }

        /// <summary>
        /// 第一个匹配项的索引
        /// </summary>
        /// <param name="t">被执行查找的Byte[]</param>
        /// <param name="searchString">要查找的字符串</param>
        /// <returns></returns>
        public static int indexOf(this byte[] t, string searchString)
        {
            byte[] b = UnInit.getEncoding().GetBytes(searchString);
            return indexOf(t, b);
        }

        /// <summary>
        /// 从开始到指定位置之间的索引
        /// </summary>
        /// <param name="t"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] subByteToIndex(this byte[] t, int i)
        {
            byte[] data = new byte[i];
            Buffer.BlockCopy(t, 0, data, 0, i);
            return data;
        }

        /// <summary>
        /// 从开始到第一次匹配位置之间的索引
        /// </summary>
        /// <param name="t"></param>
        /// <param name="searchBytes"></param>
        /// <returns></returns>
        public static byte[] subByteToIndex(this byte[] t, byte[] searchBytes)
        {
            int i = t.indexOf(searchBytes);
            if (i < 0)
            {
                return null;
            }
            return subByteToIndex(t, i);
        }

        /// <summary>
        /// 从开始到第一次匹配位置之间的索引
        /// </summary>
        /// <param name="t"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static byte[] subByteToIndex(this byte[] t, string searchString)
        {
            return subByteToIndex(t, UnInit.getEncoding().GetBytes(searchString));
        }

        /// <summary>
        /// 从指定位置到末尾的索引
        /// </summary>
        /// <param name="t"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] subByteFromIndex(this byte[] t, int i)
        {
            int count = t.Length - i;
            try
            {
                byte[] data = new byte[count];
                Buffer.BlockCopy(t, i, data, 0, count);
                return data;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从第一次匹配位置到末尾的索引
        /// </summary>
        /// <param name="t">被执行查找的Byte[]</param>
        /// <param name="searchBytes">要查找的匹配项</param>
        /// <returns></returns>
        public static byte[] subByteFromIndex(this byte[] t, byte[] searchBytes)
        {
            int i = t.indexOf(searchBytes);
            if (i < 0)
            {
                return null;
            }
            int count = t.Length - i - searchBytes.Length;
            try
            {
                byte[] data = new byte[count];
                Buffer.BlockCopy(t, i + searchBytes.Length, data, 0, count);
                return data;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从第一次匹配位置到末尾的索引
        /// </summary>
        /// <param name="t"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static byte[] subByteFromIndex(this byte[] t, string searchString)
        {
            return subByteFromIndex(t, UnInit.getEncoding().GetBytes(searchString));
        }

        /// <summary>
        /// 指定位置之间的索引
        /// </summary>
        /// <param name="t"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static byte[] subByteBetween(this byte[] t, int begin, int end)
        {
            byte[] data = new byte[end - begin + 1];
            int n = 0;
            for (int i = 0; i < t.Length; i++)
            {
                if (i >= begin && i <= end)
                {
                    data[n] = t[i];
                    n++;
                }
            }
            return data;
        }

        /// <summary>
        /// 两个匹配项之间的索引
        /// </summary>
        /// <param name="t">被执行查找的Byte[]</param>
        /// <param name="beginBytes">第一个匹配项</param>
        /// <param name="endBytes">第二个匹配项</param>
        /// <returns></returns>
        public static byte[] subByteBetween(this byte[] t, byte[] beginBytes, byte[] endBytes)
        {
            int aint = t.indexOf(beginBytes);
            if (aint < 0)
            {
                return null;
            }
            byte[] abyte = t.subByteFromIndex(beginBytes);
            return subByteToIndex(abyte, endBytes);
        }

        /// <summary>
        /// 两个匹配项之间的索引
        /// </summary>
        /// <param name="t"></param>
        /// <param name="beginString"></param>
        /// <param name="endString"></param>
        /// <returns></returns>
        public static string subStringBetween(this byte[] t, string beginString, string endString)
        {
            byte[] beginBytes = UnInit.getEncoding().GetBytes(beginString);
            byte[] endBytes = UnInit.getEncoding().GetBytes(endString);
            byte[] data = subByteBetween(t, beginBytes, endBytes);
            if (data != null)
            {
                return UnInit.getEncoding().GetString(data);
            }
            return null;
        }

        /// <summary>
        /// md5值
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string md5Hash(this byte[] t) {
            return UnEncMD5.getMd5Hash(t);
        }
    }
}
