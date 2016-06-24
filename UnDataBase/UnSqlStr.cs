using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using UnCommon;
using System.Reflection;
using System.Text;
using UnCommon.Tool;

namespace UnDataBase
{
    /// <summary>
    /// SQL语句组装
    /// </summary>
    public class UnSqlStr
    {

        #region Add语句

        /// <summary>
        /// Add字段组合(FieldList)
        /// </summary>
        /// <param name="FieldList">字段名组</param>
        /// <returns></returns>
        internal static string getAddStr(string FieldList)
        {
            string AddStr = "";
            string AddStr1 = FieldList.Trim();
            if (AddStr1 != "")
            {
                string AddStr2 = Regex.Replace("," + AddStr1, ",", "@");
                AddStr = "(" + AddStr1 + ") Values " + "(" + AddStr2 + ")";
            }
            return AddStr;
        }

        /// <summary>
        /// Add字段组合(T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_T"></param>
        /// <returns></returns>
        internal static string getAddStr<T>(SqlParameter[] pars) where T : new()
        {
            string AddStr = "";
            string AddStr1 = "";
            string AddStr2 = "";
            foreach (SqlParameter par in pars)
            {
                AddStr1 += "[" + par.ParameterName + "],";
                AddStr2 += "@" + par.ParameterName + ",";
            }
            AddStr = "(" + AddStr1.TrimEnd(',') + ") Values " + "(" + AddStr2.TrimEnd(',') + ")";
            return AddStr;
        }

        #endregion

        #region Update语句
        /// <summary>
        /// Upd字段组合(FieldList)
        /// </summary>
        /// <param name="FieldList">字段名组</param>
        /// /// <param name="KeyID">自动编号名</param>
        /// <returns></returns>
        internal static string getUpdStr(SqlParameter[] Pars)
        {
            string UpdStr = null;
            foreach (SqlParameter par in Pars)
            {
                UpdStr += "[" + par.ParameterName + "]=@" + par.ParameterName + ",";
            }
            return UpdStr.TrimEnd(',');
        }

        /// <summary>
        /// Upd字段组合(T)
        /// </summary>
        /// <typeparam name="T">实体形参</typeparam>
        /// <param name="_T">实体实例</param>
        /// <returns></returns>
        internal static string getUpdStr<T>() where T : new()
        {

            List<string> _List = UnToGen.getSqlFields<T>();
            string UpdStr = "";
            for (int i = 0; i < _List.Count; i++)
            {
                string strName = Regex.Replace(_List[i] + "", "^_", "");
                if (strName != "")
                {
                    if (UpdStr == "")
                    {
                        UpdStr = strName + "=@" + strName;

                    }
                    else
                    {
                        UpdStr += "," + strName + "=@" + strName;
                    }
                }
            }
            return UpdStr;
        }

        #endregion

        #region Query语句
        /// <summary>
        /// 组合sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static string getQuerySql<T>(string[] columns,
            string selection, string[] selectionArgs, string groupBy,
            string having, string orderBy)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select ");
            // 字段
            if (columns != null && columns.Length > 0)
            {
                int i = 0;
                foreach (string c in columns)
                {
                    if (i == 0)
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append("," + c);
                    }
                    i++;
                }
            }
            else
            {
                sb.Append("* ");
            }
            sb.Append(" From " + typeof(T).Name + " ");
            sb.Append(getSelectionSql<T>(selection, selectionArgs));
            if (groupBy != null && groupBy.Length > 0)
            {
                sb.Append(" " + groupBy);
            }
            if (having != null && having.Length > 0)
            {
                sb.Append(" " + having);
            }
            if (orderBy != null && orderBy.Length > 0)
            {
                sb.Append(" " + orderBy);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 翻页条件语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="where">条件</param>
        /// <param name="whereArgs">条件参数</param>
        /// <returns>返回组合的翻页条件语句</returns>
        public static string getQueryPageWhere<T>(string where, string[] whereArgs)
        {
            StringBuilder sb = new StringBuilder();
            if (where != null && where.Length > 0)
            {
                if (whereArgs != null)
                {
                    sb.AppendFormat(where, whereArgs);
                }
                else
                {
                    sb.Append(where);
                }
            }
            sb.Append(" ");
            return sb.ToString();
        }

        #endregion

        #region 公用方法

        /// <summary>
        /// 获得SqlPmt数组(T所有字段)
        /// </summary>
        /// <typeparam name="T">实体形参</typeparam>
        /// <param name="_T">实例</param>
        /// <returns></returns>
        internal static SqlParameter[] getSqlPmtA<T>(T t) where T : new()
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            List<string> list = UnToGen.getSqlFields<T>();

            Type Typet = typeof(T);
            for (int i = 0; i < list.Count; i++)
            {
                string strName = list[i];
                PropertyInfo pro = Typet.GetProperty(strName);
                Type type = pro.GetType();
                object value = UnToGen.convertTo(type, pro.GetValue(t, null));
                if (value != null)
                {
                    pars.Add(new SqlParameter(strName, value));
                }
            }
            SqlParameter[] sp = new SqlParameter[pars.Count];
            pars.CopyTo(sp);
            return sp;
        }

        /// <summary>
        /// 获得SqlPmt数组(FieldList)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_T"></param>
        /// <param name="FieldList"></param>
        /// <returns></returns>
        internal static SqlParameter[] getSqlPmtA<T>(T t, string FieldList) where T : new()
        {
            List<string> _List = UnToGen.getSqlFields<T>();
            string[] FieldListA = FieldList.Split(new char[] { ',' });
            List<SqlParameter> pars = new List<SqlParameter>();
            Type Typet = typeof(T);
            for (int i = 0; i < _List.Count; i++)
            {
                string strName = _List[i];
                if (("," + FieldList + ",").IndexOf("," + strName + ",") >= 0)
                {
                    PropertyInfo pro = Typet.GetProperty(strName);
                    Type type = pro.GetType();
                    object value = UnToGen.convertTo(type, pro.GetValue(t, null));
                    if (value != null)
                    {
                        pars.Add(new SqlParameter(strName, value));
                    }
                }
            }
            SqlParameter[] sp = new SqlParameter[pars.Count];
            pars.CopyTo(sp);
            return sp;
        }

        /// <summary>
        /// 条件语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static string getSelectionSql<T>(string selection, string[] selectionArgs)
        {
            StringBuilder sb = new StringBuilder();
            if (selection != null)
            {
                sb.Append("Where ");
                if (selectionArgs != null)
                {
                    sb.AppendFormat(selection, selectionArgs);
                }
                else
                {
                    sb.Append(selection);
                }
            }
            return sb.ToString();
        }

        public static string getSelectionSql<T>(string selection, string selectionArgs)
        {
            string[] sls = null;
            if (selectionArgs != null)
            {
                sls = selectionArgs.Split(',');
            }
            return getSelectionSql<T>(selection, sls);
        }

        #endregion

    }
}
