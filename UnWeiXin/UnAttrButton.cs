using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    /// <summary>
    /// 微信公众号菜单
    /// </summary>
    public class UnAttrButton
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 键值
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 网页链接
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 调用新增永久素材接口返回的合法media_id 
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        ///  子菜单
        /// </summary>
        public List<UnAttrButton> sub_button { get; set; } 
        
    }
}
