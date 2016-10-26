using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UnCommon.Entity;
using UnCommon.Files;
using UnCommon.XMMP;

namespace UnQuote.Send
{
    /// <summary>
    /// 腾讯云短信
    /// </summary>
    public class UnSendSMSTxy
    {
        /// <summary>
        /// appid
        /// </summary>
        int sdkappid;

        /// <summary>
        /// appkey
        /// </summary>
        string appkey;

        /// <summary>
        /// 网关
        /// </summary>
        string url = "https://yun.tim.qq.com/v3/tlssmssvr/sendmultisms2";

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="sdkappid"></param>
        /// <param name="appkey"></param>
        public UnSendSMSTxy(int sdkappid, string appkey)
        {
            this.sdkappid = sdkappid;
            this.appkey = appkey;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="nationCode">区号(NULL则默认86)</param>
        /// <param name="phoneNumbers">群发电话号码</param>
        /// <param name="content">发送内容(必须按模板格式)</param>
        /// <returns>返回JSON结果串</returns>
        private string sendMsgs(string nationCode, List<string> phoneNumbers, string content)
        {
            if (nationCode == null)
            {
                nationCode = "86";
            }
            if (0 == phoneNumbers.Count)
            {
                return null;
            }

            JObject data = new JObject();
            JArray tel = new JArray();
            for (int i = 0; i < phoneNumbers.Count; i++)
            {
                JObject telElement = new JObject();
                telElement.Add("nationcode", nationCode);
                telElement.Add("phone", phoneNumbers[i]);
                tel.Add(telElement);
            }
            data.Add("msg", content);
            data.Add("type", "0");          // 默认普通短信
            string sig = calculateSig(appkey, phoneNumbers);
            data.Add("sig", sig);
            data.Add("tel", tel);
            data.Add("extend", "");         // 根据需要添加，一般保持默认
            data.Add("ext", "");            // 根据需要添加，一般保持默认
            string msgString = JsonConvert.SerializeObject(data);
            Console.WriteLine(msgString);

            try
            {
                // 发送 POST 请求
                Random rnd = new Random();
                int random = rnd.Next(1000000) % (900000) + 1000000;
                string wholeUrl = url + "?sdkappid=" + sdkappid + "&random=" + random;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(wholeUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] requestData = Encoding.UTF8.GetBytes(msgString);
                request.ContentLength = requestData.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(requestData, 0, requestData.Length);
                requestStream.Close();

                // 接收返回包
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                string retString = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();

                Console.WriteLine(msgString);
                return retString;
            }
            catch (Exception e)
            {
                UnFile.writeLog("sendMsg", e.ToString());
            }
            return null;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="nationCode">区号(NULL则默认86)</param>
        /// <param name="phoneNumbers">群发电话号码</param>
        /// <param name="content">发送内容(必须按模板格式)</param>
        /// <returns>code:1=全部成功,0=部分成功,-1=全部失败</returns>
        public UnAttrRst sendMsg(string nationCode, List<string> phoneNumbers, string content)
        {
            UnAttrRst rst = new UnAttrRst();
            string retString = sendMsgs(nationCode, phoneNumbers, content);
            var dics = UnXMMPJson.jsonToDic(retString, null);
            if (dics != null)
            {
                int num = 0;
                foreach (var dic in dics)
                {
                    switch (dic["result"].ToString())
                    {
                        case "0":
                            num++;
                            break;
                        default:
                            break;
                    }
                }
                if (num > 0)
                {
                    if (num == dics.Count)
                    {
                        rst.code = 1;
                        rst.msg = "OK";
                    }
                    else
                    {
                        rst.code = 0;
                        rst.msg = retString;
                    }
                }
                else
                {
                    rst.code = -1;
                    rst.msg = retString;
                }

            }
            return rst;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="nationCode">区号(NULL则默认86)</param>
        /// <param name="phoneNumber">单个电话号码</param>
        /// <param name="content">发送内容(必须按模板格式)</param>
        /// <returns>code:1=全部成功,0=部分成功,-1=全部失败</returns>
        public UnAttrRst sendMsg(string nationCode, string phoneNumber, string content)
        {
            List<string> list = new List<string>();
            list.Add(phoneNumber);
            return sendMsg(nationCode, list, content);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="appkey">密码</param>
        /// <param name="phoneNumbers">电话号码组</param>
        /// <returns>签名后字串</returns>
        private static string calculateSig(string appkey, List<string> phoneNumbers)
        {
            string str = appkey + phoneNumbers[0];
            for (int i = 1; i < phoneNumbers.Count; i++)
            {
                str = str + "," + phoneNumbers[i];
            }
            Console.WriteLine(str);
            return stringMD5(str);
        }

        /// <summary>
        /// md5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string stringMD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] targetData = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return byteToHexStr(targetData);
        }

        /// <summary>
        /// 将二进制的数值转换为 16 进制字符串，如 "abc" => "616263"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string byteToHexStr(byte[] input)
        {
            string returnStr = "";
            if (input != null)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    returnStr += input[i].ToString("x2");
                }
            }
            return returnStr;
        }
    }
}
