using System;
using System.Collections.Generic;
using System.Web;
using Com.Alipay;
using System.Threading;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;


namespace Com.Alipay
{
    /// <summary>
    /// F2FBiz 的摘要说明
    /// </summary>
    public class F2FBiz
    {
        private F2FBiz() { }

        public static readonly IAopClient client = new DefaultAopClient(Config.serverUrl, Config.appId, Config.merchant_private_key, "", Config.version,
        Config.sign_type, Config.alipay_public_key, Config.charset);

        public static IAopClient GetAopClient()
        {
            return client;
        }

    }
}
