using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UnCommon.Tool;
using UnCommon.Extend;
using UnCommon.Config;

namespace UnCommon.Encrypt
{
    public class UnEncSign
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="input">答名内容</param>
        /// <param name="enu">签名事件</param>
        /// <returns>签名结果</returns>
        public static string sign(string input, UnEncSignEnu enu)
        {
            switch (enu)
            {
                case UnEncSignEnu.MD5:
                    input = input.md5Hash();
                    break;
                case UnEncSignEnu.SHA1:
                    input = input.sha1Hash(true);
                    break;
            }
            return input.ToLower();
        }

        /// <summary>
        /// 泛型转有序字典
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <returns></returns>
        private static SortedDictionary<string, string> tToSorted<T>(T t) where T : new()
        {
            // 排序
            SortedDictionary<string, object> sDic = UnToGen.tToSDic(t, null);
            // 排除空值
            SortedDictionary<string, string> dicArray = new SortedDictionary<string, string>();
            foreach (KeyValuePair<string, object> temp in sDic)
            {
                string key = temp.Key;
                object value = temp.Value;
                if (value != null && (string)value != "")
                {
                    dicArray.Add(key, (string)value);
                }
            }
            return dicArray;
        }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="sort">有序字典</param>
        /// <param name="fields">字段,多个用,分割,例:A,B,C</param>
        /// <param name="isr">包含/不包含</param>
        /// <returns></returns>
        public static string splitJoint(SortedDictionary<string, string> sort, string fields, bool isr)
        {
            StringBuilder prestr = new StringBuilder();
            string fdsin = "," + fields + ",";
            foreach (KeyValuePair<string, string> temp in sort)
            {
                if (fields != null)
                {
                    // 排除或者包含
                    if (isr && fdsin.IndexOf(temp.Key) < 0)
                    {
                        prestr.Append(temp.Key).Append("=").Append(temp.Value).Append("&");
                    }
                    else if (!isr && fdsin.IndexOf(temp.Key) >= 0)
                    {
                        prestr.Append(temp.Key).Append("=").Append(temp.Value).Append("&");
                    }
                }
                else
                {
                    prestr.Append(temp.Key).Append("=").Append(temp.Value).Append("&");
                }
            }
            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <param name="fields">字段,多个用,分割,例:A,B,C</param>
        /// <param name="isr">包含/不包含</param>
        /// <returns></returns>
        public static string splitJoint<T>(T t, string fields, bool isr) where T : new()
        {
            SortedDictionary<string, string> dicArray = tToSorted(t);
            return splitJoint(dicArray, fields, isr);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <param name="fields">字段,多个用,分割,例:A,B,C</param>
        /// <param name="signKey">key值</param>
        /// <param name="signEnu">签名类型</param>
        /// <returns>签名结果</returns>
        public static string sign<T>(T t, string fields, string signKey, UnEncSignEnu signEnu) where T : new()
        {
            string str = splitJoint(t, fields, false);
            if (signKey != null)
            {
                str += "&key=" + signKey;
            }
            return sign(str, signEnu);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <param name="signKey">签名密钥</param>
        /// <param name="signEnu">签名类型</param>
        /// <returns></returns>
        public static string sign<T>(T t, string signKey, UnEncSignEnu signEnu) where T : new()
        {
            string str = splitJoint(t, null, false);
            if (signKey != null)
            {
                str += "&key=" + signKey;
            }
            return sign(str, signEnu);
        }

        /// <summary>
        /// 签名验证
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">对象</param>
        /// <param name="fields">字段,多个用,分割,例:A,B,C</param>
        /// <param name="signKey">key值</param>
        /// <param name="signEnu">签名类型</param>
        /// <param name="signValue">待比较的签名</param>
        /// <returns>是否通过验证,验证之前需将对象签名属性设为null</returns>
        public static bool validSign<T>(T t, string fields, string signKey, UnEncSignEnu signEnu, string signValue) where T : new()
        {
            if (sign(t, fields, signKey, signEnu).Equals(signValue))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 签名验证
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <param name="signKey">签名密钥</param>
        /// <param name="signEnu">签名类型</param>
        /// <param name="signValue">原始签名</param>
        /// <returns>返回验证结果,验证之前需将对象签名属性设为null</returns>
        public static bool validSign<T>(T t, string signKey, UnEncSignEnu signEnu, string signValue) where T : new()
        {
            if (sign(t, null, signKey, signEnu).Equals(signValue))
            {
                return true;
            }
            return false;
        }

    }
}
