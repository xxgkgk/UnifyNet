using System;
using System.Security.Cryptography;
using UnCommon.Config;

namespace UnCommon.Tool
{
    /// <summary>
    /// 随机数
    /// </summary>
    public class UnStrRan
    {
        /// <summary>
        /// 强随机数
        /// </summary>
        /// <param name="numSeeds">最大数</param>
        /// <param name="length">递增值</param>
        /// <returns></returns>
        public static int next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static string getRandom()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + UnInit.pid();
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="min">最小长度</param>
        /// <param name="max">最大长度</param>
        /// <returns></returns>
        public static string getStr(int min, int max)
        {
            //字符列表 
            string[] s1 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            //实例化rand
            Random rand = new Random();
            string str = "";
            int rnum = rand.Next(min, max);
            for (int i = 0; i < rnum; i++)
            {
                int rnum1 = rand.Next(0, s1.Length - 1);
                str += s1[rnum1];
            }
            return str;
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int getInt(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min, max);
        }

        public static string getUID()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + UnInit.pid();
        }

    }
}
