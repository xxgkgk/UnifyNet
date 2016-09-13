using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace UnQuote.Webs
{
    /// <summary>
    /// COOKIE类
    /// </summary>
    public class UnCookie
    {
        HttpRequest request;
        HttpResponse response;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="_content"></param>
        public UnCookie(HttpContext _content)
        {
            request = _content.Request;
            response = _content.Response;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="_page"></param>
        public UnCookie(Page _page)
        {
            request = _page.Request;
            response = _page.Response;
        }

        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string getCookie(string name)
        {
            string ReturnValue = "";
            HttpCookie cookie = request.Cookies[name];
            if (cookie != null)
            {
                ReturnValue = cookie.Value.ToString();
            }
            return HttpUtility.UrlDecode(ReturnValue, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string getCookies(string className, string name)
        {
            string ReturnValue = "";
            HttpCookie cookie = request.Cookies[className];
            if (cookie != null)
            {
                ReturnValue = cookie.Values[name];
                // _Page.Response.Write(ReturnValue + ",dfdf<br/>");
            }
            else
            {
                return null;
            }
            return HttpUtility.UrlDecode(ReturnValue, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="exp"></param>
        public void setCookie(string name, string value, int exp)
        {
            HttpCookie cookie = new HttpCookie(name, HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8));
            cookie.Expires = DateTime.Now.AddSeconds(exp);
            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="className">集合名</param>
        /// <param name="array">集合元素字典</param>
        /// <param name="exp">有效时间(秒)</param>
        public void setCookies(string className, Dictionary<string, string> array, int exp)
        {
            HttpCookie cookie = new HttpCookie(className);
            if (cookie != null)
            {
                foreach (var strA in array)
                {
                    cookie.Values[strA.Key] = HttpUtility.UrlEncode(strA.Value, System.Text.Encoding.UTF8);
                }
                cookie.Expires = DateTime.Now.AddSeconds(exp);
            }
            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 清除cookie
        /// </summary>
        public void deleteCookies()
        {
            foreach (string CookieName in request.Cookies.AllKeys)
            {
                HttpCookie nHC = request.Cookies[CookieName];
                nHC.Expires = DateTime.Today.AddDays(-5);
                request.Cookies.Add(nHC);
            }
        }

    }
}
