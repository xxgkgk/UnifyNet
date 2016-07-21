using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnCommon.Extend
{
    /// <summary>
    /// 盘符信息类扩展
    /// </summary>
    public static class UnExtDriveInfo
    {
        /// <summary>
        /// 返回盘符下所有文件目录
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public static IEnumerable<String> EnumerateFiles(this DriveInfo drive)
        {
            return (new UnFileScanner()).EnumerateFiles(drive.Name);
        }
    }
}
