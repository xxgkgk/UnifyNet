using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnWeiXin
{
    /// <summary>
    /// 订单类
    /// </summary>
    [XmlRoot("xml")] 
    public class UnAttrOrder
    {
        /// <summary>
        /// 构造类
        /// </summary>
        public UnAttrOrder()
        {
        }

        /// <summary>
        /// 公众账号id
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }


        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 货币类型
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 终端ip
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 交易起始时间
        /// </summary>
        public string time_start { get; set; }
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public string time_expire { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string goods_tag { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string product_id { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string limit_pay { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }


        /// <summary>
        /// 业务结果
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }
        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 二维码链接
        /// </summary>
        public string code_url { get; set; }

        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id { get; set; }


        /// <summary>
        /// 商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
        /// </summary>
        public string out_refund_no { get; set; }
        /// <summary>
        /// 退款总金额，订单总金额，单位为分，只能为整数
        /// </summary>
        public string refund_fee { get; set; }
        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string refund_fee_type { get; set; }
        /// <summary>
        /// 操作员帐号, 默认为商户号
        /// </summary>
        public string op_user_id { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }

        /// <summary>
        /// 下载对账单的日期，格式：20140603
        /// </summary>
        public string bill_date { get; set; }
        /// <summary>
        /// 账单类型
        /// ALL，返回当日所有订单信息，默认值
        /// SUCCESS，返回当日成功支付的订单
        /// REFUND，返回当日退款订单
        /// REVOKED，已撤销的订单
        /// </summary>/
        public string bill_type { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        public string auth_code { get; set; }

    }

}
