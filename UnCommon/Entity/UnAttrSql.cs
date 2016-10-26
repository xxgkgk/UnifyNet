using System;

namespace UnCommon.Entity
{
    /// <summary>
    /// 自定义特性 属性或者类可用  支持继承
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class UnAttrSql : Attribute
    {

        /// <summary>
        /// 实体表名
        /// </summary>
        public string tableName;

        /// <summary>
        /// 字段名
        /// </summary>
        public string fieldName;

        /// <summary>
        /// 旧字段名(会将oldFieldName 重命名为 fieldName)
        /// </summary>
        public string oldFieldName;

        /// <summary>
        /// 字段类型
        /// </summary>
        public string fieldType;

        /// <summary>
        /// 字段默认值
        /// </summary>
        public string fieldDefault;

        /// <summary>
        /// 字段是否允许空值
        /// </summary>
        public bool fieldNULL = true;

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool isPrimaryKey = false;

        /// <summary>
        /// 是否外键
        /// </summary>
        public bool isForeignKey = false;

        /// <summary>
        /// 外键值
        /// </summary>
        public string[] foreignKeyValue;

        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool isUnique = false;

        /// <summary>
        /// 唯一约束值
        /// </summary>
        public string uniqueValue;

        /// <summary>
        /// 检查约束
        /// </summary>
        public bool isCheck = false;

        /// <summary>
        /// 检查约束
        /// </summary>
        public string checkValue;

        /// <summary>
        /// 索引类型,
        /// </summary>
        public IndexModel indexModel = IndexModel.None;

        /// <summary>
        /// 链接服务器表名
        /// </summary>
        public string linkedServerTableName;
    }

    /// <summary>
    /// 索引枚举
    /// </summary>
    public enum IndexModel
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 聚集索引
        /// </summary>
        Clustered,
        /// <summary>
        /// 非聚集索引
        /// </summary>
        Nonclustered,
        /// <summary>
        /// 唯一索引
        /// </summary>
        Unique,
        /// <summary>
        /// 联合唯一索引
        /// </summary>
        UnionUnique,
        /// <summary>
        /// 联合聚集索引
        /// </summary>
        UnionClustered,
        /// <summary>
        /// 联合非聚集索引
        /// </summary>
        UnionNonclustered
    }

    /// <summary>
    /// 约束枚举
    /// </summary>
    public enum ConstraintModel
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 主键
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// 唯一
        /// </summary>
        Unique,
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 检查
        /// </summary>
        Check,
        /// <summary>
        /// 外键
        /// </summary>
        ForeignKey
    }

}
