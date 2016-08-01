using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Extend;
using System.Reflection;
using UnCommon.XMMP;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;

namespace UnCommon.Tool
{
    /// <summary>
    /// 签名工具
    /// </summary>
    public class UnSign
    {
        /// <summary>
        /// 签名枚举
        /// </summary>
        public enum UnSingType
        {
            /// <summary>
            /// MD5
            /// </summary>
            MD5,
            /// <summary>
            /// SHA1
            /// </summary>
            SHA1
        }

        /// <summary>
        /// 类型
        /// </summary>
        private UnSingType _type = UnSingType.MD5;

        /// <summary>
        /// 密钥
        /// </summary>
        private string _key = "";

        /// <summary>
        /// 标记
        /// </summary>
        private string _tag = "$";

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="type">类型</param>
        public UnSign(string key, UnSingType type)
        {
            _type = type;
            _key = key;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="key">密钥</param>
        public UnSign(string key)
        {
            _key = key;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="str">签名字符串</param>
        /// <returns>返回签名结果</returns>
        public string sign(string str)
        {
            // 拼接上密钥
            str = str.ToLower();
            str = str + "&key=" + _key;
            switch (_type)
            {
                case UnSingType.MD5:
                    return str.md5Hash();
                case UnSingType.SHA1:
                    return str.sha1Hash(true);
            }
            return String.Empty;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="sort">字典</param>
        /// <returns>返回签名结果</returns>
        public string sign(SortedDictionary<string, string> sort)
        {
            return sign(getSignString(sort));
        }

        /// <summary>
        /// 签名(仅签名字段类型,泛型集合、数组等不能排序的类型)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <returns>返回签名结果</returns>
        public string sign<T>(T t)
        {
            return sign(getSignDictionary(t));
        }

        /// <summary>
        /// 字符串添加签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string addSign(string str)
        {
            return sign(str) + _tag + str;
        }

        /// <summary>
        /// 移除签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string removeSign(string str)
        {
            int i = str.IndexOf(_tag);
            string str1 = str.Substring(i + 1);
            return str1;
        }

        /// <summary>
        /// 取出签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string getSign(string str)
        {
            int i = str.IndexOf(_tag);
            if (i < 0)
            {
                i = 0;
            }
            string sign0 = str.Substring(0, i);
            return sign0;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signStr">已经签名的字符串</param>
        /// <returns></returns>
        public bool validSign(string signStr)
        {
            // 源串
            string str1 = removeSign(signStr);
            // 原签名
            string sign0 = getSign(signStr);
            // 验证签名
            string sign1 = sign(str1);
            if (sign0 == sign1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 签名验证
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象(先将签名字段设为NULL)</param>
        /// <param name="sing">对比的签名值</param>
        /// <returns>返回验证结果</returns>
        public bool validSign<T>(T t, string sing) where T : new()
        {
            if (sign(t).Equals(sing))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取出所有签名排序的属性字典(包括继承）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <param name="treeName">根名</param>
        /// <returns></returns>
        public SortedDictionary<string, string> getSignDictionary<T>(T t, string treeName)
        {
            treeName += "" + t.GetType().Name;
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            SortedDictionary<string, string> sort = new SortedDictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = treeName + "" + item.Name;
                object value = item.GetValue(t, null);
                if (value != null)
                {
                    if (isSignField(item))
                    {
                        sort.Add(name, value.ToString());
                    }
                    else if (isSignClass(item))
                    {
                        var dics = getSignDictionary(value, treeName);
                        foreach (var dic in dics)
                        {
                            sort.Add(dic.Key, dic.Value);
                        }
                    }
                    else
                    {
                        // 不能直接签名的泛型集合或数组转为JSON格式进行签名
                        if (isSignArrayClass(item))
                        {
                           
                            Console.WriteLine(item.Name + "//");
                            int arrayNum = 0;
                            foreach (object o in (value as IEnumerable))
                            {
                                // 数组自循环加1
                                string arrayName = name + "" + arrayNum.ToString();
                                arrayNum++;
                                var dics1 = getSignDictionary(o, arrayName);
                                foreach (var dic in dics1)
                                {
                                    sort.Add(dic.Key, dic.Value);
                                }
                            }
                        }
                    }
                }
            }
            return sort;
        }

        /// <summary>
        /// 取出所有签名排序的属性字典(包括继承）
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <returns></returns>
        public SortedDictionary<string, string> getSignDictionary<T>(T t)
        {
            return getSignDictionary(t, null);
        }

        /// <summary>
        /// 获得签名字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <returns></returns>
        public string getSignString<T>(T t)
        {
            return getSignString(getSignDictionary(t));
        }

        /// <summary>
        /// 获得签名串
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string getSignString(SortedDictionary<string, string> sort)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sort)
            {
                prestr.Append(temp.Key).Append("=").Append(temp.Value).Append("&");
            }
            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }

        /// <summary>
        /// 是否签名字段
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool isSignField(PropertyInfo item)
        {
            //是值,字符串
            if (item.PropertyType.IsValueType || item.PropertyType.Name == "String")
            {
                if (item.CanRead && item.CanWrite)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否签名类
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool isSignClass(PropertyInfo item)
        {
            //Console.WriteLine(item.PropertyType.Name + ":" + item.PropertyType.I);
            //是类,非字符串,非泛型
            if (item.PropertyType.IsClass && !item.PropertyType.IsGenericType && !item.PropertyType.IsArray)
            {
                if (item.CanRead && item.CanWrite)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否签名数组
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool isSignArrayClass(PropertyInfo item)
        {
            if (item.Name.IndexOf("ArrayOf") == 0)
            {
                return true;
            }
            return false;
        }

    }
}
