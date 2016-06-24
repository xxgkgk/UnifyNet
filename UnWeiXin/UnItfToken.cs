using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    public interface UnItfToken
    {
        // 获取Token
        string getToken();
        // 返回Token
        void backToken(string appid, string token, int expires);
    }
}
