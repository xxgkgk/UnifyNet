using System;
using UnQuote.Webs;

namespace Web.Admin.Master
{
    public partial class Site : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 管理员状态
        /// </summary>
        private int alState = 0;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            object sobj = Session["alState"];
            object cobj = new UnCookie(this.Page).getCookies("alCookie", "alState");
            if (sobj != null)
            {
                alState = Convert.ToInt32(Convert.ToString(sobj));
            }
            // Session或Cookie为空则回到登录页面
            if (alState == 0 || cobj == null)
            {
                Session.Clear();
                Response.Redirect("/login.htm");
            }
        }
    }
}