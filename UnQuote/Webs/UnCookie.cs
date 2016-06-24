using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.Remoting.Contexts;
using System.Web.UI;

namespace UnQuote.Webs
{
    public class UnCookie
    {
        HttpRequest request;
        HttpResponse response;

        // 实例化
        public UnCookie(HttpContext _content)
        {
            request = _content.Request;
            response = _content.Response;
        }

        // 实例化
        public UnCookie(Page _page)
        {
            request = _page.Request;
            response = _page.Response;
        }

        // 获取cookie
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

        // 获取cookies
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

        // 设置cookie
        public void setCookie(string name, string value, int exp)
        {
            HttpCookie cookie = new HttpCookie(name, HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8));
            cookie.Expires = DateTime.Now.AddSeconds(exp);
            response.Cookies.Add(cookie);
        }

        // 设置cookies
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

        // 清除cookie
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
