using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;
using System.Net;
using UnCommon.UDP;

namespace UnCommon.Interfaces
{
    /// <summary>
    /// UDP服务端接口
    /// </summary>
    public interface UnIntUdpServer
    {
        /// <summary>
        /// 上传查询
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

        /// <summary>
        /// 处理包
        /// </summary>
        /// <param name="point"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        UnAttrRst proPackage(EndPoint point, UnUdpEntity entity);

    }
}
