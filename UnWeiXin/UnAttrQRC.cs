using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public class UnAttrQRC
    {
        // 该二维码有效时间，以秒为单位。 最大不超过604800（即7天）
        public string expire_seconds { get; set; }
        // 二维码类型，QR_SCENE为临时,QR_LIMIT_SCENE为永久,QR_LIMIT_STR_SCENE为永久的字符串参数值 
        public string action_name { get; set; }
        // 二维码详细信息 
        public string action_info { get; set; }
        // 场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000） 
        public string scene_id { get; set; }
        // 场景值ID（字符串形式的ID），字符串类型，长度限制为1到64，仅永久二维码支持此字段
        public string scene_str { get; set; }
    }
}
