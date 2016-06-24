using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UnQuote.CustomView
{
    public class UnCvTreeSelect : WebControl, INamingContainer
    {
        public UnCvTreeSelect(){}

        #region 私有方法
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
        /// <summary>
        /// 数据源转为数据表
        /// </summary>
        /// <param name="obj"></param>
        private static DataTable SetData(object obj)
        {
            DataTable _DtfDT = new DataTable();
            if (obj is DataTable)
            {
                _DtfDT = (DataTable)obj;
            }
            else if (obj is IList)
            {
                _DtfDT = ListToDataTable((IList)obj);
            }
            return _DtfDT;
        }
        #endregion

        #region 选择事件
        //定义事件
        public event EventHandler SelectedIndexChanged;
        private void OnChange(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                    SelectedIndexChanged(sender, e);
            }
        }
        private Control findControl(string controlName)
        {
            for (int i = 0; i < Page.Header.Controls.Count; i++)
            {
                if (Page.Header.Controls[i].ID == controlName)
                {
                    return Page.Header.Controls[i];
                }
            }
            return null;
        }
        #endregion

        #region 载入资源
        //OnPreReader事件是在页面已经执行完所有后台代码，并且在生成标准HTML代码前，将要呈现给Page类的时候，此事件发生
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null && findControl("CtmTreeSelect_nHGC1") == null)
            {
                #region 嵌入控件所需资源
                ClientScriptManager nCSM = this.Page.ClientScript;
                string CssUrl = nCSM.GetWebResourceUrl(typeof(UnCvTreeSelect), "PUB.Css.CtmTreeSelect.css");
                string JSUrl1 = nCSM.GetWebResourceUrl(typeof(UnCvTreeSelect), "PUB.JScript.CtmPub.js");
                string JSUrl2 = nCSM.GetWebResourceUrl(typeof(UnCvTreeSelect), "PUB.JScript.CtmTreeSelect.js");

                System.Web.UI.HtmlControls.HtmlLink nHGC1 = new System.Web.UI.HtmlControls.HtmlLink();
                nHGC1.ID = "CtmTreeSelect_nHGC1";
                nHGC1.Attributes["type"] = "text/css";
                nHGC1.Attributes["rel"] = "stylesheet";
                nHGC1.Attributes["href"] = CssUrl;

                var nHGC2 = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                nHGC2.Attributes["type"] = "text/javascript";
                nHGC2.Attributes["src"] = JSUrl1;

                var nHGC3 = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                nHGC3.Attributes["type"] = "text/javascript";
                nHGC3.Attributes["src"] = JSUrl2;

                Page.Header.Controls.AddAt(2, nHGC1);
                Page.Header.Controls.AddAt(2, nHGC2);
                Page.Header.Controls.AddAt(2, nHGC3);

                #endregion
            }
            base.OnPreRender(e);
        }
        #endregion

        #region 创建子控件
        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {
            DropDownList _DropDownList = new DropDownList();
            _DropDownList.ID = "CtmTreeSelect_DDL";
            this.Controls.Add(_DropDownList);
            //附加内部事件
            _DropDownList.SelectedIndexChanged += new EventHandler(OnChange);
            if (Page.IsPostBack)
            {
                string str = _DropDownList.UniqueID;
                _Value = Page.Request.Form[str];
            }
            base.CreateChildControls();
        }
        #endregion

        #region 绑定数据源
        public override void DataBind()
        {
            if (!Page.IsPostBack)
            {
                DropDownList _DropDownList = (DropDownList)FindControl("CtmTreeSelect_DDL");
                OPTree(_DropDownList, SetData(DataSource), StartCode, 0);
                if (DefaultText != "")
                {
                    _DropDownList.Items.Insert(0, new ListItem(DefaultText, DefaultValue));
                }
            }
            base.DataBind();
        }
        #endregion

        #region 输出控件
        public override void RenderControl(HtmlTextWriter writer)
        {
            DropDownList _DropDownList = (DropDownList)FindControl("CtmTreeSelect_DDL");
            //选中的值
            if (Value != "")
            {
                _DropDownList.SelectedValue = Value;
            }
            else
            {
                _DropDownList.SelectedValue = SelectValue;
            }
            _DropDownList.AutoPostBack = AutoPostBack;
            if (onchange != "")
            {
                _DropDownList.Attributes.Add("onchange", onchange);
            }
            base.RenderControl(writer);
        }
        #endregion

        #region 树型方法
        /// <summary>
        /// 树型逻辑
        /// </summary>
        /// <param name="_DDL"></param>
        /// <param name="DT"></param>
        /// <param name="Code"></param>
        /// <param name="Tier"></param>
        private void OPTree(DropDownList _DDL, DataTable DT, string Code, int Tier)
        {
            DataColumnCollection DCC = DT.Columns;
            if (DCC.Contains(DtfID) && DCC.Contains(DtfName) && DCC.Contains(DtfNamez) && DCC.Contains(DtfCode) && DCC.Contains(DtfSort))
            {
                string IcoStr = "";
                string CodeStr = "";
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

                    string nbsp = Page.Server.HtmlDecode("&nbsp;");
                    //根据层级输出空格
                    if (Tier == 0)
                    {
                        IcoStr = "+";
                    }
                    else
                    {
                        IcoStr = nbsp + nbsp;
                    }
                    for (int iT = 1; iT <= Tier; iT++)
                    {
                        IcoStr += nbsp + nbsp + nbsp;
                    }
                    //组合对应编码做为查询下一级的条件
                    CodeStr = s_Code + s_ID;

                    //是否有下层
                    if (DT.Select("" + DtfCode + "= '" + CodeStr + "'").Length > 0)
                    {
                    }
                    else
                    {

                    }
                    ListItem _ListItem = new ListItem();
                    _ListItem.Value = s_ID;
                    _ListItem.Text = IcoStr + s_Name;
                    _DDL.Items.Add(_ListItem);
                    //回调下一层
                    OPTree(_DDL, DT, CodeStr, Tier + 1);
                }
            }
        }
        #endregion

        #region 属性
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
        //选中的值
        private string _SelectValue = "";
        [Description("选中的ID")]
        [Browsable(true)]
        public virtual string SelectValue
        {
            get { return _SelectValue; }
            set { _SelectValue = value; }
        }
        //默认文本
        private string _DefaultText = "";
        [Description("默认文本")]
        [Browsable(true)]
        public virtual string DefaultText
        {
            get { return _DefaultText; }
            set { _DefaultText = value; }
        }
        //默认文本值
        private string _DefaultValue = "";
        [Description("默认文本值")]
        [Browsable(true)]
        public virtual string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }
        //初始层编码
        private string _StartCode = "0";
        [Description("初始层编码")]
        [Browsable(true)]
        public virtual string StartCode
        {
            get { return _StartCode; }
            set { _StartCode = value; }
        }
        //文本值
        private string _Text = "";
        [Description("获得当前选中的文本")]
        [Browsable(true)]
        protected virtual string Text
        {
            get { return _Text; }
        }
        //值
        private string _Value = "";
        [Description("获得当前选中的值")]
        [Browsable(true)]
        public virtual string Value
        {
            get { return _Value; }
        }

        /// <summary>
        /// 是否产生向服务器的回发
        /// </summary>
        private bool _AutoPostBack = false;
        [Description("是否允许向服务器回发")]
        [Browsable(true)]
        public virtual bool AutoPostBack
        {
            get { return _AutoPostBack; }
            set { _AutoPostBack = value; }
        }

        /// <summary>
        /// 客户端onchange事件
        /// </summary>
        private string _onchange = "";
        [Description("客户端onchange事件")]
        [Browsable(true)]
        public virtual string onchange
        {
            get { return _onchange; }
            set { _onchange = value; }
        }
        #endregion
    }
}
