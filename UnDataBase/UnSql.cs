using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using UnCommon.Tool;
using UnCommon.Config;
using ServiceStack.Redis;
using UnCommon.Extend;
using UnCommon.XMMP;
using System.Text.RegularExpressions;
using System.IO;
using UnCommon.Files;

namespace UnDataBase
{
    /// <summary>
    /// SQL操作类
    /// </summary>
    public class UnSql
    {
        #region 私有变量/方法

        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private UnSqlHelpU help = null;

        /// <summary>
        /// Redis缓存对象
        /// </summary>
        private RedisClient redis = null;

        /// <summary>
        /// Redis键名前缀
        /// </summary>
        private string redisKeyPre = "UnSql_";

        /// <summary>
        /// 默认
        /// </summary>
        private bool _isRemoveRedis = false;

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

        /// <summary>
        /// 替换掉参数化变量
        /// </summary>
        private Regex regParms = new Regex(@"@p\d*_\d*");

        /// <summary>
        /// 获取SQL的MD5值
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        private string getSqlMD5(string strSql, SqlParameter[] parms)
        {
            string s = strSql;
            s = regParms.Replace(s, "");
            if (parms != null)
            {
                foreach (var parm in parms)
                {
                    s += parm.Value.ToString();
                }
            }
            return s.md5Hash16();
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
        /// <param name="trans">是否事务</param>
        /// <param name="redisClient">Redis</param>
        private void init(string ip, string port, string user, string pass, string dbName, bool trans, RedisClient redisClient)
        {
            this.redis = redisClient;
            string constr1 = "Data Source=" + ip + "," + port + ";Initial Catalog=master;User ID=" + user + ";Password=" + pass + ";";
            string constr2 = "Data Source=" + ip + "," + port + ";Initial Catalog=" + dbName + ";User ID=" + user + ";Password=" + pass + ";";
            this.redisKeyPre = redisKeyPre + constr2.md5Hash16() + "_";
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
        public UnSql(string ip, string port, string user, string pass,string dbName)
        {
            init(ip, port, user, pass, dbName, false, null);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="user">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="dbName">连接的数据库</param>
        /// <param name="redisClient">Redis</param>
        public UnSql(string ip, string port, string user, string pass,string dbName,RedisClient redisClient)
        {
            init(ip, port, user, pass, dbName,false, redisClient);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="user">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="dbName">连接的数据库</param>
        /// <param name="trans">是否开启事务</param>
        public UnSql(string ip, string port, string user, string pass,string dbName,bool trans)
        {
            init(ip, port, user, pass, dbName,trans, null);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="user">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="dbName">连接的数据库</param>
        /// <param name="trans">是否开启事务</param>
        /// <param name="redisClient">Redis</param>
        public UnSql(string ip, string port, string user, string pass, string dbName, bool trans, RedisClient redisClient)
        {
            init(ip, port, user, pass, dbName, trans, redisClient);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="constr">连接字符串</param>
        /// <param name="trans">是否开启事务</param>
        /// <param name="redisClient">Redis</param>
        public UnSql(string constr, bool trans, RedisClient redisClient)
        {
            this.redis = redisClient;
            this.redisKeyPre = redisKeyPre + constr.md5Hash16() + "_";
            help = new UnSqlHelpU(constr, trans);
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

        #region 库/表/字段方法

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public int? createDataBase(string dbPath, string dbName)
        {
            string s1 = UnSqlStr.createDB(dbPath, dbName);
            return help.exSql(s1);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="t">表数据对象</param>
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
            if (dropColumn(t) == null)
            {
                return false;
            }
            if (addColumn(t) == null)
            {
                return false;
            }
            if (alterColumn(t) == null)
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

        #endregion

        #region 基础方法

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int? exSql(string sql)
        {
            return help.exSql(sql);
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

        #endregion

        #region 添加数据

        /// <summary>
        /// 添加一条记录(核心)
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="t">表数据对象</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <param name="isRemoveRedis">是否清除表缓存</param>
        /// <returns>非嵌套事务返回自增ID,嵌套事务返回受影响行数</returns>
        public long insert<T>(T t, bool isXactAbort, bool? isRemoveRedis) where T : new()
        {
            SqlParameter[] SqlPmtA = UnSqlStr.getSqlPmtA<T>(t);
            StringBuilder strSql = new StringBuilder();
            if (isXactAbort)
            {
                strSql.Append("Set xact_abort ON;");
            }
            strSql.Append("Insert Into " + UnToGen.getTableName(typeof(T), isXactAbort) + " ");
            strSql.Append(UnSqlStr.getAddStr<T>(SqlPmtA));
            if (!isXactAbort)
            {
                strSql.Append(" Select SCOPE_IDENTITY() As KeyID");
                object obj = help.getExSc(strSql.ToString(), SqlPmtA);
                if (obj == null)
                {
                    return -1;
                }
                // 清除表缓存
                removeTableRedis(typeof(T), isRemoveRedis);
                return Convert.ToInt64(obj);
            }
            int? i = help.exSql(strSql.ToString(), SqlPmtA);
            if (i == null)
            {
                return -1;
            }
            // 清除表缓存
            removeTableRedis(typeof(T), isRemoveRedis);
            return i.Value;
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="t">表数据对象</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <returns>非嵌套事务返回自增ID,嵌套事务返回受影响行数</returns>
        public long insert<T>(T t, bool isXactAbort) where T : new()
        {
            return insert<T>(t, isXactAbort, null);
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

        #region 修改数据

        /// <summary>
        /// 条件修改(核心)
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="t">表数据对象</param>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否链接服务顺</param>
        /// <param name="isRemoveRedis">是否清除缓存</param>
        /// <returns></returns>
        private int? update<T>(T t, string columns, string selection, string[] selectionArgs, bool isXactAbort, bool? isRemoveRedis) where T : new()
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
            // 清除表缓存
            removeTableRedis(typeof(T), isRemoveRedis);
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
        /// <param name="isXactAbort"></param>
        /// <returns></returns>
        public int? update<T>(T t, string columns, string selection, string[] selectionArgs, bool isXactAbort) where T : new()
        {
            return update<T>(t, columns, selection, selectionArgs, isXactAbort, null);
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
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="t">表数据对象</param>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否链接服务器</param>
        /// <param name="isRemoveRedis">是否清除表缓存</param>
        /// <returns></returns>
        public int? update<T>(T t, string columns, string selection, string selectionArgs, bool isXactAbort, bool? isRemoveRedis) where T : new()
        {
            string[] args = null;
            if (selectionArgs != null)
            {
                args = selectionArgs.Split(',');
            }
            return update<T>(t, columns, selection, args, isXactAbort, isRemoveRedis);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="t">表数据对象</param>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否链接服务器</param>
        /// <returns></returns>
        public int? update<T>(T t, string columns, string selection, string selectionArgs, bool isXactAbort) where T : new()
        {
            return update<T>(t, columns, selection, selectionArgs, isXactAbort, null);
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

        #region 删除数据

        /// <summary>
        /// 删除数据(核心)
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <param name="isRemoveRedis">是否清除缓存</param>
        /// <returns></returns>
        private int? delete<T>(string selection, string[] selectionArgs, bool isXactAbort, bool? isRemoveRedis) where T : new()
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
            // 清除表缓存
            removeTableRedis(typeof(T), isRemoveRedis);
            return help.exSql(strSql.ToString(), (SqlParameter[])objs[1]);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <returns></returns>
        public int? delete<T>(string selection, string[] selectionArgs, bool isXactAbort) where T : new()
        {
            return delete<T>(selection, selectionArgs, isXactAbort, null);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <param name="isRemoveRedis">是否清除缓存</param>
        /// <returns></returns>
        public int? delete<T>(string selection, string selectionArgs, bool isXactAbort, bool? isRemoveRedis) where T : new()
        {
            string[] args = null;
            if (selectionArgs != null)
            {
                args = selectionArgs.Split(',');
            }
            return delete<T>(selection, args, isXactAbort, isRemoveRedis);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">表对应泛型</typeparam>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="isXactAbort">是否嵌套事务</param>
        /// <returns></returns>
        public int? delete<T>(string selection, string selectionArgs, bool isXactAbort) where T : new()
        {
            return delete<T>(selection, selectionArgs, isXactAbort, null);
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

        #region 查询返回泛型

        /// <summary>
        /// 查询实体(核心方法)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parms">参数</param>
        /// <param name="cacheExpire">
        /// >0 = 缓存时间(秒)
        /// -1 = 清除缓存
        /// </param>
        /// <returns>返回查询泛型数组,没有数据返回0条,cacheExpire = -1时返回NULL</returns>
        private List<T> query<T>(string strSql, SqlParameter[] parms, int? cacheExpire) where T : new()
        {
            // 缓存key
            string cacheKey = null;

            // 有缓存则返回缓存
            if (redis != null && cacheExpire != null)
            {
                cacheKey = getQueryKeyPre(typeof(T)) + getSqlMD5(strSql, parms);

                // 删除缓存
                if (cacheExpire.Value == -1)
                {
                    redis.Remove(cacheKey);
                    return null;
                }

                // 获取缓存
                var list1 = getRedis<List<T>>(cacheKey, cacheExpire);
                if (list1 != null)
                {
                    return list1;
                }
            }

            // 查询数据
            List<T> list = new List<T>();
            DataTable dt = help.getDataTable(strSql, parms);
            if (dt != null)
            {
                list = UnToGen.dtToT<T>(dt);
            }

            // 设置缓存
            setRedis(cacheKey, cacheExpire, list);
            return list;
        }

        /// <summary>
        /// 参数化获取实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件-Where ID={0}</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having"></param>
        /// <param name="orderBy">排序</param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <param name="cacheExpire">缓存时间(秒)</param>
        /// <returns>返回查询泛型数组,没有数据返回0条</returns>
        private List<T> query<T>(string[] columns, string selection, string[] selectionArgs, string groupBy, string having, string orderBy, bool isLinkedServer, int? cacheExpire) where T : new()
        {
            // 构造参数化查询
            object[] objs = toParsSelection(selection, selectionArgs);
            string strSql = UnSqlStr.getQuerySql<T>(columns, (string)objs[0], null, groupBy, having, orderBy, isLinkedServer);
            return query<T>(strSql, (SqlParameter[])objs[1], cacheExpire);
        }

        /// <summary>
        /// 参数化获取实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件-Where ID={0}</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having"></param>
        /// <param name="orderBy">排序</param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <param name="cacheExpire">缓存时间(秒)</param>
        /// <returns>返回查询泛型数组,没有数据返回0条</returns>
        private List<T> query<T>(string columns, string selection, string[] selectionArgs, string groupBy, string having, string orderBy, bool isLinkedServer, int? cacheExpire) where T : new()
        {
            string[] cs = null;
            if (columns != null)
            {
                cs = columns.Split(',');
            }
            return query<T>(cs, selection, selectionArgs, groupBy, having, orderBy, isLinkedServer, cacheExpire);
        }

        /// <summary>
        /// 参数化获取实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">字段</param>
        /// <param name="selection">条件-Where ID={0}</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">分组</param>
        /// <param name="having"></param>
        /// <param name="orderBy">排序</param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <param name="cacheExpire">缓存时间(秒)</param>
        /// <returns>返回查询泛型数组,没有数据返回0条</returns>
        private List<T> query<T>(string columns, string selection, string selectionArgs, string groupBy, string having, string orderBy, bool isLinkedServer, int? cacheExpire) where T : new()
        {
            string[] ss = null;
            if (selectionArgs != null)
            {
                ss = selectionArgs.Split(',');
            }
            return query<T>(columns, selection, ss, groupBy, having, orderBy, isLinkedServer, cacheExpire);
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
        private List<T> query<T>(string[] columns, string selection, string[] selectionArgs, string groupBy, string having, string orderBy) where T : new()
        {
            return query<T>(columns, selection, selectionArgs, groupBy, having, orderBy, false, null);
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
            return query<T>(columns, selection, selectionArgs, null, null, orderBy, isLinkedServer, null);
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
            return query<T>(columns, selection, selectionArgs, null, null, orderBy, false, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <param name="orderBy"></param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <param name="cacheExpire">缓存时间(秒)</param>
        /// <returns>返回泛型对象,无数据则为NULL</returns>
        public List<T> query<T>(string columns, string selection, string selectionArgs, string orderBy, bool isLinkedServer, int? cacheExpire) where T : new()
        {
            return query<T>(columns, selection, selectionArgs, null, null, orderBy, isLinkedServer, cacheExpire);
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
        public List<T> query<T>(string columns, string selection, string selectionArgs, string groupBy, string having, string orderBy) where T : new()
        {
            return query<T>(columns, selection, selectionArgs, groupBy, having, orderBy, false, null);
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
            List<T> list = query<T>(columns, selection, selectionArgs, orderBy, isLinkedServer);
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
            return querySingle<T>(columns, selection, selectionArgs, orderBy, false);
        }

        #endregion

        #region 查询返回DataTable

        /// <summary>
        /// 查询数据(核心)
        /// </summary>
        /// <param name="strSql">sql</param>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public DataTable queryDT(string strSql, SqlParameter[] parms)
        {
            return help.getDataTable(strSql, parms);
        }

        /// <summary>
        /// 查询数据(核心)
        /// </summary>
        /// <param name="strSql">sql</param>
        /// <param name="parms">参数</param>
        /// <param name="cacheExpire">
        /// >0 = 缓存时间
        /// </param>
        /// <returns>返回DataTable</returns>
        public DataTable queryDT<T>(string strSql, SqlParameter[] parms, int? cacheExpire)
        {
            // 缓存key
            string cacheKey = null;

            // 有缓存则返回缓存
            if (redis != null && cacheExpire != null)
            {
                cacheKey = getQueryDTKeyPre(typeof(T)) + getSqlMD5(strSql, parms);

                // 删除缓存
                if (cacheExpire.Value == -1)
                {
                    redis.Remove(cacheKey);
                    return null;
                }

                // 获取缓存
                string xml = getRedis<string>(cacheKey, cacheExpire);
                if (xml != null)
                {
                    return (DataTable)UnXMMPXml.xmlToT(typeof(DataTable), xml);
                }
            }

            // 查询数据
            DataTable dt = help.getDataTable(strSql, parms);

            // 须转为XML存储
            string xml1 = UnXMMPXml.tToXml(typeof(DataTable), dt);
            // 设置缓存
            setRedis(cacheKey, cacheExpire, xml1);
            return dt;
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
        private DataTable queryDT<T>(string[] columns, string selection, string[] selectionArgs, string groupBy, string having, string orderBy) where T : new()
        {
            string strSql = UnSqlStr.getQuerySql<T>(columns, selection, selectionArgs, groupBy, having, orderBy, false);
            return queryDT(strSql, null);
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
        public DataTable queryDT<T>(string columns, string selection, string selectionArgs, string groupBy, string having, string orderBy) where T : new()
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

        #endregion

        #region 查询返回UnSqlPage

        /// <summary>
        /// 尝试将DataTable转为T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        private void tryPageToT<T>(UnSqlPage page) where T : new()
        {
            // 转实体
            if (page.DataSource != null && page.DataSource.Rows.Count > 0)
            {
                page.TSource = UnToGen.dtToT<T>(page.DataSource);
            }
        }

        /// <summary>
        /// 获取翻页数据(核心)
        /// </summary>
        /// <param name="columns">字段</param>
        /// <param name="keyName">主键名</param>
        /// <param name="table">表名或联合表名</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="cacheExpire">
        /// >0 = 缓存时间(秒)
        /// -1 = 清除表所有缓存
        /// -2 = 清除对应页数缓存
        /// -3 = 清除所有页缓存
        /// </param>
        /// <returns>返回DataSource:数据集,CurrentPage:当前页码,PageSize:每页大小,TotalNumber:总记录数,TotalPages:总页数</returns>
        public UnSqlPage queryPage<T>(string columns, string keyName, string table, string where, string order, int currentPage, int pageSize, int? cacheExpire) where T : new()
        {
            // 缓存key
            string cacheKey = null;
            // 有缓存则返回缓存
            if (redis != null && cacheExpire != null)
            {
                // 只包含条件
                string cacheKeyPre = getQueryPageKeyPre(typeof(T)) + (columns + keyName + table + where + order).md5Hash16() + "_";
                // 包含页码及页数
                cacheKey = cacheKeyPre + pageSize + "_" + currentPage;

                // 清除表所有缓存
                if (cacheExpire.Value == -1)
                {
                    getRedisKeyPre(typeof(T));
                    return new UnSqlPage();
                }
                // 清除条件所有页缓存
                if (cacheExpire.Value == -2)
                {
                    removeByRegex("^" + cacheKeyPre + ".*");
                    return new UnSqlPage();
                }
                // 清除条件对应页缓存
                if (cacheExpire.Value == -3)
                {
                    redis.Remove(cacheKey);
                    return new UnSqlPage();
                }

                // 获取缓存
                string xml = getRedis< string>(cacheKey, cacheExpire);
                if (xml != null)
                {
                    UnSqlPage page0 = (UnSqlPage)UnXMMPXml.xmlToT(typeof(UnSqlPage), xml);
                    tryPageToT<T>(page0);
                    return page0;
                }
            }

            // 查询数据
            UnSqlPage page = help.getPage(columns, keyName, table, where, order, currentPage, pageSize);

            // 须转为XML存储
            string xml1 = UnXMMPXml.tToXml(typeof(UnSqlPage), page);
            // 设置缓存
            setRedis(cacheKey, cacheExpire, xml1);
            // 转实体
            tryPageToT<T>(page);
            return page;
        }

        /// <summary>
        /// 获取翻页数据
        /// </summary>
        /// <param name="columns">字段</param>
        /// <param name="keyName">主键名</param>
        /// <param name="table">表名或联合表名</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public UnSqlPage queryPage(string columns, string keyName, string table, string where, string order, int currentPage, int pageSize)
        {
            return queryPage<UnSqlPage>(columns, keyName, table, where, order, currentPage, pageSize, null);
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
        /// <param name="cacheExpire">缓存时间(秒)</param>
        /// <returns>返回DataSource:数据集,CurrentPage:当前页码,PageSize:每页大小,TotalNumber:总记录数,TotalPages:总页数</returns>
        public UnSqlPage queryPage<T>(string columns, string where, string[] whereArgs, string order, int currentPage, int pageSize, int? cacheExpire) where T : new()
        {
            string keyName = UnToGen.getAutoNum(typeof(T), true);
            string table = UnToGen.getTableName(typeof(T));
            where = UnSqlStr.getQueryPageWhere<T>(where, whereArgs);
            return queryPage<T>(columns, keyName, table, where, order, currentPage, pageSize, cacheExpire);
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
            return queryPage<T>(columns, where, whereArgs, order, currentPage, pageSize, null);
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
        /// <param name="cacheExpire">缓存时间(秒)</param>
        /// <returns>返回DataSource:数据集,CurrentPage:当前页码,PageSize:每页大小,TotalNumber:总记录数,TotalPages:总页数</returns>
        public UnSqlPage queryPage<T>(string columns, string where, string whereArgs, string order, int currentPage, int pageSize, int? cacheExpire) where T : new()
        {
            string[] args = null;
            if (whereArgs != null && whereArgs.Length > 0)
            {
                args = whereArgs.Split(',');
            }
            return queryPage<T>(columns, where, args, order, currentPage, pageSize, cacheExpire);
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
            return queryPage<T>(columns, where, whereArgs, order, currentPage, pageSize, null);
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
            return queryPage<T>(columns, null, (string)null, order, currentPage, pageSize);
        }

        #endregion

        #region 查询返回第1行第1列值

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

        #region 提交事务

        /// <summary>
        /// 提交事务
        /// </summary>
        public void commit()
        {
            help.commit();
        }

        #endregion

        #region Redis缓存

        /// <summary>
        /// REDIS错误日志打印状态
        /// </summary>
        private static bool redisLogLock = false;

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheExpire"></param>
        /// <param name="t"></param>
        private void setRedis<T>(string cacheKey, int? cacheExpire, T t)
        {
            try
            {
                // 设置缓存
                if (redis != null && cacheKey != null && cacheExpire.Value > 0 && t != null)
                {
                    redis.Set(cacheKey, t);
                    redis.Expire(cacheKey, cacheExpire.Value);
                }
            }
            catch (Exception e)
            {
                if(redisLogLock == false)
                {
                    redisLogLock = true;
                    UnFile.writeLog("setRedis", e.ToString() + "\r\ncacheKey:" + cacheKey + "\r\ncacheExpire:" + cacheExpire);
                    redisLogLock = false;
                }
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheExpire"></param>
        /// <returns></returns>
        private T getRedis<T>(string cacheKey, int? cacheExpire)
        {
            try
            {
                // 如果键存在
                if (redis != null && redis.Exists(cacheKey) == 1 && cacheExpire.Value > 0)
                {
                    // 尝试读取缓存
                    return redis.Get<T>(cacheKey);
                }
            }
            catch (Exception e)
            {
                if (redisLogLock == false)
                {
                    redisLogLock = true;
                    if (redis != null && redis.Exists(cacheKey) == 1)
                    {
                        redis.Remove(cacheKey);
                    }
                    UnFile.writeLog("getRedis", e.ToString() + "\r\ncacheKey:" + cacheKey + "\r\ncacheExpire:" + cacheExpire);
                    redisLogLock = false;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获取Key前缀
        /// </summary>
        /// <param name="t">表对应泛型</param>
        /// <returns></returns>
        private string getRedisKeyPre(Type t)
        {
            return redisKeyPre + UnToGen.getTableName(t) + "_";
        }

        /// <summary>
        /// 获取Query方法Key前缀
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private string getQueryKeyPre(Type t)
        {
            return getRedisKeyPre(t) + "Query_";
        }

        /// <summary>
        /// 获取QueryPage方法Key前缀
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private string getQueryPageKeyPre(Type t)
        {
            return getRedisKeyPre(t) + "QueryPage_";
        }

        /// <summary>
        /// 获取QueryDT方法Key前缀
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private string getQueryDTKeyPre(Type t)
        {
            return getRedisKeyPre(t) + "QueryDT_";
        }

        /// <summary>
        /// 匹配所有键
        /// </summary>
        /// <param name="pattern">正则</param>
        /// <returns></returns>
        private List<string> getRedisKeysByPattern(string pattern)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex(pattern);
            var keys = redis.GetAllKeys();
            foreach (var item in keys)
            {
                if (regex.IsMatch(item))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="pattern">正则</param>
        private void removeByRegex(string pattern)
        {
            redis.RemoveAll(getRedisKeysByPattern(pattern));
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void removeAllRedis()
        {
            if (redis != null)
            {
                removeByRegex("^" + redisKeyPre + ".*");
            }
        }

        /// <summary>
        /// 清除Query方法缓存
        /// </summary>
        /// <param name="t">表对应实体类型</param>
        public void removeQueryRedis(Type t)
        {
            if (redis != null)
            {
                removeByRegex("^" + getQueryKeyPre(t) + ".*");
            }
        }

        /// <summary>
        /// 清除QueryPage方法缓存 
        /// </summary>
        /// <param name="t"></param>
        public void removeQueryPageRedis(Type t)
        {
            if (redis != null)
            {
                removeByRegex("^" + getQueryPageKeyPre(t) + ".*");
            }
        }

        /// <summary>
        /// 清除QueryDT方法缓存
        /// </summary>
        /// <param name="t"></param>
        public void removeQueryDTRedis(Type t)
        {
            if (redis != null)
            {
                removeByRegex("^" + getQueryDTKeyPre(t) + ".*");
            }
        }

        /// <summary>
        /// 清除表所有缓存
        /// </summary>
        /// <param name="t">表对应泛型</param>
        /// <param name="isRemoveRedis">是否清除缓存</param>
        private void removeTableRedis(Type t, bool? isRemoveRedis)
        {
            if (isRemoveRedis == null)
            {
                isRemoveRedis = _isRemoveRedis;
            }
            if (isRemoveRedis == true)
            {
                removeQueryRedis(t);
                removeQueryPageRedis(t);
                removeQueryDTRedis(t);
            }
        }

        /// <summary>
        /// 清除表所有缓存
        /// </summary>
        /// <param name="t">表对应泛型</param>
        public void removeTableRedis(Type t)
        {
            removeQueryRedis(t);
            removeQueryPageRedis(t);
            removeQueryDTRedis(t);
        }

        #endregion
    }
}
