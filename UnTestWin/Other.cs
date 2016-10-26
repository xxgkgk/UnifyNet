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
using System.Text.RegularExpressions;
using UnCommon.HTTP;
using UnCommon.Entity;
using UnQuote.Send;

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


        public XmlData getXD()
        {
            XmlData xd = new XmlData();
            xd.ApiBase = new ApiBase();
            xd.ApiBase.ArrayOfTest = new List<Test>();
            xd.ApiBase.Model = "adad";
            xd.ApiBase.Method = "ADDD.ee";
            xd.ApiBase.IsTest = "Y";

            xd.ArrayOfApiNote = new List<ApiNote>();

            Test test = new Test();
            test.a = 125;
            test.b = "addd李";

            Test test1 = new Test();
            test1.Testa = new Testa();
            test1.Testa.a = "aaa0";
            test1.Testa.d = "bbb0";

            test1.b = "addd李1";
            test1.a = 1251;
            xd.ApiBase.ArrayOfTest.Add(test1);
            xd.ApiBase.ArrayOfTest.Add(test);

            ApiNote an = new ApiNote();
            an.NoteCode = 1;
            an.NoteMsg = "msg";

            xd.ArrayOfApiNote.Add(an);
            xd.ArrayOfApiNote.Add(an);

            return xd;
        }

        


        private void button1_Click(object sender, EventArgs e)
        {
            UnSign unsign = new UnSign("hesdjaslf54asj1fkl10jaslfjslsdlj");
            //UnSign unsign = new UnSign("123");
            Console.WriteLine(UnStrRan.getStr(256, 512));
            return;
            XmlData xd = getXD();
            string xml = UnXMMPXml.tToXml(xd.GetType(), xd);

            string addsign = unsign.addSign(xml);


            UnHttpClient http = new UnHttpClient("http://192.168.100.108:81/Handler1.ashx");
            String addSign = unsign.addSign(xml);

            bool b = unsign.validSign(addSign);
            Console.WriteLine("validSign:" + b + "");
            UnAttrRst rst = http.sendMsgSyn(addSign);
            if (rst.code > 0)
            {
                Console.WriteLine("back:" + rst.back + "");
            }
            return;

            SortedDictionary<string, string> sort = unsign.getSignDictionary(xd);
            Console.WriteLine(sort.Count + "/");

            string signstr = unsign.getSignString(sort);
            Console.WriteLine(signstr + "/");

            xd.NonceStr = "123456";
            xd.Sign = unsign.sign(xd);
            Console.WriteLine(xd.Sign + "/");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlData xd = new XmlData();
            xd.ApiNote = new ApiNote();
            xd.ApiNote.NoteMsg = "3434";
            string xml = UnXMMPXml.tToXml(typeof(XmlData), xd);
            Console.WriteLine(xml);
            Regex reg = new Regex(@"<[^<]*\sp3:[^>]*>");
            xml = reg.Replace(xml, "");
            Console.WriteLine(xml);
            xd = (XmlData)UnXMMPXml.xmlToT(typeof(XmlData), xml);
            Console.WriteLine(xd.ApiNote.NoteMsg+"//");
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

        private void button9_Click(object sender, EventArgs e)
        {
            DataTable tblDatas = new DataTable("Datas");
            tblDatas.Columns.Add("序号", Type.GetType("System.Int32"));
            tblDatas.Columns[0].AutoIncrement = true;
            tblDatas.Columns[0].AutoIncrementSeed = 1;
            tblDatas.Columns[0].AutoIncrementStep = 1;
            tblDatas.Columns.Add("原始单号", Type.GetType("System.String"));
            tblDatas.Columns.Add("流水单号", Type.GetType("System.String"));
            tblDatas.Columns.Add("结账时间", Type.GetType("System.String"));
            tblDatas.Columns.Add("状态", Type.GetType("System.String"));
            tblDatas.Columns.Add("门店", Type.GetType("System.String"));
            tblDatas.Columns.Add("交易类型", Type.GetType("System.String"));
            tblDatas.Columns.Add("消费合计(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("收款金额(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("实收金额(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("退款金额(元)", Type.GetType("System.String"));
            tblDatas.Columns.Add("会员顾客", Type.GetType("System.String"));
            tblDatas.Columns.Add("收银员", Type.GetType("System.String"));
            tblDatas.Columns.Add("列13", Type.GetType("System.String"));
            tblDatas.Columns.Add("列14", Type.GetType("System.String"));
            tblDatas.Columns.Add("列15", Type.GetType("System.String"));
            tblDatas.Columns.Add("列16", Type.GetType("System.String"));
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });
            tblDatas.Rows.Add(new object[] { null, "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15", "v16" });

            UnExport ep = new UnExport();
            string s = ep.dtToCSV(tblDatas, (string)null).ToString();
            Console.WriteLine(s);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UnSendSMSTxy txy = new UnSendSMSTxy(1400016617, "c21e95cbb478325141937e3a497c8729");
            List<string> list = new List<string>();
            list.Add("18980826967");
            list.Add("18684011818");
            list.Add("18284563899");
            var rst = txy.sendMsg("86", list, "亲，您的验证码是：22324，打死也不要告诉任何人哦！如非本人操作，请忽略本短信。 ");
            Console.WriteLine(rst.code);
            Console.WriteLine(rst.msg);
        }
    }
}
