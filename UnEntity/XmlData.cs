using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon;
using System.Xml;

namespace UnEntity
{
    public class XmlData
    {
        public string NonceStr { get; set; }
        public string Sing { get; set; }

        public ApiBase ApiBase { get; set; }
        public ApiNote ApiNote { get; set; }
        public SMSInfo SMSInfo { get; set; }
        public EmailInfo EmailInfo { get; set; }
        public WxPayOrder WxPayOrder { get; set; }
        public AdminLogin AdminLogin { get; set; }
        public List<WxPayOrder> ArrayOfWxPayOrder { get; set; }

        public List<string> listString { get; set; }

        //private XmlNode m_parameters = new XmlDocument().CreateNode(XmlNodeType.CDATA, "", "");
        ///// <summary>
        ///// 请求参数集
        ///// </summary>
        //public XmlNode Parameters
        //{
        //    get
        //    {
        //        return m_parameters;
        //    }
        //    set
        //    {
        //        m_parameters = value;
        //    }
        //}

    }
}
