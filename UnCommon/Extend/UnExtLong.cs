using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnCommon.Extend
{
    /// <summary>
    /// long扩展类
    /// </summary>
    public static class UnExtLong
    {
        public static double toDouble(this long t)
        {
            return (double)t;
        }
    }
}
