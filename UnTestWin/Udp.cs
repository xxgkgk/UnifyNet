using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnCommon.UDP;
using UnCommon.Entity;
using System.IO;
using UnCommon.Interfaces;
using UnCommon.Files;
using System.Threading;
using UnCommon.Config;
using UnCommon.Encrypt;
using UnCommon.Tool;

namespace UnTestWin
{
    public partial class Udp : Form, UnIntTransfer
    {
        public Udp()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(comboBox1.Text + "/" + comboBox2.Text + "/" + textBox1.Text);
            UnUdpClient udp = new UnUdpClient(comboBox1.Text, Convert.ToInt32(comboBox2.Text));
            udp.setIntTransfer(this);
            udp.setTimeOut(10000);
            udp.upFile(textBox1.Text);
            d = UnDate.ticksSec();
        }

        public class tran : UnIntTransfer
        {

            public void progress(UnAttrPgs pgs)
            {
                Console.WriteLine("进度：" + pgs.percentage() + "%");
            }

            public bool success(UnAttrRst rst)
            {
                i--;
                Console.WriteLine("完成" + rst.code + "/" + rst.msg + "/" + rst.back);
                return true;
            }

            public void error(UnAttrRst rst)
            {
                i--;
                Console.WriteLine("错误" + rst.code + "/" + rst.msg + "/" + rst.back);
            }
        }

        public static int i = 0;

        private void button5_Click(object sender, EventArgs e)
        {
            DirectoryInfo di0 = new DirectoryInfo(@"E:\UpTest");
            //遍历文件
            FileInfo[] fis0 = di0.GetFiles();
            ip = comboBox1.Text;
            foreach (FileInfo f0 in fis0)
            {
                //Console.WriteLine(comboBox1.Text + ":" + f0.FullName + "/" + i);
                list.Add(f0.FullName);
                //UnUdpClient udp = new UnUdpClient(comboBox1.Text, 3000);
                //udp.setIntTransfer(t);
                //udp.upFile(f0.FullName);

            }
            new Thread(test).Start();
        }

        public string ip = null;
        List<string> list = new List<string>();

        public void test()
        {
            tran t = new tran();
            int p = Convert.ToInt32(comboBox2.Text);
            while (list.Count > 0)
            {
                if (i <= 200)
                {
                    i++;
                    UnUdpClient udp = new UnUdpClient(ip, p);
                    udp.setIntTransfer(t);
                    udp.upFile(list[0]);
                    list.RemoveAt(0);
                }
                else
                {
                    //Console.WriteLine("线程：" + i);
                    Thread.Sleep(2000);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UnUdpEntity ue = new UnUdpEntity();

            ue.Event = "uqb";
            ue.Extent = ".12345";
            ue.HashCode = "11111111111111111111111111111111";
            ue.PackSize = 1024;
            ue.SubSize = 1024;

            // 超大文件
            ue.PackNo = long.MaxValue;
            ue.PackOffset = long.MaxValue;
            ue.TotalPacks = long.MaxValue;
            ue.TotalSize = long.MaxValue;

            // 2G内文件
            ue.PackNo = int.MaxValue;
            ue.PackOffset = int.MaxValue;
            ue.TotalPacks = int.MaxValue;
            ue.TotalSize = int.MaxValue;

            byte[] b = UnUdpHelp.assemblePackage(ue);
            // 大文件为129 (最大MTU)1472 - 151 = 1318 (标准MTU)548 - 151 = 397;
            // 2G内文件102 (最大MTU)1472 - 115 = 1357 (标准MTU)548 - 115 = 433;
            Console.WriteLine(b.Length);

            ue = new UnUdpEntity();

        }

        UnUdpClient udpDown = null;

        private decimal d = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            string uncode = "UNCODE//:" + UnEncDES.encrypt("ambo#$l3", textBox2.Text);
            uncode = "UNCODE//:QJoDTfIhVXsoyWCmQhclTVOS9UvpPj03";
            Console.Write(uncode);
            udpDown = new UnUdpClient(comboBox1.Text, Convert.ToInt32(comboBox2.Text));
            udpDown.setIntTransfer(this);
            udpDown.downFile(uncode);
            d = UnDate.ticksSec();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = fileDialog.FileName;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Text == "暂停")
            {
                udpDown.setPause(true);
                button8.Text = "继续";
            }
            else
            {
                udpDown.setPause(false);
                button8.Text = "暂停";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DirectoryInfo di0 = new DirectoryInfo(@"E:\UpTest");
            //遍历文件
            FileInfo[] fis0 = di0.GetFiles();
            ip = comboBox1.Text;
            foreach (FileInfo f0 in fis0)
            {
                //Console.WriteLine(comboBox1.Text + ":" + f0.FullName + "/" + i);
                list.Add(f0.FullName);
                //UnUdpClient udp = new UnUdpClient(comboBox1.Text, 3000);
                //udp.setIntTransfer(t);
                //udp.upFile(f0.FullName);  
            }
            new Thread(test1).Start();
        }

        public void test1()
        {
            tran t = new tran();
            int p = Convert.ToInt32(comboBox2.Text);
            while (list.Count > 0)
            {
                if (i <= 20)
                {
                    i++;
                    UnUdpClient client = new UnUdpClient(ip, p);
                    client.setIntTransfer(t);
                    client.downFile(list[0]);
                    list.RemoveAt(0);
                }
                else
                {
                    //Console.WriteLine("线程：" + i);
                    Thread.Sleep(2000);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string uncode = "UNCODE//:" + UnEncDES.encrypt("ambo#$l3", textBox3.Text);
            textBox4.Text = uncode;
            Console.Write(uncode);
        }

        public void progress(UnAttrPgs pgs)
        {
            Console.WriteLine("进度：" + pgs.percentage() + "%");
            Invoke(new Action(() =>
            {
                textBox5.Text = "进度：" + pgs.percentage() + "%";
            }));
        }

        public bool success(UnAttrRst rst)
        {
            Console.WriteLine("完成" + rst.code + "/" + rst.msg + "/" + rst.back);
            Console.WriteLine("用时" + (UnDate.ticksSec() - d) + "秒");
            return true;
        }

        public void error(UnAttrRst rst)
        {
            Console.WriteLine("错误" + rst.code + "/" + rst.msg + "/" + rst.back);
        }
    }
}
