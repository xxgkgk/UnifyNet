using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnEntity;
using UnCommon.Encrypt;
using UnDataBase;
using UnCommon.XMMP;
using System.Data.SqlClient;
using UnCommon;
using System.Xml;
using Jayrock.Json.Conversion;
using UnCommon.Tool;
using UnCommon.Files;
using UnCommon.Extend;

namespace UnTestWin
{
    public partial class Other : Form
    {
        public Other()
        {
            InitializeComponent();
        }

        public class TestClass
        {
            public int b { get; set; }
            public string a { get; set; }
            public string f { get; set; }
            public string z { get; set; }
            public string k { get; set; }
            public string e { get; set; }
            public string m { get; set; }
            public string o { get; set; }
            public List<string> list = new List<string>();
            public ApiBase ApiBase = new ApiBase();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UnSign unsign = new UnSign("123");

            XmlData xd = new XmlData();
            xd.ApiBase = new ApiBase();
            xd.WxPayOrder = new WxPayOrder();
            xd.WxPayOrder.Body = "dadfae";
            xd.WxPayOrder.AppID = "dffee";
            //xd.listString = new List<string>();
            //xd.listString.Add("eee");
          
            //xd.ArrayOfWxPayOrder = new List<WxPayOrder>();
            //xd.ArrayOfWxPayOrder.Add(xd.WxPayOrder);

            xd.NonceStr = "34343";// UnStrRan.getStr(16, 32);
            xd.ApiBase.Model = "WxPay";
            xd.ApiBase.Method = "JsAPI";

            string signstr = unsign.getSignString(xd);

            string s = unsign.sign(xd);
            bool isSign = unsign.validSign(xd, s);
            Console.WriteLine(signstr+"/"+s + "/" + isSign);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SortedDictionary<string, string> sPara = new SortedDictionary<string, string>();
            //sPara.Add("name", "3334");
            //string s = UnXMMPJson.tToJson(typeof(SortedDictionary<string, string>), sPara);
            //string s = UnXMMPXml.tToXml(typeof(SortedDictionary<string, string>), sPara);
            //Console.WriteLine(s);
            string xml = "";
            xml += "<xml>";
            xml += "<a_1><![CDATA[234dd]]></a_1>";
            xml += "<b_1>ddd</b_1>";
            xml += "</xml>";


            xml = xml.Replace("<xml>", "<parms>").Replace("</xml>", "</parms>");
            xml = xml.Replace("<![CDATA[", "").Replace("]]>", "");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            string json = UnXMMPJson.tToJson(typeof(XmlDocument), doc);
            Console.WriteLine(json);
            //XmlDocument doc1 = (XmlDocument)UnXMMPJson.jsonToT(typeof(XmlDocument), json);
            //Console.WriteLine(doc1.OuterXml);

            // bool b = UnSqlStr.validSafe(sql);
            //Console.WriteLine(b);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UnSql sql = new UnSql("Data Source=121.41.18.224,1433;Initial Catalog=AliDB;User ID=hpadmin;Password=cdhpadmin2013;");
            //UnSqlHelp help = new UnSqlHelp("Data Source=121.41.18.224,1433;Initial Catalog=AliDB;User ID=hpadmin;Password=cdhpadmin2013;");

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("par0", "HPKJ"));
            list.Add(new SqlParameter("par1", "cfc6f660-6824-4a77-9abf-097ebdaf9f29"));
            list.Add(new SqlParameter("par2", "7f0db3c7-77d0-4eeb-87ba-4914a99889d2"));

            List<string> args = new List<string>();
            args.Add("HPKJ");
            args.Add("cfc6f660-6824-4a77-9abf-097ebdaf9f29");
            args.Add("7f0db3c7-77d0-4eeb-87ba-4914a99889d2");

            List<AliPayOrder> ls = sql.query<AliPayOrder>(null, "Bus_Name = {0} And Bus_Trade_No In({1},{2})", args.ToArray(), null, true);
            Console.WriteLine(ls.Count);
            //DataTable dt = help.getDataTable("Select * From AliPayOrder Where Bus_Name = @par0 And Bus_Trade_No In(@par1)", list.ToArray());
            //Console.WriteLine(dt.Rows.Count);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string url = "https://dddd";
            bool b = UnStrReg.regUrl.IsMatch(url);


            Console.WriteLine(b);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string md5 = UnEncMD5.getMd5Hash(textBox1.Text, 0);
            Console.WriteLine(md5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            S s = new S();
            XmlData xd = new XmlData();
            //s.nonstr = "1133";

            s.c = "33";
            s.d = "44";
            s.z = 3;
            //s.o = new O();
            //s.o.e = "";
            //s.o.f = "ff";
            s.nonstr = "33";
            s.ArrayOfTaa = new List<Taa>();
            Taa taa = new Taa();
            taa.f = "ff";
            taa.e = "ee";
          
            s.ArrayOfTaa.Add(taa);
            s.Taa = taa;
            s.list = new List<string>();
            s.ints = new int[1];
            s.ints[0] = 2;
            s.strs = new string[2];
            

            //F f = new F();
            //f.s = new S();

            //string str = UnToGen.getFields(typeof(S));
            //string s1 = UnEncSign.splitJoint(s, null, false)+"&key=123";
            string s2 = new UnSign("123").sign(s);
            //string str = UnEncSign.sign(s,"123", UnEncSignEnu.MD5);
            //s.nonstr = "333";
            //bool isSign = UnEncSign.validSign(s.ArrayOfTaa, "123", UnEncSignEnu.MD5, str);
           // SortedDictionary<string, string> sort = UnToGen.getSignSortedDictionary<S>(s);
            //List<string> list = UnToGen.getSqlFields<S>();
            //Console.WriteLine(list.Count);

            //string str = UnEncSign.sign(s, null, "123", UnEncSignEnu.MD5);

            //bool b = UnEncSign.validSign(s, null, "123", UnEncSignEnu.MD5, str);
            //SortedDictionary<string, object> dic = UnToGen.tToSDic(s, null);

            Console.WriteLine(s2);
        }

        

        public class F
        {
            public string nonstr { get; set; }
            public string sign { get; set; }
            public byte[] bt { get; set; }
            //public S s { get; set; }
        }

        public class S : F
        {
            public string d { get; set; }
            public string c { get; set; }
            
            public int z { get; set; }
            public Taa Taa { get; set; }
            public List<Taa> ArrayOfTaa { get; set; }
            public List<string> list { get; set; }
            public int[] ints { get; set; }
            public string[] strs { get; set; }
        }

        public class Taa
        {
            public string f { get; set; }
            public string e { get; set; }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UnFileInfo info = UnQuote.Images.UnImage.createQrcPath("http://s.hesbbq.com/sale.apk", 0, UnQuote.Images.UnImageQRCEtr.H, 8);
            Console.WriteLine(info.fullName);
        }
    }
}
