using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;
using UnCommon.Config;
using UnCommon.Tool;

namespace UnCommon.UDP
{
    /// <summary>
    /// 传输数据类
    /// </summary>
    public class UnUdpState
    {

        #region Attribute

        // pid
        private int _pdi = UnInit.pid();
        /// <summary>
        /// pid
        /// </summary>
        /// <returns></returns>
        public int pid()
        {
            return _pdi;
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        public byte[] receive = new byte[1472];
        /// <summary>
        /// 发送数据
        /// </summary>
        public byte[] send = new byte[1472];

        /// <summary>
        /// 设置套接字
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public Socket socket;

        /// <summary>
        /// 连接终结点
        /// </summary>
        [NonSerialized]
        public EndPoint remote = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
        /// <summary>
        /// 唤醒时间(毫秒)
        /// </summary>
        public decimal wakeTicks = UnDate.ticksMSec();
        /// <summary>
        /// 空闲时间(毫秒)
        /// </summary>
        public decimal freeTicks()
        {
            return UnDate.ticksMSec() - wakeTicks;
        }
        /// <summary>
        /// 单位时间内下载大小
        /// </summary>
        public int unitDownLength;

        #endregion

    }

}
