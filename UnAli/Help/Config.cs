using System.IO;
using System.Web;

namespace Com.Alipay
{

    /// <summary>
    /// 配置类
    /// </summary>
    public class Config
    {

        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public static string alipay_public_key = HttpRuntime.AppDomainAppPath.ToString() + "Demo\\alipay_rsa_public_key_dev.pem";
        /// <summary>
        /// 这里要配置没有经过PKCS8转换的原始私钥
        /// </summary>
        public static string merchant_private_key = HttpRuntime.AppDomainAppPath.ToString() + "Demo\\rsa_private_key_dev.pem";
        /// <summary>
        /// 商家公钥
        /// </summary>
        public static string merchant_public_key = HttpRuntime.AppDomainAppPath.ToString() + "Demo\\rsa_public_key_dev.pem";
        /// <summary>
        /// 商家移动ID
        /// </summary>
        public static string appId = "2015042200551234";
        /// <summary>
        /// serverUrl
        /// </summary>
        public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        /// <summary>
        /// mapiUrl
        /// </summary>
        public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
        /// <summary>
        /// 商家合作ID
        /// </summary>
        public static string pid = "";
        /// <summary>
        /// 字符集
        /// </summary>
        public static string charset = "utf-8";//"utf-8";
        /// <summary>
        /// 签名类型,默认RSA
        /// </summary>
        public static string sign_type = "RSA";
        /// <summary>
        /// 版本号
        /// </summary>
        public static string version = "1.0";
     
        /// <summary>
        /// 实例化
        /// </summary>
        public Config()
        {
            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string getMerchantPublicKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_public_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
              pubkey=  pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
              pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
              pubkey = pubkey.Replace("\r", "");
              pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string getMerchantPriveteKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_private_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

    }
}