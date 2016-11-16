using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnCommon.Entity;
using UnCommon.Files;

namespace UnDataBase
{
    /// <summary>
    /// SQL操作对象帮助类
    /// </summary>
    [Obsolete]
    public class UnSqlHelp
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="constr"></param>
        public UnSqlHelp(string constr)
        {
            this.cS = constr;
        }

        // 连接字符串
        private string cS = "";

        // 连接对象
        private SqlConnection conn = null;

        // 事务数组
        private List<SqlTransaction> tranNum = new List<SqlTransaction>();

        // 事务锁
        private object tranLock = new object();

        /// <summary>
        /// 获得连接对象
        /// </summary>
        /// <returns></returns>
        private SqlConnection getConn()
        {
            if (conn == null)
            {
                conn = new SqlConnection(cS);
            }
            switch (conn.State)
            {
                case ConnectionState.Closed:
                    conn.Open();
                    break;
                case ConnectionState.Broken:
                    conn.Dispose();
                    conn.Open();
                    break;
            }
            return conn;
        }

        // 错误日志
        private void writeLog(string pre, string e)
        {
            close();
            UnFile.writeLog(pre, e);
        }

        /// <summary>
        /// 设置连接池对象
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="sqlPmtA"></param>
        private void setcmd(SqlCommand cmd, SqlConnection cn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] sqlPmtA)
        {
            cmd.Connection = cn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (sqlPmtA != null)
            {
                foreach (SqlParameter parm in sqlPmtA)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        internal bool exSql(string cmdText)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand(cmdText, getConn()))
                {
                    Sqlcmd.ExecuteNonQuery();
                    close();
                    return true;
                }
            }
            catch (Exception e)
            {
                writeLog("ExSql", e.ToString() + "\r\n" + cmdText);
                return false;
            }
        }

        /// <summary>
        /// 执行带参数SQL
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="sqlPmtA"></param>
        /// <returns></returns>
        internal bool exSql(string cmdText, SqlParameter[] sqlPmtA)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.Text, cmdText, sqlPmtA);
                    Sqlcmd.ExecuteNonQuery();
                    close();
                    return true;
                }
            }
            catch (Exception e)
            {
                writeLog("ExSql", e.ToString() + "\r\n" + cmdText);
                return false;
            }
        }

        /// <summary>
        /// 执行带参数的储存过程
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="sqlPmtA"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        internal bool exSql(string cmdText, SqlParameter[] sqlPmtA, CommandType cmdType)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.StoredProcedure, cmdText, sqlPmtA);
                    Sqlcmd.ExecuteNonQuery();
                    close();
                    return true;
                }
            }
            catch (Exception e)
            {
                writeLog("ExSql", e.ToString() + "\r\n" + cmdText);
                return false;
            }
        }

        /// <summary>
        /// 获取DataReader
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        internal SqlDataReader getDataReader(string cmdText)
        {
            try
            {
                using (SqlConnection cn = getConn())
                {
                    using (SqlCommand Sqlcmd = new SqlCommand())
                    {
                        setcmd(Sqlcmd, cn, null, CommandType.Text, cmdText, null);
                        SqlDataReader _SqlDataReader = Sqlcmd.ExecuteReader(CommandBehavior.SingleResult);
                        return _SqlDataReader;
                    }
                }
            }
            catch (Exception e)
            {
                writeLog("GetDataReader", e.ToString() + "\r\n" + cmdText);
                return null;
            }
        }

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        internal DataSet getDataSet(string cmdText)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.Text, cmdText, null);
                    SqlDataAdapter SqlDA = new SqlDataAdapter(Sqlcmd);
                    DataSet DS = new DataSet();
                    SqlDA.Fill(DS);
                    close();
                    return DS;
                }
            }
            catch (Exception e)
            {
                writeLog("GetDataSet", e.ToString() + "\r\n" + cmdText);
                return null;
            }
        }

        /// <summary>
        /// 参数化查询
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        internal DataSet getDataSet(string cmdText, SqlParameter[] parms)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.Text, cmdText, parms);
                    SqlDataAdapter SqlDA = new SqlDataAdapter(Sqlcmd);
                    DataSet DS = new DataSet();
                    SqlDA.Fill(DS);
                    close();
                    return DS;
                }
            }
            catch (Exception e)
            {
                writeLog("GetDataSet", e.ToString() + "\r\n" + cmdText);
                return null;
            }
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        internal DataTable getDataTable(string cmdText)
        {
            DataSet ds = getDataSet(cmdText);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 参数化查询
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public DataTable getDataTable(string cmdText, SqlParameter[] parms)
        {
            DataSet ds = getDataSet(cmdText, parms);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 执行SQL并返回第一行值
        /// </summary>
        /// <param name="cmdText">Sql</param>
        /// <returns></returns>
        internal object getExSc(string cmdText)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.Text, cmdText, null);
                    object obj = Sqlcmd.ExecuteScalar();
                    close();
                    return obj;
                }
            }
            catch (Exception e)
            {
                writeLog("GetExSc", e.ToString() + "\r\n" + cmdText);
                return null;
            }
        }

        /// <summary>
        /// 执行带参数SQL并返回第一行值
        /// </summary>
        /// <param name="cmdText">SQL</param>
        /// <param name="sqlPmtA">参数</param>
        /// <returns></returns>
        internal object getExSc(string cmdText, SqlParameter[] sqlPmtA)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.Text, cmdText, sqlPmtA);
                    object obj = Sqlcmd.ExecuteScalar();
                    close();
                    return obj;
                }
            }
            catch (Exception e)
            {
                string s = e.ToString() + "\r\n" + cmdText + "\r\n";
                foreach (SqlParameter par in sqlPmtA)
                {
                    //s += par.ParameterName + " " + par.SqlDbType.GetType().Name + "：" + par.Value + "\r\n";
                    s += par.ParameterName + "：" + par.Value + "\r\n";
                }
                writeLog("GetExSc", s);
                return null;
            }
        }

        /// <summary>
        /// 执行SQL并返回输出参数
        /// </summary>
        /// <param name="cmdText">SQL</param>
        /// <param name="cmdType">类型</param>
        /// <param name="sqlPmtA">参数</param>
        /// <returns></returns>
        internal IEnumerator outPutParameter(string cmdText, CommandType cmdType, SqlParameter[] sqlPmtA)
        {
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, cmdType, cmdText, sqlPmtA);
                    Sqlcmd.ExecuteScalar();
                    close();
                    return Sqlcmd.Parameters.GetEnumerator();
                }
            }
            catch (Exception e)
            {
                writeLog("GetPmt", e.ToString() + "\r\n" + cmdText);
                return null;
            }
        }


        /// <summary>
        /// 关闭连接
        /// </summary>
        private void close()
        {
            if (conn != null && tranNum.Count == 0)
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 获取翻页Key参数
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="from"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public SqlParameterCollection getPageKeys(string keyName, string from, int currentPage, int pageSize)
        {
            SqlParameter[] pmts = new SqlParameter[] {
                new SqlParameter("@KeyName", keyName),
                new SqlParameter("@From", from),
                new SqlParameter("@CurrentPage", currentPage),
                new SqlParameter("@pageSize", pageSize),
                new SqlParameter("@TotalNumber", 0),
                new SqlParameter("@TotalPages", 0),
                new SqlParameter("@Keys", "")
            };
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.StoredProcedure, "Pro_PageKeys", pmts);
                    Sqlcmd.Parameters["@Keys"].Size = 100000;
                    Sqlcmd.Parameters["@CurrentPage"].Direction = ParameterDirection.InputOutput;
                    Sqlcmd.Parameters["@pageSize"].Direction = ParameterDirection.InputOutput;
                    Sqlcmd.Parameters["@TotalNumber"].Direction = ParameterDirection.Output;
                    Sqlcmd.Parameters["@TotalPages"].Direction = ParameterDirection.Output;
                    Sqlcmd.Parameters["@Keys"].Direction = ParameterDirection.Output;
                    Sqlcmd.ExecuteNonQuery();
                    close();
                    return Sqlcmd.Parameters;
                }
            }
            catch (Exception e)
            {
                writeLog("getPageKeys", e.ToString() + "\r\n" + "KeyName:" + keyName + "\r\nfrom:" + from + "\r\ncurrentPage:" + currentPage + "\r\npageSize:" + pageSize);
                return null;
            }
        }

        /// <summary>
        /// 获取翻页数据
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="keyName"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public UnSqlPage getPage(string columns, string keyName, string table, string where, string order, int currentPage, int pageSize)
        {
            if (where == null || where.Length == 0)
            {
                where = "1 = 1";
            }
            string from = table + " Where " + where + " " + order;
            SqlParameterCollection pmts = this.getPageKeys(keyName, from, currentPage, pageSize);
            if (pmts == null)
            {
                return null;
            }
            string keys = pmts["@Keys"].Value.ToString() + "";
            string ins = "1 = 0";
            if (keys.Length > 0)
            {
                ins = keyName + " In (" + keys + ")";
            }
            if (columns == null || columns.Length == 0)
            {
                columns = " * ";
            }
            string cmdText = "Select " + columns + " From " + table + " Where " + ins + " " + order;
            UnSqlPage page = new UnSqlPage();
            page.DataSource = this.getDataTable(cmdText);
            page.CurrentPage = currentPage;
            page.PageSize = pageSize;
            page.TotalNumber = Convert.ToInt32(pmts["@TotalNumber"].Value.ToString());
            page.TotalPages = Convert.ToInt32(pmts["@TotalPages"].Value.ToString());
            return page;
        }

        /// <summary>
        /// 删除某表某列所有约束和索引
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool dropColumnCI(string tableName, string columnName)
        {
            SqlParameter[] pmts = new SqlParameter[] {
                new SqlParameter("@TableName", tableName),
                new SqlParameter("@ColumnName", columnName),
            };
            try
            {
                using (SqlCommand Sqlcmd = new SqlCommand())
                {
                    setcmd(Sqlcmd, getConn(), null, CommandType.StoredProcedure, "mp_DropColConstraintAndIndex", pmts);
                    Sqlcmd.ExecuteNonQuery();
                    close();
                    return true;
                }
            }
            catch (Exception e)
            {
                writeLog("dropCI", e.ToString() + "\r\n" + "TableName:" + tableName + "\r\nColumnName:" + columnName);
                return false;
            }
        }
    }
}
