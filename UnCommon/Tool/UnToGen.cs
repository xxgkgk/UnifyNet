using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using UnCommon.Files;
using UnCommon.Entity;
using System.Configuration;
using UnCommon.Config;

namespace UnCommon.Tool
{
    /// <summary>
    /// 泛型处理类
    /// </summary>
    public class UnToGen
    {
        /// <summary>
        /// 获取特性组(核心)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<PropertyInfo> getListFieldPropertyInfo(Type t)
        {
            var properties  = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            List<PropertyInfo> list = new List<PropertyInfo>();
            if (length <= 0)
            {
                return list;
            }
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                if (isField(item))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取字段组核心)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<string> getListField(Type t)
        {
            var list = new List<string>();
            var pros = getListFieldPropertyInfo(t);
            foreach (var item in pros)
            {
                string name = UnToGen.getFieldName(item);
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
            return list;
        }

        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static UnAttrSql getAttrSql(Type t)
        {
            //取类上的自定义特性
            UnAttrSql classAttr = null;
            object[] objs = t.GetCustomAttributes(typeof(UnAttrSql), true);
            foreach (object obj in objs)
            {
                classAttr = obj as UnAttrSql;
            }
            return classAttr;
        }

        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <param name="item">特性</param>
        /// <returns></returns>
        public static UnAttrSql getAttrSql(PropertyInfo item)
        {
            UnAttrSql attr = null;
            object[] objAttrs = item.GetCustomAttributes(typeof(UnAttrSql), true);
            if (objAttrs.Length > 0)
            {
                attr = objAttrs[0] as UnAttrSql;
            }
            return attr;
        }

        /// <summary>
        /// 获取表名(核心)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <returns></returns>
        public static string getTableName(Type t, bool isLinkedServer)
        {
            UnAttrSql attr = getAttrSql(t);
            if (attr != null)
            {
                if (!isLinkedServer || attr.tableName != null)
                {
                    return attr.tableName;
                }
                if (isLinkedServer || attr.linkedServerTableName != null)
                {
                    return attr.linkedServerTableName;
                }
            }
            if (isLinkedServer)
            {
                string lsp = ConfigurationManager.AppSettings[UnKeyName.LinkedServerPrefix];
                if (!String.IsNullOrWhiteSpace(lsp))
                {
                    return lsp + t.Name;
                }
            }
            return t.Name;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string getTableName(Type t)
        {
            return getTableName(t, false);
        }

        /// <summary>
        /// 获取字段名(核心)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string getFieldName(PropertyInfo item)
        {
            UnAttrSql attr = getAttrSql(item);
            if (attr != null && attr.fieldName != null)
            {
                return attr.fieldName;
            }
            return item.Name;
        }

        /// <summary>
        /// 获取不含自增ID的字段组(核心)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> getFieldNoAutoInc<T>()
        {
            var fields = getListField(typeof(T));
            int length = fields.Count;
            List<string> list = new List<string>();
            if (length <= 0)
            {
                return list;
            }
            // 自动编号
            string autoName = getAutoNum(typeof(T), false);
            foreach (var name in fields)
            {
                // 排除自动编号
                if (name != autoName)
                {
                    list.Add(name);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取字段组
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns></returns>
        public static List<string> getListField<T>()
        {
            return getListField(typeof(T));
        }

        /// <summary>
        /// 获得首位自动编号名
        /// </summary>
        /// <param name="t"></param>
        /// <param name="addPre"></param>
        /// <returns></returns>
        public static string getAutoNum(Type t,bool addPre)
        {
            var properties = UnToGen.getListFieldPropertyInfo(t);
            int length = properties.Count;
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
                    if (name == getTableName(t) + "ID")
                    {
                        autoName = name;
                        break;
                    }
                }
            }

            // 是否需要含表前缀
            if (addPre)
            {
                return getTableName(t) + "." + autoName;
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
            var properties = UnToGen.getListFieldPropertyInfo(typeof(T));
            int length = properties.Count;
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
            //Console.WriteLine(item.Name + "/" + item.PropertyType.IsValueType + "/" + item.PropertyType.Name);
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
            if (source == null || source is DBNull)
            {
                return null;
            }
            // string
            if (type.Equals(typeof(String)))
            {
                return Convert.ToString(source);
            }
            // Int
            if (type.Equals(typeof(Int16)) || type.Equals(typeof(Nullable<Int16>)))
            {
                return Convert.ToInt16(source);
            }
            if (type.Equals(typeof(Int32)) || type.Equals(typeof(Nullable<Int32>)))
            {
                return Convert.ToInt32(source);
            }
            if (type.Equals(typeof(Int64)) || type.Equals(typeof(Nullable<Int64>)))
            {
                return Convert.ToInt64(source);
            }
            // UInt
            if (type.Equals(typeof(UInt16)) || type.Equals(typeof(Nullable<UInt16>)))
            {
                return Convert.ToUInt16(source);
            }
            if (type.Equals(typeof(UInt32)) || type.Equals(typeof(Nullable<UInt32>)))
            {
                return Convert.ToUInt32(source);
            }
            if (type.Equals(typeof(UInt64)) || type.Equals(typeof(Nullable<UInt64>)))
            {
                return Convert.ToUInt64(source);
            }
            // 小数
            if (type.Equals(typeof(float)) || type.Equals(typeof(Nullable<float>)))
            {
                return Convert.ToSingle(source);
            }
            if (type.Equals(typeof(Decimal)) || type.Equals(typeof(Nullable<Decimal>)))
            {
                return Convert.ToDecimal(source);
            }
            if (type.Equals(typeof(Double)) || type.Equals(typeof(Nullable<Double>)))
            {
                return Convert.ToDouble(source);
            }
            // 布尔值
            if (type.Equals(typeof(Boolean)) || type.Equals(typeof(Nullable<Boolean>)))
            {
                return Convert.ToBoolean(source);
            }
            // 时间
            if (type.Equals(typeof(DateTime)) || type.Equals(typeof(Nullable<DateTime>)))
            {
                return Convert.ToDateTime(source);
            }
            // GUID
            if (type.Equals(typeof(Guid)) || type.Equals(typeof(Nullable<Guid>)))
            {
                return new Guid(source.ToString());
            }
            // 字符字节
            if (type.Equals(typeof(Char)) || type.Equals(typeof(Nullable<Char>)))
            {
                return Convert.ToChar(source);
            }
            if (type.Equals(typeof(Byte)) || type.Equals(typeof(Nullable<Byte>)))
            {
                return Convert.ToByte(source);
            }
            if (type.Equals(typeof(Byte[])))
            {
                return (Byte[])source;
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
            var properties = UnToGen.getListFieldPropertyInfo(typeof(T));
            int length = properties.Count;
            if (length <= 0)
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
            var pis = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = pis.Length;
            if (length <= 0)
            {
                return t;
            }

            for (int i = 0; i < length; i++)
            {
                PropertyInfo pi = pis[i];
                string strName = getFieldName(pi);
                if (UnToGen.isField(pi))
                {
                    // 如果是字段
                    try
                    {
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            // 忽略大小写
                            if (dc.ColumnName.ToLower().Equals(strName.ToLower()))
                            {
                                t.GetType().GetProperty(strName).SetValue(t, UnToGen.convertTo(pi.PropertyType, dr[strName]), null);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        UnFile.writeLog("drToT", strName + "\r\n" + e.ToString());
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
