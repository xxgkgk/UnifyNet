using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnDataBase
{
    public enum UnSqlConnectModel
    {
        Connect,// 只连接
        Create, // 覆盖式创建数据库
        ConnectOrCreate // 数据库存在则连接,不存在则创建数据库后连接
    }
}
