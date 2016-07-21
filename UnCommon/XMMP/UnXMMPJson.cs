using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnCommon.Tool;

namespace UnCommon.XMMP
{
    /// <summary>
    /// Json解析类
    /// </summary>
    public class UnXMMPJson
    {

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="json">json串</param>
        /// <returns></returns>
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

        /// <summary>
        /// 对象转Json
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string tToJson(Type type, object obj)
        {
            StringWriter sw = new StringWriter();
            JsonSerializer json = new JsonSerializer();
            try
            {
                // null值不序列化
                json.NullValueHandling = NullValueHandling.Ignore;
                //序列化对象
                json.Serialize(sw, obj, type);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            return sw.ToString();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootName"></param>
        /// <param name="listT"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootName"></param>
        /// <param name="listT"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static StringBuilder tToJson<T>(string rootName, List<T> listT, string fields) where T : new()
        {
            string[] fs = null;
            if (fields != null)
            {
                fs = fields.Split(',');
            }
            return tToJson<T>(rootName, listT, fs);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootName"></param>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static StringBuilder tToJson<T>(string rootName, T t, string fields) where T : new()
        {
            List<T> listT = new List<T>();
            listT.Add(t);
            return tToJson<T>(rootName, listT, fields);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public static Dictionary<string, object> jsonToDicSingle(string strJson, string rootName)
        {
            List<Dictionary<string, object>> list = jsonToDic(strJson, rootName);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// 格式化重新排序
        /// </summary>
        /// <param name="jobj">原始JSON JToken.Parse(string json);</param>
        /// <param name="obj">初始值Null</param>
        /// <returns></returns>
        public static JToken sortJson(JToken jobj, JToken obj)
        {
            if (obj == null)
            {
                obj = new JObject();
            }
            List<JToken> list = jobj.ToList<JToken>();
            if (jobj.Type == JTokenType.Object)//非数组
            {
                List<string> listsort = new List<string>();
                foreach (var item in list)
                {
                    string name = JProperty.Load(item.CreateReader()).Name;
                    listsort.Add(name);
                }
                listsort.Sort();
                List<JToken> listTemp = new List<JToken>();
                foreach (var item in listsort)
                {
                    listTemp.Add(list.Where(p => JProperty.Load(p.CreateReader()).Name == item).FirstOrDefault());
                }
                list = listTemp;
                //list.Sort((p1, p2) => JProperty.Load(p1.CreateReader()).Name.GetAnsi() - JProperty.Load(p2.CreateReader()).Name.GetAnsi());

                foreach (var item in list)
                {
                    JProperty jp = JProperty.Load(item.CreateReader());
                    if (item.First.Type == JTokenType.Object)
                    {
                        JObject sub = new JObject();
                        (obj as JObject).Add(jp.Name, sub);
                        sortJson(item.First, sub);
                    }
                    else if (item.First.Type == JTokenType.Array)
                    {
                        JArray arr = new JArray();
                        if (obj.Type == JTokenType.Object)
                        {
                            (obj as JObject).Add(jp.Name, arr);
                        }
                        else if (obj.Type == JTokenType.Array)
                        {
                            (obj as JArray).Add(arr);
                        }
                        sortJson(item.First, arr);
                    }
                    else if (item.First.Type != JTokenType.Object && item.First.Type != JTokenType.Array)
                    {
                        (obj as JObject).Add(jp.Name, item.First);
                    }
                }
            }
            else if (jobj.Type == JTokenType.Array)//数组
            {
                foreach (var item in list)
                {
                    List<JToken> listToken = item.ToList<JToken>();
                    List<string> listsort = new List<string>();
                    foreach (var im in listToken)
                    {
                        if (im.Type == JTokenType.Object)
                        {

                            JObject sub = new JObject();
                            (obj as JArray).Add(sub);
                            sortJson(im, sub);
                        }
                        else
                        {
                            string name = JProperty.Load(im.CreateReader()).Name;
                            listsort.Add(name);
                        }
                    }
                    listsort.Sort();
                    List<JToken> listTemp = new List<JToken>();
                    foreach (var im2 in listsort)
                    {
                        listTemp.Add(listToken.Where(p => JProperty.Load(p.CreateReader()).Name == im2).FirstOrDefault());
                    }
                    list = listTemp;

                    listToken = list;
                    // listToken.Sort((p1, p2) => JProperty.Load(p1.CreateReader()).Name.GetAnsi() - JProperty.Load(p2.CreateReader()).Name.GetAnsi());
                    JObject item_obj = new JObject();
                    foreach (var token in listToken)
                    {
                        JProperty jp = JProperty.Load(token.CreateReader());
                        if (token.First.Type == JTokenType.Object)
                        {
                            JObject sub = new JObject();
                            (obj as JObject).Add(jp.Name, sub);
                            sortJson(token.First, sub);
                        }
                        else if (token.First.Type == JTokenType.Array)
                        {
                            JArray arr = new JArray();
                            if (obj.Type == JTokenType.Object)
                            {
                                (obj as JObject).Add(jp.Name, arr);
                            }
                            else if (obj.Type == JTokenType.Array)
                            {
                                (obj as JArray).Add(arr);
                            }
                            sortJson(token.First, arr);
                        }
                        else if (item.First.Type != JTokenType.Object && item.First.Type != JTokenType.Array)
                        {
                            if (obj.Type == JTokenType.Object)
                            {
                                (obj as JObject).Add(jp.Name, token.First);
                            }
                            else if (obj.Type == JTokenType.Array)
                            {
                                item_obj.Add(jp.Name, token.First);
                            }
                        }
                    }
                    if (obj.Type == JTokenType.Array)
                    {
                        (obj as JArray).Add(item_obj);
                    }

                }
            }

            //string ret = obj.ToString(Formatting.None);
            return obj;
        }

        /// <summary>
        /// JSON排序
        /// </summary>
        /// <param name="jobj"></param>
        /// <returns></returns>
        public static JToken sortJson(JToken jobj)
        {
            List<JToken> list = jobj.ToList<JToken>();
            if (jobj.Type == JTokenType.Object)//非数组
            {
                
            }
            else if (jobj.Type == JTokenType.Array)//数组
            {
               
            }

            //string ret = obj.ToString(Formatting.None);
            return null;
        }

        /// <summary>
        /// 格式化重新排序
        /// </summary>
        /// <param name="json">原始JSON</param>
        public static string sortJson(string json)
        {
            String str = sortJson(JToken.Parse(json), null).ToString(Formatting.None);
            return str;
        }
    }
}
