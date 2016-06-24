using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UnCommon;
using UnCommon.Tool;

namespace Web.Admin.Master
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private int alState = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            object sobj = Session["alState"];
            if (sobj != null)
            {
                alState = Convert.ToInt32(Convert.ToString(sobj));
            }
            if (alState == 0)
            {
                Response.Redirect("/admin/login.htm");
            }
        }
    }
}