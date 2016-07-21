using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnCommon.Entity
{
    /// <summary>
    /// 进度类
    /// </summary>
    public class UnAttrPgs
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public UnAttrPgs()
        {
        }

        /// <summary>
        /// 进程ID
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 总大小
        /// </summary>
        public long totalLength { get; set; }

        /// <summary>
        /// 已完成大小
        /// </summary>
        public long length { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public float percentage()
        {
            return (float)Math.Round((length * 1.00 / totalLength) * 100, 2);
        }

        /// <summary>
        /// 统计
        /// </summary>
        public UnAttrtStati statistics = new UnAttrtStati();

    }
}
