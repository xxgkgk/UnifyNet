using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnCommon.HTTP
{
    /// <summary>
    /// 上传事件枚举
    /// </summary>
    public enum UnHttpUpEvent
    {
        // 图片
        Image,
        // 视频
        Audio,
        // 文档
        Doc

    }

    // 扩展
    public static class UnExtHttpUpEvent
    {
        // 文本
        public static string text(this UnHttpUpEvent t)
        {
            switch (t)
            {
                case UnHttpUpEvent.Image:
                    return "Image";
                case UnHttpUpEvent.Audio:
                    return "Audio";
                case UnHttpUpEvent.Doc:
                    return "Doc";
            }
            return null;
        }
    }

}
