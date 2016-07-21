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
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 视频
        /// </summary>
        Audio,
        /// <summary>
        /// 文档
        /// </summary>
        Doc

    }

    /// <summary>
    /// 上传事件枚举
    /// </summary>
    public static class UnExtHttpUpEvent
    {
        /// <summary>
        /// 文本
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
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
