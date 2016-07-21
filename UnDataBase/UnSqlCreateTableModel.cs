using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnDataBase
{
    /// <summary>
    /// 创建表模式
    /// </summary>
    public enum UnSqlCreateTableModel
    {
        /// <summary>
        /// 覆盖式创建数据表
        /// </summary>
        Create,
        /// <summary>
        /// 表不存在则创建,表存在则不作处理
        /// </summary>
        CreateNew,
        /// <summary>
        /// 表不存在则创建,表存在则作更新字段,关系SQL等
        /// </summary>
        CreateNewOrUpdate,
    }
}
