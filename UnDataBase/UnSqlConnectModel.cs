using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnDataBase
{
    /// <summary>
    /// SQL连接方式
    /// </summary>
    public enum UnSqlConnectModel
    {
        /// <summary>
        /// 只连接
        /// </summary>
        Connect,
        /// <summary>
        /// 覆盖式创建数据库
        /// </summary>
        Create,
        /// <summary>
        /// 数据库存在则连接,不存在则创建数据库后连接
        /// </summary>
        ConnectOrCreate
    }
}
