using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public class UnAttrTemplate
    {
        // 用户openid
        public string touser { get; set; }

        // 模板id
        public string template_id { get; set; }

        // 链接地址
        public string url { get; set; }

        // 数据
        //[NonSerialized]
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, UnAttrTmpData> data;

        // 行业1
        //[NonSerialized]
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> primary_industry;

        // 行业2
        //[NonSerialized]
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> secondary_industry;

        // 模板列表
        public List<UnAttrTmpInfo> template_list;

        // 模板发送data
        public List<UnAttrTmpSend> ArrayOfUnAttrTmpSend;

    }

    // 模板键值数据
    public class UnAttrTmpData
    {
        // 构造类
        public UnAttrTmpData(string value, string color)
        {
            this.value = value;
            this.color = color;
        }

        // 值
        public string value { get; set; }

        // 颜色
        public string color { get; set; }
    }

    // 模板信息
    public class UnAttrTmpInfo
    {
        // 模板ID 
        public string template_id { get; set; }
        // 模板标题
        public string title { get; set; }
        // 模板所属行业的一级行业
        public string primary_industry { get; set; }
        // 模板所属行业的二级行业
        public string deputy_industry { get; set; }
        // 模板内容
        public string content { get; set; }
        // 模板示例
        public string example { get; set; }
    }

    // 模板发送键值
    public class UnAttrTmpSend
    {
        // 键名
        public string name { get; set; }

        // 属性-键值
        public string value { get; set; }

        // 属性-颜色
        public string color { get; set; }
    }
}
