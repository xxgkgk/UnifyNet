using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Extend;

namespace UnCommon.Tool
{
    /// <summary>
    /// 比较类工具
    /// </summary>
    public class UnValidate
    {
        /// <summary>
        /// 字符串长度区间比较
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool isBetweenLength(string input, int? min, int? max)
        {
            return input.isBetweenLength(min, max);
        }
    }
}
