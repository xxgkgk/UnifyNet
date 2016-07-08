﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;
using System.Net;
using UnCommon.UDP;

namespace UnCommon.Delegates
{
    public interface UnUdpServer
    {
        // 上传开始
        UnAttrRst upStart(EndPoint point, UnUdpEntity entity);
        // 上传成功
        UnAttrRst upSuccess(EndPoint point, UnUdpEntity entity);
        // 收到消息
        UnAttrRst msgReceived(EndPoint point, UnUdpEntity entity);
        // 下载码解析
        UnAttrRst downCodeAnalyze(EndPoint point, UnUdpEntity entity);


    }
}