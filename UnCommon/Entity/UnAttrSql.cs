using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }

    /// <summary>
    /// 索引枚举
    /// </summary>
    public enum IndexModel
    {
        None,
        Clustered,
        Nonclustered,
        Unique,
        UnionNonclustered
    }

    /// <summary>
    /// 约束枚举
    /// </summary>
    public enum ConstraintModel
    {
        None,
        PrimaryKey,
        Unique,
        Default,
        Check,
        ForeignKey
    }
}
