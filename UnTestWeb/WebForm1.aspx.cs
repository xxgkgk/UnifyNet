using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UnCommon.Tool;

namespace UnTestWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            abc();



        }

        public void abc()
        {
            DataTable tblDatas = new DataTable("Datas");
            tblDatas.Columns.Add("序号", Type.GetType("System.Int32"));
            tblDatas.Columns[0].AutoIncrement = true;
            tblDatas.Columns[0].AutoIncrementSeed = 1;
            tblDatas.Columns[0].AutoIncrementStep = 1;
            tblDatas.Columns.Add("原始单号", Type.GetType("System.String"));
            tblDatas.Columns.Add("流水单号", Type.GetType("System.String"));
            tblDatas.Columns.Add("结账时间", Type.GetType("System.String"));
            tblDatas.Columns.Add("状态", Type.GetType("System.String"));
            tblDatas.Columns.Add("门店", Type.GetType("System.String"));
            tblDatas.Columns.Add("交易类型", Type.GetType("System.String"));
            tblDatas.Columns.Add("消费合计(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("收款金额(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("实收金额(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("退款金额(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("会员顾客", Type.GetType("System.String"));
            tblDatas.Columns.Add("收银员", Type.GetType("System.String"));
            tblDatas.Columns.Add("列13", Type.GetType("System.String"));
            tblDatas.Columns.Add("列14", Type.GetType("System.String"));
            tblDatas.Columns.Add("列15", Type.GetType("System.String"));
            tblDatas.Columns.Add("列16", Type.GetType("System.String"));
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });


            //tblDatas.Columns.Add("序号", Type.GetType("System.Int32"));
            //tblDatas.Columns[0].AutoIncrement = true;
            //tblDatas.Columns[0].AutoIncrementSeed = 1;
            //tblDatas.Columns[0].AutoIncrementStep = 1;
            //tblDatas.Columns.Add("原始单号", Type.GetType("System.String"));
            //tblDatas.Columns.Add("流水单号", Type.GetType("System.String"));
            //tblDatas.Columns.Add("结账时间", Type.GetType("System.String"));
            //tblDatas.Columns.Add("状态", Type.GetType("System.String"));
            //tblDatas.Columns.Add("门店", Type.GetType("System.String"));
            //tblDatas.Columns.Add("交易类型", Type.GetType("System.String"));
            //tblDatas.Columns.Add("消费合计(元)", Type.GetType("System.String"));
            //tblDatas.Columns.Add("收款金额(元)", Type.GetType("System.String"));
            //tblDatas.Columns.Add("实收金额(元)", Type.GetType("System.String"));
            //tblDatas.Columns.Add("退款金额(元)", Type.GetType("System.String"));
            //tblDatas.Columns.Add("会员顾客", Type.GetType("System.String"));
            //tblDatas.Columns.Add("收银员", Type.GetType("System.String"));

            UnExport ep = new UnExport();
            ep.exportCSV(this, "交易时间2016-1-10.csv", tblDatas, null);
            //ep.streamExport(tblDatas, new string[] { "ID", "Product" }, "a.xls", this);
        }
    }
}