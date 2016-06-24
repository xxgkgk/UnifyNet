using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using UnCommon.UDP;
using System.Threading;
using UnCommon.Interfaces;
using UnCommon.Entity;
using UnCommon;
using System.Net;
using UnCommon.XMMP;
using UnCommon.Files;
using System.Runtime.InteropServices;
using UnCommon.Encrypt;
using UnCommon.Config;

namespace UnVcUdpDownServer
{
    class Program
    {
        public delegate bool ConsoleCtrlDelegate(int ctrlType);

        // 端口号
        static int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);

        // 窗口阀值
        static int interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);

        // 任务数阀值
        static int tasks = Convert.ToInt32(ConfigurationManager.AppSettings["Tasks"]);

        // 接收包速度阀值
        static int packSpeed = Convert.ToInt32(ConfigurationManager.AppSettings["packSpeed"]);

        // 分包大小
        static int subSize = Convert.ToInt32(ConfigurationManager.AppSettings["SubSize"]);

        // 缓存队列大小
        static int cacheQueueSize = Convert.ToInt32(ConfigurationManager.AppSettings["CacheQueueSize"]);

        // 缓存有效时间
        static int cacheTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CacheTimeOut"]);

        // 线程状态
        static bool isTrue = true;

        // 事件监听对象
        static intSS ss = new intSS();

        static UnUdpServer server = null;

        // 入口
        static void Main(string[] args)
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = 30;
            Console.ForegroundColor = ConsoleColor.Green;

            server = new UnUdpServer(port);
            server.setInterval(interval);
            server.setDownSubSize(subSize);
            server.setCacheQueueSize(cacheQueueSize);
            server.setCacheTimeOut(cacheTimeOut);
            server.setIntSocketServer(ss);
            server.start();

            new Thread(udpTimerHandle).Start();
        }

        // 事件监听类
        public class intSS : UnIntUdpServer
        {
            UnAttrRst UnIntUdpServer.upStart(EndPoint point, UnUdpEntity entity)
            {
                return null;
            }

            UnAttrRst UnIntUdpServer.upSuccess(EndPoint point, UnUdpEntity entity)
            {
                return null;
            }

            UnAttrRst UnIntUdpServer.msgReceived(EndPoint point, UnUdpEntity entity)
            {
                return null;
            }

            UnAttrRst UnIntUdpServer.downCodeAnalyze(EndPoint point, UnUdpEntity entity)
            {
                try
                {
                    UnAttrRst rst = new UnAttrRst();
                    string code = UnInit.getEncoding().GetString(entity.PackData);
                    if (code.ToUpper().StartsWith("UNCODE//:"))
                    {
                        code = code.Remove(0, 9);
                        rst.code = 1;
                        rst.msg = "解码成功";
                        rst.back = UnEncDES.decrypt("ambo#$l3", code);
                        Console.Write("解码结果：" + rst.back);
                        if (code == rst.back)
                        {
                            rst.code = -1;
                            rst.msg = "错误UNCODE";
                        }
                    }
                    else
                    {
                        rst.code = -1;
                        rst.msg = "错误UNCODE";
                    }
                    return rst;
                }
                catch (Exception e)
                {
                    UnFile.writeLog("downCodeAnalyze", e.ToString());
                    return null;
                }
            }

            UnAttrRst UnIntUdpServer.proPackage(EndPoint point, UnUdpEntity entity)
            {
                try
                {
                    UnAttrRst rst = new UnAttrRst();
                    if (entity == null)
                    {
                        // 超过最大包速则丢弃
                        if (server.getStati().getReceiveNumSpeed() > packSpeed)
                        {
                            rst.code = -1;
                        }
                    }
                    else
                    {
                        // 超过最大任务数则丢弃
                        if (entity.PackNo == 1)
                        {
                            if (server.getStati().getDownTaskNum() > tasks)
                            {
                                rst.code = -2;
                            }
                        }
                    }
                    return rst;
                }
                catch (Exception e)
                {
                    UnFile.writeLog("proPackage", e.ToString());
                    return null;
                }
            }
        }

        // 计时业务线程
        private static void udpTimerHandle()
        {
            while (isTrue)
            {
                try
                {
                    Console.Clear();
                    UnAttrtStati ss = server.getStati();
                    string str = "\r\n********************************************************************************************\r\n";
                    str += "* 服务端口: " + port + " , ";
                    str += "滑动窗口: " + interval + " , ";
                    str += "任务阀: " + tasks + " 个 , ";
                    str += "包速阀: " + packSpeed + " N/S\r\n";
                    Console.WriteLine(str);
                    str = "* 分包大小: " + subSize + " B , ";
                    str += "最大缓存: " + cacheQueueSize + " , ";
                    str += "缓存时间: " + cacheTimeOut + " S\r\n";
                    Console.WriteLine(str);
                    str = "* 接收速度: " + ss.getReceiveSpeed() + " KB/S , ";
                    str += "峰值: " + ss.getReceiveSpeedPeak() + " KB/S\r\n";
                    Console.WriteLine(str);
                    str = "* 处理速度: " + ss.getProSpeed() + " KB/S , ";
                    str += "峰值: " + ss.getProSpeedPeak() + " KB/S\r\n";
                    Console.WriteLine(str);
                    str = "* 发送速度: " + ss.getSendSpeed() + " KB/S , ";
                    str += "峰值: " + ss.getSendSpeedPeak() + " KB/S\r\n";
                    Console.WriteLine(str);
                    str = "* 接收包速: " + ss.getReceiveNumSpeed() + " N/S , ";
                    str += "峰值: " + ss.getReceiveNumSpeedPeak() + " N/S\r\n";
                    Console.WriteLine(str);
                    str = "* 下载队列: " + ss.getDownTaskNum() + " 个 , ";
                    str += "峰值: " + ss.getDownTaskNumPeak() + " 个\r\n";
                    Console.WriteLine(str);
                    str = "* 文件队列: " + ss.getDownFileNum() + " 个 , ";
                    str += "峰值: " + ss.getDownFileNumPeak() + " 个\r\n";
                    Console.WriteLine(str);
                    str = "* 缓存队列: " + ss.getDownCacheNum() + " 个 , ";
                    str += "峰值: " + ss.getDownCacheNumPeak() + " 个";
                    str += "\r\n********************************************************************************************\r\n";
                    Console.WriteLine(str);
                }
                catch (Exception e)
                {
                    UnFile.writeLog("udpTimerHandle", e.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        //当用户关闭Console时，系统会发送次消息
        private const int CTRL_CLOSE_EVENT = 2;
        //Ctrl+C，系统会发送次消息
        private const int CTRL_C_EVENT = 0;
        //Ctrl+break，系统会发送次消息
        private const int CTRL_BREAK_EVENT = 1;
        //用户退出（注销），系统会发送次消息
        private const int CTRL_LOGOFF_EVENT = 5;
        //系统关闭，系统会发送次消息
        private const int CTRL_SHUTDOWN_EVENT = 6;

        private static bool HandlerRoutine(int ctrlType)
        {
            switch (ctrlType)
            {
                case CTRL_C_EVENT:
                    //MessageBox.Show("C");
                    break;
                case CTRL_BREAK_EVENT:
                    //MessageBox.Show("BREAK");
                    break;
                case CTRL_CLOSE_EVENT:
                    //MessageBox.Show("CLOSE");
                    server.close();
                    isTrue = false;
                    break;
                case CTRL_LOGOFF_EVENT:
                    break;
                case CTRL_SHUTDOWN_EVENT:
                    break;
            }
            //return true;//表示阻止响应系统对该程序的操作
            return false;//忽略处理，让系统进行默认操作
        }
    }
}
