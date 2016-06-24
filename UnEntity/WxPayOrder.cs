using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon;

namespace UnEntity
{
    public class WxPayOrder
    {
        public WxPayOrder()
        {
            
        }

        // ID
        public long WxPayOrderID { get; set; }
        // 唯一编号
        public string WxPayOrderUID { get; set; }
        // 添加时间
        public DateTime? AddTime { get; set; }
        // 添加时间戳
        public long AddTimeStamp { get; set; }
        // 支付状态
        public int PayState { get; set; }
        // APPID
        public string AppID { get; set; }
        // MCHID
        public string MchID { get; set; }
        // 用户ID
        public string Openid { get; set; }
        // 交易类型
        public string Trade_Type { get; set; }
        // 设备,默认WEB
        public string Device_Info { get; set; }
        // 随机字符串
        public string Nonce_str { get; set; }
        // 签名
        public string Sign { get; set; }
        // 商品内容
        public string Body { get; set; }
        // 商品详情
        public string Detail { get; set; }
        // 附加数据
        public string Attach { get; set; }
        // 货币类型
        public string Fee_Type { get; set; }
        // 总费用
        public int Total_Fee { get; set; }
        // 终端IP
        public string Spbill_create_ip { get; set; }
        // 交易起始时间
        public string Time_Start { get; set; }
        // 交易失效时间，最短失效时间间隔必须大于5分钟
        public string Time_Expire { get; set; }
        // 商品标记
        public string Goods_Tag { get; set; }
        // 微信通知URL
        public string Notify_Url { get; set; }
        // 商品ID
        public string Product_id { get; set; }
        // 指定支付方式
        public string Limit_pay { get; set; }
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
