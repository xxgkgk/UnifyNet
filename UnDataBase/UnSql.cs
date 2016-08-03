using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using UnCommon.Tool;
using UnCommon.Files;
using UnCommon.Config;

namespace UnDataBase
{
    /// <summary>
    /// SQL操作类
    /// </summary>
    public class UnSql
    {
        #region 私有变量/方法

        // 数据库操作对象
        private UnSqlHelpU help = null;

        /// <summary>
        /// 转参数化条件
        /// </summary>
        /// <param name="selection">条件语句,如:ID = {0} Or Name = '{1}'</param>
        /// <param name="selectionArgs">条件参数,如:1,admin</param>
        /// <returns>返回object[2],0:条件语句,1:SqlParameter参数</returns>
        private object[] toParsSelection(string selection, string[] selectionArgs)
        {
            object[] objs = new object[2];
            List<SqlParameter> list = new List<SqlParameter>();
            // 参数序号
            int i = 0;
            // 参数便名PID
            int pid = UnInit.pid();
            if (selectionArgs != null)
            {
                foreach (string arg in selectionArgs)
                {
                    string name = "@p" + i;
                    string byName = UnSqlStr.toByName(name, pid);
                    selection = selection.Replace("'{" + i + "}'", byName);
                    selection = selection.Replace("{" + i + "}", byName);
                    SqlParameter par = new SqlParameter(byName, arg);
                    list.Add(par);
                    i++;
                }
            }
            objs[0] = selection;
            objs[1] = list.ToArray();
            return objs;
        }

        #endregion

        #region 实例化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="user">账户</param>
        /// <param name="pass">密码</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="model">连接模式</param>
        /// <param name="trans">是否事务</param>
        private void init(string ip, string port, string user, string pass, string dbName, UnSqlConnectModel model, bool trans)
        {
            string constr1 = "Data Source=" + ip + "," + port + ";Initial Catalog=master;User ID=" + user + ";Password=" + pass + ";";
            string constr2 = "Data Source=" + ip + "," + port + ";Initial Catalog=" + dbName + ";User ID=" + user + ";Password=" + pass + ";";
            switch (model)
            {
                case UnSqlConnectModel.Create:
                case UnSqlConnectModel.ConnectOrCreate:
                    string crtstr = UnSqlStr.createDB(dbName, model);
                    // 创建数据库
                    new UnSqlHelpU(constr1, false).exSql(crtstr);
                    break;
            }
            help = new UnSqlHelpU(constr2, trans);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="user">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="dbName">连接的数据库</param>
        /// <param name="model">连接类型</param>
        public UnSql(string ip, string port, string user, string pass, string dbName, UnSqlConnectModel model)
        {
            init(ip, port, user, pass, dbName, model, false);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="user">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="dbName">连接的数据库</param>
        /// <param name="model">连接类型</param>
        /// <param name="trans">是否开启事务</param>
        public UnSql(string ip, string port, string user, string pass, string dbName, UnSqlConnectModel model, bool trans)
        {
            init(ip, port, user, pass, dbName, model, trans);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="constr"></param>
        /// <param name="trans"></param>
        public UnSql(string constr, bool trans)
        {
            help = new UnSqlHelpU(constr, trans);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public UnSql(string constr)
        {
            help = new UnSqlHelpU(constr);
        }

        #endregion

        #region 库/表/存储过程操作

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>返回受影响的行数:null=执行失败,-1=未执行</returns>
        public int? createTable(Type t)
        {
            string s1 = UnSqlStr.createTableBase(t);
            return help.exSql(s1);
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>返回受影响的行数:null=执行失败,-1=未执行</returns>
        public int? dropTable(Type t)
        {
            string s1 = UnSqlStr.dropTable(t);
            return help.exSql(s1);
        }

        /// <summary>
        /// 创建表关系
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>返回受影响的行数:null=执行失败,-1=未执行</returns>
        public int? createTableRelation(Type t)
        {
            string s1 = UnSqlStr.createTableRelation(t);
            if (s1.Length > 0)
            {
                return help.exSql(s1);
            }
            return -1;
        }

        /// <summary>
        /// 清除表关系
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>返回受影响的行数:null=执行失败,-1=未执行</returns>
        public int? dropTableRelation(Type t)
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
                return help.exSql(sb.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 获取数据库表字段
        /// </summary>
        /// <param name="t">类型</param>
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
        /// <param name="t">类型</param>
        /// <returns>返回受影响的行数:null=执行失败,-1=未执行</returns>
        public int? addColumn(Type t)
        {
            StringBuilder sb = new StringBuilder();
            var listDB = getDBTableColumns(t);
            var listFP = UnToGen.getListFieldPropertyInfo(t);
            string tableName = UnToGen.getTableName(t);
            foreach (var item in listFP)
            {
                string fName = UnToGen.getFieldName(item);
                // 数据库字段不分大小写
                if (listDB.Find(e => e == fName) == null)
                {
                    sb.AppendLine("Alter Table " + tableName + " Add " + UnSqlStr.getFieldTypeAndNull(item) + ";");
                }
            }
            if (sb.Length > 0)
            {
                return help.exSql(sb.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>返回受影响的行数:null=执行失败,-1=未执行</returns>
        public int? dropColumn(Type t)
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
                return help.exSql(sb.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 更改字段属性
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>返回受影响的行数:null=执行失败,-1=未执行</returns>
        public int? alterColumn(Type t)
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
                return help.exSql(sb.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>是否成功</returns>
        public bool updateTable(Type t)
        {
            int? rst = null;
            rst = dropColumn(t);
            if (rst == null)
            {
                return false;
            }
            rst = addColumn(t);
            if (rst == null)
            {
                return false;
            }
            rst = alterColumn(t);
            if (rst == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建多个表
        /// </summary>
        /// <param name="list">要创建的表数组</param>
        /// <returns>是否全部成功</returns>
        public bool createTableList(List<Type> list)
        {
            foreach (var item in list)
            {
                int? rst = createTable(item);
                if (rst == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 清除多个表关系
        /// </summary>
        /// <param name="list">要删除表关系的表数组</param>
        /// <param name="isACS">是否正序</param>
        /// <returns>是否全部成功</returns>
        public bool dropTableRelationList(List<Type> list, bool isACS)
        {
            if (!isACS)
            {
                list.Reverse();
            }
            foreach (var item in list)
            {
                int? rst = dropTableRelation(item);
                if (rst == null)
                {
                    return false;
                }
            }
            if (!isACS)
            {
                list.Reverse();
            }
            return true;
        }

        /// <summary>
        /// 清除多个表关系(倒序)
        /// </summary>
        /// <param name="list">要删除表关系的表数组</param>
        /// <returns>是否全部成功</returns>
        public bool dropTableRelationList(List<Type> list)
        {
            return dropTableRelationList(list, false);
        }

        /// <summary>
        /// 更新多个表
        /// </summary>
        /// <param name="list">要更新的表数组</param>
        /// <returns>是否全部成功</returns>
        public bool updateTableList(List<Type> list)
        {
            foreach (var item in list)
            {
                if (!updateTable(item))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 创建多个表关系
        /// </summary>
        /// <param name="list"></param>
        /// <returns>是否全部成功</returns>
        public bool createTableRelationList(List<Type> list)
        {
            foreach (var item in list)
            {
                if (createTableRelation(item) == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 更新基础存储过程/函数等
        /// </summary>
        /// <returns>是否全部成功</returns>
        public bool updateBase()
        {
            // 翻页存储过程
            if (help.exSql(UnSqlStr.drop_Pro_PageKeys()) == null)
            {
                return false;
            }
            if (help.exSql(UnSqlStr.create_Pro_PageKeys()) == null)
            {
                return false;
            }
            // 判断字段是否存在 
            //help.exSql(UnSqlStr.drop_mfn_IsColumnExists());
            //help.exSql(UnSqlStr.create_mfn_IsColumnExists());
            // 查询某个字段的所有索引
            //help.exSql(UnSqlStr.drop_mfn_GetColumnIndexes());
            //help.exSql(UnSqlStr.create_mfn_GetColumnIndexes());
            // 删除指定列的所有索引  
            //help.exSql(UnSqlStr.drop_mp_DropColumnIndexes());
            //help.exSql(UnSqlStr.create_mp_DropColumnIndexes());
            // 删除某个表的某列的所有约束
            //help.exSql(UnSqlStr.drop_mp_DropColConstraint());
            //help.exSql(UnSqlStr.create_mp_DropColConstraint());
            // 删除指定字段的所有约束和索引
            //help.exSql(UnSqlStr.drop_mp_DropColConstraintAndIndex());
            //help.exSql(UnSqlStr.create_mp_DropColConstraintAndIndex());
            return true;
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int? exSql(string sql)
        {
            return help.exSql(sql);
        }

        #endregion

        #region 添加数据

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">泛型对象</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <returns></returns>
        public long insert<T>(T t, bool isXactAbort) where T : new()
        {
            SqlParameter[] SqlPmtA = UnSqlStr.getSqlPmtA<T>(t);
            StringBuilder strSql = new StringBuilder();
            if (isXactAbort)
            {
                strSql.Append("Set xact_abort ON;");
            }
            strSql.Append("Insert Into " + UnToGen.getTableName(typeof(T), isXactAbort) + " ");
            strSql.Append(UnSqlStr.getAddStr<T>(SqlPmtA));
            object obj = null;
            if (!isXactAbort)
            {
                strSql.Append(" Select SCOPE_IDENTITY() As KeyID");
                obj = help.getExSc(strSql.ToString(), SqlPmtA);
                if (obj == null)
                {
                    return -1;
                }
                return Convert.ToInt64(obj);
            }
            obj = help.getExSc(strSql.ToString(), SqlPmtA);
            if (obj == null)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public long insert<T>(T t) where T : new()
        {
            return insert(t, false);
        }

        #endregion

        #region 删除数据

        /// <summary>
        /// 删除数据(核心)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <returns></returns>
        public int? delete<T>(string selection, string[] selectionArgs, bool isXactAbort) where T : new()
        {
            // 构造参数化查询
            object[] objs = toParsSelection(selection, selectionArgs);

            StringBuilder strSql = new StringBuilder();
            if (isXactAbort)
            {
                strSql.Append("Set xact_abort ON;");
            }
            strSql.Append("Delete " + UnToGen.getTableName(typeof(T), isXactAbort) + " ");
            string where = (string)objs[0];
            if (where.Length > 0)
            {
                strSql.Append("Where " + where);
            }
            return help.exSql(strSql.ToString(), (SqlParameter[])objs[1]);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <returns></returns>
        public int? delete<T>(string selection, string selectionArgs, bool isXactAbort) where T : new()
        {
            if (selectionArgs != null)
            {
                return delete<T>(selection, selectionArgs.Split(','), isXactAbort);
            }
            return delete<T>(selection, (string[])null, isXactAbort);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <returns></returns>
        public int? delete<T>(string selection, string selectionArgs) where T : new()
        {
            if (selectionArgs != null)
            {
                return delete<T>(selection, selectionArgs.Split(','), false);
            }
            return delete<T>(selection, (string[])null, false);
        }

        #endregion

        #region 查询数据

        /// <summary>
        /// 查询实体(核心方法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="parms"></param>
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
        /// 参数化获取实体(核心)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件-Where ID={0}</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having"></param>
        /// <param name="orderBy">排序</param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <returns></returns>
        private List<T> query<T>(string[] columns,
            string selection, string[] selectionArgs, string groupBy,
            string having, string orderBy,bool isLinkedServer) where T : new()
        {
            // 构造参数化查询
            object[] objs = toParsSelection(selection, selectionArgs);
            string strSql = UnSqlStr.getQuerySql<T>(columns, (string)objs[0], null, groupBy, having, orderBy, isLinkedServer);
            return query<T>(strSql, (SqlParameter[])objs[1]);
        }

        /// <summary>
        /// 参数化获取实体
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
            return query<T>(columns,
           selection, selectionArgs, groupBy,
            having, orderBy, false);
        }

        /// <summary>
        /// 一般查询语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <returns></returns>
        public List<T> query<T>(string columns, string selection, string selectionArgs, string orderBy, bool isLinkedServer) where T : new()
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
            return query<T>(cs, selection, ss, null, null, orderBy, isLinkedServer);
        }

        /// <summary>
        /// 一般查询语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public List<T> query<T>(string columns, string selection, string selectionArgs, string orderBy) where T : new()
        {
            return query<T>(columns, selection, selectionArgs, orderBy, false);
        }

        /// <summary>
        /// 一般查询语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
        /// <param name="isArray">参数是否为数组</param>
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
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having"></param>
        /// <param name="orderBy">排序</param>
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
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <returns></returns>
        public T querySingle<T>(string columns, string selection, string selectionArgs, string orderBy, bool isLinkedServer) where T : new()
        {
            List<T> list = query<T>(columns, selection, selectionArgs, orderBy);
            if (list.Count > 0)
            {
                return list[0];
            }
            return default(T);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public T querySingle<T>(string columns, string selection, string selectionArgs, string orderBy) where T : new()
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
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having">having</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        private DataTable queryDT<T>(string[] columns,
      string selection, string[] selectionArgs, string groupBy,
      string having, string orderBy) where T : new()
        {
            string strSql = UnSqlStr.getQuerySql<T>(columns, selection, selectionArgs, groupBy, having, orderBy, false);
            return help.getDataTable(strSql);
        }

        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having">having</param>
        /// <param name="orderBy">排序</param>
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
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
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
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="orderBy">排序</param>
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
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
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
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int? queryScalarToInt(String sql, SqlParameter[] parms)
        {
            try
            {
                return Convert.ToInt32(queryScalar(sql, parms));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 查询第一行第一个值并转为Stringt
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public String queryScalarToString(String sql, SqlParameter[] parms)
        {
            try
            {
                return queryScalar(sql, parms).ToString();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 修改数据

        /// <summary>
        /// 条件修改(核心)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="isXactAbort"></param>
        /// <returns></returns>
        public int? update<T>(T t, string columns, string selection, string[] selectionArgs, bool isXactAbort) where T : new()
        {
            // 实体属性参数化
            SqlParameter[] SqlPmtA = UnSqlStr.getSqlPmtA<T>(t, columns);
            StringBuilder strSql = new StringBuilder();
            if (isXactAbort)
            {
                strSql.Append("Set xact_abort ON;");
            }
            strSql.Append("Update " + UnToGen.getTableName(typeof(T), isXactAbort) + " Set " + UnSqlStr.getUpdStr(SqlPmtA) + " ");

            // 构造参数化查询
            object[] objs = toParsSelection(selection, selectionArgs);
            string where = (string)objs[0];
            if (where.Length > 0)
            {
                strSql.Append("Where " + where);
            }

            // 属性参数和条件参数合并
            List<SqlParameter> list = new List<SqlParameter>();
            list.AddRange(SqlPmtA);
            list.AddRange((SqlParameter[])objs[1]);

            return help.exSql(strSql.ToString(), list.ToArray());
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
        public int? update<T>(T t, string columns, string selection, string[] selectionArgs) where T : new()
        {
            return update(t, columns, selection, selectionArgs, false);
        }

        /// <summary>
        /// 条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="isXactAbort"></param>
        /// <returns></returns>
        public int? update<T>(T t, string columns, string selection, string selectionArgs, bool isXactAbort) where T : new()
        {
            string[] args = null;
            if (selectionArgs != null)
            {
                args = selectionArgs.Split(',');
            }
            return update<T>(t, columns, selection, args, isXactAbort);
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
        public int? update<T>(T t, string columns, string selection, string selectionArgs) where T : new()
        {
            string[] args = null;
            if (selectionArgs != null)
            {
                args = selectionArgs.Split(',');
            }
            return update<T>(t, columns, selection, args, false);
        }

        #endregion

        #region 提交事务

        /// <summary>
        /// 提交事务
        /// </summary>
        public void commit()
        {
            help.commit();
        }

        #endregion

    }
}
