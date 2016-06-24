using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnCommon.UDP;

namespace UnCommon.Entity
{
    /// <summary>
    /// 结果类
    /// </summary>
    public class UnAttrRst
    {
        // 实例化
        public UnAttrRst()
        { 

        }

        /// <summary>
        /// 进程ID
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 编码(1成功,-1失败）
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public string back { get; set; }

        /// <summary>
        /// 传递的数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 传输参数
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public UnUdpState UnUdpState;

    }
}
