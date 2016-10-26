using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using UnCommon.Config;

namespace UnCommon.Tool
{
    /// <summary>
    /// 导出文件类
    /// </summary>
    public class UnExport
    {
        /// <summary>
        /// 构造类
        /// </summary>
        public UnExport()
        { }

        /// <summary>
        /// 实例化对象
        /// </summary>
        private static UnExport _instance = null;

        /// <summary>
        /// 实例化
        /// </summary>
        public static UnExport Instance
        {
            get
            {
                if (_instance == null) _instance = new UnExport();
                return _instance;
            }
        }

        /// <summary>
        /// DataTable通过流导出Excel
        /// </summary>
        /// <param name="dt">数据源DataSet</param>
        /// <param name="columns">DataTable中列对应的列名(可以是中文),若为null则取DataTable中的字段名</param>
        /// <param name="fileName">保存文件名(例如：a.xls)</param>
        /// <param name="pages">页面上下文</param>
        /// <returns></returns>
        public bool streamExport(DataTable dt, string[] columns, string fileName, Page pages)
        {
            if (dt.Rows.Count > 65535) //总行数大于Excel的行数 
            {
                throw new Exception("预导出的数据总行数大于excel的行数");
            }
            if (string.IsNullOrEmpty(fileName)) return false;

            StringBuilder content = new StringBuilder();
            StringBuilder strtitle = new StringBuilder();
            content.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
            content.Append("<head><title></title><meta http-equiv='Content-Type' content=\"text/html; charset=gb2312\">");
            //注意：[if gte mso 9]到[endif]之间的代码，用于显示Excel的网格线，若不想显示Excel的网格线，可以去掉此代码
            content.Append("<!--[if gte mso 9]>");
            content.Append("<xml>");
            content.Append(" <x:ExcelWorkbook>");
            content.Append("  <x:ExcelWorksheets>");
            content.Append("   <x:ExcelWorksheet>");
            content.Append("    <x:Name>Sheet1</x:Name>");
            content.Append("    <x:WorksheetOptions>");
            content.Append("      <x:Print>");
            content.Append("       <x:ValidPrinterInfo />");
            content.Append("      </x:Print>");
            content.Append("    </x:WorksheetOptions>");
            content.Append("   </x:ExcelWorksheet>");
            content.Append("  </x:ExcelWorksheets>");
            content.Append("</x:ExcelWorkbook>");
            content.Append("</xml>");
            content.Append("<![endif]-->");
            content.Append("</head><body><table style='border-collapse:collapse;table-layout:fixed;'><tr>");

            if (columns != null)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != null && columns[i] != "")
                    {
                        content.Append("<td><b>" + columns[i] + "</b></td>");
                    }
                    else
                    {
                        content.Append("<td><b>" + dt.Columns[i].ColumnName + "</b></td>");
                    }
                }
            }
            else
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    content.Append("<td><b>" + dt.Columns[j].ColumnName + "</b></td>");
                }
            }
            content.Append("</tr>\n");

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                content.Append("<tr>");
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    object obj = dt.Rows[j][k];
                    Type type = obj.GetType();
                    if (type.Name == "Int32" || type.Name == "Single" || type.Name == "Double" || type.Name == "Decimal")
                    {
                        double d = obj == DBNull.Value ? 0.0d : Convert.ToDouble(obj);
                        if (type.Name == "Int32" || (d - Math.Truncate(d) == 0))
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0'>{0}</td>", obj);
                        else
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0.00'>{0}</td>", obj);
                    }
                    else
                        content.AppendFormat("<td style='vnd.ms-excel.numberformat:@'>{0}</td>", obj);
                }
                content.Append("</tr>\n");
            }
            content.Append("</table></body></html>");
            content.Replace("&nbsp;", "");
            pages.Response.Clear();
            pages.Response.Buffer = true;
            pages.Response.ContentType = "application/ms-excel";  //"application/ms-excel";
            pages.Response.Charset = "UTF-8";
            pages.Response.ContentEncoding = System.Text.Encoding.UTF7;
            fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            pages.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            pages.Response.Write(content.ToString());
            //pages.Response.End();  //注意，若使用此代码结束响应可能会出现“由于代码已经过优化或者本机框架位于调用堆栈之上,无法计算表达式的值。”的异常。
            HttpContext.Current.ApplicationInstance.CompleteRequest(); //用此行代码代替上一行代码，则不会出现上面所说的异常。
            return true;
        }

        /// <summary>
        /// 直接由GridView导出Excel
        /// </summary>
        /// <param name="ctl">控件(一般是GridView)</param>
        /// <param name="FileName">导出的文件名</param>
        /// <param name="removeIndexs">要移除的列的索引数组(因为有时我们并不希望把GridView中的所有列全部导出)</param>
        /// <param name="pages"></param>
        public void controlToExcel(System.Web.UI.WebControls.GridView ctl, string FileName, string[] removeIndexs, System.Web.UI.Page pages)
        {
            if (removeIndexs != null)
            {
                foreach (string index in removeIndexs)
                {
                    ctl.Columns[int.Parse(index)].Visible = false;
                }
            }
            pages.Response.Charset = "UTF-8";
            pages.Response.ContentEncoding = System.Text.Encoding.UTF7;
            pages.Response.ContentType = "application/ms-excel";
            FileName = System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            pages.Response.AppendHeader("Content-Disposition", "attachment;filename=" + "" + FileName);
            ctl.Page.EnableViewState = false;
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            ctl.RenderControl(hw);
            pages.Response.Write(tw.ToString());
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// DataTable转CSV字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public StringBuilder dtToCSV(DataTable dt, string[] columns)
        {
            // 若默认字段为空
            if (columns == null)
            {
                columns = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columns[i] = dt.Columns[i].ColumnName;
                }
            }
            StringBuilder sb = new StringBuilder();
            // 构造头部
            StringBuilder sbHead = new StringBuilder();
            foreach (var con in columns)
            {
                sbHead.Append(con + ",");
            }
            sbHead.Remove(sbHead.Length - 1, 1);
            sb.AppendLine(sbHead.ToString());
            // 构造数据
            foreach (DataRow row in dt.Rows)
            {

                StringBuilder sbData = new StringBuilder();
                foreach (var con in columns)
                {
                    string value = "";
                    if (dt.Columns.Contains(con))
                    {
                        value = row[con].ToString();
                    }
                    sbData.Append(value + ",");
                }
                sbData.Remove(sbData.Length - 1, 1);
                sb.AppendLine(sbData.ToString());
            }
            return sb;
        }

        /// <summary>
        /// DataTable转CSV字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public StringBuilder dtToCSV(DataTable dt, string columns)
        {
            string[] ary = null;
            if (!String.IsNullOrWhiteSpace(columns))
            {
                ary = columns.Split(',');
            }
            return dtToCSV(dt, ary);
        }

        /// <summary>
        /// 导出CSV文件
        /// </summary>
        /// <param name="pages">页面对象</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public bool exportCSV(Page pages, string fileName, string content)
        {
            try
            {
                pages.Response.Clear();
                pages.Response.ClearHeaders();
                pages.Response.Buffer = true;
                pages.Response.ContentType = "application/octet-stream;";
                pages.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                //pages.Response.AppendHeader("Content-Length", content.Length.ToString());
                pages.Response.Write(content);
                pages.Response.Flush();
                pages.Response.End();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 导出CSV文件
        /// </summary>
        /// <param name="pages">页面对象</param>
        /// <param name="fileName">文件名</param>
        /// <param name="dt">数据</param>
        /// <param name="columns">需要的列,NULL则为所有</param>
        /// <returns></returns>
        public bool exportCSV(Page pages, string fileName, DataTable dt, string columns)
        {
            string content = dtToCSV(dt, columns).ToString();
            return exportCSV(pages, fileName, content);
        }
    }
}