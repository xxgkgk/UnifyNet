using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;
using System.Net;
using UnCommon.UDP;

namespace UnCommon.Delegates
{
    /// <summary>
    /// UDP服务端接口
    /// </summary>
    public interface UnUdpServer
    {
        /// <summary>
        /// 上传开始
        /// </summary>
        /// <param name="point"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        UnAttrRst upStart(EndPoint point, UnUdpEntity entity);

        /// <summary>
        /// 上传成功
        /// </summary>
        /// <param name="point"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        UnAttrRst upSuccess(EndPoint point, UnUdpEntity entity);

        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="point"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        UnAttrRst msgReceived(EndPoint point, UnUdpEntity entity);

        /// <summary>
        /// 下载码解析
        /// </summary>
        /// <param name="point"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        UnAttrRst downCodeAnalyze(EndPoint point, UnUdpEntity entity);


    }
}
