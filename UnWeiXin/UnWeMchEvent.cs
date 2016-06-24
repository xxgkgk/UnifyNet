using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public enum UnWeMchEvent
    {
        统一下单,
        查询订单,
        关闭订单,
        申请退款,
        查询退款,
        下载对账单,
        刷卡支付
    }

    /// <summary>
    /// UnWeMchEvent扩展
    /// </summary>
    public static class UnExtWeMchEvent
    {
        // 接口地址
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
