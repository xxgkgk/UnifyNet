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
        none,
        upFileQuery,
        upFileQueryBack,
        upFilePackage,
        upFilePackageBack,
        msgPackage,
        msgPackageBack,
        downFileQuery,
        downFileQueryBack,
        downFile,
        downFileBack
    }

    // Udp事件枚举扩展
    public static class UnUdpEveEnumExt
    {
        // 文本
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
