using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using UnCommon;
using System.Reflection;
using System.Text;
using UnCommon.Tool;
using UnCommon.Entity;
using UnCommon.Config;

namespace UnDataBase
{
    /// <summary>
    /// SQL语句组装
    /// </summary>
    public class UnSqlStr
    {
        #region 表操作

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
            var properties = UnToGen.getListFieldPropertyInfo(t);
            int length = properties.Count;
            StringBuilder sb = new StringBuilder();
            if (length <= 0)
            {
                return null;
            }
            //取类上的自定义特性
            UnAttrSql classAttr = UnToGen.getAttrSql(t);
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
                string str = getFieldTypeAndNull(item);
                if (str != null)
                {
                    sb.AppendLine(getFieldTypeAndNull(item) + ",");
                }
            }
            sb.AppendLine(");");
            return sb.ToString();
        }

        /// <summary>
        /// 获得AttrSql,无则默认
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static UnAttrSql getAttrSqlDefault(PropertyInfo item)
        {
            string name = item.Name;
            if (UnToGen.isField(item))
            {
                UnAttrSql attr = UnToGen.getAttrSql(item);
                if (attr == null)
                {
                    attr = new UnAttrSql();
                }
                // 字段修饰
                if (attr.fieldType == null)
                {
                    Type type = item.PropertyType;
                    // string
                    if (type.Equals(typeof(String)))
                    {
                        attr.fieldType = "varchar";
                        return attr; 
                    }
                    // Int
                    if (type.Equals(typeof(Int16)) || type.Equals(typeof(Nullable<Int16>)))
                    {
                        attr.fieldType = "smallint";
                        return attr;
                    }
                    if (type.Equals(typeof(Int32)) || type.Equals(typeof(Nullable<Int32>)))
                    {
                        attr.fieldType = "int";
                        return attr;
                    }
                    if (type.Equals(typeof(Int64)) || type.Equals(typeof(Nullable<Int64>)))
                    {
                        attr.fieldType = "bigint";
                        return attr;
                    }
                    // UInt
                    if (type.Equals(typeof(UInt16)) || type.Equals(typeof(Nullable<UInt16>)))
                    {
                        attr.fieldType = "smallint";
                        return attr;
                    }
                    if (type.Equals(typeof(UInt32)) || type.Equals(typeof(Nullable<UInt32>)))
                    {
                        attr.fieldType = "int";
                        return attr;
                    }
                    if (type.Equals(typeof(UInt64)) || type.Equals(typeof(Nullable<UInt64>)))
                    {
                        attr.fieldType = "bigint";
                        return attr;
                    }
                    // 小数
                    if (type.Equals(typeof(float)) || type.Equals(typeof(Nullable<float>)))
                    {
                        attr.fieldType = "float";
                        return attr;
                    }
                    if (type.Equals(typeof(Decimal)) || type.Equals(typeof(Nullable<Decimal>)))
                    {
                        attr.fieldType = "money";
                        return attr;
                    }
                    if (type.Equals(typeof(Double)) || type.Equals(typeof(Nullable<Double>)))
                    {
                        attr.fieldType = "float";
                        return attr;
                    }
                    // 布尔值
                    if (type.Equals(typeof(Boolean)) || type.Equals(typeof(Nullable<Boolean>)))
                    {
                        attr.fieldType = "bit";
                        return attr;
                    }
                    // 时间
                    else if (type.Equals(typeof(DateTime)) || type.Equals(typeof(Nullable<DateTime>)))
                    {
                        attr.fieldType = "datetime";
                        return attr;
                    }
                    // GUID
                    if (type.Equals(typeof(Guid)) || type.Equals(typeof(Nullable<Guid>)))
                    {
                        attr.fieldType = "uniqueidentifier";
                        return attr;
                    }
                    // 字符字节
                    if (type.Equals(typeof(Char)) || type.Equals(typeof(Nullable<Char>)))
                    {
                        attr.fieldType = "string";
                        return attr;
                    }
                    if (type.Equals(typeof(Byte)) || type.Equals(typeof(Nullable<Byte>)))
                    {
                        attr.fieldType = "tinyint";
                        return attr;
                    }
                    if (type.Equals(typeof(Byte[])))
                    {
                        attr.fieldType = "binary";
                        return attr;
                    }
                }
                return attr;
            }
            return null;
        }

        /// <summary>
        /// 获得字段类型+是否空值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string getFieldTypeAndNull(PropertyInfo item)
        {
            UnAttrSql attr = getAttrSqlDefault(item);
            if (attr != null)
            {
                string nullStr = "NULL";
                if (!attr.fieldNULL)
                {
                    nullStr = "NOT NULL";
                }
                return "[" + UnToGen.getFieldName(item) + "] " + attr.fieldType + " " + nullStr;
            }
            return null;
        }

        /// <summary>
        /// 建表关联SQL
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string createTableRelation(Type t)
        {
            var properties = UnToGen.getListFieldPropertyInfo(t);
            int length = properties.Count;
            StringBuilder sb = new StringBuilder();
            if (length <= 0)
            {
                return null;
            }
            string tableName = UnToGen.getTableName(t);
            Dictionary<string, StringBuilder> dicSql = new Dictionary<string, StringBuilder>();
            dicSql.Add("con_PrimarykKey", new StringBuilder());
            dicSql.Add("con_ForeignKey", new StringBuilder());
            dicSql.Add("con_UniQue", new StringBuilder());
            dicSql.Add("con_Default", new StringBuilder());
            dicSql.Add("con_Check", new StringBuilder());
            dicSql.Add("ind_Clustered", new StringBuilder());
            dicSql.Add("ind_Nonclustered", new StringBuilder());
            dicSql.Add("ind_Unique", new StringBuilder());
            dicSql.Add("ind_UnionClustered", new StringBuilder());
            dicSql.Add("ind_UnionNonclustered", new StringBuilder());

            Dictionary<string, StringBuilder> dicNames = new Dictionary<string, StringBuilder>();
            dicNames.Add("con_PrimarykKey", new StringBuilder());
            dicNames.Add("ind_Clustered", new StringBuilder());
            dicNames.Add("ind_Unique", new StringBuilder());
            dicNames.Add("ind_UnionClustered", new StringBuilder());
            dicNames.Add("ind_UnionNonclustered", new StringBuilder());

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
                        // 主键
                        if (attr.isPrimaryKey)
                        {
                            keyName = "CON_" + tableName + "_PK_" + name;
                            if (dicSql["con_PrimarykKey"].Length == 0)
                            {
                                dicSql["con_PrimarykKey"].Append("Alter Table " + tableName + " Add Constraint " + keyName + " Primary Key(");
                            }
                            dicNames["con_PrimarykKey"].Append(name + ",");
                        }
                        // 外键
                        if (attr.isForeignKey)
                        {
                            if (attr.foreignKeyValue != null)
                            {
                                keyName = "CON_" + tableName + "_FK_" + name + "_" + attr.foreignKeyValue[0] + "_" + attr.foreignKeyValue[1];
                                dicSql["con_ForeignKey"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Foreign Key (" + name + ") References " + attr.foreignKeyValue[0] + "(" + attr.foreignKeyValue[1] + ");");
                            }
                        }
                        // 默认值
                        if (attr.fieldDefault != null)
                        {
                            keyName = "CON_" + tableName + "_DE_" + name;
                            dicSql["con_Default"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Default(" + attr.fieldDefault + ") For " + name + ";");
                        }
                        // 唯一
                        if (attr.isUnique)
                        {
                            keyName = "CON_" + tableName + "_UN_" + name;
                            dicSql["con_Default"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Unique(" + attr.uniqueValue + ") For " + name + ";");
                        }
                        // Check
                        if (attr.isCheck)
                        {
                            keyName = "CON_" + tableName + "_CH_" + name;
                            dicSql["con_Check"].AppendLine("Alter Table " + tableName + " Add Constraint " + keyName + " Check(" + attr.checkValue + ");");
                        }
                        // 索引
                        switch (attr.indexModel)
                        {
                            case IndexModel.Clustered:// 聚集索引
                                keyName = "IND_" + tableName + "_CL_" + name;
                                if (dicSql["ind_Clustered"].Length == 0)
                                {
                                    dicSql["ind_Clustered"].Append("Create Index " + keyName + " On " + tableName + "(");
                                }
                                dicNames["ind_Clustered"].Append(name + ",");
                                break;
                            case IndexModel.Nonclustered:// 非聚集索引
                                keyName = "IND_" + tableName + "_NO_" + name;
                                dicSql["ind_Nonclustered"].Append("Create Nonclustered Index " + keyName + " On " + tableName + "(" + name + ")");
                                break;
                            case IndexModel.Unique:// 唯一索引
                                keyName = "IND_" + tableName + "_UN_" + name;
                                if (dicSql["ind_Unique"].Length == 0)
                                {
                                    dicSql["ind_Unique"].Append("Create Unique Index " + keyName + " On " + tableName + "(");
                                }
                                dicNames["ind_Unique"].Append(name + ",");
                                break;
                            case IndexModel.UnionClustered:// 联合聚集索引
                                keyName = "IND_" + tableName + "_UNCL_" + name;
                                if (dicSql["ind_UnionClustered"].Length == 0)
                                {
                                    dicSql["ind_UnionClustered"].Append("Create Index " + keyName + " On " + tableName + "(");
                                }
                                dicNames["ind_UnionClustered"].Append(name + ",");
                                break;
                            case IndexModel.UnionNonclustered:// 联合非聚集索引
                                keyName = "IND_" + tableName + "_UNNO_" + name;
                                if (dicSql["ind_UnionNonclustered"].Length == 0)
                                {
                                    dicSql["ind_UnionNonclustered"].Append("Create Nonclustered Index " + keyName + " On " + tableName + "(");
                                }
                                dicNames["ind_UnionNonclustered"].Append(name + ",");
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
            return sb.ToString().Trim();
        }

        /// <summary>
        /// 查询所有约束
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string getAllConstraints(Type t)
        {
            string pre = "CON/_";
            if (t != null)
            {
                pre += UnToGen.getTableName(t) + "/_";
            }
            string s = @"SELECT * FROM dbo.sysobjects WHERE name like '"+ pre + "%' escape '/'";
            return s;
        }

        /// <summary>
        /// 查询所有索引
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string getAllIndex(Type t)
        {
            string pre = "IND/_";
            if (t != null)
            {
                pre += UnToGen.getTableName(t) + "/_";
            }
            string s = @"SELECT * FROM sys.indexes WHERE name like '" + pre + "%' escape '/'";
            return s;
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
            dicSql.Add("ind_UnionClustered", new StringBuilder());
            dicSql.Add("ind_UnionNonclustered", new StringBuilder());

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

                        // 主键
                        if (attr.isPrimaryKey)
                        {
                            keyName = "CON_PK_" + tableName + "_" + name;
                            dicSql["con_PrimarykKey"].Append(dropConstraints(ConstraintModel.PrimaryKey, tableName, keyName));
                        }
                        // 外键
                        if (attr.isForeignKey)
                        {
                            if (attr.foreignKeyValue != null)
                            {
                                keyName = "CON_FK_" + tableName + "_" + name + "_" + attr.foreignKeyValue[0] + "_" + attr.foreignKeyValue[1];
                                dicSql["con_ForeignKey"].AppendLine(dropConstraints(ConstraintModel.ForeignKey, tableName, keyName).ToString());
                            }
                        }
                        // 默认值
                        if (attr.fieldDefault != null)
                        {
                            keyName = "CON_DE_" + tableName + "_" + name;
                            dicSql["con_Default"].AppendLine(dropConstraints(ConstraintModel.Default, tableName, keyName).ToString());
                        }
                        // 唯一
                        if (attr.isUnique)
                        {
                            keyName = "CON_UN_" + tableName + "_" + name;
                            dicSql["con_UniQue"].AppendLine(dropConstraints(ConstraintModel.Unique, tableName, keyName).ToString());
                        }
                        // Check
                        if (attr.isCheck)
                        {
                            keyName = "CON_CH_" + tableName + "_" + name;
                            dicSql["con_Check"].AppendLine(dropConstraints(ConstraintModel.Check, tableName, keyName).ToString());
                        }
                        // 索引
                        switch (attr.indexModel)
                        {
                            case IndexModel.Clustered:// 聚集索引
                                keyName = tableName + ".IND_CL_" + tableName + "_" + name;
                                dicSql["ind_Clustered"].Append(dropIndex(IndexModel.Clustered, tableName, keyName));
                                break;
                            case IndexModel.Nonclustered:// 非聚集索引
                                keyName = "IND_NO_" + tableName + "_" + name;
                                dicSql["ind_Nonclustered"].Append(dropIndex(IndexModel.Nonclustered, tableName, keyName));
                                break;
                            case IndexModel.Unique:// 唯一索引
                                keyName = "IND_UN_" + tableName + "_" + name;
                                dicSql["ind_Unique"].Append(dropIndex(IndexModel.Unique, tableName, keyName));
                                break;
                            case IndexModel.UnionClustered:// 联合聚集索引
                                keyName = "IND_UNCL_" + tableName + "_" + name;
                                dicSql["ind_UnionClustered"].Append(dropIndex(IndexModel.UnionClustered, tableName, keyName));
                                break;
                            case IndexModel.UnionNonclustered:// 联合非聚集索引
                                keyName = "IND_UNNO_" + tableName + "_" + name;
                                dicSql["ind_UnionNonclustered"].Append(dropIndex(IndexModel.UnionNonclustered, tableName, keyName));
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
            return sb.ToString().Trim();
        }

        /// <summary>
        /// 删除约束SQL
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static StringBuilder dropConstraints(ConstraintModel model,string tableName, string keyName)
        {
            StringBuilder sb = new StringBuilder();
            switch (model)
            {
                case ConstraintModel.Default:
                    sb.AppendLine("IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[" + keyName + "]') AND type = 'D')");
                    sb.Append("Alter Table " + tableName + " Drop Constraint " + keyName + ";");
                    break;
                case ConstraintModel.ForeignKey:
                    sb.AppendLine("IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[" + keyName + "]') AND parent_object_id = OBJECT_ID(N'[dbo].[" + tableName + "]'))");
                    sb.Append("Alter Table " + tableName + " Drop Constraint " + keyName + ";");
                    break;
                default:
                    sb.AppendLine("IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'" + tableName + "') AND name = N'" + keyName + "')");
                    sb.Append("Alter Table " + tableName + " Drop Constraint " + keyName + ";");
                    break;
            }
            return sb;
        }

        /// <summary>
        /// 删除索引SQL
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static StringBuilder dropIndex(IndexModel model, string tableName, string keyName)
        {
            StringBuilder sb = new StringBuilder();
            switch (model)
            {
                case IndexModel.Unique:
                case IndexModel.Clustered:
                case IndexModel.Nonclustered:
                case IndexModel.UnionClustered:
                case IndexModel.UnionNonclustered:
                    sb.AppendLine("IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[" + tableName + "]') AND name = N'" + keyName + "')");
                    sb.Append("DROP INDEX [" + keyName + "] ON [dbo].[" + tableName + "] WITH ( ONLINE = OFF )");
                    break;
                default:
                    sb.AppendLine("If Exists (Select * From sys.indexes Where id = object_id('" + tableName + "') And name='" + keyName + "')");
                    sb.Append("Drop Index " + keyName + ";");
                    break;
            }
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
            return dropTable(UnToGen.getTableName(t));
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

        /// <summary>
        /// 查询表所有列
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string getAllColumn(Type t)
        {
            return "Select name from syscolumns where id=(select max(id) from sysobjects where xtype='u' and name='" + t.Name + "')";
        }

        #endregion

        #region 基础SQL

        /// <summary>
        /// 删除翻页存储过程
        /// </summary>
        /// <returns></returns>
        public static string drop_Pro_PageKeys()
        {
            string s = @"/*删除翻页存储过程*/
If Exists (Select * From sys.objects Where name = 'Pro_PageKeys')
    Drop Procedure Pro_PageKeys;
";
            return s;
        }

        /// <summary>
        /// 创建翻页存储过程
        /// </summary>
        /// <returns></returns>
        public static string create_Pro_PageKeys()
        {
            string s = @"
Create Procedure [dbo].[Pro_PageKeys]
    @KeyName varchar(50),
	@From varchar(6000),
	@CurrentPage int = 0 OutPut,
	@PageSize int = 0 OutPut,
	@TotalNumber int = 0 OutPut,
	@TotalPages int = 0 OutPut,
	@Keys varchar(3000) OutPut
As
	-- 建立表临时表
	Create Table #TmpTable0(Rid int Not Null IDENTITY(1,1), UniqueKey int)
	-- 添加临时表数据
	Exec('Insert Into #TmpTable0 Select ' + @KeyName+ ' As UniqueKey From ' + @From)
	-- 总条数
	Select @TotalNumber = Count(Rid) From #TmpTable0
	-- 总页数
	Set @TotalPages = Ceiling(Convert(decimal(20,2),@TotalNumber) / @PageSize )
	-- 建立对应Key表变量
	Declare @TmpTable1 Table(UniqueKey int)
	Insert Into @TmpTable1 Select UniqueKey From #TmpTable0 Where Rid > (@CurrentPage-1) * @PageSize And Rid < @CurrentPage * @PageSize + 1
	-- 释放表临时表
	Drop Table #TmpTable0

	-- 建立游标
	Declare cs Cursor For Select UniqueKey From @TmpTable1
	-- 打开游标
	Open cs
	-- 读取游标第一条记录
	Declare @csid varchar(8)
	Fetch Next From cs Into @csid
	-- 检查@@FETCH_STATUS的值，以便进行循环读取
	While @@FETCH_STATUS = 0
	Begin
	    Begin
			If LEN(@Keys) > 0
				Set @Keys += ',' + @csid 
			Else
				Set @Keys = @csid 
	    End
		Fetch Next From cs Into @csid
	End
	-- 关闭游标
	Close cs
	Deallocate cs
;";
            return s;
        }

        /// <summary>
        /// 删除-判断字段是否存在
        /// </summary>
        /// <returns></returns>
        public static string drop_mfn_IsColumnExists()
        {
            return @"if OBJECT_ID(N'dbo.mfn_IsColumnExists', N'FN') is not null  
    drop function dbo.mfn_IsColumnExists ;";
        }

        /// <summary>
        /// 创建-判断字段是否存在
        /// </summary>
        /// <returns></returns>
        public static string create_mfn_IsColumnExists()
        {
            return @"create function dbo.mfn_IsColumnExists(@TableName NVARCHAR(128), @ColumnName NVARCHAR(128))  
    returns bit  
as  
begin  
    declare @rt bit  
    set @rt=0  
    if (select name from sys.syscolumns where name=@ColumnName and id=OBJECT_ID(@TableName)) is not null  
        set @rt=1  
    return @rt  
end;";
        }

        /// <summary>
        /// 删除-查询某个字段的所有索引  
        /// </summary>
        /// <returns></returns>
        public static string drop_mfn_GetColumnIndexes()
        {
            return @"if OBJECT_ID(N'dbo.mfn_GetColumnIndexes', N'TF') is not null  
    drop function dbo.mfn_GetColumnIndexes;";
        }

        /// <summary>
        /// 创建-查询某个字段的所有索引  
        /// </summary>
        /// <returns></returns>
        public static string create_mfn_GetColumnIndexes()
        {
            return @"create function dbo.mfn_GetColumnIndexes(@TableName NVARCHAR(128), @ColumnName NVARCHAR(128))  
    returns @ret table  
    (  
        id int,  
        name NVARCHAR(128)  
    )  
as  
begin  
    declare @tid int, @colid int  
  
    -- 先查询出表id和列id  
    select @tid=OBJECT_ID(@tablename)  
    select @colid=colid from sys.syscolumns where id=@tid and name=@columnname  
  
    -- 查询出索引名称  
    insert into @ret select ROW_NUMBER() OVER(ORDER BY cols.index_id) as id, inds.name idxname from sys.index_columns cols  
        left join sys.indexes inds on cols.object_id=inds.object_id and cols.index_id=inds.index_id   
        where cols.object_id=@tid and column_id=@colid  
          
    return  
end;";
        }

        /// <summary>
        /// 删除-删除某个表的某列的所有约束
        /// </summary>
        /// <returns></returns>
        public static string drop_mp_DropColConstraint()
        {
            return @"if OBJECT_ID(N'dbo.mp_DropColConstraint', N'P') is not null  
    drop procedure dbo.mp_DropColConstraint;";
        }

        /// <summary>
        /// 创建-删除某个表的某列的所有约束
        /// </summary>
        /// <returns></returns>
        public static string create_mp_DropColConstraint()
        {
            return @"create procedure dbo.mp_DropColConstraint  
    @TableName NVARCHAR(128),  
    @ColumnName NVARCHAR(128)  
as  
begin  
    if OBJECT_ID(N'#t', N'TB') is not null  
        drop table #t  
      
    -- 查询主键约束、非空约束等  
    select ROW_NUMBER() over(order by CONSTRAINT_NAME) id, CONSTRAINT_NAME into #t from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where TABLE_CATALOG=DB_NAME()  
        and TABLE_NAME=@TableName and COLUMN_NAME=@ColumnName  
          
    -- 查询默认值约束  
    declare @cdefault int, @cname varchar(128)  
    select @cdefault=cdefault from sys.syscolumns where name=@ColumnName and id=OBJECT_ID(@TableName)  
              
    select @cname=name from sys.sysobjects where id=@cdefault  
    if @cname is not null  
        insert into #t select coalesce(max(id), 0)+1, @cname from #t      
  
    declare @i int, @imax int  
    select @i=1, @imax=max(id) from #t  
  
    while @i <= @imax  
    begin  
        select @cname=CONSTRAINT_NAME from #t where id=@i  
        exec('alter table ' + @tablename + ' drop constraint ' + @cname)  
        set @i = @i + 1   
    end  
  
    drop table #t  
  
end;";
        }

        /// <summary>
        /// 删除-删除指定列的所有索引  
        /// </summary>
        /// <returns></returns>
        public static string drop_mp_DropColumnIndexes()
        {
            return @"if OBJECT_ID(N'dbo.mp_DropColumnIndexes', N'P') is not null  
    drop procedure dbo.mp_DropColumnIndexes;";
        }

        /// <summary>
        /// 创建-删除指定列的所有索引   
        /// </summary>
        /// <returns></returns>
        public static string create_mp_DropColumnIndexes()
        {
            return @"create procedure dbo.mp_DropColumnIndexes  
    @TableName NVARCHAR(128),  
    @ColumnName NVARCHAR(128)  
as   
begin  
    if OBJECT_ID(N'#t', N'TB') is not null  
        drop table #t  
    create table #t  
    (  
        id int,       
        name nvarchar(128)  
    )  
      
    insert into #t select * from mfn_GetColumnIndexes(@TableName, @ColumnName)  
      
    -- 删除索引  
    declare @i int, @imax int, @idxname nvarchar(128)  
      
    select @i=1, @imax=COALESCE(max(id), 0) from #t  
    while @i<=@imax   
    begin  
        select @idxname=name from #t  
        EXEC('drop index ' + @idxname + ' on ' + @tablename)  
        set @i=@i+1  
    end  
      
    drop table #t  
end;";
        }

        /// <summary>
        /// 删除-删除指定字段的所有约束和索引  
        /// </summary>
        /// <returns></returns>
        public static string drop_mp_DropColConstraintAndIndex()
        {
            return @"if OBJECT_ID(N'dbo.mp_DropColConstraintAndIndex', N'P') is not null  
    drop procedure dbo.mp_DropColConstraintAndIndex;";
        }

        /// <summary>
        /// 创建-删除指定字段的所有约束和索引  
        /// </summary>
        /// <returns></returns>
        public static string create_mp_DropColConstraintAndIndex()
        {
            return @"create procedure dbo.mp_DropColConstraintAndIndex  
    @TableName NVARCHAR(128),  
    @ColumnName NVARCHAR(128)  
as  
begin  
    exec dbo.mp_DropColConstraint @TableName, @ColumnName  
    exec dbo.mp_DropColumnIndexes @TableName, @ColumnName  
end;";
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
        /// <param name="pars"></param>
        /// <returns></returns>
        internal static string getAddStr<T>(SqlParameter[] pars) where T : new()
        {
            string AddStr = "";
            string AddStr1 = "";
            string AddStr2 = "";
            foreach (SqlParameter par in pars)
            {
                // 将便名转为字段名
                string fname = toName(par.ParameterName);

                AddStr1 += "[" + fname + "],";
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
        /// <param name="Pars"></param>
        /// <returns></returns>
        internal static string getUpdStr(SqlParameter[] Pars)
        {
            string UpdStr = null;
            foreach (SqlParameter par in Pars)
            {
                string fname = toName(par.ParameterName);
                UpdStr += "[" + fname + "]=@" + par.ParameterName + ",";
            }
            return UpdStr.TrimEnd(',');
        }

        /// <summary>
        /// Upd字段组合(T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static string getUpdStr<T>() where T : new()
        {

            List<string> _List = UnToGen.getFieldNoAutoInc<T>();
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
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="columns">查询字段</param>
        /// <param name="selection">条件</param>
        /// <param name="selectionArgs">条件参数</param>
        /// <param name="groupBy">group</param>
        /// <param name="having">having</param>
        /// <param name="orderBy">order</param>
        /// <param name="isLinkedServer">是否链接服务器</param>
        /// <returns></returns>
        public static string getQuerySql<T>(string[] columns,
            string selection, string[] selectionArgs, string groupBy,
            string having, string orderBy,bool isLinkedServer)
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
            sb.Append(" From " + UnToGen.getTableName(typeof(T), isLinkedServer) + " ");
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
                if (orderBy.ToLower().IndexOf("order") < 0)
                {
                    orderBy = "Order By " + orderBy;
                }
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
        /// 根据字段名产生便名
        /// </summary>
        /// <param name="fName">字段名</param>
        /// <param name="pid">线程ID</param>
        /// <returns></returns>
        internal static string toByName(string fName,int pid)
        {
            // 按便名存,为了防止使用事务的时候,出现多个重名变量
            return fName + "_" + pid;
        }

        /// <summary>
        /// 便名转实名
        /// </summary>
        /// <param name="byName">参数名</param>
        /// <returns></returns>
        internal static string toName(string byName)
        {
            // 将便名转为字段名
            int last = byName.LastIndexOf('_');
            string fname = byName.Remove(last, byName.Length - last);
            return fname;
        }

        /// <summary>
        /// 获得SqlPmt数组(T所有字段)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static SqlParameter[] getSqlPmtA<T>(T t) where T : new()
        {
            return getSqlPmtA(t, null);
        }

        /// <summary>
        /// 获得SqlPmt数组(FieldList)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="FieldList"></param>
        /// <returns></returns>
        internal static SqlParameter[] getSqlPmtA<T>(T t, string FieldList) where T : new()
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            List<string> _List = UnToGen.getFieldNoAutoInc<T>();
            int pid = UnInit.pid();
            Type Typet = typeof(T);
            for (int i = 0; i < _List.Count; i++)
            {
                bool isTrue = false;
                string strName = _List[i];
                // 如果不限制字段 或者 在限制字段内
                if (FieldList == null || ("," + FieldList + ",").IndexOf("," + strName + ",") >= 0)
                {
                    isTrue = true;
                }
                if (isTrue)
                {
                    PropertyInfo pro = Typet.GetProperty(strName);
                    Type type = pro.GetType();
                    object value = UnToGen.convertTo(type, pro.GetValue(t, null));
                    if (value != null)
                    {
                        // 按便名存,为了防止使用事务的时候,出现多个重名变量
                        string byname = toByName(strName, pid);
                        pars.Add(new SqlParameter(byname, value));
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
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
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

        /// <summary>
        /// 条件语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selection"></param>
        /// <param name="selectionArgs"></param>
        /// <returns></returns>
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
