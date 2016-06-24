using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;
using UnCommon.XMMP;
using UnCommon.Config;
using UnCommon.Files;
using UnCommon.Extend;
using UnCommon.Entity;

namespace UnCommon.UDP
{
    public class UnUdpHelp
    {

        // 组装数据包
        public static byte[] assemblePackage(UnUdpEntity ue)
        {
            // 包体大小
            if (ue.PackData == null)
            {
                ue.PackData = new byte[0];
            }
            ue.PackSize = ue.PackData.Length;

            // 组装包头
            StringBuilder str = new StringBuilder();
            str.AppendFormat("{0}:{1};", new string[] { "b", ue.Event });                      //事件
            if (ue.Extent != null)
            {
                str.AppendFormat("{0}:{1};", new string[] { "c", ue.Extent });                     //文件扩展名
            }
            if (ue.HashCode != null)
            {
                str.AppendFormat("{0}:{1};", new string[] { "d", ue.HashCode });                   //文件MD5
            }
            if (ue.PackMD5 != null)
            {
                str.AppendFormat("{0}:{1};", new string[] { "e", ue.PackMD5 });                    //包MD5
            }
            if (ue.PackNo > 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "f", ue.PackNo.ToString() });          //包编号,1开始
            }
            if (ue.PackOffset > 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "g", ue.PackOffset.ToString() });      //包偏移量
            }
            if (ue.PackSize > 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "h", ue.PackSize.ToString() });        //包大小
            }
            if (ue.SubSize > 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "i", ue.SubSize.ToString() });         //分包大小
            }
            if (ue.TotalPacks > 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "j", ue.TotalPacks.ToString() });      //总包数
            }
            if (ue.TotalSize > 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "k", ue.TotalSize.ToString() });       //总大小
            }
            if (ue.UpCount > 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "l", ue.UpCount.ToString() });         //已上传数量
            }
            if (ue.IntMin != 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "m", ue.IntMin.ToString() });          //左区间
            }
            if (ue.IntMax != 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "n", ue.IntMax.ToString() });          //右区间
            }
            if (ue.State != 0)
            {
                str.AppendFormat("{0}:{1};", new string[] { "o", ue.State.ToString() });           //状态
            }

            // 包头长度 = 包体长度 + 包开头字符串长度(默认6)
            int headSize = (str.Length + 6);
            // 包开头字符串
            string al = "a:" + headSize + ";";
            // 小于6位则补齐
            if (al.Length < 6)
            {
                al = "a:0" + headSize + ";";
            }
            // 插入首位
            str.Insert(0, al);

            byte[] head = UnInit.getEncoding().GetBytes(str.ToString());
            byte[] pack = new byte[headSize + ue.PackSize];
            try
            {
                // 组装包
                Buffer.BlockCopy(head, 0, pack, 0, head.Length);
                Buffer.BlockCopy(ue.PackData, 0, pack, headSize, ue.PackData.Length);
            }
            catch (Exception e)
            {
                UnFile.writeLog("assemblePackage", e.ToString() + "\r\n" + headSize + "\r\n" + str.ToString());
                return null;
            }
            return pack;
        }

        // 解析数据包
        public static UnUdpEntity analyzePackage(byte[] data)
        {
            // 包最小为512
            if (data == null && data.Length < 512)
            {
                return null;
            }

            UnUdpEntity ue = new UnUdpEntity();
            // 包开头必须为:a:000;即最多6字节
            byte[] head = new byte[6];
            Buffer.BlockCopy(data, 0, head, 0, 6);

            // 取出包头大小
            string v = null;
            v = head.subStringBetween("a:", ";");
            if (v != null)
            {
                // 取出包头数据
                ue.HeadSize = Convert.ToInt32(v);
                head = new byte[ue.HeadSize];
                Buffer.BlockCopy(data, 0, head, 0, ue.HeadSize);
            }
            else
            {
                // 错误格式忽略
                return null;
            }

            ue.Event = head.subStringBetween("b:", ";");
            ue.Extent = head.subStringBetween("c:", ";");
            ue.HashCode = head.subStringBetween("d:", ";");
            ue.PackMD5 = head.subStringBetween("e:", ";");
            v = head.subStringBetween("f:", ";");
            if (v != null)
            {
                ue.PackNo = Convert.ToInt64(v);
            }
            v = head.subStringBetween("g:", ";");
            if (v != null)
            {
                ue.PackOffset = Convert.ToInt64(v);
            }
            v = head.subStringBetween("g:", ";");
            if (v != null)
            {
                ue.PackOffset = Convert.ToInt64(v);
            }
            v = head.subStringBetween("h:", ";");
            if (v != null)
            {
                ue.PackSize = Convert.ToInt32(v);
            }
            v = head.subStringBetween("i:", ";");
            if (v != null)
            {
                ue.SubSize = Convert.ToInt32(v);
            }
            v = head.subStringBetween("j:", ";");
            if (v != null)
            {
                ue.TotalPacks = Convert.ToInt64(v);
            }
            v = head.subStringBetween("k:", ";");
            if (v != null)
            {
                ue.TotalSize = Convert.ToInt64(v);
            }
            v = head.subStringBetween("l:", ";");
            if (v != null)
            {
                ue.UpCount = Convert.ToInt64(v);
            }
            v = head.subStringBetween("m:", ";");
            if (v != null)
            {
                ue.IntMin = Convert.ToInt64(v);
            }
            v = head.subStringBetween("n:", ";");
            if (v != null)
            {
                ue.IntMax = Convert.ToInt64(v);
            }
            v = head.subStringBetween("o:", ";");
            if (v != null)
            {
                ue.State = Convert.ToInt32(v);
            }

            // 取出包体
            ue.PackData = new byte[ue.PackSize];
            Buffer.BlockCopy(data, ue.HeadSize, ue.PackData, 0, ue.PackData.Length);
            return ue;
        }

        // 解析数据包
        public static UnUdpEntity analyzePackage(string xml)
        {
            return (UnUdpEntity)UnXMMPXml.xmlToT(typeof(UnUdpEntity), xml);
        }

        // 获得上传文件临时文件夹
        public static string getUpFileTmpDirectory(string md5)
        {
            return UnFileEvent.tmp.fullPath() + "upFile/" + md5;
        }

        // 获得配置文件路径
        public static string getUpFileTmpConfigPath(string md5)
        {
            return getUpFileTmpDirectory(md5) + "/config.unify";
        }

        // 获得接收文件路径
        public static string getUpFileReceivePath(string md5)
        {
            return getUpFileTmpDirectory(md5) + "/tmp.unify";
        }

        // 获得保存文件夹
        public static string getUpFileSaveDirectory()
        {
            return UnFileEvent.preference.fullPath() + "/upFile";
        }

        public static string getUpFileSavePath(string HashCode, string Extent)
        {
            return getUpFileSaveDirectory() + "/" + HashCode + Extent;
        }

        // DownFile-临时文件夹
        public static string getDownFileTmpDirectory(string md5)
        {
            return UnFileEvent.tmp.fullPath() + "downFile/" + md5;
        }

        // DownFile-获得配置文件路径
        public static string getDownFileTmpConfigPath(string md5)
        {
            return getDownFileTmpDirectory(md5) + "/config.unify";
        }

        // DownFile-获得接收文件路径
        public static string getDownFileReceivePath(string md5)
        {
            return getDownFileTmpDirectory(md5) + "/tmp.unify";
        }

        // DownFile-获得保存文件夹
        public static string getDownFileSaveDirectory()
        {
            return UnFileEvent.caches.fullPath() + "/downFile";
        }

        public static string getDownFileSavePath(string HashCode, string Extent)
        {
            return getDownFileSaveDirectory() + "/" + HashCode + Extent;
        }

        private const string PortReleaseGuid = "8875BD8E-4D5B-11DE-B2F4-691756D89593";

        // 查找可用udp端口
        public static int findUDPPort(int startPort)
        {
            int port = startPort;
            bool isAvailable = true;

            var mutex = new Mutex(false, string.Concat("Global/", PortReleaseGuid));
            mutex.WaitOne();
            try
            {
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints = ipGlobalProperties.GetActiveUdpListeners();
                do
                {
                    if (!isAvailable)
                    {
                        port++;
                        isAvailable = true;
                    }

                    foreach (IPEndPoint endPoint in endPoints)
                    {
                        if (endPoint.Port != port)
                            continue;
                        isAvailable = false;
                        break;
                    }

                } while (!isAvailable && port < IPEndPoint.MaxPort);

                if (!isAvailable)
                    throw new ApplicationException("Not able to find a free TCP port.");

                return port;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        // 区间计算
        public static long[] waitUps(long start, long tpn, int interval)
        {
            long[] ls = new long[2];
            long end = start + interval - 1;
            if (end > tpn)
            {
                end = tpn;
            }
            if (start > tpn)
            {
                start = -1;
                end = -1;
            }
            ls[0] = start;
            ls[1] = end;
            return ls;
        }
    }

}
