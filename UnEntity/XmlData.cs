using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon;
using System.Xml;
using System.Xml.Serialization;

namespace UnEntity
{
    public class XmlData
    {
        public string NonceStr { get; set; }
        public string Sign { get; set; }

        public ApiBase ApiBase { get; set; }

        //[XmlElement(IsNullable = false)]
        public ApiNote ApiNote { get; set; }
        public SMSInfo SMSInfo { get; set; }
        public EmailInfo EmailInfo { get; set; }
        public WxPayOrder WxPayOrder { get; set; }
        public AdminLogin AdminLogin { get; set; }

     
        public List<WxPayOrder> ArrayOfWxPayOrder { get; set; }

        public List<string> listString { get; set; }

        [XmlArray(IsNullable = false)]
        public List<ApiNote> ArrayOfApiNote { get; set; }

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

        public Test Test { get; set; }

        public Testa Testa { get; set; }

    }



    public class Testa: Test
    {
        public string c { get; set; }
        public string d { get; set; }
        public string AddTime{ get; set; }
    }

    public class Test: Test1
    {
        public string a { get; set; }
        public int? b { get; set; }

    }

    public class Test1
    {
        public string e { get; set; }
        public string f { get; set; }
    }
}
