using System.Data;

namespace UnCommon.Entity
{
    /// <summary>
    /// 翻页类
    /// </summary>
    public class UnSqlPage
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 页面尺码
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalNumber { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable DataSource { get; set; }
        /// <summary>
        /// 泛型数据源
        /// </summary>
        public object TSource { get; set; }
    }
}
