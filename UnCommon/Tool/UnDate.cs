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

        /// <summary>
        /// 计算相差毫秒数
        /// </summary>
        /// <param name="begin">开始时间(null则默认格林威治1970/1/1 00:00:00.000)</param>
        /// <param name="end">结束时间(null则默认取当前时间)</param>
        /// <returns>返回相差毫秒数</returns>
        public static decimal ticksMSec(DateTime? begin, DateTime? end)
        {
            if (begin == null)
            {
                begin = new DateTime(1970, 1, 1, 8, 0, 0);
            }
            if (end == null)
            {
                end = DateTime.Now;
            }
            return Decimal.ToInt64(Decimal.Divide(end.Value.Ticks - begin.Value.Ticks, 10000));
        }

        /// <summary>
        /// 计算相差毫秒数
        /// </summary>
        /// <returns>返回相差毫秒数</returns>
        public static decimal ticksMSec()
        {
            // 注意这里取1970/1/1 08:00:00.000,因为东八区与格林威治相差8个时区
            //return Decimal.ToInt64(Decimal.Divide(DateTime.Now.Ticks - new DateTime(1970, 1, 1, 8, 0, 0).Ticks, 10000));
            return ticksMSec(new DateTime(1970, 1, 1, 8, 0, 0), DateTime.Now);
        }

        /// <summary>
        /// 计算相差秒数
        /// </summary>
        /// <param name="begin">开始时间(null则默认格林威治1970/1/1 00:00:00.000)</param>
        /// <param name="end">结束时间(null则默认取当前时间)</param>
        /// <returns>返回相差秒数</returns>
        public static decimal ticksSec(DateTime? begin, DateTime? end)
        {
            return ticksMSec(begin, end) / 1000;
        }

        /// <summary>
        /// 当前时间戳(秒)
        /// </summary>
        /// <returns>返回当前时间戳</returns>
        public static decimal ticksSec()
        {
            return ticksMSec() / 1000;
        }

        /// <summary>
        /// 计算相差分钟数
        /// </summary>
        /// <param name="begin">开始时间(null则默认格林威治1970/1/1 00:00:00.000)</param>
        /// <param name="end">结束时间(null则默认取当前时间)</param>
        /// <returns>返回相差分钟数</returns>
        public static decimal ticksMin(DateTime? begin, DateTime? end)
        {
            return ticksMSec(begin, end) / (1000 * 60);
        }

        /// <summary>
        /// 当前时间戳(分钟)
        /// </summary>
        /// <returns>返回当前时间戳</returns>
        public static decimal ticksMin()
        {
            return ticksMSec() / (1000 * 60);
        }

        /// <summary>
        /// 计算相差小时数
        /// </summary>
        /// <param name="begin">开始时间(null则默认格林威治1970/1/1 00:00:00.000)</param>
        /// <param name="end">结束时间(null则默认取当前时间)</param>
        /// <returns>返回相差小时数</returns>
        public static decimal ticksHr(DateTime? begin, DateTime? end)
        {
            return ticksMSec(begin, end) / (1000 * 60 * 60);
        }

        /// <summary>
        /// 当前时间戳(小时)
        /// </summary>
        /// <returns>返回当前时间戳</returns>
        public static decimal ticksHr()
        {
            return ticksMSec() / (1000 * 60 * 60);
        }

        /// <summary>
        /// 计算相差天数
        /// </summary>
        /// <param name="begin">开始时间(null则默认格林威治1970/1/1 00:00:00.000)</param>
        /// <param name="end">结束时间(null则默认取当前时间)</param>
        /// <returns>返回相差天数</returns>
        public static decimal ticksDay(DateTime? begin, DateTime? end)
        {
            return ticksMSec(begin, end) / (1000 * 60 * 60 * 24);
        }

        /// <summary>
        /// 当前时间戳(天数)
        /// </summary>
        /// <returns>返回当前时间戳</returns>
        public static decimal ticksDay()
        {
            return ticksMSec() / (1000 * 60 * 60 * 24);
        }

        /// <summary>
        /// 当前时间戳(秒)
        /// </summary>
        /// <returns>返回当前时间戳</returns>
        public static long ticksSecToLong()
        {
            return Convert.ToInt64(ticksSec());
        }

        /// <summary>
        /// 当前时间戳(秒)
        /// </summary>
        /// <returns>返回当前时间戳</returns>
        public static int ticksSecToInt()
        {
            return Convert.ToInt32(ticksSec());
        }

        /// <summary>
        /// 当前时间短格式
        /// </summary>
        /// <returns>返回当前时间短格式</returns>
        public static string shortNowTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
 
    }
}