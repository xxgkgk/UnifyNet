using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnCommon.Config;
using UnCommon.Files;

namespace UnCommon.HTTP
{
    /// <summary>
    /// http帮助类
    /// </summary>
    public class UnHttpHelp
    {
        // 默认浏览器
        private static readonly string defaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        // 创建GET方式的HTTP请求  
        private static HttpWebRequest creageGet(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = defaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request;
        }

        // 创建GET方式的HTTP请求
        public static HttpWebRequest creageGet(string url, int? timeout)
        {
            return creageGet(url, timeout, null, null);
        }

        // 创建POST方式的HTTP请求  
        private static HttpWebRequest createPost(string url, int? timeout, string contentType, string eve, string userAgent, CookieCollection cookies,string _cerPath,string _cerPass)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                if (_cerPath != null)
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }
                else
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult1);
                }
                request = WebRequest.Create(url) as HttpWebRequest;
                if (_cerPath != null)
                {
                    X509Certificate2 cer = null;
                    if (_cerPass != null)
                    {
                        // p12格式
                        cer = new X509Certificate2(_cerPath, _cerPass);
                        try
                        {
                            // 自动安装证书 IIS-程序池-高级配置-加载用户配置文件（True）
                            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                            //X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                            store.Open(OpenFlags.ReadWrite);
                            store.Remove(cer);   //可省略
                            store.Add(cer);
                            store.Close();
                        }
                        catch(Exception e)
                        {
                            UnFile.writeLog("X509Store", e.ToString());
                        }
                    }
                    else
                    {
                        // cert格式
                        cer = new X509Certificate2(_cerPath);
                    }
                    request.ClientCertificates.Add(cer); 
                }
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            if (contentType == null)
            {
                contentType = "application/x-www-form-urlencoded";
            }
            if (eve != null)
            {
                request.Headers["eve"] = eve;
            }
            request.Method = "POST";
            request.ContentType = contentType;
            request.AllowAutoRedirect = true;
            request.ContentLength = 0;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = defaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request;
        }

        // 创建POST方式的HTTP请求  
        public static HttpWebRequest createPost(string url, int? timeout, string contentType, string eve)
        {
            return createPost(url, timeout, contentType, eve, null, null, null,null);
        }

        // 创建POST方式的HTTP请求  
        public static HttpWebRequest createPost(string url, int? timeout, string contentType, string eve, string cerPath,string cerPass)
        {
            return createPost(url, timeout, contentType, eve, null, null, cerPath, cerPass);
        }

        // SSL证书校验
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        // SSL不验证证书(只需通道加密)
        private static bool CheckValidationResult1(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        // 返回数据流
        public static byte[] getResponseData(HttpWebResponse response)
        {
            try
            {
                string str = null;
                Stream s = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(s, UnInit.getEncoding()))
                {
                    str = sr.ReadToEnd();
                    sr.Close();
                }
                s.Close();
                return UnInit.getEncoding().GetBytes(str);
            }
            catch
            {
                return null;
            }
        }

    }
}
