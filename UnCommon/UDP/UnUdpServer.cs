using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnCommon.Config;
using UnCommon.Entity;
using UnCommon.Extend;
using UnCommon.Files;
using UnCommon.Interfaces;
using UnCommon.Tool;
using UnCommon.XMMP;

namespace UnCommon.UDP
{
    public class UnUdpServer
    {
        // udp线程控制
        private static AutoResetEvent udpAllDone = new AutoResetEvent(false);

        // udp套接字
        private Socket udpSocket = null;

        // 窗口默认值
        private int interval = 450;

        // 是否运行
        private bool isTrue = true;

        // 事件接口
        private UnIntUdpServer intServer = null;

        // 设置事件接口
        public void setIntSocketServer(UnIntUdpServer ss)
        {
            this.intServer = ss;
        }

        // 统计
        private UnAttrtStati ss = new UnAttrtStati();

        // 获得即时统计
        public UnAttrtStati getStati()
        {
            return ss;
        }

        // 实例化
        public UnUdpServer(int udpPort)
        {
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint point = new IPEndPoint(IPAddress.Any, udpPort);
            udpSocket.Bind(point);
        }

        // 开始
        public void start()
        {
            isTrue = true;
            new Thread(udpListenHandle).Start();
            new Thread(upFileTimerHandle).Start();
            new Thread(downFileTimerHandle).Start();
        }

        // 关闭
        public void close()
        {
            isTrue = false;
        }

        private bool udpListenHandle_log = true;
        // 监听线程
        private void udpListenHandle()
        {
            while (isTrue)
            {
                try
                {
                    UnUdpState ao = new UnUdpState();
                    ao.socket = udpSocket;
                    //uint IOC_IN = 0x80000000;
                    //uint IOC_VENDOR = 0x18000000;
                    //uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                    //ao.socket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                    // 接收首位的数据报
                    ao.socket.BeginReceiveFrom(ao.receive, 0, ao.receive.Length, SocketFlags.None, ref ao.remote, new AsyncCallback(udpBeginReceiveFromBack), ao);
                    udpListenHandle_log = true;
                    udpAllDone.WaitOne();
                }
                catch (Exception ex)
                {
                    if (udpListenHandle_log)
                    {
                        udpListenHandle_log = false;
                        //UnFile.writeLog("udpListenHandle", ex.ToString());
                    }
                }
            }
        }


        private bool udpBeginReceiveFromBack_log= true;
        // 接收回调
        private void udpBeginReceiveFromBack(IAsyncResult ar)
        {
            UnUdpState ao = (UnUdpState)ar.AsyncState;
            try
            {
                int read = ao.socket.EndReceiveFrom(ar, ref ao.remote);
                ss.addReceiveLength(read);
                try
                {
                    ao.send = proPackage(ao);
                }
                catch (Exception ex)
                {
                    if (udpBeginReceiveFromBack_log)
                    {
                        udpBeginReceiveFromBack_log = false;
                        UnFile.writeLog("proPackage", ex.ToString());
                    }
                }
                if (ao.send != null)
                {
                    ao.socket.BeginSendTo(ao.send, 0, ao.send.Length, SocketFlags.None, ao.remote, new AsyncCallback(udpBeginSendToBack), ao);
                    ss.addSendLength(ao.send.Length);
                }
                udpBeginReceiveFromBack_log = true;
            }
            catch (Exception ex)
            {
                if (udpBeginReceiveFromBack_log)
                {
                    udpBeginReceiveFromBack_log = false;
                    //UnFile.writeLog("udpBeginReceiveFromBack", ex.ToString());
                }
            }
            finally
            {
                udpAllDone.Set();
            }
        }

        private bool udpBeginSendToBack_log = true;
        // 发送回调
        private void udpBeginSendToBack(IAsyncResult ar)
        {
            UnUdpState ao = (UnUdpState)ar.AsyncState;
            try
            {
                ao.socket.EndSendTo(ar);
                udpBeginSendToBack_log = true;
            }
            catch (Exception e)
            {
                if (udpBeginSendToBack_log)
                {
                    udpBeginSendToBack_log = false;
                    //UnFile.writeLog("udpBeginSendToBack", e.ToString());
                }
            }
        }

        // 处理数据包
        private byte[] proPackage(UnUdpState ao)
        {
            
            if (intServer != null)
            {
                UnAttrRst rst = intServer.proPackage(ao.remote, null);
                if (rst != null && rst.code < 0)
                {
                    return null;
                }
            }
           
            // 解析包数据
            UnUdpEntity entity = UnUdpHelp.analyzePackage(ao.receive);
            if (entity == null || ((entity.HashCode + "").Length < 16 && (entity.PackMD5 + "").Length < 16))
            {
                return null;
            }
            if (intServer != null)
            {
                UnAttrRst rst = intServer.proPackage(ao.remote, entity);
                if (rst != null && rst.code < 0)
                {
                    return null;
                }
            }

            ss.addReceiveNum(1);
            byte[] data = null;
            // 处理包数据
            switch (entity.Event.getUnUdpEveEnum())
            {
                // 上传文件
                case UnUdpEveEnum.upFilePackage:
                case UnUdpEveEnum.upFileQuery:
                    data = this.proUpFilePackage(ao.remote, entity);
                    break;
                case UnUdpEveEnum.msgPackage:
                    data = this.proMsgPackage(ao.remote, entity);
                    break;
                case UnUdpEveEnum.downFile:
                case UnUdpEveEnum.downFileQuery:
                    data = this.proDownFilePackage(ao.remote, entity);
                    break;
            }
      
            ss.addProLength(entity.PackSize);
            return data;
        }

        // 设置滑动窗口区间
        public void setInterval(int itv)
        {
            this.interval = itv;
        }

        // UpFile-文件配置列
        private List<UnUdpEntity> upFileConfigs = new List<UnUdpEntity>();

        // UpFile-更新线程
        private void upFileTimerHandle()
        {
            bool isLog = false;
            while (isTrue)
            {
                try
                {
                    for (int i = 0; i < upFileConfigs.Count; i++)
                    {
                        UnUdpEntity cofUPP = upFileConfigs[i];
                        byte[] cofUPPBts = UnUdpHelp.assemblePackage(cofUPP);
                        FileInfo cofFi = new FileInfo(UnUdpHelp.getUpFileTmpConfigPath(cofUPP.HashCode));
                        try
                        {
                            using (FileStream fs = cofFi.OpenWrite())
                            {
                                fs.SetLength(0);
                                fs.Seek(0, SeekOrigin.Begin);
                                fs.Write(cofUPPBts, 0, cofUPPBts.Length);
                            }
                        }
                        catch (Exception e)
                        {
                            UnFile.writeLog("udpUpdateConfigHandle", e.ToString());
                        }

                        // 超时移除
                        if (UnDate.ticksSec() - cofUPP.WakeTimeStamp > 5)
                        {
                            // 非正在传,则删除临时文件夹
                            if (cofUPP.State != 0)
                            {
                                File.Delete(UnUdpHelp.getUpFileReceivePath(cofUPP.HashCode));
                                File.Delete(UnUdpHelp.getUpFileTmpConfigPath(cofUPP.HashCode));
                                Directory.Delete(UnUdpHelp.getUpFileTmpDirectory(cofUPP.HashCode));
                            }
                            ss.addUpTaskNum(-1);
                            upFileConfigs.Remove(cofUPP);
                        }
                    }
                    isLog = false;
                }
                catch (Exception e)
                {
                    if (!isLog)
                    {
                        isLog = true;
                        UnFile.writeLog("upFileTimerHandle", e.ToString());
                    }
                }
                // 空闲睡眠
                if (upFileConfigs.Count == 0)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // UpFile-处理数据包
        private byte[] proUpFilePackage(EndPoint point, UnUdpEntity entity)
        {
            byte[] data = null;
            // 配置文件
            UnUdpEntity cofUPP = upFileConfigs.Find(t => t.HashCode == entity.HashCode);
            if (cofUPP == null)
            {
                cofUPP = new UnUdpEntity();
                cofUPP.HashCode = entity.HashCode;
                cofUPP.TotalPacks = entity.TotalPacks;
                cofUPP.TotalSize = entity.TotalSize;
                cofUPP.Extent = entity.Extent;
                cofUPP.SubSize = entity.SubSize;
                FileInfo cofFi = new FileInfo(UnUdpHelp.getUpFileTmpConfigPath(cofUPP.HashCode));
                FileInfo tmpFi = new FileInfo(UnUdpHelp.getUpFileReceivePath(cofUPP.HashCode));
                if (!cofFi.Exists)
                {
                    long[] ls = UnUdpHelp.waitUps(1, entity.TotalPacks, interval);
                    cofUPP.IntMin = ls[0];
                    cofUPP.IntMax = ls[1];
                    cofUPP.isReceived = new List<long>();
                    DirectoryInfo di = new DirectoryInfo(UnUdpHelp.getUpFileTmpDirectory(cofUPP.HashCode));
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    // 创建配置文件
                    using (FileStream fs = cofFi.Create())
                    {
                        byte[] prgBackBts = UnUdpHelp.assemblePackage(cofUPP);
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.Write(prgBackBts, 0, prgBackBts.Length);
                    }
                    // 创建临时文件
                    tmpFi.Create().Dispose();
                }
                else
                {
                    // 获得配置文件
                    using (FileStream fs = cofFi.OpenRead())
                    {
                        byte[] b = new byte[fs.Length];
                        fs.Read(b, 0, b.Length);
                        cofUPP = UnUdpHelp.analyzePackage(b);
                        if (cofUPP != null)
                        {
                            if (cofUPP.IntMin > 0)
                            {
                                cofUPP.UpCount = cofUPP.IntMin - 1;
                            }
                            else
                            {
                                cofUPP.UpCount = cofUPP.TotalPacks;
                            }
                        }
                    }
                }
                // 配置文件出错则删除重置
                if (cofUPP == null)
                {
                    cofFi.Delete();
                    return null;
                }
                ss.addUpTaskNum(1);
                upFileConfigs.Add(cofUPP);
            }
            // 唤醒时间
            cofUPP.WakeTimeStamp = UnDate.ticksSec();
            // 返回对象
            UnUdpEntity back = new UnUdpEntity();
            UnAttrRst rst = new UnAttrRst();

            switch (entity.Event.getUnUdpEveEnum())
            {
                case UnUdpEveEnum.upFileQuery:
                    back.IntMin = cofUPP.IntMin;
                    back.IntMax = cofUPP.IntMax;
                    back.TotalPacks = cofUPP.TotalPacks;
                    back.TotalSize = cofUPP.TotalSize;
                    back.UpCount = cofUPP.UpCount;
                    back.SubSize = cofUPP.SubSize;
                    back.Event = UnUdpEveEnum.upFileQueryBack.getText();

                    // 是否秒传
                    bool isMC = false;
                    // 开始上传
                    if (entity.State == 1)
                    {
                        if (intServer != null)
                        {
                            rst = intServer.upStart(point, entity);
                            if (rst != null)
                            {
                                cofUPP.State = rst.code;
                                back.State = cofUPP.State;
                                switch (cofUPP.State)
                                {
                                    case 1:// 传输完成
                                        cofUPP.UpCount = back.TotalPacks;
                                        back.UpCount = cofUPP.UpCount;
                                        cofUPP.WakeTimeStamp -=30;
                                        isMC = true;
                                        break;
                                    case -1:
                                    case -2:
                                        cofUPP.WakeTimeStamp -= 30;
                                        break;
                                }
                            }
                        }
                        // 默认处理
                        if (rst == null)
                        {
                            rst = new UnAttrRst();
                            rst.code = 0;
                            rst.msg = "传输中";
                        }
                    }

                    // 文件不是秒传,上传成功处理临时文件
                    if (!isMC && cofUPP.UpCount == cofUPP.TotalPacks)
                    {
                        cofUPP.State = 1;
                        back.State = cofUPP.State;
                        // 临时路径
                        string oldPath = UnUdpHelp.getUpFileReceivePath(cofUPP.HashCode);
                        if (intServer != null)
                        {
                            entity.TmpPath = oldPath;
                            rst = intServer.upSuccess(point, entity);
                            if (rst != null)
                            {
                                rst.code = 1;
                                rst.msg = "传输完成";
                            }
                        }
                        // 默认处理
                        if (rst == null)
                        {
                            // 转正式文件
                            string newPath = UnUdpHelp.getUpFileSavePath(cofUPP.HashCode, cofUPP.Extent);
                            UnFile.move(oldPath, newPath, true);
                            rst = new UnAttrRst();
                            rst.code = 1;
                            rst.msg = "上传成功";
                            rst.back = newPath;
                        }
                        cofUPP.WakeTimeStamp -= 25;
                    }

                    back.PackData = UnInit.getEncoding().GetBytes(UnXMMPXml.tToXml(typeof(UnAttrRst), rst));
                    data = UnUdpHelp.assemblePackage(back);
                    return data;
                case UnUdpEveEnum.upFilePackage:
                    // 在区间内且上传未完成
                    if (entity.PackNo >= cofUPP.IntMin && entity.PackNo <= cofUPP.IntMax && cofUPP.UpCount < cofUPP.TotalPacks)
                    {
                        // 是否已传
                        bool isHave = cofUPP.isReceived.Exists(t => t == entity.PackNo);
                        if (!isHave)
                        {
                            // 写入数据
                            using (FileStream fs = new FileStream(UnUdpHelp.getUpFileReceivePath(cofUPP.HashCode), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                fs.Lock(entity.PackOffset, entity.PackData.Length);
                                fs.Seek(entity.PackOffset, SeekOrigin.Begin);
                                fs.Write(entity.PackData, 0, entity.PackData.Length);
                                cofUPP.UpCount++;
                                cofUPP.isReceived.Add(entity.PackNo);

                                // 修改配置
                                if (cofUPP.UpCount == cofUPP.IntMax)
                                {
                                    long[] ls = UnUdpHelp.waitUps(cofUPP.IntMax + 1, entity.TotalPacks, interval);
                                    cofUPP.IntMin = ls[0];
                                    cofUPP.IntMax = ls[1];
                                    cofUPP.isReceived = new List<long>();
                                }
                                fs.Unlock(entity.PackOffset, entity.PackData.Length);
                            }
                        }
                    }

                    back.Event = UnUdpEveEnum.upFilePackageBack.getText();
                    back.PackNo = entity.PackNo;
                    back.UpCount = cofUPP.UpCount;
                    data = UnUdpHelp.assemblePackage(back);
                    return data;
            }
            return data;
        }


        // DownFile-分包大小
        private int subSize_down = 1357;

        // DownFile-缓存队列大小
        private int cacheQueueSize = 10000;

        // DownFile-缓存时间
        private int cacheTimeOut = 15;

        // DownFile-缓存锁
        private static object cacheLock = new object();

        // DownFile-设置分包大小
        public void setDownSubSize(int subSize)
        {
            subSize_down = subSize;
        }

        // DownFile-设置缓存队列大小
        public void setCacheQueueSize(int queueSize)
        {
            this.cacheQueueSize = queueSize;
        }

        // DownFile-设置缓存过期时间(秒)
        public void setCacheTimeOut(int timeOut)
        {
            this.cacheTimeOut = timeOut;
        }
        
        // DownFile-文件配置列
        private List<UnUdpEntity> downFileConfigs = new List<UnUdpEntity>();

        // DownFile-客户端列
        private List<UnUdpEntity> downFileClients = new List<UnUdpEntity>();

        // DownFile-包缓存列
        private List<UnUdpEntity> downFileCaches = new List<UnUdpEntity>();

        // DownFile-计时线程
        private void downFileTimerHandle()
        {
            bool isLog = false;
            while (isTrue)
            {
                try
                {
                    // 文件列
                    for (int i = 0; i < downFileConfigs.Count; i++)
                    {
                        UnUdpEntity cofUPP = downFileConfigs[i];
                        // 超时移除
                        if (UnDate.ticksSec() - cofUPP.WakeTimeStamp > 15)
                        {
                            downFileConfigs.Remove(cofUPP);
                            ss.addDownFileNum(-1);
                        }
                    }

                    // 任务列
                    for (int i = 0; i < downFileClients.Count; i++)
                    {
                        UnUdpEntity point = downFileClients[i];
                        // 超时移除
                        if (UnDate.ticksSec() - point.WakeTimeStamp > 15)
                        {
                            ss.addDownTaskNum(-1);
                            downFileClients.Remove(point);
                        }
                    }

                    // 缓存列(30秒缓存)
                    for (int i = 0; i < downFileCaches.Count; i++)
                    {
                        UnUdpEntity cache = downFileCaches[i];
                        // 超时移除
                        if (cache != null && (UnDate.ticksSec() - cache.WakeTimeStamp) > cacheTimeOut)
                        {
                            ss.addDownCacheNum(-1);
                            lock (cacheLock)
                            {
                                downFileCaches.Remove(cache);
                            }
                        }
                    }
                    isLog = false;
                }
                catch (Exception e)
                {
                    if (!isLog)
                    {
                        isLog = true;
                        UnFile.writeLog("downFileTimerHandle", e.ToString());
                    }
                }
                // 空闲睡眠
                if (downFileConfigs.Count == 0)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // DownFile-处理数据包
        private byte[] proDownFilePackage(EndPoint point, UnUdpEntity entity)
        {
            byte[] data = null;
            UnUdpEntity downBack = new UnUdpEntity();

            UnUdpEntity p = downFileClients.Find(t => t.Point.GetHashCode() == point.GetHashCode());
            // 建立客户端表
            if (p == null)
            {
                p = new UnUdpEntity();
                p.Point = point;
                downFileClients.Add(p);
                ss.addDownTaskNum(1);
            }
            p.WakeTimeStamp = UnDate.ticksSec();

            // 建立配置文件
            UnUdpEntity config = downFileConfigs.Find(t => t.PackMD5 == entity.PackMD5);
            if (config == null)
            {
                //UnFile.writeLog("downMD5", entity.PackMD5);
                string filePath = null;
                if (intServer != null)
                {
                    UnAttrRst rst = intServer.downCodeAnalyze(point, entity);
                    if (rst != null)
                    {
                        if (rst.code > 0)
                        {
                            filePath = rst.back;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                if (!File.Exists(filePath))
                {
                    return null;
                }
                config = new UnUdpEntity(filePath, subSize_down);
                config.PackMD5 = entity.PackMD5;
                downFileConfigs.Add(config);
                ss.addDownFileNum(1);
            }
            config.WakeTimeStamp = UnDate.ticksSec();

            switch (entity.Event.getUnUdpEveEnum())
            {
                case UnUdpEveEnum.downFileQuery:
                    downBack.Event = UnUdpEveEnum.downFileQueryBack.getText();
                    downBack.Extent = config.Extent;
                    downBack.HashCode = config.HashCode;
                    downBack.TotalPacks = config.TotalPacks;
                    downBack.TotalSize = config.TotalSize;
                    downBack.SubSize = config.SubSize;
                    long[] ls = UnUdpHelp.waitUps(entity.PackNo + 1, config.TotalPacks, interval);
                    downBack.IntMin = ls[0];
                    downBack.IntMax = ls[1];
                    data = UnUdpHelp.assemblePackage(downBack);
                    return data;
                case UnUdpEveEnum.downFile:
                    UnUdpEntity cache = null;
                    lock (cacheLock)
                    {
                        cache = downFileCaches.Find(t => t.PackNo == entity.PackNo && t.PackMD5 == config.PackMD5);
                        // 添加缓存
                        if (cache == null)
                        {
                            data = config.getDownFileBackPackage(entity.PackNo);
                            if (downFileCaches.Count < cacheQueueSize)
                            {
                                cache = new UnUdpEntity();
                                cache.PackMD5 = config.PackMD5;
                                cache.PackNo = entity.PackNo;
                                cache.PackData = data;
                                cache.WakeTimeStamp = UnDate.ticksSec();
                                downFileCaches.Add(cache);
                                ss.addDownCacheNum(1);
                            }
                        }
                        else
                        {
                            data = cache.PackData;
                            cache.WakeTimeStamp = UnDate.ticksSec();
                        }
                        ss.addProLength(config.SubSize);
                    }
                    return data;
            }
            return null;
        }


        // Msg-处理数据包
        private byte[] proMsgPackage(EndPoint point, UnUdpEntity entity)
        {
            UnUdpEntity msgBack = new UnUdpEntity();
            msgBack.Event = UnUdpEveEnum.msgPackageBack.getText();
            msgBack.HashCode = entity.HashCode;
            UnAttrRst rst = new UnAttrRst();
            rst.code = 0;
            rst.msg = "success";
            rst.back = "";
            if (intServer != null)
            {
                rst = intServer.msgReceived(point, entity);
            }
            return UnUdpHelp.assemblePackage(msgBack);
        }
    
    }
}
