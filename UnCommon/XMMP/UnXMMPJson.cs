using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using UnCommon.Tool;

namespace UnCommon.XMMP
{
    /// <summary>
    /// Json解析类
    /// </summary>
    public class UnXMMPJson
    {
  
        // json转对象
        public static object jsonToT(Type type, string json)
        {
            try
            {
                using (StringReader reader = new StringReader(json))
                {
                    JsonSerializer jsondes = new JsonSerializer();
                    return jsondes.Deserialize(reader, type);
                }
            }
            catch
            {
                return null;
            }
        }

        // 对象转json
        public static string tToJson(Type type, object obj)
        {
            StringWriter sw = new StringWriter();
            JsonSerializer json = new JsonSerializer();
            try
            {
                // null值不序列化
                json.NullValueHandling = NullValueHandling.Ignore;
                //序列化对象
                json.Serialize(sw, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            return sw.ToString();
        }

        // 对象转json
        private static StringBuilder tToJson<T>(string rootName, List<T> listT, string[] fields) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            Type _Type = new T().GetType();
            if (rootName != null)
            {
                sb.Append(@"{""" + rootName + "\":[");
            }
            if (fields == null)
            {
                fields = UnToGen.getFields<T>().Split(',');
            }
            StringBuilder sb0 = new StringBuilder();
            foreach (T _Model in listT)
            {
                sb0.Append(@"{");
                StringBuilder sb1 = new StringBuilder();
                foreach (string Filed in fields)
                {
                    try
                    {
                        string strValue = _Type.GetProperty(Filed).GetValue(_Model, null).ToString();
                        sb1.AppendFormat(@"""{0}"":""{1}"",", Filed.Replace("_", ""), strValue);
                    }
                    catch
                    {
                    }
                }
                sb0.Append(sb1.ToString().TrimEnd(','));
                sb0.Append(@"},");
            }
            sb.Append(sb0.ToString().TrimEnd(','));
            if (rootName != null)
            {
                sb.Append(@"]}");
            }
            return sb;
        }

        // 对象转json
        public static StringBuilder tToJson<T>(string rootName, List<T> listT, string fields) where T : new()
        {
            string[] fs = null;
            if (fields != null)
            {
                fs = fields.Split(',');
            }
            return tToJson<T>(rootName, listT, fs);
        }

        // 对象转json
        public static StringBuilder tToJson<T>(string rootName, T t, string fields) where T : new()
        {
            List<T> listT = new List<T>();
            listT.Add(t);
            return tToJson<T>(rootName, listT, fields);
        }

        // json转dataTable
        public static DataTable jsonToDT(string strJson, string rootName)
        {
            //加上头文件
            if (rootName != null)
            {
                strJson = @"{" + rootName + ":[" + strJson + "]}";
            }
            JsonReader jr = new JsonTextReader(new StringReader(strJson));
            DataTable dt = new DataTable();
            int SO = 0;
            int SA = 0;
            int HS = 0;
            while (jr.Read())
            {
                if (jr.TokenType == JsonToken.StartObject)
                {
                    SO++;
                }
                else if (jr.TokenType == JsonToken.StartArray)
                {
                    SA++;
                }
                else if (jr.TokenType == JsonToken.EndObject)
                {

                }
                else if (jr.TokenType == JsonToken.EndArray)
                {

                }
                else
                {
                    string PrName = jr.Value.ToString();
                    if (SO == 1)
                    {
                        dt.TableName = PrName;
                    }
                    else
                    {
                        //如果不存在列则添加
                        if (!dt.Columns.Contains(PrName))
                        {
                            DataColumn cl = new DataColumn(PrName);
                            dt.Columns.Add(cl);
                        }
                        jr.Read();
                        string JRV = "";
                        if (jr.Value != null)
                        {
                            JRV = jr.Value.ToString();
                        }
                        //如果是新行
                        if (SO - 1 > HS)
                        {
                            DataRow dr = dt.NewRow();
                            dr[PrName] = JRV;
                            dt.Rows.Add(dr);
                            HS++;
                        }
                        else
                        {
                            dt.Rows[HS - 1][PrName] = JRV;
                        }
                    }
                }
            }
            return dt;
        }

        // json转字典
        public static List<Dictionary<string, object>> jsonToDic(string strJson, string rootName)
        {
            //加上头文件
            if (rootName != null)
            {
                strJson = @"{" + rootName + ":[" + strJson + "]}";
            }
            JsonReader jr = new JsonTextReader(new StringReader(strJson));
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            int SO = 0;
            int SA = 0;
            int HS = 0;
            while (jr.Read())
            {
                if (jr.TokenType == JsonToken.StartObject)
                {
                    SO++;
                }
                else if (jr.TokenType == JsonToken.StartArray)
                {
                    SA++;
                }
                else if (jr.TokenType == JsonToken.EndObject)
                {

                }
                else if (jr.TokenType == JsonToken.EndArray)
                {

                }
                else
                {
                    string PrName = jr.Value.ToString();
                    if (SO == 1)
                    {
                    }
                    else
                    {
                        jr.Read();
                        string JRV = jr.Value.ToString();
                        //如果是新行
                        if (SO - 1 > HS)
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic[PrName] = JRV;
                            list.Add(dic);
                            HS++;
                        }
                        else
                        {
                            list[HS - 1][PrName] = JRV;
                        }
                    }
                }
            }
            return list;
        }

        // json转字典(单条)
        public static Dictionary<string, object> jsonToDicSingle(string strJson, string rootName)
        {
            List<Dictionary<string, object>> list = jsonToDic(strJson, rootName);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

    }
}
