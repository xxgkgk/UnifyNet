using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UnCommon;
using UnCommon.Config;
using UnCommon.Encrypt;
using UnCommon.Redis;
using UnCommon.Tool;
using UnCommon.XMMP;
using UnDataBase;
using UnEntity;

namespace UnTestWin
{
    public partial class Sql : Form
    {
        public Sql()
        {
            InitializeComponent();
        }


        //UnSql cn = null;
        private void button1_Click(object sender, EventArgs e)
        {
            List<Type> listT = new List<Type>();
            listT.Add(typeof(TestUser));
            listT.Add(typeof(TestUserDetail));
            listT.Add(typeof(TestCardType));
            listT.Add(typeof(TestCard));

            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "master");
            cn.createDataBase("D:/DBFile", "AEnterprise1");

            cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", true);
            cn.updateBase();
            cn.createTableList(listT);
            cn.createTableRelationList(listT);
            cn.commit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013","AEnterprise1",true);
            TestUser user = new TestUser();
            user.UnionNonclusteredA = "indexA";
            //user.UnionNonclusteredB = "";
            //user.IsDelete = true;
            user.Name = "Name_" + UnStrRan.getStr(1, 10);
            //user.Pass = UnEncMD5.getMd5Hashs(UnStrRan.getStr(1, 10));
            cn.update<TestUser>(user, null, "TestUserID = {0}", "17");


            user.NonclusteredA = "NonclusteredA";
            user.NonclusteredB = "NonclusteredB";
            user.TestUserGUID = Guid.NewGuid();
            user.TestUserUID = UnStrRan.getUID();
            //user.e = 100;
            //user.i = 1;
            //user.j = 255;
            //cn.insert(user);
            user.Name = "Name_" + UnStrRan.getStr(1, 10);
            user.TestUserGUID = Guid.NewGuid();
            user.TestUserUID = UnStrRan.getUID();
            //cn.insert(user);

            user.Name = "Name_" + UnStrRan.getStr(1, 10);

            //cn.delete<TestUser>("TestUserID = {0} Or TestUserID = {1}", "11,16");
            //cn.delete<TestUser>("TestUserID = {0} Or TestUserID = {1}", "12,15");
            //List<TestUser> users = cn.query<TestUser>(null, "TestUserID = {0} Or TestUserID = {1}", "11,16", null);
            //Console.WriteLine(users.Count+"%%");
            cn.commit();
            //cn.queryDT("Select * From abc",null);

            TestUserDetail det = new TestUserDetail();
            det.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            det.AddTimeStamp = Convert.ToInt64(UnDate.ticksSec());
            det.Country = "CN";
            det.IsDelete = false;
            det.LastTime = det.AddTime;
            det.LastTimeStamp = det.AddTimeStamp;
            det.TelAreaCode = "028";
            det.TelMobile = "18980826967";
            det.TestUserDetailGUID = Guid.NewGuid();
            det.TestUserDetailUID = UnStrRan.getUID();
            //det.TestUserGUID = list[0].TestUserGUID;

            //cn.insert(det);

            List<TestUser> list = cn.query<TestUser>(null, "TestUserID<100", null, null);
            foreach (var item in list)
            {
                //Console.WriteLine(item.e + "/" + item.i + "/" + item.j);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Type> listT = new List<Type>();
            listT.Add(typeof(TestUser));
            listT.Add(typeof(TestUserDetail));
            listT.Add(typeof(TestCardType));
            listT.Add(typeof(TestCard));

            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", true);
            //int? i = cn.renameColumn(typeof(TestUser));
            //Console.WriteLine(i);
            cn.updateBase();
            cn.createTableList(listT);
            cn.dropTableRelationList(listT);
            cn.updateTableList(listT);
            cn.createTableRelationList(listT);

            cn.updateNullToDefaultList(listT);
            cn.commit();
            //cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", true);
   
            //cn.commit();

        }

        public UnSqlHelpU getSql(bool b)
        {
            return new UnSqlHelpU("Data Source=192.168.100.141,1433;Initial Catalog=AEnterprise1;User ID=hpadmin;Password=cdhpadmin2013;", b);
        }


        private static RedisHelper redis = new RedisHelper("127.0.0.1", 6379, 3, 1);

        private void set<T>(List<T> list)
        {
            //redis.Set("sss", list);
            //redis.Expire("sss", 5);
            //var list0 = redis.Get<List<T>>("sss");
            //Console.WriteLine(list0.Count + "//");
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UnSql sql = new UnSql("Data Source=192.168.100.141,1433;Initial Catalog=AEnterprise1;User ID=hpadmin;Password=cdhpadmin2013;", false, redis);
            var list = sql.query<TestUser>(null, " 0 = {0} And 1 = {1}", "0,1", null, false, 5);
            Console.WriteLine("list数量：" + list.Count);
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.Name + "/");
            //}
           
            return;
            var page = sql.queryPage<TestUser>(null, "TestUserID = {0}", "212734", "TestUserID", 1, 3, 5);
            //page = sql.queryPage<TestUser>(null, null, (string)null, null, 2, 2, 5);
            //page = sql.queryPage<TestCard>(null, null, (string)null, null, 2, 2, 5);
            var dt = sql.queryDT<TestUser>("Select * From TestUser", null, 5);
            //Console.WriteLine(page.PageSize + "/" + page.DataSource.Rows.Count+"/"+((List<TestUser>)page.TSource)[0].TestUserID);

            //Console.WriteLine(dt.Rows.Count);
            //Console.WriteLine(page.DataSource.Rows.Count);
            //return;
            var keys = redis.getAllKeys();
            Console.WriteLine("key数量1:" + keys.Count);

            //TestUser tu = new TestUser();
            //tu.Name = UnStrRan.getStr(1, 32);
            //sql.update<TestUser>(tu, null, "TestUserID = 212734", (string)null, false, true);

            //sql.removeQueryRedis(typeof(TestUser));
            //sql.removeQueryPageRedis(typeof(TestUser));
            //sql.removeQueryDTRedis(typeof(TestUser));
            //sql.removeAllRedis();
            //redis = new RedisClient("127.0.0.1", 6379);
            keys = redis.getAllKeys();
            Console.WriteLine("key数量2:" + keys.Count);
            foreach (var item in keys)
            {
                Console.WriteLine("key:" + item);
            }
            //sql.query<TestUser>(null, null, null, null, false, -1);
            //sql.queryPage<TestUser>(null, null, (string)null, null, 1, 2, -2);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1");
            //List<TestUser> list = cn.query<TestUser>(null, "1 = 1", null, null);
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.Name + "/");
            //}
            var page = cn.queryPage<TestUser>(null, "1 = 1", (string)null, "TestUserID Desc", 1, 2, 10);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(insertUser).Start();
                int j = i % 1000;
                if (j == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }

        void insertUser()
        {
            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", true);
            TestUser user = new TestUser();
            user.UnionNonclusteredA = "indexA";
            user.Name = "Name_" + UnStrRan.getShortGUID();
            user.Pass = UnEncMD5.getMd5Hashs(UnStrRan.getStr(1, 10));
            user.NonclusteredA = "NonclusteredA";
            user.NonclusteredB = "NonclusteredB";
            user.TestUserGUID = Guid.NewGuid();
            user.TestUserUID = UnStrRan.getUID();
            cn.insert(user);
            cn.update<TestUser>(user, null, "TestUserGUID = '{0}'", user.TestUserGUID.ToString());
            cn.commit();
        }

        RedisHelper help = new RedisHelper("192.168.100.141", 6379, 3, 1);

        RedisClient rd = new RedisClient("192.168.100.141", 6379);

        private void button7_Click(object sender, EventArgs e)
        {
            Console.WriteLine(help.exists("name"));
            help.set("name", "11", 5);

            string a = help.get<string>("UnSql_6ab035824d4d942d_EnterpriseInfo_Query_f875564a16101c80");
            Console.WriteLine(a);
            var keys = help.getAllKeys();
            Console.WriteLine("key数量:" + keys.Count);
            foreach (var item in keys)
            {
                Console.WriteLine("key:" + item);
                help.remove(item);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //rd.Add("a", "1");
            rd.Set("b", "2");

      
            Console.WriteLine(rd.Get<string>("b"));
        }
    }
}
