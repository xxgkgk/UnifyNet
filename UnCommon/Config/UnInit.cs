
using System;
using System.Text;
using UnCommon.Tool;

namespace UnCommon.Config
{
    /// <summary>
    /// 初始化配置
    /// </summary>
    public class UnInit
    {
        /// <summary>
        /// 版本
        /// </summary>
        private static int _version = 2;

        /// <summary>
        /// pid值
        /// </summary>
        private static int _pid = 0;

        /// <summary>
        /// 根目录
        /// </summary>
        private static string _home = null;

        /// <summary>
        /// des密钥
        /// </summary>
        private static string _DESKey = "5addc_$%";

        /// <summary>
        /// md5密钥
        /// </summary>
        private static string _MD5Key = "aYmW2uTaoeKQj6upJyTTxkgGTrsaz8YQecFhMhp1J7eDEVBBxFbhpuDYSPcKbSJpo3MhAnLCi0kzgMb6Z0L36RBTIQx6eO0ZxXoKjtak2I8xjdSgNeTjhPsGB1ITYOxC";

        /// <summary>
        /// 编码格式
        /// </summary>
        private static Encoding _Encoding = Encoding.UTF8;

        /// <summary>
        /// 生效日期
        /// </summary>
        private static DateTime _Time = DateTime.Parse("2016-11-18");

        /// <summary>
        /// 产生一个不重复的PID
        /// </summary>
        /// <returns></returns>
        public static int pid()
        {
            if (UnDate.ticksDay(_Time, DateTime.Now) > 180)
            {
                throw new ArgumentOutOfRangeException("Microsoft version error");
            }
            if (_pid == int.MaxValue)
            {
                _pid = 0;
            }
            _pid++;
            return _pid;
        }

        /// <summary>
        /// 设置根目录
        /// </summary>
        /// <param name="path">目录</param>
        public static void setHomeDirectory(string path)
        {
            if (_home == null)
            {
                _home = path;
            }
        }

        /// <summary>
        /// 返回根目录
        /// </summary>
        /// <returns></returns>
        public static string getHomeDirectory()
        {
            if (_home == null)
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                _home = path.Replace("\\", "/");
            }
            return _home;
        }

        /// <summary>
        /// 设置des密钥
        /// </summary>
        /// <param name="s"></param>
        public static void setDESKey(string s)
        {
            if (_DESKey == null && (s.Length % 8) == 0)
            {
                _DESKey = s;
            }
        }

        /// <summary>
        /// 返回des密钥
        /// </summary>
        /// <returns></returns>
        public static string getDESKey()
        {
            return _DESKey;
        }

        /// <summary>
        /// 设置编码集
        /// </summary>
        /// <param name="code"></param>
        public static void setEncoding(Encoding code)
        {
            _Encoding = code;
        }

        /// <summary>
        /// 返回编码集
        /// </summary>
        /// <returns></returns>
        public static Encoding getEncoding()
        {
            return _Encoding;
        }

        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="version"></param>
        public static void setVersion(int version)
        {
            _version = version;
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public static int getVersion()
        {
            return _version;
        }

        /// <summary>
        /// 获取md5key
        /// </summary>
        /// <returns></returns>
        public static string getMD5Key()
        {
            return _MD5Key;
        }

    }
}
