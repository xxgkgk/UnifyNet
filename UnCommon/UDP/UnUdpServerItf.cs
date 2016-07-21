using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using UnCommon.Entity;

namespace UnCommon.UDP
{
    /// <summary>
    /// UDP服务端接口
    /// </summary>
    public interface UnUdpServerItf
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
