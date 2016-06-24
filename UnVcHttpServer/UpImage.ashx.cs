using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnCommon.HTTP;
using UnCommon.Entity;
using UnCommon.Files;
using System.IO;
using UnCommon.XMMP;
using System.Xml;
using UnEntity;
using UnCommon;
using System.Configuration;

namespace UnVcHttpServer
{
    /// <summary>
    /// UpImage 的摘要说明
    /// </summary>
    public class UpImage : IHttpHandler
    {
        public string HostPath = ConfigurationManager.AppSettings["HostPath"];
        public string HostDomain = ConfigurationManager.AppSettings["HostDomain"];

        public void ProcessRequest(HttpContext context)
        {
            BackInfo bi = new BackInfo();
            context.Response.ContentType = "text/plain";
            try
            {
                // 接收内容
                string eve = context.Request.Headers["eve"];
                string md5 = context.Request.Headers["md5"];
                string extens = context.Request.Headers["extens"];

                UnFileInfo fi = null;
                if (md5 + "" != "")
                {
                    // 查找文件
                    fi = UnFile.findFromDir(HostPath, md5);
                }
                if (fi == null)
                {
                    Stream s = context.Request.InputStream;
                    fi = saveUpFile(s, eve, md5);
                    if (fi == null)
                    {
                        bi.ReturnCode = -3;
                        bi.ReturnMsg = "参数错误：" + eve;
                        context.Response.Write(UnXMMPXml.tToXml(typeof(BackInfo), bi));
                        return;
                    }
                }
                else
                {
                    bi.ReturnCode = 2;
                    bi.ReturnMsg = "缓存文件";
                    bi.Url = HostDomain + fi.fullName.Replace(HostPath, "");
                    bi.MD5 = fi.md5;
                    context.Response.Write(UnXMMPXml.tToXml(typeof(BackInfo), bi));
                    return;
                }
                if (fi.isTooLarge)
                {
                    bi.ReturnCode = -1;
                    bi.ReturnMsg = "超过限制大小";
                    context.Response.Write(UnXMMPXml.tToXml(typeof(BackInfo), bi));
                    return;
                }
                if (fi.isWrongExtens)
                {
                    bi.ReturnCode = -2;
                    bi.ReturnMsg = "错误文件类型";
                    context.Response.Write(UnXMMPXml.tToXml(typeof(BackInfo), bi));
                    return;
                }
                bi.ReturnCode = 1;
                bi.ReturnMsg = "上传成功";
                bi.Url = HostDomain + fi.fullName.Replace(HostPath, "");
                bi.MD5 = fi.md5;
                context.Response.Write(UnXMMPXml.tToXml(typeof(BackInfo), bi));
            }
            catch (Exception e)
            {
                bi.ReturnCode = -100;
                bi.ReturnMsg = "系统错误";
                UnFile.writeLog("upFile_Error", e.ToString());
                context.Response.Write(UnXMMPXml.tToXml(typeof(BackInfo), bi));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // 保存文件
        public static UnFileInfo saveUpFile(Stream s, string eve, string md5)
        {
            switch (eve)
            {
                case "Image":
                    return UnFile.saveFile(s, "jpg,jpeg,png,gif,bmp", 1024 * 1024 * 5, md5);
                case "Doc":
                    return UnFile.saveFile(s, "txt,doc,dot,xls,ppt,pdf", 1024 * 1024, md5);
                case "Audio":
                    return UnFile.saveFile(s, "swf,mp3,mp4,wav,wmv,midi,avi,rmvb,3gp,mov,mpeg", 1024 * 1024, md5);
            }
            return null;
        }
    }
}