using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnCommon;
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

        // UnSql cn = new UnSql("Data Source=121.41.18.224,1433;Initial Catalog=master;User ID=hpadmin;Password=cdhpadmin2013;");
        UnSql cn = new UnSql("121.41.18.224", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", UnSqlConnectModel.ConnectOrCreate);

        private void button1_Click(object sender, EventArgs e)
        {
            //cn.createDB("abcde");
            //string s1 = UnSqlStr.createTable(typeof(AliPayOrder));
            //string s2 = UnSqlStr.createTableRelation(typeof(AliPayOrder));
            cn.createTable(typeof(TestUser));
            cn.createTable(typeof(TestUserDetail));
            cn.createTable(typeof(TestCardType));
            cn.createTable(typeof(TestCard));

            //cn.dropTableRelationAll(typeof(TestCard));
            //cn.dropTableRelationAll(typeof(TestCardType));
            //cn.dropTableRelationAll(typeof(TestUserDetail));
            //cn.dropTableRelationAll(typeof(TestUser));

            cn.dropTableRelation(typeof(TestCard));
            cn.dropTableRelation(typeof(TestCardType));
            cn.dropTableRelation(typeof(TestUserDetail));
            cn.dropTableRelation(typeof(TestUser));

            cn.createTableRelation(typeof(TestUser));
            cn.createTableRelation(typeof(TestUserDetail));
            cn.createTableRelation(typeof(TestCardType));
            cn.createTableRelation(typeof(TestCard));

            TestUser user = new TestUser();
            user.UnionNonclusteredA = "indexA";
            user.UnionNonclusteredB = "";
            user.IsDelete = true;
            user.Name = "Name_" + UnStrRan.getStr(1, 10);
            user.Pass = UnEncMD5.getMd5Hashs("123456");
            user.NonclusteredA = "NonclusteredA";
            user.NonclusteredB = "NonclusteredB";
            user.TestUserGUID = Guid.NewGuid();
            user.TestUserUID = UnStrRan.getUID();
            cn.insert(user);

            List<TestUser> list = cn.query<TestUser>(null, null, null, null);
            foreach(var item in list)
            {
                Console.WriteLine(item.Name + "/" + item.TestUserGUID);
            }
            Console.WriteLine(list.Count);

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
            det.TestUserGUID = list[0].TestUserGUID;

            cn.insert(det);


            //cn.dropTable(typeof(TestUser));

            //Console.WriteLine(s1);
            //Console.WriteLine(s2);
            //cn.createTable(typeof(AliPayOrder));

        }
    }
}
