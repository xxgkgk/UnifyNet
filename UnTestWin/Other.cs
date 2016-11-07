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


        


        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlData xd = new XmlData();
            xd.ArrayOfApiNote = new List<ApiNote>();
            xd.ApiNote = new ApiNote();
            xd.ApiNote.NoteCode = null;
            xd.ApiNote.NoteMsg = null;

            Testa testa = new Testa();
            testa.a = "a";
            testa.AddTime = UnDate.shortNowTime();
            testa.b = null;
            testa.c = "c";
            testa.d = "d";
            testa.e = "e";
            testa.f = "f";

            xd.Test = testa;
            xd.Testa = new Testa();
            xd.ApiBase = new ApiBase()
            {
                IsTest = "dd",
                Model = "md",
                Method = "me",
                guid = null
            };
            xd.ApiBase.ArrayOfTest = new List<Test>();
            ApiNote note = new ApiNote();
            note.NoteCode = 1;
            note.NoteMsg = "ddd";

            ApiNote note1 = new ApiNote();
            note1.NoteCode = 2;
            note1.NoteMsg = "eeee";

            xd.ArrayOfApiNote.Add(note);
            xd.ArrayOfApiNote.Add(note1);

            //xd.ArrayOfApiNote = null;
            //xd.ApiBase = null;

            //string s = UnStrReg.regPartEmail.Replace("add222@qq.com", "$1***$2");

            string s = UnXMMPXml.tToXml(typeof(XmlData), xd);
            s = @"<XmlData><ApiNote><NoteCode>1</NoteCode><NoteMsg>OK：查询成功</NoteMsg></ApiNote><Cards><CardsID>52923</CardsID><CardsUID>2016102810551249399747415</CardsUID><CardsGUID>01a7145c-b7b1-44de-9fa0-24002c0bc352</CardsGUID><CardsChipNo>0008281529</CardsChipNo><CardsNumber>0008281529</CardsNumber><AddTime>2016-10-28 10:54:56</AddTime><AddTimeStamp>1477623296567</AddTimeStamp><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CardCreateRecordGUID>0fffe23a-ac7d-4747-981b-af7bbc2b24da</CardCreateRecordGUID><MemberInfoGUID>f138cb98-7146-46b8-868b-15a131f59a44</MemberInfoGUID><Balance>1468.2600</Balance><Balance1>978.7308</Balance1><Balance2>489.5292</Balance2><EffectTime>2016-10-28 00:00:00</EffectTime><EffectTimeStamp>1477584000000</EffectTimeStamp><InvalidTime>2016-10-27</InvalidTime><InvalidTimeStamp>1477929600000</InvalidTimeStamp><CardType p3:type=""CardTypeExt"" xmlns:p3=""http://www.w3.org/2001/XMLSchema-instance"" ><CardTypeID>67</CardTypeID><CardTypeUID>20161027184835394</CardTypeUID><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CreateNum>1</CreateNum><CardTypeName>过期过期27</CardTypeName><NeedBind>1</NeedBind><IsPhysical>1</IsPhysical><FaceValue>1500.0000</FaceValue><Price>1000.0000</Price><FlatCost>12.0000</FlatCost><RechargeType>0</RechargeType><PaymentCiscount>0.5000</PaymentCiscount><IsPaymentCiscount>1</IsPaymentCiscount><RightsCiscount>0.2000</RightsCiscount><IsRightsCiscount>1</IsRightsCiscount><EffectTimeStamp>0</EffectTimeStamp><InvalidTime>2016-10-27</InvalidTime><InvalidTimeStamp>1477929600000</InvalidTimeStamp><EffectDuration>0</EffectDuration><InvalidDuration>0</InvalidDuration><ArrayOfViewCardTypeBindStore><ViewCardTypeBindStore><CardTypeBindStoreID>780</CardTypeBindStoreID><CardTypeBindStoreUID>20161027184835398</CardTypeBindStoreUID><CardTypeBindStoreGUID>273b17db-e039-4011-9224-14b29814e9fa</CardTypeBindStoreGUID><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><StoreGUID>59914f92-72e6-4935-8f71-921eb11bbea2</StoreGUID><ViewCardTypeBindStoreID>780</ViewCardTypeBindStoreID><Name>高升</Name><Address>四川省成都市武侯区</Address></ViewCardTypeBindStore><ViewCardTypeBindStore><CardTypeBindStoreID>781</CardTypeBindStoreID><CardTypeBindStoreUID>20161027184835400</CardTypeBindStoreUID><CardTypeBindStoreGUID>e9853606-8a76-455b-afcc-5f68cbb8783f</CardTypeBindStoreGUID><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><StoreGUID>0bc8d932-1ae7-49bf-86c8-06bdc2c98e80</StoreGUID><ViewCardTypeBindStoreID>781</ViewCardTypeBindStoreID><Name>丽都店</Name><Address>四川省成都市龙泉驿区</Address></ViewCardTypeBindStore><ViewCardTypeBindStore><CardTypeBindStoreID>784</CardTypeBindStoreID><CardTypeBindStoreUID>20161027184835406</CardTypeBindStoreUID><CardTypeBindStoreGUID>77528182-f299-49f3-95dd-7389b47dfeb3</CardTypeBindStoreGUID><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><StoreGUID>7b664309-2fc5-4fa4-b0ce-ad6dd9a74779</StoreGUID><ViewCardTypeBindStoreID>784</ViewCardTypeBindStoreID><Name>的萨达但是爱的</Name><Address>四川省广安市</Address></ViewCardTypeBindStore><ViewCardTypeBindStore><CardTypeBindStoreID>783</CardTypeBindStoreID><CardTypeBindStoreUID>20161027184835404</CardTypeBindStoreUID><CardTypeBindStoreGUID>38ffdd1f-6d34-4a50-8806-ad46e4a9b5ca</CardTypeBindStoreGUID><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><StoreGUID>708ce206-9679-40aa-933c-44878211383b</StoreGUID><ViewCardTypeBindStoreID>783</ViewCardTypeBindStoreID><Name>北门店</Name><Address>四川省</Address></ViewCardTypeBindStore><ViewCardTypeBindStore><CardTypeBindStoreID>782</CardTypeBindStoreID><CardTypeBindStoreUID>20161027184835402</CardTypeBindStoreUID><CardTypeBindStoreGUID>2a64d9af-8d57-4e7b-b36b-b57dd2107ce8</CardTypeBindStoreGUID><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><StoreGUID>caab4c9d-6169-4803-873a-467b89d81d28</StoreGUID><ViewCardTypeBindStoreID>782</ViewCardTypeBindStoreID><Name>华阳店</Name><Address>四川省成都市龙泉驿区</Address></ViewCardTypeBindStore></ArrayOfViewCardTypeBindStore><ArrayOfCardRechargeLadder /><IsExpire>0</IsExpire><Status>1</Status><EnteringStatus>1</EnteringStatus></CardType><MemberInfo><MemberInfoID>1088820</MemberInfoID><MemberInfoUID>20161027105913619</MemberInfoUID><MemberInfoGUID>f138cb98-7146-46b8-868b-15a131f59a44</MemberInfoGUID><IsDelete>0</IsDelete><RegTel>18811772985</RegTel><RegDevice>0</RegDevice><TradingPassWord>2466d6104a1e79065f16fc1b3b3564ff</TradingPassWord><RegDateTime>2016-10-27 10:59:13</RegDateTime><RegDateTimeStamp>1477537153322</RegDateTimeStamp><NickName>aaa</NickName><BrithdayStamp p4:nil=""true"" xmlns:p4=""http://www.w3.org/2001/XMLSchema-instance"" /><Sex>0</Sex><CitysGUID p4:nil=""true"" xmlns:p4=""http://www.w3.org/2001/XMLSchema-instance"" /></MemberInfo><StoreGUID>59914f92-72e6-4935-8f71-921eb11bbea2</StoreGUID></Cards><CardStock><CardStockID>600087</CardStockID><CardStockUID>20161027184913450</CardStockUID><CardStockGUID>dd54447f-4146-44a5-8c33-d202532eb434</CardStockGUID><CardsChipNo>0008281529</CardsChipNo><CardsNumber>0008281529</CardsNumber><CardStockType>1</CardStockType><CardStockState>1</CardStockState><AddTime>2016-10-27 18:49:13</AddTime><AddTimeStamp>1477565353829</AddTimeStamp><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><CardType><CardTypeID>67</CardTypeID><CardTypeUID>20161027184835394</CardTypeUID><CardTypeGUID>297e99fa-ff1e-4544-8ba0-a618c41a33d4</CardTypeGUID><IsDelete>0</IsDelete><IsEnable>1</IsEnable><CreateNum>1</CreateNum><CardTypeName>过期过期27</CardTypeName><NeedBind>1</NeedBind><IsPhysical>1</IsPhysical><FaceValue>1500.0000</FaceValue><Price>1000.0000</Price><FlatCost>12.0000</FlatCost><RechargeType>0</RechargeType><PaymentCiscount>0.5000</PaymentCiscount><IsPaymentCiscount>1</IsPaymentCiscount><RightsCiscount>0.2000</RightsCiscount><IsRightsCiscount>1</IsRightsCiscount><EffectTimeStamp>0</EffectTimeStamp><InvalidTime>2016-10-27</InvalidTime><InvalidTimeStamp>1477929600000</InvalidTimeStamp><EffectDuration>0</EffectDuration><InvalidDuration>0</InvalidDuration></CardType></CardStock><GeneralMsg><IsAvailable>1</IsAvailable><Msg /></GeneralMsg></XmlData>";
            //XmlData inxd = (XmlData)UnXMMPXml.xmlToT(typeof(XmlData),s);
            //Console.WriteLine(inxd.ApiNote.NoteCode + "//");
            //Console.WriteLine(inxd.Test.a + "//");
            //Console.WriteLine(inxd.ArrayOfApiNote[0].NoteMsg + "//");
            //string s = @"<Test p2:type='Testa' xmlns:p2='http://www.w3.org/2001/XMLSchema-instance'><e>e</e><f>f</f><a>a</a><b p2:nil='true' /><c>c</c><d>d</d><AddTime>2016-10-31 10:21:01</AddTime></Test>";
            UnXMMPXml.removeXmlNotes(ref s);
            Console.WriteLine(s);
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
            //UnFileInfo info = UnQuote.Images.UnImage.createQrcPath("http://s.hesbbq.com/app/sale.apk", 0, UnQuote.Images.UnImageQRCEtr.H, 8);
            UnFileInfo info = UnQuote.Images.UnImage.createQrcPath("http://s.hesbbq.com/app/padorder.apk", 0, UnQuote.Images.UnImageQRCEtr.H, 8);
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
