using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnCommon.Extend
{
    public static class UnExtDriveInfo
    {
        public static IEnumerable<String> EnumerateFiles(this DriveInfo drive)
        {
            return (new UnFileScanner()).EnumerateFiles(drive.Name);
        }
    }
}
