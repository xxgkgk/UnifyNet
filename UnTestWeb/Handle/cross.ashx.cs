using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;
using UnCommon.Encrypt;
using UnCommon.Files;
using UnCommon.Tool;
using UnCommon.XMMP;
using UnEntity;
using UnQuote.Webs;

namespace UnWebTool.Handle
{
    /// <summary>
    /// cross 的摘要说明
    /// </summary>
    public class cross : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // sid-跨站验证
                string sID = context.Session.SessionID;
                if (sID == null)
                {
                    return;
                }
                // 键值
                string key = context.Request.QueryString["key"];

                // 输出
                string write = null;
                switch (key)
                {
                    case "admin_login":
                        adminLogin(context);
                        break;
                    case "admin_show":
                        // 未登录不允许使用
                        if (context.Session["alState"] == null)
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                }
                context.Response.ContentType = "text/xml";
                context.Response.Write(write);
            }
            catch (Exception e)
            {
                UnFile.writeLog("ProcessRequest", e.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // 验证码校验
        public bool isCheckAuth(HttpContext context)
        {
            if (context.Session["isCheckAuth"] != null)
            {
                return Convert.ToBoolean(context.Session["isCheckAuth"]);
            }
            else
            {
                return false;
            }
        }

        // 后台登录
        public void adminLogin(HttpContext context)
        {
            string write = null;
            XmlData oxd = new XmlData();
            oxd.ApiNote = new ApiNote();

            if (isCheckAuth(context))
            {
                // 输入
                Stream stream = context.Request.InputStream;
                string ixml = UnTo.streamToString(stream);
                XmlData ixd = (XmlData)UnXMMPXml.xmlToT(typeof(XmlData), ixml);
                if (ixd.AdminLogin.User == "admin" && ixd.AdminLogin.Pass == UnEncMD5.getMd5Hash2("hp2015"))
                {
                    oxd.ApiNote.NoteCode = 1;
                    oxd.ApiNote.NoteMsg = "登录成功";
                    // 设置session时间（分钟）
                    context.Session.Timeout = 720;
                    // 后台登录状态
                    context.Session["alState"] = 1;
                    // 上传权限状态
                    context.Session["upState"] = 2;
                    // 后台Cookie
                    UnCookie ck = new UnCookie(context);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("alState", "1");
                    dic.Add("upState", "2");
                    dic.Add("ID", "1");
                    dic.Add("User", "admin");
                    dic.Add("Name", "管理员");
                    ck.setCookies("alCookie", dic, 5184000);
                }
                else
                {
                    oxd.ApiNote.NoteCode = -1;
                    oxd.ApiNote.NoteMsg = "账号密码错误";
                }
                write = UnXMMPXml.tToXml(typeof(XmlData), oxd);
            }
            else
            {
                oxd.ApiNote.NoteCode = -10;
                oxd.ApiNote.NoteMsg = "验证码错误";
                write = UnXMMPXml.tToXml(typeof(XmlData), oxd);
            }
            context.Response.ContentType = "text/xml";
            context.Response.Write(write);
        }
    }
}