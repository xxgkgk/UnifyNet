using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnCommon.UDP
{
    /// <summary>
    /// Udp事件枚举
    /// </summary>
    public enum UnUdpEveEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        none,
        /// <summary>
        /// 上传查询
        /// </summary>
        upFileQuery,
        /// <summary>
        /// 上传查询返回
        /// </summary>
        upFileQueryBack,
        /// <summary>
        /// 上传包
        /// </summary>
        upFilePackage,
        /// <summary>
        /// 上传包返回
        /// </summary>
        upFilePackageBack,
        /// <summary>
        /// 消息包
        /// </summary>
        msgPackage,
        /// <summary>
        /// 消息包返回
        /// </summary>
        msgPackageBack,
        /// <summary>
        /// 下载查询
        /// </summary>
        downFileQuery,
        /// <summary>
        /// 下载查询返回
        /// </summary>
        downFileQueryBack,
        /// <summary>
        /// 下载文件
        /// </summary>
        downFile,
        /// <summary>
        /// 下载文件返回
        /// </summary>
        downFileBack
    }

    /// <summary>
    /// Udp事件枚举扩展
    /// </summary>
    public static class UnUdpEveEnumExt
    {
        /// <summary>
        /// 返回文本
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string getText(this UnUdpEveEnum t)
        {
            switch (t)
            {
                case UnUdpEveEnum.upFilePackage:
                    return "up";
                case UnUdpEveEnum.upFilePackageBack:
                    return "upb";
                case UnUdpEveEnum.upFileQuery:
                    return "uq";
                case UnUdpEveEnum.upFileQueryBack:
                    return "uqb";
                case UnUdpEveEnum.msgPackage:
                    return "mp";
                case UnUdpEveEnum.msgPackageBack:
                    return "mpb";
                case UnUdpEveEnum.downFileQuery:
                    return "dq";
                case UnUdpEveEnum.downFileQueryBack:
                    return "dqb";
                case UnUdpEveEnum.downFile:
                    return "df";
                case UnUdpEveEnum.downFileBack:
                    return "dfb";
            }
            return "None";
        }
    }
}
