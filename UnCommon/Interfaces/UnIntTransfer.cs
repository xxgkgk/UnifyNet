using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;

namespace UnCommon.Interfaces
{
    public interface UnIntTransfer
    {
        // 进度
        void progress(UnAttrPgs pgs);

        // 成功
        bool success(UnAttrRst rst);

        // 失败
        void error(UnAttrRst rst);
    }
}
