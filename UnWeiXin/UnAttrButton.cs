using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    // 微信公众号菜单
    public class UnAttrButton
    {
        // 类型
        public string type { get; set; }
        // 名称
        public string name { get; set; }
        // 键值
        public string key { get; set; }
        // 网页链接
        public string url { get; set; }
        // 调用新增永久素材接口返回的合法media_id 
        public string media_id { get; set; }
        // 子菜单
        public List<UnAttrButton> sub_button { get; set; } 
        
    }
}
