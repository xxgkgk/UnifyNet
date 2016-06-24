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
    public class UnFileInfo
    {
        // 实例化
        public UnFileInfo() { }
      
        // 实例化
        public UnFileInfo(string path)
        {
            initt(path, null, null, null);
        }

        // 实例化
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

        // 基本信息
        [NonSerialized]
        [XmlIgnore]
        public FileInfo baseInfo = null;

        // 是否存在
        public bool exists = false;

        // 文件大小
        public long length = 0;

        // 文件创建时间
        public DateTime creationTime { get; set; }

        // 最后修改时间
        public DateTime lastWirteTime { get; set; }

        // 父文件夹
        public string parentFloder { get; set; }

        // 完整路径
        public string fullName { get; set; }

        // 临时路径
        [NonSerialized]
        [XmlIgnore]
        public string fullNameTmp;

        // 目录路径
        public string directoryName { get; set; }

        // 文件名
        public string name { get; set; }

        // 扩展名
        public string extens { get; set; }

        // md5标识
        public string md5 { get; set; }

        // mime
        public string mimeType { get; set; }

        // 文件过大
        [NonSerialized]
        [XmlIgnore]
        public bool isTooLarge = false;

        // 文件名错误
        [NonSerialized]
        [XmlIgnore]
        public bool isWrongName = false;

        // 错误扩展名
        [NonSerialized]
        [XmlIgnore]
        public bool isWrongExtens = false;

        // 错误类型
        [NonSerialized]
        [XmlIgnore]
        public bool isWrongType = false;

        // 是否过期
        [NonSerialized]
        [XmlIgnore]
        public bool isLate = false;

    }

}