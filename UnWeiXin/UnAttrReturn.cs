using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnWeiXin
{
    /// <summary>
    /// 返回参数类
    /// </summary>
    [XmlRoot("xml")] 
    public class UnAttrReturn
    {
        /// <summary>
        /// SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息，如非空，为错误 原因签名失败 参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 调用接口提交的公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 调用接口提交的商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 调用接口提交的终端设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 微信返回的随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 微信返回的签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 错误返回的信息描述
        /// </summary>
        public string err_code_des { get; set; }
        /// <summary>
        /// 调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        public string code_url { get; set; }

        /// <summary>
        /// 用户在商户appid下的唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public string is_subscribe { get; set; }
        /// <summary>
        /// 银行类型，采用字符串类型的银行标识
        /// </summary>
        public string bank_type { get; set; }
        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 现金支付金额订单现金支付金额
        /// </summary>
        public string cash_fee { get; set; }
        /// <summary>
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string cash_fee_type { get; set; }
        /// <summary>
        /// 代金券或立减优惠金额 小于等于订单总金额，订单总金额-代金券或立减优惠金额=现金支付金额
        /// </summary>
        public string coupon_fee { get; set; }
        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        public string coupon_count { get; set; }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户系统的订单号，与请求一致
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010
        /// </summary>
        public string time_end { get; set; }

        /// <summary>
        /// 交易状态
        /// SUCCESS—支付成功
        /// REFUND—转入退款
        /// NOTPAY—未支付
        /// CLOSED—已关闭
        /// REVOKED—已撤销（刷卡支付）
        /// USERPAYING--用户支付中
        /// PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state { get; set; }

        /// <summary>
        /// 退款记录数
        /// </summary>
        public string refund_count { get; set; }
        /// <summary>
        /// 退款状态：
        /// SUCCESS—退款成功
        /// FAIL—退款失败
        /// PROCESSING—退款处理中
        /// NOTSURE—未确定，需要商户原退款单号重新发起
        /// CHANGE—转入代发，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，资金回流到商户的现金帐号，需要商户人工干预，通过线下或者财付通转账的方式进行退款 
        /// </summary>
        public string refund_status_0 { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }
        /// <summary>
        /// 微信退款渠道
        /// </summary>
        public string refund_channel { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string refund_fee { get; set; }
        /// <summary>
        /// 现金退款金额
        /// </summary>
        public string cash_refund_fee { get; set; }

        /// <summary>
        /// 对账单内容(自定义)
        /// </summary>
        public string cus_bill { get; set; }
    }
}
