namespace UnAli
{
    /// <summary>
    /// 商品参数类
    /// </summary>
    public class UnAttrGoods
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string goods_id { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string goods_name { get; set; }
        /// <summary>
        /// 商品种类
        /// </summary>
        public string goods_category { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 商品数量/重量
        /// </summary>
        public string quantity { get; set; }
    }
}
