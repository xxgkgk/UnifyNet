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
            DataTable tblDatas = new DataTable("Datas");
            tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
            tblDatas.Columns[0].AutoIncrement = true;
            tblDatas.Columns[0].AutoIncrementSeed = 1;
            tblDatas.Columns[0].AutoIncrementStep = 1;
            tblDatas.Columns.Add("Product", Type.GetType("System.String"));
            tblDatas.Columns.Add("Version", Type.GetType("System.String"));
            tblDatas.Columns.Add("Description", Type.GetType("System.String"));
            tblDatas.Rows.Add(new object[] { null, "a", "b", "c" });
            tblDatas.Rows.Add(new object[] { null, "a", "b", "c" });
            tblDatas.Rows.Add(new object[] { null, "a", "b", "c" });
            tblDatas.Rows.Add(new object[] { null, "a", "b", "c" });
            tblDatas.Rows.Add(new object[] { null, "a", "b", "c" });


            UnExcelExport ep = new UnExcelExport();
            ep.streamExport(tblDatas, new string[] { "ID", "Product" }, "a.xls", this);
            
        }
    }
}