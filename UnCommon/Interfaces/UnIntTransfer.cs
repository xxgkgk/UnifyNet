using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;

namespace UnCommon.Interfaces
{
    /// <summary>
    /// 传输状态接口
    /// </summary>
    public interface UnIntTransfer
    {
        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="pgs"></param>
        void progress(UnAttrPgs pgs);

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="rst"></param>
        /// <returns></returns>
        bool success(UnAttrRst rst);

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="rst"></param>
        void error(UnAttrRst rst);
    }
}
