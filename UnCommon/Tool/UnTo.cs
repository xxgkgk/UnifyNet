using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using UnCommon.Config;

namespace UnCommon.Tool
{
    /// <summary>
    /// 类型转换类
    /// </summary>
    public class UnTo
    {
        // Stream转byte
        public static byte[] streamToByte(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        // Stream转string
        public static string streamToString(Stream i)
        {
            return UnInit.getEncoding().GetString(streamToByte(i));
        }

        // byte转int组
        public static int[] byteToInt(byte[] bytes)
        {
            int[] ints = new int[bytes.Length];
            int i = 0;
            foreach (byte b in bytes)
            {
                ints[i] = (b & 0xFF);
                i++;
            }
            return ints;
        }

        // byte转int字符串组
        public static string byteToIntStr(byte[] bytes)
        {
            string str = "";
            int[] ints = byteToInt(bytes);
            foreach (int b in ints)
            {
                str += b;
            }
            return str;
        }

        // 字符串转为UniCode码字符串  
        public static string stringToUnicode(string s)
        {
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format("//u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }

        // Unicode字符串转为正常字符串
        public static string unicodeToString(string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }

    }
}
