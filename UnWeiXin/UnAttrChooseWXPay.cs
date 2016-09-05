using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    /// <summary>
    /// 
    /// </summary>
    public class UnAttrChooseWXPay
    {
        /// <summary>
        /// appid
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get;set; }
        /// <summary>
        /// 随机串 小于等于32
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 统一支付接口返回的prepay_id
        /// </summary>
        public string package { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string signType { get; set; }
        /// <summary>
        /// 支付签名
        /// </summary>
        public string paySign { get; set; }
    }
}
