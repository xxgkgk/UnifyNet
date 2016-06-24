
using System;
using System.Text;
namespace UnCommon.Config
{
    /// <summary>
    /// 初始化配置
    /// </summary>
    public class UnInit
    {
        // 版本
        private static int _version = 2;

        // pid值
        private static int _pid = 0;

        // 根目录 
        private static string _home = null;

        // des密钥
        private static string _DESKey = "5addc_$%";

        // 编码格式
        private static Encoding _Encoding = Encoding.UTF8;

        // 产生一个不重复的PID
        public static int pid()
        {
            if (_pid == int.MaxValue)
            {
                _pid = 0;
            }
            _pid++;
            return _pid;
        }

        // 设置根目录
        public static void setHomeDirectory(string s)
        {
            if (_home == null)
            {
                _home = s;
            }
        }

        // 返回根目录
        public static string getHomeDirectory()
        {
            if (_home == null)
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                _home = path.Replace("\\", "/");
            }
            return _home;
        }

        // 设置des密钥
        public static void setDESKey(string s)
        {
            if (_DESKey == null && (s.Length % 8) == 0)
            {
                _DESKey = s;
            }
        }

        // 返回des密钥
        public static string getDESKey()
        {
            return _DESKey;
        }

        // 设置编码集
        public static void setEncoding(Encoding code)
        {
            _Encoding = code;
        }

        // 返回编码集
        public static Encoding getEncoding()
        {
            return _Encoding;
        }

        // 设置版本号
        public static void setVersion(int version)
        {
            _version = version;
        }

        // 获取版本号
        public static int getVersion()
        {
            return _version;
        }

    }
}
