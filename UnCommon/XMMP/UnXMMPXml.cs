using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using UnCommon;
using UnCommon.Tool;

namespace UnCommon.XMMP
{
    /// <summary>
    /// xml解析类
    /// </summary>
    public class UnXMMPXml
    {

        // 反序列化
        public static object xmlToT(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch
            {
                return null;
            }
        }

        // 反序列化
        public static object streamToT(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }

        // 序列化
        public static string tToXml(Type type, object obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                // Remove xmlns
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlWriterSettings settings = new XmlWriterSettings();
                // Remove tversion
                settings.OmitXmlDeclaration = true;
                XmlWriter writer = XmlWriter.Create(stream, settings);
                xml.Serialize(writer, obj, ns);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            stream.Dispose();

            // 去掉 nil
            Regex reg = new Regex(@"<[^<]*\sp\d:[^>]*>");
            str = reg.Replace(str, "");

            return str;
        }

        // 无声明序列化
        public static string tToXmlNoSm(Type type, object obj)
        {
            string xml = tToXml(type, obj);
            xml = xml.Replace(@"<?xml version=""1.0""?>", "");
            xml = xml.Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", "");
            xml = xml.Replace(@" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "");
            xml = xml.Trim();
            return xml;
        }

        // 对象转xml
        public static StringBuilder tToXml<T>(string rootName, string tagName, List<T> listT, string[] fields)
        {
            StringBuilder sb = new StringBuilder();
            Type tp = typeof(T);
            if (rootName != null)
            {
                sb.AppendFormat(@"<" + rootName + ">");
            }
            if (fields == null)
            {
                fields = UnToGen.getFields<T>().Split(',');
            }
            foreach (T _T in listT)
            {
                if (tagName != null)
                {
                    sb.AppendFormat(@"<" + tagName + ">");
                }
                foreach (string Filed in fields)
                {
                    try
                    {
                        string strValue = tp.GetProperty(Filed).GetValue(_T, null).ToString();
                        strValue = "<![CDATA[" + strValue + "]]>";
                        sb.AppendFormat(@"<{0}>{1}</{0}>", Filed, strValue);
                    }
                    catch
                    {
                    }
                }
                if (tagName != null)
                {
                    sb.AppendFormat(@"</" + tagName + ">");
                }
            }
            if (rootName != null)
            {
                sb.AppendFormat(@"</" + rootName + ">");
            }
            return sb;
        }

        // 对象转xml
        public static StringBuilder tToXml<T>(string rootName, string tagName, List<T> listT, string fields)
        {
            string[] fs = null;
            if (fields != null)
            {
                fs = fields.Split(',');
            }
            return tToXml<T>(rootName, tagName, listT, fs);
        }

        // 对象转xml
        public static StringBuilder tToXml<T>(string rootName, string tagName, T t, string fields)
        {
            List<T> listT = new List<T>();
            listT.Add(t);
            return tToXml<T>(rootName, tagName, listT, fields);
        }

        // 对象转XmlDocument
        public static XmlDocument tToXmlDoc<T>(List<T> list, string[] fields) where T : new()
        {
            XmlDocument xd = new XmlDocument();
            Type tp = new T().GetType();
            string TName = tp.Name;

            // 添加头
            XmlDeclaration Xd = xd.CreateXmlDeclaration("1.0", "utf-8", null);
            xd.AppendChild(Xd);
            // 添加根元素
            XmlElement _Xe0 = xd.CreateElement("xml");
            xd.AppendChild(_Xe0);

            foreach (T t in list)
            {
                // 添加类元素
                XmlElement _Xe1 = xd.CreateElement(tp.Name);
                _Xe0.AppendChild(_Xe1);
                foreach (string Filed in fields)
                {
                    // 添加字段元素
                    XmlElement _Xe2 = xd.CreateElement(Filed.Replace("_", ""));
                    _Xe1.AppendChild(_Xe2);
                    // 添加字段值
                    try
                    {
                        string strValue = tp.GetProperty(Filed).GetValue(t, null).ToString();
                        _Xe2.InnerText = strValue;
                    }
                    catch
                    {
                    }
                }
            }
            return xd;
        }

        // 对象转XmlDocument
        public static XmlDocument tToXmlDoc<T>(List<T> list) where T : new()
        {
            return tToXmlDoc(list, UnToGen.getFields<T>().Split(','));
        }

        // xml转DataTable
        public static DataTable xmlToDT(XmlDocument xd, string xPath, string tableName)
        {
            // 根据第一个元素结构建立表结构
            DataTable _DataTable = new DataTable(tableName);
            XmlNode _XmlNode = xd.SelectSingleNode(xPath);
            string colName;
            if (_XmlNode != null)
            {
                for (int i = 0; i < _XmlNode.ChildNodes.Count; i++)
                {
                    colName = _XmlNode.ChildNodes.Item(i).Name;
                    _DataTable.Columns.Add(colName);
                }
            }

            // 载入数据
            string xmlString = xd.InnerXml;
            StringReader _StringReader = new StringReader(xmlString);
            XmlTextReader _XmlTextReader = new XmlTextReader(_StringReader);
            _DataTable.ReadXml(_XmlTextReader);

            return _DataTable;
        }

        // xml转对象
        public static List<T> xmlToT<T>(XmlDocument xd,string xpath, string tableName) where T : new()
        {
            List<T> ListModel = new List<T>();
            DataTable dt = new DataTable();
            dt = xmlToDT(xd, xpath, tableName);
            ListModel = UnToGen.dtToT<T>(dt);
            return ListModel;
        }

        // xml转对象
        public static List<T> xmlToT<T>(XmlDocument xd) where T : new()
        {
            string tableName = new T().GetType().Name;
            string xpath = "/xml/" + tableName + "[1]";
            return xmlToT<T>(xd, xpath, tableName);
        }

        // 过滤非打印字符
        public static string replaceASCII(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss);
                else info.Append(cc);
            }
            return info.ToString();
        }



    }
}