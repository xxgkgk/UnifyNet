using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnCommon.Entity;
using UnCommon.HTTP;
using UnCommon.Interfaces;

namespace UnTestWin
{
    public partial class Http : Form
    {
        public Http()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UnHttpClient http = new UnHttpClient("http://upimage.shaokaolaile.com/upimage.ashx");
            http.setIntTransfer(new transfer());
            http.upFile(textBox1.Text, UnHttpUpEvent.Image);
            
        }

        public class transfer : UnIntTransfer
        {

            void UnIntTransfer.progress(UnAttrPgs pgs)
            {
                //Console.WriteLine(rst.back);
            }

            bool UnIntTransfer.success(UnAttrRst rst)
            {
                Console.WriteLine(rst.back + "##");
                return true;
            }

            void UnIntTransfer.error(UnAttrRst rst)
            {
                Console.WriteLine(rst.msg + "##");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UnHttpClient http = new UnHttpClient("https://alipayapi.shaokaolaile.com/pay/order.ashx");
            http.setIntTransfer(new transfer());
            http.sendMsg("");
        }

    }
}
