using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    /// <summary>
    /// 模板消息
    /// </summary>
    public class UnAttrTemplate
    {
        /// <summary>
        /// 用户openid
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 模板id
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        //[NonSerialized]
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, UnAttrTmpData> data;

        /// <summary>
        /// 行业1
        /// </summary>
        //[NonSerialized]
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> primary_industry;

        /// <summary>
        ///  行业2
        /// </summary>
        //[NonSerialized]
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> secondary_industry;

        /// <summary>
        /// 模板列表
        /// </summary>
        public List<UnAttrTmpInfo> template_list;

        /// <summary>
        /// 模板发送data
        /// </summary>
        public List<UnAttrTmpSend> ArrayOfUnAttrTmpSend;

    }

    /// <summary>
    /// 模板键值数据
    /// </summary>
    public class UnAttrTmpData
    {
        /// <summary>
        /// 构造类
        /// </summary>
        /// <param name="value"></param>
        /// <param name="color"></param>
        public UnAttrTmpData(string value, string color)
        {
            this.value = value;
            this.color = color;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }
    }

    /// <summary>
    /// 模板信息
    /// </summary>
    public class UnAttrTmpInfo
    {
        /// <summary>
        /// 模板ID 
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 模板标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 模板所属行业的一级行业
        /// </summary>
        public string primary_industry { get; set; }
        /// <summary>
        /// 模板所属行业的二级行业
        /// </summary>
        public string deputy_industry { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 模板示例
        /// </summary>
        public string example { get; set; }
    }

    /// <summary>
    /// 模板发送键值
    /// </summary>
    public class UnAttrTmpSend
    {
        /// <summary>
        /// 键名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 属性-键值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 属性-颜色
        /// </summary>
        public string color { get; set; }
    }
}
