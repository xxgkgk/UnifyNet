using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnWeiXin
{
    [XmlRoot("xml")] 
    public class UnAttrOrder
    {
        public UnAttrOrder()
        {
        }

        // 公众账号id
        public string appid { get; set; }
        // 商户号
        public string mch_id { get; set; }
        // 设备号
        public string device_info { get; set; }
        // 随机字符串
        public string nonce_str { get; set; }
        // 签名
        public string sign { get; set; }


        // 商品描述
        public string body { get; set; }
        // 商品详情
        public string detail { get; set; }
        // 附加数据
        public string attach { get; set; }
        // 商户订单号
        public string out_trade_no { get; set; }
        // 货币类型
        public string fee_type { get; set; }
        // 总金额
        public string total_fee { get; set; }
        // 终端ip
        public string spbill_create_ip { get; set; }
        // 交易起始时间
        public string time_start { get; set; }
        // 交易结束时间
        public string time_expire { get; set; }
        // 商品标记
        public string goods_tag { get; set; }
        // 通知地址
        public string notify_url { get; set; }
        // 商品标记
        public string trade_type { get; set; }
        // 商品标记
        public string product_id { get; set; }
        // 商品标记
        public string limit_pay { get; set; }
        // 用户标识
        public string openid { get; set; }


        // 业务结果
        public string result_code { get; set; }
        // 错误代码
        public string err_code { get; set; }
        // 错误代码描述
        public string err_code_des { get; set; }
        // 预支付交易会话标识
        public string prepay_id { get; set; }
        // 二维码链接
        public string code_url { get; set; }

        // 微信订单号
        public string transaction_id { get; set; }


        // 商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
        public string out_refund_no { get; set; }
        // 退款总金额，订单总金额，单位为分，只能为整数
        public string refund_fee { get; set; }
        // 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY
        public string refund_fee_type { get; set; }
        // 操作员帐号, 默认为商户号
        public string op_user_id { get; set; }

        // 微信退款单号
        public string refund_id { get; set; }

        // 下载对账单的日期，格式：20140603
        public string bill_date { get; set; }
        // 账单类型
        // ALL，返回当日所有订单信息，默认值
        // SUCCESS，返回当日成功支付的订单
        // REFUND，返回当日退款订单
        // REVOKED，已撤销的订单
        public string bill_type { get; set; }
        // 授权码
        public string auth_code { get; set; }

    }

}
