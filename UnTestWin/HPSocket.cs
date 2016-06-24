using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HPSocketCS;

namespace UnTestWin
{
    public partial class HPSocket : Form
    {
        public HPSocket()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpClient tc = new TcpClient();
           
            //00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000tc.s
            //UdpClient uc = new UdpClient();
        }
    }
}
