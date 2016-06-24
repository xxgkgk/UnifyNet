using System;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Data;


namespace UnQuote.CustomView
{
    public class UnCvPageFoot : WebControl, INamingContainer
    {
        #region 私用方法
        //获取图片地址
        private string ImgUrl(string ImgName)
        {
            string Url = "";
            ClientScriptManager nCSM = this.Page.ClientScript;
            Url = nCSM.GetWebResourceUrl(typeof(UnCvPageFoot), "PUB.Images." + ImgName);
            return Url;
        }
        #endregion

        #region 载入资源
        //OnPreReader事件是在页面已经执行完所有后台代码，并且在生成标准HTML代码前，将要呈现给Page类的时候，此事件发生
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null && Page.FindControl("CtmPageFoot_nHGC1") == null)
            {
                ClientScriptManager nCSM = this.Page.ClientScript;
                string CssUrl = nCSM.GetWebResourceUrl(typeof(UnCvPageFoot), "PUB.Css.CtmPageFoot.css");
                string JSUrl1 = nCSM.GetWebResourceUrl(typeof(UnCvPageFoot), "PUB.JScript.CtmPub.js");
                string JSUrl2 = nCSM.GetWebResourceUrl(typeof(UnCvPageFoot), "PUB.JScript.CtmPageFoot.js");


                //写入Page顶部
                System.Web.UI.HtmlControls.HtmlLink nHGC1 = new System.Web.UI.HtmlControls.HtmlLink();
                nHGC1.ID = "CtmPageFoot_nHGC1";
                nHGC1.Attributes["type"] = "text/css";
                nHGC1.Attributes["rel"] = "stylesheet";
                nHGC1.Attributes["href"] = CssUrl;

                var nHGC2 = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                nHGC2.Attributes["type"] = "text/javascript";
                nHGC2.Attributes["src"] = JSUrl1;

                var nHGC3 = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                nHGC3.Attributes["type"] = "text/javascript";
                nHGC3.Attributes["src"] = JSUrl2;

                Page.Header.Controls.AddAt(2,nHGC1);
                Page.Header.Controls.AddAt(2,nHGC2);
                Page.Header.Controls.AddAt(2,nHGC3);
            }
            base.OnPreRender(e);
        }
        #endregion

        #region 重绘头部
        //重绘会头部内容
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "CtmPageFoot");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            base.RenderBeginTag(writer);
        }
        #endregion

        #region 重绘内容
        //重绘主体内容
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string type = "";
            if (_Ctm_PageFootMode == PageFootMode.Submit)
            {
                type = "Submit";
            }
            else
            {
                type = "Link";
            }
            writer.Write(@"<label id='tn'>" + Ctm_TotalCount + "</label><label id='ps'>" + Ctm_PageSize + "</label>");
            writer.Write(@"<a href='javascript:CtmPageFoot_Page(&quot;lf&quot;);' id='lf'>首页</a>");
            writer.Write(@"<a href='javascript:CtmPageFoot_Page(&quot;lp&quot;);' id='lp'>上一页</a>");
            writer.Write(@"<a href='javascript:CtmPageFoot_Page(&quot;ln&quot;);' id='ln'>下一页</a>");
            writer.Write(@"<a href='javascript:CtmPageFoot_Page(&quot;ll&quot;);' id='ll'>尾页</a>");
            writer.Write(@"共<label id='pn'>0</label>页 到第<input type='text' id='cp' name='cp' value='" + Ctm_CurrentPage + "'/>页&nbsp;<input type='submit' id='sub' style='display:none;'/><a hrer id='go' value='GO' onclick='CtmPageFoot_Page(&quot;go&quot;,&quot;" + type + "&quot;)'>确定</a>");
            writer.Write(@"<script type='text/javascript'>CtmPageFoot_Page('eft','" + type + "');</script>");
        }
        #endregion

        #region 重绘底部
        //重绘底部内容
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            base.RenderEndTag(writer);
        }
        #endregion

        #region 属性
        //总记录数
        private int _Ctm_TotalCount = 0;
        [Description("总记录数")] 
        [Browsable(true)]
        public virtual int Ctm_TotalCount
        {
            get { return _Ctm_TotalCount; }
            set { _Ctm_TotalCount = value; }
        }

        //页面尺码
        private int _Ctm_PageSize = 1;
        [Description("页面尺码")]
        [Browsable(true)]
        public virtual int Ctm_PageSize
        {
            get { return _Ctm_PageSize; }
            set { _Ctm_PageSize = value; }
        }

        //当前页码
        private int _Ctm_CurrentPage = 0;
        [Description("当前页码")]
        [Browsable(true)]
        public virtual int Ctm_CurrentPage
        {
            get { return _Ctm_CurrentPage; }
            set { _Ctm_CurrentPage = value; }
        }

        /// <summary>
        /// 模式类
        /// </summary>
        public enum PageFootMode
        {
           Submit,
           Link
        }

        //模式
        private PageFootMode _Ctm_PageFootMode = PageFootMode.Submit;
        [Description("翻页模式")]
        [Browsable(true)]
        public virtual PageFootMode Ctm_PageFootMode
        {
            get { return _Ctm_PageFootMode; }
            set { _Ctm_PageFootMode = value; }
        }
        #endregion
    }
}
