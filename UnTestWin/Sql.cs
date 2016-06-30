using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnCommon;
using UnCommon.Tool;
using UnDataBase;

namespace UnTestWin
{
    public partial class Sql : Form
    {
        public Sql()
        {
            InitializeComponent();
        }

        // UnSql cn = new UnSql("Data Source=121.41.18.224,1433;Initial Catalog=master;User ID=hpadmin;Password=cdhpadmin2013;");
        UnSql cn = new UnSql("121.41.18.224", "1433", "hpadmin", "cdhpadmin2013", "abcdefg", UnSqlConnectModel.ConnectOrCreate);

        private void button1_Click(object sender, EventArgs e)
        {
            //cn.createDB("abcde");
            //string s1 = UnSqlStr.createTable(typeof(AliPayOrder));
            //string s2 = UnSqlStr.createTableRelation(typeof(AliPayOrder));
            cn.createTable(typeof(AliPayOrder));
            //Console.WriteLine(s1);
            //Console.WriteLine(s2);
            //cn.createTable(typeof(AliPayOrder));

        }
    }
}
