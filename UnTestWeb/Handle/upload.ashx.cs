using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using UnCommon.Encrypt;
using UnCommon.Entity;
using UnCommon.Files;
using UnCommon.Tool;
using UnCommon.XMMP;
using UnEntity;

namespace UnWebTool.Handle
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler, IRequiresSessionState
    {
        // 上传权限状态
        int upState = 0;

        // 图片路径
        public static string ImgPath = "/unifyhome/important/";

        // 处理接收数据
        public void ProcessRequest(HttpContext context)
        {
            upState = Convert.ToInt32(Convert.ToString(context.Session["upState"]));
            if (upState == 0)
            {
                return;
            }
            getFile(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // 获取文件
        public void getFile(HttpContext context)
        {
            UnAttrRst rst = new UnAttrRst();
            string type = context.Request["type"];
            long allowSize = 0;
            string allowType = "";
            switch (type)
            {
                case "Image":
                    allowSize = 1024 * 1024;
                    allowType = ".jpg,.jpeg,.png,.gif,.bmp";
                    break;
                case "File":
                    allowSize = 1024 * 1024;
                    allowType = ".txt,.p12";
                    break;
                default:
                    break;
            }
            HttpPostedFile hpf = context.Request.Files[0];
            if (hpf.ContentLength > 0)
            {
                if (hpf.ContentLength <= allowSize)
                {
                    string ext = System.IO.Path.GetExtension(hpf.FileName).ToLower();
                    var imp = UnFileEvent.important;
                    var name = UnStrRan.getRandom() + ext;
                    string UpPath = imp.fullPath() + "/" + name;
                    DirectoryInfo di = new DirectoryInfo(UnFileEvent.important.fullPath());
                    if (di.Exists == false)
                    {
                        di.Create();
                    }
                    hpf.SaveAs(UpPath);

                    UnFileInfo fi = new UnFileInfo(UpPath, null, allowType, null);
                    if (!fi.isWrongExtens && !fi.isWrongType)
                    {
                        rst.pid = 0;
                        rst.code = 1;
                        rst.msg = "上传成功";

                        BackInfo bi = new BackInfo();
                        switch (type)
                        {
                            case "Image":
                                bi.ReturnCode = 1;
                                bi.ReturnMsg = "传输完成";
                                bi.Url = ImgPath + imp.floder() + "/" + name;
                                break;
                            case "File":
                                bi.ReturnCode = 2;
                                bi.ReturnMsg = "传输完成";
                                bi.UNCode = "UNCODE//:" + UnEncDES.encrypt(UpPath);
                                break;
                            default:
                                break;
                        }
                        rst.back = UnXMMPXml.tToXml(typeof(BackInfo), bi);
                    }
                    else
                    {
                        rst.pid = 0;
                        rst.code = -3;
                        rst.msg = "类型错误";
                    }
                }
                else
                {
                    rst.pid = 0;
                    rst.code = -2;
                    rst.msg = "不能超过1M";
                }
            }
            else
            {
                rst.pid = 0;
                rst.code = -1;
                rst.msg = "文件不存在";
                rst.back = "";
            }

            context.Response.ContentType = "text/xml";
            var xml = UnXMMPXml.tToXml(typeof(UnAttrRst), rst);
            context.Response.Write(xml);
        }
    }
}