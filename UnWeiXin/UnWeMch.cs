using System.Collections.Generic;
using UnCommon.Entity;
using UnCommon.Extend;
using UnCommon.Files;
using UnCommon.HTTP;
using UnCommon.Tool;
using UnCommon.XMMP;
using UnQuote.Images;

namespace UnWeiXin
{
 
    public class UnWeMch
    {
        // 文档地址：https://pay.weixin.qq.com/wiki/doc/api/index.html
        // 公众账号ID
        private string _appid;
        // 商户号
        private string _mch_id;
        // 私钥
        private string _key;

        public UnWeMch()
        { 
        }
  
        // 实例化
        public UnWeMch(string appid,string mchid,string key)
        {
            _appid = appid;
            _mch_id = mchid;
            _key = key;
        }

        // 添加基础属性(appid,mch_id,nonce_str)
        private UnAttrOrder addBase(UnAttrOrder od)
        {
            od.appid = _appid;
            od.mch_id = _mch_id;
            if (od.nonce_str == null || od.nonce_str.Length < 16)
            {
                od.nonce_str = UnStrRan.getStr(16, 32);
            }
            return od;
        }

        public string sign<T>(T t) where T : new()
        {
            Dictionary<string, string> sdic = UnSign.filterPara(t, "sign");
            string str = UnSign.createLinkString(sdic) + "&key=" + _key;
            return str.md5Hash().ToUpper();
        }

        // 下单签名
        private string signOrder(UnAttrOrder od)
        {
            return sign(addBase(od));
        }

        // 通用接口
        public UnAttrReturn order(UnAttrOrder od, UnWeMchEvent evt, string cerPath)
        {
            od.sign = signOrder(od);
            string xml = UnXMMPXml.tToXml(typeof(UnAttrOrder), od);
            UnAttrReturn ret = new UnAttrReturn();
            UnHttpClient hc = new UnHttpClient(evt.apiUrl(), cerPath);
            UnAttrRst rst = hc.sendMsgSyn(xml);
            ret = (UnAttrReturn)UnXMMPXml.xmlToT(typeof(UnAttrReturn), (string)rst.data);
            if (evt == UnWeMchEvent.下载对账单)
            {
                if (ret == null)
                {
                    ret = new UnAttrReturn();
                    ret.return_code = "SUCCESS";
                    ret.out_trade_no = od.out_trade_no;
                    ret.transaction_id = od.transaction_id;
                    ret.cus_bill = (string)rst.data;
                }
            }
            else if (ret == null)
            {
                UnFile.writeLog("order", rst.code + " \r\n" + rst.msg + "\r\n" + (string)rst.data);
            }
            return ret;
        }

        public UnAttrReturn order(UnAttrOrder od, UnWeMchEvent evt, string packPath, string packPass)
        {
            od.sign = signOrder(od);
            string xml = UnXMMPXml.tToXml(typeof(UnAttrOrder), od);
            UnAttrReturn ret = new UnAttrReturn();
            UnHttpClient hc = new UnHttpClient(evt.apiUrl(), packPath, packPass);
            UnAttrRst rst = hc.sendMsgSyn(xml);
            ret = (UnAttrReturn)UnXMMPXml.xmlToT(typeof(UnAttrReturn), (string)rst.data);
            if (evt == UnWeMchEvent.下载对账单)
            {
                if (ret == null)
                {
                    ret = new UnAttrReturn();
                    ret.return_code = "SUCCESS";
                    ret.out_trade_no = od.out_trade_no;
                    ret.transaction_id = od.transaction_id;
                    ret.cus_bill = (string)rst.data;
                }
            }
            else if (ret == null)
            {
                UnFile.writeLog("order", rst.code + " \r\n" + rst.msg + "\r\n" + (string)rst.data);
            }
            return ret;
        }

        // 通用接口
        public UnAttrReturn order(UnAttrOrder od, UnWeMchEvent evt)
        {
            return order(od, evt, null);
        }

        // 生成并返回二维码图片路径
        public string getQRCPath(string code_url)
        {
            return UnImage.createQrcPath(code_url, 0, UnImageQRCEtr.Q).fullName;
        }

        // 校验签名
        public bool checkSign(UnAttrReturn ret)
        {
            Dictionary<string, string> dic = UnSign.filterPara(ret, "sign");
            if (sign(ret) == ret.sign)
            {
                return true;
            }
            return false;
        }

        // 错误事件
        public UnAttrErrorEvent errorEventFromCode(string code)
        {
            switch (code)
            {
                case "SYSTEMERROR":
                    return UnAttrErrorEvent.接口返回错误;
                case "PARAM_ERROR":
                    return UnAttrErrorEvent.参数错误;
                case "ORDERPAID":
                    return UnAttrErrorEvent.订单已支付;
                case "NOAUTH":
                    return UnAttrErrorEvent.商户无权限;
                case "AUTHCODEEXPIRE":
                    return UnAttrErrorEvent.二维码已过期;
                case "NOTENOUGH":
                    return UnAttrErrorEvent.余额不足;
                case "NOTSUPORTCARD":
                    return UnAttrErrorEvent.不支持卡类型;
                case "ORDERCLOSED":
                    return UnAttrErrorEvent.订单已关闭;
                case "ORDERREVERSED":
                    return UnAttrErrorEvent.订单已撤销;
                case "BANKERROR":
                    return UnAttrErrorEvent.银行系统异常;
                case "USERPAYING":
                    return UnAttrErrorEvent.用户支付中;
                case "AUTH_CODE_ERROR":
                    return UnAttrErrorEvent.授权码参数错误;
                case "AUTH_CODE_INVALID":
                    return UnAttrErrorEvent.授权码检验错误;
                case "XML_FORMAT_ERROR":
                    return UnAttrErrorEvent.XML格式错误;
                case "REQUIRE_POST_METHOD":
                    return UnAttrErrorEvent.请使用post方法;
                case "SIGNERROR":
                    return UnAttrErrorEvent.签名错误;
                case "LACK_PARAMS":
                    return UnAttrErrorEvent.缺少参数;
                case "NOT_UTF8":
                    return UnAttrErrorEvent.编码格式错误;
                case "BUYER_MISMATCH":
                    return UnAttrErrorEvent.支付帐号错误;
                case "APPID_NOT_EXIST":
                    return UnAttrErrorEvent.APPID不存在;
                case "MCHID_NOT_EXIST":
                    return UnAttrErrorEvent.MCHID不存在;
                case "OUT_TRADE_NO_USED":
                    return UnAttrErrorEvent.商户订单号重复;
                case "APPID_MCHID_NOT_MATCH":
                    return UnAttrErrorEvent.appid和mch_id不匹配;
            }
            return UnAttrErrorEvent.未知错误;
        }

    }
}
