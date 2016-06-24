using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Tool;
using System.Web;
using UnCommon.Extend;

namespace UnWeiXin
{
    public class UnSign
    {

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="sDic">过滤前的参数组</param>
        /// <param name="excludes">排除的key</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> filterPara(SortedDictionary<string, object> sDic, string excludes)
        {
            excludes = "," + excludes + ",";
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> temp in sDic)
            {
                string key = temp.Key.ToLower();
                object value = temp.Value;
                if (excludes.IndexOf("," + key + ",") < 0 && value != null && (string)value != "")
                {
                    dicArray.Add(key, (string)value);
                }
            }
            return dicArray;
        }

        /// <summary>
        /// 除去泛型中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">对象</param>
        /// <param name="t">排除的key</param>
        /// <returns></returns>
        public static Dictionary<string, string> filterPara<T>(T t, string excludes) where T : new()
        {
            return filterPara(UnToGen.tToSDic(t, null), excludes);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="t"></param>
        /// <param name="excludes"></param>
        /// <returns></returns>
        public static string Sign<T>(T t, string excludes) where T : new()
        {
            string s = createLinkString(filterPara(UnToGen.tToSDic(t, null), excludes));
            byte[] b = Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 签名（对参数值做urlencode）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="excludes"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string SignUrlencode<T>(T t, string excludes, Encoding code) where T : new()
        {
            return createLinkStringUrlencode(filterPara(UnToGen.tToSDic(t, null), excludes), code).md5Hash().ToUpper();
        }

        /// <summary>
        /// 除去泛型中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public static Dictionary<string, string> filterPara<T>(T t) where T : new()
        {
            return filterPara(UnToGen.tToSDic(t, null), null);
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string createLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string createLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(@temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }
    }
}
