using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;
using System.Net;
using UnCommon.UDP;

namespace UnCommon.Delegates
{
    /// <summary>
    /// 客户端传输接口
    /// </summary>
    public class UnTraClient
    {
        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="pgs"></param>
        public delegate void progressDelegate(UnAttrPgs pgs);
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="rst"></param>
        /// <returns></returns>
        public delegate bool successDelegate(UnAttrRst rst);
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="rst"></param>
        public delegate void errorDelegate(UnAttrRst rst);
    }
}
