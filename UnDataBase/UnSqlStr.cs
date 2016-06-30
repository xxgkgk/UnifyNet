using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using UnCommon;
using System.Reflection;
using System.Text;
using UnCommon.Tool;
using UnCommon.Entity;

namespace UnDataBase
{
    /// <summary>
    /// SQL语句组装
    /// </summary>
    public class UnSqlStr
    {
        #region 创建及删除

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <param name="model">创建模式</param>
        /// <returns></returns>
        public static string createDB(string dbName, UnSqlConnectModel model)
        {
            StringBuilder sb = new StringBuilder(); ;
            switch (model)
            {
                case UnSqlConnectModel.Create:
                    sb.AppendLine("Use master;");
                    sb.AppendLine("If Exists(Select * From sysdatabases Where name = '" + dbName + "') ");
                    sb.AppendLine("Drop DataBase " + dbName + ";");
                    sb.Append("Create DataBase " + dbName);
                    break;
                case UnSqlConnectModel.ConnectOrCreate:
                    sb.AppendLine("Use master;");
                    sb.AppendLine("If Not Exists(Select * From sysdatabases Where name = '" + dbName + "') ");
                    sb.Append("Create DataBase " + dbName);
                    break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 建表基本SQL
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string createTableBase(Type t)
        {
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            StringBuilder sb = new StringBuilder();
            if (length <= 0)
            {
                return null;
            }
            //取类上的自定义特性
            UnAttrSql classAttr = null;
            object[] objs = t.GetCustomAttributes(typeof(UnAttrSql), true);
            foreach (object obj in objs)
            {
                classAttr = obj as UnAttrSql;
            }
            string tableName = t.Name;
            if (classAttr != null && classAttr.tableName != null)
            {
                tableName = classAttr.tableName;
            }
            sb.AppendLine("If Not Exists (Select * From dbo.sysobjects Where name='" + tableName + "')");
            sb.AppendLine("Create Table " + tableName + "(");
            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                if (UnToGen.isField(item))
                {
                    UnAttrSql attr = null;
                    object[] objAttrs = item.GetCustomAttributes(typeof(UnAttrSql), true);
                    if (objAttrs.Length > 0)
                    {
                        attr = objAttrs[0] as UnAttrSql;
                    }
                    // 字段修饰
                    if (attr == null || attr.fieldType == null)
                    {
                        attr = new UnAttrSql();
                        attr.fieldNULL = true;
                        Type type = item.PropertyType;
                        if (type.Equals(typeof(String)))
                        {
                            attr.fieldType = "varchar(8000)";
                        }
                        else if (type.Equals(typeof(Int32)))
                        {
                            attr.fieldType = "int";
                        }
                        else if (type.Equals(typeof(Int64)))
                        {
                            attr.fieldType = "bigint";
                        }
                        else if (type.Equals(typeof(Boolean)))
                        {
                            attr.fieldType = "bit";
                        }
                        else if (type.Equals(typeof(DateTime)))
                        {
                            attr.fieldType = "datetime";
                        }
                        else if (type.Equals(typeof(Decimal)))
                        {
                            attr.fieldType = "decimal";
                        }
                        else if (type.Equals(typeof(Guid)))
                        {
                            attr.fieldType = "uniqueidentifier";
                        }
                        else
                        {
                        }
                    }
                    string nullStr = "";
                    string defaultStr = "";
                    if (!attr.fieldNULL)
                    {
                        nullStr = "Not NULL";
                    }
                    if (attr.fieldDefault != null)
                    {
                        defaultStr = "Default(" + defaultStr + ")";
                    }
                    sb.AppendLine(name + " " + attr.fieldType + " " + defaultStr + " " + nullStr + ",");
                }
            }
            sb.AppendLine(");");
            return sb.ToString();
        }

        /// <summary>
        /// 建表关联SQL
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string createTableRelation(Type t)
        {
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            StringBuilder sb = new StringBuilder();
            if (length <= 0)
            {
                return null;
            }
            //取类上的自定义特性
            UnAttrSql classAttr = null;
            object[] objs = t.GetCustomAttributes(typeof(UnAttrSql), true);
            foreach (object obj in objs)
            {
                classAttr = obj as UnAttrSql;
            }
            string tableName = t.Name;
            if (classAttr != null && classAttr.tableName != null)
            {
                tableName = classAttr.tableName;
            }
            Dictionary<string, StringBuilder> dicSql = new Dictionary<string, StringBuilder>();
            dicSql.Add("con_PrimarykKey", new StringBuilder());
            dicSql.Add("con_ForeignKey", new StringBuilder());
            dicSql.Add("con_UniQue", new StringBuilder());
            dicSql.Add("con_Default", new StringBuilder());
            dicSql.Add("con_Check", new StringBuilder());
            dicSql.Add("ind_Clustered", new StringBuilder());
            dicSql.Add("ind_Nonclustered", new StringBuilder());
            dicSql.Add("ind_Unique", new StringBuilder());

            Dictionary<string, StringBuilder> dicNames = new Dictionary<string, StringBuilder>();
            dicNames.Add("con_PrimarykKey", new StringBuilder());
            dicNames.Add("ind_Clustered", new StringBuilder());
            dicNames.Add("ind_Nonclustered", new StringBuilder());
            dicNames.Add("ind_Unique", new StringBuilder());

            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                string keyName = string.Empty;
                if (UnToGen.isField(item))
                {
                    UnAttrSql attr = null;
                    object[] objAttrs = item.GetCustomAttributes(typeof(UnAttrSql), true);
                    if (objAttrs.Length > 0)
                    {
                        attr = objAttrs[0] as UnAttrSql;
                    }
                    // 关系SQL创建
                    if (attr != null)
                    {
                        // 约束关系
                        switch (attr.constraintModel)
                        {
                            case ConstraintModel.PrimaryKey:
                                keyName = "CON_PK_" + name;
                                if (dicSql["con_PrimarykKey"].Length == 0)
                                {
                                    dicSql["con_PrimarykKey"].Append("Alter Table " + tableName + " Add Constraint " + keyName + " Primary Key(");
                                }
                                dicNames["con_PrimarykKey"].Append(name + ",");
                                break;
                            case ConstraintModel.ForeignKey:
                                if (attr.value != null)
                                {
                                    var value = (string[])attr.value;
                                    keyName = "CON_FK_" + value[0];
                                    dicSql["con_ForeignKey"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Foreign Key (" + name + ") References " + value[0] + "(" + value[1] + ");");
                                }
                                break;
                            case ConstraintModel.Unique:
                                keyName = "CON_UQ_" + name;
                                dicSql["con_UniQue"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Unique(" + name + ");");
                                break;
                            case ConstraintModel.Default:
                                keyName = "CON_DE_" + name;
                                if (attr.value != null)
                                {
                                    var value = (string)attr.value;
                                    dicSql["con_Default"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Default(" + value + ") For " + name + ";");
                                }
                                break;
                            case ConstraintModel.Check:
                                keyName = "CON_CH_" + name;
                                if (attr.value != null)
                                {
                                    var value = (string)attr.value;
                                    dicSql["con_Check"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Check(" + value + ");");
                                }
                                break;
                        }
                        // 索引关系
                        switch (attr.indexModel)
                        {
                            case IndexModel.Clustered:// 聚集索引
                                keyName = "IND_CL_" + name;
                                if (dicSql["ind_Clustered"].Length == 0)
                                {
                                    dicSql["ind_Clustered"].Append("Create Index " + keyName + " On " + tableName + "(");
                                }
                                dicNames["ind_Clustered"].Append(name + ",");
                                break;
                            case IndexModel.Nonclustered:// 非聚集索引
                                keyName = "IND_NC_" + name;
                                if (dicSql["ind_Nonclustered"].Length == 0)
                                {
                                    dicSql["ind_Nonclustered"].Append("Create Nonclustered Index " + keyName + " On " + tableName + "(");
                                }
                                dicNames["ind_Nonclustered"].Append(name + ",");
                                break;
                            case IndexModel.Unique:// 唯一索引
                                keyName = "IND_UN_" + name;
                                if (dicSql["ind_Unique"].Length == 0)
                                {
                                    dicSql["ind_Unique"].Append("Create Unique Index " + keyName + " On " + tableName + "(");
                                }
                                dicNames["ind_Unique"].Append(name + ",");
                                break;
                        }
                    }
                }
            }

            // 组合多字段约束/索引
            foreach (var item in dicNames)
            {
                if (item.Value.Length > 0)
                {
                    dicSql[item.Key].Append(dicNames[item.Key].ToString().TrimEnd().TrimEnd(',') + ");");
                }
            }

            // 添加语句
            foreach (var item in dicSql)
            {
                if (item.Value.Length > 0)
                {
                    sb.AppendLine(item.Value.ToString());
                }
            }
            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 删除关系SQL
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string dropTableRelation(Type t)
        {
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int length = properties.Length;
            StringBuilder sb = new StringBuilder();
            if (length <= 0)
            {
                return null;
            }
            //取类上的自定义特性
            UnAttrSql classAttr = null;
            object[] objs = t.GetCustomAttributes(typeof(UnAttrSql), true);
            foreach (object obj in objs)
            {
                classAttr = obj as UnAttrSql;
            }
            string tableName = t.Name;
            if (classAttr != null && classAttr.tableName != null)
            {
                tableName = classAttr.tableName;
            }
            Dictionary<string, StringBuilder> dicSql = new Dictionary<string, StringBuilder>();
            dicSql.Add("con_PrimarykKey", new StringBuilder());
            dicSql.Add("con_ForeignKey", new StringBuilder());
            dicSql.Add("con_UniQue", new StringBuilder());
            dicSql.Add("con_Default", new StringBuilder());
            dicSql.Add("con_Check", new StringBuilder());
            dicSql.Add("ind_Clustered", new StringBuilder());
            dicSql.Add("ind_Nonclustered", new StringBuilder());
            dicSql.Add("ind_Unique", new StringBuilder());

            Dictionary<string, StringBuilder> dicNames = new Dictionary<string, StringBuilder>();
            dicNames.Add("con_PrimarykKey", new StringBuilder());
            dicNames.Add("ind_Clustered", new StringBuilder());
            dicNames.Add("ind_Nonclustered", new StringBuilder());
            dicNames.Add("ind_Unique", new StringBuilder());

            for (int i = 0; i < length; i++)
            {
                PropertyInfo item = properties[i];
                string name = item.Name;
                string keyName = string.Empty;
                if (UnToGen.isField(item))
                {
                    UnAttrSql attr = null;
                    object[] objAttrs = item.GetCustomAttributes(typeof(UnAttrSql), true);
                    if (objAttrs.Length > 0)
                    {
                        attr = objAttrs[0] as UnAttrSql;
                    }
                    // 关系SQL创建
                    if (attr != null)
                    {
                        // 约束关系
                        switch (attr.constraintModel)
                        {
                            case ConstraintModel.PrimaryKey:
                                keyName = "CON_PK_" + name;
                                dicSql["con_PrimarykKey"].Append(dropConstraints(tableName, keyName));
                                break;
                            case ConstraintModel.ForeignKey:
                                if (attr.value != null)
                                {
                                    var value = (string[])attr.value;
                                    keyName = "CON_FK_" + value[0];
                                    dicSql["con_ForeignKey"].AppendLine(dropConstraints(tableName, keyName).ToString());
                                }
                                break;
                            case ConstraintModel.Unique:
                                keyName = "CON_UQ_" + name;
                                dicSql["con_UniQue"].AppendLine(dropConstraints(tableName, keyName).ToString());
                                break;
                            case ConstraintModel.Default:
                                keyName = "CON_DE_" + name;
                                if (attr.value != null)
                                {
                                    var value = (string)attr.value;
                                    dicSql["con_Default"].AppendLine(dropConstraints(tableName, keyName).ToString());
                                }
                                break;
                            case ConstraintModel.Check:
                                keyName = "CON_CH_" + name;
                                if (attr.value != null)
                                {
                                    var value = (string)attr.value;
                                    dicSql["con_Check"].AppendLine(dropConstraints(tableName, keyName).ToString());
                                }
                                break;
                        }
                        // 索引关系
                        switch (attr.indexModel)
                        {
                            case IndexModel.Clustered:// 聚集索引
                                keyName = tableName + ".IND_CL_" + name;
                                dicSql["ind_Clustered"].Append(dropIndex(tableName, keyName));
                                break;
                            case IndexModel.Nonclustered:// 非聚集索引
                                keyName = "IND_NC_" + name;
                                dicSql["ind_Clustered"].Append(dropIndex(tableName, keyName));
                                break;
                            case IndexModel.Unique:// 唯一索引
                                keyName = "IND_UN_" + name;
                                dicSql["ind_Clustered"].Append(dropIndex(tableName, keyName));
                                break;
                        }
                    }
                }
            }

            // 添加语句
            foreach (var item in dicSql)
            {
                if (item.Value.Length > 0)
                {
                    sb.AppendLine(item.Value.ToString());
                }
            }
            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 删除约束SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="keyName">约束名</param>
        /// <returns></returns>
        private static StringBuilder dropConstraints(string tableName,string keyName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("If Exists (Select * From sys.check_constraints Where object_id = OBJECT_ID(N'[dbo].[" + keyName + "]') And parent_object_id = OBJECT_ID(N'[dbo].[" + tableName + "]'))");
            sb.Append("Alter Table " + tableName + " Drop Constraint " + keyName + ";");
            return sb;
        }

        /// <summary>
        /// 删除索引SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="keyName">约束名</param>
        /// <returns></returns>
        private static StringBuilder dropIndex(string tableName, string keyName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("If Exists (Select * From sysindexes Where id = object_id('" + tableName + "') And name='" + keyName + "')");
            sb.Append("Drop Index " + keyName + ";");
            return sb;
        }

        /// <summary>
        /// 删除表SQL
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string dropTable(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("If Exists(Select * From sys.objects Where object_id=object_id(N'" + name + "'))");
            sb.Append("Drop Table " + name + ";");
            return sb.ToString();
        }

        /// <summary>
        /// 删除表SQL
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string dropTable(Type t)
        {
            return dropTable(t.GetType().Name);
        }

        /// <summary>
        /// 修改表名SQL
        /// </summary>
        /// <param name="oldName">旧表名</param>
        /// <param name="newName">新表名</param>
        /// <returns></returns>
        public static string renameTable(string oldName, string newName)
        {
            return "Alter Table " + oldName + " Rename To " + newName;
        }

        /// <summary>
        /// 修改表名SQL
        /// </summary>
        /// <param name="t">表实体类型</param>
        /// <param name="newName">新表名</param>
        /// <returns></returns>
        public static string renameTable(Type t, string newName)
        {
            return renameTable(t.Name, newName);
        }

        /// <summary>
        /// 添加字段SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名</param>
        /// <returns></returns>
        public static string addColumn(string tableName, string columnName)
        {
            return "Alter Table " + tableName + " Add Column " + columnName;
        }

        /// <summary>
        /// 添加字段SQL
        /// </summary>
        /// <param name="t">表实体类型</param>
        /// <param name="columnName">字段名</param>
        /// <returns></returns>
        public static string addColumn(Type t, string columnName)
        {
            return addColumn(t.Name, columnName);
        }

        #endregion

        #region 存储过程/函数等

        /// <summary>
        /// 删除翻页存储过程
        /// </summary>
        /// <returns></returns>
        public static string drop_Prc_Pageing()
        {
            string s = @"/*删除翻页存储过程*/
If Exists (Select * From sys.objects Where name = 'Prc_Pageing')
    Drop Procedure Prc_Pageing;
";
            return s;
        }

        /// <summary>
        /// 创建翻页存储过程
        /// </summary>
        /// <returns></returns>
        public static string create_Prc_Pageing()
        {
            string s = @"
Create Procedure [dbo].[Prc_Pageing]
	@strSql varchar(1000),
	@CurrentPage int,
	@PageSize int,
	@TotalCount float OUTPUT,
	@PageCount int OUTPUT,
	@IDList varchar(500) OUTPUT
AS
	SET @TotalCount=0
	SET @IDList='0'
	/*建立表临时表*/
	CREATE table #tt0(rid int NOT NULL IDENTITY(1,1), AutoID int)
	/*动态SQL*/ 
	DECLARE @str varchar(2000)
	SET @str='INSERT INTO #tt0 '+@strSql
	EXEC(@str)
	/*总记录数*/
	SELECT @TotalCount=Count(rid) from #tt0
	SET @PageCount=CEILING(@TotalCount/@PageSize)
	/*建立对应数据ID表变量*/
	DECLARE @tt1 table(AutoID int)
	INSERT INTO @tt1 SELECT AutoID FROM #tt0 Where rid>(@CurrentPage-1)*@PageSize And rid<@CurrentPage*@PageSize+1
	/*释放表临时表*/
	DROP table #tt0

	/*建立游标*/
	DECLARE cs cursor FOR SELECT AutoID FROM @tt1
	/*打开游标*/
	OPEN cs
	/*读取游标第一条记录*/
	DECLARE @csid varchar(8)
	FETCH NEXT FROM cs INTO @csid
	/*检查@@FETCH_STATUS的值，以便进行循环读取*/
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @IDList=@IDList+','+@csid 
		FETCH  NEXT FROM cs INTO @csid 
	END
	/*关闭游标*/
	CLOSE cs
	DEALLOCATE cs
;";
            return s;
        }



        #endregion

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
