using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnEntity
{
    public class BackInfo
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int ReturnCode { get; set; }
        /// <summary>
        /// 状态提示
        /// </summary>
        public string ReturnMsg { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// md5验证
        /// </summary>
        public string MD5 { get; set; }
        /// <summary>
        /// 下载协议码
        /// </summary>
        public string UNCode { get; set; }
    }
}
