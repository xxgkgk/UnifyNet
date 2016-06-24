using System;
using System.Collections.Generic;
using System.IO;

namespace UnCommon.Files
{
    /// <summary>
    /// 文件事件枚举
    /// </summary>
    public enum UnFileEvent
    {
        /// <summary>
        /// 缓存
        /// </summary>
        caches,
        /// <summary>
        /// 重要
        /// </summary>
        important,
        /// <summary>
        /// 永久
        /// </summary>
        preference,
        /// <summary>
        /// 临时
        /// </summary>
        tmp,
        /// <summary>
        /// 日志记录
        /// </summary>
        log
    }

    /// <summary>
    /// 文件事件枚举扩展
    /// </summary>
    public static class UnExtFileEvent
    {
        /// <summary>
        /// 文件属性
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static UnFileInfo attributes(this UnFileEvent t)
        {
            string home = UnFile.getHomeDirectory();
            // 缓存
            string caches = home + "/Caches";
            // 重要文件
            string important = home + "/Important";
            // 永久文件
            string preference = home + "/Preference";
            // 临时文件
            string tmp = home + "/Tmp";
            // 错误日志
            string log = home + "/Log";

            UnFileInfo ufa = new UnFileInfo();
            switch (t)
            {
                case UnFileEvent.caches:
                    ufa.parentFloder = "";
                    ufa.directoryName = caches + "/";
                    break;
                case UnFileEvent.important:
                    ufa.parentFloder = DateTime.Now.ToString("yyyy-MM-dd");
                    ufa.directoryName = important + "/" + ufa.parentFloder;
                    break;
                case UnFileEvent.preference:
                    ufa.parentFloder = DateTime.Now.ToString("yyyy-MM-dd");
                    ufa.directoryName = preference + "/" + ufa.parentFloder;
                    break;
                case UnFileEvent.tmp:
                    ufa.parentFloder = "";
                    ufa.directoryName = tmp + "/";
                    break;
                case UnFileEvent.log:
                    ufa.parentFloder = DateTime.Now.ToString("yyyy-MM-dd");
                    ufa.directoryName = log + "/" + ufa.parentFloder;
                    break;
                default:
                    ufa.parentFloder = "";
                    ufa.directoryName = tmp + "/";
                    break;
            }
            return ufa;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string fullPath(this UnFileEvent t)
        {
            return t.attributes().directoryName;
        }

        /// <summary>
        /// 文件夹
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string floder(this UnFileEvent t)
        {
            return t.attributes().parentFloder;
        }


    }
}
