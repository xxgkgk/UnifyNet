using Aop.Api;
using Aop.Api.Request;
using UnCommon.XMMP;
using UnQuote.Images;

namespace UnAli
{
    /// <summary>
    /// 支付基础类
    /// </summary>
    public class UnAliMch
    {
        /// <summary>
        /// 配置参数
        /// </summary>
        private UnAttrPub _config;
        /// <summary>
        /// 客户端对象
        /// </summary>
        private IAopClient _client;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="appid">开发者APPID</param>
        /// <param name="merchant_private_key">商家私钥</param>
        /// <param name="alipay_public_key">支付宝公钥</param>
        public UnAliMch(string appid, string merchant_private_key, string alipay_public_key)
        {
            UnAttrPub config = new UnAttrPub();
            config.serverUrl = "https://openapi.alipay.com/gateway.do";
            config.app_id = appid;
            config.version = "1.0";
            config.sign_type = "RSA";
            config.priKeyPem = merchant_private_key;
            config.aliPubKeyPem = alipay_public_key;
            init(config);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="appid">开发者APPID</param>
        /// <param name="merchant_private_key">商家私钥</param>
        /// <param name="alipay_public_key">支付宝公钥</param>
        /// <param name="notify_url">结果通知地址</param>
        public UnAliMch(string appid, string merchant_private_key, string alipay_public_key, string notify_url)
        {
            UnAttrPub config = new UnAttrPub();
            config.serverUrl = "https://openapi.alipay.com/gateway.do";
            config.app_id = appid;
            config.version = "1.0";
            config.sign_type = "RSA";
            config.priKeyPem = merchant_private_key;
            config.aliPubKeyPem = alipay_public_key;
            config.notify_url = notify_url;
            init(config);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="config"></param>
        public UnAliMch(UnAttrPub config)
        {
            init(config);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        public void init(UnAttrPub config)
        {
            _config = config;
            _client = new DefaultAopClient(config.serverUrl, config.app_id, config.priKeyPem, "", config.version, config.sign_type, config.aliPubKeyPem, config.charset);
            
        }

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="ct">订单对象</param>
        /// <param name="eve">事件</param>
        /// <returns>返回结果</returns>
        public AopResponse order(UnAttrContent ct, UnAliMchEvent eve)
        {
            string biz_content = UnXMMPJson.tToJson(typeof(UnAttrContent), ct);
            AopResponse response = null;
            switch (eve)
            {
                case UnAliMchEvent.预下单:
                    AlipayTradePrecreateRequest precreate = new AlipayTradePrecreateRequest();
                    precreate.BizContent = biz_content;
                    precreate.SetNotifyUrl(_config.notify_url);
                    response = _client.Execute(precreate);
                    break;
                case UnAliMchEvent.查询订单:
                    AlipayTradeQueryRequest query = new AlipayTradeQueryRequest();
                    query.BizContent = biz_content;
                    response = _client.Execute(query);
                    break;
                case UnAliMchEvent.撤销订单:
                    AlipayTradeCancelRequest cancel = new AlipayTradeCancelRequest();
                    cancel.BizContent = biz_content;
                    response = _client.Execute(cancel);
                    break;
                case UnAliMchEvent.申请退款:
                    AlipayTradeRefundRequest refund = new AlipayTradeRefundRequest();
                    refund.BizContent = biz_content;
                    response = _client.Execute(refund);
                    break;
                case UnAliMchEvent.条码支付:
                    AlipayTradePayRequest pay = new AlipayTradePayRequest();
                    pay.BizContent = biz_content;
                    response = _client.Execute(pay);
                    break;
            }
            return response;
        }

        /// <summary>
        /// 生成并返回二维码图片路径
        /// </summary>
        /// <param name="code_url"></param>
        /// <returns></returns>
        public string getQRCPath(string code_url)
        {
            return UnImage.createQrcPath(code_url, 0, UnImageQRCEtr.Q).fullName;
        }

        /// <summary>
        /// 错误事件
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public UnAttrErrorEvent errorEventFromCode(string code)
        {
             switch (code)
            {
                // 系统错误,请使用相同的参数再次调用
                case "ACQ.SYSTEM_ERROR":
                    return UnAttrErrorEvent.系统错误;
                // 参数无效,请求参数有错，重新检查请求后，再调用退款
                case "ACQ.INVALID_PARAMETER":
                    return UnAttrErrorEvent.参数无效;
                // 无权限使用接口,联系支付宝小二签约条码支 付
                case "ACQ.ACCESS_FORBIDDEN":
                    return UnAttrErrorEvent.无权限使用接口;
                // 订单信息中包含违禁词,修改订单信息后，重新发起请求
                case "ACQ.EXIST_FORBIDDEN_ WORD":
                    return UnAttrErrorEvent.订单信息中包含违禁词;
                // 应用 APP_ID 填写错误,联系支付宝小二，确认 APP_ID 的状态
                case "ACQ.PARTNER_ERROR":
                    return UnAttrErrorEvent.应用APP_ID填写错误;
                // 订单总金额超过限额,修改订单金额再发起请求
                case "ACQ.TOTAL_FEE_EXCEED":
                    return UnAttrErrorEvent.订单总金额超过限额;
                // 交易信息被篡改,更换商家订单号后，重新发起请求
                case "ACQ.CONTEXT_INCONSISTENT":
                    return UnAttrErrorEvent.交易信息被篡改;
                // 交易已被支付,确认该笔交易信息是否为当前买家的，如果是则认为交易付款成功，如果不是则更换商家订单号后，重新发起请求
                case "ACQ.TRADE_HAS_SUCCESS":
                    return UnAttrErrorEvent.交易已被支付;
                // 交易已经关闭,更换商家订单号后，重新发起请求
                case "ACQ.TRADE_HAS_CLOSE":
                    return UnAttrErrorEvent.交易已经关闭;
                // 买卖家不能相同,更换买家重新付款
                case "ACQ.BUYER_SELLER_EQUAL":
                    return UnAttrErrorEvent.买卖家不能相同;
                // 交易买家不匹配,更换商家订单号后，重新发起请求
                case "ACQ.TRADE_BUYER_NOT_MATCH":
                    return UnAttrErrorEvent.交易买家不匹配;
                // 买家状态非法,用户联系支付宝小二，确认买家状态为什么非法
                case "ACQ.BUYER_ENABLE_STATUS_FORBID":
                    return UnAttrErrorEvent.买家状态非法;
                // 买家付款日限额超限,更换买家进行支付
                case "ACQ.BUYER_PAYMENT_AMOUNT_DAY_LIMIT_ERROR":
                    return UnAttrErrorEvent.买家付款日限额超限;
                // 商户收款额度超限,联系支付宝小二提高限额
                case "ACQ.BEYOND_PAY_RESTRICTION":
                    return UnAttrErrorEvent.商户收款额度超限;
                // 商户收款金额超过月限额,联系支付宝小二提高限额
                case "ACQ.BEYOND_PER_RECEIPT_RESTRICTION":
                    return UnAttrErrorEvent.商户收款金额超过月限额;
                // 买家付款月额度超限,让买家更换账号后，重新付款或者更换其它付款方式
                case "ACQ.BUYER_PAYMENT_AMOUNT_MONTH_LIMIT_ERROR":
                    return UnAttrErrorEvent.买家付款月额度超限;
                // 商家账号被冻结,联系支付宝小二，解冻账号
                case "ACQ.SELLER_BEEN_BLOCKED":
                    return UnAttrErrorEvent.商家账号被冻结;
                // 买家未通过人行认证,让用户联系支付宝小二并更 换其它付款方式
                case "ACQ.ERROR_BUYER_CERTIFY_LEVEL_LIMIT":
                    return UnAttrErrorEvent.买家未通过人行认证;
                // 支付授权码无效,用户刷新条码后，重新扫码发起请求
                case "ACQ.PAYMENT_AUTH_CODE_INVALID":
                    return UnAttrErrorEvent.支付授权码无效;
                // 买家余额不足,买家绑定新的银行卡或者支付宝余额有钱后再发起支付
                case "ACQ.BUYER_BALANCE_NOT_ENOUGH":
                    return UnAttrErrorEvent.买家余额不足;
                // 余额支付功能关闭,用户打开余额支付开关后，再重新进行支付
                case "ACQ.ERROR_BALANCE_PAYMENT_DISABLE":
                    return UnAttrErrorEvent.余额支付功能关闭;
                // 唤起移动收银台失败,用户刷新条码后，重新扫码发起请求
                case "ACQ.PULL_MOBILE_CASHIER_FAIL":
                    return UnAttrErrorEvent.唤起移动收银台失败;
                // 用户的无线支付开关关闭,用户在PC上打开无线支付开关后，再重新发起支付
                case "ACQ.MOBILE_PAYMENT_SWITCH_OFF":
                    return UnAttrErrorEvent.用户的无线支付开关关闭;
                // 支付有风险,更换其它付款方式
                case "ACQ.PAYMENT_REQUEST_HAS_RISK":
                    return UnAttrErrorEvent.支付有风险;
                // 没用可用的支付具,更换其它付款方式
                case "ACQ.NO_PAYMENT_INSTUMENTS_AVAILABLE":
                    return UnAttrErrorEvent.没用可用的支付具;
                // 用户当面付付款开关关闭
                case "USER_FACE_PAYMENT_SWITCH_OFF":
                    return UnAttrErrorEvent.用户当面付付款开关关闭;
            }
             return UnAttrErrorEvent.未知错误;
        }

    }
}
