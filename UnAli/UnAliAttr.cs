using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnAli
{
    public class UnAliAttr
    {
        public UnAliAttr()
        {

        }
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

        // 商家合作ID
        public string partner { get; set; }
        // 卖家ID
        public string seller_id { get; set; }
        // 卖家邮箱
        public string seller_email { get; set; }
        // 第三方支付单号
        public string out_trade_no { get; set; }
        // 商品主题
        public string subject { get; set; }
        // 商家内容
        public string body { get; set; }
        // 商品总价
        public string total_fee { get; set; }
        // 接口名称
        public string service { get; set; }
        // 默认值为：1（商品购买）
        public string payment_type { get; set; }
        // 参数编码字符串
        public string _input_charset { get; set; }
        // 编码方式
        public string sign_type { get; set; }
        // 支付有效时间(30m---)
        public string it_b_pay { get; set; }
        // 商品展示地址
        public string show_url { get; set; }
        // 支付宝消息验证地址
        public string Https_veryfy_url { get; set; }
        // 支付宝网关地址（新）
        public string gateway_new { get; set; }
        // 支付宝服务器主动通知地址
        public string notify_url { get; set; }
        // 支付宝处理完请求后跳转地址
        public string return_url { get; set; }
        // 默认加密
        public string key { get; set; }
        // rsa私钥
        public string rsa_private { get; set; }
        // rsa公钥
        public string rsa_public { get; set; }
        // 公共回传参数
        public string extra_common_param { get; set; }


        // 支付链接
        public string pay_url { get; set; }
    }
}
