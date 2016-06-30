using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnDataBase
{
    public enum UnSqlCreateTableModel
    {
        Create, // 覆盖式创建数据表
        CreateNew, // 表不存在则创建,表存在则不作处理
        CreateNewOrUpdate,// 表不存在则创建,表存在则作更新字段,关系SQL等
    }
}
