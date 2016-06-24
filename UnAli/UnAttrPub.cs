using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnAli
{
    public class UnAttrPub
    {
        // 开发者的AppId
        public string app_id { get; set; }
        // 接口名称
        public string method { get; set; }
        // 参数字符编码
        public string charset { get; set; }
        // 签名类型
        public string sign_type { get; set; }
        // 签名
        public string sign { get; set; }
        // 接口异步通知url
        public string notify_url { get; set; }
        // 时间戳
        public string timestamp { get; set; }
        // 接口版本号
        public string version { get; set; }
        // 业务参数
        public string biz_content { get; set; }
        // 发起消息的支付宝接口名称
        public string service { get; set; }
        // 支付宝网关
        public string serverUrl { get; set; }
        // 用户私钥
        public string priKeyPem { get; set; }
        // 用户公钥
        public string pubKeyPem { get; set; }
        // 支付宝公钥
        public string aliPubKeyPem { get; set; }

    }
}
