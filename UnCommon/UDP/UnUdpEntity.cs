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
    public class UnUdpEntity
    {
        // 实例化
        public UnUdpEntity()
        { 
        }

        // 实例化(文件分包)
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

        // 实例化(消息分包)
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

        // 实例化(下载)
        public UnUdpEntity(string code, bool isDown)
        {
            this.PackData = UnInit.getEncoding().GetBytes(code);
            this.PackMD5 = UnEncMD5.getMd5Hash(code);
            this.PackSize = this.PackData.Length;
        }

        // 事件
        public string Event { get; set; }
        // 区间值
        public long IntMin { get; set; }
        // 区间值
        public long IntMax { get; set; }
        // 已传包数
        public long UpCount { get; set; }
        // 扩展名
        public string Extent { get; set; }
        // HashCode
        public string HashCode { get; set; }
        // 分包大小
        public int SubSize { get; set; }
        // 总包数
        public long TotalPacks { get; set; }
        // 总大小
        public long TotalSize { get; set; }
        // 包编号
        public long PackNo { get; set; }
        // 包偏移量
        public long PackOffset { get; set; }
        // 包内容大小
        public int PackSize { get; set; }
        // 包md5
        public string PackMD5 { get; set; }
        // 状态(-2:文件过大,-1:文件类型错误,0:上传中,1:上传成功,2:极速秒传)
        public int State { get; set; }
        // 原文件名(不含扩展名)
        public string OgnName { get; set; }

        // 头大小
        public int HeadSize { get; set; }
        // 包内容
        public byte[] PackData { get; set; }
        // 临时路径
        public string TmpPath { get; set; }

        // 文件路径
        public string FullName { get; set; }

        // 终结点
        public EndPoint Point { get; set; }

        // 唤醒时间
        [NonSerialized]
        [XmlIgnore]
        public decimal WakeTimeStamp = 0;
        // 是否发送
        [NonSerialized]
        [XmlIgnore]
        public bool isSend = false;
        // 已接收包
        [NonSerialized]
        [XmlIgnore]
        public List<long> isReceived = new List<long>();

        // UpFile获取包
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

        // UpFile获取发送包
        public byte[] getUpFileSendPackage(long _pn)
        {
            UnUdpEntity ue = getUpFilePackage(_pn);
            ue.Event = UnUdpEveEnum.upFilePackage.getText();
            return UnUdpHelp.assemblePackage(ue);
        }

        // UpFile获取查询包
        public byte[] getUpFileQueryPackage(int state)
        {
            UnUdpEntity ue = getUpFilePackage(0);
            ue.Event = UnUdpEveEnum.upFileQuery.getText();
            ue.State = state;
            return UnUdpHelp.assemblePackage(ue);
        }

        // Msg获取发送包
        public byte[] getMsgSendPackage()
        {
            this.Event = UnUdpEveEnum.msgPackage.getText();
            return UnUdpHelp.assemblePackage(this);
        }

        // DownFile获取发送包
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

        // DownFile获取查询包
        public byte[] getDownFileQueryPackage(long startNo)
        {
            UnUdpEntity ue = getUpFilePackage(0);
            ue.Event = UnUdpEveEnum.downFileQuery.getText();
            ue.PackData = this.PackData;
            ue.PackMD5 = this.PackMD5;
            ue.PackNo = startNo;
            return UnUdpHelp.assemblePackage(ue);
        }

        // UpFile获取发送包
        public byte[] getDownFileBackPackage(long _pn)
        {
            UnUdpEntity ue = getUpFilePackage(_pn);
            ue.Event = UnUdpEveEnum.downFileBack.getText();
            return UnUdpHelp.assemblePackage(ue);
        }
    
    }
}
