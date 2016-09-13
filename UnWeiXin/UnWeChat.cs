using System;
using System.Collections.Generic;
using System.Web;
using UnCommon.Entity;
using UnCommon.Files;
using UnCommon.HTTP;
using UnCommon.Tool;
using UnCommon.XMMP;

namespace UnWeiXin
{
    /// <summary>
    /// 微信公众号基础类
    /// </summary>
    public class UnWeChat
    {
        /// <summary>
        /// 应用id
        /// </summary>
        private string _appid = "wx8162f4aa084d5a0e";
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string _appsecret = "f8019b1d64f3199d9c44d60c5dc98142";

        /// <summary>
        /// 接口地址
        /// </summary>
        private static string _url = "https://api.weixin.qq.com/cgi-bin/";
        /// <summary>
        /// 网页授权接口地址
        /// </summary>
        private static string _urlWeb = "https://api.weixin.qq.com/sns/";

        /// <summary>
        /// 获取token
        /// </summary>
        private string _urlToken = _url + "token?grant_type=client_credential";
        /// <summary>
        /// 创建菜单
        /// </summary>
        private string _urlCreateMenu = _url + "menu/create";
        /// <summary>
        /// 删除菜单
        /// </summary>
        private string _urlDeleteMenu = _url + "menu/delete";
        /// <summary>
        /// 创建二维码
        /// </summary>
        private string _urlCreateQrcode = _url + "qrcode/create";
        /// <summary>
        /// 二维码图片地址
        /// </summary>
        private string _urlShowQrcode = "https://mp.weixin.qq.com/cgi-bin/showqrcode";
        /// <summary>
        /// 用户信息
        /// </summary>
        private string _urlUserInfo = _url + "user/info";
        /// <summary>
        /// 提交素材
        /// </summary>
        private string _urlUpload = _url + "user/upload?access_token=ACCESS_TOKEN&type=TYPE";
        /// <summary>
        /// 网页授权
        /// </summary>
        private string _urlAuthorize = "https://open.weixin.qq.com/connect/oauth2/authorize";
        /// <summary>
        /// 网页令牌
        /// </summary>
        private string _urlWebToken = _urlWeb + "oauth2/access_token";
        /// <summary>
        /// 网页令牌刷新
        /// </summary>
        private string _urlWebRefreshToken = _urlWeb + "oauth2/refresh_token";
        /// <summary>
        /// 网页用户信息
        /// </summary>
        private string _urlWebUserInfo = _urlWeb + "userinfo";
        /// <summary>
        /// allow
        /// </summary>
        private bool _allowRefresh = false;
        /// <summary>
        /// token
        /// </summary>
        private string _access_token = string.Empty;

        /// <summary>
        /// 令牌接口
        /// </summary>
        private UnItfToken _itfToken;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        public UnWeChat(string appid, string appsecret)
        {
            this._appid = appid;
            this._appsecret = appsecret;
            this._urlToken += "&appid=" + _appid + "&secret=" + _appsecret;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="itf"></param>
        /// <param name="allowRefresh"></param>
        public UnWeChat(string appid, string appsecret, UnItfToken itf, bool allowRefresh)
        {
            this._appid = appid;
            this._appsecret = appsecret;
            this._urlToken += "&appid=" + _appid + "&secret=" + _appsecret;
            this._itfToken = itf;
            this._allowRefresh = allowRefresh;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="itf"></param>
        public UnWeChat(string appid, string appsecret, UnItfToken itf)
        {
            this._appid = appid;
            this._appsecret = appsecret;
            this._urlToken += "&appid=" + _appid + "&secret=" + _appsecret;
            this._itfToken = itf;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="accesstoken"></param>
        public UnWeChat(string appid, string appsecret, string accesstoken)
        {
            this._appid = appid;
            this._appsecret = appsecret;
            this._urlToken += "&appid=" + _appid + "&secret=" + _appsecret;
            this._access_token = accesstoken;
        }

        /// <summary>
        /// 设置全局Token委托
        /// </summary>
        /// <param name="itf"></param>
        public void setItfToken(UnItfToken itf)
        {
            this._itfToken = itf;
        }

        /// <summary>
        /// 接收地址验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        public void valid(HttpContext context, string token)
        {
            var echostr = context.Request["echoStr"].ToString();
            var signature = context.Request["signature"].ToString();
            var timestamp = context.Request["timestamp"].ToString();
            var nonce = context.Request["nonce"].ToString();
            if (checkSignature(signature, timestamp, nonce, token) && !string.IsNullOrEmpty(echostr))
            {
                context.Response.Write(echostr);
                context.Response.End();
            }
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool checkSignature(string signature, string timestamp, string nonce, string token)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            //字典排序 
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 公众号ID
        /// </summary>
        /// <returns></returns>
        public string getAppID()
        {
            return _appid;
        }

        /// <summary>
        /// 同步提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string sendMsgSyn(string url, string msg)
        {
            string str = null;
            UnHttpClient hc = new UnHttpClient(url);
            UnAttrRst rst = hc.sendMsgSyn(msg);
            if (rst.code == 1)
            {
                str = (string)rst.data;
            }
            if (str == null)
            {
                UnFile.writeLog("sendMsgSyn", rst.code + ":" + rst.msg + "\r\n" + url + "\r\n" + msg);
            }
            return str;
        }

        // 获取token状态
        //private static bool onGet = false;

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string token()
        {
            if (_access_token != string.Empty && _access_token != null)
            {
                return _access_token;
            }
            string tk = this._itfToken.getToken();
            if (tk == null && _allowRefresh)
            {
                UnHttpClient hc = new UnHttpClient(_urlToken);
                UnAttrRst rst = hc.sendMsgSyn("");
                try
                {
                    Dictionary<string, object> dic = UnXMMPJson.jsonToDicSingle((string)rst.data, "Json");
                    tk = dic["access_token"].ToString();
                    int expires = Convert.ToInt32(dic["expires_in"].ToString());
                    _itfToken.backToken(_appid, tk, expires);
                }
                catch (Exception e)
                {
                    tk = null;
                    UnFile.writeLog("token", e.ToString());
                }
                finally
                {
                }
            }
            return tk;
        }

        /// <summary>
        /// 检查结果是否成功
        /// </summary>
        /// <param name="rst"></param>
        /// <returns></returns>
        private bool checkErr(UnAttrRst rst)
        {
            try
            {
                UnAttrErr err = (UnAttrErr)UnXMMPJson.jsonToT(typeof(UnAttrErr), (string)rst.data);
                if (err.errcode == 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// 新建菜单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string createMenu(string json)
        {
            string url = this._urlCreateMenu + "?access_token=" + token();
            return sendMsgSyn(url, json);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        public bool deleteMeun()
        {
            string url = this._urlDeleteMenu += "?access_token=" + token();
            if (sendMsgSyn(url, "") != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="qrc"></param>
        /// <returns></returns>
        public string createQRC(UnAttrQRC qrc)
        {
            string url = this._urlCreateQrcode + "?access_token=" + token();
            //string json = UnXMMPJson.tToJson(typeof(UnAttrQRC), qrc);
            string json = "{\"expire_seconds\": " + qrc.expire_seconds + ", \"action_name\": \"" + qrc.action_name + "\", \"action_info\": {\"scene\": {\"scene_id\": " + qrc.scene_id + "}}}";
            UnHttpClient hc = new UnHttpClient(url);
            UnAttrRst rst = hc.sendMsgSyn(json);
            try
            {
                //UnFile.writeLog("createQRC00", token() + "\r\n" + json + "\r\n" + (string)rst.data);
                UnAttrTicket ti = (UnAttrTicket)UnXMMPJson.jsonToT(typeof(UnAttrTicket), (string)rst.data);
                string ticket = System.Web.HttpUtility.UrlEncode(ti.ticket, System.Text.Encoding.UTF8);
                return _urlShowQrcode + "?ticket=" + ticket;
            }
            catch
            {
                try
                {
                    UnFile.writeLog("createQRC", (string)rst.data);
                }
                catch (Exception e)
                {
                    UnFile.writeLog("createQRC", e.ToString());
                }
            }
            return null;
        }

        /// <summary>
        /// 获得用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string getUserInfo(string openid)
        {
            string url = this._urlUserInfo + "?access_token=" + token() + "&openid=" + openid + "&lang=zh_CN";
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 获取授权url
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="reurl"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string getAuthUrl(string scope, string reurl, string state)
        {
            string url = this._urlAuthorize + "?appid=" + _appid + "&redirect_uri=" + reurl + "&response_type=code&scope=" + scope + "&state=" + state + "#wechat_redirect";
            return url;
        }

        /// <summary>
        /// 获取网页access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getWebToken(string code)
        {
            string url = this._urlWebToken + "?appid=" + _appid + "&secret=" + _appsecret + "&code=" + code + "&grant_type=authorization_code";
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 获取网页refresh_token
        /// </summary>
        /// <param name="reToken"></param>
        /// <returns></returns>
        public string getWebReToken(string reToken)
        {
            string url = this._urlWebRefreshToken + "?appid=" + _appid + "&grant_type=refresh_token&refresh_token=" + reToken;
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 获取网页用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="webToken"></param>
        /// <returns></returns>
        public string getWebUserInfo(string openid, string webToken)
        {
            string url = this._urlWebUserInfo + "?access_token=" + webToken + "&openid=" + openid + "&lang=zh_CN";
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 添加临时素材
        /// </summary>
        /// <returns></returns>
        public string upMaterial()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=" + token();
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 获取jsticket
        /// </summary>
        /// <returns></returns>
        public string getJsapiTicket()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token() + "&type=jsapi";
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 获取文本XML
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string getTextXML(UnAttrMsg msg)
        {
            msg.MsgType = "text";
            msg.CreateTime = Convert.ToInt64(UnDate.ticksSec());
            return UnXMMPXml.tToXmlNoSm(typeof(UnAttrMsg), msg);
        }

        /// <summary>
        /// 获取图文XML
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string getNewsXML(UnAttrMsg msg)
        {
            msg.MsgType = "news";
            msg.CreateTime = Convert.ToInt64(UnDate.ticksSec());
            msg.ArticleCount = msg.Articles.Count.ToString();

            string xml = "<xml>";
            xml += UnXMMPXml.tToXml(null, null, msg, (string)null);
            xml += UnXMMPXml.tToXml("Articles", "item", msg.Articles, (string)null);
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        public string setIndustry(string id1, string id2)
        {
            string str = "{'industry_id1':'" + id1 + "','industry_id2':'" + id2 + "'}";
            string url = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token=" + token();
            return sendMsgSyn(url, str);
        }

        /// <summary>
        /// 获取设置的行业信息
        /// </summary>
        /// <returns></returns>
        public string getIndustry()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/template/get_industry?access_token=" + token();
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 获取设置的行业信息
        /// </summary>
        /// <returns></returns>
        public UnAttrTemplate getIndustryToT()
        {
            string rtstr = getIndustry();
            UnAttrTemplate tmp = (UnAttrTemplate)UnXMMPJson.jsonToT(typeof(UnAttrTemplate), rtstr);
            if (tmp == null)
            {
                UnFile.writeLog("getIndustryToT", rtstr);
            }
            return tmp;
        }

        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="template_id"></param>
        /// <returns></returns>
        public string addTemplate(string template_id)
        {
            string str = "{'template_id'='" + template_id + "'}";
            string url = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token=" + token();
            return sendMsgSyn(url, str);
        }

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        public string getAllPrivateTemplate()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token=" + token();
            return sendMsgSyn(url, "");
        }

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        public List<UnAttrTmpInfo> getAllPrivateTemplateToT()
        {
            string rtstr = getAllPrivateTemplate();
            UnAttrTemplate tmp = (UnAttrTemplate)UnXMMPJson.jsonToT(typeof(UnAttrTemplate), rtstr);
            if (tmp == null || tmp.template_list == null)
            {
                UnFile.writeLog("getAllPrivateTemplateToT", rtstr + "\r\n" + token());
            }
            return tmp.template_list;
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="template_id"></param>
        /// <returns></returns>
        public string delPrivateTemplate(string template_id)
        {
            string str = "{'template_id'='" + template_id + "'}";
            string url = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?access_token=" + token();
            return sendMsgSyn(url, str);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="template_id"></param>
        /// <returns></returns>
        public UnAttrErr delPrivateTemplateToT(string template_id)
        {
            string str = delPrivateTemplate(template_id);
            UnAttrErr err = (UnAttrErr)UnXMMPJson.jsonToT(typeof(UnAttrErr), str);
            return err;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string sendTemplate(string str)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token();
            return sendMsgSyn(url, str);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public UnAttrErr sendTemplate(UnAttrTemplate tmp)
        {
            // 如果data数据是用ArrayOfUnAttrTmpSend承载
            if (tmp.ArrayOfUnAttrTmpSend != null && tmp.data == null)
            {
                tmp.data = new Dictionary<string, UnAttrTmpData>();
                foreach (var send in tmp.ArrayOfUnAttrTmpSend)
                {
                    tmp.data.Add(send.name, new UnAttrTmpData(send.value, send.color));
                }
            }
            string str = UnXMMPJson.tToJson(typeof(UnAttrTemplate), tmp);
            UnAttrErr err = (UnAttrErr)UnXMMPJson.jsonToT(typeof(UnAttrErr), sendTemplate(str));
            if (err == null)
            {
                UnFile.writeLog("sendTemplate", str + "\r\n" + token());
            }
            return err;
        }
    }
}
