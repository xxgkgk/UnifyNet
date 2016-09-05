using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    /// <summary>
    /// 错误类
    /// </summary>
    public class UnAttrErr
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误提示
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public string msgid { get; set; }
    }
}
