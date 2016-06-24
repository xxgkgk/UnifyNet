using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public class UnAttrChooseWXPay
    {
        // appid
        public string appId { get; set; }
        // 时间戳
        public long timestamp { get;set; }
        // 随机串 <=32
        public string nonceStr { get; set; }
        // 统一支付接口返回的prepay_id
        public string package { get; set; }
        // 签名方式
        public string signType { get; set; }
        // 支付签名
        public string paySign { get; set; }
    }
}
