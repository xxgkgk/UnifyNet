using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnWeiXin
{
    /// <summary>
    /// TOKEN处理接口
    /// </summary>
    public interface UnItfToken
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        string getToken();
        /// <summary>
        /// 返回Token
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="token"></param>
        /// <param name="expires"></param>
        void backToken(string appid, string token, int expires);
    }
}
