using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiceStack.Redis;

namespace UnTestWin
{
    public partial class Redis : Form
    {
        public Redis()
        {
            InitializeComponent();
        }

        static RedisClient redisClient = new RedisClient("127.0.0.1", 6379);//redis服务IP和端口
        private void Redis_Load(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            redisClient.Set("name", "李大龙");
            string s = redisClient.Get<string>("name");
            Console.WriteLine(s+"///");
        }
    }
}
