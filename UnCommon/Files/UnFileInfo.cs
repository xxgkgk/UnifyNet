using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using UnCommon.Encrypt;
using UnCommon.Tool;

namespace UnCommon.Files
{
    /// <summary>
    /// 文件信息类
    /// </summary>
    public class UnFileInfo
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public UnFileInfo() { }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="path"></param>
        public UnFileInfo(string path)
        {
            initt(path, null, null, null);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="allowSize"></param>
        /// <param name="allowType"></param>
        /// <param name="timeOutMsec"></param>
        public UnFileInfo(string path, long? allowSize, string allowType, long? timeOutMsec)
        {
            initt(path, allowSize, allowType, timeOutMsec);
        }

        // 初始方法
        private void initt(string path, long? allowSize, string allowType, long? timeOutMsec)
        {
            string p1 = path.Replace("\\", "/");
            this.fullName = p1;
            int a = p1.LastIndexOf("/");
            int b = p1.LastIndexOf(".");
            this.directoryName = p1.Substring(0, a);
            this.name = p1.Substring(a+1);
            this.extens = p1.Substring(b);
            this.fullNameTmp = UnFileEvent.tmp.fullPath() + this.name.Replace(this.extens, ".tmp");

            baseInfo = new FileInfo(path);
            if (baseInfo.Exists)
            {
                this.exists = baseInfo.Exists;
                this.length = baseInfo.Length;
                this.creationTime = baseInfo.CreationTime;
                this.lastWirteTime = baseInfo.LastWriteTime;
                this.md5 = UnEncMD5.getMd5Hash(baseInfo);
                this.mimeType = UnFile.getMimeType(path);

                if (allowSize != null)
                {
                    if (baseInfo.Length > allowSize)
                    {
                        this.isTooLarge = true;
                    }
                }

                if (allowType != null)
                {
                    this.isWrongExtens = true; ;
                    if ((allowType).Contains(this.extens))
                    {
                        this.isWrongExtens = false;;
                    }
                    this.isWrongType = true;
                    List<string> list = UnFile.expandedFormFile(path);
                    foreach (string ext in list)
                    {
                        if ((allowType).Contains("." + ext))
                        {
                            this.isWrongType = false;
                            break;
                        }
                    }
                }

                if (timeOutMsec != null)
                {
                    if (timeOutMsec >= 0)
                    {
                        var msec = UnDate.ticksMSec(this.lastWirteTime, DateTime.Now) - timeOutMsec;
                        if (msec > 0)
                        {
                            this.isLate = true;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 基本信息
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public FileInfo baseInfo = null;

        /// <summary>
        /// 是否存在
        /// </summary>
        public bool exists = false;

        /// <summary>
        /// 文件大小
        /// </summary>
        public long length = 0;

        /// <summary>
        /// 文件创建时间
        /// </summary>
        public DateTime creationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime lastWirteTime { get; set; }

        /// <summary>
        /// 父文件夹
        /// </summary>
        public string parentFloder { get; set; }

        /// <summary>
        /// 完整路径
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// 临时路径
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public string fullNameTmp;

        /// <summary>
        /// 目录路径
        /// </summary>
        public string directoryName { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string extens { get; set; }

        /// <summary>
        /// md5标识
        /// </summary>
        public string md5 { get; set; }

        /// <summary>
        /// mime
        /// </summary>
        public string mimeType { get; set; }

        /// <summary>
        /// 文件过大
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public bool isTooLarge = false;

        /// <summary>
        /// 文件名错误
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public bool isWrongName = false;

        /// <summary>
        /// 错误扩展名
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public bool isWrongExtens = false;

        /// <summary>
        /// 错误类型
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public bool isWrongType = false;

        /// <summary>
        /// 是否过期
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        public bool isLate = false;

    }

}