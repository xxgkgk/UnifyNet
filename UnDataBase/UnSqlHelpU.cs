using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UnCommon.Files;

namespace UnDataBase
{
    /// <summary>
    /// SQL帮助类
    /// </summary>
    public class UnSqlHelpU
    {
        #region 私有变量 

        /// <summary>
        /// 业务操作对象
        /// </summary>
        private SqlCommand comd = null;

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string cs = null;

        /// <summary>
        /// 是否事务
        /// </summary>
        private bool isTrans = false;

        #endregion

        #region 实例化

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="constr">连接字符串</param>
        /// <param name="istrans">是否事务</param>
        public UnSqlHelpU(string constr, bool istrans)
        {
            cs = constr;
            isTrans = istrans;
    
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public UnSqlHelpU(string constr)
        {
            cs = constr;
            isTrans = false;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获得cmd对象
        /// </summary>
        /// <param name="cmdType">cmd类型</param>
        /// <param name="cmdText">cmd命令</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        private SqlCommand getCmd(CommandType cmdType, string cmdText, SqlParameter[] paras)
        {
            if (comd == null)
            {
                this.comd = new SqlCommand();
                this.comd.Connection = new SqlConnection(cs);
                this.comd.Connection.Open();
                if (isTrans)
                {
                    this.comd.Transaction = this.comd.Connection.BeginTransaction();
                }
            }
            comd.CommandText = cmdText;
            comd.CommandType = cmdType;
            if (paras != null)
            {
                foreach (SqlParameter parm in paras)
                {
                    this.comd.Parameters.Add(parm);
                }
            }
            return comd;
        }

        /// <summary>
        /// 执行SQL并返回输出参数
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <param name="cmdType">cmd类型</param>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        private IEnumerator outPutParameter(string cmdText, CommandType cmdType, SqlParameter[] parms)
        {
            try
            {
                SqlCommand Sqlcmd = getCmd(CommandType.Text, cmdText, parms);
                Sqlcmd.ExecuteScalar();
                close();
                return Sqlcmd.Parameters.GetEnumerator();
            }
            catch (Exception e)
            {
                writeLog("getExSc", e, cmdText, parms);
                return null;
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="ex"></param>
        /// <param name="cmdText"></param>
        /// <param name="parms"></param>
        private void writeLog(string pre, Exception ex, string cmdText, SqlParameter[] parms)
        {
            if (this.comd.Transaction != null)
            {
                this.comd.Transaction.Rollback();
                this.comd.Transaction = null;
            }
            close();
            string s = ex.ToString() + "\r\n链接串:" + cs + "\r\nCmdText：" + cmdText + "\r\n";
            if (parms != null)
            {
                foreach (SqlParameter par in parms)
                {
                    //s += par.ParameterName + " " + par.SqlDbType.GetType().Name + "：" + par.Value + "\r\n";
                    s += par.ParameterName + "：" + par.Value + "\r\n";
                }
            }

            UnFile.writeLog(pre, s);
        }

        /// <summary>
        /// 关闭对象
        /// </summary>
        public void close()
        {
            // cmd对象不存在则忽略
            if (this.comd == null)
            {
                return;
            }
            // 有事务则忽略
            if (this.comd.Transaction != null)
            {
                return;
            }
            //Console.WriteLine("关闭连接");
            this.comd.Connection.Dispose();
            this.comd.Dispose();
            this.comd = null;
        }

        #endregion

        #region 执行SQL

        /// <summary>
        /// 执行带参数SQL
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <param name="parms">参数</param>
        /// <returns>返回受影响的行数</returns>
        public int? exSql(string cmdText, SqlParameter[] parms)
        {
            try
            {
                SqlCommand Sqlcmd = getCmd(CommandType.Text, cmdText, parms);
                int i = Sqlcmd.ExecuteNonQuery();
                close();
                return i;

            }
            catch (Exception e)
            {
                writeLog("exSql", e, cmdText, parms);
                return null;
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <returns>返回受影响的行数</returns>
        public int? exSql(string cmdText)
        {
            return exSql(cmdText, null);
        }

        #endregion

        #region 查询表值

        /// <summary>
        /// 获取DataReader
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <returns>返回SqlDataReader</returns>
        public SqlDataReader getDataReader(string cmdText)
        {
            try
            {
                SqlCommand Sqlcmd = getCmd(CommandType.Text, cmdText, null);
                SqlDataReader _SqlDataReader = Sqlcmd.ExecuteReader(CommandBehavior.SingleResult);
                close();
                return _SqlDataReader;
            }
            catch (Exception e)
            {
                writeLog("getDataReader", e, cmdText, null);
                return null;
            }
        }

        /// <summary>
        /// 获取DataSet(参数化)
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <param name="parms">参数</param>
        /// <returns>返回DataSet</returns>
        public DataSet getDataSet(string cmdText, SqlParameter[] parms)
        {
            try
            {
                SqlCommand Sqlcmd = getCmd(CommandType.Text, cmdText, parms);
                SqlDataAdapter SqlDA = new SqlDataAdapter(Sqlcmd);
                DataSet DS = new DataSet();
                SqlDA.Fill(DS);
                close();
                return DS;
            }
            catch (Exception e)
            {
                writeLog("getDataSet", e, cmdText, parms);
                return null;
            }
        }

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <returns>返回SqlDataReader</returns>
        public DataSet getDataSet(string cmdText)
        {
            return getDataSet(cmdText, null);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <returns>返回DataTable</returns>
        public DataTable getDataTable(string cmdText)
        {
            DataSet ds = getDataSet(cmdText);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 获取DataTable(参数化)
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <param name="parms">参数</param>
        /// <returns>返回DataTable</returns>
        public DataTable getDataTable(string cmdText, SqlParameter[] parms)
        {
            DataSet ds = getDataSet(cmdText, parms);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #endregion

        #region 查询第1行第1列

        /// <summary>
        /// 执行SQL并返回第一行第一列值
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <param name="parms">参数</param>
        /// <returns>返回第一行第一列值</returns>
        public object getExSc(string cmdText, SqlParameter[] parms)
        {
            try
            {
                SqlCommand Sqlcmd = getCmd(CommandType.Text, cmdText, parms);
                object obj = Sqlcmd.ExecuteScalar();
                close();
                return obj;
            }
            catch (Exception e)
            {
                writeLog("getExSc", e, cmdText, parms);
                return null;
            }
        }

        /// <summary>
        /// 执行SQL并返回第一行第一列值
        /// </summary>
        /// <param name="cmdText">cmd命令</param>
        /// <returns>返回第一行第一列值</returns>
        public object getExSc(string cmdText)
        {
            return getExSc(cmdText, null);
        }

        #endregion

        #region 查询翻页

        /// <summary>
        /// 获取翻页键值列
        /// </summary>
        /// <param name="keyName">主键名</param>
        /// <param name="from">from语句</param>
        /// <param name="currentPage">查询的页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回页键值列</returns>
        private SqlParameterCollection getPageKeys(string keyName, string from, int currentPage, int pageSize)
        {
            SqlParameter[] parms = new SqlParameter[] {
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
                SqlCommand Sqlcmd = getCmd(CommandType.StoredProcedure, "Pro_PageKeys", parms);
                Sqlcmd.Parameters["@Keys"].Size = 100000;
                Sqlcmd.Parameters["@CurrentPage"].Direction = ParameterDirection.InputOutput;
                Sqlcmd.Parameters["@pageSize"].Direction = ParameterDirection.InputOutput;
                Sqlcmd.Parameters["@TotalNumber"].Direction = ParameterDirection.Output;
                Sqlcmd.Parameters["@TotalPages"].Direction = ParameterDirection.Output;
                Sqlcmd.Parameters["@Keys"].Direction = ParameterDirection.Output;
                Sqlcmd.ExecuteNonQuery();
                var ret = Sqlcmd.Parameters;
                close();
                return ret;
            }
            catch (Exception e)
            {
                writeLog("getPageKeys", e, "Pro_PageKeys", parms);
                return null;
            }
        }

        /// <summary>
        /// 获取翻页结果
        /// </summary>
        /// <param name="columns">查询字段</param>
        /// <param name="keyName">主键名</param>
        /// <param name="table">表名或表联查语句</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">查询的页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回翻页结果</returns>
        public UnSqlPage getPage(string columns, string keyName, string table, string where, string order, int currentPage, int pageSize)
        {
            if (String.IsNullOrWhiteSpace(where))
            {
                where = "1 = 1";
            }
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.ToLower().IndexOf("order") < 0)
                {
                    order = "Order By " + order;
                }
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

        #endregion

        #region 提交事务

        /// <summary>
        /// 提交事务
        /// </summary>
        public void commit()
        {
            // cmd对象不存在则忽略
            if (this.comd == null)
            {
                return;
            }
            // 不存在事务则忽略
            if (this.comd.Transaction == null)
            {
                return;
            }
            this.comd.Transaction.Commit();
            this.comd.Transaction = null;
            close();
        }

        #endregion;
    }
}
