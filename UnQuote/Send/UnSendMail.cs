using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Net.Mail;
using System.Net;
using UnCommon;

namespace UnQuote.Send
{
    public class UnSendMail
    {
        // eamil主机
        private string _emHost = "smtp.zztzfx.com";
        // email主机账号
        private string _emUser = "server@zztzfx.com";
        // email主机密码
        private string _emPass = "server-0000";

        // 实例化(发邮件)
        public UnSendMail(string emHost, string emUser, string emPass)
        {
            _emHost = emHost;
            _emUser = emUser;
            _emPass = emPass;
        }

        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="to">收件箱</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <returns>1成功,-1发送失败,-2创建实例失败</returns>
        public int send(string to, string subject, string body)
        {
            int i = 1;
            try
            {
                using (MailMessage message = new MailMessage(_emUser, to, subject, body))
                {
                    message.SubjectEncoding = Encoding.UTF8;
                    message.BodyEncoding = Encoding.UTF8;
                    message.IsBodyHtml = true;

                    SmtpClient mailClient = new SmtpClient(_emHost);
                    mailClient.Credentials = new NetworkCredential(_emUser, _emPass);

                    try
                    {
                        mailClient.Send(message);
                    }
                    catch
                    {
                        //发送失败
                        i = -1;
                    }
                }
            }
            catch
            {
                //创建实例失败
                i = -2;
            }
            return i;
        }

    }
}