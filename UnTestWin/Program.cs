using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UnCommon.Config;

namespace UnTestWin
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //UnInit.setVersion(2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new WeiXin());
            //Application.Run(new Udp());
            //Application.Run(new Http());
            //Application.Run(new Ali());
            //Application.Run(new Other());
            Application.Run(new Sql());
        }
    }
}
