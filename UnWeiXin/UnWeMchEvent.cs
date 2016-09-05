using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    /// <summary>
    /// 微信支付事件
    /// </summary>
    public enum UnWeMchEvent
    {
        /// <summary>
        /// 统一下单
        /// </summary>
        统一下单,
        /// <summary>
        /// 查询订单
        /// </summary>
        查询订单,
        /// <summary>
        /// 关闭订单
        /// </summary>
        关闭订单,
        /// <summary>
        /// 申请退款
        /// </summary>
        申请退款,
        /// <summary>
        /// 查询退款
        /// </summary>
        查询退款,
        /// <summary>
        /// 下载对账单
        /// </summary>
        下载对账单,
        /// <summary>
        /// 刷卡支付
        /// </summary>
        刷卡支付
    }

    /// <summary>
    /// UnWeMchEvent扩展
    /// </summary>
    public static class UnExtWeMchEvent
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string apiUrl(this UnWeMchEvent t)
        {
            switch (t)
            {
                case UnWeMchEvent.统一下单:
                    return "https://api.mch.weixin.qq.com/pay/unifiedorder";
                case UnWeMchEvent.查询订单:
                    return "https://api.mch.weixin.qq.com/pay/orderquery";
                case UnWeMchEvent.关闭订单:
                    return "https://api.mch.weixin.qq.com/pay/closeorder";
                case UnWeMchEvent.申请退款:
                    return "https://api.mch.weixin.qq.com/secapi/pay/refund";
                case UnWeMchEvent.查询退款:
                    return "https://api.mch.weixin.qq.com/pay/refundquery";
                case UnWeMchEvent.下载对账单:
                    return "https://api.mch.weixin.qq.com/pay/downloadbill";
                case UnWeMchEvent.刷卡支付:
                    return "https://api.mch.weixin.qq.com/pay/micropay";
            }
            return null;
        }

    }
}
