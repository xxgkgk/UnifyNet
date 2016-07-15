using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnCommon;
using UnCommon.Config;
using UnCommon.Encrypt;
using UnCommon.Tool;
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

        private void button4_Click(object sender, EventArgs e)
        {
            UnSqlHelpU sql = new UnSqlHelpU("Data Source=192.168.100.141,1433;Initial Catalog=AEnterprise1;User ID=hpadmin;Password=cdhpadmin2013;",false);
            //UnSqlHelpU sql1 = new UnSqlHelpU("Data Source=192.168.100.141,1433;Initial Catalog=AEnterprise1;User ID=hpadmin;Password=cdhpadmin2013;", false);
            //int? i = getSql(false).exSql("Select * From Test1");
            //getSql(false).getDataTable("Select * From Test1");
            //getSql(false).exSql("INSERT INTO Test1 (a, b) VALUES ('aa', 'bb')");
            sql.exSql("INSERT INTO Test1 (a, b,c,d) VALUES ('aa', 'bb','cc',2)");
            sql.exSql("INSERT INTO Test1 (a, b,c,d) VALUES ('aa', 'bb','fdeee',1)");
            //sql.commit();
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
    }
}
