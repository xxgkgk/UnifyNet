using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using UnCommon.Files;

namespace UnCommon.Tool
{
    /// <summary>
    /// 泛型处理类
    /// </summary>
    public class UnToGen
    {
        /// <summary>
        /// 获取泛型属性组
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns></returns>
        public static List<string> getListField<T>()
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            List<string> list = new List<string>();
            if (length <= 0)
            {
                return list;
            }
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                if (isField(item))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取SQL字段组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> getSqlFields<T>()
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            List<string> list = new List<string>();
            if (length <= 0)
            {
                return list;
            }
            // 自动编号
            string autoName = getAutoNum<T>(false);
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                // 排除自动编号
                if (isField(item) && name != autoName)
                {
                    // 排除仅大小写不同的字段
                    bool isHave = false;
                    foreach (string p in list)
                    {
                        if (p.ToLower() == name.ToLower())
                        {
                            isHave = true;
                        }
                    }
                    if (!isHave)
                    {
                        list.Add(name);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获得首位自动编号名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string getAutoNum<T>(bool addPre)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            // 自动编号
            string autoName = null;
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                if (isField(item))
                {
                    // 默认自动编号为首位
                    autoName = name;
                    // 如果找到构造为 表名+ID 则就是自动编号
                    if (name == typeof(T).Name + "ID")
                    {
                        autoName = name;
                        break;
                    }
                }
            }

            // 是否需要含表前缀
            if (addPre)
            {
                return typeof(T).Name + "." + autoName;
            }
            else
            {
                return autoName;
            }
        }

        /// <summary>
        /// 获取泛型属性组
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns></returns>
        public static string getFields<T>()
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            string strA = null;
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                if (isField(item))
                {
                    if (strA == null)
                    {
                        strA = name;
                    }
                    else
                    {
                        strA += "," + name;
                    }
                }
                else
                {
                    Console.WriteLine(name+"***");

                    
                }
            }
            return strA;
        }

 
        /// <summary>
        /// 取出所有属性(包括继承)
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns></returns>
        public static string getFields(Type t)
        {
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            string strA = null;
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                if (isField(item))
                {
                    if (strA == null)
                    {
                        strA = name;
                    }
                    else
                    {
                        strA += "," + name;
                    }
                }
                else if (isClass(item))
                {
                    strA += "," + getFields(item.PropertyType);
                }
            }
            return strA;
        }

        /// <summary>
        /// 是否字段
        /// </summary>
        /// <param name="item">数据元</param>
        /// <returns></returns>
        public static bool isField(PropertyInfo item)
        {
            //Console.WriteLine(item.PropertyType.IsValueType+"/" + item.PropertyType.Name);
            //是值,字符串,字节
            if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String") || item.PropertyType.Name.StartsWith("Byte"))
            {
                //Console.WriteLine("属性:" + item.Name);
                if (item.CanRead && item.CanWrite)
                {
                    //Console.WriteLine("属性:" + item.Name);
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 是否类
        /// </summary>
        /// <param name="item">数据元</param>
        /// <returns></returns>
        public static bool isClass(PropertyInfo item)
        {
            //是类,非字符串,非泛型
            if (item.PropertyType.IsClass)
            {
                if (item.CanRead && item.CanWrite)
                {
                    //Console.WriteLine("类：" + item.Name);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 强制转换数据类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static object convertTo(Type type, object source)
        {
            if (type.Equals(typeof(String)))
            {
                source = Convert.ToString(source);
            }
            else if (type.Equals(typeof(Int32)))
            {
                source = Convert.ToInt32(source);
            }
            else if (type.Equals(typeof(Int64)))
            {
                source = Convert.ToInt64(source);
            }
            else if (type.Equals(typeof(Boolean)))
            {
                source = Convert.ToBoolean(source);
            }
            else if (type.Equals(typeof(DateTime)))
            {
                if (source == null)
                {
                    source = DateTime.Now;
                }
                else
                {
                    source = Convert.ToDateTime(source);
                }
            }
            else if (type.Equals(typeof(Decimal)))
            {
                source = Convert.ToDecimal(source);
            }
            else if (type.Equals(typeof(Guid)))
            {
                source = new Guid(source.ToString());
            }
            else if (type.Equals(typeof(Byte)))
            {
                source = Convert.ToByte(source);
            }
            else if (type.Equals(typeof(Byte[])))
            {
                if (source != null)
                {
                    source = (Byte[])source;
                }
            }
            else
            {
            }
            return source;
        }

        /// <summary>
        /// 获取属性组
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <returns></returns>
        public static string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1},", name, value);
                }
                else
                {
                    getProperties(value);
                }
            } return tStr;
        }

        /// <summary>
        /// DataRow转全泛型(核心方法)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dr">数据列</param>
        /// <returns></returns>
        public static T drToT<T>(DataRow dr) where T : new()
        {
            T t = new T();
            PropertyInfo[] pis = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = pis.Length;
            if (length <= 0)
            {
                return t;
            }

            for (int i = 0; i < length; i++)
            {
                PropertyInfo pi = pis[i];
                string strName = pi.Name;
                if (UnToGen.isField(pi))
                {
                    // 如果是字段
                    try
                    {
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            if (dc.ColumnName.Equals(strName))
                            {
                                t.GetType().GetProperty(strName).SetValue(t, UnToGen.convertTo(pi.PropertyType, dr[strName]), null);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        UnFile.writeLog("drToT", e.ToString());
                    }
                }
                else if (UnToGen.isClass(pi))
                {
                    // 如果是类型
                }
            }
            return t;
        }

        /// <summary>
        /// DataTable转全泛型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static List<T> dtToT<T>(DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(drToT<T>(dr));
            }
            return list;
        }

        /// <summary>
        /// 字典排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static SortedDictionary<string, object> tToSDic<T>(T t, string fields)
        {
            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
            Type tp = typeof(T);
            PropertyInfo[] prts = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < prts.Length; i++)
            {
                PropertyInfo item = prts[i];
                if (isField(item))
                {
                    bool isb = true;
                    if (fields != null && ("," + fields + ",").IndexOf("," + item.Name + ",") < 0)
                    {
                        isb = false;
                    }
                    if (isb)
                    {
                        try
                        {
                            string strValue = item.GetValue(t, null).ToString();
                            dic.Add(item.Name, strValue);
                        }
                        catch{}
                    }
                }
                else if (isClass(item))
                {
                }
            }
            return dic;
        }

    }
}
