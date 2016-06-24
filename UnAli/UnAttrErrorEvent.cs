using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnAli
{
    public enum UnAttrErrorEvent
    {
        未知错误,
        系统错误,
        参数无效,
        无权限使用接口,
        订单信息中包含违禁词,
        应用APP_ID填写错误,
        订单总金额超过限额,
        交易信息被篡改,
        交易已被支付,
        交易已经关闭,
        买卖家不能相同,
        交易买家不匹配,
        买家状态非法,
        买家付款日限额超限,
        商户收款额度超限,
        商户收款金额超过月限额,
        买家付款月额度超限,
        商家账号被冻结,
        买家未通过人行认证,
        支付授权码无效,
        买家余额不足,
        余额支付功能关闭,
        唤起移动收银台失败,
        用户的无线支付开关关闭,
        支付有风险,
        没用可用的支付具,
        用户当面付付款开关关闭
    }

    public static class UnExtAttrErrorEvent
    {
       
    }
}
