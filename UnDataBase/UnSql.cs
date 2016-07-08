using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using UnCommon.Tool;
using UnCommon.Files;

namespace UnDataBase
{
    public class UnSql
    {
        #region 私有变量

        // 数据库操作对象
        private UnSqlHelp help = null;

        #endregion

        #region 实例化

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public UnSql(string constr)
        {
            help = new UnSqlHelp(constr);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="user">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="dbName">连接的数据库</param>
        public UnSql(string ip, string port, string user, string pass, string dbName, UnSqlConnectModel model)
        {
            string constr;
            switch (model)
            {
                case UnSqlConnectModel.Create:
                case UnSqlConnectModel.ConnectOrCreate:
                    constr = "Data Source=" + ip + "," + port + ";Initial Catalog=master;User ID=" + user + ";Password=" + pass + ";";
                    string crtstr = UnSqlStr.createDB(dbName, model);
                    // 创建对象
                    help = new UnSqlHelp(constr);
                    // 创建数据库
                    help.exSql(crtstr);
                    break;
            }
            constr = "Data Source=" + ip + "," + port + ";Initial Catalog=" + dbName + ";User ID=" + user + ";Password=" + pass + ";";
            help = new UnSqlHelp(constr);
            // 翻页存储过程
            help.exSql(UnSqlStr.drop_Prc_Pageing());
            help.exSql(UnSqlStr.create_Prc_Pageing());
            // 判断字段是否存在 
            help.exSql(UnSqlStr.drop_mfn_IsColumnExists());
            help.exSql(UnSqlStr.create_mfn_IsColumnExists());
            // 查询某个字段的所有索引
            help.exSql(UnSqlStr.drop_mfn_GetColumnIndexes());
            help.exSql(UnSqlStr.create_mfn_GetColumnIndexes());
            // 删除指定列的所有索引  
            help.exSql(UnSqlStr.drop_mp_DropColumnIndexes());
            help.exSql(UnSqlStr.create_mp_DropColumnIndexes());
            // 删除某个表的某列的所有约束
            help.exSql(UnSqlStr.drop_mp_DropColConstraint());
            help.exSql(UnSqlStr.create_mp_DropColConstraint());
            // 删除指定字段的所有约束和索引
            help.exSql(UnSqlStr.drop_mp_DropColConstraintAndIndex());
            help.exSql(UnSqlStr.create_mp_DropColConstraintAndIndex());
        }

        #endregion

        #region 表操作

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="t"></param>
        public void createTable(Type t)
        {
            string s1 = UnSqlStr.createTableBase(t);
            //Console.WriteLine(s1);
            help.exSql(s1);
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="t"></param>
        public void dropTable(Type t)
        {
            string s1 = UnSqlStr.dropTable(t);
            //Console.WriteLine(s1);
            help.exSql(s1);
        }

        /// <summary>
        /// 创建表关系
        /// </summary>
        public void createTableRelation(Type t)
        {
            string s1 = UnSqlStr.createTableRelation(t);
            //Console.WriteLine(s1);
            if (s1.Length > 0)
            {
                help.exSql(s1);
            }
        }

        /// <summary>
        /// 清除表关系
        /// </summary>
        /// <param name="t"></param>
        public void dropTableRelation(Type t)
        {
            var sb = new StringBuilder();
            var tableName = UnToGen.getTableName(t);
            // 所有约束
            var dt = help.getDataTable(UnSqlStr.getAllConstraints(t));
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    sb.AppendLine("Alter Table " + tableName + " Drop Constraint " + item["name"] + ";");
                }
            }
            // 所有索引
            dt = help.getDataTable(UnSqlStr.getAllIndex(t));
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    sb.AppendLine("Drop Index " + item["name"] + " On " + tableName + ";");
                }
            }
            if (sb.Length > 0)
            {
                //Console.WriteLine(sb.ToString());
                help.exSql(sb.ToString());
            }
        }

        /// <summary>
        /// 清除表所有约束和索引
        /// </summary>
        /// <param name="t"></param>
        private void dropTableRelationAll(Type t)
        {
            string tableName = UnToGen.getTableName(t);
            var list = UnToGen.getListField(t);
            foreach (var name in list)
            {
                help.dropColumnCI(tableName, name);
            }
        }

        /// <summary>
        /// 获取数据库表字段
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private List<string> getDBTableColumns(Type t)
        {
            var list = new List<string>();
            string sql = "Select * from syscolumns Where ID=OBJECT_ID('" + UnToGen.getTableName(t) + "')";
            var dt = queryDT(sql, null);
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item["name"].ToString());
                }
            }
            return list;
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="t"></param>
        public void addColumn(Type t)
        {
            StringBuilder sb = new StringBuilder();
            var listDB = getDBTableColumns(t);
            var listFP = UnToGen.getListFieldPropertyInfo(t);
            string tableName = UnToGen.getTableName(t);
            foreach (var item in listFP)
            {
                string fName = UnToGen.getFieldName(item);
                if (listDB.Find(e => e == fName) == null)
                {
                    sb.AppendLine("Alter Table " + tableName + " Add " + UnSqlStr.getFieldTypeAndNull(item) + ";");
                }
            }
            if (sb.Length > 0)
            {
                help.exSql(sb.ToString());
                //Console.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="t"></param>
        public void dropColumn(Type t)
        {
            StringBuilder sb = new StringBuilder();
            var listDB = getDBTableColumns(t);
            var listFP = UnToGen.getListFieldPropertyInfo(t);
            string tableName = UnToGen.getTableName(t);
            foreach (var name in listDB)
            {
                bool isHave = false;
                foreach (var item in listFP)
                {
                    string fName = UnToGen.getFieldName(item);
                    if (name == fName)
                    {
                        isHave = true;
                        break;
                    }
                }
                // 如果不存在
                if (!isHave)
                {
                    sb.AppendLine("Alter Table " + tableName + " Drop Column " + name + ";");
                }
            }
            if (sb.Length > 0)
            {
                help.exSql(sb.ToString());
                //Console.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// 更改字段属性
        /// </summary>
        /// <param name="t"></param>
        public void alterColumn(Type t)
        {
            StringBuilder sb = new StringBuilder();
            var listDB = getDBTableColumns(t);
            var listFP = UnToGen.getListFieldPropertyInfo(t);
            string tableName = UnToGen.getTableName(t);
            foreach (var name in listDB)
            {
                foreach (var item in listFP)
                {
                    string fName = UnToGen.getFieldName(item);
                    var attr = UnToGen.getAttrSql(item);
                    if (name == fName)
                    {
                        string fType = UnSqlStr.getFieldTypeAndNull(item);
                        // 不能修改自增
                        if (fType != null && fType.ToUpper().IndexOf("IDENTITY") < 0)
                        {
                            sb.AppendLine("Alter Table " + tableName + " Alter Column " + UnSqlStr.getFieldTypeAndNull(item) + ";");
                        }
                        break;
                    }
                }
            }
            if (sb.Length > 0)
            {
                help.exSql(sb.ToString());
                //Console.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="t"></param>
        public void updateTable(Type t)
        {
            addColumn(t);
            dropColumn(t);
            alterColumn(t);
        }

        /// <summary>
        /// 创建多个表
        /// </summary>
        /// <param name="t"></param>
        public void createTableList(List<Type> list)
        {
            foreach (var item in list)
            {
                createTable(item);
            }
        }

        /// <summary>
        /// 清除多个表关系
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isACS"></param>
        public void dropTableRelationList(List<Type> list, bool isACS)
        {
            if (!isACS)
            {
                list.Reverse();
            }
            foreach (var item in list)
            {
                dropTableRelation(item);
            }
            if (!isACS)
            {
                list.Reverse();
            }
        }

        /// <summary>
        /// 清除多个表关系(倒序)
        /// </summary>
        /// <param name="list"></param>
        public void dropTableRelationList(List<Type> list)
        {
            dropTableRelationList(list, false);
        }

        /// <summary>
        /// 更新多个表
        /// </summary>
        /// <param name="list"></param>
        public void updateTableList(List<Type> list)
        {
            foreach (var item in list)
            {
                updateTable(item);
            }
        }

        /// <summary>
        /// 创建多个表关系
        /// </summary>
        /// <param name="list"></param>
        public void createTableRelationList(List<Type> list)
        {
            foreach (var item in list)
            {
                createTableRelation(item);
            }
        }

        #endregion

        #region 基础SQL

        private void dropTableCI(Type t)
        {
            string s = @"";
        }

        #endregion

        #region 添加数据

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public long insert<T>(T t) where T : new()
        {
            SqlParameter[] SqlPmtA = UnSqlStr.getSqlPmtA<T>(t);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Insert Into " + UnToGen.getTableName(typeof(T)) + " ");
            strSql.Append(UnSqlStr.getAddStr<T>(SqlPmtA));
            strSql.Append(" Select SCOPE_IDENTITY() As KeyID");
            long KeyID = Convert.ToInt64(help.getExSc(strSql.ToString(), SqlPmtA));
            return KeyID;
        }

        #endregion

        #region 删除数据

        public bool delete<T>(string selection, string selectionArgs) where T : new()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Delete " + UnToGen.getTableName(typeof(T)) + " ");
            strSql.Append(UnSqlStr.getSelectionSql<T>(selection, selectionArgs));
            return help.exSql(strSql.ToString());
        }

        #endregion

        #region 查询数据

        /// <summary>
        /// 查询实体(核心方法)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        private List<T> query<T>(string strSql, SqlParameter[] parms) where T : new()
        {
            List<T> list = new List<T>();
            DataTable dt = help.getDataTable(strSql, parms);
            if (dt != null)
            {
                list = UnToGen.dtToT<T>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件-Where ID={0}</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having"></param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        private List<T> query<T>(string[] columns,
            string selection, string[] selectionArgs, string groupBy,
            string having, string orderBy) where T : new()
        {
            List<SqlParameter> list = new List<SqlParameter>();
            int i = 0;
            if (selectionArgs != null)
            {
                foreach (string arg in selectionArgs)
                {
                    selection = selection.Replace("'{" + i + "}'", "@p" + i);
                    selection = selection.Replace("{" + i + "}", "@p" + i);
                    SqlParameter par = new SqlParameter("p" + i, arg);
                    list.Add(par);
                    i++;
                }
            }
            string strSql = UnSqlStr.getQuerySql<T>(columns, selection, null, groupBy, having, orderBy);
            return query<T>(strSql, list.ToArray());
        }

        /// <summary>
        /// 一般查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<T> query<T>(string columns, string selection, string selectionArgs, string orderBy) where T : new()
        {
            string[] cs = null;
            string[] ss = null;

            if (columns != null)
            {
                cs = columns.Split(',');
            }
            if (selectionArgs != null)
            {
                ss = selectionArgs.Split(',');
            }
            return query<T>(cs, selection, ss, null, null, orderBy);
        }

        /// <summary>
        /// 一般查询语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">属性</param>
        /// <param name="selection">条件语句</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public List<T> query<T>(string columns, string selection, string[] selectionArgs, string orderBy, bool isArray) where T : new()
        {
            string[] cs = null;
            if (columns != null)
            {
                cs = columns.Split(',');
            }
            return query<T>(cs, selection, selectionArgs, null, null, orderBy);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<T> query<T>(string columns,
        string selection, string selectionArgs, string groupBy,
        string having, string orderBy) where T : new()
        {
            string[] cs = null;
            string[] ss = null;
            if (columns != null)
            {
                cs = columns.Split(',');
            }
            if (selectionArgs != null)
            {
                ss = selectionArgs.Split(',');
            }
            return query<T>(cs, selection, ss, groupBy, having, orderBy);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private T querySingle<T>(string columns, string selection, string selectionArgs, string orderBy) where T : new()
        {
            List<T> list = query<T>(columns, selection, selectionArgs, orderBy);
            if (list.Count > 0)
            {
                return list[0];
            }
            return default(T);
        }

        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private DataTable queryDT<T>(string[] columns,
      string selection, string[] selectionArgs, string groupBy,
      string having, string orderBy) where T : new()
        {
            string strSql = UnSqlStr.getQuerySql<T>(columns, selection, selectionArgs, groupBy, having, orderBy);
            return help.getDataTable(strSql);
        }

        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataTable queryDT<T>(string columns,
        string selection, string selectionArgs, string groupBy,
        string having, string orderBy) where T : new()
        {
            string[] cs = null;
            string[] ss = null;
            if (columns != null)
            {
                cs = columns.Split(',');
            }
            if (selectionArgs != null)
            {
                ss = selectionArgs.Split(',');
            }
            return queryDT<T>(cs, selection, ss, groupBy, having, orderBy);
        }

        /// <summary>
        /// 一般查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataTable queryDT<T>(string columns, string selection, string selectionArgs, string orderBy) where T : new()
        {
            string[] cs = null;
            string[] ss = null;

            if (columns != null)
            {
                cs = columns.Split(',');
            }
            if (selectionArgs != null)
            {
                ss = selectionArgs.Split(',');
            }
            return queryDT<T>(cs, selection, ss, null, null, orderBy);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataRow queryDTSingle<T>(string columns, string selection, string selectionArgs, string orderBy) where T : new()
        {
            DataTable list = queryDT<T>(columns, selection, selectionArgs, orderBy);
            if (list.Rows.Count > 0)
            {
                return list.Rows[0];
            }
            return null;
        }

        /// <summary>
        /// 获取翻页数据
        /// </summary>
        /// <param name="columns">字段</param>
        /// <param name="keyName">主键名</param>
        /// <param name="table">表名</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回DataSource:数据集,CurrentPage:当前页码,PageSize:每页大小,TotalNumber:总记录数,TotalPages:总页数</returns>
        public UnSqlPage queryPage(string columns, string keyName, string table, string where, string order, int currentPage, int pageSize)
        {
            UnSqlPage page = help.getPage(columns, keyName, table, where, order, currentPage, pageSize);
            return page;
        }

        /// <summary>
        /// 获取翻页数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="where">条件</param>
        /// <param name="whereArgs">条件参数</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回DataSource:数据集,CurrentPage:当前页码,PageSize:每页大小,TotalNumber:总记录数,TotalPages:总页数</returns>
        public UnSqlPage queryPage<T>(string columns, string where, string[] whereArgs, string order, int currentPage, int pageSize) where T : new()
        {
            string keyName = UnToGen.getAutoNum(typeof(T), true);
            string table = UnToGen.getTableName(typeof(T));
            where = UnSqlStr.getQueryPageWhere<T>(where, whereArgs);
            UnSqlPage page = help.getPage(columns, keyName, table, where, order, currentPage, pageSize);
            page.TSource = UnToGen.dtToT<T>(page.DataSource);
            return page;
        }

        /// <summary>
        /// 获取翻页数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="where">条件</param>
        /// <param name="whereArgs">条件参数</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回DataSource:数据集,CurrentPage:当前页码,PageSize:每页大小,TotalNumber:总记录数,TotalPages:总页数</returns>
        public UnSqlPage queryPage<T>(string columns, string where, string whereArgs, string order, int currentPage, int pageSize) where T : new()
        {
            string keyName = UnToGen.getAutoNum(typeof(T), true);
            string table = UnToGen.getTableName(typeof(T));
            string[] args = null;
            if (whereArgs != null && whereArgs.Length > 0)
            {
                args = whereArgs.Split(',');
            }
            where = UnSqlStr.getQueryPageWhere<T>(where, args);
            UnSqlPage page = help.getPage(columns, keyName, table, where, order, currentPage, pageSize);
            if (page.DataSource != null && page.DataSource.Rows.Count > 0)
            {
                page.TSource = UnToGen.dtToT<T>(page.DataSource);
            }
            else
            {
                page.TSource = new List<T>();
            }
            return page;
        }

        /// <summary>
        /// 获取翻页数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回DataSource:数据集,CurrentPage:当前页码,PageSize:每页大小,TotalNumber:总记录数,TotalPages:总页数</returns>
        public UnSqlPage queryPage<T>(string columns, string order, int currentPage, int pageSize) where T : new()
        {
            string keyName = UnToGen.getAutoNum(typeof(T), true);
            string table = UnToGen.getTableName(typeof(T));
            UnSqlPage page = help.getPage(columns, keyName, table, null, order, currentPage, pageSize);
            page.TSource = UnToGen.dtToT<T>(page.DataSource);
            return page;
        }

        /// <summary>
        /// 递归查询
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="ID">开始ID</param>
        /// <returns>返回所有递归ID</returns>
        public string recursiveID<T>(int ID) where T : new()
        {
            string tableName = UnToGen.getTableName(typeof(T));
            string CodeName = ""; ;
            foreach (string str1 in UnToGen.getListField<T>())
            {
                if (str1.Replace("Code", "") != str1)
                {
                    CodeName = str1.Replace("_", "");
                }
            }
            string IDList = "";
            DataTable _DataTable = help.getDataTable("declare @Id Int;set @Id=" + ID + ";With GetInd as(Select * from " + UnToGen.getTableName(typeof(T)) + " where ID=@Id union all select w1.* from " + UnToGen.getTableName(typeof(T)) + " w1 inner join GetInd on w1." + CodeName + " = convert(nvarchar(20),GetInd." + CodeName + ") + convert(nvarchar(20),GetInd.ID)) select * from GetInd");
            foreach (DataRow _DataRow in _DataTable.Rows)
            {
                IDList += _DataRow["ID"] + ",";
            }
            return IDList.TrimEnd(',');
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parms">参数</param>
        /// <returns>返回DataTable</returns>
        public DataTable queryDT(string sql, SqlParameter[] parms)
        {
            return help.getDataTable(sql, parms);
        }

        /// <summary>
        /// 查询第一行第一个值
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parms"参数></param>
        /// <returns>无数据则返回NULL</returns>
        public object queryScalar(String sql, SqlParameter[] parms)
        {
            DataTable dt = queryDT(sql, parms);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0];
            }
            return null;
        }

        /// <summary>
        /// 查询第一行第一个值并转为Int
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parms"参数></param>
        public int? queryScalarToInt(String sql, SqlParameter[] parms)
        {
            try
            {
                return Convert.ToInt32(queryScalar(sql, parms));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 查询第一行第一个值并转为Stringt
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parms"参数></param>
        public String queryScalarToString(String sql, SqlParameter[] parms)
        {
            try
            {
                return queryScalar(sql, parms).ToString();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region 修改数据

        /// <summary>
        /// 条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool update<T>(T t, string columns, string selection, string selectionArgs) where T : new()
        {
            SqlParameter[] SqlPmtA = UnSqlStr.getSqlPmtA<T>(t, columns);
            bool b = false;
            string sql = "Update " + UnToGen.getTableName(typeof(T)) + " Set " + UnSqlStr.getUpdStr(SqlPmtA) + " " + UnSqlStr.getSelectionSql<T>(selection, selectionArgs);
            b = help.exSql(sql, SqlPmtA);
            return b;
        }

        /// <summary>
        /// 条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <returns></returns>
        public bool update<T>(T t, string columns, string selection, string[] selectionArgs) where T : new()
        {
            SqlParameter[] SqlPmtA = UnSqlStr.getSqlPmtA<T>(t, columns);
            bool b = false;
            string sql = "Update " + UnToGen.getTableName(typeof(T)) + " Set " + UnSqlStr.getUpdStr(SqlPmtA) + " " + UnSqlStr.getSelectionSql<T>(selection, selectionArgs);
            b = help.exSql(sql, SqlPmtA);
            return b;
        }

        #endregion


    }
}
