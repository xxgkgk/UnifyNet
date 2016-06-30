using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;

namespace UnCommon
{
    [UnAttrSql(tableName = "AliPayOrder")]
    /// <summary>
    /// 支付宝订单
    /// </summary>
    public class AliPayOrder
    {
        // ID
        [UnAttrSql(fieldType = "bigint IDENTITY(1,1)")]
        public long AliPayOrderID { get; set; }
        // 唯一编号
        [UnAttrSql(fieldType = "varchar(50) Not Null", constraintModel = ConstraintModel.PrimaryKey)]
        public string AliPayOrderUID { get; set; }
        // 添加时间
        public string AddTime { get; set; }
        // 添加时间戳
        public long AddTimeStamp { get; set; }
        // 支付状态
        public int PayState { get; set; }
        // 支付状态
        [UnAttrSql(indexModel = IndexModel.Clustered)]
        public int PayType { get; set; }
        // 支付状态备注
        public string PayTypeRemark { get; set; }
        // 接口名称
        public string Service { get; set; }
        // 合作者身份ID
        public string Partner { get; set; }
        // 客户端号
        [UnAttrSql(constraintModel = ConstraintModel.ForeignKey, value = new string[]{"A","id"})]
        public string AppID { get; set; }
        // 总费用
        public decimal Total_Fee { get; set; }
        // 订单标题
        [UnAttrSql(constraintModel = ConstraintModel.ForeignKey, value = new string[] { "B", "uid" })]
        public string Subject { get; set; }
        // 订单描述
        public string Body { get; set; }
        // 有效日期
        public string Timeout_express { get; set; }
        // 附加数据
        public string Attach { get; set; }
        // 服务器异步通知页面路径
        public string Notify_Url { get; set; }
        // 通知参数(JSON)
        public string ParmsJSON { get; set; }
        // 通知参数(XML)
        public string ParmsXML { get; set; }
        // 商家名称 
        public string Bus_Name { get; set; }
        // 商家订单号
        public string Bus_Trade_No { get; set; }
        // 商家通知URL
        public string Bus_Notify_Url { get; set; }
        // 商家随机字符串
        public string Bus_NonceStr { get; set; }
        // 商家签名验证
        public string Bus_Sign { get; set; }
    }
}
