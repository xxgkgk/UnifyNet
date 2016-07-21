using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnCommon.Config;
using UnCommon.Tool;
using Microsoft.Win32;

namespace UnCommon.Files
{
    /// <summary>
    /// 文件处理类
    /// </summary>
    public class UnFile
    {
        /// <summary>
        /// 返回根路径
        /// </summary>
        /// <returns></returns>
        public static string getHomeDirectory()
        {
            return UnInit.getHomeDirectory() + "UnifyHome";
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="ufe">文件事件</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isCover">是否覆盖</param>
        /// <returns></returns>
        public static FileInfo createFile(UnFileEvent ufe, string fileName, bool isCover)
        {
            var name = fileName;
            // 文件名是否非法
            var from = name.IndexOf(".");
            if (from < 0)
            {
                return null;
            }

            // 扩展名是否非法
            var ext = name.Substring(from);
            if (ext == null || ext.Length < 2)
            {
                return null;
            }

            // 随机生成文件名
            if (from == 0)
            {
                name = UnStrRan.getRandom() + name;
            }

            // 是否存在文件夹
            DirectoryInfo di = new DirectoryInfo(ufe.fullPath());
            if (di.Exists == false)
            {
                di.Create();
            }

            // 是否存在文件
            var path = ufe.fullPath() + "/" + name;
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists || isCover)
            {
                fi.Create().Dispose();
            }
            return fi;
        }

        /// <summary>
        /// 新建目录
        /// </summary>
        /// <param name="unfc"></param>
        /// <returns></returns>
        public static bool createDirectory(UnFileEvent unfc)
        {
            DirectoryInfo di = new DirectoryInfo(unfc.fullPath());
            if (di.Exists)
            {
                di.Create();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="ufe">事件</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static bool isExists(UnFileEvent ufe, string fileName)
        {
            // 是否存在文件
            var path = ufe.fullPath() + "/" + fileName;
            FileInfo fi = new FileInfo(path);
            return fi.Exists;
        }


        /// <summary>
        /// mime字典
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> dicMime()
        {
            var dicMIMEType = new Dictionary<string, string>();
            // 文本
            dicMIMEType["txt"] = "text/plain";
            dicMIMEType["xml"] = "text/xml";
            dicMIMEType["css"] = "text/css";
            dicMIMEType["html"] = "text/html";
            // 图片
            dicMIMEType["jpg"] = "image/jpeg,image/pjpeg";
            dicMIMEType["jpe"] = "image/jpeg";
            dicMIMEType["jpeg"] = "image/jpeg";
            dicMIMEType["gif"] = "image/gif";
            dicMIMEType["png"] = "image/png,image/x-png";
            dicMIMEType["bmp"] = "image/bmp";
            // 文档
            dicMIMEType["doc"] = "application/msword";
            dicMIMEType["dot"] = "application/msword";
            dicMIMEType["xls"] = "application/vnd.ms-excel,application/msexcel";
            dicMIMEType["ppt"] = "application/mspowerpoint";
            dicMIMEType["pdf"] = "application/pdf";
            dicMIMEType["p12"] = "application/x-pkcs12";
            dicMIMEType["pfx"] = "application/x-pkcs12";
            dicMIMEType["p7b"] = "application/x-pkcs12";
            // 压缩
            dicMIMEType["zip"] = "application/x-zip-compressed,application/zip";
            dicMIMEType["rar"] = "application/x-rar-compressed";
            // 媒体
            dicMIMEType["swf"] = "application/x-shockwave-flash";
            dicMIMEType["mp3"] = "audio/mpeg";
            dicMIMEType["wav"] = "audio/x-wav";
            dicMIMEType["wmv"] = "audio/x-ms-wmv";
            dicMIMEType["midi"] = "audio/x-midi";
            dicMIMEType["mp4"] = "video/mp4";
            dicMIMEType["avi"] = "video/x-msvideo";
            dicMIMEType["rmvb"] = "video/vnd.rn-realvideo";
            dicMIMEType["3gp"] = "video/3gpp";
            dicMIMEType["mov"] = "video/quicktime";
            dicMIMEType["mpeg"] = "video/mpeg";
            // 其它
            dicMIMEType["apk"] = "application/vnd.android.package-archive";
            return dicMIMEType;
        }

        /// <summary>
        /// 获取文件扩展名组
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public static List<string> expandedFromMime(string mime)
        {
            var s = new List<string>();
            foreach (KeyValuePair<string, string> d in dicMime())
            {
                if (("," + d.Value + ",").IndexOf("," + (mime + "").ToLower() + ",") >= 0)
                {
                    s.Add(d.Key);
                }
            }
            return s;
        }

        /// <summary>
        /// 获取文件扩展名组
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static List<string> expandedFormFile(Stream fs)
        {
            List<string> list = new List<string>();
            string TypeCode = null;
            BinaryReader r = new BinaryReader(fs);
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                TypeCode = buffer.ToString();
                buffer = r.ReadByte();
                TypeCode += buffer.ToString();
            }
            catch
            {
            }
            switch (TypeCode)
            {
                case "255216":
                    list.Add("jpg");
                    break;
                case "7173":
                    list.Add("gif");
                    break;
                case "6677":
                    list.Add("bmp");
                    break;
                case "13780":
                    list.Add("png");
                    break;
                case "7790":
                    list.Add("com");
                    list.Add("dll");
                    list.Add("exe");
                    break;
                case "8297":
                    list.Add("rar");
                    break;
                case "8075":
                    list.Add("zip");
                    break;
                case "6063":
                    list.Add("xml");
                    break;
                case "6033":
                    list.Add("html");
                    break;
                case "239187":
                    list.Add("aspx");
                    break;
                case "117115":
                    list.Add("cs");
                    break;
                case "119105":
                    list.Add("js");
                    break;
                case "210187":
                    list.Add("txt");
                    break;
                case "255254":
                    list.Add("sql");
                    list.Add("rdp");
                    break;
                case "64101":
                    list.Add("bat");
                    break;
                case "10056":
                    list.Add("batseed");
                    break;
                case "5666":
                    list.Add("psd");
                    break;
                case "3780":
                    list.Add("pdf");
                    break;
                case "7384":
                    list.Add("chm");
                    break;
                case "70105":
                    list.Add("log");
                    break;
                case "8269":
                    list.Add("reg");
                    break;
                case "6395":
                    list.Add("hlp");
                    break;
                case "208207":
                    list.Add("doc");
                    list.Add("xls");
                    list.Add("docx");
                    list.Add("xlsx");
                    break;
                case null:
                    list.Add("unknow-");
                    break;
                default:
                    list.Add("unknow-" + TypeCode);
                    break;
            }
            return list;
        }

        /// <summary>
        /// 获取文件扩展名组
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> expandedFormFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                List<string> list = expandedFormFile(fs);
                // 流不能判断则通过mime判断
                if (list.Count == 1 && list[0].Contains("unknow-"))
                {
                    string mime = getMimeType(fileName);
                    list = expandedFromMime(mime);
                }
                return list;
            }
        }

        /// <summary>
        /// 获取文件mime类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string getMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="pre">文件名前缀</param>
        /// <param name="str">日志内容</param>
        public static void writeLog(string pre, string str)
        {
            writeLog(pre, str, true);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="pre">文件名前缀</param>
        /// <param name="str">内容</param>
        /// <param name="addRandom">是否添加随机后缀</param>
        public static void writeLog(string pre, string str,bool addRandom)
        {
            writeLog(pre, str, addRandom, "txt");
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="pre">文件名前缀</param>
        /// <param name="str">内容</param>
        /// <param name="addRandom">是否添加随机后续</param>
        /// <param name="ext">文件扩展名</param>
        public static void writeLog(string pre, string str, bool addRandom,string ext)
        {
            try
            {
                String name = pre + "." + ext;
                if (addRandom)
                {
                    name = pre + "_" + UnStrRan.getRandom() + "." + ext;
                }
                string filePath = UnFile.createFile(UnFileEvent.log, name, true).FullName;
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(str);
                    sw.Flush();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 查找文件
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="md5">md5码</param>
        /// <returns></returns>
        public static UnFileInfo findFromDisc(string dir, string md5)
        {
            try
            {
                dir = dir.Replace("/", @"\");
                string home = UnFile.getHomeDirectory();
                int length = home.IndexOf("/");
                string drive = home.Substring(0, length);
                UnFileScanner fs = new UnFileScanner();
                IEnumerable<string> a = fs.EnumerateFiles(drive);
                foreach (string name in a)
                {
                    if (name.IndexOf(dir) > -1 && name.IndexOf(md5 + ".") > -1)
                    {
                        return new UnFileInfo(name);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        /// <summary>
        /// 从文件夹中查找文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static UnFileInfo findFromDir(string dir, string md5)
        {
            dir = dir.Replace("/", @"\");
            DirectoryInfo di0 = new DirectoryInfo(dir);
            if (!di0.Exists)
            {
                return null;
            }
            //遍历文件
            FileInfo[] fis0 = di0.GetFiles();
            foreach (FileInfo f0 in fis0)
            {
                string name = f0.FullName;
                if (name.IndexOf(dir) > -1 && name.IndexOf(md5 + ".") > -1)
                {
                    return new UnFileInfo(name);
                }
            }
            //遍历文件夹
            DirectoryInfo[] dis0 = di0.GetDirectories();
            foreach (DirectoryInfo di in dis0)
            {
                UnFileInfo f = findFromDir(di.FullName, md5);
                if (f != null)
                {
                    return f;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据MD5码查找文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        private static UnFileInfo findFromMD5(string dir, string md5)
        {

            try
            {
                return findFromDisc(dir, md5);
            }
            catch
            {
                return findFromDir(dir, md5);
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="s"></param>
        /// <param name="exps"></param>
        /// <param name="max"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static UnFileInfo saveFile(Stream s, string exps, long max, string md5)
        {
            UnFileInfo fa = new UnFileInfo();
            // 校验文件大小
            if (s.Length > max)
            {
                fa.isTooLarge = true;
                return fa;
            }
            // 校验文件类型
            string kzm = "no";
            List<string> exts = UnFile.expandedFormFile(s);
            foreach (string ext in exts)
            {
                if (("," + exps + ",").IndexOf("," + ext + ",") > -1)
                {
                    kzm = ext;
                    break;
                }
            }

            if (exps != null && kzm == "no")
            {
                fa.isWrongExtens = true;
                return fa;
            }

            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, buffer.Length);
            FileInfo fi = UnFile.createFile(UnFileEvent.important, md5 + "." + kzm, true);
            using (FileStream fs = new FileStream(fi.FullName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
                fs.Close();
            }
            s.Close();
            fa = new UnFileInfo(fi.FullName);
            return fa;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <param name="isCorver"></param>
        /// <returns></returns>
        public static bool move(string oldPath, string newPath, bool isCorver)
        {
            FileInfo oldFile = new FileInfo(oldPath);
            if (!oldFile.Exists)
            {
                return false;
            }
            UnFileInfo newInfo = new UnFileInfo(newPath);
            DirectoryInfo di = new DirectoryInfo(newInfo.directoryName);
            if (!di.Exists)
            {
                di.Create();
            }
            // 存在则删除
            FileInfo newFile = new FileInfo(newPath);
            if (newFile.Exists)
            {
                // 覆盖
                if (isCorver)
                {
                    newFile.Delete();
                }
                else
                {
                    return false;
                }
            }
            File.Move(oldPath, newPath);
            return true;
        }
    }
}