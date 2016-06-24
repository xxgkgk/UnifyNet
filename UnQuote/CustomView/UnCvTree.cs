using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Reflection;

namespace UnQuote.CustomView
{
    public class UnCvTree : WebControl, INamingContainer, IPostBackEventHandler
    {
        /// <summary>
        /// List转为DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        private static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        //重绘默认外层标签
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Span;
            }
        }
        //重写TagKey我们也有必要重写此方法，设定对应样式属性
        protected override Style CreateControlStyle()
        {
            //return new TableStyle(ViewState);//表格
            return base.CreateControlStyle();//默认样式
        }
        //OnPreReader事件是在页面已经执行完所有后台代码，并且在生成标准HTML代码前，将要呈现给Page类的时候，此事件发生
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null && Page.FindControl("CtmTreeView_nHGC1") == null)
            {
                #region 向head嵌入所需资源
                //获取注册的客户端资源地址
                ClientScriptManager nCSM = this.Page.ClientScript;
                string CssUrl = nCSM.GetWebResourceUrl(typeof(UnCvTree), "PUB.Css.CtmTreeView.css");
                string JSUrl1 = nCSM.GetWebResourceUrl(typeof(UnCvTree), "PUB.JScript.CtmPub.js");
                string JSUrl2 = nCSM.GetWebResourceUrl(typeof(UnCvTree), "PUB.JScript.CtmTreeView.js");

                System.Web.UI.HtmlControls.HtmlLink nHGC1 = new System.Web.UI.HtmlControls.HtmlLink();
                nHGC1.ID = "CtmTreeView_nHGC1";
                nHGC1.Attributes["type"] = "text/css";
                nHGC1.Attributes["rel"] = "stylesheet";
                nHGC1.Attributes["href"] = CssUrl;

                var nHGC2 = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                nHGC2.Attributes["type"] = "text/javascript";
                nHGC2.Attributes["src"] = JSUrl1;

                var nHGC3= new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                nHGC3.Attributes["type"] = "text/javascript";
                nHGC3.Attributes["src"] = JSUrl2;

                var nHGC4 = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                nHGC4.Attributes["type"] = "text/javascript";
                nHGC4.InnerHtml = "var AddUrl='" + ImgUrl("+") + "';var SubUrl='" + ImgUrl("-") + "';";

                Page.Header.Controls.AddAt(2,nHGC1);
                Page.Header.Controls.AddAt(2,nHGC2);
                Page.Header.Controls.AddAt(2,nHGC3);
                Page.Header.Controls.AddAt(2,nHGC4);
                #endregion
            }
            base.OnPreRender(e);
        }
        //重绘会头部内容
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "CtmTreeView");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.RenderBeginTag(writer);
        }
        //重绘主体内容
        protected override void RenderContents(HtmlTextWriter writer)
        {
            //绘制递增框
            writer.Write(@"" +
              "<div id='tj' onmousemove=mv_box('tj',1) onmouseup=mv_box('tj',2)>" +
                "<div class='tjtop' onmousedown=mv_box('tj',0)>" +
                  "<div class='tt1'>添加栏目</div>" +
                  "<div class='tt2' title='关闭' onclick=swh_box('tj')>X</div>" +
                "</div>" +
                "<div class='tj1'>上级：<label id='tj_sj'></label><input type='hidden' id='tj_code' name='tj_code'/></div>" +
                "<div class='tj1'>名称：<input type='text' id='tj_name' name='tj_name' style='width:100px;'/></div>" +
                "<div class='tjfoot'><input type='button' value='保存' onclick='Tree_Add()'/></div>" +
              "</div>");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "center"); 
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //绘制顶栏
            writer.Write(@"" +
             "<div class='Tree_ct' id='Tree_cttop'>" +
                "<div class='Tree_ct1'>" + DtfIDStr + "</div>" +
                "<div class='Tree_ct2'>" + DtfNameStr + "/" + DtfSortStr + "</div>" +
                "<div class='Tree_ct3'>" + DtfNamezStr + "</div>" +
                "<div class='Tree_ct4'>" + DtfCodeStr + "</div>" +
                "<div class='Tree_ct5'>操作</div>" +
             "</div><div style='clear:both;'></div>");
            //设置数据表
            SetData(_DataSource);
            //绘制中间
            OPTree(writer,_DtfDT, "0", 0);
            //绘制底栏
            writer.Write(@"<div class='Tree_ct' id='Tree_ctfoot'><input type='submit' name='"
                + this.UniqueID + 
                "' value='保存' />&nbsp;&nbsp;<input type='button' value='新增顶级' onclick='Tree_Add_Open()'/>&nbsp;&nbsp;<input type='reset' value='重置'/></div>");
            writer.RenderEndTag();
            base.RenderContents(writer);//重绘
        }
        //重绘底部内容
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            base.RenderEndTag(writer);
        }
        //获取图片地址
        private string ImgUrl(string str)
        {
            string Url = "";
            ClientScriptManager nCSM = this.Page.ClientScript;
            if (str == "+")
            {
                Url = nCSM.GetWebResourceUrl(typeof(UnCvTree), "PUB.Images.+.jpg");
            }
            else if (str == "-")
            {
                Url = nCSM.GetWebResourceUrl(typeof(UnCvTree), "PUB.Images.-.jpg");
            }
            else
            {
                Url = nCSM.GetWebResourceUrl(typeof(UnCvTree), "PUB.Images.0.gif");
            }
            return Url;
        }

        /// <summary>
        /// 回调输出
        /// </summary>
        /// <param name="writer">写对象</param>
        /// <param name="DT">数据源</param>
        /// <param name="Code">编码</param>
        /// <param name="Tier">层级</param>
        private void OPTree(HtmlTextWriter writer, DataTable DT, string Code, int Tier)
        {
            DataColumnCollection DCC = DT.Columns;
            if (DCC.Contains(DtfID) && DCC.Contains(DtfName) && DCC.Contains(DtfNamez) && DCC.Contains(DtfCode) && DCC.Contains(DtfSort))
            {
                string IcoStr = "";
                string CodeStr = "";
                string ClassName = "";
                string OPstr = "";
                string SeStr = "" + DtfCode + " = '" + Code + "'";
                DataRow[] nDR = DT.Select(SeStr);
                

                string s_ID = "";
                string s_Name = "";
                string s_NameEN = "";
                string s_Code = "";
                string s_Sort = "";
                int nDR_Le = nDR.Length;
                for (int i = 0; i < nDR_Le; i++)
                {
                    s_ID = nDR[i][DtfID].ToString();
                    s_Name = nDR[i][DtfName].ToString();
                    s_NameEN = nDR[i][DtfNamez].ToString();
                    s_Code = nDR[i][DtfCode].ToString();
                    s_Sort = nDR[i][DtfSort].ToString();

                    //根据层级输出空格
                    IcoStr = "";
                    for (int iT = 0; iT <= Tier; iT++)
                    {
                        IcoStr += "&#12288&#12288";
                    }
                    //组合对应编码做为查询下一级的条件
                    CodeStr = s_Code + s_ID;
                    
                    //是否显示
                    ClassName = "close";
                    if (Unfold == true || Code == "0")
                    {
                        ClassName = "open";
                    }
                    else
                    {
                        ClassName = "close";
                    }
                    //是否有下层
                    if (DT.Select("" + DtfCode + "= '" + CodeStr + "'").Length > 0)
                    {
                        if (Unfold == true)
                        {
                            IcoStr += "<span class='yes' onclick='Tree_Switch(this,&quot;Tree_" + CodeStr + "&quot;)'><img src='" + ImgUrl("-") + "'/></span>";
                        }
                        else
                        {
                            IcoStr += "<span class='no' onclick='Tree_Switch(this,&quot;Tree_" + CodeStr + "&quot;)'><img src='" + ImgUrl("+") + "'/></span>";
                        }
                    }
                    else
                    {
                        IcoStr += "<span class='yes'><img src='" + ImgUrl("0") + "'/></span>";
                    }
                    //输出格式
                    OPstr = "" +
                           "<div id='TreeCt" + s_ID + "' class='Tree_ct'>" +
                             "<div class='Tree_ct1'>" + s_ID + "</div>" +
                             "<div class='Tree_ct2'>" +
                               "<table><tr><td>" + IcoStr + "</td><td><input class='name' id='" + DtfName + s_ID + "' name='" + DtfName + s_ID + "' type='text' value='" + s_Name + "'/></td><td><input class='sort' id='" + DtfSort + s_ID + "' name='" + DtfSort + s_ID + "' type='text' value='" + s_Sort + "'/></td></tr></table>" +
                             "</div>" +
                             "<div class='Tree_ct3'>" +
                               "<table><tr><td><input class='namez' id='" + DtfNamez + s_ID + "' name='" + DtfNamez + s_ID + "' type='text' value='" + s_NameEN + "'/><td></tr></table>" +
                             "</div>" +
                             "<div class='Tree_ct4'>" +
                               "<table><tr><td><input class='code' id='" + DtfCode + s_ID + "' name='" + DtfCode + s_ID + "' type='text' value='" + s_Code + "' readonly='readonly'/><td></tr></table>" +
                             "</div>" +
                             "<div class='Tree_ct5'><input type='button' value='添加下级' onclick='Tree_Add_Open(&quot;" + DtfName + "&quot;,&quot;" + s_ID + "&quot;,&quot;" + s_Code + "&quot;)'/>  <input type='button' onclick='Tree_Del(&quot;" + s_ID + "&quot;,&quot;" + s_Code + "&quot;)' value='删除' /> </div>" +
                           "</div><div style='clear:both;'></div>";
                    //输出外衣Start
                    writer.Write(@"<div id='Tree_" + CodeStr + "' name='Tree_" + Code + "' class='" + ClassName + "'>");
                    //输出当前层
                    writer.Write(@OPstr);
                    //回调下一层
                    OPTree(writer, _DtfDT, CodeStr, Tier + 1);
                    //输出外衣End
                    writer.Write(@"</div>");
                }
            }
        }

        //编号文字显示
        private string _DtfIDStr = "编号";
        [Description("编号文字显示")]
        [Browsable(true)]
        public virtual string DtfIDStr
        {
            get { return _DtfIDStr; }
            set { _DtfIDStr = value; }
        }

        //表"ID"字段名
        private string _DtfID = "";
        [Description("'编号'字段名")]
        [Browsable(true)]
        public virtual string DtfID
        {
            get { return _DtfID; }
            set { _DtfID = value; }
        }

        //主名称文字显示
        private string _DtfNameStr = "名称";
        [Description("主名称文字显示")]
        [Browsable(true)]
        public virtual string DtfNameStr
        {
            get { return _DtfNameStr; }
            set { _DtfNameStr = value; }
        }

        //表"主名称"字段名
        private string _DtfName = "";
        [Description("'主名称'字段名")]
        [Browsable(true)]
        public virtual string DtfName
        {
            get { return _DtfName; }
            set { _DtfName = value; }
        }

        //副名称文字显示
        private string _DtfNamezStr = "副名称";
        [Description("副名称文字显示")]
        [Browsable(true)]
        public virtual string DtfNamezStr
        {
            get { return _DtfNamezStr; }
            set { _DtfNamezStr = value; }
        }

        //表"副名称"字段名
        private string _DtfNamez = "";
        [Description("'副名称'字段名")]
        [Browsable(true)]
        public virtual string DtfNamez
        {
            get { return _DtfNamez; }
            set { _DtfNamez = value; }
        }

        //编码文字显示
        private string _DtfCodeStr = "编码";
        [Description("编码文字显示")]
        [Browsable(true)]
        public virtual string DtfCodeStr
        {
            get { return _DtfCodeStr; }
            set { _DtfCodeStr = value; }
        }

        //表"编码"字段名
        [Description("'编码'字段名")]
        [Browsable(true)]
        private string _DtfCode = "";
        public virtual string DtfCode
        {
            get { return _DtfCode; }
            set { _DtfCode = value; }
        }

        //序号文字显示
        private string _DtfSortStr = "序号";
        [Description("序号文字显示")]
        [Browsable(true)]
        public virtual string DtfSortStr
        {
            get { return _DtfSortStr; }
            set { _DtfSortStr = value; }
        }

        //表"排序"字段名
        private string _DtfSort = "";
        [Description("'排序'字段名")]
        [Browsable(true)]
        public virtual string DtfSort
        {
            get { return _DtfSort; }
            set { _DtfSort = value; }
        }

        //数据源
        private object _DataSource = new object();
        [Description("支持DataTable,IList")]
        [Browsable(true)]
        public virtual object DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }
        //数据表
        public static DataTable _DtfDT = new DataTable();
        /// <summary>
        /// 数据源转为数据表
        /// </summary>
        /// <param name="obj"></param>
        private static void SetData(object obj)
        {
            if (obj is DataTable)
            {
                _DtfDT = (DataTable)obj;
            }
            else if (obj is IList)
            {
                _DtfDT = ListToDataTable((IList)obj);
            }
        }

        //增加Div的name属性
        private bool _Unfold = false;
        [Description("初始是否展开")]
        [Browsable(true)]
        public virtual bool Unfold
        {
            get { return _Unfold; }
            set { _Unfold = value; }
        }

        // 定义一个Click事件委托对象
        private static readonly object EventClick = new object();

        //实现Click事件属性
        [Description("Click事件属性"), Category("Action")]
        public event EventHandler Click
        {
            add
            {
                Events.AddHandler(EventClick, value);
            }
            remove
            {
                Events.RemoveHandler(EventClick, value);
            }
        }

        //实现事件方法
        protected virtual void OnClick(EventArgs e)
        {
            EventHandler clickHandler = (EventHandler)Events[EventClick];

            if (clickHandler != null)
            {
                clickHandler(this, e);
            }
        }

        // 实现IPostBackEventHandler接口成员
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            OnClick(EventArgs.Empty);
        }

    }
}
