namespace UnAli
{
    /// <summary>
    /// 订单信息类
    /// </summary>
    public class UnAttrContent
    {
        /// <summary>
        /// 商户订单号，64个字符以内、只能包含字母、数字、下划线;需保证在商户端不重复
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 如果该值为空，则默认为商户签约账号对应的支付宝用户 ID
        /// </summary>
        public string seller_id {get; set;}
        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000], 如果同时传入了【打折金额】，【不可打折金额】，【订单总 金额】三者，则必须满足如下条件:【订单总金额】=【打折 金额】+【不可打折金额】
        /// </summary>
        public string total_amount { get; set; }
        /// <summary>
        /// 参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]如果该值未传入，但传入了【订 单总金额】，【不可打折金额】 则该值默认为【订单总金额】- 【不可打折金额】
        /// </summary>
        public string discountable_amount { get; set; }
        /// <summary>
        /// 不参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]如果该值未传入，但传入了【订 单总金额】，【打折金额】，则该值默认为【订单总金额】- 【打折金额】
        /// </summary>
        public string undiscountable_amount { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 对交易或商品的描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 订单包含的商品列表信息， Json 格式，其它说明详见:“商品明细说明”
        /// </summary>
        public object goods_detail { get; set; }
        /// <summary>
        /// 商户的操作员编号
        /// </summary>
        public string operator_id { get; set; }
        /// <summary>
        /// 商户门店编号
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// 业务扩展参数
        /// </summary>
        public object extend_params { get; set; }
        /// <summary>
        /// 该笔订单允许的最晚付款时间，逾期将关闭交易。格式为: yyyy-MM-dd HH:mm:ss 
        /// </summary>
        public string time_expire { get; set; }

        /// <summary>
        /// 支付宝交易凭证号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 需要退款的金额，该金额不能 大于订单金额，单位为元，支持两位小数
        /// </summary>
        public string refund_amount { get; set; }
        /// <summary>
        /// 标识一次退款请求，同一笔交 易多次退款需要保证唯一
        /// </summary>
        public string out_request_no { get; set; }
        /// <summary>
        /// 退款的原因说明
        /// </summary>
        public string refund_reason { get; set; }
        /// <summary>
        /// 支付宝的商家店铺编号
        /// </summary>
        public string alipay_store_id { get; set; }
        /// <summary>
        /// 商户的终端编号
        /// </summary>
        public string terminal_id { get; set; }

        /// <summary>
        /// 支付场景
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// 支付授权码
        /// </summary>
        public string auth_code { get; set; }
        /// <summary>
        /// 该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c- 当天（无论交易何时创建， 都在 0 点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m。该值优先级低于time_expire
        /// </summary>
        public string timeout_express { get; set; }
        /// <summary>
        /// 描述分账信息，Json格式
        /// </summary>
        public string royalty_info { get; set; }
    }

}
