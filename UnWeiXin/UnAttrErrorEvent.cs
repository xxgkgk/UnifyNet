using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    /// <summary>
    /// 错误事件
    /// </summary>
    public enum UnAttrErrorEvent
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        未知错误,
        /// <summary>
        /// 接口返回错误
        /// </summary>
        接口返回错误,
        /// <summary>
        /// 参数错误
        /// </summary>
        参数错误,
        /// <summary>
        /// 订单已支付
        /// </summary>
        订单已支付,
        /// <summary>
        /// 商户无权限
        /// </summary>
        商户无权限,
        /// <summary>
        /// 二维码已过期
        /// </summary>
        二维码已过期,
        /// <summary>
        /// 余额不足
        /// </summary>
        余额不足,
        /// <summary>
        /// 不支持卡类型
        /// </summary>
        不支持卡类型,
        /// <summary>
        /// 订单已关闭
        /// </summary>
        订单已关闭,
        /// <summary>
        /// 订单已撤销
        /// </summary>
        订单已撤销,
        /// <summary>
        /// 银行系统异常
        /// </summary>
        银行系统异常,
        /// <summary>
        /// 用户支付中
        /// </summary>
        用户支付中,
        /// <summary>
        /// 授权码参数错误
        /// </summary>
        授权码参数错误,
        /// <summary>
        /// 授权码检验错误
        /// </summary>
        授权码检验错误,
        /// <summary>
        /// XML格式错误
        /// </summary>
        XML格式错误,
        /// <summary>
        /// 请使用post方法
        /// </summary>
        请使用post方法,
        /// <summary>
        /// 签名错误
        /// </summary>
        签名错误,
        /// <summary>
        /// 缺少参数
        /// </summary>
        缺少参数,
        /// <summary>
        /// 编码格式错误
        /// </summary>
        编码格式错误,
        /// <summary>
        /// 支付帐号错误
        /// </summary>
        支付帐号错误,
        /// <summary>
        /// APPID不存在
        /// </summary>
        APPID不存在,
        /// <summary>
        /// MCHID不存在
        /// </summary>
        MCHID不存在,
        /// <summary>
        /// 商户订单号重复
        /// </summary>
        商户订单号重复,
        /// <summary>
        /// appid和mch_id不匹配
        /// </summary>
        appid和mch_id不匹配
    }
}
