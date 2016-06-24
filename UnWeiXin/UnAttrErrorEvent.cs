using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public enum UnAttrErrorEvent
    {
        未知错误,
        接口返回错误,
        参数错误,
        订单已支付,
        商户无权限,
        二维码已过期,
        余额不足,
        不支持卡类型,
        订单已关闭,
        订单已撤销,
        银行系统异常,
        用户支付中,
        授权码参数错误,
        授权码检验错误,
        XML格式错误,
        请使用post方法,
        签名错误,
        缺少参数,
        编码格式错误,
        支付帐号错误,
        APPID不存在,
        MCHID不存在,
        商户订单号重复,
        appid和mch_id不匹配
    }
}
