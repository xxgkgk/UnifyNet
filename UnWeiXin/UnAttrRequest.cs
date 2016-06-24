using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public class UnAttrRequest
    {
        public UnAttrRequest()
        {
            this.EventKey = "";
            this.CreateTime = 0;
            this.Description = "";
            this.Event = "";
            this.EventKey = "";
            this.Format = "";
            this.Label = "";
            this.Latitude = "";
            this.Location_X = "";
            this.Location_Y = "";
            this.Longitude = "";
            this.MediaId = "";
            this.MsgId = 0;
            this.MsgType = "";
            this.PicUrl = "";
            this.Precision = "";
            this.Scale = "";
            this.ThumbMediaId = "";
            this.Ticket = "";
            this.Title = "";
            this.ToUserName = "";
            this.Url = "";
        }

        // 开发者微信号（一个OpenID） 
        public string ToUserName { get; set; }

        // 发送方帐号（一个OpenID） 
        public string FromUserName { get; set; }

        // 消息创建时间 （整型） 
        public long CreateTime { get; set; }

        // 消息类型
        public string MsgType { get; set; }

        // 事件类型
        public string Event { get; set; }

        // 事件KEY值，qrscene_为前缀，后面为二维码的参数值 
        public string EventKey { get; set; }

        // 二维码的ticket，可用来换取二维码图片 
        public string Ticket { get; set; }

        // 图片链接
        public string PicUrl { get; set; }

        // 图片消息媒体id，可以调用多媒体文件下载接口拉取数据
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

        // 消息id，64位整型 
        public long MsgId { get; set; }

        // 地理位置纬度 
        public string Latitude { get; set; }

        // 地理位置经度
        public string Longitude { get; set; }

        // 地理位置精度 
        public string Precision { get; set; }
    }
}
