using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnAli;
using Aop.Api;
using Aop.Api.Response;
using UnCommon.Tool;
using UnQuote;
using UnQuote.Images;
using UnCommon.Extend;

namespace UnTestWin
{
    public partial class Ali : Form
    {
        public Ali()
        {
            InitializeComponent();
        }


        // 支付宝接口支付实例,appid,privateKeyPem,aliPublicKeyPem
        UnAliMch mch;
        private void button1_Click(object sender, EventArgs e)
        {
            mch = new UnAliMch("2015082800238114", @"E:\BaiduCloudPan\Adata\HesAliRsaKey\rsa_private_key.pem", @"E:\BaiduCloudPan\Adata\HesAliRsaKey\ali_rsa_public_key.pem", "http://www.baidu.com");
            //UnAttrPub _config = new UnAttrPub();
            //_config.serverUrl = "https://openapi.alipay.com/gateway.do";
            //_config.app_id = "2015082800238114";
            //_config.version = "1.0";
            //_config.sign_type = "RSA";
            //_config.priKeyPem = @"E:\BaiduCloudPan\Adata\HesAliRsaKey\rsa_private_key.pem";
            //_config.aliPubKeyPem = @"E:\BaiduCloudPan\Adata\HesAliRsaKey\ali_rsa_public_key.pem";
            //mch = new UnAliMch(_config);

            UnAttrContent ct = new UnAttrContent();
            // * 商户订单号
            ct.out_trade_no = UnStrRan.getRandom();
            // * 订单金额
            ct.total_amount = "0.05";
            // * 订单标题
            ct.subject = "订单标题";

            // 订单描述
            //ct.body = "订单描述";
            // 该笔订单允许的最晚付款时间
            //ct.time_expire = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
            ct.timeout_express = "90m";
            // 商户操作员编号
            //ct.operator_id = "";
            // 商户门店编号
            //ct.store_id = "";
            // 机具终端编号`
            //ct.terminal_id = "";
            UnAttrExtend extend = new UnAttrExtend();
            extend.id = "1";
            extend.num = "1";
            extend.guid = Guid.NewGuid().ToString(); ;
            extend.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            extend.content = "自定义内容";
            //ct.extend_params = extend;

            // 商品详细列表
            List<UnAttrGoods> list = new List<UnAttrGoods>();
            UnAttrGoods gd = new UnAttrGoods();
            gd.goods_id = "apple-01";
            gd.goods_name = "ipad";
            gd.quantity = "1";
            gd.price = "0.05";
            list.Add(gd);
            //ct.goods_detail = list;

            AopResponse ret = mch.order(ct, UnAliMchEvent.预下单);
            if (ret != null)
            {
                AlipayTradePrecreateResponse res = (AlipayTradePrecreateResponse)ret;
                Console.WriteLine(res.Code + "/" + res.Msg);
                if (res.Code == "10000")
                {
                    string qrcPath = mch.getQRCPath(res.QrCode);
                    Console.WriteLine(res.QrCode + "/" + qrcPath);
                }
                else if (res.Code == "40004")
                {
                    Console.WriteLine(res.SubCode + "/" + res.SubMsg);
                    // 错误事件
                    UnAttrErrorEvent eve = mch.errorEventFromCode(res.SubCode);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mch = new UnAliMch("2015082800238114", @"E:\BaiduCloudPan\Adata\HesAliRsaKey\rsa_private_key.pem", @"E:\BaiduCloudPan\Adata\HesAliRsaKey\ali_rsa_public_key.pem", "http://www.baidu.com");
            UnAttrContent ct = new UnAttrContent();
            // * 商户订单号
            ct.out_trade_no = UnStrRan.getRandom();
            // * 支付场景
            ct.scene = "bar_code";
            // * 用户支付宝钱包中的“付款码”信息(手输/扫码)
            ct.auth_code = textBox1.Text;
            // * 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]，如果同时传入了【打折金额】，【不可打折金额】，【订单总 金额】三者，则必须满足如下条件:【订单总金额】=【打折金额】+【不可打折金额】
            ct.total_amount = "0.01";
            // * 订单标题
            ct.subject = "订单标题";

            // 商户操作员编号
            //ct.operator_id = "";
            // 商户门店编号
            //ct.store_id = "";
            // 机具终端编号
            //ct.terminal_id = "";

            AopResponse ret = mch.order(ct, UnAliMchEvent.条码支付);
            if (ret != null)
            {
                AlipayTradePayResponse res = (AlipayTradePayResponse)ret;
                Console.WriteLine(res.Code + "/" + res.Msg);
                // 订单交易成功
                if (res.Code == "10000")
                {
                    Console.WriteLine(res.TradeNo + "/" + res.OpenId + "/" + res.BuyerLogonId);
                }
                // 订单创建成功支付处理中
                else if (res.Code == "10003")
                {
                    Console.WriteLine(res.TradeNo + "/" + res.OpenId + "/" + res.BuyerLogonId);
                }
                else if (res.Code == "40004")
                {
                    Console.WriteLine(res.SubCode + "/" + res.SubMsg);
                    // 错误事件
                    UnAttrErrorEvent eve = mch.errorEventFromCode(res.SubCode);
                }
            }
        }

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine(e.KeyChar);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(e.KeyChar);
            this.Text = sb.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = "李1";
            Console.WriteLine(s.byteLength());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
