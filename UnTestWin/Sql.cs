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

        UnSql cn = new UnSql("192.168.100.141", "1433", "hpadmin", "cdhpadmin2013", "AEnterprise1", UnSqlConnectModel.ConnectOrCreate);

        private void button1_Click(object sender, EventArgs e)
        {
            List<Type> listT = new List<Type>();
            listT.Add(typeof(TestUser));
            listT.Add(typeof(TestUserDetail));
            listT.Add(typeof(TestCardType));
            listT.Add(typeof(TestCard));

            cn.updateBase();
            cn.createTableList(listT);
            cn.createTableRelationList(listT);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TestUser user = new TestUser();
            user.UnionNonclusteredA = "indexA";
            //user.UnionNonclusteredB = "";
            //user.IsDelete = true;
            user.Name = "Name_" + UnStrRan.getStr(1, 10);
            user.Pass = UnEncMD5.getMd5Hashs("123456");
            user.NonclusteredA = "NonclusteredA";
            user.NonclusteredB = "NonclusteredB";
            user.TestUserGUID = Guid.NewGuid();
            user.TestUserUID = UnStrRan.getUID();
            //user.e = 100;
            //user.i = 1;
            //user.j = 255;
            cn.insert(user);
            cn.queryDT("Select * From abc",null);

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

            cn.updateBase();
            cn.createTableList(listT);
            cn.dropTableRelationList(listT);
            cn.updateTableList(listT);
            cn.createTableRelationList(listT);
        }
    }
}
