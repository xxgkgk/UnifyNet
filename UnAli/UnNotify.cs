using Com.Alipay;
using System.Collections.Generic;

namespace UnAli
{
    /// <summary>
    /// 通知类
    /// </summary>
    public static class UnNotify
    {
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="sPara">参数字典</param>
        /// <param name="charset">字符集</param>
        /// <param name="sign_type">签名类型</param>
        /// <param name="partner">商家</param>
        /// <param name="mapiUrl">网关</param>
        /// <returns></returns>
        public static bool verify(SortedDictionary<string, string> sPara, string charset, string sign_type, string partner, string mapiUrl)
        {
            Notify aliNotify = new Notify(charset, sign_type, partner, mapiUrl);

            return aliNotify.Verify(sPara, sPara["notify_id"], sPara["sign"]);
        }
    }
}
