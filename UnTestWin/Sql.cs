﻿using ServiceStack.Redis;
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

            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", UnSqlConnectModel.ConnectOrCreate, true);

            cn.updateBase();
            cn.createTableList(listT);
            cn.createTableRelationList(listT);
            cn.commit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", UnSqlConnectModel.Connect, true);
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

            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", UnSqlConnectModel.Connect);
            cn.updateBase();
            cn.createTableList(listT);
            cn.dropTableRelationList(listT);
            cn.updateTableList(listT);
            cn.createTableRelationList(listT);
        }

        public UnSqlHelpU getSql(bool b)
        {
            return new UnSqlHelpU("Data Source=192.168.100.141,1433;Initial Catalog=AEnterprise1;User ID=hpadmin;Password=cdhpadmin2013;", b);
        }


        private static RedisClient redis = new RedisClient("192.168.100.141", 6379);

        private void set<T>(List<T> list)
        {
            redis.Set("sss", list);
            redis.Expire("sss", 5);
            var list0 = redis.Get<List<T>>("sss");
            Console.WriteLine(list0.Count + "//");
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UnSql sql = new UnSql("Data Source=192.168.100.141,1433;Initial Catalog=AEnterprise1;User ID=hpadmin;Password=cdhpadmin2013;", false, redis);
            var list = sql.query<TestUser>(null, null, null, null, false, 5);
            Console.WriteLine("list数量：" + list.Count);
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.Name + "/");
            //}


            var page = sql.queryPage<TestUser>(null, "TestUserID = 212734", (string)null, null, 1, 3, 5);
            page = sql.queryPage<TestUser>(null, null, (string)null, null, 2, 2, 5);
            page = sql.queryPage<TestCard>(null, null, (string)null, null, 2, 2, 5);
            Console.WriteLine(page.PageSize + "/" + page.DataSource.Rows.Count);

            var dt = sql.queryDT<TestUser>("Select * From TestUser", null, 5);
            //page = sql.queryPage<TestUser>(null, null, (string)null, null, 2, 2, 5);
            Console.WriteLine(dt.Rows.Count);

            var keys = redis.GetAllKeys();
            Console.WriteLine("key数量1:" + keys.Count);

            TestUser tu = new TestUser();
            tu.Name = UnStrRan.getStr(1, 32);
            sql.update<TestUser>(tu, null, "TestUserID = 212734", (string)null, false, true);

            //sql.removeQueryPageRedis(typeof(TestUser));
            sql.removeQueryDTRedis(typeof(TestUser));
            sql.removeQueryRedis(typeof(TestUser));

            keys = redis.GetAllKeys();
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
            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", UnSqlConnectModel.Connect);
            List<TestUser> list = cn.query<TestUser>(null, "1 = 1", null, null);
            foreach (var item in list)
            {
                Console.WriteLine(item.Name + "/");
            }
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
            UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", UnSqlConnectModel.Connect, true);
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

        private void button7_Click(object sender, EventArgs e)
        {
            //UnSql sql = new UnSql("Data Source=192.168.100.141,1433;Initial Catalog=AEnterprise1;User ID=hpadmin;Password=cdhpadmin2013;", false, redis);
            UnSqlPage page = new UnSqlPage();

            DataTable memTable = new DataTable("tableName");
            memTable.Columns.Add(new DataColumn("ID", typeof(int)));
            memTable.Columns.Add(new DataColumn("Username", typeof(string)));
            memTable.Columns.Add(new DataColumn("Password", typeof(Guid)));

            DataRow row = memTable.NewRow();
            row["ID"] = 1;
            row["Username"] = "badbug";
            row["Password"] = Guid.NewGuid();

            memTable.Rows.Add(row);
            page.DataSource = memTable;
           
            var tu = new TestUser();
            tu.Name = "aac";
            page.PageSize = 12;

            string s = UnXMMPXml.tToXml(typeof(UnSqlPage), page);
            Console.WriteLine(s);

            redis.Set("aa", s);
            var b = redis.Get<string>("aa");


            UnSqlPage dt = (UnSqlPage)UnXMMPXml.xmlToT(typeof(UnSqlPage), b);
            Console.WriteLine(dt.DataSource.Rows[0]["Username"]+"//"+ dt.PageSize);
   




            var a = redis.GetAllKeys();
            Console.WriteLine(a.Count);
            foreach (var item in a)
            {
                //Console.WriteLine(item);
            }
     
           redis.RemoveByRegex("UnSql_.*");
          
            //redis.RemoveByPattern("UnSql_*");
            //sql.removeAllRedis();
        }
    }
}
