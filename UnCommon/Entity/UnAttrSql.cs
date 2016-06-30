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
        /// 字段类开
        /// </summary>
        public string fieldType;

        /// <summary>
        /// 字段默认值
        /// </summary>
        public string fieldDefault = null;

        /// <summary>
        /// 字段是否允许空值
        /// </summary>
        public bool fieldNULL = true;

        /// <summary>
        /// 约束类型
        /// </summary>
        public ConstraintModel constraintModel = ConstraintModel.None;

        /// <summary>
        /// 参数值集合
        /// </summary>
        public object value;

        /// <summary>
        /// 是索引,
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
        Unique
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
