namespace UnAli
{
    /// <summary>
    /// 公共参数类
    /// </summary>
    public class UnAttrPub
    {
        /// <summary>
        /// 开发者的AppId
        /// </summary>
        public string app_id { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// 参数字符编码
        /// </summary>
        public string charset { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 接口异步通知url
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 接口版本号
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 业务参数
        /// </summary>
        public string biz_content { get; set; }
        /// <summary>
        /// 发起消息的支付宝接口名称
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 支付宝网关
        /// </summary>
        public string serverUrl { get; set; }
        /// <summary>
        /// 用户私钥
        /// </summary>
        public string priKeyPem { get; set; }
        /// <summary>
        /// 用户公钥
        /// </summary>
        public string pubKeyPem { get; set; }
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string aliPubKeyPem { get; set; }

    }
}
