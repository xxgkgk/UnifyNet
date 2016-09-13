using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace UnAli
{
    /// <summary>
    /// 支付帮助类
    /// </summary>
    public class UnAliHelp
    {

        /// <summary>
        /// [即时到账]提交请求接口
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        public static string getPayUrl(UnAliAttr ap)
        {
            //把请求参数打包成数组
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>();
            sd.Add("partner", ap.partner);
            sd.Add("_input_charset", ap._input_charset);
            sd.Add("service", ap.service);
            sd.Add("payment_type", ap.payment_type);
            sd.Add("notify_url", ap.notify_url);
            sd.Add("return_url", ap.return_url);
            sd.Add("seller_email", ap.seller_email);
            sd.Add("out_trade_no", ap.out_trade_no);
            sd.Add("subject", ap.subject);
            sd.Add("total_fee", ap.total_fee);
            sd.Add("body", ap.body);
            sd.Add("show_url", ap.show_url);
            //sd.Add("anti_phishing_key", "dd");
            //sd.Add("exter_invoke_ip", exter_invoke_ip);
            Dictionary<string, string> dc = filterPara(sd);


            //待签名字符串
            string prestr = createLinkString(dc);
            //签名字符串
            string mysign = signPrestr(prestr, ap.key, ap._input_charset);

            //签名结果与签名方式加入请求提交参数组中
            dc.Add("sign", mysign);
            dc.Add("sign_type", ap.sign_type);

            //待请求参数数组字符串
            string strRequestData = createLinkStringUrlencode(dc, Encoding.GetEncoding(ap._input_charset));

            return ap.gateway_new + strRequestData;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> filterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“”字符拼接成字符串
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string createLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string createLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(@temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        public static string signPrestr(string prestr, string key, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> getRequestPost(HttpContext context)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = context.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], context.Request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}
