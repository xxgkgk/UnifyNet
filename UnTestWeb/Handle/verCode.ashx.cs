using System.Web;
using System.Web.SessionState;
using UnCommon.Encrypt;
using UnCommon.Tool;
using UnCommon.XMMP;
using UnEntity;
using UnQuote.Images;
using UnQuote.Webs;

namespace UnWebTool.Handle
{
    /// <summary>
    /// authCode 的摘要说明
    /// </summary>
    public class verCode : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            // sid-跨站验证
            string sID = context.Session.SessionID;
            if (sID == null)
            {
                return;
            }
            // 键值
            string key = context.Request.QueryString["key"];
            switch (key)
            {
                case "create":
                    create(context);
                    break;
                case "valid":
                    valid(context);
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // 创建验证码
        public void create(HttpContext context)
        {
            string code = UnStrRan.getInt(100000, 999999) + "";
            context.Response.ClearContent();
            context.Response.ContentType = "image/gif";
            context.Response.BinaryWrite(UnImage.createVerCode(code).ToArray());

            code = UnEncDES.encrypt(code);
            UnCookie uc = new UnCookie(context);
            uc.setCookie("verCode", code, 3000);
        }

        // 校验验证码
        public void valid(HttpContext context)
        {
            string code = context.Request.QueryString["code"];
            string verCode = new UnCookie(context).getCookie("verCode");
            verCode = UnEncDES.decrypt(verCode);
            XmlData outxd = new XmlData();
            outxd.ApiNote = new ApiNote();
            if (code == verCode)
            {
                outxd.ApiNote.NoteCode = 1;
                outxd.ApiNote.NoteMsg = "success";
                // 设置成功标志
                context.Session["isCheckAuth"] = true;
            }
            else
            {
                outxd.ApiNote.NoteCode = -1;
                outxd.ApiNote.NoteMsg = "error";
            }
            string write = UnXMMPXml.tToXml(typeof(XmlData), outxd);
            context.Response.ContentType = "text/plain";
            context.Response.Write(write);
        }
    }
}