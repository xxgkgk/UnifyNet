using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnWeiXin
{
    [XmlRoot("xml")] 
    public class UnAttrMsg
    {
        // 开发者微信号||发送方帐号（一个OpenID） 
        public string ToUserName { get; set; }
        // 开发者微信号||发送方帐号（一个OpenID） 
        public string FromUserName { get; set; }
        // 消息创建时间 （整型） 
        public long CreateTime { get; set; }
        // 消息类型
        public string MsgType { get; set; }
        // 消息id，64位整型
        public string MsgId { get; set; }

        // 文本消息内容 
        public string Content { get; set; }

        // 语音消息媒体id，可以调用多媒体文件下载接口拉取数据
        public string MediaId { get; set; }
        // 语音格式，如amr，speex等 
        public string Format { get; set; }

        // 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        public string ThumbMediaId { get; set; }

        // 地理位置维度
        public string Location_X { get; set; }
        // 地理位置经度
        public string Location_Y { get; set; }
        // 地图缩放大小
        public string Scale { get; set; }
        // 地理位置信息 
        public string Label { get; set; }

        // 消息标题
        public string Title { get; set; }
        // 消息描述
        public string Description { get; set; }
        // 消息链接
        public string Url { get; set; }

        // 事件类型
        public string Event { get; set; }
        // 事件KEY值
        public string EventKey { get; set; }

        // 二维码的ticket，可用来换取二维码图片
        public string Ticket { get; set; }

        // 地理位置纬度 
        public string Latitude { get; set; }
        // 地理位置经度 
        public string Longitude { get; set; }
        // 地理位置精度 
        public string Precision { get; set; }

        // 音乐链接
        public string MusicURL { get; set; }
        // 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        public string HQMusicUrl { get; set; }

        // 图文消息个数，限制为10条以内 
        public string ArticleCount { get; set; }

        // 图文列表
        public List<UnAttrNew> Articles { get; set; }
    }
}
