using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;
using System.Net;
using UnCommon.UDP;

namespace UnCommon.Delegates
{
    public class UnTraClient
    {
        // 进度
        public delegate void progressDelegate(UnAttrPgs pgs);
        // 成功
        public delegate bool successDelegate(UnAttrRst rst);
        // 错误
        public delegate void errorDelegate(UnAttrRst rst);
    }
}
