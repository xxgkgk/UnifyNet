using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnCommon.HTTP;

namespace UnTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            UnHttpServerAbs httpServer;
            if (args.GetLength(0) > 0)
            {
                httpServer = new UnHttpServerTest(Convert.ToInt16(args[0]));
            }
            else
            {
                httpServer = new UnHttpServerTest(90);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }
    }
}
