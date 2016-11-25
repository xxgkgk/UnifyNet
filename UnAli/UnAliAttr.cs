
namespace UnAli
{
    /// <summary>
    /// 支付参数类
    /// </summary>
    public class UnAliAttr
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public UnAliAttr()
        {

        }

        /// <summary>
        /// 支付参数
        /// </summary>
        /// <param name="PayType">
        /// 支付类型
        /// 1=网页支付
        /// 2=app支付
        /// </param>
        public UnAliAttr(int PayType)
        {
            partner = "2088411486262468";
            seller_id = "hpkj@zztzfx.com";
            seller_email = "hpkj@zztzfx.com";
            _input_charset = "utf-8";
            payment_type = "1";
            Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
            switch (PayType)
            {
                // app
                case 2:
                    service = "mobile.securitypay.pay";
                    rsa_private = @"MIICXQIBAAKBgQDXMNUhLEaFyriwc6noRIrOYBMtiOmaIPmMiIl7nnx1TPIn1cl1EmRUG61vQ6BwtnII235pw6xJ7pN80DJOO4oFl+RdLJ3WUoH5F7VaIC0whaYTjSROlrTGV0uwQE0ucS/3kPCrsZo7mcyR3iqbdhyLF70imrjizWsMj5GWJ1pe7QIDAQABAoGAHLUoCbflZJ3py6hyh0j1l5ibllyIhYZWpFfmt3YqNl/ggk30BUlB1zKmDYzhD4hXaDUYBTYUevx38pO30lYBEIUkv9VcnV/8kibPaULY3VxTjCe/Zc96bqOiBj4weYA3Sjz716JOC2pBaBKH685oPSHOyxtw6MlFUvQOk8P0rEUCQQD933npU6DiueO/e0tlwGq+IEIvOPvc+yeUttdLeSwmQX/e5IzybiTWcagOELEZ+coGH8o0Br+hInGbu0T/bZLvAkEA2P5jXLE5uSkG/GptTAESk6Pndw4XlRaGIhrAS728SruQi/NyoZu28sP93zXPWoYlWneEbqjgyeXOW6aJTOw74wJBAIkBmaE56JFzEF2+BCqddP22GwGxFvti7zFqmzW35wOeRYvce9Q5bNft7mvoxirmiwhTD6SUH8at9/G7VrkiQTECQBuT0Prxxx1CGpIv0G4tYLgEx6D42IRpFWj2L2Qw2+I/jjJLIVFqocieogAhLZYaoFqr/9e0fMUMALrjImX1S/MCQQDNqHegyFyo8HDSHMgUF/t32CGfEqzJ/0JYLF9C3aoUFJCLtBC4/zi1EJ6r40IZht2IAhNeiLcAVD2Ctz1VoyuY";
                    rsa_public = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
                    sign_type = "RSA";
                    notify_url = "http://www.zztzfx.com/pay/alipay_wap/notify.aspx";
                    return_url = "http://www.zztzfx.com/pay/alipay_wap/return.aspx";
                    break;
                default:
                    service = "create_direct_pay_by_user";
                    key = "dczjxbcbfchc734334l20gvwiot11u5i";
                    sign_type = "MD5";
                    gateway_new = "https://mapi.alipay.com/gateway.do?";
                    notify_url = "http://www.zztzfx.com/pay/alipay/notify.aspx";
                    return_url = "http://www.zztzfx.com/pay/alipay/return.aspx";
                    break;
            }
        }

        /// <summary>
        /// 商家合作ID
        /// </summary>
        public string partner { get; set; }
        /// <summary>
        /// 卖家ID
        /// </summary>
        public string seller_id { get; set; }
        /// <summary>
        /// 卖家邮箱
        /// </summary>
        public string seller_email { get; set; }
        /// <summary>
        /// 第三方支付单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商品主题
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 商家内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商品总价
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 默认值为：1（商品购买）
        /// </summary>
        public string payment_type { get; set; }
        /// <summary>
        /// 参数编码字符串
        /// </summary>
        public string _input_charset { get; set; }
        /// <summary>
        /// 编码方式
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 支付有效时间(30m---)
        /// </summary>
        public string it_b_pay { get; set; }
        /// <summary>
        /// 商品展示地址
        /// </summary>
        public string show_url { get; set; }
        /// <summary>
        /// 支付宝消息验证地址
        /// </summary>
        public string Https_veryfy_url { get; set; }
        /// <summary>
        /// 支付宝网关地址（新）
        /// </summary>
        public string gateway_new { get; set; }
        /// <summary>
        /// 支付宝服务器主动通知地址
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 支付宝处理完请求后跳转地址
        /// </summary>
        public string return_url { get; set; }
        /// <summary>
        /// 默认加密
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// rsa私钥
        /// </summary>
        public string rsa_private { get; set; }
        /// <summary>
        /// rsa公钥
        /// </summary>
        public string rsa_public { get; set; }
        /// <summary>
        /// 公共回传参数
        /// </summary>
        public string extra_common_param { get; set; }


        /// <summary>
        /// 支付链接
        /// </summary>
        public string pay_url { get; set; }
    }
}
