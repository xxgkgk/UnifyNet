using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using UnCommon.Entity;
using UnCommon.Config;
using UnCommon.Files;
using UnCommon.Tool;
using UnCommon.Extend;
using UnCommon.Interfaces;
using UnCommon.XMMP;
using System.IO;

namespace UnCommon.UDP
{
    public class UnUdpClient
    {
        // 数据包对象
        UnUdpEntity upp = null;

        // 进程ID
        private int _pid = 0;

        // 完成
        private bool _isFinish = true;

        // 暂停
        private bool _isPause = false;

        // udp线程控制
        private AutoResetEvent udpAllDone = new AutoResetEvent(false);

        // udp端口
        public int udpPort = 0;

        // udp主机
        private IPEndPoint udpHost;

        // udp套接字
        private Socket udpSocket = null;

        // 通讯时间戳
        private decimal traTicks = 0;

        // 延时时间
        private int sleepTime = 2000;

        // 重发时间
        private int resendTime = 5000;

        // 超时时间
        private int timeOut = 2147483647;

        // 超时时间戳
        private decimal timeOutTicks = 0;


        // 进度列
        private List<UnAttrPgs> pgss = new List<UnAttrPgs>();

        // 监听接口
        private UnIntTransfer intTransfer = null;

        // 设置监听接口
        public void setIntTransfer(UnIntTransfer tran)
        {
            this.intTransfer = tran;
        }

        // 进度值max
        private float prgMax = 0;

        // 统计
        private UnAttrtStati ss = new UnAttrtStati();


        // 实例化
        public UnUdpClient(string host, int port)
        {
            this.udpPort = port;
            this.udpHost = new IPEndPoint(IPAddress.Parse(host), port);
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint uip = new IPEndPoint(IPAddress.Any, UnUdpHelp.findUDPPort(0));
            this.udpSocket.Bind(uip);
            this._pid = UnInit.pid();
        }

        // 线程ID
        public int pid()
        {
            return _pid;
        }

        // 暂停
        public void setPause(bool b)
        {
            this._isPause = b;
            if (!b)
            {
                // 设置超时时间戳
                timeOutTicks = UnDate.ticksMSec();
            }
        }

        // 获取暂停状态
        public bool isPause()
        {
            return _isPause;
        }

        // 设置超时时间
        public void setTimeOut(int time)
        {
            if (time < resendTime)
            {
                time = resendTime;
            }
            this.timeOut = time;
        }

        // 关闭
        public void close()
        {
            _isFinish = true;
        }

        // 监听线程
        private void udpListenHandle()
        {
            bool isLog = false;
            while (!_isFinish)
            {
                if (!_isPause)
                {
                    try
                    {
                        UnUdpState ao = new UnUdpState();
                        ao.socket = udpSocket;
                        ao.socket.BeginReceiveFrom(ao.receive, 0, ao.receive.Length, SocketFlags.None, ref ao.remote, new AsyncCallback(udpBeginReceiveFromBack), ao);
                        udpAllDone.WaitOne();
                        isLog = false;
                    }
                    catch (Exception ex)
                    {
                        udpAllDone.Set();
                        if (!isLog)
                        {
                            isLog = true;
                            UnFile.writeLog("udpListenHandle", ex.ToString());
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // 计时处理线程
        private void udpTimerHandle()
        {
            while (true)
            {
                if (!_isPause)
                {
                    if (pgss.Count > 0)
                    {
                        // 进度委托
                        if (intTransfer != null)
                        {
                            pgss[0].statistics = ss;
                            intTransfer.progress(pgss[0]);
                        }
                        if (pgss.Count > 0)
                        {
                            pgss.RemoveAt(0);
                        }
                        Thread.Sleep(500);
                    }
                    else
                    {
                        if (_isFinish)
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // udp接收回调
        private void udpBeginReceiveFromBack(IAsyncResult ar)
        {
            UnUdpState ao = (UnUdpState)ar.AsyncState;
            try
            {
                // 解析数据包
                ao.socket.EndReceiveFrom(ar, ref ao.remote);
                UnUdpEntity etBack = UnUdpHelp.analyzePackage(ao.receive);
                // 处理数据包
                switch (etBack.Event.getUnUdpEveEnum())
                {
                    // 上传文件
                    case UnUdpEveEnum.upFilePackageBack:
                    case UnUdpEveEnum.upFileQueryBack:
                        this.udpProUpFileBack(etBack);
                        break;
                    case UnUdpEveEnum.msgPackageBack:
                        this.udpProMsgBack(etBack);
                        break;
                    case UnUdpEveEnum.downFileQueryBack:
                    case UnUdpEveEnum.downFileBack:
                        this.udpProDownFileBack(etBack);
                        break;
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("udpBeginReceiveFromBack", ex.ToString());
            }
            finally
            {
                udpAllDone.Set();
            }
        }

        // udp发送包
        private void udpSendPack(byte[] send)
        {
            try
            {
                UnUdpState ao = new UnUdpState();
                ao.send = send;
                ao.socket = this.udpSocket;
                ao.socket.BeginSendTo(ao.send, 0, ao.send.Length, SocketFlags.None, udpHost, new AsyncCallback(udpBeginSendBack), ao);
            }
            catch(Exception e)
            {
                UnFile.writeLog("udpSendPack",e.ToString());
            }
        }

        // udp发送回调
        private void udpBeginSendBack(IAsyncResult ar)
        {
            UnUdpState ao = (UnUdpState)ar.AsyncState;
            ao.socket.EndSend(ar);
        }


        // UpFile
        public void upFile(string filePath)
        {
            if (_isFinish)
            {
                upp = new UnUdpEntity(filePath, subSize_up);
                // 启动监听、发送、业务线程
                _isFinish = false;
                setPause(false);
                new Thread(udpListenHandle).Start();
                new Thread(udpUpFileSendHandle).Start();
                new Thread(udpTimerHandle).Start();
            }
        }

        // UpFile-上传文件分包大小
        private int subSize_up = 1357;

        // UpFile-设置分包大小
        public void setUpSubSize(int subSize)
        {
            this.subSize_up = subSize;
        }

        // UpFile-窗口
        private List<UnUdpEntity> sendIntervals = new List<UnUdpEntity>();

        // UpFile-查询状态
        private int queryState = 1;

        // UpFile-发送线程
        private void udpUpFileSendHandle()
        {
            List<UnUdpEntity> list = new List<UnUdpEntity>();
            while (!_isFinish)
            {
                if (!_isPause)
                {
                    // 超时判断
                    if (UnDate.ticksMSec() - timeOutTicks < timeOut)
                    {
                        list = sendIntervals.FindAll(t => !t.isSend);
                        if (list.Count == 0)
                        {
                            if (UnDate.ticksMSec() - traTicks > resendTime)
                            {
                                // 如果首次查询已返回,则累加,否则一直默认为首次查询
                                if (queryState > 1)
                                {
                                    queryState++;
                                }
                                udpSendPack(upp.getUpFileQueryPackage(queryState));
                                traTicks = UnDate.ticksMSec();
                            }
                            else
                            {
                                Thread.Sleep(sleepTime);
                            }
                        }
                        else
                        {
                            foreach (var uup in list)
                            {
                                udpSendPack(upp.getUpFileSendPackage(uup.PackNo));
                                Thread.Sleep(1);
                            }
                            Thread.Sleep(sleepTime);
                        }
                    }
                    else
                    {
                        _isPause = true;
                        pgss.Clear();
                        if (intTransfer != null)
                        {
                            UnAttrRst rst = new UnAttrRst();
                            rst.pid = _pid;
                            rst.code = -1;
                            rst.msg = "发送超时!";
                            rst.back = "";
                            intTransfer.error(rst);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // UpFile-返回包处理
        private void udpProUpFileBack(UnUdpEntity etBack)
        {
            // 刷新超时时间戳
            timeOutTicks = UnDate.ticksMSec();
            switch (etBack.Event.getUnUdpEveEnum())
            {
                case UnUdpEveEnum.upFileQueryBack:
                    sleepTime = Convert.ToInt32(UnDate.ticksMSec() - traTicks) + 1;
                    if (sleepTime > 10000)
                    {
                        sleepTime = 2000;
                    }
                    switch (etBack.State)
                    {
                        case 1:
                        case 2:
                            _isFinish = true;
                            if (intTransfer != null)
                            {
                                UnAttrRst rst = (UnAttrRst)UnXMMPXml.xmlToT(typeof(UnAttrRst), UnInit.getEncoding().GetString(etBack.PackData));
                                rst.pid = _pid;
                                intTransfer.success(rst);
                            }
                            break;
                        case -1:
                        case -2:
                            _isFinish = true;
                            if (intTransfer != null)
                            {
                                UnAttrRst rst = (UnAttrRst)UnXMMPXml.xmlToT(typeof(UnAttrRst), UnInit.getEncoding().GetString(etBack.PackData));
                                rst.pid = _pid;
                                intTransfer.error(rst);
                            }
                            break;
                        default:
                            // 首次则累加
                            if (queryState == 1)
                            {
                                queryState++;
                            }
                            sendIntervals = new List<UnUdpEntity>();
                            for (long i = etBack.IntMin; i <= etBack.IntMax; i++)
                            {
                                UnUdpEntity uup = new UnUdpEntity();
                                uup.PackNo = i;
                                upp.isSend = false;
                                sendIntervals.Add(uup);
                            }
                            break;
                    }
                    break;
                case UnUdpEveEnum.upFilePackageBack:
                    UnUdpEntity up = sendIntervals.Find(t => t.PackNo == etBack.PackNo);
                    if (up != null)
                    {
                        up.isSend = true;
                        ss.addSendLength(etBack.PackSize);
                    }
                    udpAddUpProgress(etBack);
                    break;
            }
        }

        // UpFile-添加进度
        private void udpAddUpProgress(UnUdpEntity up)
        {
            UnAttrPgs pgs = new UnAttrPgs();
            pgs.pid = _pid;
            if (up.UpCount < upp.TotalPacks - 1)
            {
                pgs.length = up.UpCount * upp.SubSize;
            }
            else
            {
                if (up.UpCount == upp.TotalPacks)
                {
                    prgMax = 0;
                    pgss.Clear();
                }
                pgs.length = upp.TotalSize;
            }
            pgs.totalLength = upp.TotalSize;
            float per = pgs.percentage();
            if (per > prgMax)
            {
                if (pgss.Count < 20)
                {
                    prgMax = per;
                    pgss.Add(pgs);
                }
                else
                {
                    if ((int)per > (int)prgMax)
                    {
                        prgMax = per;
                        pgss.Add(pgs);
                        pgss.RemoveAt(0);
                    }
                }
            }
        }


        // SendMsg
        private void sendMsg(string msg)
        {
            if (_isFinish)
            {
                upp = new UnUdpEntity(UnInit.getEncoding().GetBytes(msg));
                // 添加待发送窗口
                sendIntervals.Clear();
                sendIntervals.Add(upp);
                // 启动监听、发送、业务线程
                _isFinish = false;
                setPause(false);
                new Thread(udpListenHandle).Start();
                new Thread(udpMsgSendHandle).Start();
                new Thread(udpTimerHandle).Start();
            }
        }

        // SendMsg-发送线程
        private void udpMsgSendHandle()
        {
            while (!_isFinish)
            {
                if (!_isPause)
                {
                    // 超时判断
                    if (UnDate.ticksMSec() - timeOutTicks < timeOut)
                    {
                        if (UnDate.ticksMSec() - traTicks > resendTime)
                        {
                            udpSendPack(upp.getMsgSendPackage());
                            traTicks = UnDate.ticksMSec();
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        _isFinish = true;
                        if (intTransfer != null)
                        {
                            UnAttrRst rst = new UnAttrRst();
                            rst.pid = _pid;
                            rst.code = -1;
                            rst.msg = "发送超时!";
                            rst.back = "";
                            intTransfer.error(rst);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // SendMsg-返回包处理
        private void udpProMsgBack(UnUdpEntity etBack)
        {
            switch (etBack.Event.getUnUdpEveEnum())
            {
                case UnUdpEveEnum.msgPackageBack:
                    if (sendIntervals[0].HashCode == etBack.HashCode)
                    {
                        _isFinish = true;
                        sendIntervals.RemoveAt(0);
                        if (intTransfer != null)
                        {
                            UnAttrRst rst = (UnAttrRst)UnXMMPXml.xmlToT(typeof(UnAttrRst), UnInit.getEncoding().GetString(etBack.PackData));
                            rst.pid = _pid;
                            intTransfer.success(rst);
                        }
                    }
                    break;
            }
        }


        // DownFile
        public void downFile(string downCode)
        {
            if (_isFinish)
            {
                upp = new UnUdpEntity(downCode, true);
                // 启动监听、发送、业务线程
                _isFinish = false;
                setPause(false);
                new Thread(udpListenHandle).Start();
                new Thread(udpDownFileSendHandle).Start();
                new Thread(udpTimerHandle).Start();
            }
        }

        // DownFile-下载文件发包休眠
        private int downSleep = 1;

        // DownFile-休眠区间MAX
        private int downSleepMax = 7;

        // DownFile-休眠区间MAX
        private int downSleepMin = 1;

        // DownFile-掉包率
        private float dbPer = 0;

        // DownFile-是否开始计算新一次掉包率
        private bool isdbPer = true;

        // DownFile-允许掉包最大值
        private float dbPerMax = 0.05F;

        // DownFile-允许掉包最小值
        private float dbPerMin = 0.01F;

        // DownFile-设置休眠区间(毫秒)
        public void setDownSleepInterval(int min, int max)
        {
            downSleepMin = min;
            downSleepMax = max;
        }

        // DownFile-设置掉包区间
        public void setDropRateInterval(float min, float max)
        {
            dbPerMin = min;
            dbPerMax = max;
        }

        // DownFile-获取掉包率
        public float getDropRate()
        {
            return dbPer;
        }

        // DownFile-窗口
        private List<UnUdpEntity> downIntervals = new List<UnUdpEntity>();

        // DownFile-状态
        private UnUdpEntity downQuery = new UnUdpEntity();

        // DownFile-发送线程
        private void udpDownFileSendHandle()
        {
            List<UnUdpEntity> list = new List<UnUdpEntity>();
            while (!_isFinish)
            {
                if (!_isPause)
                {
                    // 超时判断
                    if (UnDate.ticksMSec() - timeOutTicks < timeOut)
                    {
                        list = downIntervals.FindAll(t => !t.isSend);
                        if (list.Count == 0)
                        {
                            // 如果接收窗口完成,设定开始新窗口的包编号
                            if (downIntervals.Count > 0)
                            {
                                downQuery.PackNo = downQuery.UpCount;
                            }
                            if (UnDate.ticksMSec() - traTicks > resendTime)
                            {
                                udpSendPack(upp.getDownFileQueryPackage(downQuery.PackNo));
                                traTicks = UnDate.ticksMSec();
                                Console.WriteLine("新窗口：" + downQuery.PackNo);
                            }
                            else
                            {
                                Thread.Sleep(sleepTime);
                            }
                        }
                        else
                        {
                            if (isdbPer && list.Count < downIntervals.Count)
                            {
                                isdbPer = false;
                                dbPer = (float)list.Count / (float)downIntervals.Count;
                                if (dbPer > dbPerMax)
                                {
                                    downSleep++;
                                    if (downSleep > downSleepMax)
                                    {
                                        downSleep = downSleepMax;
                                    }
                                }
                                else if (dbPer < dbPerMin)
                                {
                                    downSleep--;
                                    if (downSleep < 1)
                                    {
                                        downSleep = downSleepMin;
                                    }
                                }
                                //Console.WriteLine("掉包：" + list.Count + "/" + downIntervals.Count + "/" + sleepTime + "/" + dbPer + "/" + downSleep);
                            }

                            foreach (var uup in list)
                            {
                                byte[] send = upp.getDownFileSendPackage(uup.PackNo);
                                udpSendPack(send);
                                Thread.Sleep(downSleep);
                            }
                            Thread.Sleep(sleepTime);
                        }
                    }
                    else
                    {
                        _isPause = true;
                        pgss.Clear();
                        if (intTransfer != null)
                        {
                            UnAttrRst rst = new UnAttrRst();
                            rst.pid = _pid;
                            rst.code = -1;
                            rst.msg = "下载超时!";
                            rst.back = "";
                            intTransfer.error(rst);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // DonwFile-返回包处理
        private void udpProDownFileBack(UnUdpEntity etBack)
        {
            // 刷新超时时间戳
            timeOutTicks = UnDate.ticksMSec();
            switch (etBack.Event.getUnUdpEveEnum())
            {
                case UnUdpEveEnum.downFileQueryBack:
                    sleepTime = Convert.ToInt32(UnDate.ticksMSec() - traTicks) + 1;
                    if (sleepTime > 10000)
                    {
                        sleepTime = 2000;
                    }
                    // 初始化
                    FileInfo cofFi = new FileInfo(UnUdpHelp.getDownFileTmpConfigPath(etBack.HashCode));
                    FileInfo tmpFi = new FileInfo(UnUdpHelp.getDownFileReceivePath(etBack.HashCode));
                    if (!cofFi.Exists)
                    {
                        DirectoryInfo di = new DirectoryInfo(UnUdpHelp.getDownFileTmpDirectory(etBack.HashCode));
                        if (!di.Exists)
                        {
                            di.Create();
                        }
                        cofFi.Create().Dispose();
                        tmpFi.Create().Dispose();
                        downQuery = etBack;
                    }

                    // 第一次初始化
                    if (downQuery.TotalPacks == 0)
                    {
                        // 获得配置文件
                        using (FileStream fs = cofFi.OpenRead())
                        {
                            byte[] b = new byte[fs.Length];
                            fs.Read(b, 0, b.Length);
                            downQuery = UnUdpHelp.analyzePackage(b);
                        }
                    }
                    else
                    {
                        downQuery.IntMin = etBack.IntMin;
                        downQuery.IntMax = etBack.IntMax;
                    }

                    // 写入配置文件
                    using (FileStream fs = new FileStream(UnUdpHelp.getDownFileTmpConfigPath(etBack.HashCode), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        byte[] prgBackBts = UnUdpHelp.assemblePackage(downQuery);
                        fs.SetLength(0);
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.Write(prgBackBts, 0, prgBackBts.Length);
                    }

                    // 建立窗口
                    downIntervals = new List<UnUdpEntity>();
                    for (long i = downQuery.IntMin; i <= downQuery.IntMax; i++)
                    {
                        UnUdpEntity uup = new UnUdpEntity();
                        uup.PackNo = i;
                        upp.isSend = false;
                        downIntervals.Add(uup);
                    }
                    isdbPer = true;
                    break;
                case UnUdpEveEnum.downFileBack:
                    UnUdpEntity up = downIntervals.Find(t => t.PackNo == etBack.PackNo && t.isSend == false);
                    string tmpPath = UnUdpHelp.getDownFileReceivePath(downQuery.HashCode);
                    if (up != null)
                    {
                        // 写入数据
                        using (FileStream fs = new FileStream(tmpPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            fs.Lock(etBack.PackOffset, etBack.PackData.Length);
                            fs.Seek(etBack.PackOffset, SeekOrigin.Begin);
                            fs.Write(etBack.PackData, 0, etBack.PackData.Length);
                            fs.Unlock(etBack.PackOffset, etBack.PackData.Length);
                            downQuery.UpCount++;
                            up.isSend = true;
                        }
                        udpAddDownProgress(downQuery);
                    }
                    // 下载完成
                    if (downQuery.UpCount == downQuery.TotalPacks)
                    {
                        _isFinish = true;
                        // 转正式文件
                        string newPath = UnUdpHelp.getDownFileSavePath(downQuery.HashCode, downQuery.Extent);
                        UnFile.move(tmpPath, newPath, true);
                        // 删除临时文件夹
                        File.Delete(UnUdpHelp.getDownFileTmpConfigPath(downQuery.HashCode));
                        Directory.Delete(UnUdpHelp.getDownFileTmpDirectory(downQuery.HashCode));
                        if (intTransfer != null)
                        {
                            UnAttrRst rst = new UnAttrRst();
                            rst.pid = _pid;
                            rst.code = 1;
                            rst.msg = "下载完成";
                            rst.back = newPath;
                            intTransfer.success(rst);
                        }
                    }
                    break;
            }
        }

        // DownFile-添加进度
        private void udpAddDownProgress(UnUdpEntity up)
        {
            UnAttrPgs pgs = new UnAttrPgs();
            pgs.pid = _pid;
            if (up.UpCount < up.TotalPacks - 1)
            {
                pgs.length = up.UpCount * up.SubSize;
            }
            else
            {
                if (up.UpCount == up.TotalPacks)
                {
                    prgMax = 0;
                    pgss.Clear();
                }
                pgs.length = up.TotalSize;
            }
            pgs.totalLength = up.TotalSize;
            float per = pgs.percentage();
            if (per > prgMax)
            {
                if (pgss.Count < 20)
                {
                    prgMax = per;
                    pgss.Add(pgs);
                }
                else
                {
                    if ((int)per > (int)prgMax)
                    {
                        prgMax = per;
                        pgss.Add(pgs);
                        pgss.RemoveAt(0);
                    }
                }
            }
        }


    }
}
