using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnCommon;
using System.Web;
using UnCommon.Entity;
using UnCommon.HTTP;

namespace UnQuote.Send
{
 
    /// <summary>
    /// 发短信工具
    /// </summary>
    public class UnSendSMS
    {
        // 平台类型
        private UnSendSMSType _type = UnSendSMSType.默认;
        // 账号
        private string _User = "LKSDK0001641";
        // 密码
        private string _Pass = "HPKJSMS123";

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="type">厂商</param>
        /// <param name="user">账号</param>
        /// <param name="pass">密码</param>
        public UnSendSMS(UnSendSMSType type, string user, string pass)
        {
            _type = type;
            _User = user;
            _Pass = pass;
        }

        /// <summary>
        /// 发消息 
        /// </summary>
        /// <param name="pNum">手机号</param>
        /// <param name="sCont">类型</param>
        /// <param name="sTime">发送时间,null则为当前时间</param>
        /// <returns></returns>
        public UnAttrRst send(string pNum, string sCont, string sTime)
        {
            switch (_type)
            {
                case UnSendSMSType.同创凌凯:
                    return sendTclk(pNum, sCont, sTime);
                default:
                    return sendTclk(pNum, sCont, sTime);
            }
        }

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        public UnAttrRst queryBalance()
        {
            switch (_type)
            {
                case UnSendSMSType.同创凌凯:
                    return queryBalanceTclk();
                default:
                    return queryBalanceTclk();
            }
        }

        #region 同创凌凯[http://sdk.mb345.com:88]

        private string _urlTclk = "http://mb345.com:999";
 
        // 发短信;
        private UnAttrRst sendTclk(string pNum, string sCont, string sTime)
        {
            if (sTime == null)
            {
                sTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            sCont = HttpUtility.UrlEncode(sCont, Encoding.GetEncoding("GB2312"));
            string _url = _urlTclk + "/WS/Send.aspx?CorpID=" + _User + "&Pwd=" + _Pass + "&Mobile=" + pNum + "&Content=" + sCont + "&Cell=&SendTime=" + sTime;
            UnHttpClient hc = new UnHttpClient(_url);
            UnAttrRst rst = hc.sendMsgSyn("");
            return rst;
        }

        // 查剩余条数
        private UnAttrRst queryBalanceTclk()
        {
            return new UnHttpClient(_urlTclk + "/WS/SelSum.aspx?CorpID=" + _User + "&Pwd=" + _Pass).sendMsgSyn("");
        }

        #endregion

    }
}
