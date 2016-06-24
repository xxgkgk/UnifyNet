using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnWeiXin
{
    [XmlRoot("item")] 
    public class UnAttrNew
    {
        // 标题
        public string Title { get; set; }
        // 描述
        public string Description { get; set; }
        // 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        public string PicUrl { get; set; }
        // 点击图文消息跳转链接
        public string Url { get; set; }
    }
}
