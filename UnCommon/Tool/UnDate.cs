using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnCommon.Tool
{
    /// <summary>
    /// 从格林威治1970/1/1 00:00:00.000开始的时间类
    /// </summary>
    public class UnDate
    {

        // 获取总毫秒数
        public static decimal ticksMSec(DateTime begin, DateTime end)
        {
            return Decimal.ToInt64(Decimal.Divide(end.Ticks - begin.Ticks, 10000));
        }

        // 获取总毫秒数
        public static decimal ticksMSec()
        {
            // 注意这里取1970/1/1 08:00:00.000,因为东八区与格林威治相差8个时区
            //return Decimal.ToInt64(Decimal.Divide(DateTime.Now.Ticks - new DateTime(1970, 1, 1, 8, 0, 0).Ticks, 10000));
            return ticksMSec(new DateTime(1970, 1, 1, 8, 0, 0), DateTime.Now);
        }

        // 获取总秒数
        public static decimal ticksSec(DateTime begin, DateTime end)
        {
            return ticksMSec(begin, end) / 1000;
        }

        // 获取总秒数
        public static decimal ticksSec()
        {
            return ticksMSec() / 1000;
        }

        // 获取总分钟数
        public static decimal ticksMin(DateTime begin, DateTime end)
        {
            return ticksMSec(begin, end) / (1000 * 60);
        }

        // 获取总分钟数
        public static decimal ticksMin()
        {
            return ticksMSec() / (1000 * 60);
        }

        // 获取总小时数
        public static decimal ticksHr(DateTime begin, DateTime end)
        {
            return ticksMSec(begin, end) / (1000 * 60 * 60);
        }

        // 获取总小时数
        public static decimal ticksHr()
        {
            return ticksMSec() / (1000 * 60 * 60);
        }

        // 获取总天数
        public static decimal ticksDay(DateTime begin, DateTime end)
        {
            return ticksMSec(begin, end) / (1000 * 60 * 60 * 24);
        }

        // 获取总天数
        public static decimal ticksDay()
        {
            return ticksMSec() / (1000 * 60 * 60 * 24);
        }

        // 时间戳(long)
        public static long ticksSecToLong()
        {
            return Convert.ToInt64(ticksSec());
        }

        // 时间戳(int)
        public static int ticksSecToInt()
        {
            return Convert.ToInt32(ticksSec());
        }

        // 当前短时间格式
        public static string shortNowTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
 
    }
}