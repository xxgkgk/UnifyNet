using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Extend;

namespace UnCommon.Tool
{
    public class UnValidate
    {
        // 字符串长度区间比较
        public static bool isBetweenLength(string input, int? min, int? max)
        {
            return input.isBetweenLength(min, max);
        }
    }
}
