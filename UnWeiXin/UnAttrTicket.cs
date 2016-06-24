using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public class UnAttrTicket
    {
        // 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码
        public string ticket { get; set; }
        // 二维码的有效时间，以秒为单位。最大不超过1800
        public string expire_seconds { get; set; }
        // 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        public string url { get; set; }
    }
}
