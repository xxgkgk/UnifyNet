using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using UnCommon.Entity;
using UnCommon.Config;
using UnCommon.Files;
using UnCommon.Extend;
using UnCommon.Encrypt;
using System.Net;

namespace UnCommon.UDP
{
    /// <summary>
    /// UDP分包实体
    /// </summary>
    public class UnUdpEntity
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public UnUdpEntity()
        { 
        }

        /// <summary>
        /// 实例化(文件分包)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="subSize"></param>
        public UnUdpEntity(string filePath, int subSize)
        {
            UnFileInfo uf = new UnFileInfo(filePath);
            this.FullName = filePath;
          
            this.Extent = uf.extens;
            this.HashCode = uf.md5;
            this.SubSize = subSize;
            this.TotalPacks = (long)Math.Ceiling((double)uf.length / this.SubSize);
            if (this.TotalPacks == 0)
            {
                TotalPacks = 1;
            }
            this.TotalSize = uf.length;
        }

        /// <summary>
        /// 实例化(消息分包)
        /// </summary>
        /// <param name="msg"></param>
        public UnUdpEntity(byte[] msg)
        {
            this.FullName = null;
            this.PackData = msg;
            this.PackSize = msg.Length;
            this.Extent = ".msg";
            this.HashCode = msg.md5Hash();
            this.TotalPacks = 1;
            this.TotalSize = msg.Length;
            this.SubSize = msg.Length;
        }

        /// <summary>
        /// 实例化(下载)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isDown"></param>
        public UnUdpEntity(string code, bool isDown)
        {
            this.PackData = UnInit.getEncoding().GetBytes(code);
            this.PackMD5 = UnEncMD5.getMd5Hash(code);
            this.PackSize = this.PackData.Length;
        }

        /// <summary>
        /// 事件
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 区间值
        /// </summary>
        public long IntMin { get; set; }
        /// <summary>
        /// 区间值
        /// </summary>
        public long IntMax { get; set; }
        /// <summary>
        /// 已传包数
        /// </summary>
        public long UpCount { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extent { get; set; }
        /// <summary>
        /// HashCode
        /// </summary>
        public string HashCode { get; set; }
        /// <summary>
        /// 分包大小
        /// </summary>
        public int SubSize { get; set; }
        /// <summary>
        /// 总包数
        /// </summary>
        public long TotalPacks { get; set; }
        /// <summary>
        /// 总大小
        /// </summary>
        public long TotalSize { get; set; }
        /// <summary>
        /// 包编号
        /// </summary>
        public long PackNo { get; set; }
        /// <summary>
        /// 包偏移量
        /// </summary>
        public long PackOffset { get; set; }
        /// <summary>
        /// 包内容大小
        /// </summary>
        public int PackSize { get; set; }
        /// <summary>
        /// 包md5
        /// </summary>
        public string PackMD5 { get; set; }
        /// <summary>
        /// 状态(-2:文件过大,-1:文件类型错误,0:上传中,1:上传成功,2:极速秒传)
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 原文件名(不含扩展名)
        /// </summary>
        public string OgnName { get; set; }

        /// <summary>
        /// 头大小
        /// </summary>
        public int HeadSize { get; set; }
        /// <summary>
        /// 包内容
        /// </summary>
        public byte[] PackData { get; set; }
        /// <summary>
        /// 临时路径
        /// </summary>
        public string TmpPath { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 终结点
        /// </summary>
        public EndPoint Point { get; set; }

        /// <summary>
        /// 唤醒时间
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public decimal WakeTimeStamp = 0;
        /// <summary>
        /// 是否发送
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public bool isSend = false;
        /// <summary>
        /// 已接收包
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public List<long> isReceived = new List<long>();

        /// <summary>
        /// UpFile获取包
        /// </summary>
        /// <param name="_pn"></param>
        /// <returns></returns>
        private UnUdpEntity getUpFilePackage(long _pn)
        {
            UnUdpEntity ue = new UnUdpEntity();
            ue.Extent = this.Extent;
            ue.HashCode = this.HashCode;
            ue.TotalPacks = this.TotalPacks;
            ue.TotalSize = this.TotalSize;
            ue.SubSize = this.SubSize;
            if (_pn > 0)
            {
                ue.PackNo = _pn;
                ue.PackOffset = (ue.PackNo - 1) * ue.SubSize;
                ue.PackSize = ue.SubSize;
                if (ue.PackNo == ue.TotalPacks)
                {
                    ue.PackSize = (int)(ue.TotalSize - (ue.PackNo - 1) * ue.SubSize);
                }
                using (FileStream fs = new FileStream(this.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] data = new byte[ue.PackSize];
                    fs.Seek(ue.PackOffset, SeekOrigin.Begin);
                    fs.Read(data, 0, ue.PackSize);
                    ue.PackData = data;
                }
            }
            return ue;
        }

        /// <summary>
        /// UpFile获取发送包
        /// </summary>
        /// <param name="_pn"></param>
        /// <returns></returns>
        public byte[] getUpFileSendPackage(long _pn)
        {
            UnUdpEntity ue = getUpFilePackage(_pn);
            ue.Event = UnUdpEveEnum.upFilePackage.getText();
            return UnUdpHelp.assemblePackage(ue);
        }

        /// <summary>
        /// UpFile获取查询包
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public byte[] getUpFileQueryPackage(int state)
        {
            UnUdpEntity ue = getUpFilePackage(0);
            ue.Event = UnUdpEveEnum.upFileQuery.getText();
            ue.State = state;
            return UnUdpHelp.assemblePackage(ue);
        }

        /// <summary>
        /// Msg获取发送包
        /// </summary>
        /// <returns></returns>
        public byte[] getMsgSendPackage()
        {
            this.Event = UnUdpEveEnum.msgPackage.getText();
            return UnUdpHelp.assemblePackage(this);
        }

        /// <summary>
        /// DownFile获取发送包
        /// </summary>
        /// <param name="_pn"></param>
        /// <returns></returns>
        public byte[] getDownFileSendPackage(long _pn)
        {
            UnUdpEntity ue = new UnUdpEntity();
            ue.Event = UnUdpEveEnum.downFile.getText();
            ue.PackData = this.PackData;
            ue.PackSize = ue.PackData.Length;
            ue.PackMD5 = this.PackMD5;
            ue.PackNo = _pn;
            return UnUdpHelp.assemblePackage(ue);
        }

        /// <summary>
        /// DownFile获取查询包
        /// </summary>
        /// <param name="startNo"></param>
        /// <returns></returns>
        public byte[] getDownFileQueryPackage(long startNo)
        {
            UnUdpEntity ue = getUpFilePackage(0);
            ue.Event = UnUdpEveEnum.downFileQuery.getText();
            ue.PackData = this.PackData;
            ue.PackMD5 = this.PackMD5;
            ue.PackNo = startNo;
            return UnUdpHelp.assemblePackage(ue);
        }

        /// <summary>
        /// UpFile获取发送包
        /// </summary>
        /// <param name="_pn"></param>
        /// <returns></returns>
        public byte[] getDownFileBackPackage(long _pn)
        {
            UnUdpEntity ue = getUpFilePackage(_pn);
            ue.Event = UnUdpEveEnum.downFileBack.getText();
            return UnUdpHelp.assemblePackage(ue);
        }
    
    }
}
