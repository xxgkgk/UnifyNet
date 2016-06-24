using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.UDP;
using System.Configuration;
using UnCommon.Interfaces;
using System.Threading;
using UnCommon.Entity;
using UnCommon;
using System.Net;
using UnCommon.XMMP;
using UnCommon.Files;
using System.Runtime.InteropServices;
using UnCommon.Encrypt;
using UnEntity;

namespace UnVcFileServer
{
    class Program
    {
        public delegate bool ConsoleCtrlDelegate(int ctrlType);

        // 端口号
        static int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);

        // 窗口阀值
        static int interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);

        // 允许类型
        static string fileTypes = ConfigurationManager.AppSettings["FileTypes"];

        // 文件大小阀值
        // 1M = 1048576
        static long fileSize = Convert.ToInt64(ConfigurationManager.AppSettings["FileSize"]);

        // 任务数阀值
        static int tasks = Convert.ToInt32(ConfigurationManager.AppSettings["Tasks"]);

        // 接收包速度阀值
        static int packSpeed = Convert.ToInt32(ConfigurationManager.AppSettings["packSpeed"]);

        // 存储路径
        static string SavePath = ConfigurationManager.AppSettings["SavePath"];


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
            server.setIntSocketServer(ss);
            server.start();

            new Thread(udpTimerHandle).Start();
        }

        // 事件监听类
        public class intSS : UnIntUdpServer
        {
            UnAttrRst UnIntUdpServer.upStart(EndPoint point, UnUdpEntity entity)
            {
                UnAttrRst rst = new UnAttrRst();
                BackInfo bi = new BackInfo();

                // 类型检测
                if (fileTypes.IndexOf(".*") < 0 && ("," + fileTypes.ToLower() + ",").IndexOf("," + entity.Extent.ToLower() + ",") < 0)
                {
                    bi.ReturnCode = -1;
                    bi.ReturnMsg = "只允许: " + fileTypes + " 格式";

                    rst.code = -1;
                    rst.msg = "文件类型错误";
                    rst.back = UnXMMPXml.tToXml(typeof(BackInfo), bi);
                    return rst;
                }
                // 大小检测
                if (entity.TotalSize > fileSize)
                {
                    bi.ReturnCode = -2;
                    bi.ReturnMsg = "文件超过: " + fileSize + " 字节";

                    rst.code = -2;
                    rst.msg = "文件大小错误";
                    rst.back = UnXMMPXml.tToXml(typeof(BackInfo), bi);
                    return rst;
                }
                // 查询文件是否已存在
                UnFileInfo fi = UnFile.findFromDir(SavePath, entity.HashCode);
                if (fi != null)
                {
                    bi.ReturnCode = 2;
                    bi.ReturnMsg = "极速秒传";
                    bi.MD5 = entity.HashCode;
                    bi.Url = SavePath + fi.fullName.Replace(SavePath, "");

                    rst.code = 1;
                    rst.msg = "传输完成";
                    rst.back = UnXMMPXml.tToXml(typeof(BackInfo), bi);

                    Console.WriteLine("***************极速秒传***************");
                    Console.WriteLine("【路径】" + fi.fullName);
                    Console.WriteLine("【URL】" + bi.Url);

                    return rst;
                }
                return null;
            }

            UnAttrRst UnIntUdpServer.upSuccess(EndPoint point, UnUdpEntity entity)
            {
                // 转正式文件
                string dicName = DateTime.Now.ToString("yyyy-MM-dd") + "/" + entity.HashCode + entity.Extent;
                string newPath = SavePath + dicName;
                UnFile.move(entity.TmpPath, newPath, true);

                // 自定义返回
                BackInfo bi = new BackInfo();
                bi.ReturnCode = 1;
                bi.ReturnMsg = "上传成功";
                bi.MD5 = entity.HashCode;
                bi.UNCode = "UNCODE//:" + UnEncDES.encrypt("ambo#$l3", newPath);

                UnAttrRst rst = new UnAttrRst();
                rst.back = UnXMMPXml.tToXml(typeof(BackInfo), bi);

                Console.WriteLine("***************上传成功***************");
                Console.WriteLine("【路径】" + newPath);
                Console.WriteLine("【UNCode】" + bi.UNCode);
                return rst;
            }

            UnAttrRst UnIntUdpServer.msgReceived(EndPoint point, UnUdpEntity entity)
            {
                UnAttrRst rst = new UnAttrRst();
                return new UnAttrRst();
            }

            UnAttrRst UnIntUdpServer.downCodeAnalyze(EndPoint point, UnUdpEntity entity)
            {
                return new UnAttrRst();
            }

            UnAttrRst UnIntUdpServer.proPackage(EndPoint point, UnUdpEntity entity)
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
                        if (server.getStati().getUpTaskNum() > tasks)
                        {
                            rst.code = -2;
                        }
                    }
                }
                return rst;
            }
        }

        // 计时业务线程
        private static void udpTimerHandle()
        {
            while (isTrue)
            {
                Console.Clear();
                UnAttrtStati ss = server.getStati();
                string str = "\r\n********************************************************************************************\r\n";
                str += "* 服务端口: " + port + " , ";
                str += "滑动窗口: " + interval + " , ";
                str += "任务阀: " + tasks + " 个 , ";
                str += "包速阀: " + packSpeed + " N/S\r\n";
                Console.WriteLine(str);
                str = "* 允许类型: " + fileTypes + " , ";
                str += "允许大小: " + fileSize + " 字节(" + Math.Round(fileSize * 1.00 / (1024 * 1024), 3) + " MB)\r\n";
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
                str = "* 接收任务: " + ss.getUpTaskNum() + " 个 , ";
                str += "峰值: " + ss.getUpTaskNumPeak() + " 个";
                str += "\r\n********************************************************************************************\r\n";
                Console.WriteLine(str);
                Thread.Sleep(5000);
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
